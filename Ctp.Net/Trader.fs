namespace Ctp.Net

open System
open Ctp.Net.Bridge
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Logging.Abstractions

[<RequireQualifiedAccess>]
type private TraderSystemEvent =
    | FrontConnected
    | FrontDisconnected of int
    | HeartBeatWarning of int
    | PrivateSeqNo of int

[<RequireQualifiedAccess>]
type private TraderPushNotification =
    | OrderReceived of OrderUpdate
    | TradeReceived of TradeUpdate

type private TraderAgentMessage =
    | SystemEvent of TraderSystemEvent
    | CorrelatedError of RspInfo option * int * bool
    | CorrelatedResponse of PendingResponseCompletionPolicy * objnull option * RspInfo option * int * bool
    | OrderCommandResponse of string * RspInfo option * int * bool
    | PushNotification of TraderPushNotification

type TraderClient
    (
        options: CtpOptions,
        ?encodings: CtpEncodingOptions,
        ?privateTopicResumeType: int,
        ?privateTopicSequenceNo: int,
        ?publicTopicResumeType: int,
        ?loggerFactory: ILoggerFactory
    )
    =
    let loggerFactory = defaultArg loggerFactory NullLoggerFactory.Instance
    let logger = loggerFactory.CreateLogger<TraderClient>()
    let coordinatorLogger = loggerFactory.CreateLogger<ConnectionCoordinator>()

    let bridgeEncodings: EncodingPair =
        let value = defaultArg encodings CtpEncodingOptions.Default

        { OutboundEncoding = value.OutboundEncoding
          InboundEncoding = value.InboundEncoding }

    let nextRequestId = ClientHelpers.nextRequestId

    let pending = PendingQueryDict(logger = logger)

    let frontConnectedEvent = Event<unit>()
    let frontDisconnectedEvent = Event<int>()
    let heartBeatWarningEvent = Event<int>()
    let privateSeqNoEvent = Event<int>()
    let rspErrorEvent = Event<RspInfo>()
    let orderEvent = Event<OrderUpdate>()
    let tradeEvent = Event<TradeUpdate>()
    let api = new TraderApi(options.FlowPath, options.ProductionMode, encodings = bridgeEncodings)

    let connectionCoordinator =
        ConnectionCoordinator(
            (fun () ->
                api.RegisterFront(options.FrontAddress)
                api.SubscribePrivateTopic(defaultArg privateTopicResumeType 0, defaultArg privateTopicSequenceNo 1)
                api.SubscribePublicTopic(defaultArg publicTopicResumeType 0)
                api.Init()),
            logger = coordinatorLogger
        )

    let agent =
        MailboxProcessor.Start(fun inbox ->
            let rec loop () = async {
                let! message = inbox.Receive()

                match message with
                | SystemEvent TraderSystemEvent.FrontConnected ->
                    connectionCoordinator.HandleFrontConnected()
                    frontConnectedEvent.Trigger()
                | SystemEvent(TraderSystemEvent.FrontDisconnected reason) ->
                    connectionCoordinator.HandleFrontDisconnected()
                    frontDisconnectedEvent.Trigger reason
                | SystemEvent(TraderSystemEvent.HeartBeatWarning lapse) -> heartBeatWarningEvent.Trigger lapse
                | SystemEvent(TraderSystemEvent.PrivateSeqNo seqNo) -> privateSeqNoEvent.Trigger seqNo
                | CorrelatedError(rspInfo, requestId, isLast) ->
                    rspInfo
                    |> Option.iter (fun info ->
                        logger.LogError("CTP error: [{ErrorId}] {ErrorMessage}", info.ErrorId, info.ErrorMessage)
                        rspErrorEvent.Trigger info)

                    if isLast then
                        rspInfo
                        |> Option.iter (fun info ->
                            logger.LogWarning("RspError isLast=true for request {RequestId}", requestId)

                            pending.TryFail(requestId, info))
                | CorrelatedResponse(completionPolicy, response, rspInfo, requestId, isLast) ->
                    pending.TryHandleResponse(requestId, response, rspInfo, isLast, completionPolicy)
                | OrderCommandResponse(operationName, rspInfo, requestId, isLast) when isLast ->
                    rspInfo
                    |> Option.filter (fun info -> info.ErrorId <> 0)
                    |> Option.iter (fun info ->
                        logger.LogError(
                            "{OperationName} failed for request {RequestId}: [{ErrorId}] {ErrorMessage}",
                            operationName,
                            requestId,
                            info.ErrorId,
                            info.ErrorMessage
                        )

                        rspErrorEvent.Trigger info)
                | PushNotification(TraderPushNotification.OrderReceived order) -> orderEvent.Trigger order
                | PushNotification(TraderPushNotification.TradeReceived trade) -> tradeEvent.Trigger trade
                | _ -> ()

                return! loop ()
            }

            loop ())

    let postCorrelatedResponse completionPolicy response rsp requestId isLast =
        agent.Post(CorrelatedResponse(completionPolicy, response |> Option.map box, rsp, requestId, isLast))

    let postOrderCommandResponse operationName _ rsp requestId isLast =
        agent.Post(OrderCommandResponse(operationName, rsp, requestId, isLast))

    do
        api.SetCallbacks
            { TraderCallbacks.Empty with
                FrontConnected = Some(fun () -> agent.Post(SystemEvent TraderSystemEvent.FrontConnected))
                FrontDisconnected =
                    Some(fun reason -> agent.Post(SystemEvent(TraderSystemEvent.FrontDisconnected reason)))
                HeartBeatWarning =
                    Some(fun lapse -> agent.Post(SystemEvent(TraderSystemEvent.HeartBeatWarning lapse)))
                RtnPrivateSeqNo =
                    Some(fun seqNo -> agent.Post(SystemEvent(TraderSystemEvent.PrivateSeqNo seqNo)))
                RspError = Some(fun rsp requestId isLast -> agent.Post(CorrelatedError(rsp, requestId, isLast)))
                RspAuthenticate = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspSettlementInfoConfirm = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspUserLogin = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspUserLogout = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspQryTradingAccount = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryInvestorPosition = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryInstrumentMarginRate = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryExchangeMarginRate = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryInstrumentCommissionRate = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspOrderInsert = Some(postOrderCommandResponse "Order insert")
                RspOrderAction = Some(postOrderCommandResponse "Order action")
                RtnOrder = Some(fun order -> agent.Post(PushNotification(TraderPushNotification.OrderReceived order)))
                RtnTrade = Some(fun trade -> agent.Post(PushNotification(TraderPushNotification.TradeReceived trade))) }

    member _.FrontConnected = frontConnectedEvent.Publish
    member _.FrontDisconnected = frontDisconnectedEvent.Publish
    member _.HeartBeatWarning = heartBeatWarningEvent.Publish
    member _.PrivateSeqNoReceived = privateSeqNoEvent.Publish
    member _.RspError = rspErrorEvent.Publish
    member _.OrderReceived = orderEvent.Publish
    member _.TradeReceived = tradeEvent.Publish

    member _.Connect(?timeout: TimeSpan) = connectionCoordinator.Connect(?timeout = timeout)

    member _.Join() = api.Join()

    member _.AuthenticateAsync() =
        logger.LogDebug("Sending authenticate request")
        let requestId = nextRequestId ()
        let request = OptionHelpers.createAuthenticateRequest options
        let completion = ClientHelpers.createCompletionSource<Result<AuthenticateResponse list, RspInfo>> ()
        pending.Register(requestId, "Authenticate", completion)
        let result = api.ReqAuthenticate(request, requestId)

        if result <> 0 then
            logger.LogError("Authenticate request failed with native return code {ReturnCode}", result)
            pending.TryRemove requestId
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    member _.SettlementInfoConfirmAsync() =
        logger.LogDebug("Sending settlement info confirm request")
        let requestId = nextRequestId ()
        let request = OptionHelpers.createSettlementInfoConfirmRequest options
        let completion = ClientHelpers.createCompletionSource<Result<SettlementInfoConfirm list, RspInfo>> ()
        pending.Register(requestId, "SettlementInfoConfirm", completion)
        let result = api.ReqSettlementInfoConfirm(request, requestId)

        if result <> 0 then
            logger.LogError("Settlement info confirm request failed with native return code {ReturnCode}", result)
            pending.TryRemove requestId
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    member _.LoginAsync() =
        logger.LogDebug("Sending login request")
        let requestId = nextRequestId ()
        let request = OptionHelpers.createUserLoginRequest options
        let completion = ClientHelpers.createCompletionSource<Result<UserLoginResponse list, RspInfo>> ()
        pending.Register(requestId, "Login", completion)
        let result = api.ReqUserLogin(request, requestId)

        if result <> 0 then
            logger.LogError("Login request failed with native return code {ReturnCode}", result)
            pending.TryRemove requestId
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    member _.LogoutAsync() =
        logger.LogDebug("Sending logout request")
        let requestId = nextRequestId ()
        let request = OptionHelpers.createUserLogoutRequest options
        let completion = ClientHelpers.createCompletionSource<Result<UserLogoutResponse list, RspInfo>> ()
        pending.Register(requestId, "Logout", completion)
        let result = api.ReqUserLogout(request, requestId)

        if result <> 0 then
            logger.LogError("Logout request failed with native return code {ReturnCode}", result)
            pending.TryRemove requestId
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore
        else
            // Current CTP SDK does not reliably invoke OnRspUserLogout, so a successful request is treated as completion.
            pending.TryAccumulate(
                requestId,
                Some { UserLogoutResponse.BrokerId = request.BrokerId; UserId = request.UserId },
                None,
                true
            )

        completion.Task |> ClientHelpers.awaitTask

    member private _.QueryAsync<'TItem, 'TRequest>
        (queryName: string)
        (request: 'TRequest)
        (apiCall: 'TRequest * int -> int)
        : Async<Result<'TItem list, RspInfo>>
        =
        logger.LogDebug("Sending {QueryName} request", queryName)
        let requestId = nextRequestId ()
        let completion = ClientHelpers.createCompletionSource<Result<'TItem list, RspInfo>> ()
        pending.Register(requestId, queryName, completion)
        let errCode = apiCall (request, requestId)

        if errCode <> 0 then
            logger.LogError("{QueryName} request failed with native return code {ReturnCode}", queryName, errCode)
            pending.TryRemove requestId
            completion.TrySetResult(Error(ClientHelpers.apiReturnError errCode)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    member this.QueryTradingAccountAsync(?currencyId: string, ?bizType: char, ?accountId: string) =
        let request =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              CurrencyId = currencyId
              BizType = bizType
              AccountId = accountId }

        this.QueryAsync<TradingAccount, QueryTradingAccountRequest>
            (nameof QueryTradingAccountRequest)
            request
            api.ReqQryTradingAccount

    member this.QueryInvestorPositionAsync(?exchangeId: string, ?investUnitId: string, ?instrumentId: string) =
        let request: QueryInvestorPositionRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              ExchangeId = exchangeId
              InvestUnitId = investUnitId
              InstrumentId = instrumentId }

        this.QueryAsync<InvestorPosition, QueryInvestorPositionRequest>
            (nameof QueryInvestorPositionRequest)
            request
            api.ReqQryInvestorPosition

    member this.QueryInstrumentMarginRateAsync
        (hedgeFlag: char, instrumentId: string, ?exchangeId: string, ?investUnitId: string)
        =
        let request: QueryInstrumentMarginRateRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              HedgeFlag = hedgeFlag
              ExchangeId = exchangeId
              InvestUnitId = investUnitId
              InstrumentId = instrumentId }

        this.QueryAsync<InstrumentMarginRate, QueryInstrumentMarginRateRequest>
            (nameof QueryInstrumentMarginRateRequest)
            request
            api.ReqQryInstrumentMarginRate

    member this.QueryExchangeMarginRateAsync(hedgeFlag: char, instrumentId: string, ?exchangeId: string) =
        let request: QueryExchangeMarginRateRequest =
            { BrokerId = options.BrokerId
              HedgeFlag = hedgeFlag
              ExchangeId = exchangeId
              InstrumentId = instrumentId }

        this.QueryAsync<ExchangeMarginRate, QueryExchangeMarginRateRequest>
            (nameof QueryExchangeMarginRateRequest)
            request
            api.ReqQryExchangeMarginRate

    member this.QueryInstrumentCommissionRateAsync(instrumentId: string, ?exchangeId: string, ?investUnitId: string) =
        let request: QueryInstrumentCommissionRateRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              ExchangeId = exchangeId
              InvestUnitId = investUnitId
              InstrumentId = instrumentId }

        this.QueryAsync<InstrumentCommissionRate, QueryInstrumentCommissionRateRequest>
            (nameof QueryInstrumentCommissionRateRequest)
            request
            api.ReqQryInstrumentCommissionRate

    member _.InsertOrderAsync(request: InputOrderRequest) = async {
        logger.LogDebug("Inserting order: {InstrumentId}", request.InstrumentId)
        let requestId = nextRequestId ()
        let result = api.ReqOrderInsert(request, requestId)

        return
            if result = 0 then
                logger.LogDebug("Order insert accepted, requestId={RequestId}", requestId)
                Ok requestId
            else
                logger.LogError("Order insert failed with native return code {ReturnCode}", result)
                Error(ClientHelpers.apiReturnError result)
    }

    member _.CancelOrderAsync(request: InputOrderActionRequest) = async {
        logger.LogDebug("Cancelling order")
        let requestId = nextRequestId ()
        let result = api.ReqOrderAction(request, requestId)

        return
            if result = 0 then
                logger.LogDebug("Order cancel accepted, requestId={RequestId}", requestId)
                Ok requestId
            else
                logger.LogError("Order cancel failed with native return code {ReturnCode}", result)
                Error(ClientHelpers.apiReturnError result)
    }

    interface IDisposable with
        member _.Dispose() = (api :> IDisposable).Dispose()
