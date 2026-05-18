namespace Ctp.Net

open System
open FSharpPlus
open System.Text
open Ctp.Net.Bridge
open System.Threading
open System.Threading.Tasks
open Microsoft.Extensions.Logging
open System.Collections.Concurrent

type CtpEncodingOptions =
    { OutboundEncoding: Encoding
      InboundEncoding: Encoding }

    static member Default =
        let defaults = EncodingPair.Default

        { OutboundEncoding = defaults.OutboundEncoding
          InboundEncoding = defaults.InboundEncoding }

type CtpOptions =
    { FrontAddress: string
      FlowPath: string option
      ProductionMode: bool
      BrokerId: string
      UserId: string
      Password: string
      UserProductInfo: string
      AppId: string
      AuthCode: string }

    static member Create
        (
            frontAddress: string,
            brokerId: string,
            userId: string,
            password: string,
            ?flowPath: string,
            ?productionMode: bool,
            ?userProductInfo: string,
            ?appId: string,
            ?authCode: string
        )
        =
        { FrontAddress = frontAddress
          FlowPath = flowPath
          ProductionMode = defaultArg productionMode true
          BrokerId = brokerId
          UserId = userId
          Password = password
          UserProductInfo = defaultArg userProductInfo ""
          AppId = defaultArg appId ""
          AuthCode = defaultArg authCode "" }

type CtpFlowControlOptions =
    { MaxDispatchesPerSecond: int
      MaxQueriesPerSecond: int
      MaxNativeReturnCodeRetries: int
      NativeReturnCodeRetryDelay: TimeSpan
      MaxQueryRspErrorRetries: int
      QueryRspErrorRetryDelay: TimeSpan
      QueryCompletionTimeout: TimeSpan
      SubscriptionBatchSize: int
      SubscriptionBatchDelay: TimeSpan }

    static member Default =
        { MaxDispatchesPerSecond = 5
          MaxQueriesPerSecond = 2
          MaxNativeReturnCodeRetries = 3
          NativeReturnCodeRetryDelay = TimeSpan.FromMilliseconds 250.0
          MaxQueryRspErrorRetries = 3
          QueryRspErrorRetryDelay = TimeSpan.FromMilliseconds 500.0
          QueryCompletionTimeout = TimeSpan.FromSeconds 15.0
          SubscriptionBatchSize = 500
          SubscriptionBatchDelay = TimeSpan.FromSeconds 1.0 }

[<RequireQualifiedAccess>]
type ConnectError =
    | Timeout of TimeSpan
    | Cancelled
    | NativeOperationFailed of string


module internal OptionHelpers =
    let createUserLoginRequest (options: CtpOptions) =
        let request = UserLoginRequest.Create(options.BrokerId, options.UserId, options.Password)

        { request with UserProductInfo = options.UserProductInfo }

    let createUserLogoutRequest (options: CtpOptions) = UserLogoutRequest.Create(options.BrokerId, options.UserId)

    let createAuthenticateRequest (options: CtpOptions) =
        { BrokerId = options.BrokerId
          UserId = options.UserId
          UserProductInfo =
            if String.IsNullOrEmpty options.UserProductInfo then
                None
            else
                Some options.UserProductInfo
          AuthCode = options.AuthCode
          AppId = options.AppId }

    let createSettlementInfoConfirmRequest (options: CtpOptions) =
        { BrokerId = options.BrokerId
          InvestorId = options.UserId
          ConfirmDate = ""
          ConfirmTime = ""
          SettlementId = 0
          AccountId = ""
          CurrencyId = "" }

module internal ClientHelpers =
    let nextRequestId =
        let counter = ref 0
        fun () -> Interlocked.Increment(counter)

    let createCompletionSource<'T> () =
        TaskCompletionSource<'T>(TaskCreationOptions.RunContinuationsAsynchronously)

    let resultFromRspInfo (rspInfo: RspInfo option) =
        match rspInfo with
        | Some info when info.ErrorId <> 0 -> Error info
        | _ -> Ok()

    let apiReturnError code =
        { ErrorId = code
          ErrorMessage = $"CTP native call returned {code}"
          RawErrorMessage = Array.empty }

    let apiTimeoutError operationName (timeout: TimeSpan) =
        { ErrorId = -10001
          ErrorMessage = $"{operationName} timed out after {timeout.TotalSeconds} seconds"
          RawErrorMessage = Array.empty }

    let completeWithFailure
        (dict: ConcurrentDictionary<int, TaskCompletionSource<Result<'T, RspInfo>>>)
        requestId
        error
        =
        let mutable completion = Unchecked.defaultof<_>

        if dict.TryRemove(requestId, &completion) then
            completion.TrySetResult(Error error) |> ignore

    let completeWithResult
        (dict: ConcurrentDictionary<int, TaskCompletionSource<Result<'T, RspInfo>>>)
        requestId
        result
        =
        let mutable completion = Unchecked.defaultof<_>

        if dict.TryRemove(requestId, &completion) then
            completion.TrySetResult(result) |> ignore

    let awaitTask (task: Task<'T>) = async {
        let! cancellationToken = Async.CancellationToken
        return! task.WaitAsync(cancellationToken) |> Async.AwaitTask
    }

type internal SinglePendingResult<'T>() =
    let syncRoot = obj ()
    let mutable pending: TaskCompletionSource<'T> option = None

    member _.Begin() =
        let completion = ClientHelpers.createCompletionSource<'T> ()

        lock syncRoot (fun () ->
            match pending with
            | Some _ -> invalidOp "Another operation is already pending."
            | None -> pending <- Some completion)

        completion

    member _.TryTake() =
        lock syncRoot (fun () ->
            let current = pending
            pending <- None
            current)

    member this.TrySetResult(result: 'T) =
        this.TryTake()
        |> Option.iter (fun completion -> completion.TrySetResult(result) |> ignore)

type internal SinglePendingRequest<'TRequest, 'TResult>() =
    let syncRoot = obj ()
    let mutable pending: ('TRequest * TaskCompletionSource<'TResult>) option = None

    member _.Begin(request: 'TRequest) =
        let completion = ClientHelpers.createCompletionSource<'TResult> ()

        lock syncRoot (fun () ->
            match pending with
            | Some _ -> invalidOp "Another operation is already pending."
            | None -> pending <- Some(request, completion))

        completion

    member _.TryTake() =
        lock syncRoot (fun () ->
            let current = pending
            pending <- None
            current)

    member this.TrySetResult(result: 'TResult) =
        this.TryTake()
        |> Option.iter (fun (_, completion) -> completion.TrySetResult(result) |> ignore)

    member this.TrySetResultFromRequest(mapResult: 'TRequest -> 'TResult) =
        this.TryTake()
        |> Option.iter (fun (request, completion) -> completion.TrySetResult(mapResult request) |> ignore)

[<RequireQualifiedAccess>]
type internal PendingResponseCompletionPolicy =
    | FinalOnly
    | StreamUntilLast

type internal PendingQueryDict(?logger: ILogger) =
    let logger = defaultArg logger Abstractions.NullLogger.Instance

    let dict =
        ConcurrentDictionary<
            int,
            {| Items: ResizeArray<objnull>
               OperationName: string
               Finalize: RspInfo option -> unit |}
         >()

    member _.Register<'a>(requestId, operationName: string, completion: TaskCompletionSource<Result<'a list, RspInfo>>) =
        let items = ResizeArray<objnull>()

        let finalize rspInfo =
            match ClientHelpers.resultFromRspInfo rspInfo with
            | Error info ->
                logger.LogError(
                    "{OperationName} operation failed for request {RequestId}: [{ErrorId}] {ErrorMessage}",
                    operationName,
                    requestId,
                    info.ErrorId,
                    info.ErrorMessage
                )

                completion.TrySetResult(Error info) |> ignore
            | Ok() ->
                logger.LogDebug(
                    "{OperationName} operation completed for request {RequestId} with {ItemCount} items",
                    operationName,
                    requestId,
                    items.Count
                )

                items |> Seq.cast<'a> |> List.ofSeq |> Ok |> completion.TrySetResult |> ignore

        dict.TryAdd(requestId, {| Items = items; OperationName = operationName; Finalize = finalize |})
        |> ignore

    member _.TryRemove(requestId: int) = dict.TryRemove requestId |> ignore

    member _.TryFail(requestId: int, error: RspInfo) =
        match dict.TryRemove requestId with
        | true, removed ->
            logger.LogError(
                "{OperationName} operation failed for request {RequestId} via RspError: [{ErrorId}] {ErrorMessage}",
                removed.OperationName,
                requestId,
                error.ErrorId,
                error.ErrorMessage
            )

            removed.Finalize(Some error)
        | _ -> ()

    member _.TryHandleResponse
        (
            requestId: int,
            item: objnull option,
            rspInfo: RspInfo option,
            isLast: bool,
            completionPolicy: PendingResponseCompletionPolicy
        )
        =
        match dict.TryGetValue requestId with
        | true, state ->
            match completionPolicy with
            | PendingResponseCompletionPolicy.FinalOnly when not isLast ->
                logger.LogTrace(
                    "Ignoring non-final response for {OperationName} request {RequestId}",
                    state.OperationName,
                    requestId
                )
            | PendingResponseCompletionPolicy.FinalOnly ->
                item |> Option.iter state.Items.Add

                dict.TryRemove requestId
                |> Option.ofPair
                |> Option.iter (fun removed -> removed.Finalize rspInfo)
            | PendingResponseCompletionPolicy.StreamUntilLast ->
                item |> Option.iter state.Items.Add

                if isLast then
                    dict.TryRemove requestId
                    |> Option.ofPair
                    |> Option.iter (fun removed -> removed.Finalize rspInfo)
                else
                    logger.LogTrace(
                        "Received response for {OperationName} request {RequestId} with {ItemCount} items",
                        state.OperationName,
                        requestId,
                        state.Items.Count
                    )
        | _ -> ()

    member this.TryAccumulate<'a>(requestId, item: 'a option, rspInfo: RspInfo option, isLast: bool) =
        this.TryHandleResponse(
            requestId,
            item |> Option.map box,
            rspInfo,
            isLast,
            PendingResponseCompletionPolicy.StreamUntilLast
        )

module internal FlowControlHelpers =
    let isRetryableNativeReturnCode = function
        | -2
        | -3 -> true
        | _ -> false

    let isRetryableQueryError = function
        | 90
        | 154 -> true
        | _ -> false

type internal DispatchRateGate(maxDispatchesPerSecond: int) =
    let syncRoot = SemaphoreSlim(1, 1)
    let mutable lastDispatchAt = DateTimeOffset.MinValue

    let minInterval =
        if maxDispatchesPerSecond <= 0 then
            TimeSpan.Zero
        else
            TimeSpan.FromSeconds(1.0 / float maxDispatchesPerSecond)

    member _.AwaitTurnAsync(?cancellationToken: CancellationToken) = task {
        let cancellationToken = defaultArg cancellationToken CancellationToken.None
        do! syncRoot.WaitAsync(cancellationToken)

        try
            let now = DateTimeOffset.UtcNow
            let nextAllowedAt = lastDispatchAt + minInterval

            if nextAllowedAt > now then
                do! Task.Delay(nextAllowedAt - now, cancellationToken)

            lastDispatchAt <- DateTimeOffset.UtcNow
        finally
            syncRoot.Release() |> ignore
    }

type internal SerialDispatchGate() =
    let gate = SemaphoreSlim(1, 1)

    member _.EnterAsync(?cancellationToken: CancellationToken) = task {
        let cancellationToken = defaultArg cancellationToken CancellationToken.None
        do! gate.WaitAsync(cancellationToken)

        return
            { new IDisposable with
                member _.Dispose() = gate.Release() |> ignore }
    }

type internal FlowController(options: CtpFlowControlOptions, ?logger: ILogger) =
    let logger = defaultArg logger Abstractions.NullLogger.Instance
    let dispatchGate = DispatchRateGate(options.MaxDispatchesPerSecond)
    let queryRateGate = DispatchRateGate(options.MaxQueriesPerSecond)
    let queryExecutionGate = SerialDispatchGate()

    member _.AwaitDispatchAsync(?cancellationToken: CancellationToken) =
        dispatchGate.AwaitTurnAsync(?cancellationToken = cancellationToken)

    member _.AwaitQueryDispatchAsync(?cancellationToken: CancellationToken) = task {
        do! dispatchGate.AwaitTurnAsync(?cancellationToken = cancellationToken)
        do! queryRateGate.AwaitTurnAsync(?cancellationToken = cancellationToken)
    }

    member _.AcquireQueryExecutionAsync(?cancellationToken: CancellationToken) =
        queryExecutionGate.EnterAsync(?cancellationToken = cancellationToken)

    member _.ShouldRetryNativeReturnCode(attempt: int, returnCode: int) =
        attempt < options.MaxNativeReturnCodeRetries
        && FlowControlHelpers.isRetryableNativeReturnCode returnCode

    member _.ShouldRetryQueryError(attempt: int, rspInfo: RspInfo) =
        attempt < options.MaxQueryRspErrorRetries
        && FlowControlHelpers.isRetryableQueryError rspInfo.ErrorId

    member _.DelayBeforeNativeRetryAsync(operationName: string, attempt: int, returnCode: int, ?cancellationToken: CancellationToken) = task {
        logger.LogWarning(
            "Retrying {OperationName} after native return code {ReturnCode} (attempt {Attempt})",
            operationName,
            returnCode,
            attempt
        )

        do! Task.Delay(options.NativeReturnCodeRetryDelay, defaultArg cancellationToken CancellationToken.None)
    }

    member _.DelayBeforeQueryRetryAsync(operationName: string, attempt: int, errorId: int, ?cancellationToken: CancellationToken) = task {
        logger.LogWarning(
            "Retrying {OperationName} after query error {ErrorId} (attempt {Attempt})",
            operationName,
            errorId,
            attempt
        )

        do! Task.Delay(options.QueryRspErrorRetryDelay, defaultArg cancellationToken CancellationToken.None)
    }

    member _.AwaitQueryCompletionAsync
        (
            operationName: string,
            requestId: int,
            pending: PendingQueryDict,
            completionTask: Task<Result<'T list, RspInfo>>
        )
        = task {
            try
                return! completionTask.WaitAsync(options.QueryCompletionTimeout)
            with :? TimeoutException ->
                logger.LogWarning(
                    "{OperationName} timed out after {TimeoutSeconds}s for request {RequestId}",
                    operationName,
                    options.QueryCompletionTimeout.TotalSeconds,
                    requestId
                )

                pending.TryRemove requestId
                return Error(ClientHelpers.apiTimeoutError operationName options.QueryCompletionTimeout)
        }

    member _.BatchSubscriptions(instrumentIds: string list) =
        instrumentIds |> List.chunkBySize (max 1 options.SubscriptionBatchSize)

    member _.SubscriptionBatchDelay = options.SubscriptionBatchDelay

type internal ConnectionStartPhase =
    | NotStarted
    | Starting
    | Started

type internal ConnectionCoordinator(startConnection: unit -> unit, ?logger: ILogger) =
    let logger = defaultArg logger Abstractions.NullLogger.Instance
    let syncRoot = obj ()
    let mutable startPhase = NotStarted
    let mutable isConnected = false
    let mutable pendingConnection = ClientHelpers.createCompletionSource<Result<unit, ConnectError>> ()

    let normalizeTimeout timeout =
        match timeout with
        | Some value when value = Timeout.InfiniteTimeSpan -> None
        | value -> value

    member private _.AwaitConnectionResult
        (
            connectionTask: Task<Result<unit, ConnectError>>,
            timeout: TimeSpan option,
            cancellationToken: CancellationToken
        )
        =
        task {
            let normalizedTimeout = normalizeTimeout timeout

            try
                let! result =
                    match normalizedTimeout with
                    | Some value -> connectionTask.WaitAsync(value, cancellationToken)
                    | None -> connectionTask.WaitAsync(cancellationToken)

                return result
            with
            | :? TimeoutException ->
                match normalizedTimeout with
                | Some value ->
                    logger.LogWarning("Connection attempt timed out after {TimeoutSeconds}s", value.TotalSeconds)
                    return Error(ConnectError.Timeout value)
                | None -> return Error(ConnectError.Cancelled)
            | :? OperationCanceledException ->
                logger.LogWarning("Connection attempt was cancelled")
                return Error ConnectError.Cancelled
        }

    member _.HandleFrontConnected() =
        logger.LogDebug("Front connected")

        let mutable completion: TaskCompletionSource<Result<unit, ConnectError>> option = None

        lock syncRoot (fun () ->
            if not isConnected then
                isConnected <- true
                completion <- Some pendingConnection)

        completion |> Option.iter (fun value -> value.TrySetResult(Ok()) |> ignore)

    member _.HandleFrontDisconnected() =
        logger.LogInformation("Front disconnected")

        lock syncRoot (fun () ->
            if isConnected then
                isConnected <- false
                pendingConnection <- ClientHelpers.createCompletionSource<Result<unit, ConnectError>> ())

    member internal this.ConnectTask(?timeout: TimeSpan, ?cancellationToken: CancellationToken) = task {
        let cancellationToken = defaultArg cancellationToken CancellationToken.None
        let mutable waitTask = Unchecked.defaultof<Task<Result<unit, ConnectError>>>
        let mutable shouldStart = false
        let mutable immediateResult = Unchecked.defaultof<Result<unit, ConnectError>>

        let hasImmediateResult =
            lock syncRoot (fun () ->
                if isConnected then
                    immediateResult <- Ok()
                    true
                else
                    waitTask <- pendingConnection.Task

                    if startPhase.IsNotStarted then
                        startPhase <- Starting
                        shouldStart <- true

                    false)

        if hasImmediateResult then
            logger.LogDebug("Already connected, returning immediately")
            return immediateResult
        else
            if shouldStart then
                logger.LogInformation("Initiating front connection")

                try
                    startConnection ()

                    lock syncRoot (fun () ->
                        if startPhase = Starting then
                            startPhase <- Started)
                with ex ->
                    logger.LogError(ex, "Native connection initiation failed: {Message}", ex.Message)

                    let error = Error(ConnectError.NativeOperationFailed ex.Message)

                    lock syncRoot (fun () ->
                        if startPhase = Starting then
                            startPhase <- NotStarted

                        let failedAttempt = pendingConnection
                        pendingConnection <- ClientHelpers.createCompletionSource<Result<unit, ConnectError>> ()
                        failedAttempt.TrySetResult(error) |> ignore)

            return! this.AwaitConnectionResult(waitTask, timeout, cancellationToken)
    }

    member this.Connect(?timeout: TimeSpan) = async {
        let! cancellationToken = Async.CancellationToken

        return!
            this.ConnectTask(?timeout = timeout, cancellationToken = cancellationToken)
            |> Async.AwaitTask
    }
