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
    | GenericNotification of obj

type private TraderAgentMessage =
    | SystemEvent of TraderSystemEvent
    | CorrelatedError of RspInfo option * int * bool
    | CorrelatedResponse of PendingResponseCompletionPolicy * objnull option * RspInfo option * int * bool
    | OrderCommandResponse of string * RspInfo option * int * bool
    | PushNotification of TraderPushNotification
    | AsyncErrorPush of obj * RspInfo option

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
    let notificationEvent = Event<obj>()
    let asyncErrorEvent = Event<obj>()
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
                | PushNotification(TraderPushNotification.GenericNotification item) -> notificationEvent.Trigger item
                | AsyncErrorPush(item, rspInfo) ->
                    asyncErrorEvent.Trigger(item, rspInfo)
                    rspInfo |> Option.iter (fun info -> rspErrorEvent.Trigger info)
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
                HeartBeatWarning = Some(fun lapse -> agent.Post(SystemEvent(TraderSystemEvent.HeartBeatWarning lapse)))
                RtnPrivateSeqNo = Some(fun seqNo -> agent.Post(SystemEvent(TraderSystemEvent.PrivateSeqNo seqNo)))
                RspError = Some(fun rsp requestId isLast -> agent.Post(CorrelatedError(rsp, requestId, isLast)))
                RspAuthenticate = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspSettlementInfoConfirm = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspUserLogin = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspUserLogout = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspQryTradingAccount = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryInvestorPosition = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryInstrumentMarginRate =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryExchangeMarginRate = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryInstrumentCommissionRate =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspOrderInsert = Some(postOrderCommandResponse "Order insert")
                RspOrderAction = Some(postOrderCommandResponse "Order action")
                RtnOrder = Some(fun order -> agent.Post(PushNotification(TraderPushNotification.OrderReceived order)))
                RtnTrade = Some(fun trade -> agent.Post(PushNotification(TraderPushNotification.TradeReceived trade)))
                RspQryAccountregister = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryBrokerTradingAlgos = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryBrokerTradingParams = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryCfmmcTradingAccountKey =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryClassifiedInstrument =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryCombAction = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryCombInstrumentGuard = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryCombLeg = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryCombPromotionParam = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryContractBank = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryDepthMarketData = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryEWarrantOffset = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryExchange = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryExchangeMarginRateAdjust =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryExchangeRate = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryExecOrder = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryForQuote = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryHedgeCfm = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryInstrument = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryInstrumentOrderCommRate =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryInvestUnit = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryInvestor = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryInvestorCommodityGroupSpmmMargin =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryInvestorCommoditySpmmMargin =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryInvestorInfoCommRec = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryInvestorPortfMarginRatio =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryInvestorPortfSetting =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryInvestorPositionCombineDetail =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryInvestorPositionDetail =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryInvestorProdRcamsMargin =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryInvestorProdRuleMargin =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryInvestorProdSpbmDetail =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryInvestorProductGroupMargin =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryMaxOrderVolume = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryMmInstrumentCommissionRate =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryMmOptionInstrCommRate =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryNotice = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryOffsetSetting = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryOptionInstrCommRate = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryOptionInstrTradeCost =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryOptionSelfClose = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryOrder = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryParkedOrder = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryParkedOrderAction = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryProduct = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryProductExchRate = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryProductGroup = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryQuote = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryRcamsCombProductInfo =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryRcamsInstrParameter = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryRcamsInterParameter = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryRcamsIntraParameter = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryRcamsInvestorCombPosition =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryRcamsShortOptAdjustParam =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryRiskSettleInvstPosition =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryRiskSettleProductStatus =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryRuleInstrParameter = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryRuleInterParameter = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryRuleIntraParameter = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQrySecAgentAcIdMap = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQrySecAgentCheckMode = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQrySecAgentTradeInfo = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQrySecAgentTradingAccount =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQrySettlementInfo = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQrySettlementInfoConfirm =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQrySpbmAddOnInterParameter =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQrySpbmFutureParameter = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQrySpbmInterParameter = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQrySpbmIntraParameter = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQrySpbmInvestorPortfDef =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQrySpbmOptionParameter = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQrySpbmPortfDefinition = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQrySpdApply = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQrySpmmInstParam = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQrySpmmProductParam = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryTrade = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryTraderOffer = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryTradingCode = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryTradingNotice = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryTransferBank = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryTransferSerial = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspQryUserSession = Some(postCorrelatedResponse PendingResponseCompletionPolicy.StreamUntilLast)
                RspGenSmsCode = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspGenUserCaptcha = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspGenUserText = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspQueryCfmmcTradingAccountToken =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspTradingAccountPasswordUpdate = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspUserAuthMethod = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspUserPasswordUpdate = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspBatchOrderAction = Some(postOrderCommandResponse "BatchOrderAction")
                RspCancelOffsetSetting = Some(postOrderCommandResponse "CancelOffsetSetting")
                RspCombActionInsert = Some(postOrderCommandResponse "CombActionInsert")
                RspExecOrderAction = Some(postOrderCommandResponse "ExecOrderAction")
                RspExecOrderInsert = Some(postOrderCommandResponse "ExecOrderInsert")
                RspForQuoteInsert = Some(postOrderCommandResponse "ForQuoteInsert")
                RspFromBankToFutureByFuture = Some(postOrderCommandResponse "FromBankToFutureByFuture")
                RspFromFutureToBankByFuture = Some(postOrderCommandResponse "FromFutureToBankByFuture")
                RspHedgeCfm = Some(postOrderCommandResponse "HedgeCfm")
                RspHedgeCfmAction = Some(postOrderCommandResponse "HedgeCfmAction")
                RspOffsetSetting = Some(postOrderCommandResponse "OffsetSetting")
                RspOptionSelfCloseAction = Some(postOrderCommandResponse "OptionSelfCloseAction")
                RspOptionSelfCloseInsert = Some(postOrderCommandResponse "OptionSelfCloseInsert")
                RspParkedOrderAction = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspParkedOrderInsert = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspQueryBankAccountMoneyByFuture = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspQuoteAction = Some(postOrderCommandResponse "QuoteAction")
                RspQuoteInsert = Some(postOrderCommandResponse "QuoteInsert")
                RspRemoveParkedOrder = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspRemoveParkedOrderAction = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspSpdApply = Some(postOrderCommandResponse "SpdApply")
                RspSpdApplyAction = Some(postOrderCommandResponse "SpdApplyAction")
                RtnCombAction =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.GenericNotification(box item))))
                RtnExecOrder =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.GenericNotification(box item))))
                RtnForQuoteRsp =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.GenericNotification(box item))))
                RtnFromBankToFutureByFuture =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.GenericNotification(box item))))
                RtnFromFutureToBankByFuture =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.GenericNotification(box item))))
                RtnHedgeCfm =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.GenericNotification(box item))))
                RtnOffsetSetting =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.GenericNotification(box item))))
                RtnOptionSelfClose =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.GenericNotification(box item))))
                RtnQueryBankBalanceByFuture =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.GenericNotification(box item))))
                RtnQuote =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.GenericNotification(box item))))
                RtnSpdApply =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.GenericNotification(box item))))
                ErrRtnBankToFutureByFuture = Some(fun item rsp -> agent.Post(AsyncErrorPush(box item, rsp)))
                ErrRtnBatchOrderAction = Some(fun item rsp -> agent.Post(AsyncErrorPush(box item, rsp)))
                ErrRtnCancelOffsetSetting = Some(fun item rsp -> agent.Post(AsyncErrorPush(box item, rsp)))
                ErrRtnCombActionInsert = Some(fun item rsp -> agent.Post(AsyncErrorPush(box item, rsp)))
                ErrRtnExecOrderAction = Some(fun item rsp -> agent.Post(AsyncErrorPush(box item, rsp)))
                ErrRtnExecOrderInsert = Some(fun item rsp -> agent.Post(AsyncErrorPush(box item, rsp)))
                ErrRtnForQuoteInsert = Some(fun item rsp -> agent.Post(AsyncErrorPush(box item, rsp)))
                ErrRtnFutureToBankByFuture = Some(fun item rsp -> agent.Post(AsyncErrorPush(box item, rsp)))
                ErrRtnHedgeCfm = Some(fun item rsp -> agent.Post(AsyncErrorPush(box item, rsp)))
                ErrRtnHedgeCfmAction = Some(fun item rsp -> agent.Post(AsyncErrorPush(box item, rsp)))
                ErrRtnOffsetSetting = Some(fun item rsp -> agent.Post(AsyncErrorPush(box item, rsp)))
                ErrRtnOptionSelfCloseAction = Some(fun item rsp -> agent.Post(AsyncErrorPush(box item, rsp)))
                ErrRtnOptionSelfCloseInsert = Some(fun item rsp -> agent.Post(AsyncErrorPush(box item, rsp)))
                ErrRtnOrderAction = Some(fun item rsp -> agent.Post(AsyncErrorPush(box item, rsp)))
                ErrRtnOrderInsert = Some(fun item rsp -> agent.Post(AsyncErrorPush(box item, rsp)))
                ErrRtnQueryBankBalanceByFuture = Some(fun item rsp -> agent.Post(AsyncErrorPush(box item, rsp)))
                ErrRtnQuoteAction = Some(fun item rsp -> agent.Post(AsyncErrorPush(box item, rsp)))
                ErrRtnQuoteInsert = Some(fun item rsp -> agent.Post(AsyncErrorPush(box item, rsp)))
                ErrRtnSpdApply = Some(fun item rsp -> agent.Post(AsyncErrorPush(box item, rsp)))
                ErrRtnSpdApplyAction = Some(fun item rsp -> agent.Post(AsyncErrorPush(box item, rsp))) }

    interface IDisposable with
        member _.Dispose() = (api :> IDisposable).Dispose()

    member _.FrontConnected = frontConnectedEvent.Publish
    member _.FrontDisconnected = frontDisconnectedEvent.Publish
    member _.HeartBeatWarning = heartBeatWarningEvent.Publish
    member _.PrivateSeqNoReceived = privateSeqNoEvent.Publish
    member _.RspError = rspErrorEvent.Publish
    member _.OrderReceived = orderEvent.Publish
    member _.TradeReceived = tradeEvent.Publish
    member _.NotificationReceived = notificationEvent.Publish
    member _.AsyncErrorReceived = asyncErrorEvent.Publish

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

    member this.QueryTradingAccountAsync(currencyId: string, ?bizType: char, ?accountId: string) =
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

    member this.QueryInvestorPositionAsync(instrumentId: string, ?exchangeId: string, ?investUnitId: string) =
        let request: QueryInvestorPositionRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              ExchangeId = exchangeId
              InvestUnitId = investUnitId
              InstrumentId = instrumentId
              Reserve1 = None }

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
              InstrumentId = instrumentId
              Reserve1 = None }

        this.QueryAsync<InstrumentMarginRate, QueryInstrumentMarginRateRequest>
            (nameof QueryInstrumentMarginRateRequest)
            request
            api.ReqQryInstrumentMarginRate

    member this.QueryExchangeMarginRateAsync(hedgeFlag: char, instrumentId: string, ?exchangeId: string) =
        let request: QueryExchangeMarginRateRequest =
            { BrokerId = options.BrokerId
              HedgeFlag = hedgeFlag
              ExchangeId = exchangeId
              InstrumentId = instrumentId
              Reserve1 = None }

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
              InstrumentId = instrumentId
              Reserve1 = None }

        this.QueryAsync<InstrumentCommissionRate, QueryInstrumentCommissionRateRequest>
            (nameof QueryInstrumentCommissionRateRequest)
            request
            api.ReqQryInstrumentCommissionRate

    // ---- Utility / diagnostic methods ----

    member _.GetApiVersion() = TraderApi.GetApiVersion()

    member _.GetTradingDay() = api.GetTradingDay()

    member _.GetFrontInfo() = api.GetFrontInfo()

    // ---- Connection configuration (call before Connect) ----

    member _.RegisterNameServer(nsAddress: string) = api.RegisterNameServer(nsAddress)

    member _.RegisterFensUserInfo(request: FensUserInfo) = api.RegisterFensUserInfo(request)

    // ---- Regulatory / system-info methods ----

    member _.RegisterUserSystemInfo(info: UserSystemInfo) = api.RegisterUserSystemInfo(info)

    member _.SubmitUserSystemInfo(info: UserSystemInfo) = api.SubmitUserSystemInfo(info)

    member _.RegisterWechatUserSystemInfo(info: WechatUserSystemInfo) = api.RegisterWechatUserSystemInfo(info)

    member _.SubmitWechatUserSystemInfo(info: WechatUserSystemInfo) = api.SubmitWechatUserSystemInfo(info)

    // ---- Auth / password / captcha methods ----

    member _.ReqUserPasswordUpdateAsync(oldPassword: string, newPassword: string) =
        logger.LogDebug("Sending user password update request")
        let requestId = nextRequestId ()
        let request: UserPasswordUpdate =
            { BrokerId = options.BrokerId; UserId = options.UserId
              OldPassword = oldPassword; NewPassword = newPassword }
        let completion = ClientHelpers.createCompletionSource<Result<UserPasswordUpdate list, RspInfo>> ()
        pending.Register(requestId, "UserPasswordUpdate", completion)
        let result = api.ReqUserPasswordUpdate(request, requestId)

        if result <> 0 then
            logger.LogError("User password update request failed with native return code {ReturnCode}", result)
            pending.TryRemove requestId
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    member _.ReqTradingAccountPasswordUpdateAsync
        (accountId: string, oldPassword: string, newPassword: string, currencyId: string)
        =
        logger.LogDebug("Sending trading account password update request")
        let requestId = nextRequestId ()
        let request: TradingAccountPasswordUpdate =
            { BrokerId = options.BrokerId; AccountId = accountId
              OldPassword = oldPassword; NewPassword = newPassword
              CurrencyId = currencyId }
        let completion = ClientHelpers.createCompletionSource<Result<TradingAccountPasswordUpdate list, RspInfo>> ()
        pending.Register(requestId, "TradingAccountPasswordUpdate", completion)
        let result = api.ReqTradingAccountPasswordUpdate(request, requestId)

        if result <> 0 then
            logger.LogError("Trading account password update request failed with native return code {ReturnCode}", result)
            pending.TryRemove requestId
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    member _.ReqUserAuthMethodAsync(?tradingDay: string) =
        logger.LogDebug("Sending user auth method request")
        let requestId = nextRequestId ()
        let request: UserAuthMethodRequest =
            { BrokerId = Some options.BrokerId; UserId = Some options.UserId; TradingDay = tradingDay }
        let completion = ClientHelpers.createCompletionSource<Result<RspUserAuthMethod list, RspInfo>> ()
        pending.Register(requestId, "UserAuthMethod", completion)
        let result = api.ReqUserAuthMethod(request, requestId)

        if result <> 0 then
            logger.LogError("User auth method request failed with native return code {ReturnCode}", result)
            pending.TryRemove requestId
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    member _.ReqGenUserCaptchaAsync(?tradingDay: string) =
        logger.LogDebug("Sending gen user captcha request")
        let requestId = nextRequestId ()
        let request: GenUserCaptchaRequest =
            { BrokerId = Some options.BrokerId; UserId = Some options.UserId; TradingDay = tradingDay }
        let completion = ClientHelpers.createCompletionSource<Result<RspGenUserCaptcha list, RspInfo>> ()
        pending.Register(requestId, "GenUserCaptcha", completion)
        let result = api.ReqGenUserCaptcha(request, requestId)

        if result <> 0 then
            logger.LogError("Gen user captcha request failed with native return code {ReturnCode}", result)
            pending.TryRemove requestId
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    member _.ReqGenUserTextAsync(?tradingDay: string) =
        logger.LogDebug("Sending gen user text request")
        let requestId = nextRequestId ()
        let request: GenUserTextRequest =
            { BrokerId = Some options.BrokerId; UserId = Some options.UserId; TradingDay = tradingDay }
        let completion = ClientHelpers.createCompletionSource<Result<RspGenUserText list, RspInfo>> ()
        pending.Register(requestId, "GenUserText", completion)
        let result = api.ReqGenUserText(request, requestId)

        if result <> 0 then
            logger.LogError("Gen user text request failed with native return code {ReturnCode}", result)
            pending.TryRemove requestId
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    member _.ReqUserLoginWithCaptchaAsync
        (captcha: string, ?tradingDay: string, ?userProductInfo: string, ?interfaceProductInfo: string,
         ?protocolInfo: string, ?macAddress: string, ?loginRemark: string, ?clientIpPort: int, ?clientIpAddress: string)
        =
        logger.LogDebug("Sending user login with captcha request")
        let requestId = nextRequestId ()
        let request: UserLoginWithCaptchaRequest =
            { TradingDay = tradingDay; BrokerId = options.BrokerId; UserId = options.UserId
              Password = options.Password; UserProductInfo = userProductInfo
              InterfaceProductInfo = interfaceProductInfo; ProtocolInfo = protocolInfo
              MacAddress = macAddress; Reserve1 = None; LoginRemark = loginRemark
              Captcha = captcha; ClientIpPort = clientIpPort; ClientIpAddress = clientIpAddress }
        let completion = ClientHelpers.createCompletionSource<Result<UserLoginResponse list, RspInfo>> ()
        pending.Register(requestId, "LoginWithCaptcha", completion)
        let result = api.ReqUserLoginWithCaptcha(request, requestId)

        if result <> 0 then
            logger.LogError("User login with captcha request failed with native return code {ReturnCode}", result)
            pending.TryRemove requestId
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    member _.ReqUserLoginWithTextAsync
        (text: string, ?tradingDay: string, ?userProductInfo: string, ?interfaceProductInfo: string,
         ?protocolInfo: string, ?macAddress: string, ?loginRemark: string, ?clientIpPort: int, ?clientIpAddress: string)
        =
        logger.LogDebug("Sending user login with text request")
        let requestId = nextRequestId ()
        let request: UserLoginWithTextRequest =
            { TradingDay = tradingDay; BrokerId = options.BrokerId; UserId = options.UserId
              Password = options.Password; UserProductInfo = userProductInfo
              InterfaceProductInfo = interfaceProductInfo; ProtocolInfo = protocolInfo
              MacAddress = macAddress; Reserve1 = None; LoginRemark = loginRemark
              Text = text; ClientIpPort = clientIpPort; ClientIpAddress = clientIpAddress }
        let completion = ClientHelpers.createCompletionSource<Result<UserLoginResponse list, RspInfo>> ()
        pending.Register(requestId, "LoginWithText", completion)
        let result = api.ReqUserLoginWithText(request, requestId)

        if result <> 0 then
            logger.LogError("User login with text request failed with native return code {ReturnCode}", result)
            pending.TryRemove requestId
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    member _.ReqUserLoginWithOtpAsync
        (otpPassword: string, ?tradingDay: string, ?userProductInfo: string, ?interfaceProductInfo: string,
         ?protocolInfo: string, ?macAddress: string, ?loginRemark: string, ?clientIpPort: int, ?clientIpAddress: string)
        =
        logger.LogDebug("Sending user login with OTP request")
        let requestId = nextRequestId ()
        let request: UserLoginWithOtpRequest =
            { TradingDay = tradingDay; BrokerId = options.BrokerId; UserId = options.UserId
              Password = options.Password; UserProductInfo = userProductInfo
              InterfaceProductInfo = interfaceProductInfo; ProtocolInfo = protocolInfo
              MacAddress = macAddress; Reserve1 = None; LoginRemark = loginRemark
              OtpPassword = otpPassword; ClientIpPort = clientIpPort; ClientIpAddress = clientIpAddress }
        let completion = ClientHelpers.createCompletionSource<Result<UserLoginResponse list, RspInfo>> ()
        pending.Register(requestId, "LoginWithOtp", completion)
        let result = api.ReqUserLoginWithOtp(request, requestId)

        if result <> 0 then
            logger.LogError("User login with OTP request failed with native return code {ReturnCode}", result)
            pending.TryRemove requestId
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    member _.ReqGenSmsCodeAsync(mobile: string) =
        logger.LogDebug("Sending gen SMS code request")
        let requestId = nextRequestId ()
        let request: GenSmsCodeRequest =
            { BrokerId = options.BrokerId; UserId = options.UserId; Mobile = mobile }
        let completion = ClientHelpers.createCompletionSource<Result<RspGenSmsCode list, RspInfo>> ()
        pending.Register(requestId, "GenSmsCode", completion)
        let result = api.ReqGenSmsCode(request, requestId)

        if result <> 0 then
            logger.LogError("Gen SMS code request failed with native return code {ReturnCode}", result)
            pending.TryRemove requestId
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    // ---- Order insertion and action ----

    member _.InsertOrderAsync(request: InputOrderRequest) =
        let requestId = nextRequestId ()
        let result = api.ReqOrderInsert(request, requestId)

        if result <> 0 then
            logger.LogError("Order insert request failed with native return code {ReturnCode}", result)

        async.Return requestId

    member _.CancelOrderAsync(request: InputOrderActionRequest) =
        let requestId = nextRequestId ()
        let result = api.ReqOrderAction(request, requestId)

        if result <> 0 then
            logger.LogError("Order action request failed with native return code {ReturnCode}", result)

        async.Return requestId

    // ---- Execution / quote / hedge / combination command methods ----

    member _.ReqExecOrderInsertAsync(request: InputExecOrderRequest) =
        let requestId = nextRequestId ()
        let result = api.ReqExecOrderInsert(request, requestId)

        if result <> 0 then
            logger.LogError("Exec order insert request failed with native return code {ReturnCode}", result)

        async.Return requestId

    member _.ReqExecOrderActionAsync(request: InputExecOrderActionRequest) =
        let requestId = nextRequestId ()
        let result = api.ReqExecOrderAction(request, requestId)

        if result <> 0 then
            logger.LogError("Exec order action request failed with native return code {ReturnCode}", result)

        async.Return requestId

    member _.ReqForQuoteInsertAsync(instrumentId: string, ?exchangeId: string) =
        let requestId = nextRequestId ()
        let request: InputForQuoteRequest =
            { BrokerId = options.BrokerId; InvestorId = options.UserId
              Reserve1 = None; ForQuoteRef = None; UserId = None
              ExchangeId = exchangeId; InvestUnitId = None
              Reserve2 = None; MacAddress = None
              InstrumentId = instrumentId; IpAddress = None }
        let result = api.ReqForQuoteInsert(request, requestId)

        if result <> 0 then
            logger.LogError("For quote insert request failed with native return code {ReturnCode}", result)

        async.Return requestId

    member _.ReqQuoteInsertAsync(request: InputQuoteRequest) =
        let requestId = nextRequestId ()
        let result = api.ReqQuoteInsert(request, requestId)

        if result <> 0 then
            logger.LogError("Quote insert request failed with native return code {ReturnCode}", result)

        async.Return requestId

    member _.ReqQuoteActionAsync(request: InputQuoteActionRequest) =
        let requestId = nextRequestId ()
        let result = api.ReqQuoteAction(request, requestId)

        if result <> 0 then
            logger.LogError("Quote action request failed with native return code {ReturnCode}", result)

        async.Return requestId

    member _.ReqBatchOrderActionAsync(frontId: int, sessionId: int, ?exchangeId: string) =
        let requestId = nextRequestId ()
        let request: InputBatchOrderActionRequest =
            { BrokerId = Some options.BrokerId; InvestorId = options.UserId
              OrderActionRef = 0; RequestId = 0; FrontId = frontId
              SessionId = sessionId; ExchangeId = exchangeId
              UserId = None; InvestUnitId = None; Reserve1 = None
              MacAddress = None; IpAddress = None }
        let result = api.ReqBatchOrderAction(request, requestId)

        if result <> 0 then
            logger.LogError("Batch order action request failed with native return code {ReturnCode}", result)

        async.Return requestId

    member _.ReqOptionSelfCloseInsertAsync(request: InputOptionSelfCloseRequest) =
        let requestId = nextRequestId ()
        let result = api.ReqOptionSelfCloseInsert(request, requestId)

        if result <> 0 then
            logger.LogError("Option self close insert request failed with native return code {ReturnCode}", result)

        async.Return requestId

    member _.ReqOptionSelfCloseActionAsync(request: InputOptionSelfCloseActionRequest) =
        let requestId = nextRequestId ()
        let result = api.ReqOptionSelfCloseAction(request, requestId)

        if result <> 0 then
            logger.LogError("Option self close action request failed with native return code {ReturnCode}", result)

        async.Return requestId

    member _.ReqCombActionInsertAsync(request: InputCombActionRequest) =
        let requestId = nextRequestId ()
        let result = api.ReqCombActionInsert(request, requestId)

        if result <> 0 then
            logger.LogError("Comb action insert request failed with native return code {ReturnCode}", result)

        async.Return requestId

    member _.ReqOffsetSettingAsync(request: InputOffsetSettingRequest) =
        let requestId = nextRequestId ()
        let result = api.ReqOffsetSetting(request, requestId)

        if result <> 0 then
            logger.LogError("Offset setting request failed with native return code {ReturnCode}", result)

        async.Return requestId

    member _.ReqCancelOffsetSettingAsync(request: InputOffsetSettingRequest) =
        let requestId = nextRequestId ()
        let result = api.ReqCancelOffsetSetting(request, requestId)

        if result <> 0 then
            logger.LogError("Cancel offset setting request failed with native return code {ReturnCode}", result)

        async.Return requestId

    member _.ReqSpdApplyAsync(request: InputSpdApplyRequest) =
        let requestId = nextRequestId ()
        let result = api.ReqSpdApply(request, requestId)

        if result <> 0 then
            logger.LogError("Spd apply request failed with native return code {ReturnCode}", result)

        async.Return requestId

    member _.ReqSpdApplyActionAsync(request: InputSpdApplyActionRequest) =
        let requestId = nextRequestId ()
        let result = api.ReqSpdApplyAction(request, requestId)

        if result <> 0 then
            logger.LogError("Spd apply action request failed with native return code {ReturnCode}", result)

        async.Return requestId

    member _.ReqHedgeCfmAsync(request: InputHedgeCfmRequest) =
        let requestId = nextRequestId ()
        let result = api.ReqHedgeCfm(request, requestId)

        if result <> 0 then
            logger.LogError("Hedge cfm request failed with native return code {ReturnCode}", result)

        async.Return requestId

    member _.ReqHedgeCfmActionAsync(request: InputHedgeCfmActionRequest) =
        let requestId = nextRequestId ()
        let result = api.ReqHedgeCfmAction(request, requestId)

        if result <> 0 then
            logger.LogError("Hedge cfm action request failed with native return code {ReturnCode}", result)

        async.Return requestId

    // ---- Parked order methods (FinalOnly) ----

    member _.ReqParkedOrderInsertAsync(request: ParkedOrder) =
        logger.LogDebug("Sending parked order insert request")
        let requestId = nextRequestId ()
        let completion = ClientHelpers.createCompletionSource<Result<ParkedOrder list, RspInfo>> ()
        pending.Register(requestId, "ParkedOrderInsert", completion)
        let result = api.ReqParkedOrderInsert(request, requestId)

        if result <> 0 then
            logger.LogError("Parked order insert request failed with native return code {ReturnCode}", result)
            pending.TryRemove requestId
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    member _.ReqParkedOrderActionAsync(request: ParkedOrderAction) =
        logger.LogDebug("Sending parked order action request")
        let requestId = nextRequestId ()
        let completion = ClientHelpers.createCompletionSource<Result<ParkedOrderAction list, RspInfo>> ()
        pending.Register(requestId, "ParkedOrderAction", completion)
        let result = api.ReqParkedOrderAction(request, requestId)

        if result <> 0 then
            logger.LogError("Parked order action request failed with native return code {ReturnCode}", result)
            pending.TryRemove requestId
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    member _.ReqRemoveParkedOrderAsync(parkedOrderId: string, ?investUnitId: string) =
        logger.LogDebug("Sending remove parked order request")
        let requestId = nextRequestId ()
        let request: RemoveParkedOrder =
            { BrokerId = options.BrokerId; InvestorId = options.UserId
              ParkedOrderId = parkedOrderId; InvestUnitId = investUnitId }
        let completion = ClientHelpers.createCompletionSource<Result<RemoveParkedOrder list, RspInfo>> ()
        pending.Register(requestId, "RemoveParkedOrder", completion)
        let result = api.ReqRemoveParkedOrder(request, requestId)

        if result <> 0 then
            logger.LogError("Remove parked order request failed with native return code {ReturnCode}", result)
            pending.TryRemove requestId
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    member _.ReqRemoveParkedOrderActionAsync(parkedOrderActionId: string, ?investUnitId: string) =
        logger.LogDebug("Sending remove parked order action request")
        let requestId = nextRequestId ()
        let request: RemoveParkedOrderAction =
            { BrokerId = options.BrokerId; InvestorId = options.UserId
              ParkedOrderActionId = parkedOrderActionId; InvestUnitId = investUnitId }
        let completion = ClientHelpers.createCompletionSource<Result<RemoveParkedOrderAction list, RspInfo>> ()
        pending.Register(requestId, "RemoveParkedOrderAction", completion)
        let result = api.ReqRemoveParkedOrderAction(request, requestId)

        if result <> 0 then
            logger.LogError("Remove parked order action request failed with native return code {ReturnCode}", result)
            pending.TryRemove requestId
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    // ---- Bank transfer methods ----

    member _.FromBankToFutureByFutureAsync(request: TransferRequest) =
        let requestId = nextRequestId ()
        let result = api.ReqFromBankToFutureByFuture(request, requestId)

        if result <> 0 then
            logger.LogError("From bank to future by future request failed with native return code {ReturnCode}", result)

        async.Return requestId

    member _.FromFutureToBankByFutureAsync(request: TransferRequest) =
        let requestId = nextRequestId ()
        let result = api.ReqFromFutureToBankByFuture(request, requestId)

        if result <> 0 then
            logger.LogError("From future to bank by future request failed with native return code {ReturnCode}", result)

        async.Return requestId

    member _.QueryBankAccountMoneyByFutureAsync(request: ReqQueryAccount) =
        logger.LogDebug("Sending query bank account money by future request")
        let requestId = nextRequestId ()
        let completion = ClientHelpers.createCompletionSource<Result<ReqQueryAccount list, RspInfo>> ()
        pending.Register(requestId, "QueryBankAccountMoneyByFuture", completion)
        let result = api.ReqQueryBankAccountMoneyByFuture(request, requestId)

        if result <> 0 then
            logger.LogError("Query bank account money by future request failed with native return code {ReturnCode}", result)
            pending.TryRemove requestId
            completion.TrySetResult(Error(ClientHelpers.apiReturnError result)) |> ignore

        completion.Task |> ClientHelpers.awaitTask

    // ---- Query methods ----

    member this.QueryMaxOrderVolumeAsync
        (direction: char, offsetFlag: char, hedgeFlag: char, instrumentId: string,
         ?maxVolume: int, ?exchangeId: string, ?investUnitId: string)
        =
        let request: QryMaxOrderVolumeRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              Direction = direction
              OffsetFlag = offsetFlag
              HedgeFlag = hedgeFlag
              MaxVolume = maxVolume
              ExchangeId = exchangeId
              InvestUnitId = investUnitId
              InstrumentId = instrumentId }

        this.QueryAsync<QryMaxOrderVolumeRequest, QryMaxOrderVolumeRequest>
            (nameof QryMaxOrderVolumeRequest)
            request
            api.ReqQryMaxOrderVolume

    member this.QueryOrderAsync
        (exchangeId: string, orderSysId: string, insertTimeStart: string, insertTimeEnd: string,
         instrumentId: string, ?investUnitId: string)
        =
        let request: QryOrderRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              ExchangeId = exchangeId
              OrderSysId = orderSysId
              InsertTimeStart = insertTimeStart
              InsertTimeEnd = insertTimeEnd
              InvestUnitId = investUnitId
              InstrumentId = instrumentId }

        this.QueryAsync<OrderUpdate, QryOrderRequest>(nameof QryOrderRequest) request api.ReqQryOrder

    member this.QueryTradeAsync
        (?exchangeId: string, ?tradeId: string, ?tradeTimeStart: string, ?tradeTimeEnd: string,
         ?investUnitId: string, ?instrumentId: string)
        =
        let request: QryTradeRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              ExchangeId = exchangeId
              TradeId = tradeId
              TradeTimeStart = tradeTimeStart
              TradeTimeEnd = tradeTimeEnd
              InvestUnitId = investUnitId
              InstrumentId = instrumentId }

        this.QueryAsync<TradeUpdate, QryTradeRequest>(nameof QryTradeRequest) request api.ReqQryTrade

    member this.QueryInvestorAsync() =
        let request: QryInvestorRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId }

        this.QueryAsync<Investor, QryInvestorRequest>(nameof QryInvestorRequest) request api.ReqQryInvestor

    member this.QueryTradingCodeAsync
        (exchangeId: string, clientId: string, clientIdType: char, ?investUnitId: string)
        =
        let request: QryTradingCodeRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              ExchangeId = exchangeId
              ClientId = clientId
              ClientIdType = clientIdType
              InvestUnitId = investUnitId }

        this.QueryAsync<TradingCode, QryTradingCodeRequest>(nameof QryTradingCodeRequest) request api.ReqQryTradingCode

    member this.QueryUserSessionAsync(frontId: int, sessionId: int) =
        let request: QryUserSessionRequest =
            { FrontId = frontId
              SessionId = sessionId
              BrokerId = options.BrokerId
              UserId = options.UserId }

        this.QueryAsync<UserSession, QryUserSessionRequest>(nameof QryUserSessionRequest) request api.ReqQryUserSession

    member this.QueryExchangeAsync(exchangeId: string) =
        let request: QryExchangeRequest = { ExchangeId = exchangeId }
        this.QueryAsync<Exchange, QryExchangeRequest>(nameof QryExchangeRequest) request api.ReqQryExchange

    member this.QueryProductAsync(productId: string, ?productClass: char, ?exchangeId: string) =
        let request: QryProductRequest =
            { Reserve1 = None
              ProductClass = productClass
              ExchangeId = exchangeId
              ProductId = productId }

        this.QueryAsync<Product, QryProductRequest>(nameof QryProductRequest) request api.ReqQryProduct

    member this.QueryInstrumentAsync
        (exchangeId: string, instrumentId: string, exchangeInstId: string, productId: string)
        =
        let request: QryInstrumentRequest =
            { Reserve1 = None
              ExchangeId = exchangeId
              Reserve2 = None
              Reserve3 = None
              InstrumentId = instrumentId
              ExchangeInstId = exchangeInstId
              ProductId = productId }

        this.QueryAsync<Instrument, QryInstrumentRequest>(nameof QryInstrumentRequest) request api.ReqQryInstrument

    member this.QueryDepthMarketDataAsync(instrumentId: string, productClass: char, ?exchangeId: string) =
        let request: QryDepthMarketDataRequest =
            { Reserve1 = None
              ExchangeId = exchangeId
              InstrumentId = instrumentId
              ProductClass = productClass }

        this.QueryAsync<DepthMarketData, QryDepthMarketDataRequest>
            (nameof QryDepthMarketDataRequest)
            request
            api.ReqQryDepthMarketData

    member this.QueryTraderOfferAsync(?exchangeId: string, ?participantId: string, ?traderId: string) =
        let request: QryTraderOfferRequest =
            { ExchangeId = exchangeId
              ParticipantId = participantId
              TraderId = traderId }

        this.QueryAsync<TraderOffer, QryTraderOfferRequest>(nameof QryTraderOfferRequest) request api.ReqQryTraderOffer

    member this.QuerySettlementInfoAsync(tradingDay: string, ?accountId: string, ?currencyId: string) =
        let request: QrySettlementInfoRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              TradingDay = tradingDay
              AccountId = accountId
              CurrencyId = currencyId }

        this.QueryAsync<SettlementInfo, QrySettlementInfoRequest>(nameof QrySettlementInfoRequest) request api.ReqQrySettlementInfo

    member this.QueryTransferBankAsync(?bankId: string, ?bankBrchId: string) =
        let request: QryTransferBankRequest =
            { BankId = bankId
              BankBrchId = bankBrchId }

        this.QueryAsync<TransferBank, QryTransferBankRequest>(nameof QryTransferBankRequest) request api.ReqQryTransferBank

    member this.QueryInvestorPositionDetailAsync(instrumentId: string, ?exchangeId: string, ?investUnitId: string) =
        let request: QryInvestorPositionDetailRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              ExchangeId = exchangeId
              InvestUnitId = investUnitId
              InstrumentId = instrumentId }

        this.QueryAsync<InvestorPositionDetail, QryInvestorPositionDetailRequest>
            (nameof QryInvestorPositionDetailRequest)
            request
            api.ReqQryInvestorPositionDetail

    member this.QueryNoticeAsync() =
        let request: QryNoticeRequest = { BrokerId = options.BrokerId }
        this.QueryAsync<Notice, QryNoticeRequest>(nameof QryNoticeRequest) request api.ReqQryNotice

    member this.QuerySettlementInfoConfirmAsync(?accountId: string, ?currencyId: string) =
        let request: QrySettlementInfoConfirmRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              AccountId = accountId
              CurrencyId = currencyId }

        this.QueryAsync<SettlementInfoConfirm, QrySettlementInfoConfirmRequest>
            (nameof QrySettlementInfoConfirmRequest)
            request
            api.ReqQrySettlementInfoConfirm

    member this.QueryInvestorPositionCombineDetailAsync(combInstrumentId: string, ?exchangeId: string, ?investUnitId: string) =
        let request: QryInvestorPositionCombineDetailRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              ExchangeId = exchangeId
              InvestUnitId = investUnitId
              CombInstrumentId = combInstrumentId }

        this.QueryAsync<InvestorPositionCombineDetail, QryInvestorPositionCombineDetailRequest>
            (nameof QryInvestorPositionCombineDetailRequest)
            request
            api.ReqQryInvestorPositionCombineDetail

    member this.QueryCfmmcTradingAccountKeyAsync() =
        let request: QryCfmmcTradingAccountKeyRequest =
            { BrokerId = Some options.BrokerId
              InvestorId = Some options.UserId }

        this.QueryAsync<CfmmcTradingAccountKey, QryCfmmcTradingAccountKeyRequest>
            (nameof QryCfmmcTradingAccountKeyRequest)
            request
            api.ReqQryCfmmcTradingAccountKey

    member this.QueryEWarrantOffsetAsync(exchangeId: string, instrumentId: string, ?investUnitId: string) =
        let request: QryEWarrantOffsetRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              ExchangeId = exchangeId
              Reserve1 = None
              InvestUnitId = investUnitId
              InstrumentId = instrumentId }

        this.QueryAsync<EWarrantOffset, QryEWarrantOffsetRequest>(nameof QryEWarrantOffsetRequest) request api.ReqQryEWarrantOffset

    member this.QueryInvestorProductGroupMarginAsync
        (productGroupId: string, ?hedgeFlag: char, ?exchangeId: string, ?investUnitId: string)
        =
        let request: QryInvestorProductGroupMarginRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              HedgeFlag = hedgeFlag
              ExchangeId = exchangeId
              InvestUnitId = investUnitId
              ProductGroupId = productGroupId }

        this.QueryAsync<InvestorProductGroupMargin, QryInvestorProductGroupMarginRequest>
            (nameof QryInvestorProductGroupMarginRequest)
            request
            api.ReqQryInvestorProductGroupMargin

    member this.QueryExchangeMarginRateAdjustAsync(instrumentId: string, hedgeFlag: char) =
        let request: QryExchangeMarginRateAdjustRequest =
            { BrokerId = options.BrokerId
              Reserve1 = None
              HedgeFlag = hedgeFlag
              InstrumentId = instrumentId }

        this.QueryAsync<ExchangeMarginRateAdjust, QryExchangeMarginRateAdjustRequest>
            (nameof QryExchangeMarginRateAdjustRequest)
            request
            api.ReqQryExchangeMarginRateAdjust

    member this.QueryExchangeRateAsync(fromCurrencyId: string, toCurrencyId: string) =
        let request: QryExchangeRate =
            { BrokerId = options.BrokerId
              FromCurrencyId = fromCurrencyId
              ToCurrencyId = toCurrencyId }

        this.QueryAsync<ExchangeRate, QryExchangeRate>(nameof QryExchangeRate) request api.ReqQryExchangeRate

    member this.QuerySecAgentAcIdMapAsync(accountId: string, currencyId: string) =
        let request: QrySecAgentAcIdMapRequest =
            { BrokerId = options.BrokerId
              UserId = options.UserId
              AccountId = accountId
              CurrencyId = currencyId }

        this.QueryAsync<SecAgentAcIdMap, QrySecAgentAcIdMapRequest>
            (nameof QrySecAgentAcIdMapRequest)
            request
            api.ReqQrySecAgentAcIdMap

    member this.QueryProductExchRateAsync(productId: string, ?exchangeId: string) =
        let request: QryProductExchRateRequest =
            { Reserve1 = None
              ExchangeId = exchangeId
              ProductId = productId }

        this.QueryAsync<ProductExchRate, QryProductExchRateRequest>
            (nameof QryProductExchRateRequest)
            request
            api.ReqQryProductExchRate

    member this.QueryProductGroupAsync(exchangeId: string, productId: string) =
        let request: QryProductGroupRequest =
            { Reserve1 = None
              ExchangeId = exchangeId
              ProductId = productId }

        this.QueryAsync<ProductGroup, QryProductGroupRequest>(nameof QryProductGroupRequest) request api.ReqQryProductGroup

    member this.QueryMmInstrumentCommissionRateAsync(instrumentId: string) =
        let request: QryMmInstrumentCommissionRateRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              InstrumentId = instrumentId }

        this.QueryAsync<MmInstrumentCommissionRate, QryMmInstrumentCommissionRateRequest>
            (nameof QryMmInstrumentCommissionRateRequest)
            request
            api.ReqQryMmInstrumentCommissionRate

    member this.QueryMmOptionInstrCommRateAsync(instrumentId: string) =
        let request: QryMmOptionInstrCommRateRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              InstrumentId = instrumentId }

        this.QueryAsync<MmOptionInstrCommRate, QryMmOptionInstrCommRateRequest>
            (nameof QryMmOptionInstrCommRateRequest)
            request
            api.ReqQryMmOptionInstrCommRate

    member this.QueryInstrumentOrderCommRateAsync(instrumentId: string) =
        let request: QryInstrumentOrderCommRateRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              InstrumentId = instrumentId }

        this.QueryAsync<InstrumentOrderCommRate, QryInstrumentOrderCommRateRequest>
            (nameof QryInstrumentOrderCommRateRequest)
            request
            api.ReqQryInstrumentOrderCommRate

    member this.QuerySecAgentTradingAccountAsync(currencyId: string, ?bizType: char, ?accountId: string) =
        let request: QueryTradingAccountRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              CurrencyId = currencyId
              BizType = bizType
              AccountId = accountId }

        this.QueryAsync<TradingAccount, QueryTradingAccountRequest>
            (nameof QueryTradingAccountRequest)
            request
            api.ReqQrySecAgentTradingAccount

    member this.QuerySecAgentCheckModeAsync() =
        let request: QrySecAgentCheckModeRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId }

        this.QueryAsync<SecAgentCheckMode, QrySecAgentCheckModeRequest>
            (nameof QrySecAgentCheckModeRequest)
            request
            api.ReqQrySecAgentCheckMode

    member this.QuerySecAgentTradeInfoAsync(brokerSecAgentId: string) =
        let request: QrySecAgentTradeInfoRequest =
            { BrokerId = options.BrokerId
              BrokerSecAgentId = brokerSecAgentId }

        this.QueryAsync<SecAgentTradeInfo, QrySecAgentTradeInfoRequest>
            (nameof QrySecAgentTradeInfoRequest)
            request
            api.ReqQrySecAgentTradeInfo

    member this.QueryOptionInstrTradeCostAsync
        (instrumentId: string, hedgeFlag: char, inputPrice: decimal, underlyingPrice: decimal,
         ?exchangeId: string, ?investUnitId: string)
        =
        let request: QryOptionInstrTradeCostRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              HedgeFlag = hedgeFlag
              InputPrice = inputPrice
              UnderlyingPrice = underlyingPrice
              ExchangeId = exchangeId
              InvestUnitId = investUnitId
              InstrumentId = instrumentId }

        this.QueryAsync<OptionInstrTradeCost, QryOptionInstrTradeCostRequest>
            (nameof QryOptionInstrTradeCostRequest)
            request
            api.ReqQryOptionInstrTradeCost

    member this.QueryOptionInstrCommRateAsync(instrumentId: string, ?exchangeId: string, ?investUnitId: string) =
        let request: QryOptionInstrCommRateRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              ExchangeId = exchangeId
              InvestUnitId = investUnitId
              InstrumentId = instrumentId }

        this.QueryAsync<OptionInstrCommRate, QryOptionInstrCommRateRequest>
            (nameof QryOptionInstrCommRateRequest)
            request
            api.ReqQryOptionInstrCommRate

    member this.QueryExecOrderAsync
        (exchangeId: string, execOrderSysId: string, insertTimeStart: string, insertTimeEnd: string,
         instrumentId: string)
        =
        let request: QryExecOrderRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              ExchangeId = exchangeId
              ExecOrderSysId = execOrderSysId
              InsertTimeStart = insertTimeStart
              InsertTimeEnd = insertTimeEnd
              InstrumentId = instrumentId }

        this.QueryAsync<ExecOrder, QryExecOrderRequest>(nameof QryExecOrderRequest) request api.ReqQryExecOrder

    member this.QueryForQuoteAsync
        (exchangeId: string, insertTimeStart: string, insertTimeEnd: string, instrumentId: string,
         ?investUnitId: string)
        =
        let request: QryForQuoteRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              ExchangeId = exchangeId
              InsertTimeStart = insertTimeStart
              InsertTimeEnd = insertTimeEnd
              InvestUnitId = investUnitId
              InstrumentId = instrumentId }

        this.QueryAsync<ForQuote, QryForQuoteRequest>(nameof QryForQuoteRequest) request api.ReqQryForQuote

    member this.QueryQuoteAsync
        (exchangeId: string, quoteSysId: string, insertTimeStart: string, insertTimeEnd: string,
         instrumentId: string, ?investUnitId: string)
        =
        let request: QryQuoteRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              ExchangeId = exchangeId
              QuoteSysId = quoteSysId
              InsertTimeStart = insertTimeStart
              InsertTimeEnd = insertTimeEnd
              InvestUnitId = investUnitId
              InstrumentId = instrumentId }

        this.QueryAsync<Quote, QryQuoteRequest>(nameof QryQuoteRequest) request api.ReqQryQuote

    member this.QueryOptionSelfCloseAsync
        (exchangeId: string, optionSelfCloseSysId: string, insertTimeStart: string, insertTimeEnd: string,
         instrumentId: string)
        =
        let request: QryOptionSelfCloseRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              ExchangeId = exchangeId
              OptionSelfCloseSysId = optionSelfCloseSysId
              InsertTimeStart = insertTimeStart
              InsertTimeEnd = insertTimeEnd
              InstrumentId = instrumentId }

        this.QueryAsync<OptionSelfClose, QryOptionSelfCloseRequest>
            (nameof QryOptionSelfCloseRequest)
            request
            api.ReqQryOptionSelfClose

    member this.QueryInvestUnitAsync(?investUnitId: string) =
        let request: QryInvestUnitRequest =
            { BrokerId = Some options.BrokerId
              InvestorId = Some options.UserId
              InvestUnitId = investUnitId }

        this.QueryAsync<InvestUnit, QryInvestUnitRequest>(nameof QryInvestUnitRequest) request api.ReqQryInvestUnit

    member this.QueryCombInstrumentGuardAsync(exchangeId: string, ?instrumentId: string) =
        let request: QryCombInstrumentGuardRequest =
            { BrokerId = Some options.BrokerId
              Reserve1 = None
              ExchangeId = exchangeId
              InstrumentId = instrumentId }

        this.QueryAsync<CombInstrumentGuard, QryCombInstrumentGuardRequest>
            (nameof QryCombInstrumentGuardRequest)
            request
            api.ReqQryCombInstrumentGuard

    member this.QueryCombActionAsync(exchangeId: string, instrumentId: string, ?investUnitId: string) =
        let request: QryCombActionRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              ExchangeId = exchangeId
              InvestUnitId = investUnitId
              InstrumentId = instrumentId }

        this.QueryAsync<CombAction, QryCombActionRequest>(nameof QryCombActionRequest) request api.ReqQryCombAction

    member this.QueryTransferSerialAsync(accountId: string, bankId: string, currencyId: string) =
        let request: QryTransferSerialRequest =
            { BrokerId = options.BrokerId
              AccountId = accountId
              BankId = bankId
              CurrencyId = currencyId }

        this.QueryAsync<TransferSerial, QryTransferSerialRequest>
            (nameof QryTransferSerialRequest)
            request
            api.ReqQryTransferSerial

    member this.QueryAccountregisterAsync(?accountId: string, ?bankId: string, ?bankBranchId: string, ?currencyId: string) =
        let request: QryAccountregisterRequest =
            { BrokerId = Some options.BrokerId
              AccountId = accountId
              BankId = bankId
              BankBranchId = bankBranchId
              CurrencyId = currencyId }

        this.QueryAsync<Accountregister, QryAccountregisterRequest>
            (nameof QryAccountregisterRequest)
            request
            api.ReqQryAccountregister

    member this.QueryContractBankAsync(bankId: string, bankBrchId: string) =
        let request: QryContractBankRequest =
            { BrokerId = options.BrokerId
              BankId = bankId
              BankBrchId = bankBrchId }

        this.QueryAsync<ContractBank, QryContractBankRequest>(nameof QryContractBankRequest) request api.ReqQryContractBank

    member this.QueryParkedOrderAsync(exchangeId: string, instrumentId: string, ?investUnitId: string) =
        let request: QryParkedOrderRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              ExchangeId = exchangeId
              InvestUnitId = investUnitId
              InstrumentId = instrumentId }

        this.QueryAsync<ParkedOrder, QryParkedOrderRequest>(nameof QryParkedOrderRequest) request api.ReqQryParkedOrder

    member this.QueryParkedOrderActionAsync(exchangeId: string, instrumentId: string, ?investUnitId: string) =
        let request: QryParkedOrderActionRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              ExchangeId = exchangeId
              InvestUnitId = investUnitId
              InstrumentId = instrumentId }

        this.QueryAsync<ParkedOrderAction, QryParkedOrderActionRequest>
            (nameof QryParkedOrderActionRequest)
            request
            api.ReqQryParkedOrderAction

    member this.QueryTradingNoticeAsync(?investUnitId: string) =
        let request: QryTradingNoticeRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              InvestUnitId = investUnitId }

        this.QueryAsync<TradingNotice, QryTradingNoticeRequest>(nameof QryTradingNoticeRequest) request api.ReqQryTradingNotice

    member this.QueryBrokerTradingParamsAsync(currencyId: string, ?accountId: string) =
        let request: QryBrokerTradingParamsRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              CurrencyId = currencyId
              AccountId = accountId }

        this.QueryAsync<BrokerTradingParams, QryBrokerTradingParamsRequest>
            (nameof QryBrokerTradingParamsRequest)
            request
            api.ReqQryBrokerTradingParams

    member this.QueryBrokerTradingAlgosAsync(exchangeId: string, instrumentId: string) =
        let request: QryBrokerTradingAlgosRequest =
            { BrokerId = options.BrokerId
              ExchangeId = exchangeId
              Reserve1 = None
              InstrumentId = instrumentId }

        this.QueryAsync<BrokerTradingAlgos, QryBrokerTradingAlgosRequest>
            (nameof QryBrokerTradingAlgosRequest)
            request
            api.ReqQryBrokerTradingAlgos

    member this.QueryCfmmcTradingAccountTokenAsync(?investUnitId: string) =
        let request: QueryCfmmcTradingAccountTokenRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              InvestUnitId = investUnitId }

        this.QueryAsync<QueryCfmmcTradingAccountTokenRequest, QueryCfmmcTradingAccountTokenRequest>
            (nameof QueryCfmmcTradingAccountTokenRequest)
            request
            api.ReqQueryCfmmcTradingAccountToken

    member this.QueryClassifiedInstrumentAsync
        (tradingType: char, classType: char, ?instrumentId: string, ?exchangeId: string, ?exchangeInstId: string,
         ?productId: string)
        =
        let request: QryClassifiedInstrumentRequest =
            { InstrumentId = instrumentId
              ExchangeId = exchangeId
              ExchangeInstId = exchangeInstId
              ProductId = productId
              TradingType = tradingType
              ClassType = classType }

        this.QueryAsync<Instrument, QryClassifiedInstrumentRequest>
            (nameof QryClassifiedInstrumentRequest)
            request
            api.ReqQryClassifiedInstrument

    member this.QueryCombPromotionParamAsync(?exchangeId: string, ?instrumentId: string) =
        let request: QryCombPromotionParamRequest =
            { ExchangeId = exchangeId
              InstrumentId = instrumentId }

        this.QueryAsync<CombPromotionParam, QryCombPromotionParamRequest>
            (nameof QryCombPromotionParamRequest)
            request
            api.ReqQryCombPromotionParam

    member this.QueryRiskSettleInvstPositionAsync(?instrumentId: string) =
        let request: QryRiskSettleInvstPositionRequest =
            { BrokerId = Some options.BrokerId
              InvestorId = Some options.UserId
              InstrumentId = instrumentId }

        this.QueryAsync<RiskSettleInvstPosition, QryRiskSettleInvstPositionRequest>
            (nameof QryRiskSettleInvstPositionRequest)
            request
            api.ReqQryRiskSettleInvstPosition

    member this.QueryRiskSettleProductStatusAsync(?productId: string) =
        let request: QryRiskSettleProductStatusRequest = { ProductId = productId }

        this.QueryAsync<RiskSettleProductStatus, QryRiskSettleProductStatusRequest>
            (nameof QryRiskSettleProductStatusRequest)
            request
            api.ReqQryRiskSettleProductStatus

    member this.QuerySpbmFutureParameterAsync(exchangeId: string, instrumentId: string, prodFamilyCode: string) =
        let request: QrySpbmFutureParameterRequest =
            { ExchangeId = exchangeId
              InstrumentId = instrumentId
              ProdFamilyCode = prodFamilyCode }

        this.QueryAsync<SpbmFutureParameter, QrySpbmFutureParameterRequest>
            (nameof QrySpbmFutureParameterRequest)
            request
            api.ReqQrySpbmFutureParameter

    member this.QuerySpbmOptionParameterAsync(exchangeId: string, instrumentId: string, prodFamilyCode: string) =
        let request: QrySpbmOptionParameterRequest =
            { ExchangeId = exchangeId
              InstrumentId = instrumentId
              ProdFamilyCode = prodFamilyCode }

        this.QueryAsync<SpbmOptionParameter, QrySpbmOptionParameterRequest>
            (nameof QrySpbmOptionParameterRequest)
            request
            api.ReqQrySpbmOptionParameter

    member this.QuerySpbmIntraParameterAsync(exchangeId: string, prodFamilyCode: string) =
        let request: QrySpbmIntraParameterRequest =
            { ExchangeId = exchangeId
              ProdFamilyCode = prodFamilyCode }

        this.QueryAsync<SpbmIntraParameter, QrySpbmIntraParameterRequest>
            (nameof QrySpbmIntraParameterRequest)
            request
            api.ReqQrySpbmIntraParameter

    member this.QuerySpbmInterParameterAsync(exchangeId: string, leg1ProdFamilyCode: string, leg2ProdFamilyCode: string) =
        let request: QrySpbmInterParameterRequest =
            { ExchangeId = exchangeId
              Leg1ProdFamilyCode = leg1ProdFamilyCode
              Leg2ProdFamilyCode = leg2ProdFamilyCode }

        this.QueryAsync<SpbmInterParameter, QrySpbmInterParameterRequest>
            (nameof QrySpbmInterParameterRequest)
            request
            api.ReqQrySpbmInterParameter

    member this.QuerySpbmPortfDefinitionAsync
        (exchangeId: string, portfolioDefId: string, prodFamilyCode: string)
        =
        let request: QrySpbmPortfDefinitionRequest =
            { ExchangeId = exchangeId
              PortfolioDefId = portfolioDefId
              ProdFamilyCode = prodFamilyCode }

        this.QueryAsync<SpbmPortfDefinition, QrySpbmPortfDefinitionRequest>
            (nameof QrySpbmPortfDefinitionRequest)
            request
            api.ReqQrySpbmPortfDefinition

    member this.QuerySpbmInvestorPortfDefAsync(exchangeId: string) =
        let request: QrySpbmInvestorPortfDefRequest =
            { ExchangeId = exchangeId
              BrokerId = options.BrokerId
              InvestorId = options.UserId }

        this.QueryAsync<SpbmInvestorPortfDef, QrySpbmInvestorPortfDefRequest>
            (nameof QrySpbmInvestorPortfDefRequest)
            request
            api.ReqQrySpbmInvestorPortfDef

    member this.QueryInvestorPortfMarginRatioAsync(exchangeId: string, ?productGroupId: string) =
        let request: QryInvestorPortfMarginRatioRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              ExchangeId = exchangeId
              ProductGroupId = productGroupId }

        this.QueryAsync<InvestorPortfMarginRatio, QryInvestorPortfMarginRatioRequest>
            (nameof QryInvestorPortfMarginRatioRequest)
            request
            api.ReqQryInvestorPortfMarginRatio

    member this.QueryInvestorProdSpbmDetailAsync(exchangeId: string, prodFamilyCode: string) =
        let request: QryInvestorProdSpbmDetailRequest =
            { ExchangeId = exchangeId
              BrokerId = options.BrokerId
              InvestorId = options.UserId
              ProdFamilyCode = prodFamilyCode }

        this.QueryAsync<InvestorProdSpbmDetail, QryInvestorProdSpbmDetailRequest>
            (nameof QryInvestorProdSpbmDetailRequest)
            request
            api.ReqQryInvestorProdSpbmDetail

    member this.QueryInvestorCommoditySpmmMarginAsync(commodityId: string) =
        let request: QryInvestorCommoditySpmmMarginRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              CommodityId = commodityId }

        this.QueryAsync<InvestorCommoditySpmmMargin, QryInvestorCommoditySpmmMarginRequest>
            (nameof QryInvestorCommoditySpmmMarginRequest)
            request
            api.ReqQryInvestorCommoditySpmmMargin

    member this.QueryInvestorCommodityGroupSpmmMarginAsync(commodityGroupId: string) =
        let request: QryInvestorCommodityGroupSpmmMarginRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              CommodityGroupId = commodityGroupId }

        this.QueryAsync<InvestorCommodityGroupSpmmMargin, QryInvestorCommodityGroupSpmmMarginRequest>
            (nameof QryInvestorCommodityGroupSpmmMarginRequest)
            request
            api.ReqQryInvestorCommodityGroupSpmmMargin

    member this.QuerySpmmInstParamAsync(instrumentId: string) =
        let request: QrySpmmInstParamRequest = { InstrumentId = instrumentId }

        this.QueryAsync<SpmmInstParam, QrySpmmInstParamRequest>(nameof QrySpmmInstParamRequest) request api.ReqQrySpmmInstParam

    member this.QuerySpmmProductParamAsync(productId: string) =
        let request: QrySpmmProductParamRequest = { ProductId = productId }

        this.QueryAsync<SpmmProductParam, QrySpmmProductParamRequest>
            (nameof QrySpmmProductParamRequest)
            request
            api.ReqQrySpmmProductParam

    member this.QuerySpbmAddOnInterParameterAsync(exchangeId: string, leg1ProdFamilyCode: string, leg2ProdFamilyCode: string) =
        let request: QrySpbmAddOnInterParameterRequest =
            { ExchangeId = exchangeId
              Leg1ProdFamilyCode = leg1ProdFamilyCode
              Leg2ProdFamilyCode = leg2ProdFamilyCode }

        this.QueryAsync<SpbmAddOnInterParameter, QrySpbmAddOnInterParameterRequest>
            (nameof QrySpbmAddOnInterParameterRequest)
            request
            api.ReqQrySpbmAddOnInterParameter

    member this.QueryRcamsCombProductInfoAsync(productId: string, combProductId: string, productGroupId: string) =
        let request: QryRcamsCombProductInfoRequest =
            { ProductId = productId
              CombProductId = combProductId
              ProductGroupId = productGroupId }

        this.QueryAsync<RcamsCombProductInfo, QryRcamsCombProductInfoRequest>
            (nameof QryRcamsCombProductInfoRequest)
            request
            api.ReqQryRcamsCombProductInfo

    member this.QueryRcamsInstrParameterAsync(productId: string) =
        let request: QryRcamsInstrParameterRequest = { ProductId = productId }

        this.QueryAsync<RcamsInstrParameter, QryRcamsInstrParameterRequest>
            (nameof QryRcamsInstrParameterRequest)
            request
            api.ReqQryRcamsInstrParameter

    member this.QueryRcamsIntraParameterAsync(combProductId: string) =
        let request: QryRcamsIntraParameterRequest = { CombProductId = combProductId }

        this.QueryAsync<RcamsIntraParameter, QryRcamsIntraParameterRequest>
            (nameof QryRcamsIntraParameterRequest)
            request
            api.ReqQryRcamsIntraParameter

    member this.QueryRcamsInterParameterAsync(productGroupId: string, combProduct1: string, combProduct2: string) =
        let request: QryRcamsInterParameterRequest =
            { ProductGroupId = productGroupId
              CombProduct1 = combProduct1
              CombProduct2 = combProduct2 }

        this.QueryAsync<RcamsInterParameter, QryRcamsInterParameterRequest>
            (nameof QryRcamsInterParameterRequest)
            request
            api.ReqQryRcamsInterParameter

    member this.QueryRcamsShortOptAdjustParamAsync(combProductId: string) =
        let request: QryRcamsShortOptAdjustParamRequest = { CombProductId = combProductId }

        this.QueryAsync<RcamsShortOptAdjustParam, QryRcamsShortOptAdjustParamRequest>
            (nameof QryRcamsShortOptAdjustParamRequest)
            request
            api.ReqQryRcamsShortOptAdjustParam

    member this.QueryRcamsInvestorCombPositionAsync(instrumentId: string, combInstrumentId: string) =
        let request: QryRcamsInvestorCombPositionRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              InstrumentId = instrumentId
              CombInstrumentId = combInstrumentId }

        this.QueryAsync<RcamsInvestorCombPosition, QryRcamsInvestorCombPositionRequest>
            (nameof QryRcamsInvestorCombPositionRequest)
            request
            api.ReqQryRcamsInvestorCombPosition

    member this.QueryInvestorProdRcamsMarginAsync(combProductId: string, productGroupId: string) =
        let request: QryInvestorProdRcamsMarginRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              CombProductId = combProductId
              ProductGroupId = productGroupId }

        this.QueryAsync<InvestorProdRcamsMargin, QryInvestorProdRcamsMarginRequest>
            (nameof QryInvestorProdRcamsMarginRequest)
            request
            api.ReqQryInvestorProdRcamsMargin

    member this.QueryRuleInstrParameterAsync(exchangeId: string, instrumentId: string) =
        let request: QryRuleInstrParameterRequest =
            { ExchangeId = exchangeId
              InstrumentId = instrumentId }

        this.QueryAsync<RuleInstrParameter, QryRuleInstrParameterRequest>
            (nameof QryRuleInstrParameterRequest)
            request
            api.ReqQryRuleInstrParameter

    member this.QueryRuleIntraParameterAsync(exchangeId: string, prodFamilyCode: string) =
        let request: QryRuleIntraParameterRequest =
            { ExchangeId = exchangeId
              ProdFamilyCode = prodFamilyCode }

        this.QueryAsync<RuleIntraParameter, QryRuleIntraParameterRequest>
            (nameof QryRuleIntraParameterRequest)
            request
            api.ReqQryRuleIntraParameter

    member this.QueryRuleInterParameterAsync
        (commodityGroupId: string, exchangeId: string, leg1ProdFamilyCode: string, ?leg2ProdFamilyCode: string)
        =
        let request: QryRuleInterParameterRequest =
            { ExchangeId = exchangeId
              Leg1ProdFamilyCode = leg1ProdFamilyCode
              Leg2ProdFamilyCode = leg2ProdFamilyCode
              CommodityGroupId = commodityGroupId }

        this.QueryAsync<RuleInterParameter, QryRuleInterParameterRequest>
            (nameof QryRuleInterParameterRequest)
            request
            api.ReqQryRuleInterParameter

    member this.QueryInvestorProdRuleMarginAsync
        (exchangeId: string, prodFamilyCode: string, commodityGroupId: string)
        =
        let request: QryInvestorProdRuleMarginRequest =
            { ExchangeId = exchangeId
              BrokerId = options.BrokerId
              InvestorId = options.UserId
              ProdFamilyCode = prodFamilyCode
              CommodityGroupId = commodityGroupId }

        this.QueryAsync<InvestorProdRuleMargin, QryInvestorProdRuleMarginRequest>
            (nameof QryInvestorProdRuleMarginRequest)
            request
            api.ReqQryInvestorProdRuleMargin

    member this.QueryInvestorPortfSettingAsync(exchangeId: string) =
        let request: QryInvestorPortfSettingRequest =
            { ExchangeId = exchangeId
              BrokerId = options.BrokerId
              InvestorId = options.UserId }

        this.QueryAsync<InvestorPortfSetting, QryInvestorPortfSettingRequest>
            (nameof QryInvestorPortfSettingRequest)
            request
            api.ReqQryInvestorPortfSetting

    member this.QueryInvestorInfoCommRecAsync(instrumentId: string) =
        let request: QryInvestorInfoCommRecRequest =
            { InvestorId = options.UserId
              InstrumentId = instrumentId
              BrokerId = options.BrokerId }

        this.QueryAsync<InvestorInfoCommRec, QryInvestorInfoCommRecRequest>
            (nameof QryInvestorInfoCommRecRequest)
            request
            api.ReqQryInvestorInfoCommRec

    member this.QueryCombLegAsync(legInstrumentId: string) =
        let request: QryCombLeg = { LegInstrumentId = legInstrumentId }

        this.QueryAsync<CombLeg, QryCombLeg>(nameof QryCombLeg) request api.ReqQryCombLeg

    member this.QueryOffsetSettingAsync(productId: string, offsetType: char) =
        let request: QryOffsetSettingRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              ProductId = productId
              OffsetType = offsetType }

        this.QueryAsync<OffsetSetting, QryOffsetSettingRequest>(nameof QryOffsetSettingRequest) request api.ReqQryOffsetSetting

    member this.QuerySpdApplyAsync(exchangeId: string, orderSysId: string, firstLegInstrumentId: string, secondLegInstrumentId: string) =
        let request: QrySpdApplyRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              ExchangeId = exchangeId
              OrderSysId = orderSysId
              FirstLegInstrumentId = firstLegInstrumentId
              SecondLegInstrumentId = secondLegInstrumentId }

        this.QueryAsync<SpdApply, QrySpdApplyRequest>(nameof QrySpdApplyRequest) request api.ReqQrySpdApply

    member this.QueryHedgeCfmAsync(exchangeId: string, orderSysId: string, instrumentId: string) =
        let request: QryHedgeCfmRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              ExchangeId = exchangeId
              OrderSysId = orderSysId
              InstrumentId = instrumentId }

        this.QueryAsync<HedgeCfm, QryHedgeCfmRequest>(nameof QryHedgeCfmRequest) request api.ReqQryHedgeCfm
