namespace Ctp.Net

open System
open FSharpPlus
open Ctp.Net.Bridge
open Microsoft.Extensions.Logging

type private MdAgentMessage =
    | FrontConnected
    | FrontDisconnected of int
    | HeartBeatWarning of int
    | RspError of RspInfo option * int * bool
    | RspUserLogin of UserLoginResponse option * RspInfo option * int * bool
    | RspSubMarketData of SpecificInstrument option * RspInfo option * int * bool
    | RspUnsubMarketData of SpecificInstrument option * RspInfo option * int * bool
    | RspUserLogout of UserLogoutResponse option * RspInfo option * int * bool
    | RtnDepthMarketData of DepthMarketData

type MdClient
    (
        options: CtpOptions,
        ?encodings: CtpEncodingOptions,
        ?useUdp: bool,
        ?useMulticast: bool,
        ?loggerFactory: ILoggerFactory
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

    member _.Connect(?timeout: TimeSpan) = connectionCoordinator.Connect(?timeout = timeout)

    member _.Join() = api.Join()

    member _.LoginAsync() =
        logger.LogDebug("Sending login request")
        let requestId = nextRequestId ()
        let request = OptionHelpers.createUserLoginRequest options
        let completion = loginPending.Begin()
        let result = api.ReqUserLogin(request, requestId)

        if result <> 0 then
            logger.LogError("Login request failed with native return code {ReturnCode}", result)
            loginPending.TryTake() |> ignore
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    member _.LogoutAsync() =
        logger.LogDebug("Sending logout request")
        let requestId = nextRequestId ()
        let request = OptionHelpers.createUserLogoutRequest options
        let completion = logoutPending.Begin()
        let result = api.ReqUserLogout(request, requestId)

        if result <> 0 then
            logger.LogError("Logout request failed with native return code {ReturnCode}", result)
            logoutPending.TryTake() |> ignore
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore
        else
            // Current CTP SDK does not reliably invoke OnRspUserLogout, so a successful request is treated as completion.
            logoutPending.TrySetResult(Ok { BrokerId = request.BrokerId; UserId = request.UserId })

        completion.Task |> ClientHelpers.awaitTask

    member _.SubscribeMarketDataAsync(instrumentIds: string seq) =
        let requested = List.ofSeq instrumentIds
        logger.LogDebug("Subscribing to {InstrumentCount} instruments", requested.Length)
        let completion = subscribePending.Begin requested
        let result = api.SubscribeMarketData requested

        if result <> 0 then
            logger.LogError("SubscribeMarketData failed with native return code {ReturnCode}", result)
            subscribePending.TryTake() |> ignore
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    member _.UnsubscribeMarketDataAsync(instrumentIds: string seq) =
        let requested = List.ofSeq instrumentIds
        logger.LogDebug("Unsubscribing from {InstrumentCount} instruments", requested.Length)
        let completion = unsubscribePending.Begin requested
        let result = api.UnsubscribeMarketData requested

        if result <> 0 then
            logger.LogError("UnsubscribeMarketData failed with native return code {ReturnCode}", result)
            unsubscribePending.TryTake() |> ignore
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    interface IDisposable with
        member _.Dispose() = (api :> IDisposable).Dispose()
