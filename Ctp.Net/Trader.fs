namespace Ctp.Net

open System
open Ctp.Net.Bridge
open System.Threading
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
    | OrderReceived of OrderUpdateResponse
    | TradeReceived of TradeUpdateResponse
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
        ?loggerFactory: ILoggerFactory,
        ?flowControl: CtpFlowControlOptions
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
    let orderEvent = Event<OrderUpdateResponse>()
    let tradeEvent = Event<TradeUpdateResponse>()
    let notificationEvent = Event<obj>()
    let asyncErrorEvent = Event<obj>()
    let api = new TraderApi(options.FlowPath, options.ProductionMode, encodings = bridgeEncodings)
    let requestFlow = FlowController(defaultArg flowControl CtpFlowControlOptions.Default, logger = logger)

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
                GenSmsCodeResponse = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                GenUserCaptchaResponse = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                GenUserTextResponse = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspQueryCfmmcTradingAccountToken =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspTradingAccountPasswordUpdate = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                UserAuthMethodResponse = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspUserPasswordUpdate = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspBatchOrderAction = Some(postOrderCommandResponse "BatchOrderActionResponse")
                RspCancelOffsetSetting = Some(postOrderCommandResponse "CancelOffsetSettingResponse")
                RspCombActionInsert = Some(postOrderCommandResponse "CombActionInsert")
                RspExecOrderAction = Some(postOrderCommandResponse "ExecOrderActionResponse")
                RspExecOrderInsert = Some(postOrderCommandResponse "ExecOrderInsert")
                RspForQuoteInsert = Some(postOrderCommandResponse "ForQuoteInsert")
                RspFromBankToFutureByFuture = Some(postOrderCommandResponse "FromBankToFutureByFuture")
                RspFromFutureToBankByFuture = Some(postOrderCommandResponse "FromFutureToBankByFuture")
                RspHedgeCfm = Some(postOrderCommandResponse "HedgeCfmResponse")
                RspHedgeCfmAction = Some(postOrderCommandResponse "HedgeCfmActionResponse")
                RspOffsetSetting = Some(postOrderCommandResponse "OffsetSettingResponse")
                RspOptionSelfCloseAction = Some(postOrderCommandResponse "OptionSelfCloseActionResponse")
                RspOptionSelfCloseInsert = Some(postOrderCommandResponse "OptionSelfCloseInsert")
                RspParkedOrderAction = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspParkedOrderInsert = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspQueryBankAccountMoneyByFuture =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspQuoteAction = Some(postOrderCommandResponse "QuoteActionResponse")
                RspQuoteInsert = Some(postOrderCommandResponse "QuoteInsert")
                RspRemoveParkedOrder = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspRemoveParkedOrderAction = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspSpdApply = Some(postOrderCommandResponse "SpdApplyResponse")
                RspSpdApplyAction = Some(postOrderCommandResponse "SpdApplyActionResponse")
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

    member _.Connect(?timeout: TimeSpan, ?cancellationToken: CancellationToken) =
        match timeout with
        | Some timeout -> connectionCoordinator.Connect(timeout = timeout, ?cancellationToken = cancellationToken)
        | None -> connectionCoordinator.Connect(?cancellationToken = cancellationToken)

    member _.Join() = api.Join()

    member private _.RunPendingRequestAsync<'TResponse, 'TRequest>
        (operationName: string)
        (request: 'TRequest)
        (apiCall: 'TRequest * int -> int)
        (onAccepted: int -> unit)
        (cancellationToken: CancellationToken option)
        : Async<Result<'TResponse list, RspInfo>>
        =
        async {
            let! ambientCancellationToken = Async.CancellationToken
            let cancellationToken = defaultArg cancellationToken ambientCancellationToken

            let rec executeAttempt attempt = async {
                do!
                    requestFlow.AwaitDispatchAsync(cancellationToken = cancellationToken)
                    |> Async.AwaitTask

                logger.LogDebug("Sending {OperationName} request", operationName)

                let requestId = nextRequestId ()
                let completion = ClientHelpers.createCompletionSource<Result<'TResponse list, RspInfo>> ()
                pending.Register(requestId, operationName, completion)
                let result = apiCall (request, requestId)

                if result <> 0 then
                    pending.TryRemove requestId

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
                    onAccepted requestId
                    return! (completion.Task |> ClientHelpers.awaitTaskWithCancellation cancellationToken)
            }

            return! executeAttempt 0
        }

    member private _.RunCommandAsync
        (operationName: string)
        (apiCall: int -> int)
        (cancellationToken: CancellationToken option)
        : Async<int>
        =
        async {
            let! ambientCancellationToken = Async.CancellationToken
            let cancellationToken = defaultArg cancellationToken ambientCancellationToken

            let rec executeAttempt attempt = async {
                do!
                    requestFlow.AwaitDispatchAsync(cancellationToken = cancellationToken)
                    |> Async.AwaitTask

                logger.LogDebug("Sending {OperationName} request", operationName)

                let requestId = nextRequestId ()
                let result = apiCall requestId

                if result <> 0 then
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

                        return requestId
                else
                    return requestId
            }

            return! executeAttempt 0
        }

    member private _.QueryAsync<'TItem, 'TRequest>
        (queryName: string)
        (request: 'TRequest)
        (apiCall: 'TRequest * int -> int)
        (cancellationToken: CancellationToken option)
        : Async<Result<'TItem list, RspInfo>>
        =
        async {
            let! ambientCancellationToken = Async.CancellationToken
            let cancellationToken = defaultArg cancellationToken ambientCancellationToken

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
                        pending.Register(requestId, queryName, completion)
                        let errCode = apiCall (request, requestId)

                        if errCode <> 0 then
                            pending.TryRemove requestId

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
                                requestFlow.AwaitQueryCompletionAsync(queryName, requestId, pending, completion.Task)

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

    member this.AuthenticateAsync(?cancellationToken: CancellationToken) =
        let request = OptionHelpers.createAuthenticateRequest options

        this.RunPendingRequestAsync<AuthenticateResponse, AuthenticateRequest>
            "Authenticate"
            request
            api.ReqAuthenticate
            ignore
            cancellationToken

    member this.SettlementInfoConfirmAsync(?cancellationToken: CancellationToken) =
        let request = OptionHelpers.createSettlementInfoConfirmRequest options

        this.RunPendingRequestAsync<SettlementInfoConfirm, SettlementInfoConfirm>
            "SettlementInfoConfirm"
            request
            api.ReqSettlementInfoConfirm
            ignore
            cancellationToken

    member this.LoginAsync(?cancellationToken: CancellationToken) =
        let request = OptionHelpers.createUserLoginRequest options

        this.RunPendingRequestAsync<UserLoginResponse, UserLoginRequest>
            "Login"
            request
            api.ReqUserLogin
            ignore
            cancellationToken

    member this.LogoutAsync(?cancellationToken: CancellationToken) =
        let request = OptionHelpers.createUserLogoutRequest options

        this.RunPendingRequestAsync<UserLogoutResponse, UserLogoutRequest>
            "Logout"
            request
            api.ReqUserLogout
            (fun requestId ->
                // Current CTP SDK does not reliably invoke OnRspUserLogout, so a successful request is treated as completion.
                pending.TryAccumulate(
                    requestId,
                    Some { UserLogoutResponse.BrokerId = request.BrokerId; UserId = request.UserId },
                    None,
                    true
                ))
            cancellationToken

    member this.QueryTradingAccountAsync
        (currencyId: string, ?bizType: BizType, ?accountId: string, ?cancellationToken: CancellationToken)
        =
        let request =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              CurrencyId = currencyId
              BizType = bizType
              AccountId = accountId }

        this.QueryAsync<TradingAccountResponse, QueryTradingAccountRequest>
            (nameof QueryTradingAccountRequest)
            request
            api.ReqQryTradingAccount
            cancellationToken

    member this.QueryInvestorPositionAsync
        (instrumentId: string, ?exchangeId: string, ?investUnitId: string, ?cancellationToken: CancellationToken)
        =
        let request: QueryInvestorPositionRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              ExchangeId = exchangeId
              InvestUnitId = investUnitId
              InstrumentId = instrumentId
              Reserve1 = None }

        this.QueryAsync<InvestorPositionResponse, QueryInvestorPositionRequest>
            (nameof QueryInvestorPositionRequest)
            request
            api.ReqQryInvestorPosition
            cancellationToken

    member this.QueryInstrumentMarginRateAsync
        (
            hedgeFlag: HedgeFlag,
            instrumentId: string,
            ?exchangeId: string,
            ?investUnitId: string,
            ?cancellationToken: CancellationToken
        )
        =
        let request: QueryInstrumentMarginRateRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              HedgeFlag = hedgeFlag
              ExchangeId = exchangeId
              InvestUnitId = investUnitId
              InstrumentId = instrumentId
              Reserve1 = None }

        this.QueryAsync<InstrumentMarginRateResponse, QueryInstrumentMarginRateRequest>
            (nameof QueryInstrumentMarginRateRequest)
            request
            api.ReqQryInstrumentMarginRate
            cancellationToken

    member this.QueryExchangeMarginRateAsync
        (hedgeFlag: HedgeFlag, instrumentId: string, ?exchangeId: string, ?cancellationToken: CancellationToken)
        =
        let request: QueryExchangeMarginRateRequest =
            { BrokerId = options.BrokerId
              HedgeFlag = hedgeFlag
              ExchangeId = exchangeId
              InstrumentId = instrumentId
              Reserve1 = None }

        this.QueryAsync<ExchangeMarginRateResponse, QueryExchangeMarginRateRequest>
            (nameof QueryExchangeMarginRateRequest)
            request
            api.ReqQryExchangeMarginRate
            cancellationToken

    member this.QueryInstrumentCommissionRateAsync
        (instrumentId: string, ?exchangeId: string, ?investUnitId: string, ?cancellationToken: CancellationToken)
        =
        let request: QueryInstrumentCommissionRateRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              ExchangeId = exchangeId
              InvestUnitId = investUnitId
              InstrumentId = instrumentId
              Reserve1 = None }

        this.QueryAsync<InstrumentCommissionRateResponse, QueryInstrumentCommissionRateRequest>
            (nameof QueryInstrumentCommissionRateRequest)
            request
            api.ReqQryInstrumentCommissionRate
            cancellationToken

    // ---- Utility / diagnostic methods ----

    member _.GetApiVersion() = TraderApi.GetApiVersion()

    member _.GetTradingDay() = api.GetTradingDay()

    member _.GetFrontInfo() = api.GetFrontInfo()

    // ---- Connection configuration (call before Connect) ----

    member _.RegisterNameServer(nsAddress: string) = api.RegisterNameServer(nsAddress)

    member _.RegisterFensUserInfo(request: FensUserInfoRequest) = api.RegisterFensUserInfo(request)

    // ---- Regulatory / system-info methods ----

    member _.RegisterUserSystemInfo(info: UserSystemInfoRequest) = api.RegisterUserSystemInfo(info)

    member _.SubmitUserSystemInfo(info: UserSystemInfoRequest) = api.SubmitUserSystemInfo(info)

    member _.RegisterWechatUserSystemInfo(info: WechatUserSystemInfoRequest) = api.RegisterWechatUserSystemInfo(info)

    member _.SubmitWechatUserSystemInfo(info: WechatUserSystemInfoRequest) = api.SubmitWechatUserSystemInfo(info)

    // ---- Auth / password / captcha methods ----

    member this.ReqUserPasswordUpdateAsync
        (oldPassword: string, newPassword: string, ?cancellationToken: CancellationToken)
        =
        let request: UserPasswordUpdate =
            { BrokerId = options.BrokerId
              UserId = options.UserId
              OldPassword = oldPassword
              NewPassword = newPassword }

        this.RunPendingRequestAsync<UserPasswordUpdate, UserPasswordUpdate>
            "UserPasswordUpdate"
            request
            api.ReqUserPasswordUpdate
            ignore
            cancellationToken

    member this.ReqTradingAccountPasswordUpdateAsync
        (
            accountId: string,
            oldPassword: string,
            newPassword: string,
            currencyId: string,
            ?cancellationToken: CancellationToken
        )
        =
        let request: TradingAccountPasswordUpdate =
            { BrokerId = options.BrokerId
              AccountId = accountId
              OldPassword = oldPassword
              NewPassword = newPassword
              CurrencyId = currencyId }

        this.RunPendingRequestAsync<TradingAccountPasswordUpdate, TradingAccountPasswordUpdate>
            "TradingAccountPasswordUpdate"
            request
            api.ReqTradingAccountPasswordUpdate
            ignore
            cancellationToken

    member this.ReqUserAuthMethodAsync(?tradingDay: string, ?cancellationToken: CancellationToken) =
        let request: UserAuthMethodRequest =
            { BrokerId = Some options.BrokerId
              UserId = Some options.UserId
              TradingDay = tradingDay }

        this.RunPendingRequestAsync<UserAuthMethodResponse, UserAuthMethodRequest>
            "UserAuthMethod"
            request
            api.ReqUserAuthMethod
            ignore
            cancellationToken

    member this.ReqGenUserCaptchaAsync(?tradingDay: string, ?cancellationToken: CancellationToken) =
        let request: GenUserCaptchaRequest =
            { BrokerId = Some options.BrokerId
              UserId = Some options.UserId
              TradingDay = tradingDay }

        this.RunPendingRequestAsync<GenUserCaptchaResponse, GenUserCaptchaRequest>
            "GenUserCaptcha"
            request
            api.ReqGenUserCaptcha
            ignore
            cancellationToken

    member this.ReqGenUserTextAsync(?tradingDay: string, ?cancellationToken: CancellationToken) =
        let request: GenUserTextRequest =
            { BrokerId = Some options.BrokerId
              UserId = Some options.UserId
              TradingDay = tradingDay }

        this.RunPendingRequestAsync<GenUserTextResponse, GenUserTextRequest>
            "GenUserText"
            request
            api.ReqGenUserText
            ignore
            cancellationToken

    member this.ReqUserLoginWithCaptchaAsync
        (
            captcha: string,
            ?tradingDay: string,
            ?userProductInfo: string,
            ?interfaceProductInfo: string,
            ?protocolInfo: string,
            ?macAddress: string,
            ?loginRemark: string,
            ?clientIpPort: int,
            ?clientIpAddress: string,
            ?cancellationToken: CancellationToken
        )
        =
        let request: UserLoginWithCaptchaRequest =
            { TradingDay = tradingDay
              BrokerId = options.BrokerId
              UserId = options.UserId
              Password = options.Password
              UserProductInfo = userProductInfo
              InterfaceProductInfo = interfaceProductInfo
              ProtocolInfo = protocolInfo
              MacAddress = macAddress
              Reserve1 = None
              LoginRemark = loginRemark
              Captcha = captcha
              ClientIpPort = clientIpPort
              ClientIpAddress = clientIpAddress }

        this.RunPendingRequestAsync<UserLoginResponse, UserLoginWithCaptchaRequest>
            "LoginWithCaptcha"
            request
            api.ReqUserLoginWithCaptcha
            ignore
            cancellationToken

    member this.ReqUserLoginWithTextAsync
        (
            text: string,
            ?tradingDay: string,
            ?userProductInfo: string,
            ?interfaceProductInfo: string,
            ?protocolInfo: string,
            ?macAddress: string,
            ?loginRemark: string,
            ?clientIpPort: int,
            ?clientIpAddress: string,
            ?cancellationToken: CancellationToken
        )
        =
        let request: UserLoginWithTextRequest =
            { TradingDay = tradingDay
              BrokerId = options.BrokerId
              UserId = options.UserId
              Password = options.Password
              UserProductInfo = userProductInfo
              InterfaceProductInfo = interfaceProductInfo
              ProtocolInfo = protocolInfo
              MacAddress = macAddress
              Reserve1 = None
              LoginRemark = loginRemark
              Text = text
              ClientIpPort = clientIpPort
              ClientIpAddress = clientIpAddress }

        this.RunPendingRequestAsync<UserLoginResponse, UserLoginWithTextRequest>
            "LoginWithText"
            request
            api.ReqUserLoginWithText
            ignore
            cancellationToken

    member this.ReqUserLoginWithOtpAsync
        (
            otpPassword: string,
            ?tradingDay: string,
            ?userProductInfo: string,
            ?interfaceProductInfo: string,
            ?protocolInfo: string,
            ?macAddress: string,
            ?loginRemark: string,
            ?clientIpPort: int,
            ?clientIpAddress: string,
            ?cancellationToken: CancellationToken
        )
        =
        let request: UserLoginWithOtpRequest =
            { TradingDay = tradingDay
              BrokerId = options.BrokerId
              UserId = options.UserId
              Password = options.Password
              UserProductInfo = userProductInfo
              InterfaceProductInfo = interfaceProductInfo
              ProtocolInfo = protocolInfo
              MacAddress = macAddress
              Reserve1 = None
              LoginRemark = loginRemark
              OtpPassword = otpPassword
              ClientIpPort = clientIpPort
              ClientIpAddress = clientIpAddress }

        this.RunPendingRequestAsync<UserLoginResponse, UserLoginWithOtpRequest>
            "LoginWithOtp"
            request
            api.ReqUserLoginWithOtp
            ignore
            cancellationToken

    member this.ReqGenSmsCodeAsync(mobile: string, ?cancellationToken: CancellationToken) =
        let request: GenSmsCodeRequest =
            { BrokerId = options.BrokerId; UserId = options.UserId; Mobile = mobile }

        this.RunPendingRequestAsync<GenSmsCodeResponse, GenSmsCodeRequest>
            "GenSmsCode"
            request
            api.ReqGenSmsCode
            ignore
            cancellationToken

    // ---- Order insertion and action ----

    member this.InsertOrderAsync(request: InputOrderRequest, ?cancellationToken: CancellationToken) =
        this.RunCommandAsync "OrderInsert" (fun requestId -> api.ReqOrderInsert(request, requestId)) cancellationToken

    member this.CancelOrderAsync(request: InputOrderActionRequest, ?cancellationToken: CancellationToken) =
        this.RunCommandAsync "OrderActionResponse" (fun requestId -> api.ReqOrderAction(request, requestId)) cancellationToken

    // ---- Execution / quote / hedge / combination command methods ----

    member this.ReqExecOrderInsertAsync(request: InputExecOrderRequest, ?cancellationToken: CancellationToken) =
        this.RunCommandAsync
            "ExecOrderInsert"
            (fun requestId -> api.ReqExecOrderInsert(request, requestId))
            cancellationToken

    member this.ReqExecOrderActionAsync(request: InputExecOrderActionRequest, ?cancellationToken: CancellationToken) =
        this.RunCommandAsync
            "ExecOrderActionResponse"
            (fun requestId -> api.ReqExecOrderAction(request, requestId))
            cancellationToken

    member this.ReqForQuoteInsertAsync
        (instrumentId: string, ?exchangeId: string, ?cancellationToken: CancellationToken)
        =
        let request: InputForQuoteRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              ForQuoteRef = None
              UserId = None
              ExchangeId = exchangeId
              InvestUnitId = None
              Reserve2 = None
              MacAddress = None
              InstrumentId = instrumentId
              IpAddress = None }

        this.RunCommandAsync
            "ForQuoteInsert"
            (fun requestId -> api.ReqForQuoteInsert(request, requestId))
            cancellationToken

    member this.ReqQuoteInsertAsync(request: InputQuoteRequest, ?cancellationToken: CancellationToken) =
        this.RunCommandAsync "QuoteInsert" (fun requestId -> api.ReqQuoteInsert(request, requestId)) cancellationToken

    member this.ReqQuoteActionAsync(request: InputQuoteActionRequest, ?cancellationToken: CancellationToken) =
        this.RunCommandAsync "QuoteActionResponse" (fun requestId -> api.ReqQuoteAction(request, requestId)) cancellationToken

    member this.ReqBatchOrderActionAsync
        (frontId: int, sessionId: int, ?exchangeId: string, ?cancellationToken: CancellationToken)
        =
        let request: InputBatchOrderActionRequest =
            { BrokerId = Some options.BrokerId
              InvestorId = options.UserId
              OrderActionRef = 0
              RequestId = 0
              FrontId = frontId
              SessionId = sessionId
              ExchangeId = exchangeId
              UserId = None
              InvestUnitId = None
              Reserve1 = None
              MacAddress = None
              IpAddress = None }

        this.RunCommandAsync
            "BatchOrderActionResponse"
            (fun requestId -> api.ReqBatchOrderAction(request, requestId))
            cancellationToken

    member this.ReqOptionSelfCloseInsertAsync
        (request: InputOptionSelfCloseRequest, ?cancellationToken: CancellationToken)
        =
        this.RunCommandAsync
            "OptionSelfCloseInsert"
            (fun requestId -> api.ReqOptionSelfCloseInsert(request, requestId))
            cancellationToken

    member this.ReqOptionSelfCloseActionAsync
        (request: InputOptionSelfCloseActionRequest, ?cancellationToken: CancellationToken)
        =
        this.RunCommandAsync
            "OptionSelfCloseActionResponse"
            (fun requestId -> api.ReqOptionSelfCloseAction(request, requestId))
            cancellationToken

    member this.ReqCombActionInsertAsync(request: InputCombActionRequest, ?cancellationToken: CancellationToken) =
        this.RunCommandAsync
            "CombActionInsert"
            (fun requestId -> api.ReqCombActionInsert(request, requestId))
            cancellationToken

    member this.ReqOffsetSettingAsync(request: InputOffsetSettingRequest, ?cancellationToken: CancellationToken) =
        this.RunCommandAsync
            "OffsetSettingResponse"
            (fun requestId -> api.ReqOffsetSetting(request, requestId))
            cancellationToken

    member this.ReqCancelOffsetSettingAsync(request: InputOffsetSettingRequest, ?cancellationToken: CancellationToken) =
        this.RunCommandAsync
            "CancelOffsetSettingResponse"
            (fun requestId -> api.ReqCancelOffsetSetting(request, requestId))
            cancellationToken

    member this.ReqSpdApplyAsync(request: InputSpdApplyRequest, ?cancellationToken: CancellationToken) =
        this.RunCommandAsync "SpdApplyResponse" (fun requestId -> api.ReqSpdApply(request, requestId)) cancellationToken

    member this.ReqSpdApplyActionAsync(request: InputSpdApplyActionRequest, ?cancellationToken: CancellationToken) =
        this.RunCommandAsync
            "SpdApplyActionResponse"
            (fun requestId -> api.ReqSpdApplyAction(request, requestId))
            cancellationToken

    member this.ReqHedgeCfmAsync(request: InputHedgeCfmRequest, ?cancellationToken: CancellationToken) =
        this.RunCommandAsync "HedgeCfmResponse" (fun requestId -> api.ReqHedgeCfm(request, requestId)) cancellationToken

    member this.ReqHedgeCfmActionAsync(request: InputHedgeCfmActionRequest, ?cancellationToken: CancellationToken) =
        this.RunCommandAsync
            "HedgeCfmActionResponse"
            (fun requestId -> api.ReqHedgeCfmAction(request, requestId))
            cancellationToken

    // ---- Parked order methods (FinalOnly) ----

    member this.ReqParkedOrderInsertAsync(request: ParkedOrder, ?cancellationToken: CancellationToken) =
        this.RunPendingRequestAsync<ParkedOrder, ParkedOrder>
            "ParkedOrderInsert"
            request
            api.ReqParkedOrderInsert
            ignore
            cancellationToken

    member this.ReqParkedOrderActionAsync(request: ParkedOrderAction, ?cancellationToken: CancellationToken) =
        this.RunPendingRequestAsync<ParkedOrderAction, ParkedOrderAction>
            "ParkedOrderAction"
            request
            api.ReqParkedOrderAction
            ignore
            cancellationToken

    member this.ReqRemoveParkedOrderAsync
        (parkedOrderId: string, ?investUnitId: string, ?cancellationToken: CancellationToken)
        =
        let request: RemoveParkedOrder =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              ParkedOrderId = parkedOrderId
              InvestUnitId = investUnitId }

        this.RunPendingRequestAsync<RemoveParkedOrder, RemoveParkedOrder>
            "RemoveParkedOrder"
            request
            api.ReqRemoveParkedOrder
            ignore
            cancellationToken

    member this.ReqRemoveParkedOrderActionAsync
        (parkedOrderActionId: string, ?investUnitId: string, ?cancellationToken: CancellationToken)
        =
        let request: RemoveParkedOrderAction =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              ParkedOrderActionId = parkedOrderActionId
              InvestUnitId = investUnitId }

        this.RunPendingRequestAsync<RemoveParkedOrderAction, RemoveParkedOrderAction>
            "RemoveParkedOrderAction"
            request
            api.ReqRemoveParkedOrderAction
            ignore
            cancellationToken

    // ---- Bank transfer methods ----

    member this.FromBankToFutureByFutureAsync(request: TransferRequest, ?cancellationToken: CancellationToken) =
        this.RunCommandAsync
            "FromBankToFutureByFuture"
            (fun requestId -> api.ReqFromBankToFutureByFuture(request, requestId))
            cancellationToken

    member this.FromFutureToBankByFutureAsync(request: TransferRequest, ?cancellationToken: CancellationToken) =
        this.RunCommandAsync
            "FromFutureToBankByFuture"
            (fun requestId -> api.ReqFromFutureToBankByFuture(request, requestId))
            cancellationToken

    member this.QueryBankAccountMoneyByFutureAsync(request: ReqQueryAccount, ?cancellationToken: CancellationToken) =
        this.RunPendingRequestAsync<ReqQueryAccount, ReqQueryAccount>
            "QueryBankAccountMoneyByFuture"
            request
            api.ReqQueryBankAccountMoneyByFuture
            ignore
            cancellationToken

    // ---- Query methods ----

    member this.QueryMaxOrderVolumeAsync
        (
            direction: Direction,
            offsetFlag: OffsetFlag,
            hedgeFlag: HedgeFlag,
            instrumentId: string,
            ?maxVolume: int,
            ?exchangeId: string,
            ?investUnitId: string,
            ?cancellationToken: CancellationToken
        )
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
            cancellationToken

    member this.QueryOrderAsync
        (
            exchangeId: string,
            orderSysId: string,
            insertTimeStart: string,
            insertTimeEnd: string,
            instrumentId: string,
            ?investUnitId: string,
            ?cancellationToken: CancellationToken
        )
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

        this.QueryAsync<OrderUpdateResponse, QryOrderRequest> (nameof QryOrderRequest) request api.ReqQryOrder cancellationToken

    member this.QueryTradeAsync
        (
            ?exchangeId: string,
            ?tradeId: string,
            ?tradeTimeStart: string,
            ?tradeTimeEnd: string,
            ?investUnitId: string,
            ?instrumentId: string,
            ?cancellationToken: CancellationToken
        )
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

        this.QueryAsync<TradeUpdateResponse, QryTradeRequest> (nameof QryTradeRequest) request api.ReqQryTrade cancellationToken

    member this.QueryInvestorAsync(?cancellationToken: CancellationToken) =
        let request: QryInvestorRequest = { BrokerId = options.BrokerId; InvestorId = options.UserId }

        this.QueryAsync<InvestorResponse, QryInvestorRequest>
            (nameof QryInvestorRequest)
            request
            api.ReqQryInvestor
            cancellationToken

    member this.QueryTradingCodeAsync
        (
            exchangeId: string,
            clientId: string,
            clientIdType: ClientIdType,
            ?investUnitId: string,
            ?cancellationToken: CancellationToken
        )
        =
        let request: QryTradingCodeRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              ExchangeId = exchangeId
              ClientId = clientId
              ClientIdType = clientIdType
              InvestUnitId = investUnitId }

        this.QueryAsync<TradingCodeResponse, QryTradingCodeRequest>
            (nameof QryTradingCodeRequest)
            request
            api.ReqQryTradingCode
            cancellationToken

    member this.QueryUserSessionAsync(frontId: int, sessionId: int, ?cancellationToken: CancellationToken) =
        let request: QryUserSessionRequest =
            { FrontId = frontId
              SessionId = sessionId
              BrokerId = options.BrokerId
              UserId = options.UserId }

        this.QueryAsync<UserSessionResponse, QryUserSessionRequest>
            (nameof QryUserSessionRequest)
            request
            api.ReqQryUserSession
            cancellationToken

    member this.QueryExchangeAsync(exchangeId: string, ?cancellationToken: CancellationToken) =
        let request: QryExchangeRequest = { ExchangeId = exchangeId }

        this.QueryAsync<ExchangeResponse, QryExchangeRequest>
            (nameof QryExchangeRequest)
            request
            api.ReqQryExchange
            cancellationToken

    member this.QueryProductAsync
        (productId: string, ?productClass: ProductClass, ?exchangeId: string, ?cancellationToken: CancellationToken)
        =
        let request: QryProductRequest =
            { Reserve1 = None
              ProductClass = productClass
              ExchangeId = exchangeId
              ProductId = productId }

        this.QueryAsync<ProductResponse, QryProductRequest>
            (nameof QryProductRequest)
            request
            api.ReqQryProduct
            cancellationToken

    member this.QueryInstrumentAsync
        (
            exchangeId: string,
            instrumentId: string,
            exchangeInstId: string,
            productId: string,
            ?cancellationToken: CancellationToken
        )
        =
        let request: QryInstrumentRequest =
            { Reserve1 = None
              ExchangeId = exchangeId
              Reserve2 = None
              Reserve3 = None
              InstrumentId = instrumentId
              ExchangeInstId = exchangeInstId
              ProductId = productId }

        this.QueryAsync<InstrumentResponse, QryInstrumentRequest>
            (nameof QryInstrumentRequest)
            request
            api.ReqQryInstrument
            cancellationToken

    member this.QueryDepthMarketDataAsync
        (instrumentId: string, productClass: ProductClass, ?exchangeId: string, ?cancellationToken: CancellationToken)
        =
        let request: QryDepthMarketDataRequest =
            { Reserve1 = None
              ExchangeId = exchangeId
              InstrumentId = instrumentId
              ProductClass = productClass }

        this.QueryAsync<DepthMarketDataResponse, QryDepthMarketDataRequest>
            (nameof QryDepthMarketDataRequest)
            request
            api.ReqQryDepthMarketData
            cancellationToken

    member this.QueryTraderOfferAsync
        (?exchangeId: string, ?participantId: string, ?traderId: string, ?cancellationToken: CancellationToken)
        =
        let request: QryTraderOfferRequest =
            { ExchangeId = exchangeId; ParticipantId = participantId; TraderId = traderId }

        this.QueryAsync<TraderOfferResponse, QryTraderOfferRequest>
            (nameof QryTraderOfferRequest)
            request
            api.ReqQryTraderOffer
            cancellationToken

    member this.QuerySettlementInfoAsync
        (tradingDay: string, ?accountId: string, ?currencyId: string, ?cancellationToken: CancellationToken)
        =
        let request: QrySettlementInfoRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              TradingDay = tradingDay
              AccountId = accountId
              CurrencyId = currencyId }

        this.QueryAsync<SettlementInfoResponse, QrySettlementInfoRequest>
            (nameof QrySettlementInfoRequest)
            request
            api.ReqQrySettlementInfo
            cancellationToken

    member this.QueryTransferBankAsync(?bankId: string, ?bankBrchId: string, ?cancellationToken: CancellationToken) =
        let request: QryTransferBankRequest = { BankId = bankId; BankBrchId = bankBrchId }

        this.QueryAsync<TransferBankResponse, QryTransferBankRequest>
            (nameof QryTransferBankRequest)
            request
            api.ReqQryTransferBank
            cancellationToken

    member this.QueryInvestorPositionDetailAsync
        (instrumentId: string, ?exchangeId: string, ?investUnitId: string, ?cancellationToken: CancellationToken)
        =
        let request: QryInvestorPositionDetailRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              ExchangeId = exchangeId
              InvestUnitId = investUnitId
              InstrumentId = instrumentId }

        this.QueryAsync<InvestorPositionDetailResponse, QryInvestorPositionDetailRequest>
            (nameof QryInvestorPositionDetailRequest)
            request
            api.ReqQryInvestorPositionDetail
            cancellationToken

    member this.QueryNoticeAsync(?cancellationToken: CancellationToken) =
        let request: QryNoticeRequest = { BrokerId = options.BrokerId }
        this.QueryAsync<NoticeResponse, QryNoticeRequest> (nameof QryNoticeRequest) request api.ReqQryNotice cancellationToken

    member this.QuerySettlementInfoConfirmAsync
        (?accountId: string, ?currencyId: string, ?cancellationToken: CancellationToken)
        =
        let request: QrySettlementInfoConfirmRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              AccountId = accountId
              CurrencyId = currencyId }

        this.QueryAsync<SettlementInfoConfirm, QrySettlementInfoConfirmRequest>
            (nameof QrySettlementInfoConfirmRequest)
            request
            api.ReqQrySettlementInfoConfirm
            cancellationToken

    member this.QueryInvestorPositionCombineDetailAsync
        (combInstrumentId: string, ?exchangeId: string, ?investUnitId: string, ?cancellationToken: CancellationToken)
        =
        let request: QryInvestorPositionCombineDetailRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              ExchangeId = exchangeId
              InvestUnitId = investUnitId
              CombInstrumentId = combInstrumentId }

        this.QueryAsync<InvestorPositionCombineDetailResponse, QryInvestorPositionCombineDetailRequest>
            (nameof QryInvestorPositionCombineDetailRequest)
            request
            api.ReqQryInvestorPositionCombineDetail
            cancellationToken

    member this.QueryCfmmcTradingAccountKeyAsync(?cancellationToken: CancellationToken) =
        let request: QryCfmmcTradingAccountKeyRequest =
            { BrokerId = Some options.BrokerId; InvestorId = Some options.UserId }

        this.QueryAsync<CfmmcTradingAccountKeyResponse, QryCfmmcTradingAccountKeyRequest>
            (nameof QryCfmmcTradingAccountKeyRequest)
            request
            api.ReqQryCfmmcTradingAccountKey
            cancellationToken

    member this.QueryEWarrantOffsetAsync
        (exchangeId: string, instrumentId: string, ?investUnitId: string, ?cancellationToken: CancellationToken)
        =
        let request: QryEWarrantOffsetRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              ExchangeId = exchangeId
              Reserve1 = None
              InvestUnitId = investUnitId
              InstrumentId = instrumentId }

        this.QueryAsync<EWarrantOffsetResponse, QryEWarrantOffsetRequest>
            (nameof QryEWarrantOffsetRequest)
            request
            api.ReqQryEWarrantOffset
            cancellationToken

    member this.QueryInvestorProductGroupMarginAsync
        (
            productGroupId: string,
            ?hedgeFlag: HedgeFlag,
            ?exchangeId: string,
            ?investUnitId: string,
            ?cancellationToken: CancellationToken
        )
        =
        let request: QryInvestorProductGroupMarginRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              HedgeFlag = hedgeFlag
              ExchangeId = exchangeId
              InvestUnitId = investUnitId
              ProductGroupId = productGroupId }

        this.QueryAsync<InvestorProductGroupMarginResponse, QryInvestorProductGroupMarginRequest>
            (nameof QryInvestorProductGroupMarginRequest)
            request
            api.ReqQryInvestorProductGroupMargin
            cancellationToken

    member this.QueryExchangeMarginRateAdjustAsync
        (instrumentId: string, hedgeFlag: HedgeFlag, ?cancellationToken: CancellationToken)
        =
        let request: QryExchangeMarginRateAdjustRequest =
            { BrokerId = options.BrokerId
              Reserve1 = None
              HedgeFlag = hedgeFlag
              InstrumentId = instrumentId }

        this.QueryAsync<ExchangeMarginRateAdjustResponse, QryExchangeMarginRateAdjustRequest>
            (nameof QryExchangeMarginRateAdjustRequest)
            request
            api.ReqQryExchangeMarginRateAdjust
            cancellationToken

    member this.QueryExchangeRateAsync
        (fromCurrencyId: string, toCurrencyId: string, ?cancellationToken: CancellationToken)
        =
        let request: QryExchangeRateRequest =
            { BrokerId = options.BrokerId
              FromCurrencyId = fromCurrencyId
              ToCurrencyId = toCurrencyId }

        this.QueryAsync<ExchangeRateResponse, QryExchangeRateRequest>
            (nameof QryExchangeRateRequest)
            request
            api.ReqQryExchangeRate
            cancellationToken

    member this.QuerySecAgentAcIdMapAsync
        (accountId: string, currencyId: string, ?cancellationToken: CancellationToken)
        =
        let request: QrySecAgentAcIdMapRequest =
            { BrokerId = options.BrokerId
              UserId = options.UserId
              AccountId = accountId
              CurrencyId = currencyId }

        this.QueryAsync<SecAgentAcIdMapResponse, QrySecAgentAcIdMapRequest>
            (nameof QrySecAgentAcIdMapRequest)
            request
            api.ReqQrySecAgentAcIdMap
            cancellationToken

    member this.QueryProductExchRateAsync
        (productId: string, ?exchangeId: string, ?cancellationToken: CancellationToken)
        =
        let request: QryProductExchRateRequest =
            { Reserve1 = None; ExchangeId = exchangeId; ProductId = productId }

        this.QueryAsync<ProductExchRateResponse, QryProductExchRateRequest>
            (nameof QryProductExchRateRequest)
            request
            api.ReqQryProductExchRate
            cancellationToken

    member this.QueryProductGroupAsync(exchangeId: string, productId: string, ?cancellationToken: CancellationToken) =
        let request: QryProductGroupRequest =
            { Reserve1 = None; ExchangeId = exchangeId; ProductId = productId }

        this.QueryAsync<ProductGroupResponse, QryProductGroupRequest>
            (nameof QryProductGroupRequest)
            request
            api.ReqQryProductGroup
            cancellationToken

    member this.QueryMmInstrumentCommissionRateAsync(instrumentId: string, ?cancellationToken: CancellationToken) =
        let request: QryMmInstrumentCommissionRateRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              InstrumentId = instrumentId }

        this.QueryAsync<MmInstrumentCommissionRateResponse, QryMmInstrumentCommissionRateRequest>
            (nameof QryMmInstrumentCommissionRateRequest)
            request
            api.ReqQryMmInstrumentCommissionRate
            cancellationToken

    member this.QueryMmOptionInstrCommRateAsync(instrumentId: string, ?cancellationToken: CancellationToken) =
        let request: QryMmOptionInstrCommRateRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              InstrumentId = instrumentId }

        this.QueryAsync<MmOptionInstrCommRateResponse, QryMmOptionInstrCommRateRequest>
            (nameof QryMmOptionInstrCommRateRequest)
            request
            api.ReqQryMmOptionInstrCommRate
            cancellationToken

    member this.QueryInstrumentOrderCommRateAsync(instrumentId: string, ?cancellationToken: CancellationToken) =
        let request: QryInstrumentOrderCommRateRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              InstrumentId = instrumentId }

        this.QueryAsync<InstrumentOrderCommRateResponse, QryInstrumentOrderCommRateRequest>
            (nameof QryInstrumentOrderCommRateRequest)
            request
            api.ReqQryInstrumentOrderCommRate
            cancellationToken

    member this.QuerySecAgentTradingAccountAsync
        (currencyId: string, ?bizType: BizType, ?accountId: string, ?cancellationToken: CancellationToken)
        =
        let request: QueryTradingAccountRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              CurrencyId = currencyId
              BizType = bizType
              AccountId = accountId }

        this.QueryAsync<TradingAccountResponse, QueryTradingAccountRequest>
            (nameof QueryTradingAccountRequest)
            request
            api.ReqQrySecAgentTradingAccount
            cancellationToken

    member this.QuerySecAgentCheckModeAsync(?cancellationToken: CancellationToken) =
        let request: QrySecAgentCheckModeRequest = { BrokerId = options.BrokerId; InvestorId = options.UserId }

        this.QueryAsync<SecAgentCheckModeResponse, QrySecAgentCheckModeRequest>
            (nameof QrySecAgentCheckModeRequest)
            request
            api.ReqQrySecAgentCheckMode
            cancellationToken

    member this.QuerySecAgentTradeInfoAsync(brokerSecAgentId: string, ?cancellationToken: CancellationToken) =
        let request: QrySecAgentTradeInfoRequest =
            { BrokerId = options.BrokerId; BrokerSecAgentId = brokerSecAgentId }

        this.QueryAsync<SecAgentTradeInfoResponse, QrySecAgentTradeInfoRequest>
            (nameof QrySecAgentTradeInfoRequest)
            request
            api.ReqQrySecAgentTradeInfo
            cancellationToken

    member this.QueryOptionInstrTradeCostAsync
        (
            instrumentId: string,
            hedgeFlag: HedgeFlag,
            inputPrice: decimal,
            underlyingPrice: decimal,
            ?exchangeId: string,
            ?investUnitId: string,
            ?cancellationToken: CancellationToken
        )
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

        this.QueryAsync<OptionInstrTradeCostResponse, QryOptionInstrTradeCostRequest>
            (nameof QryOptionInstrTradeCostRequest)
            request
            api.ReqQryOptionInstrTradeCost
            cancellationToken

    member this.QueryOptionInstrCommRateAsync
        (instrumentId: string, ?exchangeId: string, ?investUnitId: string, ?cancellationToken: CancellationToken)
        =
        let request: QryOptionInstrCommRateRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              ExchangeId = exchangeId
              InvestUnitId = investUnitId
              InstrumentId = instrumentId }

        this.QueryAsync<OptionInstrCommRateResponse, QryOptionInstrCommRateRequest>
            (nameof QryOptionInstrCommRateRequest)
            request
            api.ReqQryOptionInstrCommRate
            cancellationToken

    member this.QueryExecOrderAsync
        (
            exchangeId: string,
            execOrderSysId: string,
            insertTimeStart: string,
            insertTimeEnd: string,
            instrumentId: string,
            ?cancellationToken: CancellationToken
        )
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

        this.QueryAsync<ExecOrderResponse, QryExecOrderRequest>
            (nameof QryExecOrderRequest)
            request
            api.ReqQryExecOrder
            cancellationToken

    member this.QueryForQuoteAsync
        (
            exchangeId: string,
            insertTimeStart: string,
            insertTimeEnd: string,
            instrumentId: string,
            ?investUnitId: string,
            ?cancellationToken: CancellationToken
        )
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

        this.QueryAsync<ForQuoteResponse, QryForQuoteRequest>
            (nameof QryForQuoteRequest)
            request
            api.ReqQryForQuote
            cancellationToken

    member this.QueryQuoteAsync
        (
            exchangeId: string,
            quoteSysId: string,
            insertTimeStart: string,
            insertTimeEnd: string,
            instrumentId: string,
            ?investUnitId: string,
            ?cancellationToken: CancellationToken
        )
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

        this.QueryAsync<QuoteResponse, QryQuoteRequest> (nameof QryQuoteRequest) request api.ReqQryQuote cancellationToken

    member this.QueryOptionSelfCloseAsync
        (
            exchangeId: string,
            optionSelfCloseSysId: string,
            insertTimeStart: string,
            insertTimeEnd: string,
            instrumentId: string,
            ?cancellationToken: CancellationToken
        )
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

        this.QueryAsync<OptionSelfCloseResponse, QryOptionSelfCloseRequest>
            (nameof QryOptionSelfCloseRequest)
            request
            api.ReqQryOptionSelfClose
            cancellationToken

    member this.QueryInvestUnitAsync(?investUnitId: string, ?cancellationToken: CancellationToken) =
        let request: QryInvestUnitRequest =
            { BrokerId = Some options.BrokerId
              InvestorId = Some options.UserId
              InvestUnitId = investUnitId }

        this.QueryAsync<InvestUnitResponse, QryInvestUnitRequest>
            (nameof QryInvestUnitRequest)
            request
            api.ReqQryInvestUnit
            cancellationToken

    member this.QueryCombInstrumentGuardAsync
        (exchangeId: string, ?instrumentId: string, ?cancellationToken: CancellationToken)
        =
        let request: QryCombInstrumentGuardRequest =
            { BrokerId = Some options.BrokerId
              Reserve1 = None
              ExchangeId = exchangeId
              InstrumentId = instrumentId }

        this.QueryAsync<CombInstrumentGuardResponse, QryCombInstrumentGuardRequest>
            (nameof QryCombInstrumentGuardRequest)
            request
            api.ReqQryCombInstrumentGuard
            cancellationToken

    member this.QueryCombActionAsync
        (exchangeId: string, instrumentId: string, ?investUnitId: string, ?cancellationToken: CancellationToken)
        =
        let request: QryCombActionRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              ExchangeId = exchangeId
              InvestUnitId = investUnitId
              InstrumentId = instrumentId }

        this.QueryAsync<CombActionResponse, QryCombActionRequest>
            (nameof QryCombActionRequest)
            request
            api.ReqQryCombAction
            cancellationToken

    member this.QueryTransferSerialAsync
        (accountId: string, bankId: string, currencyId: string, ?cancellationToken: CancellationToken)
        =
        let request: QryTransferSerialRequest =
            { BrokerId = options.BrokerId
              AccountId = accountId
              BankId = bankId
              CurrencyId = currencyId }

        this.QueryAsync<TransferSerialResponse, QryTransferSerialRequest>
            (nameof QryTransferSerialRequest)
            request
            api.ReqQryTransferSerial
            cancellationToken

    member this.QueryAccountregisterAsync
        (
            ?accountId: string,
            ?bankId: string,
            ?bankBranchId: string,
            ?currencyId: string,
            ?cancellationToken: CancellationToken
        )
        =
        let request: QryAccountregisterRequest =
            { BrokerId = Some options.BrokerId
              AccountId = accountId
              BankId = bankId
              BankBranchId = bankBranchId
              CurrencyId = currencyId }

        this.QueryAsync<AccountregisterResponse, QryAccountregisterRequest>
            (nameof QryAccountregisterRequest)
            request
            api.ReqQryAccountregister
            cancellationToken

    member this.QueryContractBankAsync(bankId: string, bankBrchId: string, ?cancellationToken: CancellationToken) =
        let request: QryContractBankRequest =
            { BrokerId = options.BrokerId; BankId = bankId; BankBrchId = bankBrchId }

        this.QueryAsync<ContractBankResponse, QryContractBankRequest>
            (nameof QryContractBankRequest)
            request
            api.ReqQryContractBank
            cancellationToken

    member this.QueryParkedOrderAsync
        (exchangeId: string, instrumentId: string, ?investUnitId: string, ?cancellationToken: CancellationToken)
        =
        let request: QryParkedOrderRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              ExchangeId = exchangeId
              InvestUnitId = investUnitId
              InstrumentId = instrumentId }

        this.QueryAsync<ParkedOrder, QryParkedOrderRequest>
            (nameof QryParkedOrderRequest)
            request
            api.ReqQryParkedOrder
            cancellationToken

    member this.QueryParkedOrderActionAsync
        (exchangeId: string, instrumentId: string, ?investUnitId: string, ?cancellationToken: CancellationToken)
        =
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
            cancellationToken

    member this.QueryTradingNoticeAsync(?investUnitId: string, ?cancellationToken: CancellationToken) =
        let request: QryTradingNoticeRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              InvestUnitId = investUnitId }

        this.QueryAsync<TradingNoticeResponse, QryTradingNoticeRequest>
            (nameof QryTradingNoticeRequest)
            request
            api.ReqQryTradingNotice
            cancellationToken

    member this.QueryBrokerTradingParamsAsync
        (currencyId: string, ?accountId: string, ?cancellationToken: CancellationToken)
        =
        let request: QryBrokerTradingParamsRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              CurrencyId = currencyId
              AccountId = accountId }

        this.QueryAsync<BrokerTradingParamsResponse, QryBrokerTradingParamsRequest>
            (nameof QryBrokerTradingParamsRequest)
            request
            api.ReqQryBrokerTradingParams
            cancellationToken

    member this.QueryBrokerTradingAlgosAsync
        (exchangeId: string, instrumentId: string, ?cancellationToken: CancellationToken)
        =
        let request: QryBrokerTradingAlgosRequest =
            { BrokerId = options.BrokerId
              ExchangeId = exchangeId
              Reserve1 = None
              InstrumentId = instrumentId }

        this.QueryAsync<BrokerTradingAlgosResponse, QryBrokerTradingAlgosRequest>
            (nameof QryBrokerTradingAlgosRequest)
            request
            api.ReqQryBrokerTradingAlgos
            cancellationToken

    member this.QueryCfmmcTradingAccountTokenAsync(?investUnitId: string, ?cancellationToken: CancellationToken) =
        let request: QueryCfmmcTradingAccountTokenRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              InvestUnitId = investUnitId }

        this.QueryAsync<QueryCfmmcTradingAccountTokenRequest, QueryCfmmcTradingAccountTokenRequest>
            (nameof QueryCfmmcTradingAccountTokenRequest)
            request
            api.ReqQueryCfmmcTradingAccountToken
            cancellationToken

    member this.QueryClassifiedInstrumentAsync
        (
            tradingType: TradingType,
            classType: ClassType,
            ?instrumentId: string,
            ?exchangeId: string,
            ?exchangeInstId: string,
            ?productId: string,
            ?cancellationToken: CancellationToken
        )
        =
        let request: QryClassifiedInstrumentRequest =
            { InstrumentId = instrumentId
              ExchangeId = exchangeId
              ExchangeInstId = exchangeInstId
              ProductId = productId
              TradingType = tradingType
              ClassType = classType }

        this.QueryAsync<InstrumentResponse, QryClassifiedInstrumentRequest>
            (nameof QryClassifiedInstrumentRequest)
            request
            api.ReqQryClassifiedInstrument
            cancellationToken

    member this.QueryCombPromotionParamAsync
        (?exchangeId: string, ?instrumentId: string, ?cancellationToken: CancellationToken)
        =
        let request: QryCombPromotionParamRequest = { ExchangeId = exchangeId; InstrumentId = instrumentId }

        this.QueryAsync<CombPromotionParamResponse, QryCombPromotionParamRequest>
            (nameof QryCombPromotionParamRequest)
            request
            api.ReqQryCombPromotionParam
            cancellationToken

    member this.QueryRiskSettleInvstPositionAsync(?instrumentId: string, ?cancellationToken: CancellationToken) =
        let request: QryRiskSettleInvstPositionRequest =
            { BrokerId = Some options.BrokerId
              InvestorId = Some options.UserId
              InstrumentId = instrumentId }

        this.QueryAsync<RiskSettleInvstPositionResponse, QryRiskSettleInvstPositionRequest>
            (nameof QryRiskSettleInvstPositionRequest)
            request
            api.ReqQryRiskSettleInvstPosition
            cancellationToken

    member this.QueryRiskSettleProductStatusAsync(?productId: string, ?cancellationToken: CancellationToken) =
        let request: QryRiskSettleProductStatusRequest = { ProductId = productId }

        this.QueryAsync<RiskSettleProductStatusResponse, QryRiskSettleProductStatusRequest>
            (nameof QryRiskSettleProductStatusRequest)
            request
            api.ReqQryRiskSettleProductStatus
            cancellationToken

    member this.QuerySpbmFutureParameterAsync
        (exchangeId: string, instrumentId: string, prodFamilyCode: string, ?cancellationToken: CancellationToken)
        =
        let request: QrySpbmFutureParameterRequest =
            { ExchangeId = exchangeId
              InstrumentId = instrumentId
              ProdFamilyCode = prodFamilyCode }

        this.QueryAsync<SpbmFutureParameterResponse, QrySpbmFutureParameterRequest>
            (nameof QrySpbmFutureParameterRequest)
            request
            api.ReqQrySpbmFutureParameter
            cancellationToken

    member this.QuerySpbmOptionParameterAsync
        (exchangeId: string, instrumentId: string, prodFamilyCode: string, ?cancellationToken: CancellationToken)
        =
        let request: QrySpbmOptionParameterRequest =
            { ExchangeId = exchangeId
              InstrumentId = instrumentId
              ProdFamilyCode = prodFamilyCode }

        this.QueryAsync<SpbmOptionParameterResponse, QrySpbmOptionParameterRequest>
            (nameof QrySpbmOptionParameterRequest)
            request
            api.ReqQrySpbmOptionParameter
            cancellationToken

    member this.QuerySpbmIntraParameterAsync
        (exchangeId: string, prodFamilyCode: string, ?cancellationToken: CancellationToken)
        =
        let request: QrySpbmIntraParameterRequest = { ExchangeId = exchangeId; ProdFamilyCode = prodFamilyCode }

        this.QueryAsync<SpbmIntraParameterResponse, QrySpbmIntraParameterRequest>
            (nameof QrySpbmIntraParameterRequest)
            request
            api.ReqQrySpbmIntraParameter
            cancellationToken

    member this.QuerySpbmInterParameterAsync
        (
            exchangeId: string,
            leg1ProdFamilyCode: string,
            leg2ProdFamilyCode: string,
            ?cancellationToken: CancellationToken
        )
        =
        let request: QrySpbmInterParameterRequest =
            { ExchangeId = exchangeId
              Leg1ProdFamilyCode = leg1ProdFamilyCode
              Leg2ProdFamilyCode = leg2ProdFamilyCode }

        this.QueryAsync<SpbmInterParameterResponse, QrySpbmInterParameterRequest>
            (nameof QrySpbmInterParameterRequest)
            request
            api.ReqQrySpbmInterParameter
            cancellationToken

    member this.QuerySpbmPortfDefinitionAsync
        (exchangeId: string, portfolioDefId: string, prodFamilyCode: string, ?cancellationToken: CancellationToken)
        =
        let request: QrySpbmPortfDefinitionRequest =
            { ExchangeId = exchangeId
              PortfolioDefId = portfolioDefId
              ProdFamilyCode = prodFamilyCode }

        this.QueryAsync<SpbmPortfDefinitionResponse, QrySpbmPortfDefinitionRequest>
            (nameof QrySpbmPortfDefinitionRequest)
            request
            api.ReqQrySpbmPortfDefinition
            cancellationToken

    member this.QuerySpbmInvestorPortfDefAsync(exchangeId: string, ?cancellationToken: CancellationToken) =
        let request: QrySpbmInvestorPortfDefRequest =
            { ExchangeId = exchangeId
              BrokerId = options.BrokerId
              InvestorId = options.UserId }

        this.QueryAsync<SpbmInvestorPortfDefResponse, QrySpbmInvestorPortfDefRequest>
            (nameof QrySpbmInvestorPortfDefRequest)
            request
            api.ReqQrySpbmInvestorPortfDef
            cancellationToken

    member this.QueryInvestorPortfMarginRatioAsync
        (exchangeId: string, ?productGroupId: string, ?cancellationToken: CancellationToken)
        =
        let request: QryInvestorPortfMarginRatioRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              ExchangeId = exchangeId
              ProductGroupId = productGroupId }

        this.QueryAsync<InvestorPortfMarginRatioResponse, QryInvestorPortfMarginRatioRequest>
            (nameof QryInvestorPortfMarginRatioRequest)
            request
            api.ReqQryInvestorPortfMarginRatio
            cancellationToken

    member this.QueryInvestorProdSpbmDetailAsync
        (exchangeId: string, prodFamilyCode: string, ?cancellationToken: CancellationToken)
        =
        let request: QryInvestorProdSpbmDetailRequest =
            { ExchangeId = exchangeId
              BrokerId = options.BrokerId
              InvestorId = options.UserId
              ProdFamilyCode = prodFamilyCode }

        this.QueryAsync<InvestorProdSpbmDetailResponse, QryInvestorProdSpbmDetailRequest>
            (nameof QryInvestorProdSpbmDetailRequest)
            request
            api.ReqQryInvestorProdSpbmDetail
            cancellationToken

    member this.QueryInvestorCommoditySpmmMarginAsync(commodityId: string, ?cancellationToken: CancellationToken) =
        let request: QryInvestorCommoditySpmmMarginRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              CommodityId = commodityId }

        this.QueryAsync<InvestorCommoditySpmmMarginResponse, QryInvestorCommoditySpmmMarginRequest>
            (nameof QryInvestorCommoditySpmmMarginRequest)
            request
            api.ReqQryInvestorCommoditySpmmMargin
            cancellationToken

    member this.QueryInvestorCommodityGroupSpmmMarginAsync
        (commodityGroupId: string, ?cancellationToken: CancellationToken)
        =
        let request: QryInvestorCommodityGroupSpmmMarginRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              CommodityGroupId = commodityGroupId }

        this.QueryAsync<InvestorCommodityGroupSpmmMarginResponse, QryInvestorCommodityGroupSpmmMarginRequest>
            (nameof QryInvestorCommodityGroupSpmmMarginRequest)
            request
            api.ReqQryInvestorCommodityGroupSpmmMargin
            cancellationToken

    member this.QuerySpmmInstParamAsync(instrumentId: string, ?cancellationToken: CancellationToken) =
        let request: QrySpmmInstParamRequest = { InstrumentId = instrumentId }

        this.QueryAsync<SpmmInstParamResponse, QrySpmmInstParamRequest>
            (nameof QrySpmmInstParamRequest)
            request
            api.ReqQrySpmmInstParam
            cancellationToken

    member this.QuerySpmmProductParamAsync(productId: string, ?cancellationToken: CancellationToken) =
        let request: QrySpmmProductParamRequest = { ProductId = productId }

        this.QueryAsync<SpmmProductParamResponse, QrySpmmProductParamRequest>
            (nameof QrySpmmProductParamRequest)
            request
            api.ReqQrySpmmProductParam
            cancellationToken

    member this.QuerySpbmAddOnInterParameterAsync
        (
            exchangeId: string,
            leg1ProdFamilyCode: string,
            leg2ProdFamilyCode: string,
            ?cancellationToken: CancellationToken
        )
        =
        let request: QrySpbmAddOnInterParameterRequest =
            { ExchangeId = exchangeId
              Leg1ProdFamilyCode = leg1ProdFamilyCode
              Leg2ProdFamilyCode = leg2ProdFamilyCode }

        this.QueryAsync<SpbmAddOnInterParameterResponse, QrySpbmAddOnInterParameterRequest>
            (nameof QrySpbmAddOnInterParameterRequest)
            request
            api.ReqQrySpbmAddOnInterParameter
            cancellationToken

    member this.QueryRcamsCombProductInfoAsync
        (productId: string, combProductId: string, productGroupId: string, ?cancellationToken: CancellationToken)
        =
        let request: QryRcamsCombProductInfoRequest =
            { ProductId = productId
              CombProductId = combProductId
              ProductGroupId = productGroupId }

        this.QueryAsync<RcamsCombProductInfoResponse, QryRcamsCombProductInfoRequest>
            (nameof QryRcamsCombProductInfoRequest)
            request
            api.ReqQryRcamsCombProductInfo
            cancellationToken

    member this.QueryRcamsInstrParameterAsync(productId: string, ?cancellationToken: CancellationToken) =
        let request: QryRcamsInstrParameterRequest = { ProductId = productId }

        this.QueryAsync<RcamsInstrParameterResponse, QryRcamsInstrParameterRequest>
            (nameof QryRcamsInstrParameterRequest)
            request
            api.ReqQryRcamsInstrParameter
            cancellationToken

    member this.QueryRcamsIntraParameterAsync(combProductId: string, ?cancellationToken: CancellationToken) =
        let request: QryRcamsIntraParameterRequest = { CombProductId = combProductId }

        this.QueryAsync<RcamsIntraParameterResponse, QryRcamsIntraParameterRequest>
            (nameof QryRcamsIntraParameterRequest)
            request
            api.ReqQryRcamsIntraParameter
            cancellationToken

    member this.QueryRcamsInterParameterAsync
        (productGroupId: string, combProduct1: string, combProduct2: string, ?cancellationToken: CancellationToken)
        =
        let request: QryRcamsInterParameterRequest =
            { ProductGroupId = productGroupId
              CombProduct1 = combProduct1
              CombProduct2 = combProduct2 }

        this.QueryAsync<RcamsInterParameterResponse, QryRcamsInterParameterRequest>
            (nameof QryRcamsInterParameterRequest)
            request
            api.ReqQryRcamsInterParameter
            cancellationToken

    member this.QueryRcamsShortOptAdjustParamAsync(combProductId: string, ?cancellationToken: CancellationToken) =
        let request: QryRcamsShortOptAdjustParamRequest = { CombProductId = combProductId }

        this.QueryAsync<RcamsShortOptAdjustParamResponse, QryRcamsShortOptAdjustParamRequest>
            (nameof QryRcamsShortOptAdjustParamRequest)
            request
            api.ReqQryRcamsShortOptAdjustParam
            cancellationToken

    member this.QueryRcamsInvestorCombPositionAsync
        (instrumentId: string, combInstrumentId: string, ?cancellationToken: CancellationToken)
        =
        let request: QryRcamsInvestorCombPositionRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              InstrumentId = instrumentId
              CombInstrumentId = combInstrumentId }

        this.QueryAsync<RcamsInvestorCombPositionResponse, QryRcamsInvestorCombPositionRequest>
            (nameof QryRcamsInvestorCombPositionRequest)
            request
            api.ReqQryRcamsInvestorCombPosition
            cancellationToken

    member this.QueryInvestorProdRcamsMarginAsync
        (combProductId: string, productGroupId: string, ?cancellationToken: CancellationToken)
        =
        let request: QryInvestorProdRcamsMarginRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              CombProductId = combProductId
              ProductGroupId = productGroupId }

        this.QueryAsync<InvestorProdRcamsMarginResponse, QryInvestorProdRcamsMarginRequest>
            (nameof QryInvestorProdRcamsMarginRequest)
            request
            api.ReqQryInvestorProdRcamsMargin
            cancellationToken

    member this.QueryRuleInstrParameterAsync
        (exchangeId: string, instrumentId: string, ?cancellationToken: CancellationToken)
        =
        let request: QryRuleInstrParameterRequest = { ExchangeId = exchangeId; InstrumentId = instrumentId }

        this.QueryAsync<RuleInstrParameterResponse, QryRuleInstrParameterRequest>
            (nameof QryRuleInstrParameterRequest)
            request
            api.ReqQryRuleInstrParameter
            cancellationToken

    member this.QueryRuleIntraParameterAsync
        (exchangeId: string, prodFamilyCode: string, ?cancellationToken: CancellationToken)
        =
        let request: QryRuleIntraParameterRequest = { ExchangeId = exchangeId; ProdFamilyCode = prodFamilyCode }

        this.QueryAsync<RuleIntraParameterResponse, QryRuleIntraParameterRequest>
            (nameof QryRuleIntraParameterRequest)
            request
            api.ReqQryRuleIntraParameter
            cancellationToken

    member this.QueryRuleInterParameterAsync
        (
            commodityGroupId: string,
            exchangeId: string,
            leg1ProdFamilyCode: string,
            ?leg2ProdFamilyCode: string,
            ?cancellationToken: CancellationToken
        )
        =
        let request: QryRuleInterParameterRequest =
            { ExchangeId = exchangeId
              Leg1ProdFamilyCode = leg1ProdFamilyCode
              Leg2ProdFamilyCode = leg2ProdFamilyCode
              CommodityGroupId = commodityGroupId }

        this.QueryAsync<RuleInterParameterResponse, QryRuleInterParameterRequest>
            (nameof QryRuleInterParameterRequest)
            request
            api.ReqQryRuleInterParameter
            cancellationToken

    member this.QueryInvestorProdRuleMarginAsync
        (exchangeId: string, prodFamilyCode: string, commodityGroupId: string, ?cancellationToken: CancellationToken)
        =
        let request: QryInvestorProdRuleMarginRequest =
            { ExchangeId = exchangeId
              BrokerId = options.BrokerId
              InvestorId = options.UserId
              ProdFamilyCode = prodFamilyCode
              CommodityGroupId = commodityGroupId }

        this.QueryAsync<InvestorProdRuleMarginResponse, QryInvestorProdRuleMarginRequest>
            (nameof QryInvestorProdRuleMarginRequest)
            request
            api.ReqQryInvestorProdRuleMargin
            cancellationToken

    member this.QueryInvestorPortfSettingAsync(exchangeId: string, ?cancellationToken: CancellationToken) =
        let request: QryInvestorPortfSettingRequest =
            { ExchangeId = exchangeId
              BrokerId = options.BrokerId
              InvestorId = options.UserId }

        this.QueryAsync<InvestorPortfSettingResponse, QryInvestorPortfSettingRequest>
            (nameof QryInvestorPortfSettingRequest)
            request
            api.ReqQryInvestorPortfSetting
            cancellationToken

    member this.QueryInvestorInfoCommRecAsync(instrumentId: string, ?cancellationToken: CancellationToken) =
        let request: QryInvestorInfoCommRecRequest =
            { InvestorId = options.UserId
              InstrumentId = instrumentId
              BrokerId = options.BrokerId }

        this.QueryAsync<InvestorInfoCommRecResponse, QryInvestorInfoCommRecRequest>
            (nameof QryInvestorInfoCommRecRequest)
            request
            api.ReqQryInvestorInfoCommRec
            cancellationToken

    member this.QueryCombLegAsync(legInstrumentId: string, ?cancellationToken: CancellationToken) =
        let request: QryCombLegRequest = { LegInstrumentId = legInstrumentId }

        this.QueryAsync<CombLegResponse, QryCombLegRequest> (nameof QryCombLegRequest) request api.ReqQryCombLeg cancellationToken

    member this.QueryOffsetSettingAsync
        (productId: string, offsetType: OffsetType, ?cancellationToken: CancellationToken)
        =
        let request: QryOffsetSettingRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              ProductId = productId
              OffsetType = offsetType }

        this.QueryAsync<OffsetSettingResponse, QryOffsetSettingRequest>
            (nameof QryOffsetSettingRequest)
            request
            api.ReqQryOffsetSetting
            cancellationToken

    member this.QuerySpdApplyAsync
        (
            exchangeId: string,
            orderSysId: string,
            firstLegInstrumentId: string,
            secondLegInstrumentId: string,
            ?cancellationToken: CancellationToken
        )
        =
        let request: QrySpdApplyRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              ExchangeId = exchangeId
              OrderSysId = orderSysId
              FirstLegInstrumentId = firstLegInstrumentId
              SecondLegInstrumentId = secondLegInstrumentId }

        this.QueryAsync<SpdApplyResponse, QrySpdApplyRequest>
            (nameof QrySpdApplyRequest)
            request
            api.ReqQrySpdApply
            cancellationToken

    member this.QueryHedgeCfmAsync
        (exchangeId: string, orderSysId: string, instrumentId: string, ?cancellationToken: CancellationToken)
        =
        let request: QryHedgeCfmRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              ExchangeId = exchangeId
              OrderSysId = orderSysId
              InstrumentId = instrumentId }

        this.QueryAsync<HedgeCfmResponse, QryHedgeCfmRequest>
            (nameof QryHedgeCfmRequest)
            request
            api.ReqQryHedgeCfm
            cancellationToken
