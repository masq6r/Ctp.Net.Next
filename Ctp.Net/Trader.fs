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
    | CombActionReceived of CombActionResponse
    | ExecOrderReceived of ExecOrderResponse
    | ForQuoteRspReceived of ForQuoteRspResponse
    | FromBankToFutureByFutureReceived of TransferResponse
    | FromFutureToBankByFutureReceived of TransferResponse
    | HedgeCfmReceived of HedgeCfmResponse
    | OffsetSettingReceived of OffsetSettingResponse
    | OptionSelfCloseReceived of OptionSelfCloseResponse
    | QueryBankBalanceByFutureReceived of NotifyQueryAccountResponse
    | QuoteReceived of QuoteResponse
    | SpdApplyReceived of SpdApplyResponse
    | InstrumentStatusReceived of InstrumentStatusResponse
    | BulletinReceived of BulletinResponse
    | TradingNoticeReceived of TradingNoticeInfoResponse
    | ErrorConditionalOrderReceived of ErrorConditionalOrderResponse
    | CfmmcTradingAccountTokenReceived of CfmmcTradingAccountTokenResponse
    | FromBankToFutureByBankReceived of TransferResponse
    | FromFutureToBankByBankReceived of TransferResponse
    | RepealFromBankToFutureByBankReceived of RepealResponse
    | RepealFromFutureToBankByBankReceived of RepealResponse
    | RepealFromBankToFutureByFutureManualReceived of RepealResponse
    | RepealFromFutureToBankByFutureManualReceived of RepealResponse
    | RepealFromBankToFutureByFutureReceived of RepealResponse
    | RepealFromFutureToBankByFutureReceived of RepealResponse
    | OpenAccountByBankReceived of OpenAccountResponse
    | CancelAccountByBankReceived of CancelAccountResponse
    | ChangeAccountByBankReceived of ChangeAccountResponse

type private TraderAgentMessage =
    | SystemEvent of TraderSystemEvent
    | CorrelatedError of RspInfo option * int * bool
    | CorrelatedResponse of PendingResponseCompletionPolicy * objnull option * RspInfo option * int * bool
    | OrderCommandResponse of TraderCommandResponse * (unit -> unit)
    | PushNotification of TraderPushNotification
    | AsyncErrorPush of TraderAsyncError * (unit -> unit)

module internal TraderCallbackNames =
    let private normalizeAcronyms (name: string) =
        name.Replace("Cfmmc", "CFMMC")
            .Replace("AcId", "ACID")
            .Replace("Mm", "MM")
            .Replace("Spbm", "SPBM")
            .Replace("Spmm", "SPMM")
            .Replace("Rcams", "RCAMS")
            .Replace("Rule", "RULE")
            .Replace("Sms", "SMS")

    let forOperation (operationName: string) =
        match operationName with
        | "Login"
        | "LoginWithCaptcha"
        | "LoginWithText"
        | "LoginWithOtp" -> "OnRspUserLogin"
        | "Logout" -> "OnRspUserLogout"
        | "ParkedOrderRequest" -> "OnRspParkedOrderInsert"
        | "QueryBankAccountMoneyByFuture" -> "OnRspQueryBankAccountMoneyByFuture"
        | "QueryCfmmcTradingAccountToken"
        | "QueryCfmmcTradingAccountTokenRequest" -> "OnRspQueryCFMMCTradingAccountToken"
        | name ->
            let requestSuffix = "Request"

            let callbackName =
                if name.StartsWith("Query", StringComparison.Ordinal) && name.EndsWith(requestSuffix, StringComparison.Ordinal) then
                    let callbackSuffix = name.Substring(5, name.Length - 5 - requestSuffix.Length)
                    $"OnRspQry{callbackSuffix}"
                elif name.StartsWith("Qry", StringComparison.Ordinal) && name.EndsWith(requestSuffix, StringComparison.Ordinal) then
                    let callbackSuffix = name.Substring(0, name.Length - requestSuffix.Length)
                    $"OnRsp{callbackSuffix}"
                else
                    $"OnRsp{name}"

            normalizeAcronyms callbackName

module internal TraderOperationNames =
    [<Literal>]
    let OrderAction = "OrderAction"

    [<Literal>]
    let BatchOrderAction = "BatchOrderAction"

    [<Literal>]
    let CancelOffsetSetting = "CancelOffsetSetting"

    [<Literal>]
    let ExecOrderAction = "ExecOrderAction"

    [<Literal>]
    let HedgeCfm = "HedgeCfm"

    [<Literal>]
    let HedgeCfmAction = "HedgeCfmAction"

    [<Literal>]
    let OffsetSetting = "OffsetSetting"

    [<Literal>]
    let OptionSelfCloseAction = "OptionSelfCloseAction"

    [<Literal>]
    let QuoteAction = "QuoteAction"

    [<Literal>]
    let SpdApply = "SpdApply"

    [<Literal>]
    let SpdApplyAction = "SpdApplyAction"

type TraderClient
    (
        options: CtpOptions,
        ?encodings: CtpEncodingOptions,
        ?privateTopicResumeType: ResumeType,
        ?privateTopicSequenceNo: int,
        ?publicTopicResumeType: ResumeType,
        ?loggerFactory: ILoggerFactory,
        ?flowControl: CtpFlowControlOptions,
        ?endpoint: CtpEndpoint
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
    let combActionEvent = Event<CombActionResponse>()
    let execOrderEvent = Event<ExecOrderResponse>()
    let forQuoteRspEvent = Event<ForQuoteRspResponse>()
    let fromBankToFutureByFutureEvent = Event<TransferResponse>()
    let fromFutureToBankByFutureEvent = Event<TransferResponse>()
    let hedgeCfmEvent = Event<HedgeCfmResponse>()
    let offsetSettingEvent = Event<OffsetSettingResponse>()
    let optionSelfCloseEvent = Event<OptionSelfCloseResponse>()
    let queryBankBalanceByFutureEvent = Event<NotifyQueryAccountResponse>()
    let quoteEvent = Event<QuoteResponse>()
    let spdApplyEvent = Event<SpdApplyResponse>()
    let instrumentStatusEvent = Event<InstrumentStatusResponse>()
    let bulletinEvent = Event<BulletinResponse>()
    let tradingNoticeEvent = Event<TradingNoticeInfoResponse>()
    let errorConditionalOrderEvent = Event<ErrorConditionalOrderResponse>()
    let cfmmcTradingAccountTokenEvent = Event<CfmmcTradingAccountTokenResponse>()
    let fromBankToFutureByBankEvent = Event<TransferResponse>()
    let fromFutureToBankByBankEvent = Event<TransferResponse>()
    let repealFromBankToFutureByBankEvent = Event<RepealResponse>()
    let repealFromFutureToBankByBankEvent = Event<RepealResponse>()
    let repealFromBankToFutureByFutureManualEvent = Event<RepealResponse>()
    let repealFromFutureToBankByFutureManualEvent = Event<RepealResponse>()
    let repealFromBankToFutureByFutureEvent = Event<RepealResponse>()
    let repealFromFutureToBankByFutureEvent = Event<RepealResponse>()
    let openAccountByBankEvent = Event<OpenAccountResponse>()
    let cancelAccountByBankEvent = Event<CancelAccountResponse>()
    let changeAccountByBankEvent = Event<ChangeAccountResponse>()
    let notificationEvent = Event<obj>()
    let asyncErrorEvent = Event<obj>()
    let asyncErrorDetailedEvent = Event<TraderAsyncError>()
    let commandResponseEvent = Event<TraderCommandResponse>()
    let bankToFutureByFutureErrorEvent = Event<TraderAsyncErrorData<TransferAckResponse>>()
    let batchOrderActionErrorEvent = Event<TraderAsyncErrorData<BatchOrderActionResponse>>()
    let cancelOffsetSettingErrorEvent = Event<TraderAsyncErrorData<CancelOffsetSettingResponse>>()
    let combActionInsertErrorEvent = Event<TraderAsyncErrorData<InputCombActionResponse>>()
    let execOrderActionErrorEvent = Event<TraderAsyncErrorData<ExecOrderActionResponse>>()
    let execOrderInsertErrorEvent = Event<TraderAsyncErrorData<InputExecOrderResponse>>()
    let forQuoteInsertErrorEvent = Event<TraderAsyncErrorData<InputForQuoteResponse>>()
    let futureToBankByFutureErrorEvent = Event<TraderAsyncErrorData<TransferAckResponse>>()
    let hedgeCfmErrorEvent = Event<TraderAsyncErrorData<InputHedgeCfmResponse>>()
    let hedgeCfmActionErrorEvent = Event<TraderAsyncErrorData<HedgeCfmActionResponse>>()
    let offsetSettingErrorEvent = Event<TraderAsyncErrorData<InputOffsetSettingResponse>>()
    let optionSelfCloseActionErrorEvent = Event<TraderAsyncErrorData<OptionSelfCloseActionResponse>>()
    let optionSelfCloseInsertErrorEvent = Event<TraderAsyncErrorData<InputOptionSelfCloseResponse>>()
    let orderActionErrorEvent = Event<TraderAsyncErrorData<OrderActionResponse>>()
    let orderInsertErrorEvent = Event<TraderAsyncErrorData<InputOrderResponse>>()
    let queryBankBalanceByFutureErrorEvent = Event<TraderAsyncErrorData<QueryBankAccountMoneyResponse>>()
    let quoteActionErrorEvent = Event<TraderAsyncErrorData<QuoteActionResponse>>()
    let quoteInsertErrorEvent = Event<TraderAsyncErrorData<InputQuoteResponse>>()
    let spdApplyErrorEvent = Event<TraderAsyncErrorData<InputSpdApplyResponse>>()
    let spdApplyActionErrorEvent = Event<TraderAsyncErrorData<SpdApplyActionResponse>>()
    let repealBankToFutureByFutureManualErrorEvent = Event<TraderAsyncErrorData<RepealRequest>>()
    let repealFutureToBankByFutureManualErrorEvent = Event<TraderAsyncErrorData<RepealRequest>>()
    let orderInsertResponseEvent = Event<TraderCommandResponseData<InputOrderResponse>>()
    let orderActionResponseEvent = Event<TraderCommandResponseData<InputOrderActionResponse>>()
    let batchOrderActionResponseEvent = Event<TraderCommandResponseData<InputBatchOrderActionResponse>>()
    let cancelOffsetSettingResponseEvent = Event<TraderCommandResponseData<InputOffsetSettingResponse>>()
    let combActionInsertResponseEvent = Event<TraderCommandResponseData<InputCombActionResponse>>()
    let execOrderActionResponseEvent = Event<TraderCommandResponseData<InputExecOrderActionResponse>>()
    let execOrderInsertResponseEvent = Event<TraderCommandResponseData<InputExecOrderResponse>>()
    let forQuoteInsertResponseEvent = Event<TraderCommandResponseData<InputForQuoteResponse>>()
    let fromBankToFutureByFutureResponseEvent = Event<TraderCommandResponseData<TransferAckResponse>>()
    let fromFutureToBankByFutureResponseEvent = Event<TraderCommandResponseData<TransferAckResponse>>()
    let hedgeCfmResponseEvent = Event<TraderCommandResponseData<InputHedgeCfmResponse>>()
    let hedgeCfmActionResponseEvent = Event<TraderCommandResponseData<InputHedgeCfmActionResponse>>()
    let offsetSettingResponseEvent = Event<TraderCommandResponseData<InputOffsetSettingResponse>>()
    let optionSelfCloseActionResponseEvent = Event<TraderCommandResponseData<InputOptionSelfCloseActionResponse>>()
    let optionSelfCloseInsertResponseEvent = Event<TraderCommandResponseData<InputOptionSelfCloseResponse>>()
    let quoteActionResponseEvent = Event<TraderCommandResponseData<InputQuoteActionResponse>>()
    let quoteInsertResponseEvent = Event<TraderCommandResponseData<InputQuoteResponse>>()
    let spdApplyResponseEvent = Event<TraderCommandResponseData<InputSpdApplyResponse>>()
    let spdApplyActionResponseEvent = Event<TraderCommandResponseData<InputSpdApplyActionResponse>>()
    let api = new TraderApi(options.FlowPath, options.ProductionMode, encodings = bridgeEncodings)
    let requestFlow = FlowController(defaultArg flowControl CtpFlowControlOptions.Default, logger = logger)
    let mutable configuredEndpoint = defaultArg endpoint (CtpEndpoint.Front options.FrontAddress)
    let mutable fensUserInfo =
        match configuredEndpoint with
        | CtpEndpoint.NameServer(_, fens) -> fens
        | CtpEndpoint.Front _ -> None

    let triggerNotification (typedEvent: Event<'T>) (item: 'T) =
        typedEvent.Trigger item
        notificationEvent.Trigger(box item)

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
                api.RegisterFensUserInfo request
                |> BridgeHelpers.throwOnNonZero
                <| "ctp_trader_register_fens_user_info")

    let connectionCoordinator =
        ConnectionCoordinator(
            (fun () ->
                registerEndpoint ()
                api.SubscribePrivateTopic(
                    defaultArg privateTopicResumeType ResumeTypeValidation.privateDefault,
                    defaultArg privateTopicSequenceNo 1
                )

                api.SubscribePublicTopic(defaultArg publicTopicResumeType ResumeTypeValidation.publicDefault)
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
                    let operationName = pending.TryGetOperationName requestId |> Option.defaultValue "CorrelatedRequest"
                    let callbackName = TraderCallbackNames.forOperation operationName

                    commandResponseEvent.Trigger
                        { CallbackName = callbackName
                          OperationName = operationName
                          RequestId = requestId
                          IsLast = isLast
                          RspInfo = rspInfo
                          Payload = response }

                    pending.TryHandleResponse(requestId, response, rspInfo, isLast, completionPolicy)
                | OrderCommandResponse(response, triggerTyped) ->
                    commandResponseEvent.Trigger response
                    triggerTyped ()

                    if response.IsLast then
                        response.RspInfo
                        |> Option.filter (fun info -> info.ErrorId <> 0)
                        |> Option.iter (fun info ->
                            logger.LogError(
                                "{OperationName} failed for request {RequestId}: [{ErrorId}] {ErrorMessage}",
                                response.OperationName,
                                response.RequestId,
                                info.ErrorId,
                                info.ErrorMessage
                            )

                            rspErrorEvent.Trigger info)
                | PushNotification(TraderPushNotification.OrderReceived order) -> orderEvent.Trigger order
                | PushNotification(TraderPushNotification.TradeReceived trade) -> tradeEvent.Trigger trade
                | PushNotification(TraderPushNotification.CombActionReceived item) ->
                    triggerNotification combActionEvent item
                | PushNotification(TraderPushNotification.ExecOrderReceived item) ->
                    triggerNotification execOrderEvent item
                | PushNotification(TraderPushNotification.ForQuoteRspReceived item) ->
                    triggerNotification forQuoteRspEvent item
                | PushNotification(TraderPushNotification.FromBankToFutureByFutureReceived item) ->
                    triggerNotification fromBankToFutureByFutureEvent item
                | PushNotification(TraderPushNotification.FromFutureToBankByFutureReceived item) ->
                    triggerNotification fromFutureToBankByFutureEvent item
                | PushNotification(TraderPushNotification.HedgeCfmReceived item) ->
                    triggerNotification hedgeCfmEvent item
                | PushNotification(TraderPushNotification.OffsetSettingReceived item) ->
                    triggerNotification offsetSettingEvent item
                | PushNotification(TraderPushNotification.OptionSelfCloseReceived item) ->
                    triggerNotification optionSelfCloseEvent item
                | PushNotification(TraderPushNotification.QueryBankBalanceByFutureReceived item) ->
                    triggerNotification queryBankBalanceByFutureEvent item
                | PushNotification(TraderPushNotification.QuoteReceived item) ->
                    triggerNotification quoteEvent item
                | PushNotification(TraderPushNotification.SpdApplyReceived item) ->
                    triggerNotification spdApplyEvent item
                | PushNotification(TraderPushNotification.InstrumentStatusReceived item) ->
                    triggerNotification instrumentStatusEvent item
                | PushNotification(TraderPushNotification.BulletinReceived item) ->
                    triggerNotification bulletinEvent item
                | PushNotification(TraderPushNotification.TradingNoticeReceived item) ->
                    triggerNotification tradingNoticeEvent item
                | PushNotification(TraderPushNotification.ErrorConditionalOrderReceived item) ->
                    triggerNotification errorConditionalOrderEvent item
                | PushNotification(TraderPushNotification.CfmmcTradingAccountTokenReceived item) ->
                    triggerNotification cfmmcTradingAccountTokenEvent item
                | PushNotification(TraderPushNotification.FromBankToFutureByBankReceived item) ->
                    triggerNotification fromBankToFutureByBankEvent item
                | PushNotification(TraderPushNotification.FromFutureToBankByBankReceived item) ->
                    triggerNotification fromFutureToBankByBankEvent item
                | PushNotification(TraderPushNotification.RepealFromBankToFutureByBankReceived item) ->
                    triggerNotification repealFromBankToFutureByBankEvent item
                | PushNotification(TraderPushNotification.RepealFromFutureToBankByBankReceived item) ->
                    triggerNotification repealFromFutureToBankByBankEvent item
                | PushNotification(TraderPushNotification.RepealFromBankToFutureByFutureManualReceived item) ->
                    triggerNotification repealFromBankToFutureByFutureManualEvent item
                | PushNotification(TraderPushNotification.RepealFromFutureToBankByFutureManualReceived item) ->
                    triggerNotification repealFromFutureToBankByFutureManualEvent item
                | PushNotification(TraderPushNotification.RepealFromBankToFutureByFutureReceived item) ->
                    triggerNotification repealFromBankToFutureByFutureEvent item
                | PushNotification(TraderPushNotification.RepealFromFutureToBankByFutureReceived item) ->
                    triggerNotification repealFromFutureToBankByFutureEvent item
                | PushNotification(TraderPushNotification.OpenAccountByBankReceived item) ->
                    triggerNotification openAccountByBankEvent item
                | PushNotification(TraderPushNotification.CancelAccountByBankReceived item) ->
                    triggerNotification cancelAccountByBankEvent item
                | PushNotification(TraderPushNotification.ChangeAccountByBankReceived item) ->
                    triggerNotification changeAccountByBankEvent item
                | AsyncErrorPush(detailed, triggerTyped) ->
                    // Keep the old F# event payload shape: a boxed (payload, rspInfo option) tuple.
                    asyncErrorEvent.Trigger(box (detailed.Payload, detailed.RspInfo))
                    asyncErrorDetailedEvent.Trigger detailed
                    triggerTyped ()
                    detailed.RspInfo |> Option.iter (fun info -> rspErrorEvent.Trigger info)

                return! loop ()
            }

            loop ())

    let postCorrelatedResponse completionPolicy response rsp requestId isLast =
        agent.Post(CorrelatedResponse(completionPolicy, response |> Option.map box, rsp, requestId, isLast))

    let postOrderCommandResponse
        operationName
        (typedEvent: Event<TraderCommandResponseData<'TPayload>>)
        (payload: 'TPayload option)
        rsp
        requestId
        isLast
        =
        let callbackName = TraderCallbackNames.forOperation operationName

        let typed =
            { CallbackName = callbackName
              OperationName = operationName
              RequestId = requestId
              IsLast = isLast
              RspInfo = rsp
              Payload = payload }

        agent.Post(
            OrderCommandResponse(
                { CallbackName = callbackName
                  OperationName = operationName
                  RequestId = requestId
                  IsLast = isLast
                  RspInfo = rsp
                  Payload = payload |> Option.map box },
                fun () -> typedEvent.Trigger typed
            )
        )

    let asyncErrorPayload (item: 'T option) : obj =
        item |> Option.map box |> Option.defaultValue (Unchecked.defaultof<obj>)

    let postAsyncError
        callbackName
        (typedEvent: Event<TraderAsyncErrorData<'TPayload>>)
        (payload: 'TPayload option)
        rsp
        =
        let typed =
            { CallbackName = callbackName
              Payload = payload
              RspInfo = rsp }

        agent.Post(
            AsyncErrorPush(
                { CallbackName = callbackName
                  Payload = asyncErrorPayload payload
                  RspInfo = rsp },
                fun () -> typedEvent.Trigger typed
            )
        )

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
                RspOrderInsert = Some(postOrderCommandResponse "OrderInsert" orderInsertResponseEvent)
                RspOrderAction =
                    Some(postOrderCommandResponse TraderOperationNames.OrderAction orderActionResponseEvent)
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
                RspBatchOrderAction =
                    Some(postOrderCommandResponse TraderOperationNames.BatchOrderAction batchOrderActionResponseEvent)
                RspCancelOffsetSetting =
                    Some(postOrderCommandResponse TraderOperationNames.CancelOffsetSetting cancelOffsetSettingResponseEvent)
                RspCombActionInsert =
                    Some(postOrderCommandResponse "CombActionInsert" combActionInsertResponseEvent)
                RspExecOrderAction =
                    Some(postOrderCommandResponse TraderOperationNames.ExecOrderAction execOrderActionResponseEvent)
                RspExecOrderInsert =
                    Some(postOrderCommandResponse "ExecOrderInsert" execOrderInsertResponseEvent)
                RspForQuoteInsert =
                    Some(postOrderCommandResponse "ForQuoteInsert" forQuoteInsertResponseEvent)
                RspFromBankToFutureByFuture =
                    Some(postOrderCommandResponse "FromBankToFutureByFuture" fromBankToFutureByFutureResponseEvent)
                RspFromFutureToBankByFuture =
                    Some(postOrderCommandResponse "FromFutureToBankByFuture" fromFutureToBankByFutureResponseEvent)
                RspHedgeCfm =
                    Some(postOrderCommandResponse TraderOperationNames.HedgeCfm hedgeCfmResponseEvent)
                RspHedgeCfmAction =
                    Some(postOrderCommandResponse TraderOperationNames.HedgeCfmAction hedgeCfmActionResponseEvent)
                RspOffsetSetting =
                    Some(postOrderCommandResponse TraderOperationNames.OffsetSetting offsetSettingResponseEvent)
                RspOptionSelfCloseAction =
                    Some(
                        postOrderCommandResponse
                            TraderOperationNames.OptionSelfCloseAction
                            optionSelfCloseActionResponseEvent
                    )
                RspOptionSelfCloseInsert =
                    Some(postOrderCommandResponse "OptionSelfCloseInsert" optionSelfCloseInsertResponseEvent)
                RspParkedOrderAction = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspParkedOrderInsert = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspQueryBankAccountMoneyByFuture =
                    Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspQuoteAction =
                    Some(postOrderCommandResponse TraderOperationNames.QuoteAction quoteActionResponseEvent)
                RspQuoteInsert = Some(postOrderCommandResponse "QuoteInsert" quoteInsertResponseEvent)
                RspRemoveParkedOrder = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspRemoveParkedOrderAction = Some(postCorrelatedResponse PendingResponseCompletionPolicy.FinalOnly)
                RspSpdApply =
                    Some(postOrderCommandResponse TraderOperationNames.SpdApply spdApplyResponseEvent)
                RspSpdApplyAction =
                    Some(postOrderCommandResponse TraderOperationNames.SpdApplyAction spdApplyActionResponseEvent)
                RtnCombAction =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.CombActionReceived item)))
                RtnExecOrder =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.ExecOrderReceived item)))
                RtnForQuoteRsp =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.ForQuoteRspReceived item)))
                RtnFromBankToFutureByFuture =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.FromBankToFutureByFutureReceived item)))
                RtnFromFutureToBankByFuture =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.FromFutureToBankByFutureReceived item)))
                RtnHedgeCfm =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.HedgeCfmReceived item)))
                RtnOffsetSetting =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.OffsetSettingReceived item)))
                RtnOptionSelfClose =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.OptionSelfCloseReceived item)))
                RtnQueryBankBalanceByFuture =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.QueryBankBalanceByFutureReceived item)))
                RtnQuote =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.QuoteReceived item)))
                RtnSpdApply =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.SpdApplyReceived item)))
                RtnInstrumentStatus =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.InstrumentStatusReceived item)))
                RtnBulletin =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.BulletinReceived item)))
                RtnTradingNotice =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.TradingNoticeReceived item)))
                RtnErrorConditionalOrder =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.ErrorConditionalOrderReceived item)))
                RtnCfmmcTradingAccountToken =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.CfmmcTradingAccountTokenReceived item)))
                RtnFromBankToFutureByBank =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.FromBankToFutureByBankReceived item)))
                RtnFromFutureToBankByBank =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.FromFutureToBankByBankReceived item)))
                RtnRepealFromBankToFutureByBank =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.RepealFromBankToFutureByBankReceived item)))
                RtnRepealFromFutureToBankByBank =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.RepealFromFutureToBankByBankReceived item)))
                RtnRepealFromBankToFutureByFutureManual =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.RepealFromBankToFutureByFutureManualReceived item)))
                RtnRepealFromFutureToBankByFutureManual =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.RepealFromFutureToBankByFutureManualReceived item)))
                RtnRepealFromBankToFutureByFuture =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.RepealFromBankToFutureByFutureReceived item)))
                RtnRepealFromFutureToBankByFuture =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.RepealFromFutureToBankByFutureReceived item)))
                RtnOpenAccountByBank =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.OpenAccountByBankReceived item)))
                RtnCancelAccountByBank =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.CancelAccountByBankReceived item)))
                RtnChangeAccountByBank =
                    Some(fun item -> agent.Post(PushNotification(TraderPushNotification.ChangeAccountByBankReceived item)))
                ErrRtnRepealBankToFutureByFutureManual =
                    Some(postAsyncError "OnErrRtnRepealBankToFutureByFutureManual" repealBankToFutureByFutureManualErrorEvent)
                ErrRtnRepealFutureToBankByFutureManual =
                    Some(postAsyncError "OnErrRtnRepealFutureToBankByFutureManual" repealFutureToBankByFutureManualErrorEvent)
                ErrRtnBankToFutureByFuture =
                    Some(postAsyncError "OnErrRtnBankToFutureByFuture" bankToFutureByFutureErrorEvent)
                ErrRtnBatchOrderAction =
                    Some(postAsyncError "OnErrRtnBatchOrderAction" batchOrderActionErrorEvent)
                ErrRtnCancelOffsetSetting =
                    Some(postAsyncError "OnErrRtnCancelOffsetSetting" cancelOffsetSettingErrorEvent)
                ErrRtnCombActionInsert =
                    Some(postAsyncError "OnErrRtnCombActionInsert" combActionInsertErrorEvent)
                ErrRtnExecOrderAction =
                    Some(postAsyncError "OnErrRtnExecOrderAction" execOrderActionErrorEvent)
                ErrRtnExecOrderInsert =
                    Some(postAsyncError "OnErrRtnExecOrderInsert" execOrderInsertErrorEvent)
                ErrRtnForQuoteInsert =
                    Some(postAsyncError "OnErrRtnForQuoteInsert" forQuoteInsertErrorEvent)
                ErrRtnFutureToBankByFuture =
                    Some(postAsyncError "OnErrRtnFutureToBankByFuture" futureToBankByFutureErrorEvent)
                ErrRtnHedgeCfm =
                    Some(postAsyncError "OnErrRtnHedgeCfm" hedgeCfmErrorEvent)
                ErrRtnHedgeCfmAction =
                    Some(postAsyncError "OnErrRtnHedgeCfmAction" hedgeCfmActionErrorEvent)
                ErrRtnOffsetSetting =
                    Some(postAsyncError "OnErrRtnOffsetSetting" offsetSettingErrorEvent)
                ErrRtnOptionSelfCloseAction =
                    Some(postAsyncError "OnErrRtnOptionSelfCloseAction" optionSelfCloseActionErrorEvent)
                ErrRtnOptionSelfCloseInsert =
                    Some(postAsyncError "OnErrRtnOptionSelfCloseInsert" optionSelfCloseInsertErrorEvent)
                ErrRtnOrderAction =
                    Some(postAsyncError "OnErrRtnOrderAction" orderActionErrorEvent)
                ErrRtnOrderInsert =
                    Some(postAsyncError "OnErrRtnOrderInsert" orderInsertErrorEvent)
                ErrRtnQueryBankBalanceByFuture =
                    Some(postAsyncError "OnErrRtnQueryBankBalanceByFuture" queryBankBalanceByFutureErrorEvent)
                ErrRtnQuoteAction =
                    Some(postAsyncError "OnErrRtnQuoteAction" quoteActionErrorEvent)
                ErrRtnQuoteInsert =
                    Some(postAsyncError "OnErrRtnQuoteInsert" quoteInsertErrorEvent)
                ErrRtnSpdApply =
                    Some(postAsyncError "OnErrRtnSpdApply" spdApplyErrorEvent)
                ErrRtnSpdApplyAction =
                    Some(postAsyncError "OnErrRtnSpdApplyAction" spdApplyActionErrorEvent) }

    interface IDisposable with
        member _.Dispose() = (api :> IDisposable).Dispose()

    member _.FrontConnected = frontConnectedEvent.Publish
    member _.FrontDisconnected = frontDisconnectedEvent.Publish
    member _.HeartBeatWarning = heartBeatWarningEvent.Publish
    member _.PrivateSeqNoReceived = privateSeqNoEvent.Publish
    member _.RspError = rspErrorEvent.Publish
    member _.OrderReceived = orderEvent.Publish
    member _.TradeReceived = tradeEvent.Publish
    member _.CombActionReceived = combActionEvent.Publish
    member _.ExecOrderReceived = execOrderEvent.Publish
    member _.ForQuoteRspReceived = forQuoteRspEvent.Publish
    member _.FromBankToFutureByFutureReceived = fromBankToFutureByFutureEvent.Publish
    member _.FromFutureToBankByFutureReceived = fromFutureToBankByFutureEvent.Publish
    member _.HedgeCfmReceived = hedgeCfmEvent.Publish
    member _.OffsetSettingReceived = offsetSettingEvent.Publish
    member _.OptionSelfCloseReceived = optionSelfCloseEvent.Publish
    member _.QueryBankBalanceByFutureReceived = queryBankBalanceByFutureEvent.Publish
    member _.QuoteReceived = quoteEvent.Publish
    member _.SpdApplyReceived = spdApplyEvent.Publish
    member _.InstrumentStatusReceived = instrumentStatusEvent.Publish
    member _.BulletinReceived = bulletinEvent.Publish
    member _.TradingNoticeReceived = tradingNoticeEvent.Publish
    member _.ErrorConditionalOrderReceived = errorConditionalOrderEvent.Publish
    member _.CfmmcTradingAccountTokenReceived = cfmmcTradingAccountTokenEvent.Publish
    member _.FromBankToFutureByBankReceived = fromBankToFutureByBankEvent.Publish
    member _.FromFutureToBankByBankReceived = fromFutureToBankByBankEvent.Publish
    member _.RepealFromBankToFutureByBankReceived = repealFromBankToFutureByBankEvent.Publish
    member _.RepealFromFutureToBankByBankReceived = repealFromFutureToBankByBankEvent.Publish
    member _.RepealFromBankToFutureByFutureManualReceived = repealFromBankToFutureByFutureManualEvent.Publish
    member _.RepealFromFutureToBankByFutureManualReceived = repealFromFutureToBankByFutureManualEvent.Publish
    member _.RepealFromBankToFutureByFutureReceived = repealFromBankToFutureByFutureEvent.Publish
    member _.RepealFromFutureToBankByFutureReceived = repealFromFutureToBankByFutureEvent.Publish
    member _.OpenAccountByBankReceived = openAccountByBankEvent.Publish
    member _.CancelAccountByBankReceived = cancelAccountByBankEvent.Publish
    member _.ChangeAccountByBankReceived = changeAccountByBankEvent.Publish

    member _.BankToFutureByFutureErrorReceived = bankToFutureByFutureErrorEvent.Publish
    member _.BatchOrderActionErrorReceived = batchOrderActionErrorEvent.Publish
    member _.CancelOffsetSettingErrorReceived = cancelOffsetSettingErrorEvent.Publish
    member _.CombActionInsertErrorReceived = combActionInsertErrorEvent.Publish
    member _.ExecOrderActionErrorReceived = execOrderActionErrorEvent.Publish
    member _.ExecOrderInsertErrorReceived = execOrderInsertErrorEvent.Publish
    member _.ForQuoteInsertErrorReceived = forQuoteInsertErrorEvent.Publish
    member _.FutureToBankByFutureErrorReceived = futureToBankByFutureErrorEvent.Publish
    member _.HedgeCfmErrorReceived = hedgeCfmErrorEvent.Publish
    member _.HedgeCfmActionErrorReceived = hedgeCfmActionErrorEvent.Publish
    member _.OffsetSettingErrorReceived = offsetSettingErrorEvent.Publish
    member _.OptionSelfCloseActionErrorReceived = optionSelfCloseActionErrorEvent.Publish
    member _.OptionSelfCloseInsertErrorReceived = optionSelfCloseInsertErrorEvent.Publish
    member _.OrderActionErrorReceived = orderActionErrorEvent.Publish
    member _.OrderInsertErrorReceived = orderInsertErrorEvent.Publish
    member _.QueryBankBalanceByFutureErrorReceived = queryBankBalanceByFutureErrorEvent.Publish
    member _.QuoteActionErrorReceived = quoteActionErrorEvent.Publish
    member _.QuoteInsertErrorReceived = quoteInsertErrorEvent.Publish
    member _.SpdApplyErrorReceived = spdApplyErrorEvent.Publish
    member _.SpdApplyActionErrorReceived = spdApplyActionErrorEvent.Publish
    member _.RepealBankToFutureByFutureManualErrorReceived = repealBankToFutureByFutureManualErrorEvent.Publish
    member _.RepealFutureToBankByFutureManualErrorReceived = repealFutureToBankByFutureManualErrorEvent.Publish

    member _.OrderInsertResponseReceived = orderInsertResponseEvent.Publish
    member _.OrderActionResponseReceived = orderActionResponseEvent.Publish
    member _.BatchOrderActionResponseReceived = batchOrderActionResponseEvent.Publish
    member _.CancelOffsetSettingResponseReceived = cancelOffsetSettingResponseEvent.Publish
    member _.CombActionInsertResponseReceived = combActionInsertResponseEvent.Publish
    member _.ExecOrderActionResponseReceived = execOrderActionResponseEvent.Publish
    member _.ExecOrderInsertResponseReceived = execOrderInsertResponseEvent.Publish
    member _.ForQuoteInsertResponseReceived = forQuoteInsertResponseEvent.Publish
    member _.FromBankToFutureByFutureResponseReceived = fromBankToFutureByFutureResponseEvent.Publish
    member _.FromFutureToBankByFutureResponseReceived = fromFutureToBankByFutureResponseEvent.Publish
    member _.HedgeCfmResponseReceived = hedgeCfmResponseEvent.Publish
    member _.HedgeCfmActionResponseReceived = hedgeCfmActionResponseEvent.Publish
    member _.OffsetSettingResponseReceived = offsetSettingResponseEvent.Publish
    member _.OptionSelfCloseActionResponseReceived = optionSelfCloseActionResponseEvent.Publish
    member _.OptionSelfCloseInsertResponseReceived = optionSelfCloseInsertResponseEvent.Publish
    member _.QuoteActionResponseReceived = quoteActionResponseEvent.Publish
    member _.QuoteInsertResponseReceived = quoteInsertResponseEvent.Publish
    member _.SpdApplyResponseReceived = spdApplyResponseEvent.Publish
    member _.SpdApplyActionResponseReceived = spdApplyActionResponseEvent.Publish

    [<Obsolete("Use the callback-specific strongly typed notification events.")>]
    member _.NotificationReceived = notificationEvent.Publish

    [<Obsolete("Use the callback-specific strongly typed error events.")>]
    member _.AsyncErrorReceived = asyncErrorEvent.Publish
    [<Obsolete("Use the callback-specific strongly typed error events.")>]
    member _.AsyncErrorDetailedReceived = asyncErrorDetailedEvent.Publish
    [<Obsolete("Use the callback-specific strongly typed command response events.")>]
    member _.CommandResponseReceived = commandResponseEvent.Publish

    member _.Connect(?timeout: TimeSpan) =
        match timeout with
        | Some timeout -> connectionCoordinator.Connect(timeout = timeout)
        | None -> connectionCoordinator.Connect()

    member _.Join() = api.Join()

    member private _.RunPendingRequestAsync<'TResponse, 'TRequest>
        (operationName: string)
        (request: 'TRequest)
        (apiCall: 'TRequest * int -> int)
        (onAccepted: int -> unit)
        : Async<Result<'TResponse list, RspInfo>>
        =
        async {
            let! cancellationToken = Async.CancellationToken

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

    member private _.RunCommandTryAsync
        (operationName: string)
        (apiCall: int -> int)
        : Async<Result<int, RspInfo>>
        =
        CommandDispatch.runAsync operationName nextRequestId requestFlow logger apiCall

    member private this.RunCommandAsync
        (operationName: string)
        (apiCall: int -> int)
        : Async<int>
        =
        async {
            let! result = this.RunCommandTryAsync operationName apiCall

            match result with
            | Ok requestId -> return requestId
            | Error info -> return raise (NativeRequestException(operationName, info.ErrorId))
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

    member this.AuthenticateAsync() =
        let request = OptionHelpers.createAuthenticateRequest options

        this.RunPendingRequestAsync<AuthenticateResponse, AuthenticateRequest>
            "Authenticate"
            request
            api.ReqAuthenticate
            ignore
           

    member this.SettlementInfoConfirmAsync() =
        let request = OptionHelpers.createSettlementInfoConfirmRequest options

        this.RunPendingRequestAsync<SettlementInfoConfirmResponse, SettlementInfoConfirmRequest>
            "SettlementInfoConfirm"
            request
            api.ReqSettlementInfoConfirm
            ignore
           

    member this.LoginAsync() =
        let request = OptionHelpers.createUserLoginRequest options

        this.RunPendingRequestAsync<UserLoginResponse, UserLoginRequest>
            "Login"
            request
            api.ReqUserLogin
            ignore
           

    member this.LogoutAsync() =
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
           

    member this.QueryTradingAccountAsync
        (?currencyId: string, ?bizType: BizType, ?accountId: string)
        =
        let request =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              CurrencyId = defaultArg currencyId ""
              BizType = bizType
              AccountId = accountId }

        this.QueryAsync<TradingAccountResponse, QueryTradingAccountRequest>
            (nameof QueryTradingAccountRequest)
            request
            api.ReqQryTradingAccount
           

    member this.QueryInvestorPositionAsync
        (instrumentId: string, ?exchangeId: string, ?investUnitId: string)
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
           

    member this.QueryInstrumentMarginRateAsync
        (
            hedgeFlag: HedgeFlag,
            instrumentId: string,
            ?exchangeId: string,
            ?investUnitId: string
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
           

    member this.QueryExchangeMarginRateAsync
        (hedgeFlag: HedgeFlag, instrumentId: string, ?exchangeId: string)
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
           

    member this.QueryInstrumentCommissionRateAsync
        (instrumentId: string, ?exchangeId: string, ?investUnitId: string)
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
           

    // ---- Utility / diagnostic methods ----

    member _.GetApiVersion() = TraderApi.GetApiVersion()

    member _.GetTradingDay() = api.GetTradingDay()

    member _.GetFrontInfo() = api.GetFrontInfo()

    // ---- Connection configuration (call before Connect) ----

    member _.RegisterNameServer(nsAddress: string) =
        if String.IsNullOrWhiteSpace nsAddress then
            invalidArg (nameof nsAddress) "NameServer address must not be empty."

        api.RegisterNameServer(nsAddress)
        configuredEndpoint <- CtpEndpoint.NameServer(nsAddress, fensUserInfo)

    member _.RegisterFensUserInfo(request: FensUserInfoRequest) =
        validateFensUserInfo request
        let result = api.RegisterFensUserInfo request

        if result = 0 then
            fensUserInfo <- Some request

        result

    // ---- Regulatory / system-info methods ----

    member _.RegisterUserSystemInfo(info: UserSystemInfoRequest) = api.RegisterUserSystemInfo(info)

    member _.SubmitUserSystemInfo(info: UserSystemInfoRequest) = api.SubmitUserSystemInfo(info)

    member _.RegisterWechatUserSystemInfo(info: WechatUserSystemInfoRequest) = api.RegisterWechatUserSystemInfo(info)

    member _.SubmitWechatUserSystemInfo(info: WechatUserSystemInfoRequest) = api.SubmitWechatUserSystemInfo(info)

    // ---- Auth / password / captcha methods ----

    member this.ReqUserPasswordUpdateAsync
        (oldPassword: string, newPassword: string)
        =
        let request: UserPasswordUpdateRequest =
            { BrokerId = options.BrokerId
              UserId = options.UserId
              OldPassword = oldPassword
              NewPassword = newPassword }

        this.RunPendingRequestAsync<UserPasswordUpdateResponse, UserPasswordUpdateRequest>
            "UserPasswordUpdate"
            request
            api.ReqUserPasswordUpdate
            ignore
           

    member this.ReqTradingAccountPasswordUpdateAsync
        (
            accountId: string,
            oldPassword: string,
            newPassword: string,
            currencyId: string
                    )
        =
        let request: TradingAccountPasswordUpdateRequest =
            { BrokerId = options.BrokerId
              AccountId = accountId
              OldPassword = oldPassword
              NewPassword = newPassword
              CurrencyId = currencyId }

        this.RunPendingRequestAsync<TradingAccountPasswordUpdateResponse, TradingAccountPasswordUpdateRequest>
            "TradingAccountPasswordUpdate"
            request
            api.ReqTradingAccountPasswordUpdate
            ignore
           

    member this.ReqUserAuthMethodAsync(?tradingDay: DateOnly) =
        let request: UserAuthMethodRequest =
            { BrokerId = Some options.BrokerId
              UserId = Some options.UserId
              TradingDay = tradingDay }

        this.RunPendingRequestAsync<UserAuthMethodResponse, UserAuthMethodRequest>
            "UserAuthMethod"
            request
            api.ReqUserAuthMethod
            ignore
           

    member this.ReqGenUserCaptchaAsync(?tradingDay: DateOnly) =
        let request: GenUserCaptchaRequest =
            { BrokerId = Some options.BrokerId
              UserId = Some options.UserId
              TradingDay = tradingDay }

        this.RunPendingRequestAsync<GenUserCaptchaResponse, GenUserCaptchaRequest>
            "GenUserCaptcha"
            request
            api.ReqGenUserCaptcha
            ignore
           

    member this.ReqGenUserTextAsync(?tradingDay: DateOnly) =
        let request: GenUserTextRequest =
            { BrokerId = Some options.BrokerId
              UserId = Some options.UserId
              TradingDay = tradingDay }

        this.RunPendingRequestAsync<GenUserTextResponse, GenUserTextRequest>
            "GenUserText"
            request
            api.ReqGenUserText
            ignore
           

    member this.ReqUserLoginWithCaptchaAsync
        (
            captcha: string,
            ?tradingDay: DateOnly,
            ?userProductInfo: string,
            ?interfaceProductInfo: string,
            ?protocolInfo: string,
            ?macAddress: string,
            ?loginRemark: string,
            ?clientIpPort: int,
            ?clientIpAddress: string
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
           

    member this.ReqUserLoginWithTextAsync
        (
            text: string,
            ?tradingDay: DateOnly,
            ?userProductInfo: string,
            ?interfaceProductInfo: string,
            ?protocolInfo: string,
            ?macAddress: string,
            ?loginRemark: string,
            ?clientIpPort: int,
            ?clientIpAddress: string
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
           

    member this.ReqUserLoginWithOtpAsync
        (
            otpPassword: string,
            ?tradingDay: DateOnly,
            ?userProductInfo: string,
            ?interfaceProductInfo: string,
            ?protocolInfo: string,
            ?macAddress: string,
            ?loginRemark: string,
            ?clientIpPort: int,
            ?clientIpAddress: string
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
           

    member this.ReqGenSmsCodeAsync(mobile: string) =
        let request: GenSmsCodeRequest =
            { BrokerId = options.BrokerId; UserId = options.UserId; Mobile = mobile }

        this.RunPendingRequestAsync<GenSmsCodeResponse, GenSmsCodeRequest>
            "GenSmsCode"
            request
            api.ReqGenSmsCode
            ignore
           

    // ---- Order insertion and action ----

    member this.TryInsertOrderAsync(request: InputOrderRequest) =
        this.RunCommandTryAsync "OrderInsert" (fun requestId -> api.ReqOrderInsert(request, requestId))

    member this.InsertOrderAsync(request: InputOrderRequest) =
        this.RunCommandAsync "OrderInsert" (fun requestId -> api.ReqOrderInsert(request, requestId))

    member this.TryCancelOrderAsync(request: InputOrderActionRequest) =
        this.RunCommandTryAsync
            TraderOperationNames.OrderAction
            (fun requestId -> api.ReqOrderAction(request, requestId))

    member this.CancelOrderAsync(request: InputOrderActionRequest) =
        this.RunCommandAsync
            TraderOperationNames.OrderAction
            (fun requestId -> api.ReqOrderAction(request, requestId))

    // ---- Execution / quote / hedge / combination command methods ----

    member this.TryReqExecOrderInsertAsync(request: InputExecOrderRequest) =
        this.RunCommandTryAsync
            "ExecOrderInsert"
            (fun requestId -> api.ReqExecOrderInsert(request, requestId))

    member this.ReqExecOrderInsertAsync(request: InputExecOrderRequest) =
        this.RunCommandAsync
            "ExecOrderInsert"
            (fun requestId -> api.ReqExecOrderInsert(request, requestId))

    member this.TryReqExecOrderActionAsync(request: InputExecOrderActionRequest) =
        this.RunCommandTryAsync
            TraderOperationNames.ExecOrderAction
            (fun requestId -> api.ReqExecOrderAction(request, requestId))

    member this.ReqExecOrderActionAsync(request: InputExecOrderActionRequest) =
        this.RunCommandAsync
            TraderOperationNames.ExecOrderAction
            (fun requestId -> api.ReqExecOrderAction(request, requestId))

    member this.ReqForQuoteInsertAsync
        (instrumentId: string, ?exchangeId: string)
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

    member this.TryReqForQuoteInsertAsync
        (instrumentId: string, ?exchangeId: string)
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

        this.RunCommandTryAsync
            "ForQuoteInsert"
            (fun requestId -> api.ReqForQuoteInsert(request, requestId))

    member this.TryReqQuoteInsertAsync(request: InputQuoteRequest) =
        this.RunCommandTryAsync "QuoteInsert" (fun requestId -> api.ReqQuoteInsert(request, requestId))

    member this.ReqQuoteInsertAsync(request: InputQuoteRequest) =
        this.RunCommandAsync "QuoteInsert" (fun requestId -> api.ReqQuoteInsert(request, requestId))

    member this.TryReqQuoteActionAsync(request: InputQuoteActionRequest) =
        this.RunCommandTryAsync
            TraderOperationNames.QuoteAction
            (fun requestId -> api.ReqQuoteAction(request, requestId))

    member this.ReqQuoteActionAsync(request: InputQuoteActionRequest) =
        this.RunCommandAsync
            TraderOperationNames.QuoteAction
            (fun requestId -> api.ReqQuoteAction(request, requestId))

    member this.ReqBatchOrderActionAsync
        (frontId: int, sessionId: int, ?exchangeId: string)
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
            TraderOperationNames.BatchOrderAction
            (fun requestId -> api.ReqBatchOrderAction(request, requestId))

    member this.TryReqBatchOrderActionAsync
        (frontId: int, sessionId: int, ?exchangeId: string)
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

        this.RunCommandTryAsync
            TraderOperationNames.BatchOrderAction
            (fun requestId -> api.ReqBatchOrderAction(request, requestId))

    member this.TryReqOptionSelfCloseInsertAsync(request: InputOptionSelfCloseRequest) =
        this.RunCommandTryAsync
            "OptionSelfCloseInsert"
            (fun requestId -> api.ReqOptionSelfCloseInsert(request, requestId))

    member this.ReqOptionSelfCloseInsertAsync
        (request: InputOptionSelfCloseRequest)
        =
        this.RunCommandAsync
            "OptionSelfCloseInsert"
            (fun requestId -> api.ReqOptionSelfCloseInsert(request, requestId))

    member this.TryReqOptionSelfCloseActionAsync(request: InputOptionSelfCloseActionRequest) =
        this.RunCommandTryAsync
            TraderOperationNames.OptionSelfCloseAction
            (fun requestId -> api.ReqOptionSelfCloseAction(request, requestId))

    member this.ReqOptionSelfCloseActionAsync
        (request: InputOptionSelfCloseActionRequest)
        =
        this.RunCommandAsync
            TraderOperationNames.OptionSelfCloseAction
            (fun requestId -> api.ReqOptionSelfCloseAction(request, requestId))

    member this.TryReqCombActionInsertAsync(request: InputCombActionRequest) =
        this.RunCommandTryAsync
            "CombActionInsert"
            (fun requestId -> api.ReqCombActionInsert(request, requestId))

    member this.ReqCombActionInsertAsync(request: InputCombActionRequest) =
        this.RunCommandAsync
            "CombActionInsert"
            (fun requestId -> api.ReqCombActionInsert(request, requestId))

    member this.TryReqOffsetSettingAsync(request: InputOffsetSettingRequest) =
        this.RunCommandTryAsync
            TraderOperationNames.OffsetSetting
            (fun requestId -> api.ReqOffsetSetting(request, requestId))

    member this.ReqOffsetSettingAsync(request: InputOffsetSettingRequest) =
        this.RunCommandAsync
            TraderOperationNames.OffsetSetting
            (fun requestId -> api.ReqOffsetSetting(request, requestId))

    member this.TryReqCancelOffsetSettingAsync(request: InputOffsetSettingRequest) =
        this.RunCommandTryAsync
            TraderOperationNames.CancelOffsetSetting
            (fun requestId -> api.ReqCancelOffsetSetting(request, requestId))

    member this.ReqCancelOffsetSettingAsync(request: InputOffsetSettingRequest) =
        this.RunCommandAsync
            TraderOperationNames.CancelOffsetSetting
            (fun requestId -> api.ReqCancelOffsetSetting(request, requestId))

    member this.TryReqSpdApplyAsync(request: InputSpdApplyRequest) =
        this.RunCommandTryAsync TraderOperationNames.SpdApply (fun requestId -> api.ReqSpdApply(request, requestId))

    member this.ReqSpdApplyAsync(request: InputSpdApplyRequest) =
        this.RunCommandAsync TraderOperationNames.SpdApply (fun requestId -> api.ReqSpdApply(request, requestId))

    member this.TryReqSpdApplyActionAsync(request: InputSpdApplyActionRequest) =
        this.RunCommandTryAsync
            TraderOperationNames.SpdApplyAction
            (fun requestId -> api.ReqSpdApplyAction(request, requestId))

    member this.ReqSpdApplyActionAsync(request: InputSpdApplyActionRequest) =
        this.RunCommandAsync
            TraderOperationNames.SpdApplyAction
            (fun requestId -> api.ReqSpdApplyAction(request, requestId))
           

    member this.TryReqHedgeCfmAsync(request: InputHedgeCfmRequest) =
        this.RunCommandTryAsync TraderOperationNames.HedgeCfm (fun requestId -> api.ReqHedgeCfm(request, requestId))

    member this.ReqHedgeCfmAsync(request: InputHedgeCfmRequest) =
        this.RunCommandAsync TraderOperationNames.HedgeCfm (fun requestId -> api.ReqHedgeCfm(request, requestId))

    member this.TryReqHedgeCfmActionAsync(request: InputHedgeCfmActionRequest) =
        this.RunCommandTryAsync
            TraderOperationNames.HedgeCfmAction
            (fun requestId -> api.ReqHedgeCfmAction(request, requestId))

    member this.ReqHedgeCfmActionAsync(request: InputHedgeCfmActionRequest) =
        this.RunCommandAsync
            TraderOperationNames.HedgeCfmAction
            (fun requestId -> api.ReqHedgeCfmAction(request, requestId))
           

    // ---- Parked order methods (FinalOnly) ----

    member this.ReqParkedOrderInsertAsync(request: ParkedOrderRequest) =
        this.RunPendingRequestAsync<ParkedOrderResponse, ParkedOrderRequest>
            "ParkedOrderInsert"
            request
            api.ReqParkedOrderInsert
            ignore
           

    member this.ReqParkedOrderActionAsync(request: ParkedOrderActionRequest) =
        this.RunPendingRequestAsync<ParkedOrderActionResponse, ParkedOrderActionRequest>
            "ParkedOrderAction"
            request
            api.ReqParkedOrderAction
            ignore
           

    member this.ReqRemoveParkedOrderAsync
        (parkedOrderId: string, ?investUnitId: string)
        =
        let request: RemoveParkedOrderRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              ParkedOrderId = parkedOrderId
              InvestUnitId = investUnitId }

        this.RunPendingRequestAsync<RemoveParkedOrderResponse, RemoveParkedOrderRequest>
            "RemoveParkedOrder"
            request
            api.ReqRemoveParkedOrder
            ignore
           

    member this.ReqRemoveParkedOrderActionAsync
        (parkedOrderActionId: string, ?investUnitId: string)
        =
        let request: RemoveParkedOrderActionRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              ParkedOrderActionId = parkedOrderActionId
              InvestUnitId = investUnitId }

        this.RunPendingRequestAsync<RemoveParkedOrderActionResponse, RemoveParkedOrderActionRequest>
            "RemoveParkedOrderAction"
            request
            api.ReqRemoveParkedOrderAction
            ignore
           

    // ---- Bank transfer methods ----

    member this.TryFromBankToFutureByFutureAsync(request: TransferRequest) =
        this.RunCommandTryAsync
            "FromBankToFutureByFuture"
            (fun requestId -> api.ReqFromBankToFutureByFuture(request, requestId))

    member this.FromBankToFutureByFutureAsync(request: TransferRequest) =
        this.RunCommandAsync
            "FromBankToFutureByFuture"
            (fun requestId -> api.ReqFromBankToFutureByFuture(request, requestId))
           

    member this.TryFromFutureToBankByFutureAsync(request: TransferRequest) =
        this.RunCommandTryAsync
            "FromFutureToBankByFuture"
            (fun requestId -> api.ReqFromFutureToBankByFuture(request, requestId))

    member this.FromFutureToBankByFutureAsync(request: TransferRequest) =
        this.RunCommandAsync
            "FromFutureToBankByFuture"
            (fun requestId -> api.ReqFromFutureToBankByFuture(request, requestId))
           

    member this.QueryBankAccountMoneyByFutureAsync
        (request: QueryBankAccountMoneyRequest)
        =
        this.RunPendingRequestAsync<QueryBankAccountMoneyResponse, QueryBankAccountMoneyRequest>
            "QueryBankAccountMoneyByFuture"
            request
            api.ReqQueryBankAccountMoneyByFuture
            ignore
           

    // ---- Query methods ----

    member this.QueryMaxOrderVolumeAsync
        (
            direction: Direction,
            offsetFlag: OffsetFlag,
            hedgeFlag: HedgeFlag,
            instrumentId: string,
            ?maxVolume: int,
            ?exchangeId: string,
            ?investUnitId: string
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

        this.QueryAsync<MaxOrderVolumeResponse, QryMaxOrderVolumeRequest>
            (nameof QryMaxOrderVolumeRequest)
            request
            api.ReqQryMaxOrderVolume
           

    member this.QueryOrderAsync
        (
            exchangeId: string,
            orderSysId: string,
            insertTimeStart: string,
            insertTimeEnd: string,
            instrumentId: string,
            ?investUnitId: string
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

        this.QueryAsync<OrderUpdateResponse, QryOrderRequest>
            (nameof QryOrderRequest)
            request
            api.ReqQryOrder
           

    member this.QueryTradeAsync
        (
            ?exchangeId: string,
            ?tradeId: string,
            ?tradeTimeStart: string,
            ?tradeTimeEnd: string,
            ?investUnitId: string,
            ?instrumentId: string
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

        this.QueryAsync<TradeUpdateResponse, QryTradeRequest>
            (nameof QryTradeRequest)
            request
            api.ReqQryTrade
           

    member this.QueryInvestorAsync() =
        let request: QryInvestorRequest = { BrokerId = options.BrokerId; InvestorId = options.UserId }

        this.QueryAsync<InvestorResponse, QryInvestorRequest>
            (nameof QryInvestorRequest)
            request
            api.ReqQryInvestor
           

    member this.QueryTradingCodeAsync
        (
            exchangeId: string,
            clientId: string,
            clientIdType: ClientIdType,
            ?investUnitId: string
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
           

    member this.QueryUserSessionAsync(frontId: int, sessionId: int) =
        let request: QryUserSessionRequest =
            { FrontId = frontId
              SessionId = sessionId
              BrokerId = options.BrokerId
              UserId = options.UserId }

        this.QueryAsync<UserSessionResponse, QryUserSessionRequest>
            (nameof QryUserSessionRequest)
            request
            api.ReqQryUserSession
           

    member this.QueryExchangeAsync(exchangeId: string) =
        let request: QryExchangeRequest = { ExchangeId = exchangeId }

        this.QueryAsync<ExchangeResponse, QryExchangeRequest>
            (nameof QryExchangeRequest)
            request
            api.ReqQryExchange
           

    member this.QueryProductAsync
        (productId: string, ?productClass: ProductClass, ?exchangeId: string)
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
           

    member this.QueryInstrumentAsync
        (
            exchangeId: string,
            instrumentId: string,
            exchangeInstId: string,
            productId: string
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
           

    member this.QueryDepthMarketDataAsync
        (instrumentId: string, productClass: ProductClass, ?exchangeId: string)
        =
        let request: QryDepthMarketDataRequest =
            { Reserve1 = None
              ExchangeId = exchangeId
              InstrumentId = instrumentId
              ProductClass = productClass }

        this.QueryAsync<DepthMarketData, QryDepthMarketDataRequest>
            (nameof QryDepthMarketDataRequest)
            request
            api.ReqQryDepthMarketData
           

    member this.QueryTraderOfferAsync
        (?exchangeId: string, ?participantId: string, ?traderId: string)
        =
        let request: QryTraderOfferRequest =
            { ExchangeId = exchangeId; ParticipantId = participantId; TraderId = traderId }

        this.QueryAsync<TraderOfferResponse, QryTraderOfferRequest>
            (nameof QryTraderOfferRequest)
            request
            api.ReqQryTraderOffer
           

    member this.QuerySettlementInfoAsync
        (tradingDay: DateOnly, ?accountId: string, ?currencyId: string)
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
           

    member this.QueryTransferBankAsync(?bankId: string, ?bankBrchId: string) =
        let request: QryTransferBankRequest = { BankId = bankId; BankBrchId = bankBrchId }

        this.QueryAsync<TransferBankResponse, QryTransferBankRequest>
            (nameof QryTransferBankRequest)
            request
            api.ReqQryTransferBank
           

    member this.QueryInvestorPositionDetailAsync
        (instrumentId: string, ?exchangeId: string, ?investUnitId: string)
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
           

    member this.QueryNoticeAsync() =
        let request: QryNoticeRequest = { BrokerId = options.BrokerId }

        this.QueryAsync<NoticeResponse, QryNoticeRequest>
            (nameof QryNoticeRequest)
            request
            api.ReqQryNotice
           

    member this.QuerySettlementInfoConfirmAsync
        (?accountId: string, ?currencyId: string)
        =
        let request: QrySettlementInfoConfirmRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              AccountId = accountId
              CurrencyId = currencyId }

        this.QueryAsync<SettlementInfoConfirmResponse, QrySettlementInfoConfirmRequest>
            (nameof QrySettlementInfoConfirmRequest)
            request
            api.ReqQrySettlementInfoConfirm
           

    member this.QueryInvestorPositionCombineDetailAsync
        (combInstrumentId: string, ?exchangeId: string, ?investUnitId: string)
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
           

    member this.QueryCfmmcTradingAccountKeyAsync() =
        let request: QryCfmmcTradingAccountKeyRequest =
            { BrokerId = Some options.BrokerId; InvestorId = Some options.UserId }

        this.QueryAsync<CfmmcTradingAccountKeyResponse, QryCfmmcTradingAccountKeyRequest>
            (nameof QryCfmmcTradingAccountKeyRequest)
            request
            api.ReqQryCfmmcTradingAccountKey
           

    member this.QueryEWarrantOffsetAsync
        (exchangeId: string, instrumentId: string, ?investUnitId: string)
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
           

    member this.QueryInvestorProductGroupMarginAsync
        (
            productGroupId: string,
            ?hedgeFlag: HedgeFlag,
            ?exchangeId: string,
            ?investUnitId: string
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
           

    member this.QueryExchangeMarginRateAdjustAsync
        (instrumentId: string, hedgeFlag: HedgeFlag)
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
           

    member this.QueryExchangeRateAsync
        (fromCurrencyId: string, toCurrencyId: string)
        =
        let request: QryExchangeRateRequest =
            { BrokerId = options.BrokerId
              FromCurrencyId = fromCurrencyId
              ToCurrencyId = toCurrencyId }

        this.QueryAsync<ExchangeRateResponse, QryExchangeRateRequest>
            (nameof QryExchangeRateRequest)
            request
            api.ReqQryExchangeRate
           

    member this.QuerySecAgentAcIdMapAsync
        (accountId: string, currencyId: string)
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
           

    member this.QueryProductExchRateAsync
        (productId: string, ?exchangeId: string)
        =
        let request: QryProductExchRateRequest =
            { Reserve1 = None; ExchangeId = exchangeId; ProductId = productId }

        this.QueryAsync<ProductExchRateResponse, QryProductExchRateRequest>
            (nameof QryProductExchRateRequest)
            request
            api.ReqQryProductExchRate
           

    member this.QueryProductGroupAsync(exchangeId: string, productId: string) =
        let request: QryProductGroupRequest =
            { Reserve1 = None; ExchangeId = exchangeId; ProductId = productId }

        this.QueryAsync<ProductGroupResponse, QryProductGroupRequest>
            (nameof QryProductGroupRequest)
            request
            api.ReqQryProductGroup
           

    member this.QueryMmInstrumentCommissionRateAsync(instrumentId: string) =
        let request: QryMmInstrumentCommissionRateRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              InstrumentId = instrumentId }

        this.QueryAsync<MmInstrumentCommissionRateResponse, QryMmInstrumentCommissionRateRequest>
            (nameof QryMmInstrumentCommissionRateRequest)
            request
            api.ReqQryMmInstrumentCommissionRate
           

    member this.QueryMmOptionInstrCommRateAsync(instrumentId: string) =
        let request: QryMmOptionInstrCommRateRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              InstrumentId = instrumentId }

        this.QueryAsync<MmOptionInstrCommRateResponse, QryMmOptionInstrCommRateRequest>
            (nameof QryMmOptionInstrCommRateRequest)
            request
            api.ReqQryMmOptionInstrCommRate
           

    member this.QueryInstrumentOrderCommRateAsync(instrumentId: string) =
        let request: QryInstrumentOrderCommRateRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              InstrumentId = instrumentId }

        this.QueryAsync<InstrumentOrderCommRateResponse, QryInstrumentOrderCommRateRequest>
            (nameof QryInstrumentOrderCommRateRequest)
            request
            api.ReqQryInstrumentOrderCommRate
           

    member this.QuerySecAgentTradingAccountAsync
        (currencyId: string, ?bizType: BizType, ?accountId: string)
        =
        let request: QueryTradingAccountRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              CurrencyId = currencyId
              BizType = bizType
              AccountId = accountId }

        this.QueryAsync<TradingAccountResponse, QueryTradingAccountRequest>
            "QuerySecAgentTradingAccountRequest"
            request
            api.ReqQrySecAgentTradingAccount
           

    member this.QuerySecAgentCheckModeAsync() =
        let request: QrySecAgentCheckModeRequest = { BrokerId = options.BrokerId; InvestorId = options.UserId }

        this.QueryAsync<SecAgentCheckModeResponse, QrySecAgentCheckModeRequest>
            (nameof QrySecAgentCheckModeRequest)
            request
            api.ReqQrySecAgentCheckMode
           

    member this.QuerySecAgentTradeInfoAsync(brokerSecAgentId: string) =
        let request: QrySecAgentTradeInfoRequest =
            { BrokerId = options.BrokerId; BrokerSecAgentId = brokerSecAgentId }

        this.QueryAsync<SecAgentTradeInfoResponse, QrySecAgentTradeInfoRequest>
            (nameof QrySecAgentTradeInfoRequest)
            request
            api.ReqQrySecAgentTradeInfo
           

    member this.QueryOptionInstrTradeCostAsync
        (
            instrumentId: string,
            hedgeFlag: HedgeFlag,
            inputPrice: decimal,
            underlyingPrice: decimal,
            ?exchangeId: string,
            ?investUnitId: string
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
           

    member this.QueryOptionInstrCommRateAsync
        (instrumentId: string, ?exchangeId: string, ?investUnitId: string)
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
           

    member this.QueryExecOrderAsync
        (
            exchangeId: string,
            execOrderSysId: string,
            insertTimeStart: string,
            insertTimeEnd: string,
            instrumentId: string
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
           

    member this.QueryForQuoteAsync
        (
            exchangeId: string,
            insertTimeStart: string,
            insertTimeEnd: string,
            instrumentId: string,
            ?investUnitId: string
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
           

    member this.QueryQuoteAsync
        (
            exchangeId: string,
            quoteSysId: string,
            insertTimeStart: string,
            insertTimeEnd: string,
            instrumentId: string,
            ?investUnitId: string
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

        this.QueryAsync<QuoteResponse, QryQuoteRequest>
            (nameof QryQuoteRequest)
            request
            api.ReqQryQuote
           

    member this.QueryOptionSelfCloseAsync
        (
            exchangeId: string,
            optionSelfCloseSysId: string,
            insertTimeStart: string,
            insertTimeEnd: string,
            instrumentId: string
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
           

    member this.QueryInvestUnitAsync(?investUnitId: string) =
        let request: QryInvestUnitRequest =
            { BrokerId = Some options.BrokerId
              InvestorId = Some options.UserId
              InvestUnitId = investUnitId }

        this.QueryAsync<InvestUnitResponse, QryInvestUnitRequest>
            (nameof QryInvestUnitRequest)
            request
            api.ReqQryInvestUnit
           

    member this.QueryCombInstrumentGuardAsync
        (exchangeId: string, ?instrumentId: string)
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
           

    member this.QueryCombActionAsync
        (exchangeId: string, instrumentId: string, ?investUnitId: string)
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
           

    member this.QueryTransferSerialAsync
        (accountId: string, bankId: string, currencyId: string)
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
           

    member this.QueryAccountregisterAsync
        (
            ?accountId: string,
            ?bankId: string,
            ?bankBranchId: string,
            ?currencyId: string
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
           

    member this.QueryContractBankAsync(bankId: string, bankBrchId: string) =
        let request: QryContractBankRequest =
            { BrokerId = options.BrokerId; BankId = bankId; BankBrchId = bankBrchId }

        this.QueryAsync<ContractBankResponse, QryContractBankRequest>
            (nameof QryContractBankRequest)
            request
            api.ReqQryContractBank
           

    member this.QueryParkedOrderAsync
        (exchangeId: string, instrumentId: string, ?investUnitId: string)
        =
        let request: QryParkedOrderRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              ExchangeId = exchangeId
              InvestUnitId = investUnitId
              InstrumentId = instrumentId }

        this.QueryAsync<ParkedOrderResponse, QryParkedOrderRequest>
            (nameof QryParkedOrderRequest)
            request
            api.ReqQryParkedOrder
           

    member this.QueryParkedOrderActionAsync
        (exchangeId: string, instrumentId: string, ?investUnitId: string)
        =
        let request: QryParkedOrderActionRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              Reserve1 = None
              ExchangeId = exchangeId
              InvestUnitId = investUnitId
              InstrumentId = instrumentId }

        this.QueryAsync<ParkedOrderActionResponse, QryParkedOrderActionRequest>
            (nameof QryParkedOrderActionRequest)
            request
            api.ReqQryParkedOrderAction
           

    member this.QueryTradingNoticeAsync(?investUnitId: string) =
        let request: QryTradingNoticeRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              InvestUnitId = investUnitId }

        this.QueryAsync<TradingNoticeResponse, QryTradingNoticeRequest>
            (nameof QryTradingNoticeRequest)
            request
            api.ReqQryTradingNotice
           

    member this.QueryBrokerTradingParamsAsync
        (currencyId: string, ?accountId: string)
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
           

    member this.QueryBrokerTradingAlgosAsync
        (exchangeId: string, instrumentId: string)
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
           

    member this.QueryCfmmcTradingAccountTokenAsync(?investUnitId: string) =
        let request: QueryCfmmcTradingAccountTokenRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              InvestUnitId = investUnitId }

        this.QueryAsync<QueryCfmmcTradingAccountTokenResponse, QueryCfmmcTradingAccountTokenRequest>
            (nameof QueryCfmmcTradingAccountTokenRequest)
            request
            api.ReqQueryCfmmcTradingAccountToken
           

    member this.QueryClassifiedInstrumentAsync
        (
            tradingType: TradingType,
            classType: ClassType,
            ?instrumentId: string,
            ?exchangeId: string,
            ?exchangeInstId: string,
            ?productId: string
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
           

    member this.QueryCombPromotionParamAsync
        (?exchangeId: string, ?instrumentId: string)
        =
        let request: QryCombPromotionParamRequest = { ExchangeId = exchangeId; InstrumentId = instrumentId }

        this.QueryAsync<CombPromotionParamResponse, QryCombPromotionParamRequest>
            (nameof QryCombPromotionParamRequest)
            request
            api.ReqQryCombPromotionParam
           

    member this.QueryRiskSettleInvstPositionAsync(?instrumentId: string) =
        let request: QryRiskSettleInvstPositionRequest =
            { BrokerId = Some options.BrokerId
              InvestorId = Some options.UserId
              InstrumentId = instrumentId }

        this.QueryAsync<RiskSettleInvstPositionResponse, QryRiskSettleInvstPositionRequest>
            (nameof QryRiskSettleInvstPositionRequest)
            request
            api.ReqQryRiskSettleInvstPosition
           

    member this.QueryRiskSettleProductStatusAsync(?productId: string) =
        let request: QryRiskSettleProductStatusRequest = { ProductId = productId }

        this.QueryAsync<RiskSettleProductStatusResponse, QryRiskSettleProductStatusRequest>
            (nameof QryRiskSettleProductStatusRequest)
            request
            api.ReqQryRiskSettleProductStatus
           

    member this.QuerySpbmFutureParameterAsync
        (exchangeId: string, instrumentId: string, prodFamilyCode: string)
        =
        let request: QrySpbmFutureParameterRequest =
            { ExchangeId = exchangeId
              InstrumentId = instrumentId
              ProdFamilyCode = prodFamilyCode }

        this.QueryAsync<SpbmFutureParameterResponse, QrySpbmFutureParameterRequest>
            (nameof QrySpbmFutureParameterRequest)
            request
            api.ReqQrySpbmFutureParameter
           

    member this.QuerySpbmOptionParameterAsync
        (exchangeId: string, instrumentId: string, prodFamilyCode: string)
        =
        let request: QrySpbmOptionParameterRequest =
            { ExchangeId = exchangeId
              InstrumentId = instrumentId
              ProdFamilyCode = prodFamilyCode }

        this.QueryAsync<SpbmOptionParameterResponse, QrySpbmOptionParameterRequest>
            (nameof QrySpbmOptionParameterRequest)
            request
            api.ReqQrySpbmOptionParameter
           

    member this.QuerySpbmIntraParameterAsync
        (exchangeId: string, prodFamilyCode: string)
        =
        let request: QrySpbmIntraParameterRequest = { ExchangeId = exchangeId; ProdFamilyCode = prodFamilyCode }

        this.QueryAsync<SpbmIntraParameterResponse, QrySpbmIntraParameterRequest>
            (nameof QrySpbmIntraParameterRequest)
            request
            api.ReqQrySpbmIntraParameter
           

    member this.QuerySpbmInterParameterAsync
        (
            exchangeId: string,
            leg1ProdFamilyCode: string,
            leg2ProdFamilyCode: string
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
           

    member this.QuerySpbmPortfDefinitionAsync
        (exchangeId: string, portfolioDefId: string, prodFamilyCode: string)
        =
        let request: QrySpbmPortfDefinitionRequest =
            { ExchangeId = exchangeId
              PortfolioDefId = portfolioDefId
              ProdFamilyCode = prodFamilyCode }

        this.QueryAsync<SpbmPortfDefinitionResponse, QrySpbmPortfDefinitionRequest>
            (nameof QrySpbmPortfDefinitionRequest)
            request
            api.ReqQrySpbmPortfDefinition
           

    member this.QuerySpbmInvestorPortfDefAsync(exchangeId: string) =
        let request: QrySpbmInvestorPortfDefRequest =
            { ExchangeId = exchangeId
              BrokerId = options.BrokerId
              InvestorId = options.UserId }

        this.QueryAsync<SpbmInvestorPortfDefResponse, QrySpbmInvestorPortfDefRequest>
            (nameof QrySpbmInvestorPortfDefRequest)
            request
            api.ReqQrySpbmInvestorPortfDef
           

    member this.QueryInvestorPortfMarginRatioAsync
        (exchangeId: string, ?productGroupId: string)
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
           

    member this.QueryInvestorProdSpbmDetailAsync
        (exchangeId: string, prodFamilyCode: string)
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
           

    member this.QueryInvestorCommoditySpmmMarginAsync(commodityId: string) =
        let request: QryInvestorCommoditySpmmMarginRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              CommodityId = commodityId }

        this.QueryAsync<InvestorCommoditySpmmMarginResponse, QryInvestorCommoditySpmmMarginRequest>
            (nameof QryInvestorCommoditySpmmMarginRequest)
            request
            api.ReqQryInvestorCommoditySpmmMargin
           

    member this.QueryInvestorCommodityGroupSpmmMarginAsync
        (commodityGroupId: string)
        =
        let request: QryInvestorCommodityGroupSpmmMarginRequest =
            { BrokerId = options.BrokerId
              InvestorId = options.UserId
              CommodityGroupId = commodityGroupId }

        this.QueryAsync<InvestorCommodityGroupSpmmMarginResponse, QryInvestorCommodityGroupSpmmMarginRequest>
            (nameof QryInvestorCommodityGroupSpmmMarginRequest)
            request
            api.ReqQryInvestorCommodityGroupSpmmMargin
           

    member this.QuerySpmmInstParamAsync(instrumentId: string) =
        let request: QrySpmmInstParamRequest = { InstrumentId = instrumentId }

        this.QueryAsync<SpmmInstParamResponse, QrySpmmInstParamRequest>
            (nameof QrySpmmInstParamRequest)
            request
            api.ReqQrySpmmInstParam
           

    member this.QuerySpmmProductParamAsync(productId: string) =
        let request: QrySpmmProductParamRequest = { ProductId = productId }

        this.QueryAsync<SpmmProductParamResponse, QrySpmmProductParamRequest>
            (nameof QrySpmmProductParamRequest)
            request
            api.ReqQrySpmmProductParam
           

    member this.QuerySpbmAddOnInterParameterAsync
        (
            exchangeId: string,
            leg1ProdFamilyCode: string,
            leg2ProdFamilyCode: string
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
           

    member this.QueryRcamsCombProductInfoAsync
        (productId: string, combProductId: string, productGroupId: string)
        =
        let request: QryRcamsCombProductInfoRequest =
            { ProductId = productId
              CombProductId = combProductId
              ProductGroupId = productGroupId }

        this.QueryAsync<RcamsCombProductInfoResponse, QryRcamsCombProductInfoRequest>
            (nameof QryRcamsCombProductInfoRequest)
            request
            api.ReqQryRcamsCombProductInfo
           

    member this.QueryRcamsInstrParameterAsync(productId: string) =
        let request: QryRcamsInstrParameterRequest = { ProductId = productId }

        this.QueryAsync<RcamsInstrParameterResponse, QryRcamsInstrParameterRequest>
            (nameof QryRcamsInstrParameterRequest)
            request
            api.ReqQryRcamsInstrParameter
           

    member this.QueryRcamsIntraParameterAsync(combProductId: string) =
        let request: QryRcamsIntraParameterRequest = { CombProductId = combProductId }

        this.QueryAsync<RcamsIntraParameterResponse, QryRcamsIntraParameterRequest>
            (nameof QryRcamsIntraParameterRequest)
            request
            api.ReqQryRcamsIntraParameter
           

    member this.QueryRcamsInterParameterAsync
        (productGroupId: string, combProduct1: string, combProduct2: string)
        =
        let request: QryRcamsInterParameterRequest =
            { ProductGroupId = productGroupId
              CombProduct1 = combProduct1
              CombProduct2 = combProduct2 }

        this.QueryAsync<RcamsInterParameterResponse, QryRcamsInterParameterRequest>
            (nameof QryRcamsInterParameterRequest)
            request
            api.ReqQryRcamsInterParameter
           

    member this.QueryRcamsShortOptAdjustParamAsync(combProductId: string) =
        let request: QryRcamsShortOptAdjustParamRequest = { CombProductId = combProductId }

        this.QueryAsync<RcamsShortOptAdjustParamResponse, QryRcamsShortOptAdjustParamRequest>
            (nameof QryRcamsShortOptAdjustParamRequest)
            request
            api.ReqQryRcamsShortOptAdjustParam
           

    member this.QueryRcamsInvestorCombPositionAsync
        (instrumentId: string, combInstrumentId: string)
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
           

    member this.QueryInvestorProdRcamsMarginAsync
        (combProductId: string, productGroupId: string)
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
           

    member this.QueryRuleInstrParameterAsync
        (exchangeId: string, instrumentId: string)
        =
        let request: QryRuleInstrParameterRequest = { ExchangeId = exchangeId; InstrumentId = instrumentId }

        this.QueryAsync<RuleInstrParameterResponse, QryRuleInstrParameterRequest>
            (nameof QryRuleInstrParameterRequest)
            request
            api.ReqQryRuleInstrParameter
           

    member this.QueryRuleIntraParameterAsync
        (exchangeId: string, prodFamilyCode: string)
        =
        let request: QryRuleIntraParameterRequest = { ExchangeId = exchangeId; ProdFamilyCode = prodFamilyCode }

        this.QueryAsync<RuleIntraParameterResponse, QryRuleIntraParameterRequest>
            (nameof QryRuleIntraParameterRequest)
            request
            api.ReqQryRuleIntraParameter
           

    member this.QueryRuleInterParameterAsync
        (
            commodityGroupId: string,
            exchangeId: string,
            leg1ProdFamilyCode: string,
            ?leg2ProdFamilyCode: string
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
           

    member this.QueryInvestorProdRuleMarginAsync
        (exchangeId: string, prodFamilyCode: string, commodityGroupId: string)
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
           

    member this.QueryInvestorPortfSettingAsync(exchangeId: string) =
        let request: QryInvestorPortfSettingRequest =
            { ExchangeId = exchangeId
              BrokerId = options.BrokerId
              InvestorId = options.UserId }

        this.QueryAsync<InvestorPortfSettingResponse, QryInvestorPortfSettingRequest>
            (nameof QryInvestorPortfSettingRequest)
            request
            api.ReqQryInvestorPortfSetting
           

    member this.QueryInvestorInfoCommRecAsync(instrumentId: string) =
        let request: QryInvestorInfoCommRecRequest =
            { InvestorId = options.UserId
              InstrumentId = instrumentId
              BrokerId = options.BrokerId }

        this.QueryAsync<InvestorInfoCommRecResponse, QryInvestorInfoCommRecRequest>
            (nameof QryInvestorInfoCommRecRequest)
            request
            api.ReqQryInvestorInfoCommRec
           

    member this.QueryCombLegAsync(legInstrumentId: string) =
        let request: QryCombLegRequest = { LegInstrumentId = legInstrumentId }

        this.QueryAsync<CombLegResponse, QryCombLegRequest>
            (nameof QryCombLegRequest)
            request
            api.ReqQryCombLeg
           

    member this.QueryOffsetSettingAsync
        (productId: string, offsetType: OffsetType)
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
           

    member this.QuerySpdApplyAsync
        (
            exchangeId: string,
            orderSysId: string,
            firstLegInstrumentId: string,
            secondLegInstrumentId: string
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
           

    member this.QueryHedgeCfmAsync
        (exchangeId: string, orderSysId: string, instrumentId: string)
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
           
