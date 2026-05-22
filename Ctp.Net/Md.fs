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
    | RspUserLogout of UserLogoutResponse option * RspInfo option * int * bool
    | RtnDepthMarketData of DepthMarketData

type MdClient
    (
        options: CtpOptions,
        ?encodings: CtpEncodingOptions,
        ?useUdp: bool,
        ?useMulticast: bool,
        ?loggerFactory: ILoggerFactory,
        ?flowControl: CtpFlowControlOptions
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

    let frontConnectedEvent = Event<unit>()
    let frontDisconnectedEvent = Event<int>()
    let heartBeatWarningEvent = Event<int>()
    let rspErrorEvent = Event<RspInfo>()
    let depthMarketDataEvent = Event<DepthMarketData>()

    let api =
        new MdApi(
            options.FlowPath,
            defaultArg useUdp false,
            defaultArg useMulticast false,
            options.ProductionMode,
            encodings = bridgeEncodings
        )

    let requestFlow = FlowController(defaultArg flowControl CtpFlowControlOptions.Default, logger = logger)

    let connectionCoordinator =
        ConnectionCoordinator(
            (fun () ->
                api.RegisterFront(options.FrontAddress)
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
                | RtnDepthMarketData data -> depthMarketDataEvent.Trigger data
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
                RtnDepthMarketData = Some(fun data -> agent.Post(RtnDepthMarketData data)) }

    member _.FrontConnected = frontConnectedEvent.Publish
    member _.FrontDisconnected = frontDisconnectedEvent.Publish
    member _.HeartBeatWarning = heartBeatWarningEvent.Publish
    member _.RspError = rspErrorEvent.Publish
    member _.DepthMarketDataReceived = depthMarketDataEvent.Publish

    member _.Connect(?timeout: TimeSpan) =
        match timeout with
        | Some timeout -> connectionCoordinator.Connect(timeout = timeout)
        | None -> connectionCoordinator.Connect()

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
                    let! result =
                        this.RunSubscriptionBatchAsync operationName batch pendingState apiCall

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

    member this.SubscribeMarketDataAsync(instrumentIds: string seq) =
        let requested = List.ofSeq instrumentIds
        logger.LogDebug("Subscribing to {InstrumentCount} instruments", requested.Length)

        this.RunSubscriptionAsync
            "SubscribeMarketData"
            requested
            subscribePending
            api.SubscribeMarketData

    member this.UnsubscribeMarketDataAsync(instrumentIds: string seq) =
        let requested = List.ofSeq instrumentIds
        logger.LogDebug("Unsubscribing from {InstrumentCount} instruments", requested.Length)

        this.RunSubscriptionAsync
            "UnsubscribeMarketData"
            requested
            unsubscribePending
            api.UnsubscribeMarketData

    interface IDisposable with
        member _.Dispose() = (api :> IDisposable).Dispose()
