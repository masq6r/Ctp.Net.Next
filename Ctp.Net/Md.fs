namespace Ctp.Net

open System
open FSharpPlus
open Ctp.Net.Bridge
open System.Threading
open System.Threading.Tasks
open Microsoft.Extensions.Logging

type private MdAgentMessage =
    | FrontConnected
    | FrontDisconnected of int
    | HeartBeatWarning of int
    | RspError of RspInfo option * int * bool
    | RspUserLogin of UserLoginResponse option * RspInfo option * int * bool
    | RspSubMarketData of SpecificInstrumentResponse option * RspInfo option * int * bool
    | RspUnsubMarketData of SpecificInstrumentResponse option * RspInfo option * int * bool
    | RspQryMulticastInstrument of MulticastInstrumentResponse option * RspInfo option * int * bool
    | RspSubForQuoteRsp of SpecificInstrumentResponse option * RspInfo option * int * bool
    | RspUnsubForQuoteRsp of SpecificInstrumentResponse option * RspInfo option * int * bool
    | RspUserLogout of UserLogoutResponse option * RspInfo option * int * bool
    | RtnDepthMarketData of DepthMarketData
    | RtnForQuoteRsp of ForQuoteRspResponse

type MdClient
    (
        options: CtpOptions,
        ?encodings: CtpEncodingOptions,
        ?useUdp: bool,
        ?useMulticast: bool,
        ?loggerFactory: ILoggerFactory,
        ?flowControl: CtpFlowControlOptions,
        ?autoResubscribe: bool,
        ?endpoint: CtpEndpoint
    )
    =
    let loggerFactory = defaultArg loggerFactory Abstractions.NullLoggerFactory.Instance
    let logger = loggerFactory.CreateLogger<MdClient>()
    let coordinatorLogger = loggerFactory.CreateLogger<ConnectionCoordinator>()

    let bridgeEncodings: EncodingPair =
        let value = defaultArg encodings CtpEncodingOptions.Default

        { OutboundEncoding = value.OutboundEncoding
          InboundEncoding = value.InboundEncoding }

    let nextRequestId = ClientHelpers.nextRequestId
    let loginPending = SinglePendingResult<Result<UserLoginResponse, RspInfo>>()
    let logoutPending = SinglePendingResult<Result<UserLogoutResponse, RspInfo>>()
    let subscribePending = SinglePendingRequest<string list, Result<string list, RspInfo>>()
    let unsubscribePending = SinglePendingRequest<string list, Result<string list, RspInfo>>()
    let subscribeForQuotePending = SinglePendingRequest<string list, Result<string list, RspInfo>>()
    let unsubscribeForQuotePending = SinglePendingRequest<string list, Result<string list, RspInfo>>()
    let multicastPending = PendingQueryDict(logger = logger)

    let autoResubscribe = defaultArg autoResubscribe true
    let subscribedInstruments = System.Collections.Generic.HashSet<string>()
    let subscribedInstrumentsLock = obj ()
    let autoReconnectSemaphore = new SemaphoreSlim(1, 1)

    let frontConnectedEvent = Event<unit>()
    let frontDisconnectedEvent = Event<int>()
    let heartBeatWarningEvent = Event<int>()
    let rspErrorEvent = Event<RspInfo>()
    let depthMarketDataEvent = Event<DepthMarketData>()
    let multicastInstrumentEvent = Event<MulticastInstrumentResponse>()
    let forQuoteRspEvent = Event<ForQuoteRspResponse>()

    let api =
        new MdApi(
            options.FlowPath,
            defaultArg useUdp false,
            defaultArg useMulticast false,
            options.ProductionMode,
            encodings = bridgeEncodings
        )

    let mutable configuredEndpoint = defaultArg endpoint (CtpEndpoint.Front options.FrontAddress)
    let mutable fensUserInfo =
        match configuredEndpoint with
        | CtpEndpoint.NameServer(_, fens) -> fens
        | CtpEndpoint.Front _ -> None

    let validateFensUserInfo (request: FensUserInfoRequest) =
        if String.IsNullOrWhiteSpace request.BrokerId then
            invalidArg "BrokerId" "FENS broker id must not be empty."

        if String.IsNullOrWhiteSpace request.UserId then
            invalidArg "UserId" "FENS user id must not be empty."

    let registerEndpoint () =
        match configuredEndpoint with
        | CtpEndpoint.Front address ->
            if String.IsNullOrWhiteSpace address then
                invalidArg (nameof options.FrontAddress) "Front address must not be empty."

            api.RegisterFront(address)
        | CtpEndpoint.NameServer(address, fens) ->
            if String.IsNullOrWhiteSpace address then
                invalidArg (nameof address) "NameServer address must not be empty."

            api.RegisterNameServer(address)
            fensUserInfo <- fens |> Option.orElse fensUserInfo
            fensUserInfo
            |> Option.iter (fun request ->
                validateFensUserInfo request
                api.RegisterFensUserInfo request)

    let requestFlow = FlowController(defaultArg flowControl CtpFlowControlOptions.Default, logger = logger)

    let connectionCoordinator =
        ConnectionCoordinator(
            (fun () ->
                registerEndpoint ()
                api.Init()),
            logger = coordinatorLogger
        )

    let agent =
        MailboxProcessor.Start(fun inbox ->
            let rec loop () = async {
                let! message = inbox.Receive()

                match message with
                | FrontConnected ->
                    connectionCoordinator.HandleFrontConnected()
                    frontConnectedEvent.Trigger()
                | FrontDisconnected reason ->
                    connectionCoordinator.HandleFrontDisconnected()
                    frontDisconnectedEvent.Trigger reason
                | HeartBeatWarning timeLapse -> heartBeatWarningEvent.Trigger timeLapse
                | RspError(rspInfo, _, isLast) ->
                    rspInfo
                    |> Option.iter (fun info ->
                        logger.LogError("CTP error: [{ErrorId}] {ErrorMessage}", info.ErrorId, info.ErrorMessage)
                        rspErrorEvent.Trigger info)

                    if isLast then
                        rspInfo
                        |> Option.iter (fun info ->
                            logger.LogWarning("RspError isLast=true, failing all pending operations")
                            loginPending.TrySetResult(Error info)
                            logoutPending.TrySetResult(Error info))
                | RspUserLogin(login, rspInfo, _, isLast) when isLast ->
                    let result =
                        match ClientHelpers.resultFromRspInfo rspInfo, login with
                        | Error info, _ ->
                            logger.LogError(
                                "Md login failed: [{ErrorId}] {ErrorMessage}",
                                info.ErrorId,
                                info.ErrorMessage
                            )

                            Error info
                        | Ok(), Some value ->
                            logger.LogInformation("Md login succeeded")
                            Ok value
                        | Ok(), None -> Error(ClientHelpers.apiReturnError -2)

                    loginPending.TrySetResult result
                | RspUserLogout(logout, rspInfo, _, isLast) when isLast ->
                    let result =
                        match ClientHelpers.resultFromRspInfo rspInfo, logout with
                        | Error info, _ ->
                            logger.LogError(
                                "Md logout failed: [{ErrorId}] {ErrorMessage}",
                                info.ErrorId,
                                info.ErrorMessage
                            )

                            Error info
                        | Ok(), Some value ->
                            logger.LogInformation("Md logout succeeded")
                            Ok value
                        | Ok(), None -> Error(ClientHelpers.apiReturnError -2)

                    logoutPending.TrySetResult result
                | RspSubMarketData(_, rspInfo, _, isLast) ->
                    match ClientHelpers.resultFromRspInfo rspInfo with
                    | Error info -> subscribePending.TrySetResult(Error info)
                    | Ok() when isLast -> subscribePending.TrySetResultFromRequest Ok
                    | _ -> ()
                | RspUnsubMarketData(_, rspInfo, _, isLast) ->
                    match ClientHelpers.resultFromRspInfo rspInfo with
                    | Error info -> unsubscribePending.TrySetResult(Error info)
                    | Ok() when isLast -> unsubscribePending.TrySetResultFromRequest Ok
                    | _ -> ()
                | RspQryMulticastInstrument(instrument, rspInfo, requestId, isLast) ->
                    multicastPending.TryAccumulate(requestId, instrument, rspInfo, isLast)
                    instrument |> Option.iter multicastInstrumentEvent.Trigger
                | RspSubForQuoteRsp(_, rspInfo, _, isLast) ->
                    match ClientHelpers.resultFromRspInfo rspInfo with
                    | Error info -> subscribeForQuotePending.TrySetResult(Error info)
                    | Ok() when isLast -> subscribeForQuotePending.TrySetResultFromRequest Ok
                    | _ -> ()
                | RspUnsubForQuoteRsp(_, rspInfo, _, isLast) ->
                    match ClientHelpers.resultFromRspInfo rspInfo with
                    | Error info -> unsubscribeForQuotePending.TrySetResult(Error info)
                    | Ok() when isLast -> unsubscribeForQuotePending.TrySetResultFromRequest Ok
                    | _ -> ()
                | RtnDepthMarketData data -> depthMarketDataEvent.Trigger data
                | RtnForQuoteRsp data -> forQuoteRspEvent.Trigger data
                | _ -> ()

                return! loop ()
            }

            loop ())

    do
        api.SetCallbacks
            { MdCallbacks.Empty with
                FrontConnected = Some(fun () -> agent.Post FrontConnected)
                FrontDisconnected = Some(fun reason -> agent.Post(FrontDisconnected reason))
                HeartBeatWarning = Some(fun lapse -> agent.Post(HeartBeatWarning lapse))
                RspError = Some(fun rsp requestId isLast -> agent.Post(RspError(rsp, requestId, isLast)))
                RspUserLogin =
                    Some(fun login rsp requestId isLast -> agent.Post(RspUserLogin(login, rsp, requestId, isLast)))
                RspUserLogout =
                    Some(fun logout rsp requestId isLast -> agent.Post(RspUserLogout(logout, rsp, requestId, isLast)))
                RspSubMarketData =
                    Some(fun instrument rsp requestId isLast ->
                        agent.Post(RspSubMarketData(instrument, rsp, requestId, isLast)))
                RspUnsubMarketData =
                    Some(fun instrument rsp requestId isLast ->
                        agent.Post(RspUnsubMarketData(instrument, rsp, requestId, isLast)))
                RspQryMulticastInstrument =
                    Some(fun instrument rsp requestId isLast ->
                        agent.Post(RspQryMulticastInstrument(instrument, rsp, requestId, isLast)))
                RspSubForQuoteRsp =
                    Some(fun instrument rsp requestId isLast ->
                        agent.Post(RspSubForQuoteRsp(instrument, rsp, requestId, isLast)))
                RspUnsubForQuoteRsp =
                    Some(fun instrument rsp requestId isLast ->
                        agent.Post(RspUnsubForQuoteRsp(instrument, rsp, requestId, isLast)))
                RtnDepthMarketData = Some(fun data -> agent.Post(RtnDepthMarketData data))
                RtnForQuoteRsp = Some(fun data -> agent.Post(RtnForQuoteRsp data)) }

    member _.FrontConnected = frontConnectedEvent.Publish
    member _.FrontDisconnected = frontDisconnectedEvent.Publish
    member _.HeartBeatWarning = heartBeatWarningEvent.Publish
    member _.RspError = rspErrorEvent.Publish
    member _.DepthMarketDataReceived = depthMarketDataEvent.Publish
    member _.MulticastInstrumentReceived = multicastInstrumentEvent.Publish
    member _.ForQuoteRspReceived = forQuoteRspEvent.Publish

    member _.GetApiVersion() = MdApi.GetApiVersion()

    member _.GetTradingDay() = api.GetTradingDay()

    // These registrations must be completed before Connect. The endpoint constructor option
    // performs the same registration automatically for new callers.
    member _.RegisterNameServer(nsAddress: string) =
        if String.IsNullOrWhiteSpace nsAddress then
            invalidArg (nameof nsAddress) "NameServer address must not be empty."

        api.RegisterNameServer(nsAddress)
        configuredEndpoint <- CtpEndpoint.NameServer(nsAddress, fensUserInfo)

    member _.RegisterFensUserInfo(request: FensUserInfoRequest) =
        validateFensUserInfo request
        api.RegisterFensUserInfo(request)
        fensUserInfo <- Some request

    member this.Connect(?timeout: TimeSpan) = async {
        let! result =
            match timeout with
            | Some timeout -> connectionCoordinator.Connect(timeout = timeout)
            | None -> connectionCoordinator.Connect()

        match result with
        | Ok() when autoResubscribe && connectionCoordinator.ConnectionCount > 1 ->
            Async.Start(this.AutoResubscribeAsync())
        | _ -> ()

        return result
    }

    member _.Join() = api.Join()

    member private _.RunSinglePendingOperationAsync<'TRequest, 'TResponse>
        (operationName: string)
        (request: 'TRequest)
        (pendingState: SinglePendingResult<Result<'TResponse, RspInfo>>)
        (apiCall: 'TRequest * int -> int)
        (onAccepted: 'TRequest -> unit)
        : Async<Result<'TResponse, RspInfo>>
        =
        async {
            let! cancellationToken = Async.CancellationToken

            let rec executeAttempt attempt = async {
                do!
                    requestFlow.AwaitDispatchAsync(cancellationToken = cancellationToken)
                    |> Async.AwaitTask

                logger.LogDebug("Sending {OperationName} request", operationName)

                let requestId = nextRequestId ()
                let completion = pendingState.Begin()
                let result = apiCall (request, requestId)

                if result <> 0 then
                    pendingState.TryTake() |> ignore

                    if requestFlow.ShouldRetryNativeReturnCode(attempt, result) then
                        do!
                            requestFlow.DelayBeforeNativeRetryAsync(
                                operationName,
                                attempt + 1,
                                result,
                                cancellationToken = cancellationToken
                            )
                            |> Async.AwaitTask

                        return! executeAttempt (attempt + 1)
                    else
                        logger.LogError(
                            "{OperationName} request failed with native return code {ReturnCode}",
                            operationName,
                            result
                        )

                        completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore
                        return! (completion.Task |> ClientHelpers.awaitTaskWithCancellation cancellationToken)
                else
                    onAccepted request
                    return! (completion.Task |> ClientHelpers.awaitTaskWithCancellation cancellationToken)
            }

            return! executeAttempt 0
        }

    member private _.RunSubscriptionBatchAsync
        (operationName: string)
        (requested: string list)
        (pendingState: SinglePendingRequest<string list, Result<string list, RspInfo>>)
        (apiCall: string list -> int)
        : Async<Result<string list, RspInfo>>
        =
        async {
            let! cancellationToken = Async.CancellationToken

            let rec executeAttempt attempt = async {
                do!
                    requestFlow.AwaitDispatchAsync(cancellationToken = cancellationToken)
                    |> Async.AwaitTask

                logger.LogDebug(
                    "Sending {OperationName} batch for {InstrumentCount} instruments",
                    operationName,
                    requested.Length
                )

                let completion = pendingState.Begin requested
                let result = apiCall requested

                if result <> 0 then
                    pendingState.TryTake() |> ignore

                    if requestFlow.ShouldRetryNativeReturnCode(attempt, result) then
                        do!
                            requestFlow.DelayBeforeNativeRetryAsync(
                                operationName,
                                attempt + 1,
                                result,
                                cancellationToken = cancellationToken
                            )
                            |> Async.AwaitTask

                        return! executeAttempt (attempt + 1)
                    else
                        logger.LogError(
                            "{OperationName} failed with native return code {ReturnCode}",
                            operationName,
                            result
                        )

                        completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore
                        return! (completion.Task |> ClientHelpers.awaitTaskWithCancellation cancellationToken)
                else
                    return! (completion.Task |> ClientHelpers.awaitTaskWithCancellation cancellationToken)
            }

            return! executeAttempt 0
        }

    member private this.RunSubscriptionAsync
        (operationName: string)
        (instrumentIds: string list)
        (pendingState: SinglePendingRequest<string list, Result<string list, RspInfo>>)
        (apiCall: string list -> int)
        : Async<Result<string list, RspInfo>>
        =
        async {
            let! cancellationToken = Async.CancellationToken
            let batches = requestFlow.BatchSubscriptions instrumentIds

            let rec loop completedBatches remainingBatches = async {
                match remainingBatches with
                | [] -> return Ok(List.rev completedBatches |> List.concat)
                | batch :: rest ->
                    let! result = this.RunSubscriptionBatchAsync operationName batch pendingState apiCall

                    match result with
                    | Error error -> return Error error
                    | Ok completedBatch ->
                        match rest with
                        | [] -> return Ok(List.rev (completedBatch :: completedBatches) |> List.concat)
                        | _ ->
                            do!
                                Task.Delay(requestFlow.SubscriptionBatchDelay, cancellationToken)
                                |> Async.AwaitTask

                            return! loop (completedBatch :: completedBatches) rest
            }

            return! loop [] batches
        }

    member this.LoginAsync() =
        let request = OptionHelpers.createUserLoginRequest options

        this.RunSinglePendingOperationAsync<UserLoginRequest, UserLoginResponse>
            "Login"
            request
            loginPending
            api.ReqUserLogin
            ignore

    member this.LogoutAsync() =
        let request = OptionHelpers.createUserLogoutRequest options

        this.RunSinglePendingOperationAsync<UserLogoutRequest, UserLogoutResponse>
            "Logout"
            request
            logoutPending
            api.ReqUserLogout
            (fun acceptedRequest ->
                // Current CTP SDK does not reliably invoke OnRspUserLogout, so a successful request is treated as completion.
                logoutPending.TrySetResult(Ok { BrokerId = acceptedRequest.BrokerId; UserId = acceptedRequest.UserId }))

    member this.SubscribeMarketDataAsync(instrumentIds: string seq) = async {
        let requested = List.ofSeq instrumentIds
        logger.LogDebug("Subscribing to {InstrumentCount} instruments", requested.Length)

        let! result = this.RunSubscriptionAsync "SubscribeMarketData" requested subscribePending api.SubscribeMarketData

        match result with
        | Ok instruments ->
            lock subscribedInstrumentsLock (fun () ->
                instruments |> List.iter (fun i -> subscribedInstruments.Add(i) |> ignore))
        | Error _ -> ()

        return result
    }

    member this.UnsubscribeMarketDataAsync(instrumentIds: string seq) = async {
        let requested = List.ofSeq instrumentIds
        logger.LogDebug("Unsubscribing from {InstrumentCount} instruments", requested.Length)

        let! result =
            this.RunSubscriptionAsync "UnsubscribeMarketData" requested unsubscribePending api.UnsubscribeMarketData

        match result with
        | Ok instruments ->
            lock subscribedInstrumentsLock (fun () ->
                instruments |> List.iter (fun i -> subscribedInstruments.Remove(i) |> ignore))
        | Error _ -> ()

        return result
    }

    member this.SubscribeForQuoteRspAsync(instrumentIds: string seq) = async {
        let requested = List.ofSeq instrumentIds
        logger.LogDebug("Subscribing for quote responses for {InstrumentCount} instruments", requested.Length)

        return!
            this.RunSubscriptionAsync
                "SubscribeForQuoteRsp"
                requested
                subscribeForQuotePending
                api.SubscribeForQuoteRsp
    }

    member this.UnsubscribeForQuoteRspAsync(instrumentIds: string seq) = async {
        let requested = List.ofSeq instrumentIds
        logger.LogDebug("Unsubscribing for quote responses for {InstrumentCount} instruments", requested.Length)

        return!
            this.RunSubscriptionAsync
                "UnsubscribeForQuoteRsp"
                requested
                unsubscribeForQuotePending
                api.UnsubscribeForQuoteRsp
    }

    member private _.QueryAsync<'TItem, 'TRequest>
        (queryName: string)
        (request: 'TRequest)
        (apiCall: 'TRequest * int -> int)
        : Async<Result<'TItem list, RspInfo>>
        =
        async {
            let! cancellationToken = Async.CancellationToken

            let! queryLease =
                requestFlow.AcquireQueryExecutionAsync(cancellationToken = cancellationToken)
                |> Async.AwaitTask

            let queryLifetime = task {
                try
                    let rec executeAttempt attempt = task {
                        do! requestFlow.AwaitQueryDispatchAsync(cancellationToken = cancellationToken)
                        logger.LogDebug("Sending {QueryName} request", queryName)

                        let requestId = nextRequestId ()
                        let completion = ClientHelpers.createCompletionSource<Result<'TItem list, RspInfo>> ()
                        multicastPending.Register(requestId, queryName, completion)
                        let errCode = apiCall (request, requestId)

                        if errCode <> 0 then
                            multicastPending.TryRemove requestId

                            if requestFlow.ShouldRetryNativeReturnCode(attempt, errCode) then
                                do!
                                    requestFlow.DelayBeforeNativeRetryAsync(
                                        queryName,
                                        attempt + 1,
                                        errCode,
                                        cancellationToken = cancellationToken
                                    )

                                return! executeAttempt (attempt + 1)
                            else
                                logger.LogError(
                                    "{QueryName} request failed with native return code {ReturnCode}",
                                    queryName,
                                    errCode
                                )

                                return Error(ClientHelpers.apiReturnError errCode)
                        else
                            let! result =
                                requestFlow.AwaitQueryCompletionAsync(
                                    queryName,
                                    requestId,
                                    multicastPending,
                                    completion.Task
                                )

                            match result with
                            | Error info when requestFlow.ShouldRetryQueryError(attempt, info) ->
                                do!
                                    requestFlow.DelayBeforeQueryRetryAsync(
                                        queryName,
                                        attempt + 1,
                                        info.ErrorId,
                                        cancellationToken = cancellationToken
                                    )

                                return! executeAttempt (attempt + 1)
                            | _ -> return result
                    }

                    return! executeAttempt 0
                finally
                    queryLease.Dispose()
            }

            return! (queryLifetime |> ClientHelpers.awaitTaskWithCancellation cancellationToken)
        }

    member this.QueryMulticastInstrumentAsync(topicId: int, ?instrumentId: string) =
        let request =
            { TopicId = topicId
              InstrumentId = instrumentId }

        this.QueryAsync<MulticastInstrumentResponse, QryMulticastInstrumentRequest>
            "QryMulticastInstrument"
            request
            api.ReqQryMulticastInstrument

    member private this.AutoResubscribeAsync() = async {
        let entered = autoReconnectSemaphore.Wait(0)

        if not entered then
            return ()
        else
            try
                try
                    let instruments =
                        lock subscribedInstrumentsLock (fun () ->
                            if subscribedInstruments.Count = 0 then
                                []
                            else
                                List.ofSeq subscribedInstruments)

                    if instruments.IsEmpty then
                        return ()
                    else
                        logger.LogInformation(
                            "Auto-resubscribing {InstrumentCount} instruments after reconnection",
                            instruments.Length
                        )

                        let! loginResult = this.LoginAsync()

                        match loginResult with
                        | Error info ->
                            logger.LogWarning(
                                "Auto-resubscription login failed: [{ErrorId}] {ErrorMessage}",
                                info.ErrorId,
                                info.ErrorMessage
                            )

                            return ()
                        | Ok _ ->
                            let! subscribeResult = this.SubscribeMarketDataAsync(instruments)

                            match subscribeResult with
                            | Ok _ -> logger.LogInformation("Auto-resubscription completed successfully")
                            | Error info ->
                                logger.LogWarning(
                                    "Auto-resubscription failed: [{ErrorId}] {ErrorMessage}",
                                    info.ErrorId,
                                    info.ErrorMessage
                                )
                with
                | :? InvalidOperationException ->
                    logger.LogDebug("User is driving reconnection, auto-resubscription yields")
                | ex -> logger.LogWarning(ex, "Auto-resubscription failed")
            finally
                autoReconnectSemaphore.Release() |> ignore
    }

    interface IDisposable with
        member _.Dispose() = (api :> IDisposable).Dispose()
