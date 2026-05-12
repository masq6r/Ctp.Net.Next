namespace Ctp.Net

open System
open System.Collections.Concurrent
open System.Threading.Tasks
open Ctp.Bridge.Net

type private TraderAgentMessage =
    | FrontConnected
    | FrontDisconnected of int
    | HeartBeatWarning of int
    | RspError of CtpError option * int * bool
    | RspAuthenticate of AuthenticateResponse option * CtpError option * int * bool
    | RspSettlementInfoConfirm of SettlementInfoConfirm option * CtpError option * int * bool
    | RspUserLogin of UserLoginResponse option * CtpError option * int * bool
    | RspUserLogout of UserLogoutResponse option * CtpError option * int * bool
    | RspQryTradingAccount of TradingAccount option * CtpError option * int * bool
    | RspQryInvestorPosition of InvestorPosition option * CtpError option * int * bool
    | RspOrderInsert of InputOrderRequest option * CtpError option * int * bool
    | RspOrderAction of InputOrderActionRequest option * CtpError option * int * bool
    | RtnOrder of OrderUpdate
    | RtnTrade of TradeUpdate

type TraderClient
    (
        options: CtpOptions,
        ?encodings: CtpEncodingOptions,
        ?privateTopicResumeType: int,
        ?privateTopicSequenceNo: int,
        ?publicTopicResumeType: int
    )
    =
    let bridgeEncodings: EncodingPair =
        let value = defaultArg encodings CtpEncodingOptions.Default

        { OutboundEncoding = value.OutboundEncoding
          InboundEncoding = value.InboundEncoding }

    let nextRequestId = ClientHelpers.nextRequestId
    let authenticatePending = SinglePendingResult<Result<AuthenticateResponse, CtpError>>()
    let settlementInfoConfirmPending = SinglePendingResult<Result<SettlementInfoConfirm, CtpError>>()
    let loginPending = SinglePendingResult<Result<UserLoginResponse, CtpError>>()
    let logoutPending = SinglePendingResult<Result<UserLogoutResponse, CtpError>>()

    let tradingAccountPending =
        ConcurrentDictionary<
            int,
            ResizeArray<TradingAccount> * TaskCompletionSource<Result<TradingAccount list, CtpError>>
         >()

    let investorPositionPending =
        ConcurrentDictionary<
            int,
            ResizeArray<InvestorPosition> * TaskCompletionSource<Result<InvestorPosition list, CtpError>>
         >()

    let frontConnectedEvent = Event<unit>()
    let frontDisconnectedEvent = Event<int>()
    let heartBeatWarningEvent = Event<int>()
    let rspErrorEvent = Event<CtpError>()
    let orderEvent = Event<OrderUpdate>()
    let tradeEvent = Event<TradeUpdate>()
    let api = new TraderApi(options.FlowPath, options.ProductionMode, encodings = bridgeEncodings)

    let connectionCoordinator =
        ConnectionCoordinator(fun () ->
            api.RegisterFront(options.FrontAddress)
            api.SubscribePrivateTopic(defaultArg privateTopicResumeType 0, defaultArg privateTopicSequenceNo 1)
            api.SubscribePublicTopic(defaultArg publicTopicResumeType 0)
            api.Init())

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
                | HeartBeatWarning lapse -> heartBeatWarningEvent.Trigger lapse
                | RspError(rspInfo, requestId, isLast) ->
                    rspInfo |> Option.iter rspErrorEvent.Trigger

                    if isLast then
                        rspInfo
                        |> Option.iter (fun info ->
                            authenticatePending.TrySetResult(Error info)
                            settlementInfoConfirmPending.TrySetResult(Error info)
                            loginPending.TrySetResult(Error info)
                            logoutPending.TrySetResult(Error info)

                            let mutable accounts = Unchecked.defaultof<_>

                            if tradingAccountPending.TryRemove(requestId, &accounts) then
                                snd accounts |> fun tcs -> tcs.TrySetResult(Error info) |> ignore

                            let mutable positions = Unchecked.defaultof<_>

                            if investorPositionPending.TryRemove(requestId, &positions) then
                                snd positions |> fun tcs -> tcs.TrySetResult(Error info) |> ignore)
                | RspAuthenticate(response, rspInfo, _, isLast) when isLast ->
                    let result =
                        match ClientHelpers.resultFromRspInfo rspInfo, response with
                        | Error info, _ -> Error info
                        | Ok(), Some value -> Ok value
                        | Ok(), None -> Error(ClientHelpers.apiReturnError -2)

                    authenticatePending.TrySetResult result
                | RspSettlementInfoConfirm(response, rspInfo, _, isLast) when isLast ->
                    let result =
                        match ClientHelpers.resultFromRspInfo rspInfo, response with
                        | Error info, _ -> Error info
                        | Ok(), Some value -> Ok value
                        | Ok(), None -> Error(ClientHelpers.apiReturnError -2)

                    settlementInfoConfirmPending.TrySetResult result
                | RspUserLogin(response, rspInfo, _, isLast) when isLast ->
                    let result =
                        match ClientHelpers.resultFromRspInfo rspInfo, response with
                        | Error info, _ -> Error info
                        | Ok(), Some value -> Ok value
                        | Ok(), None -> Error(ClientHelpers.apiReturnError -2)

                    loginPending.TrySetResult result
                | RspUserLogout(response, rspInfo, _, isLast) when isLast ->
                    let result =
                        match ClientHelpers.resultFromRspInfo rspInfo, response with
                        | Error info, _ -> Error info
                        | Ok(), Some value -> Ok value
                        | Ok(), None -> Error(ClientHelpers.apiReturnError -2)

                    logoutPending.TrySetResult result
                | RspQryTradingAccount(account, rspInfo, requestId, isLast) ->
                    let mutable state = Unchecked.defaultof<_>

                    if tradingAccountPending.TryGetValue(requestId, &state) then
                        account |> Option.iter (fun item -> fst state |> fun items -> items.Add(item))

                        if isLast then
                            let mutable removed = Unchecked.defaultof<_>

                            if tradingAccountPending.TryRemove(requestId, &removed) then
                                match ClientHelpers.resultFromRspInfo rspInfo with
                                | Error info -> snd removed |> fun tcs -> tcs.TrySetResult(Error info) |> ignore
                                | Ok() ->
                                    snd removed
                                    |> fun tcs -> tcs.TrySetResult(Ok(List.ofSeq (fst removed))) |> ignore
                | RspQryInvestorPosition(position, rspInfo, requestId, isLast) ->
                    let mutable state = Unchecked.defaultof<_>

                    if investorPositionPending.TryGetValue(requestId, &state) then
                        position |> Option.iter (fun item -> fst state |> fun items -> items.Add(item))

                        if isLast then
                            let mutable removed = Unchecked.defaultof<_>

                            if investorPositionPending.TryRemove(requestId, &removed) then
                                match ClientHelpers.resultFromRspInfo rspInfo with
                                | Error info -> snd removed |> fun tcs -> tcs.TrySetResult(Error info) |> ignore
                                | Ok() ->
                                    snd removed
                                    |> fun tcs -> tcs.TrySetResult(Ok(List.ofSeq (fst removed))) |> ignore
                | RspOrderInsert(_, rspInfo, _, isLast) when isLast ->
                    rspInfo
                    |> Option.filter (fun info -> info.ErrorId <> 0)
                    |> Option.iter rspErrorEvent.Trigger
                | RspOrderAction(_, rspInfo, _, isLast) when isLast ->
                    rspInfo
                    |> Option.filter (fun info -> info.ErrorId <> 0)
                    |> Option.iter rspErrorEvent.Trigger
                | RtnOrder order -> orderEvent.Trigger order
                | RtnTrade trade -> tradeEvent.Trigger trade
                | _ -> ()

                return! loop ()
            }

            loop ())

    do
        api.SetCallbacks
            { TraderCallbacks.Empty with
                FrontConnected = Some(fun () -> agent.Post FrontConnected)
                FrontDisconnected = Some(fun reason -> agent.Post(FrontDisconnected reason))
                HeartBeatWarning = Some(fun lapse -> agent.Post(HeartBeatWarning lapse))
                RspError = Some(fun rsp requestId isLast -> agent.Post(RspError(rsp, requestId, isLast)))
                RspAuthenticate =
                    Some(fun response rsp requestId isLast ->
                        agent.Post(RspAuthenticate(response, rsp, requestId, isLast)))
                RspSettlementInfoConfirm =
                    Some(fun response rsp requestId isLast ->
                        agent.Post(RspSettlementInfoConfirm(response, rsp, requestId, isLast)))
                RspUserLogin =
                    Some(fun response rsp requestId isLast ->
                        agent.Post(RspUserLogin(response, rsp, requestId, isLast)))
                RspUserLogout =
                    Some(fun response rsp requestId isLast ->
                        agent.Post(RspUserLogout(response, rsp, requestId, isLast)))
                RspQryTradingAccount =
                    Some(fun account rsp requestId isLast ->
                        agent.Post(RspQryTradingAccount(account, rsp, requestId, isLast)))
                RspQryInvestorPosition =
                    Some(fun position rsp requestId isLast ->
                        agent.Post(RspQryInvestorPosition(position, rsp, requestId, isLast)))
                RspOrderInsert =
                    Some(fun request rsp requestId isLast ->
                        agent.Post(RspOrderInsert(request, rsp, requestId, isLast)))
                RspOrderAction =
                    Some(fun request rsp requestId isLast ->
                        agent.Post(RspOrderAction(request, rsp, requestId, isLast)))
                RtnOrder = Some(fun order -> agent.Post(RtnOrder order))
                RtnTrade = Some(fun trade -> agent.Post(RtnTrade trade)) }

    member _.FrontConnected = frontConnectedEvent.Publish
    member _.FrontDisconnected = frontDisconnectedEvent.Publish
    member _.HeartBeatWarning = heartBeatWarningEvent.Publish
    member _.RspError = rspErrorEvent.Publish
    member _.OrderReceived = orderEvent.Publish
    member _.TradeReceived = tradeEvent.Publish

    member _.Connect(?timeout: TimeSpan) = connectionCoordinator.Connect(?timeout = timeout)

    member _.Join() = api.Join()

    member _.AuthenticateAsync() =
        let requestId = nextRequestId ()
        let request = OptionHelpers.createAuthenticateRequest options
        let completion = authenticatePending.Begin()
        let result = api.ReqAuthenticate(request, requestId)

        if result <> 0 then
            authenticatePending.TryTake() |> ignore
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    member _.SettlementInfoConfirmAsync() =
        let requestId = nextRequestId ()
        let request = OptionHelpers.createSettlementInfoConfirmRequest options
        let completion = settlementInfoConfirmPending.Begin()
        let result = api.ReqSettlementInfoConfirm(request, requestId)

        if result <> 0 then
            settlementInfoConfirmPending.TryTake() |> ignore
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    member _.LoginAsync() =
        let requestId = nextRequestId ()
        let request = OptionHelpers.createUserLoginRequest options
        let completion = loginPending.Begin()
        let result = api.ReqUserLogin(request, requestId)

        if result <> 0 then
            loginPending.TryTake() |> ignore
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    member _.LogoutAsync() =
        let requestId = nextRequestId ()
        let request = OptionHelpers.createUserLogoutRequest options
        let completion = logoutPending.Begin()
        let result = api.ReqUserLogout(request, requestId)

        if result <> 0 then
            logoutPending.TryTake() |> ignore
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore
        else
            // Current CTP SDK does not reliably invoke OnRspUserLogout, so a successful request is treated as completion.
            logoutPending.TrySetResult(Ok { BrokerId = request.BrokerId; UserId = request.UserId })

        completion.Task |> ClientHelpers.awaitTask

    member _.QueryTradingAccountAsync(request: QueryTradingAccountRequest) =
        let requestId = nextRequestId ()
        let completion = ClientHelpers.createCompletionSource<Result<TradingAccount list, CtpError>> ()
        tradingAccountPending[requestId] <- (ResizeArray(), completion)
        let result = api.ReqQryTradingAccount(request, requestId)

        if result <> 0 then
            tradingAccountPending.TryRemove(requestId) |> ignore
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    member _.QueryInvestorPositionAsync(request: QueryInvestorPositionRequest) =
        let requestId = nextRequestId ()
        let completion = ClientHelpers.createCompletionSource<Result<InvestorPosition list, CtpError>> ()
        investorPositionPending[requestId] <- (ResizeArray(), completion)
        let result = api.ReqQryInvestorPosition(request, requestId)

        if result <> 0 then
            investorPositionPending.TryRemove(requestId) |> ignore
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    member _.InsertOrderAsync(request: InputOrderRequest) = async {
        let requestId = nextRequestId ()
        let result = api.ReqOrderInsert(request, requestId)

        return
            if result = 0 then
                Ok requestId
            else
                Error(ClientHelpers.apiReturnError result)
    }

    member _.CancelOrderAsync(request: InputOrderActionRequest) = async {
        let requestId = nextRequestId ()
        let result = api.ReqOrderAction(request, requestId)

        return
            if result = 0 then
                Ok requestId
            else
                Error(ClientHelpers.apiReturnError result)
    }

    interface IDisposable with
        member _.Dispose() = (api :> IDisposable).Dispose()
