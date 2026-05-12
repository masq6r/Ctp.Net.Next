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

[<RequireQualifiedAccess>]
type ConnectError =
    | Timeout of TimeSpan
    | Cancelled
    | NativeOperationFailed of string


module internal OptionHelpers =
    let createUserLoginRequest (options: CtpOptions) =
        let request = RequestUserLogin.Create(options.BrokerId, options.UserId, options.Password)

        { request with UserProductInfo = options.UserProductInfo }

    let createUserLogoutRequest (options: CtpOptions) = RequestUserLogout.Create(options.BrokerId, options.UserId)

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

    let awaitTask (task: Task<'T>) = Async.AwaitTask task

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

type internal PendingQueryDict(?logger: ILogger) =
    let logger = defaultArg logger Abstractions.NullLogger.Instance

    let dict =
        ConcurrentDictionary<
            int,
            {| Items: ResizeArray<obj>
               QueryName: string
               Finalize: RspInfo option -> unit |}
         >()

    member _.Register<'a>(requestId, queryName: string, completion: TaskCompletionSource<Result<'a list, RspInfo>>) =
        let items = ResizeArray<obj>()

        let finalize rspInfo =
            match ClientHelpers.resultFromRspInfo rspInfo with
            | Error info ->
                logger.LogError(
                    "{QueryName} query failed for request {RequestId}: [{ErrorId}] {ErrorMessage}",
                    queryName,
                    requestId,
                    info.ErrorId,
                    info.ErrorMessage
                )

                completion.TrySetResult(Error info) |> ignore
            | Ok() ->
                logger.LogDebug(
                    "{QueryName} query completed for request {RequestId} with {ItemCount} items",
                    queryName,
                    requestId,
                    items.Count
                )

                items |> Seq.cast<'a> |> List.ofSeq |> Ok |> completion.TrySetResult |> ignore

        dict.TryAdd(requestId, {| Items = items; QueryName = queryName; Finalize = finalize |})
        |> ignore

    member _.TryRemove(requestId: int) = dict.TryRemove requestId |> ignore

    member _.TryFail(requestId: int, error: RspInfo) =
        match dict.TryRemove requestId with
        | true, removed ->
            logger.LogError(
                "{QueryName} query failed for request {RequestId} via RspError: [{ErrorId}] {ErrorMessage}",
                removed.QueryName,
                requestId,
                error.ErrorId,
                error.ErrorMessage
            )

            removed.Finalize(Some error)
        | _ -> ()

    member _.TryAccumulate<'a>(requestId, item: 'a option, rspInfo: RspInfo option, isLast: bool) =
        match dict.TryGetValue requestId with
        | true, state ->
            item |> Option.iter state.Items.Add

            if isLast then
                dict.TryRemove requestId
                |> Option.ofPair
                |> Option.iter (fun removed -> removed.Finalize rspInfo)
            else
                logger.LogTrace(
                    "Received response for {QueryName} query with request {RequestId}",
                    state.QueryName,
                    requestId,
                    state.Items.Count
                )
        | _ -> ()

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
