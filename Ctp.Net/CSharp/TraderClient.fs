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
    let notificationEvent = Event<EventHandler<obj>, obj>()
    let asyncErrorEvent = Event<EventHandler<CtpEventArgs<obj, RspInfo>>, CtpEventArgs<obj, RspInfo>>()

    do
        inner.FrontConnected.Add(fun () -> frontConnectedEvent.Trigger(null, EventArgs.Empty))
        inner.FrontDisconnected.Add(fun r -> frontDisconnectedEvent.Trigger(null, r))
        inner.HeartBeatWarning.Add(fun l -> heartBeatWarningEvent.Trigger(null, l))
        inner.PrivateSeqNoReceived.Add(fun s -> privateSeqNoEvent.Trigger(null, s))
        inner.RspError.Add(fun i -> rspErrorEvent.Trigger(null, i))
        inner.OrderReceived.Add(fun o -> orderEvent.Trigger(null, o))
        inner.TradeReceived.Add(fun t -> tradeEvent.Trigger(null, t))
        inner.NotificationReceived.Add(fun n -> notificationEvent.Trigger(null, n))

        inner.AsyncErrorReceived.Add(fun data ->
            asyncErrorEvent.Trigger(null, CtpEventArgs(data, Unchecked.defaultof<RspInfo>)))

    new(options: CtpOptions) = new TraderClient(new Ctp.Net.TraderClient(options))

    new
        (
            options: CtpOptions,
            encodings: CtpEncodingOptions,
            [<Optional>] privateTopicResumeType: Nullable<int>,
            [<Optional>] privateTopicSequenceNo: Nullable<int>,
            [<Optional>] publicTopicResumeType: Nullable<int>,
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
    member _.NotificationReceived = notificationEvent.Publish

    [<CLIEvent>]
    member _.AsyncErrorReceived = asyncErrorEvent.Publish

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
            (inner.QueryTradingAccountAsync(?currencyId = Option.ofObj currencyId, ?accountId = Option.ofObj accountId))

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
                ?exchangeId = Option.ofObj exchangeId,
                ?investUnitId = Option.ofObj investUnitId
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
                ?investUnitId = Option.ofObj investUnitId
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
                ?exchangeId = Option.ofObj exchangeId,
                ?tradeId = Option.ofObj tradeId,
                ?tradeTimeStart = Option.ofObj tradeTimeStart,
                ?tradeTimeEnd = Option.ofObj tradeTimeEnd,
                ?investUnitId = Option.ofObj investUnitId,
                ?instrumentId = Option.ofObj instrumentId
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

    interface IDisposable with
        member _.Dispose() = (inner :> IDisposable).Dispose()
