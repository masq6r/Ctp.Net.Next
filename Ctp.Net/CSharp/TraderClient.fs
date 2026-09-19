namespace Ctp.Net.CSharp

open System
open System.Runtime.InteropServices
open System.Threading
open System.Threading.Tasks
open System.Collections.Generic
open Ctp.Net
open Ctp.Net.Bridge
open Microsoft.Extensions.Logging

type TraderClient private (inner: Ctp.Net.TraderClient) =

    let frontConnectedEvent = Event<EventHandler, EventArgs>()
    let frontDisconnectedEvent = Event<EventHandler<int>, int>()
    let heartBeatWarningEvent = Event<EventHandler<int>, int>()
    let privateSeqNoEvent = Event<EventHandler<int>, int>()
    let rspErrorEvent = Event<EventHandler<RspInfo>, RspInfo>()
    let orderEvent = Event<EventHandler<OrderUpdateResponse>, OrderUpdateResponse>()
    let tradeEvent = Event<EventHandler<TradeUpdateResponse>, TradeUpdateResponse>()
    let combActionEvent = Event<EventHandler<CombActionResponse>, CombActionResponse>()
    let execOrderEvent = Event<EventHandler<ExecOrderResponse>, ExecOrderResponse>()
    let forQuoteRspEvent = Event<EventHandler<ForQuoteRspResponse>, ForQuoteRspResponse>()
    let fromBankToFutureByFutureEvent = Event<EventHandler<TransferResponse>, TransferResponse>()
    let fromFutureToBankByFutureEvent = Event<EventHandler<TransferResponse>, TransferResponse>()
    let hedgeCfmEvent = Event<EventHandler<HedgeCfmResponse>, HedgeCfmResponse>()
    let offsetSettingEvent = Event<EventHandler<OffsetSettingResponse>, OffsetSettingResponse>()
    let optionSelfCloseEvent = Event<EventHandler<OptionSelfCloseResponse>, OptionSelfCloseResponse>()
    let queryBankBalanceByFutureEvent = Event<EventHandler<NotifyQueryAccountResponse>, NotifyQueryAccountResponse>()
    let quoteEvent = Event<EventHandler<QuoteResponse>, QuoteResponse>()
    let spdApplyEvent = Event<EventHandler<SpdApplyResponse>, SpdApplyResponse>()
    let instrumentStatusEvent = Event<EventHandler<InstrumentStatusResponse>, InstrumentStatusResponse>()
    let bulletinEvent = Event<EventHandler<BulletinResponse>, BulletinResponse>()
    let tradingNoticeEvent = Event<EventHandler<TradingNoticeInfoResponse>, TradingNoticeInfoResponse>()
    let errorConditionalOrderEvent = Event<EventHandler<ErrorConditionalOrderResponse>, ErrorConditionalOrderResponse>()
    let cfmmcTradingAccountTokenEvent =
        Event<EventHandler<CfmmcTradingAccountTokenResponse>, CfmmcTradingAccountTokenResponse>()
    let fromBankToFutureByBankEvent = Event<EventHandler<TransferResponse>, TransferResponse>()
    let fromFutureToBankByBankEvent = Event<EventHandler<TransferResponse>, TransferResponse>()
    let repealFromBankToFutureByBankEvent = Event<EventHandler<RepealResponse>, RepealResponse>()
    let repealFromFutureToBankByBankEvent = Event<EventHandler<RepealResponse>, RepealResponse>()
    let repealFromBankToFutureByFutureManualEvent = Event<EventHandler<RepealResponse>, RepealResponse>()
    let repealFromFutureToBankByFutureManualEvent = Event<EventHandler<RepealResponse>, RepealResponse>()
    let repealFromBankToFutureByFutureEvent = Event<EventHandler<RepealResponse>, RepealResponse>()
    let repealFromFutureToBankByFutureEvent = Event<EventHandler<RepealResponse>, RepealResponse>()
    let openAccountByBankEvent = Event<EventHandler<OpenAccountResponse>, OpenAccountResponse>()
    let cancelAccountByBankEvent = Event<EventHandler<CancelAccountResponse>, CancelAccountResponse>()
    let changeAccountByBankEvent = Event<EventHandler<ChangeAccountResponse>, ChangeAccountResponse>()
    let notificationEvent = Event<EventHandler<obj>, obj>()
    let asyncErrorEvent = Event<EventHandler<CtpEventArgs<obj, RspInfo>>, CtpEventArgs<obj, RspInfo>>()
    let asyncErrorDetailedEvent = Event<EventHandler<TraderAsyncError>, TraderAsyncError>()
    let commandResponseEvent = Event<EventHandler<TraderCommandResponse>, TraderCommandResponse>()
    let bankToFutureByFutureErrorEvent = Event<EventHandler<TraderAsyncErrorEventArgs<TransferAckResponse>>, TraderAsyncErrorEventArgs<TransferAckResponse>>()
    let batchOrderActionErrorEvent = Event<EventHandler<TraderAsyncErrorEventArgs<BatchOrderActionResponse>>, TraderAsyncErrorEventArgs<BatchOrderActionResponse>>()
    let cancelOffsetSettingErrorEvent = Event<EventHandler<TraderAsyncErrorEventArgs<CancelOffsetSettingResponse>>, TraderAsyncErrorEventArgs<CancelOffsetSettingResponse>>()
    let combActionInsertErrorEvent = Event<EventHandler<TraderAsyncErrorEventArgs<InputCombActionResponse>>, TraderAsyncErrorEventArgs<InputCombActionResponse>>()
    let execOrderActionErrorEvent = Event<EventHandler<TraderAsyncErrorEventArgs<ExecOrderActionResponse>>, TraderAsyncErrorEventArgs<ExecOrderActionResponse>>()
    let execOrderInsertErrorEvent = Event<EventHandler<TraderAsyncErrorEventArgs<InputExecOrderResponse>>, TraderAsyncErrorEventArgs<InputExecOrderResponse>>()
    let forQuoteInsertErrorEvent = Event<EventHandler<TraderAsyncErrorEventArgs<InputForQuoteResponse>>, TraderAsyncErrorEventArgs<InputForQuoteResponse>>()
    let futureToBankByFutureErrorEvent = Event<EventHandler<TraderAsyncErrorEventArgs<TransferAckResponse>>, TraderAsyncErrorEventArgs<TransferAckResponse>>()
    let hedgeCfmErrorEvent = Event<EventHandler<TraderAsyncErrorEventArgs<InputHedgeCfmResponse>>, TraderAsyncErrorEventArgs<InputHedgeCfmResponse>>()
    let hedgeCfmActionErrorEvent = Event<EventHandler<TraderAsyncErrorEventArgs<HedgeCfmActionResponse>>, TraderAsyncErrorEventArgs<HedgeCfmActionResponse>>()
    let offsetSettingErrorEvent = Event<EventHandler<TraderAsyncErrorEventArgs<InputOffsetSettingResponse>>, TraderAsyncErrorEventArgs<InputOffsetSettingResponse>>()
    let optionSelfCloseActionErrorEvent = Event<EventHandler<TraderAsyncErrorEventArgs<OptionSelfCloseActionResponse>>, TraderAsyncErrorEventArgs<OptionSelfCloseActionResponse>>()
    let optionSelfCloseInsertErrorEvent = Event<EventHandler<TraderAsyncErrorEventArgs<InputOptionSelfCloseResponse>>, TraderAsyncErrorEventArgs<InputOptionSelfCloseResponse>>()
    let orderActionErrorEvent = Event<EventHandler<TraderAsyncErrorEventArgs<OrderActionResponse>>, TraderAsyncErrorEventArgs<OrderActionResponse>>()
    let orderInsertErrorEvent = Event<EventHandler<TraderAsyncErrorEventArgs<InputOrderResponse>>, TraderAsyncErrorEventArgs<InputOrderResponse>>()
    let queryBankBalanceByFutureErrorEvent = Event<EventHandler<TraderAsyncErrorEventArgs<QueryBankAccountMoneyResponse>>, TraderAsyncErrorEventArgs<QueryBankAccountMoneyResponse>>()
    let quoteActionErrorEvent = Event<EventHandler<TraderAsyncErrorEventArgs<QuoteActionResponse>>, TraderAsyncErrorEventArgs<QuoteActionResponse>>()
    let quoteInsertErrorEvent = Event<EventHandler<TraderAsyncErrorEventArgs<InputQuoteResponse>>, TraderAsyncErrorEventArgs<InputQuoteResponse>>()
    let spdApplyErrorEvent = Event<EventHandler<TraderAsyncErrorEventArgs<InputSpdApplyResponse>>, TraderAsyncErrorEventArgs<InputSpdApplyResponse>>()
    let spdApplyActionErrorEvent = Event<EventHandler<TraderAsyncErrorEventArgs<SpdApplyActionResponse>>, TraderAsyncErrorEventArgs<SpdApplyActionResponse>>()
    let repealBankToFutureByFutureManualErrorEvent = Event<EventHandler<TraderAsyncErrorEventArgs<RepealRequest>>, TraderAsyncErrorEventArgs<RepealRequest>>()
    let repealFutureToBankByFutureManualErrorEvent = Event<EventHandler<TraderAsyncErrorEventArgs<RepealRequest>>, TraderAsyncErrorEventArgs<RepealRequest>>()
    let orderInsertResponseEvent = Event<EventHandler<TraderCommandResponseEventArgs<InputOrderResponse>>, TraderCommandResponseEventArgs<InputOrderResponse>>()
    let orderActionResponseEvent = Event<EventHandler<TraderCommandResponseEventArgs<InputOrderActionResponse>>, TraderCommandResponseEventArgs<InputOrderActionResponse>>()
    let batchOrderActionResponseEvent = Event<EventHandler<TraderCommandResponseEventArgs<InputBatchOrderActionResponse>>, TraderCommandResponseEventArgs<InputBatchOrderActionResponse>>()
    let cancelOffsetSettingResponseEvent = Event<EventHandler<TraderCommandResponseEventArgs<InputOffsetSettingResponse>>, TraderCommandResponseEventArgs<InputOffsetSettingResponse>>()
    let combActionInsertResponseEvent = Event<EventHandler<TraderCommandResponseEventArgs<InputCombActionResponse>>, TraderCommandResponseEventArgs<InputCombActionResponse>>()
    let execOrderActionResponseEvent = Event<EventHandler<TraderCommandResponseEventArgs<InputExecOrderActionResponse>>, TraderCommandResponseEventArgs<InputExecOrderActionResponse>>()
    let execOrderInsertResponseEvent = Event<EventHandler<TraderCommandResponseEventArgs<InputExecOrderResponse>>, TraderCommandResponseEventArgs<InputExecOrderResponse>>()
    let forQuoteInsertResponseEvent = Event<EventHandler<TraderCommandResponseEventArgs<InputForQuoteResponse>>, TraderCommandResponseEventArgs<InputForQuoteResponse>>()
    let fromBankToFutureByFutureResponseEvent = Event<EventHandler<TraderCommandResponseEventArgs<TransferAckResponse>>, TraderCommandResponseEventArgs<TransferAckResponse>>()
    let fromFutureToBankByFutureResponseEvent = Event<EventHandler<TraderCommandResponseEventArgs<TransferAckResponse>>, TraderCommandResponseEventArgs<TransferAckResponse>>()
    let hedgeCfmResponseEvent = Event<EventHandler<TraderCommandResponseEventArgs<InputHedgeCfmResponse>>, TraderCommandResponseEventArgs<InputHedgeCfmResponse>>()
    let hedgeCfmActionResponseEvent = Event<EventHandler<TraderCommandResponseEventArgs<InputHedgeCfmActionResponse>>, TraderCommandResponseEventArgs<InputHedgeCfmActionResponse>>()
    let offsetSettingResponseEvent = Event<EventHandler<TraderCommandResponseEventArgs<InputOffsetSettingResponse>>, TraderCommandResponseEventArgs<InputOffsetSettingResponse>>()
    let optionSelfCloseActionResponseEvent = Event<EventHandler<TraderCommandResponseEventArgs<InputOptionSelfCloseActionResponse>>, TraderCommandResponseEventArgs<InputOptionSelfCloseActionResponse>>()
    let optionSelfCloseInsertResponseEvent = Event<EventHandler<TraderCommandResponseEventArgs<InputOptionSelfCloseResponse>>, TraderCommandResponseEventArgs<InputOptionSelfCloseResponse>>()
    let quoteActionResponseEvent = Event<EventHandler<TraderCommandResponseEventArgs<InputQuoteActionResponse>>, TraderCommandResponseEventArgs<InputQuoteActionResponse>>()
    let quoteInsertResponseEvent = Event<EventHandler<TraderCommandResponseEventArgs<InputQuoteResponse>>, TraderCommandResponseEventArgs<InputQuoteResponse>>()
    let spdApplyResponseEvent = Event<EventHandler<TraderCommandResponseEventArgs<InputSpdApplyResponse>>, TraderCommandResponseEventArgs<InputSpdApplyResponse>>()
    let spdApplyActionResponseEvent = Event<EventHandler<TraderCommandResponseEventArgs<InputSpdApplyActionResponse>>, TraderCommandResponseEventArgs<InputSpdApplyActionResponse>>()

    let triggerNotification (typedEvent: Event<EventHandler<'TPayload>, 'TPayload>) payload =
        typedEvent.Trigger(null, payload)
        notificationEvent.Trigger(null, box payload)

    let projectAsyncError
        (source: IEvent<TraderAsyncErrorData<'TPayload>>)
        (target: Event<EventHandler<TraderAsyncErrorEventArgs<'TPayload>>, TraderAsyncErrorEventArgs<'TPayload>>)
        =
        source.Add(fun data ->
            target.Trigger(null, CSharpHelpers.asyncErrorEventArgs data)

            let payload = data.Payload |> Option.map box |> Option.defaultValue Unchecked.defaultof<obj>
            let detailed: TraderAsyncError =
                { CallbackName = data.CallbackName
                  Payload = payload
                  RspInfo = data.RspInfo }

            asyncErrorDetailedEvent.Trigger(null, detailed)

            let rspInfo = data.RspInfo |> Option.defaultValue Unchecked.defaultof<RspInfo>
            asyncErrorEvent.Trigger(null, CtpEventArgs(payload, rspInfo)))

    let projectCommandResponse
        (source: IEvent<TraderCommandResponseData<'TPayload>>)
        (target: Event<EventHandler<TraderCommandResponseEventArgs<'TPayload>>, TraderCommandResponseEventArgs<'TPayload>>)
        =
        source.Add(fun data ->
            target.Trigger(null, CSharpHelpers.commandResponseEventArgs data)

            commandResponseEvent.Trigger(
                null,
                { CallbackName = data.CallbackName
                  OperationName = data.OperationName
                  RequestId = data.RequestId
                  IsLast = data.IsLast
                  RspInfo = data.RspInfo
                  Payload = data.Payload |> Option.map box }
            ))

    do
        inner.FrontConnected.Add(fun () -> frontConnectedEvent.Trigger(null, EventArgs.Empty))
        inner.FrontDisconnected.Add(fun r -> frontDisconnectedEvent.Trigger(null, r))
        inner.HeartBeatWarning.Add(fun l -> heartBeatWarningEvent.Trigger(null, l))
        inner.PrivateSeqNoReceived.Add(fun s -> privateSeqNoEvent.Trigger(null, s))
        inner.RspError.Add(fun i -> rspErrorEvent.Trigger(null, i))
        inner.OrderReceived.Add(fun o -> orderEvent.Trigger(null, o))
        inner.TradeReceived.Add(fun t -> tradeEvent.Trigger(null, t))
        inner.CombActionReceived.Add(triggerNotification combActionEvent)
        inner.ExecOrderReceived.Add(triggerNotification execOrderEvent)
        inner.ForQuoteRspReceived.Add(triggerNotification forQuoteRspEvent)
        inner.FromBankToFutureByFutureReceived.Add(triggerNotification fromBankToFutureByFutureEvent)
        inner.FromFutureToBankByFutureReceived.Add(triggerNotification fromFutureToBankByFutureEvent)
        inner.HedgeCfmReceived.Add(triggerNotification hedgeCfmEvent)
        inner.OffsetSettingReceived.Add(triggerNotification offsetSettingEvent)
        inner.OptionSelfCloseReceived.Add(triggerNotification optionSelfCloseEvent)
        inner.QueryBankBalanceByFutureReceived.Add(triggerNotification queryBankBalanceByFutureEvent)
        inner.QuoteReceived.Add(triggerNotification quoteEvent)
        inner.SpdApplyReceived.Add(triggerNotification spdApplyEvent)
        inner.InstrumentStatusReceived.Add(triggerNotification instrumentStatusEvent)
        inner.BulletinReceived.Add(triggerNotification bulletinEvent)
        inner.TradingNoticeReceived.Add(triggerNotification tradingNoticeEvent)
        inner.ErrorConditionalOrderReceived.Add(triggerNotification errorConditionalOrderEvent)
        inner.CfmmcTradingAccountTokenReceived.Add(triggerNotification cfmmcTradingAccountTokenEvent)
        inner.FromBankToFutureByBankReceived.Add(triggerNotification fromBankToFutureByBankEvent)
        inner.FromFutureToBankByBankReceived.Add(triggerNotification fromFutureToBankByBankEvent)
        inner.RepealFromBankToFutureByBankReceived.Add(triggerNotification repealFromBankToFutureByBankEvent)
        inner.RepealFromFutureToBankByBankReceived.Add(triggerNotification repealFromFutureToBankByBankEvent)
        inner.RepealFromBankToFutureByFutureManualReceived.Add(fun item ->
            triggerNotification repealFromBankToFutureByFutureManualEvent item)
        inner.RepealFromFutureToBankByFutureManualReceived.Add(fun item ->
            triggerNotification repealFromFutureToBankByFutureManualEvent item)
        inner.RepealFromBankToFutureByFutureReceived.Add(fun item ->
            triggerNotification repealFromBankToFutureByFutureEvent item)
        inner.RepealFromFutureToBankByFutureReceived.Add(fun item ->
            triggerNotification repealFromFutureToBankByFutureEvent item)
        inner.OpenAccountByBankReceived.Add(triggerNotification openAccountByBankEvent)
        inner.CancelAccountByBankReceived.Add(triggerNotification cancelAccountByBankEvent)
        inner.ChangeAccountByBankReceived.Add(triggerNotification changeAccountByBankEvent)

        projectAsyncError inner.BankToFutureByFutureErrorReceived bankToFutureByFutureErrorEvent
        projectAsyncError inner.BatchOrderActionErrorReceived batchOrderActionErrorEvent
        projectAsyncError inner.CancelOffsetSettingErrorReceived cancelOffsetSettingErrorEvent
        projectAsyncError inner.CombActionInsertErrorReceived combActionInsertErrorEvent
        projectAsyncError inner.ExecOrderActionErrorReceived execOrderActionErrorEvent
        projectAsyncError inner.ExecOrderInsertErrorReceived execOrderInsertErrorEvent
        projectAsyncError inner.ForQuoteInsertErrorReceived forQuoteInsertErrorEvent
        projectAsyncError inner.FutureToBankByFutureErrorReceived futureToBankByFutureErrorEvent
        projectAsyncError inner.HedgeCfmErrorReceived hedgeCfmErrorEvent
        projectAsyncError inner.HedgeCfmActionErrorReceived hedgeCfmActionErrorEvent
        projectAsyncError inner.OffsetSettingErrorReceived offsetSettingErrorEvent
        projectAsyncError inner.OptionSelfCloseActionErrorReceived optionSelfCloseActionErrorEvent
        projectAsyncError inner.OptionSelfCloseInsertErrorReceived optionSelfCloseInsertErrorEvent
        projectAsyncError inner.OrderActionErrorReceived orderActionErrorEvent
        projectAsyncError inner.OrderInsertErrorReceived orderInsertErrorEvent
        projectAsyncError inner.QueryBankBalanceByFutureErrorReceived queryBankBalanceByFutureErrorEvent
        projectAsyncError inner.QuoteActionErrorReceived quoteActionErrorEvent
        projectAsyncError inner.QuoteInsertErrorReceived quoteInsertErrorEvent
        projectAsyncError inner.SpdApplyErrorReceived spdApplyErrorEvent
        projectAsyncError inner.SpdApplyActionErrorReceived spdApplyActionErrorEvent
        projectAsyncError inner.RepealBankToFutureByFutureManualErrorReceived repealBankToFutureByFutureManualErrorEvent
        projectAsyncError inner.RepealFutureToBankByFutureManualErrorReceived repealFutureToBankByFutureManualErrorEvent

        projectCommandResponse inner.OrderInsertResponseReceived orderInsertResponseEvent
        projectCommandResponse inner.OrderActionResponseReceived orderActionResponseEvent
        projectCommandResponse inner.BatchOrderActionResponseReceived batchOrderActionResponseEvent
        projectCommandResponse inner.CancelOffsetSettingResponseReceived cancelOffsetSettingResponseEvent
        projectCommandResponse inner.CombActionInsertResponseReceived combActionInsertResponseEvent
        projectCommandResponse inner.ExecOrderActionResponseReceived execOrderActionResponseEvent
        projectCommandResponse inner.ExecOrderInsertResponseReceived execOrderInsertResponseEvent
        projectCommandResponse inner.ForQuoteInsertResponseReceived forQuoteInsertResponseEvent
        projectCommandResponse inner.FromBankToFutureByFutureResponseReceived fromBankToFutureByFutureResponseEvent
        projectCommandResponse inner.FromFutureToBankByFutureResponseReceived fromFutureToBankByFutureResponseEvent
        projectCommandResponse inner.HedgeCfmResponseReceived hedgeCfmResponseEvent
        projectCommandResponse inner.HedgeCfmActionResponseReceived hedgeCfmActionResponseEvent
        projectCommandResponse inner.OffsetSettingResponseReceived offsetSettingResponseEvent
        projectCommandResponse inner.OptionSelfCloseActionResponseReceived optionSelfCloseActionResponseEvent
        projectCommandResponse inner.OptionSelfCloseInsertResponseReceived optionSelfCloseInsertResponseEvent
        projectCommandResponse inner.QuoteActionResponseReceived quoteActionResponseEvent
        projectCommandResponse inner.QuoteInsertResponseReceived quoteInsertResponseEvent
        projectCommandResponse inner.SpdApplyResponseReceived spdApplyResponseEvent
        projectCommandResponse inner.SpdApplyActionResponseReceived spdApplyActionResponseEvent

    new(options: CtpOptions) = new TraderClient(new Ctp.Net.TraderClient(options))

    new(configuration: TraderClientOptions) =
        if isNull (box configuration) then
            nullArg (nameof configuration)

        new TraderClient(
            new Ctp.Net.TraderClient(
                configuration.Options,
                ?encodings = CSharpHelpers.valueToOption configuration.Encodings,
                ?privateTopicResumeType = CSharpHelpers.nullableToOption configuration.PrivateTopicResumeType,
                ?privateTopicSequenceNo = CSharpHelpers.nullableToOption configuration.PrivateTopicSequenceNo,
                ?publicTopicResumeType = CSharpHelpers.nullableToOption configuration.PublicTopicResumeType,
                ?loggerFactory = CSharpHelpers.nullToOption configuration.LoggerFactory,
                ?flowControl = CSharpHelpers.valueToOption configuration.FlowControl,
                ?endpoint = Some configuration.Endpoint
            )
        )

    new(options: CtpOptions, endpoint: CtpEndpoint) =
        new TraderClient(new Ctp.Net.TraderClient(options, ?endpoint = Some endpoint))

    new
        (
            options: CtpOptions,
            encodings: CtpEncodingOptions,
            [<Optional>] privateTopicResumeType: Nullable<ResumeType>,
            [<Optional>] privateTopicSequenceNo: Nullable<int>,
            [<Optional>] publicTopicResumeType: Nullable<ResumeType>,
            [<Optional>] loggerFactory: ILoggerFactory,
            flowControl: CtpFlowControlOptions
        )
        =
        let nullToOpt (v: 'T) = if obj.ReferenceEquals(box v, null) then None else Some v

        new TraderClient(
            new Ctp.Net.TraderClient(
                options,
                ?encodings = nullToOpt encodings,
                ?privateTopicResumeType = CSharpHelpers.nullableToOption privateTopicResumeType,
                ?privateTopicSequenceNo = CSharpHelpers.nullableToOption privateTopicSequenceNo,
                ?publicTopicResumeType = CSharpHelpers.nullableToOption publicTopicResumeType,
                ?loggerFactory = CSharpHelpers.nullToOption loggerFactory,
                ?flowControl = nullToOpt flowControl
            )
        )

    [<CLIEvent>]
    member _.FrontConnected = frontConnectedEvent.Publish

    [<CLIEvent>]
    member _.FrontDisconnected = frontDisconnectedEvent.Publish

    [<CLIEvent>]
    member _.HeartBeatWarning = heartBeatWarningEvent.Publish

    [<CLIEvent>]
    member _.PrivateSeqNoReceived = privateSeqNoEvent.Publish

    [<CLIEvent>]
    member _.RspError = rspErrorEvent.Publish

    [<CLIEvent>]
    member _.OrderReceived = orderEvent.Publish

    [<CLIEvent>]
    member _.TradeReceived = tradeEvent.Publish

    [<CLIEvent>]
    member _.CombActionReceived = combActionEvent.Publish

    [<CLIEvent>]
    member _.ExecOrderReceived = execOrderEvent.Publish

    [<CLIEvent>]
    member _.ForQuoteRspReceived = forQuoteRspEvent.Publish

    [<CLIEvent>]
    member _.FromBankToFutureByFutureReceived = fromBankToFutureByFutureEvent.Publish

    [<CLIEvent>]
    member _.FromFutureToBankByFutureReceived = fromFutureToBankByFutureEvent.Publish

    [<CLIEvent>]
    member _.HedgeCfmReceived = hedgeCfmEvent.Publish

    [<CLIEvent>]
    member _.OffsetSettingReceived = offsetSettingEvent.Publish

    [<CLIEvent>]
    member _.OptionSelfCloseReceived = optionSelfCloseEvent.Publish

    [<CLIEvent>]
    member _.QueryBankBalanceByFutureReceived = queryBankBalanceByFutureEvent.Publish

    [<CLIEvent>]
    member _.QuoteReceived = quoteEvent.Publish

    [<CLIEvent>]
    member _.SpdApplyReceived = spdApplyEvent.Publish

    [<CLIEvent>]
    member _.InstrumentStatusReceived = instrumentStatusEvent.Publish

    [<CLIEvent>]
    member _.BulletinReceived = bulletinEvent.Publish

    [<CLIEvent>]
    member _.TradingNoticeReceived = tradingNoticeEvent.Publish

    [<CLIEvent>]
    member _.ErrorConditionalOrderReceived = errorConditionalOrderEvent.Publish

    [<CLIEvent>]
    member _.CfmmcTradingAccountTokenReceived = cfmmcTradingAccountTokenEvent.Publish

    [<CLIEvent>]
    member _.FromBankToFutureByBankReceived = fromBankToFutureByBankEvent.Publish

    [<CLIEvent>]
    member _.FromFutureToBankByBankReceived = fromFutureToBankByBankEvent.Publish

    [<CLIEvent>]
    member _.RepealFromBankToFutureByBankReceived = repealFromBankToFutureByBankEvent.Publish

    [<CLIEvent>]
    member _.RepealFromFutureToBankByBankReceived = repealFromFutureToBankByBankEvent.Publish

    [<CLIEvent>]
    member _.RepealFromBankToFutureByFutureManualReceived = repealFromBankToFutureByFutureManualEvent.Publish

    [<CLIEvent>]
    member _.RepealFromFutureToBankByFutureManualReceived = repealFromFutureToBankByFutureManualEvent.Publish

    [<CLIEvent>]
    member _.RepealFromBankToFutureByFutureReceived = repealFromBankToFutureByFutureEvent.Publish

    [<CLIEvent>]
    member _.RepealFromFutureToBankByFutureReceived = repealFromFutureToBankByFutureEvent.Publish

    [<CLIEvent>]
    member _.OpenAccountByBankReceived = openAccountByBankEvent.Publish

    [<CLIEvent>]
    member _.CancelAccountByBankReceived = cancelAccountByBankEvent.Publish

    [<CLIEvent>]
    member _.ChangeAccountByBankReceived = changeAccountByBankEvent.Publish

    [<CLIEvent; Obsolete("Use the callback-specific strongly typed notification events.")>]
    member _.NotificationReceived = notificationEvent.Publish

    [<CLIEvent; Obsolete("Use the callback-specific strongly typed error events.")>]
    member _.AsyncErrorReceived = asyncErrorEvent.Publish

    [<CLIEvent; Obsolete("Use the callback-specific strongly typed error events.")>]
    member _.AsyncErrorDetailedReceived = asyncErrorDetailedEvent.Publish

    [<CLIEvent; Obsolete("Use the callback-specific strongly typed command response events.")>]
    member _.CommandResponseReceived = commandResponseEvent.Publish

    [<CLIEvent>]
    member _.BankToFutureByFutureErrorReceived = bankToFutureByFutureErrorEvent.Publish
    [<CLIEvent>]
    member _.BatchOrderActionErrorReceived = batchOrderActionErrorEvent.Publish
    [<CLIEvent>]
    member _.CancelOffsetSettingErrorReceived = cancelOffsetSettingErrorEvent.Publish
    [<CLIEvent>]
    member _.CombActionInsertErrorReceived = combActionInsertErrorEvent.Publish
    [<CLIEvent>]
    member _.ExecOrderActionErrorReceived = execOrderActionErrorEvent.Publish
    [<CLIEvent>]
    member _.ExecOrderInsertErrorReceived = execOrderInsertErrorEvent.Publish
    [<CLIEvent>]
    member _.ForQuoteInsertErrorReceived = forQuoteInsertErrorEvent.Publish
    [<CLIEvent>]
    member _.FutureToBankByFutureErrorReceived = futureToBankByFutureErrorEvent.Publish
    [<CLIEvent>]
    member _.HedgeCfmErrorReceived = hedgeCfmErrorEvent.Publish
    [<CLIEvent>]
    member _.HedgeCfmActionErrorReceived = hedgeCfmActionErrorEvent.Publish
    [<CLIEvent>]
    member _.OffsetSettingErrorReceived = offsetSettingErrorEvent.Publish
    [<CLIEvent>]
    member _.OptionSelfCloseActionErrorReceived = optionSelfCloseActionErrorEvent.Publish
    [<CLIEvent>]
    member _.OptionSelfCloseInsertErrorReceived = optionSelfCloseInsertErrorEvent.Publish
    [<CLIEvent>]
    member _.OrderActionErrorReceived = orderActionErrorEvent.Publish
    [<CLIEvent>]
    member _.OrderInsertErrorReceived = orderInsertErrorEvent.Publish
    [<CLIEvent>]
    member _.QueryBankBalanceByFutureErrorReceived = queryBankBalanceByFutureErrorEvent.Publish
    [<CLIEvent>]
    member _.QuoteActionErrorReceived = quoteActionErrorEvent.Publish
    [<CLIEvent>]
    member _.QuoteInsertErrorReceived = quoteInsertErrorEvent.Publish
    [<CLIEvent>]
    member _.SpdApplyErrorReceived = spdApplyErrorEvent.Publish
    [<CLIEvent>]
    member _.SpdApplyActionErrorReceived = spdApplyActionErrorEvent.Publish
    [<CLIEvent>]
    member _.RepealBankToFutureByFutureManualErrorReceived = repealBankToFutureByFutureManualErrorEvent.Publish
    [<CLIEvent>]
    member _.RepealFutureToBankByFutureManualErrorReceived = repealFutureToBankByFutureManualErrorEvent.Publish

    [<CLIEvent>]
    member _.OrderInsertResponseReceived = orderInsertResponseEvent.Publish
    [<CLIEvent>]
    member _.OrderActionResponseReceived = orderActionResponseEvent.Publish
    [<CLIEvent>]
    member _.BatchOrderActionResponseReceived = batchOrderActionResponseEvent.Publish
    [<CLIEvent>]
    member _.CancelOffsetSettingResponseReceived = cancelOffsetSettingResponseEvent.Publish
    [<CLIEvent>]
    member _.CombActionInsertResponseReceived = combActionInsertResponseEvent.Publish
    [<CLIEvent>]
    member _.ExecOrderActionResponseReceived = execOrderActionResponseEvent.Publish
    [<CLIEvent>]
    member _.ExecOrderInsertResponseReceived = execOrderInsertResponseEvent.Publish
    [<CLIEvent>]
    member _.ForQuoteInsertResponseReceived = forQuoteInsertResponseEvent.Publish
    [<CLIEvent>]
    member _.FromBankToFutureByFutureResponseReceived = fromBankToFutureByFutureResponseEvent.Publish
    [<CLIEvent>]
    member _.FromFutureToBankByFutureResponseReceived = fromFutureToBankByFutureResponseEvent.Publish
    [<CLIEvent>]
    member _.HedgeCfmResponseReceived = hedgeCfmResponseEvent.Publish
    [<CLIEvent>]
    member _.HedgeCfmActionResponseReceived = hedgeCfmActionResponseEvent.Publish
    [<CLIEvent>]
    member _.OffsetSettingResponseReceived = offsetSettingResponseEvent.Publish
    [<CLIEvent>]
    member _.OptionSelfCloseActionResponseReceived = optionSelfCloseActionResponseEvent.Publish
    [<CLIEvent>]
    member _.OptionSelfCloseInsertResponseReceived = optionSelfCloseInsertResponseEvent.Publish
    [<CLIEvent>]
    member _.QuoteActionResponseReceived = quoteActionResponseEvent.Publish
    [<CLIEvent>]
    member _.QuoteInsertResponseReceived = quoteInsertResponseEvent.Publish
    [<CLIEvent>]
    member _.SpdApplyResponseReceived = spdApplyResponseEvent.Publish
    [<CLIEvent>]
    member _.SpdApplyActionResponseReceived = spdApplyActionResponseEvent.Publish

    member _.ConnectAsync
        ([<Optional>] timeout: Nullable<TimeSpan>, [<Optional>] cancellationToken: CancellationToken)
        : Task
        =
        let comp =
            match CSharpHelpers.nullableToOption timeout with
            | Some t -> inner.Connect(timeout = t)
            | None -> inner.Connect()

        CSharpHelpers.startConnectAsync cancellationToken comp

    member _.Join() = inner.Join()

    member _.GetApiVersion() = inner.GetApiVersion()

    member _.GetTradingDay() = inner.GetTradingDay()

    member _.GetFrontInfo() = inner.GetFrontInfo()

    member _.RegisterNameServer(nsAddress: string) = inner.RegisterNameServer(nsAddress)

    member _.RegisterFensUserInfo(request: FensUserInfoRequest) = inner.RegisterFensUserInfo(request)

    member _.RegisterUserSystemInfo(info: UserSystemInfoRequest) = inner.RegisterUserSystemInfo(info)

    member _.SubmitUserSystemInfo(info: UserSystemInfoRequest) = inner.SubmitUserSystemInfo(info)

    member _.RegisterWechatUserSystemInfo(info: WechatUserSystemInfoRequest) =
        inner.RegisterWechatUserSystemInfo(info)

    member _.SubmitWechatUserSystemInfo(info: WechatUserSystemInfoRequest) =
        inner.SubmitWechatUserSystemInfo(info)

    // ---- Auth / session (single-response) ----

    member _.AuthenticateAsync([<Optional>] cancellationToken: CancellationToken) : Task<AuthenticateResponse> =
        CSharpHelpers.startAsync
            cancellationToken
            (async {
                let! r = inner.AuthenticateAsync()

                return
                    match r with
                    | Ok(first :: _) -> Ok first
                    | Ok [] -> Error({ ErrorId = -1; ErrorMessage = "No response"; RawErrorMessage = Array.empty })
                    | Error e -> Error e
            })

    member _.LoginAsync([<Optional>] cancellationToken: CancellationToken) : Task<UserLoginResponse> =
        CSharpHelpers.startAsync
            cancellationToken
            (async {
                let! r = inner.LoginAsync()

                return
                    match r with
                    | Ok(first :: _) -> Ok first
                    | Ok [] -> Error({ ErrorId = -1; ErrorMessage = "No response"; RawErrorMessage = Array.empty })
                    | Error e -> Error e
            })

    member _.LogoutAsync([<Optional>] cancellationToken: CancellationToken) : Task<UserLogoutResponse> =
        CSharpHelpers.startAsync
            cancellationToken
            (async {
                let! r = inner.LogoutAsync()

                return
                    match r with
                    | Ok(first :: _) -> Ok first
                    | Ok [] -> Error({ ErrorId = -1; ErrorMessage = "No response"; RawErrorMessage = Array.empty })
                    | Error e -> Error e
            })

    member _.SettlementInfoConfirmAsync
        ([<Optional>] cancellationToken: CancellationToken)
        : Task<SettlementInfoConfirmResponse>
        =
        CSharpHelpers.startAsync
            cancellationToken
            (async {
                let! r = inner.SettlementInfoConfirmAsync()

                return
                    match r with
                    | Ok(first :: _) -> Ok first
                    | Ok [] -> Error({ ErrorId = -1; ErrorMessage = "No response"; RawErrorMessage = Array.empty })
                    | Error e -> Error e
            })

    // ---- Core queries (list → IReadOnlyList) ----

    member _.QueryTradingAccountAsync
        (
            [<Optional>] currencyId: string,
            [<Optional>] accountId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<TradingAccountResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (inner.QueryTradingAccountAsync(?currencyId = CSharpHelpers.valueToOption currencyId, ?accountId = CSharpHelpers.valueToOption accountId))

    member _.QueryTradingAccountAsync
        (
            currencyId: string,
            bizType: BizType,
            [<Optional>] accountId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<TradingAccountResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (inner.QueryTradingAccountAsync(
                ?currencyId = CSharpHelpers.valueToOption currencyId,
                ?bizType = Some bizType,
                ?accountId = CSharpHelpers.valueToOption accountId
            ))

    member _.QueryInvestorPositionAsync
        (
            instrumentId: string,
            [<Optional>] exchangeId: string,
            [<Optional>] investUnitId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<InvestorPositionResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (inner.QueryInvestorPositionAsync(
                instrumentId,
                ?exchangeId = CSharpHelpers.valueToOption exchangeId,
                ?investUnitId = CSharpHelpers.valueToOption investUnitId
            ))

    member _.QueryOrderAsync
        (
            exchangeId: string,
            orderSysId: string,
            insertTimeStart: string,
            insertTimeEnd: string,
            instrumentId: string,
            [<Optional>] investUnitId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<OrderUpdateResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (inner.QueryOrderAsync(
                exchangeId,
                orderSysId,
                insertTimeStart,
                insertTimeEnd,
                instrumentId,
                ?investUnitId = CSharpHelpers.valueToOption investUnitId
            ))

    member _.QueryTradeAsync
        (
            [<Optional>] exchangeId: string,
            [<Optional>] tradeId: string,
            [<Optional>] tradeTimeStart: string,
            [<Optional>] tradeTimeEnd: string,
            [<Optional>] investUnitId: string,
            [<Optional>] instrumentId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<TradeUpdateResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (inner.QueryTradeAsync(
                ?exchangeId = CSharpHelpers.valueToOption exchangeId,
                ?tradeId = CSharpHelpers.valueToOption tradeId,
                ?tradeTimeStart = CSharpHelpers.valueToOption tradeTimeStart,
                ?tradeTimeEnd = CSharpHelpers.valueToOption tradeTimeEnd,
                ?investUnitId = CSharpHelpers.valueToOption investUnitId,
                ?instrumentId = CSharpHelpers.valueToOption instrumentId
            ))

    member _.QueryInvestorAsync
        ([<Optional>] cancellationToken: CancellationToken)
        : Task<IReadOnlyList<InvestorResponse>>
        =
        CSharpHelpers.startAsyncList cancellationToken (inner.QueryInvestorAsync())

    member _.QueryInstrumentAsync
        (
            exchangeId: string,
            instrumentId: string,
            exchangeInstId: string,
            productId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<InstrumentResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (inner.QueryInstrumentAsync(exchangeId, instrumentId, exchangeInstId, productId))

    member _.QueryInstrumentMarginRateAsync
        (
            hedgeFlag: HedgeFlag,
            instrumentId: string,
            [<Optional>] exchangeId: string,
            [<Optional>] investUnitId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<InstrumentMarginRateResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (inner.QueryInstrumentMarginRateAsync(
                hedgeFlag,
                instrumentId,
                ?exchangeId = CSharpHelpers.valueToOption exchangeId,
                ?investUnitId = CSharpHelpers.valueToOption investUnitId
            ))

    member _.QueryExchangeMarginRateAsync
        (
            hedgeFlag: HedgeFlag,
            instrumentId: string,
            [<Optional>] exchangeId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<ExchangeMarginRateResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (inner.QueryExchangeMarginRateAsync(hedgeFlag, instrumentId, ?exchangeId = CSharpHelpers.valueToOption exchangeId))

    member _.QueryInstrumentCommissionRateAsync
        (
            instrumentId: string,
            [<Optional>] exchangeId: string,
            [<Optional>] investUnitId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<InstrumentCommissionRateResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (inner.QueryInstrumentCommissionRateAsync(
                instrumentId,
                ?exchangeId = CSharpHelpers.valueToOption exchangeId,
                ?investUnitId = CSharpHelpers.valueToOption investUnitId
            ))

    // ---- Commands (fire-and-forget → Task<int>) ----

    member _.InsertOrderAsync
        (request: InputOrderRequest, [<Optional>] cancellationToken: CancellationToken)
        : Task<int>
        =
        CSharpHelpers.startCommandAsync cancellationToken (inner.InsertOrderAsync(request))

    member _.CancelOrderAsync
        (request: InputOrderActionRequest, [<Optional>] cancellationToken: CancellationToken)
        : Task<int>
        =
        CSharpHelpers.startCommandAsync cancellationToken (inner.CancelOrderAsync(request))

    member internal _.Inner = inner

    interface IDisposable with
        member _.Dispose() = (inner :> IDisposable).Dispose()

type TraderClient with

    // ---- Full Trader projection (mechanically shaped from the F# surface) ----
    member this.FromBankToFutureByFutureAsync
        (
            request: TransferRequest,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<int>
        =
        CSharpHelpers.startCommandAsync
            cancellationToken
            (this.Inner.FromBankToFutureByFutureAsync(request))

    member this.FromFutureToBankByFutureAsync
        (
            request: TransferRequest,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<int>
        =
        CSharpHelpers.startCommandAsync
            cancellationToken
            (this.Inner.FromFutureToBankByFutureAsync(request))

    member this.QueryAccountregisterAsync
        (
            [<Optional>] accountId: string,
            [<Optional>] bankId: string,
            [<Optional>] bankBranchId: string,
            [<Optional>] currencyId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<AccountregisterResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryAccountregisterAsync(?accountId = CSharpHelpers.valueToOption accountId,
                ?bankId = CSharpHelpers.valueToOption bankId,
                ?bankBranchId = CSharpHelpers.valueToOption bankBranchId,
                ?currencyId = CSharpHelpers.valueToOption currencyId))

    member this.QueryBankAccountMoneyByFutureAsync
        (
            request: ReqQueryAccount,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<ReqQueryAccount>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryBankAccountMoneyByFutureAsync(request))

    member this.QueryBrokerTradingAlgosAsync
        (
            exchangeId: string,
            instrumentId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<BrokerTradingAlgosResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryBrokerTradingAlgosAsync(exchangeId,
                instrumentId))

    member this.QueryBrokerTradingParamsAsync
        (
            currencyId: string,
            [<Optional>] accountId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<BrokerTradingParamsResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryBrokerTradingParamsAsync(currencyId,
                ?accountId = CSharpHelpers.valueToOption accountId))

    member this.QueryCfmmcTradingAccountKeyAsync
        (
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<CfmmcTradingAccountKeyResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryCfmmcTradingAccountKeyAsync())

    member this.QueryCfmmcTradingAccountTokenAsync
        (
            [<Optional>] investUnitId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<QueryCfmmcTradingAccountTokenRequest>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryCfmmcTradingAccountTokenAsync(?investUnitId = CSharpHelpers.valueToOption investUnitId))

    member this.QueryClassifiedInstrumentAsync
        (
            tradingType: TradingType,
            classType: ClassType,
            [<Optional>] instrumentId: string,
            [<Optional>] exchangeId: string,
            [<Optional>] exchangeInstId: string,
            [<Optional>] productId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<InstrumentResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryClassifiedInstrumentAsync(tradingType,
                classType,
                ?instrumentId = CSharpHelpers.valueToOption instrumentId,
                ?exchangeId = CSharpHelpers.valueToOption exchangeId,
                ?exchangeInstId = CSharpHelpers.valueToOption exchangeInstId,
                ?productId = CSharpHelpers.valueToOption productId))

    member this.QueryCombActionAsync
        (
            exchangeId: string,
            instrumentId: string,
            [<Optional>] investUnitId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<CombActionResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryCombActionAsync(exchangeId,
                instrumentId,
                ?investUnitId = CSharpHelpers.valueToOption investUnitId))

    member this.QueryCombInstrumentGuardAsync
        (
            exchangeId: string,
            [<Optional>] instrumentId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<CombInstrumentGuardResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryCombInstrumentGuardAsync(exchangeId,
                ?instrumentId = CSharpHelpers.valueToOption instrumentId))

    member this.QueryCombLegAsync
        (
            legInstrumentId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<CombLegResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryCombLegAsync(legInstrumentId))

    member this.QueryCombPromotionParamAsync
        (
            [<Optional>] exchangeId: string,
            [<Optional>] instrumentId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<CombPromotionParamResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryCombPromotionParamAsync(?exchangeId = CSharpHelpers.valueToOption exchangeId,
                ?instrumentId = CSharpHelpers.valueToOption instrumentId))

    member this.QueryContractBankAsync
        (
            bankId: string,
            bankBrchId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<ContractBankResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryContractBankAsync(bankId,
                bankBrchId))

    member this.QueryDepthMarketDataAsync
        (
            instrumentId: string,
            productClass: ProductClass,
            [<Optional>] exchangeId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<DepthMarketData>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryDepthMarketDataAsync(instrumentId,
                productClass,
                ?exchangeId = CSharpHelpers.valueToOption exchangeId))

    member this.QueryEWarrantOffsetAsync
        (
            exchangeId: string,
            instrumentId: string,
            [<Optional>] investUnitId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<EWarrantOffsetResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryEWarrantOffsetAsync(exchangeId,
                instrumentId,
                ?investUnitId = CSharpHelpers.valueToOption investUnitId))

    member this.QueryExchangeAsync
        (
            exchangeId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<ExchangeResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryExchangeAsync(exchangeId))

    member this.QueryExchangeMarginRateAdjustAsync
        (
            instrumentId: string,
            hedgeFlag: HedgeFlag,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<ExchangeMarginRateAdjustResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryExchangeMarginRateAdjustAsync(instrumentId,
                hedgeFlag))

    member this.QueryExchangeRateAsync
        (
            fromCurrencyId: string,
            toCurrencyId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<ExchangeRateResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryExchangeRateAsync(fromCurrencyId,
                toCurrencyId))

    member this.QueryExecOrderAsync
        (
            exchangeId: string,
            execOrderSysId: string,
            insertTimeStart: string,
            insertTimeEnd: string,
            instrumentId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<ExecOrderResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryExecOrderAsync(exchangeId,
                execOrderSysId,
                insertTimeStart,
                insertTimeEnd,
                instrumentId))

    member this.QueryForQuoteAsync
        (
            exchangeId: string,
            insertTimeStart: string,
            insertTimeEnd: string,
            instrumentId: string,
            [<Optional>] investUnitId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<ForQuoteResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryForQuoteAsync(exchangeId,
                insertTimeStart,
                insertTimeEnd,
                instrumentId,
                ?investUnitId = CSharpHelpers.valueToOption investUnitId))

    member this.QueryHedgeCfmAsync
        (
            exchangeId: string,
            orderSysId: string,
            instrumentId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<HedgeCfmResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryHedgeCfmAsync(exchangeId,
                orderSysId,
                instrumentId))

    member this.QueryInstrumentOrderCommRateAsync
        (
            instrumentId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<InstrumentOrderCommRateResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryInstrumentOrderCommRateAsync(instrumentId))

    member this.QueryInvestUnitAsync
        (
            [<Optional>] investUnitId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<InvestUnitResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryInvestUnitAsync(?investUnitId = CSharpHelpers.valueToOption investUnitId))

    member this.QueryInvestorCommodityGroupSpmmMarginAsync
        (
            commodityGroupId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<InvestorCommodityGroupSpmmMarginResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryInvestorCommodityGroupSpmmMarginAsync(commodityGroupId))

    member this.QueryInvestorCommoditySpmmMarginAsync
        (
            commodityId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<InvestorCommoditySpmmMarginResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryInvestorCommoditySpmmMarginAsync(commodityId))

    member this.QueryInvestorInfoCommRecAsync
        (
            instrumentId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<InvestorInfoCommRecResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryInvestorInfoCommRecAsync(instrumentId))

    member this.QueryInvestorPortfMarginRatioAsync
        (
            exchangeId: string,
            [<Optional>] productGroupId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<InvestorPortfMarginRatioResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryInvestorPortfMarginRatioAsync(exchangeId,
                ?productGroupId = CSharpHelpers.valueToOption productGroupId))

    member this.QueryInvestorPortfSettingAsync
        (
            exchangeId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<InvestorPortfSettingResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryInvestorPortfSettingAsync(exchangeId))

    member this.QueryInvestorPositionCombineDetailAsync
        (
            combInstrumentId: string,
            [<Optional>] exchangeId: string,
            [<Optional>] investUnitId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<InvestorPositionCombineDetailResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryInvestorPositionCombineDetailAsync(combInstrumentId,
                ?exchangeId = CSharpHelpers.valueToOption exchangeId,
                ?investUnitId = CSharpHelpers.valueToOption investUnitId))

    member this.QueryInvestorPositionDetailAsync
        (
            instrumentId: string,
            [<Optional>] exchangeId: string,
            [<Optional>] investUnitId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<InvestorPositionDetailResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryInvestorPositionDetailAsync(instrumentId,
                ?exchangeId = CSharpHelpers.valueToOption exchangeId,
                ?investUnitId = CSharpHelpers.valueToOption investUnitId))

    member this.QueryInvestorProdRcamsMarginAsync
        (
            combProductId: string,
            productGroupId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<InvestorProdRcamsMarginResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryInvestorProdRcamsMarginAsync(combProductId,
                productGroupId))

    member this.QueryInvestorProdRuleMarginAsync
        (
            exchangeId: string,
            prodFamilyCode: string,
            commodityGroupId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<InvestorProdRuleMarginResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryInvestorProdRuleMarginAsync(exchangeId,
                prodFamilyCode,
                commodityGroupId))

    member this.QueryInvestorProdSpbmDetailAsync
        (
            exchangeId: string,
            prodFamilyCode: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<InvestorProdSpbmDetailResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryInvestorProdSpbmDetailAsync(exchangeId,
                prodFamilyCode))

    member this.QueryInvestorProductGroupMarginAsync
        (
            productGroupId: string,
            [<Optional>] exchangeId: string,
            [<Optional>] investUnitId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<InvestorProductGroupMarginResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryInvestorProductGroupMarginAsync(productGroupId,
                ?exchangeId = CSharpHelpers.valueToOption exchangeId,
                ?investUnitId = CSharpHelpers.valueToOption investUnitId))

    member this.QueryInvestorProductGroupMarginAsync
        (
            productGroupId: string,
            hedgeFlag: HedgeFlag,
            [<Optional>] exchangeId: string,
            [<Optional>] investUnitId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<InvestorProductGroupMarginResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryInvestorProductGroupMarginAsync(productGroupId,
                ?hedgeFlag = Some hedgeFlag,
                ?exchangeId = CSharpHelpers.valueToOption exchangeId,
                ?investUnitId = CSharpHelpers.valueToOption investUnitId))

    member this.QueryMaxOrderVolumeAsync
        (
            direction: Direction,
            offsetFlag: OffsetFlag,
            hedgeFlag: HedgeFlag,
            instrumentId: string,
            [<Optional>] maxVolume: Nullable<int>,
            [<Optional>] exchangeId: string,
            [<Optional>] investUnitId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<QryMaxOrderVolumeRequest>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryMaxOrderVolumeAsync(direction,
                offsetFlag,
                hedgeFlag,
                instrumentId,
                ?maxVolume = CSharpHelpers.nullableToOption maxVolume,
                ?exchangeId = CSharpHelpers.valueToOption exchangeId,
                ?investUnitId = CSharpHelpers.valueToOption investUnitId))

    member this.QueryMmInstrumentCommissionRateAsync
        (
            instrumentId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<MmInstrumentCommissionRateResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryMmInstrumentCommissionRateAsync(instrumentId))

    member this.QueryMmOptionInstrCommRateAsync
        (
            instrumentId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<MmOptionInstrCommRateResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryMmOptionInstrCommRateAsync(instrumentId))

    member this.QueryNoticeAsync
        (
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<NoticeResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryNoticeAsync())

    member this.QueryOffsetSettingAsync
        (
            productId: string,
            offsetType: OffsetType,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<OffsetSettingResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryOffsetSettingAsync(productId,
                offsetType))

    member this.QueryOptionInstrCommRateAsync
        (
            instrumentId: string,
            [<Optional>] exchangeId: string,
            [<Optional>] investUnitId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<OptionInstrCommRateResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryOptionInstrCommRateAsync(instrumentId,
                ?exchangeId = CSharpHelpers.valueToOption exchangeId,
                ?investUnitId = CSharpHelpers.valueToOption investUnitId))

    member this.QueryOptionInstrTradeCostAsync
        (
            instrumentId: string,
            hedgeFlag: HedgeFlag,
            inputPrice: decimal,
            underlyingPrice: decimal,
            [<Optional>] exchangeId: string,
            [<Optional>] investUnitId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<OptionInstrTradeCostResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryOptionInstrTradeCostAsync(instrumentId,
                hedgeFlag,
                inputPrice,
                underlyingPrice,
                ?exchangeId = CSharpHelpers.valueToOption exchangeId,
                ?investUnitId = CSharpHelpers.valueToOption investUnitId))

    member this.QueryOptionSelfCloseAsync
        (
            exchangeId: string,
            optionSelfCloseSysId: string,
            insertTimeStart: string,
            insertTimeEnd: string,
            instrumentId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<OptionSelfCloseResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryOptionSelfCloseAsync(exchangeId,
                optionSelfCloseSysId,
                insertTimeStart,
                insertTimeEnd,
                instrumentId))

    member this.QueryParkedOrderActionAsync
        (
            exchangeId: string,
            instrumentId: string,
            [<Optional>] investUnitId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<ParkedOrderAction>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryParkedOrderActionAsync(exchangeId,
                instrumentId,
                ?investUnitId = CSharpHelpers.valueToOption investUnitId))

    member this.QueryParkedOrderAsync
        (
            exchangeId: string,
            instrumentId: string,
            [<Optional>] investUnitId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<ParkedOrder>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryParkedOrderAsync(exchangeId,
                instrumentId,
                ?investUnitId = CSharpHelpers.valueToOption investUnitId))

    member this.QueryProductAsync
        (
            productId: string,
            [<Optional>] exchangeId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<ProductResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryProductAsync(productId, ?exchangeId = CSharpHelpers.valueToOption exchangeId))

    member this.QueryProductAsync
        (
            productId: string,
            productClass: ProductClass,
            [<Optional>] exchangeId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<ProductResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryProductAsync(productId,
                ?productClass = Some productClass,
                ?exchangeId = CSharpHelpers.valueToOption exchangeId))

    member this.QueryProductExchRateAsync
        (
            productId: string,
            [<Optional>] exchangeId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<ProductExchRateResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryProductExchRateAsync(productId,
                ?exchangeId = CSharpHelpers.valueToOption exchangeId))

    member this.QueryProductGroupAsync
        (
            exchangeId: string,
            productId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<ProductGroupResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryProductGroupAsync(exchangeId,
                productId))

    member this.QueryQuoteAsync
        (
            exchangeId: string,
            quoteSysId: string,
            insertTimeStart: string,
            insertTimeEnd: string,
            instrumentId: string,
            [<Optional>] investUnitId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<QuoteResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryQuoteAsync(exchangeId,
                quoteSysId,
                insertTimeStart,
                insertTimeEnd,
                instrumentId,
                ?investUnitId = CSharpHelpers.valueToOption investUnitId))

    member this.QueryRcamsCombProductInfoAsync
        (
            productId: string,
            combProductId: string,
            productGroupId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<RcamsCombProductInfoResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryRcamsCombProductInfoAsync(productId,
                combProductId,
                productGroupId))

    member this.QueryRcamsInstrParameterAsync
        (
            productId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<RcamsInstrParameterResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryRcamsInstrParameterAsync(productId))

    member this.QueryRcamsInterParameterAsync
        (
            productGroupId: string,
            combProduct1: string,
            combProduct2: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<RcamsInterParameterResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryRcamsInterParameterAsync(productGroupId,
                combProduct1,
                combProduct2))

    member this.QueryRcamsIntraParameterAsync
        (
            combProductId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<RcamsIntraParameterResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryRcamsIntraParameterAsync(combProductId))

    member this.QueryRcamsInvestorCombPositionAsync
        (
            instrumentId: string,
            combInstrumentId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<RcamsInvestorCombPositionResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryRcamsInvestorCombPositionAsync(instrumentId,
                combInstrumentId))

    member this.QueryRcamsShortOptAdjustParamAsync
        (
            combProductId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<RcamsShortOptAdjustParamResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryRcamsShortOptAdjustParamAsync(combProductId))

    member this.QueryRiskSettleInvstPositionAsync
        (
            [<Optional>] instrumentId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<RiskSettleInvstPositionResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryRiskSettleInvstPositionAsync(?instrumentId = CSharpHelpers.valueToOption instrumentId))

    member this.QueryRiskSettleProductStatusAsync
        (
            [<Optional>] productId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<RiskSettleProductStatusResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryRiskSettleProductStatusAsync(?productId = CSharpHelpers.valueToOption productId))

    member this.QueryRuleInstrParameterAsync
        (
            exchangeId: string,
            instrumentId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<RuleInstrParameterResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryRuleInstrParameterAsync(exchangeId,
                instrumentId))

    member this.QueryRuleInterParameterAsync
        (
            commodityGroupId: string,
            exchangeId: string,
            leg1ProdFamilyCode: string,
            [<Optional>] leg2ProdFamilyCode: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<RuleInterParameterResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryRuleInterParameterAsync(commodityGroupId,
                exchangeId,
                leg1ProdFamilyCode,
                ?leg2ProdFamilyCode = CSharpHelpers.valueToOption leg2ProdFamilyCode))

    member this.QueryRuleIntraParameterAsync
        (
            exchangeId: string,
            prodFamilyCode: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<RuleIntraParameterResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryRuleIntraParameterAsync(exchangeId,
                prodFamilyCode))

    member this.QuerySecAgentAcIdMapAsync
        (
            accountId: string,
            currencyId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<SecAgentAcIdMapResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QuerySecAgentAcIdMapAsync(accountId,
                currencyId))

    member this.QuerySecAgentCheckModeAsync
        (
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<SecAgentCheckModeResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QuerySecAgentCheckModeAsync())

    member this.QuerySecAgentTradeInfoAsync
        (
            brokerSecAgentId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<SecAgentTradeInfoResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QuerySecAgentTradeInfoAsync(brokerSecAgentId))

    member this.QuerySecAgentTradingAccountAsync
        (
            currencyId: string,
            [<Optional>] accountId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<TradingAccountResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QuerySecAgentTradingAccountAsync(currencyId, ?accountId = CSharpHelpers.valueToOption accountId))

    member this.QuerySecAgentTradingAccountAsync
        (
            currencyId: string,
            bizType: BizType,
            [<Optional>] accountId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<TradingAccountResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QuerySecAgentTradingAccountAsync(currencyId,
                ?bizType = Some bizType,
                ?accountId = CSharpHelpers.valueToOption accountId))

    member this.QuerySettlementInfoAsync
        (
            tradingDay: DateOnly,
            [<Optional>] accountId: string,
            [<Optional>] currencyId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<SettlementInfoResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QuerySettlementInfoAsync(tradingDay,
                ?accountId = CSharpHelpers.valueToOption accountId,
                ?currencyId = CSharpHelpers.valueToOption currencyId))

    member this.QuerySettlementInfoConfirmAsync
        (
            [<Optional>] accountId: string,
            [<Optional>] currencyId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<SettlementInfoConfirm>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QuerySettlementInfoConfirmAsync(?accountId = CSharpHelpers.valueToOption accountId,
                ?currencyId = CSharpHelpers.valueToOption currencyId))

    member this.QuerySpbmAddOnInterParameterAsync
        (
            exchangeId: string,
            leg1ProdFamilyCode: string,
            leg2ProdFamilyCode: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<SpbmAddOnInterParameterResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QuerySpbmAddOnInterParameterAsync(exchangeId,
                leg1ProdFamilyCode,
                leg2ProdFamilyCode))

    member this.QuerySpbmFutureParameterAsync
        (
            exchangeId: string,
            instrumentId: string,
            prodFamilyCode: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<SpbmFutureParameterResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QuerySpbmFutureParameterAsync(exchangeId,
                instrumentId,
                prodFamilyCode))

    member this.QuerySpbmInterParameterAsync
        (
            exchangeId: string,
            leg1ProdFamilyCode: string,
            leg2ProdFamilyCode: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<SpbmInterParameterResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QuerySpbmInterParameterAsync(exchangeId,
                leg1ProdFamilyCode,
                leg2ProdFamilyCode))

    member this.QuerySpbmIntraParameterAsync
        (
            exchangeId: string,
            prodFamilyCode: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<SpbmIntraParameterResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QuerySpbmIntraParameterAsync(exchangeId,
                prodFamilyCode))

    member this.QuerySpbmInvestorPortfDefAsync
        (
            exchangeId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<SpbmInvestorPortfDefResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QuerySpbmInvestorPortfDefAsync(exchangeId))

    member this.QuerySpbmOptionParameterAsync
        (
            exchangeId: string,
            instrumentId: string,
            prodFamilyCode: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<SpbmOptionParameterResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QuerySpbmOptionParameterAsync(exchangeId,
                instrumentId,
                prodFamilyCode))

    member this.QuerySpbmPortfDefinitionAsync
        (
            exchangeId: string,
            portfolioDefId: string,
            prodFamilyCode: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<SpbmPortfDefinitionResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QuerySpbmPortfDefinitionAsync(exchangeId,
                portfolioDefId,
                prodFamilyCode))

    member this.QuerySpdApplyAsync
        (
            exchangeId: string,
            orderSysId: string,
            firstLegInstrumentId: string,
            secondLegInstrumentId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<SpdApplyResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QuerySpdApplyAsync(exchangeId,
                orderSysId,
                firstLegInstrumentId,
                secondLegInstrumentId))

    member this.QuerySpmmInstParamAsync
        (
            instrumentId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<SpmmInstParamResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QuerySpmmInstParamAsync(instrumentId))

    member this.QuerySpmmProductParamAsync
        (
            productId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<SpmmProductParamResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QuerySpmmProductParamAsync(productId))

    member this.QueryTraderOfferAsync
        (
            [<Optional>] exchangeId: string,
            [<Optional>] participantId: string,
            [<Optional>] traderId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<TraderOfferResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryTraderOfferAsync(?exchangeId = CSharpHelpers.valueToOption exchangeId,
                ?participantId = CSharpHelpers.valueToOption participantId,
                ?traderId = CSharpHelpers.valueToOption traderId))

    member this.QueryTradingCodeAsync
        (
            exchangeId: string,
            clientId: string,
            clientIdType: ClientIdType,
            [<Optional>] investUnitId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<TradingCodeResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryTradingCodeAsync(exchangeId,
                clientId,
                clientIdType,
                ?investUnitId = CSharpHelpers.valueToOption investUnitId))

    member this.QueryTradingNoticeAsync
        (
            [<Optional>] investUnitId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<TradingNoticeResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryTradingNoticeAsync(?investUnitId = CSharpHelpers.valueToOption investUnitId))

    member this.QueryTransferBankAsync
        (
            [<Optional>] bankId: string,
            [<Optional>] bankBrchId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<TransferBankResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryTransferBankAsync(?bankId = CSharpHelpers.valueToOption bankId,
                ?bankBrchId = CSharpHelpers.valueToOption bankBrchId))

    member this.QueryTransferSerialAsync
        (
            accountId: string,
            bankId: string,
            currencyId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<TransferSerialResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryTransferSerialAsync(accountId,
                bankId,
                currencyId))

    member this.QueryUserSessionAsync
        (
            frontId: int,
            sessionId: int,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<UserSessionResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.QueryUserSessionAsync(frontId,
                sessionId))

    member this.ReqBatchOrderActionAsync
        (
            frontId: int,
            sessionId: int,
            [<Optional>] exchangeId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<int>
        =
        CSharpHelpers.startCommandAsync
            cancellationToken
            (this.Inner.ReqBatchOrderActionAsync(frontId,
                sessionId,
                ?exchangeId = CSharpHelpers.valueToOption exchangeId))

    member this.ReqCancelOffsetSettingAsync
        (
            request: InputOffsetSettingRequest,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<int>
        =
        CSharpHelpers.startCommandAsync
            cancellationToken
            (this.Inner.ReqCancelOffsetSettingAsync(request))

    member this.ReqCombActionInsertAsync
        (
            request: InputCombActionRequest,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<int>
        =
        CSharpHelpers.startCommandAsync
            cancellationToken
            (this.Inner.ReqCombActionInsertAsync(request))

    member this.ReqExecOrderActionAsync
        (
            request: InputExecOrderActionRequest,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<int>
        =
        CSharpHelpers.startCommandAsync
            cancellationToken
            (this.Inner.ReqExecOrderActionAsync(request))

    member this.ReqExecOrderInsertAsync
        (
            request: InputExecOrderRequest,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<int>
        =
        CSharpHelpers.startCommandAsync
            cancellationToken
            (this.Inner.ReqExecOrderInsertAsync(request))

    member this.ReqForQuoteInsertAsync
        (
            instrumentId: string,
            [<Optional>] exchangeId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<int>
        =
        CSharpHelpers.startCommandAsync
            cancellationToken
            (this.Inner.ReqForQuoteInsertAsync(instrumentId,
                ?exchangeId = CSharpHelpers.valueToOption exchangeId))

    member this.ReqGenSmsCodeAsync
        (
            mobile: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<GenSmsCodeResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.ReqGenSmsCodeAsync(mobile))

    member this.ReqGenUserCaptchaAsync
        (
            [<Optional>] tradingDay: Nullable<DateOnly>,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<GenUserCaptchaResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.ReqGenUserCaptchaAsync(?tradingDay = CSharpHelpers.nullableToOption tradingDay))

    member this.ReqGenUserTextAsync
        (
            [<Optional>] tradingDay: Nullable<DateOnly>,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<GenUserTextResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.ReqGenUserTextAsync(?tradingDay = CSharpHelpers.nullableToOption tradingDay))

    member this.ReqHedgeCfmActionAsync
        (
            request: InputHedgeCfmActionRequest,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<int>
        =
        CSharpHelpers.startCommandAsync
            cancellationToken
            (this.Inner.ReqHedgeCfmActionAsync(request))

    member this.ReqHedgeCfmAsync
        (
            request: InputHedgeCfmRequest,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<int>
        =
        CSharpHelpers.startCommandAsync
            cancellationToken
            (this.Inner.ReqHedgeCfmAsync(request))

    member this.ReqOffsetSettingAsync
        (
            request: InputOffsetSettingRequest,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<int>
        =
        CSharpHelpers.startCommandAsync
            cancellationToken
            (this.Inner.ReqOffsetSettingAsync(request))

    member this.ReqOptionSelfCloseActionAsync
        (
            request: InputOptionSelfCloseActionRequest,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<int>
        =
        CSharpHelpers.startCommandAsync
            cancellationToken
            (this.Inner.ReqOptionSelfCloseActionAsync(request))

    member this.ReqOptionSelfCloseInsertAsync
        (
            request: InputOptionSelfCloseRequest,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<int>
        =
        CSharpHelpers.startCommandAsync
            cancellationToken
            (this.Inner.ReqOptionSelfCloseInsertAsync(request))

    member this.ReqParkedOrderActionAsync
        (
            request: ParkedOrderAction,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<ParkedOrderAction>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.ReqParkedOrderActionAsync(request))

    member this.ReqParkedOrderInsertAsync
        (
            request: ParkedOrder,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<ParkedOrder>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.ReqParkedOrderInsertAsync(request))

    member this.ReqQuoteActionAsync
        (
            request: InputQuoteActionRequest,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<int>
        =
        CSharpHelpers.startCommandAsync
            cancellationToken
            (this.Inner.ReqQuoteActionAsync(request))

    member this.ReqQuoteInsertAsync
        (
            request: InputQuoteRequest,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<int>
        =
        CSharpHelpers.startCommandAsync
            cancellationToken
            (this.Inner.ReqQuoteInsertAsync(request))

    member this.ReqRemoveParkedOrderActionAsync
        (
            parkedOrderActionId: string,
            [<Optional>] investUnitId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<RemoveParkedOrderAction>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.ReqRemoveParkedOrderActionAsync(parkedOrderActionId,
                ?investUnitId = CSharpHelpers.valueToOption investUnitId))

    member this.ReqRemoveParkedOrderAsync
        (
            parkedOrderId: string,
            [<Optional>] investUnitId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<RemoveParkedOrder>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.ReqRemoveParkedOrderAsync(parkedOrderId,
                ?investUnitId = CSharpHelpers.valueToOption investUnitId))

    member this.ReqSpdApplyActionAsync
        (
            request: InputSpdApplyActionRequest,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<int>
        =
        CSharpHelpers.startCommandAsync
            cancellationToken
            (this.Inner.ReqSpdApplyActionAsync(request))

    member this.ReqSpdApplyAsync
        (
            request: InputSpdApplyRequest,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<int>
        =
        CSharpHelpers.startCommandAsync
            cancellationToken
            (this.Inner.ReqSpdApplyAsync(request))

    member this.ReqTradingAccountPasswordUpdateAsync
        (
            accountId: string,
            oldPassword: string,
            newPassword: string,
            currencyId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<TradingAccountPasswordUpdate>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.ReqTradingAccountPasswordUpdateAsync(accountId,
                oldPassword,
                newPassword,
                currencyId))

    member this.ReqUserAuthMethodAsync
        (
            [<Optional>] tradingDay: Nullable<DateOnly>,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<UserAuthMethodResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.ReqUserAuthMethodAsync(?tradingDay = CSharpHelpers.nullableToOption tradingDay))

    member this.ReqUserLoginWithCaptchaAsync
        (
            captcha: string,
            [<Optional>] tradingDay: Nullable<DateOnly>,
            [<Optional>] userProductInfo: string,
            [<Optional>] interfaceProductInfo: string,
            [<Optional>] protocolInfo: string,
            [<Optional>] macAddress: string,
            [<Optional>] loginRemark: string,
            [<Optional>] clientIpPort: Nullable<int>,
            [<Optional>] clientIpAddress: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<UserLoginResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.ReqUserLoginWithCaptchaAsync(captcha,
                ?tradingDay = CSharpHelpers.nullableToOption tradingDay,
                ?userProductInfo = CSharpHelpers.valueToOption userProductInfo,
                ?interfaceProductInfo = CSharpHelpers.valueToOption interfaceProductInfo,
                ?protocolInfo = CSharpHelpers.valueToOption protocolInfo,
                ?macAddress = CSharpHelpers.valueToOption macAddress,
                ?loginRemark = CSharpHelpers.valueToOption loginRemark,
                ?clientIpPort = CSharpHelpers.nullableToOption clientIpPort,
                ?clientIpAddress = CSharpHelpers.valueToOption clientIpAddress))

    member this.ReqUserLoginWithOtpAsync
        (
            otpPassword: string,
            [<Optional>] tradingDay: Nullable<DateOnly>,
            [<Optional>] userProductInfo: string,
            [<Optional>] interfaceProductInfo: string,
            [<Optional>] protocolInfo: string,
            [<Optional>] macAddress: string,
            [<Optional>] loginRemark: string,
            [<Optional>] clientIpPort: Nullable<int>,
            [<Optional>] clientIpAddress: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<UserLoginResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.ReqUserLoginWithOtpAsync(otpPassword,
                ?tradingDay = CSharpHelpers.nullableToOption tradingDay,
                ?userProductInfo = CSharpHelpers.valueToOption userProductInfo,
                ?interfaceProductInfo = CSharpHelpers.valueToOption interfaceProductInfo,
                ?protocolInfo = CSharpHelpers.valueToOption protocolInfo,
                ?macAddress = CSharpHelpers.valueToOption macAddress,
                ?loginRemark = CSharpHelpers.valueToOption loginRemark,
                ?clientIpPort = CSharpHelpers.nullableToOption clientIpPort,
                ?clientIpAddress = CSharpHelpers.valueToOption clientIpAddress))

    member this.ReqUserLoginWithTextAsync
        (
            text: string,
            [<Optional>] tradingDay: Nullable<DateOnly>,
            [<Optional>] userProductInfo: string,
            [<Optional>] interfaceProductInfo: string,
            [<Optional>] protocolInfo: string,
            [<Optional>] macAddress: string,
            [<Optional>] loginRemark: string,
            [<Optional>] clientIpPort: Nullable<int>,
            [<Optional>] clientIpAddress: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<UserLoginResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.ReqUserLoginWithTextAsync(text,
                ?tradingDay = CSharpHelpers.nullableToOption tradingDay,
                ?userProductInfo = CSharpHelpers.valueToOption userProductInfo,
                ?interfaceProductInfo = CSharpHelpers.valueToOption interfaceProductInfo,
                ?protocolInfo = CSharpHelpers.valueToOption protocolInfo,
                ?macAddress = CSharpHelpers.valueToOption macAddress,
                ?loginRemark = CSharpHelpers.valueToOption loginRemark,
                ?clientIpPort = CSharpHelpers.nullableToOption clientIpPort,
                ?clientIpAddress = CSharpHelpers.valueToOption clientIpAddress))

    member this.ReqUserPasswordUpdateAsync
        (
            oldPassword: string,
            newPassword: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<UserPasswordUpdate>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (this.Inner.ReqUserPasswordUpdateAsync(oldPassword,
                newPassword))
