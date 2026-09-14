namespace Ctp.Net.Next.CSharp

open System
open System.Runtime.InteropServices
open System.Threading
open System.Threading.Tasks
open System.Collections.Generic
open Ctp.Net.Next
open Ctp.Net.Next.Bridge
open Microsoft.Extensions.Logging

type MdClient private (inner: Ctp.Net.Next.MdClient) =

    let frontConnectedEvent = Event<EventHandler, EventArgs>()
    let frontDisconnectedEvent = Event<EventHandler<int>, int>()
    let heartBeatWarningEvent = Event<EventHandler<int>, int>()
    let rspErrorEvent = Event<EventHandler<RspInfo>, RspInfo>()
    let depthMarketDataEvent = Event<EventHandler<DepthMarketData>, DepthMarketData>()
    let multicastInstrumentEvent = Event<EventHandler<MulticastInstrumentResponse>, MulticastInstrumentResponse>()
    let forQuoteRspEvent = Event<EventHandler<ForQuoteRspResponse>, ForQuoteRspResponse>()

    do
        inner.FrontConnected.Add(fun () -> frontConnectedEvent.Trigger(null, EventArgs.Empty))
        inner.FrontDisconnected.Add(fun r -> frontDisconnectedEvent.Trigger(null, r))
        inner.HeartBeatWarning.Add(fun l -> heartBeatWarningEvent.Trigger(null, l))
        inner.RspError.Add(fun i -> rspErrorEvent.Trigger(null, i))
        inner.DepthMarketDataReceived.Add(fun d -> depthMarketDataEvent.Trigger(null, d))
        inner.MulticastInstrumentReceived.Add(fun i -> multicastInstrumentEvent.Trigger(null, i))
        inner.ForQuoteRspReceived.Add(fun i -> forQuoteRspEvent.Trigger(null, i))

    new(options: CtpOptions, [<Optional; DefaultParameterValue(true)>] autoResubscribe: bool) =
        new MdClient(new Ctp.Net.Next.MdClient(options, ?autoResubscribe = Some autoResubscribe))

    new(configuration: MdClientOptions) =
        if isNull (box configuration) then
            nullArg (nameof configuration)

        new MdClient(
            new Ctp.Net.Next.MdClient(
                configuration.Options,
                ?encodings = CSharpHelpers.valueToOption configuration.Encodings,
                ?useUdp = Some configuration.UseUdp,
                ?useMulticast = Some configuration.UseMulticast,
                ?loggerFactory = CSharpHelpers.nullToOption configuration.LoggerFactory,
                ?flowControl = CSharpHelpers.valueToOption configuration.FlowControl,
                ?autoResubscribe = Some configuration.AutoResubscribe,
                ?endpoint = Some configuration.Endpoint
            )
        )

    new(options: CtpOptions, endpoint: CtpEndpoint) =
        new MdClient(new Ctp.Net.Next.MdClient(options, ?endpoint = Some endpoint))

    new
        (
            options: CtpOptions,
            encodings: CtpEncodingOptions,
            [<Optional>] useUdp: bool,
            [<Optional>] useMulticast: bool,
            [<Optional>] loggerFactory: ILoggerFactory,
            flowControl: CtpFlowControlOptions,
            [<Optional; DefaultParameterValue(true)>] autoResubscribe: bool
        )
        =
        let nullToOpt (v: 'T) = if obj.ReferenceEquals(box v, null) then None else Some v

        new MdClient(
            new Ctp.Net.Next.MdClient(
                options,
                ?encodings = nullToOpt encodings,
                ?useUdp = (if useUdp then Some useUdp else None),
                ?useMulticast = (if useMulticast then Some useMulticast else None),
                ?loggerFactory = CSharpHelpers.nullToOption loggerFactory,
                ?flowControl = nullToOpt flowControl,
                ?autoResubscribe = Some autoResubscribe
            )
        )

    [<CLIEvent>]
    member _.FrontConnected = frontConnectedEvent.Publish

    [<CLIEvent>]
    member _.FrontDisconnected = frontDisconnectedEvent.Publish

    [<CLIEvent>]
    member _.HeartBeatWarning = heartBeatWarningEvent.Publish

    [<CLIEvent>]
    member _.RspError = rspErrorEvent.Publish

    [<CLIEvent>]
    member _.DepthMarketDataReceived = depthMarketDataEvent.Publish

    [<CLIEvent>]
    member _.MulticastInstrumentReceived = multicastInstrumentEvent.Publish

    [<CLIEvent>]
    member _.ForQuoteRspReceived = forQuoteRspEvent.Publish

    member _.GetApiVersion() = inner.GetApiVersion()

    member _.GetTradingDay() = inner.GetTradingDay()

    member _.RegisterNameServer(nsAddress: string) = inner.RegisterNameServer(nsAddress)

    member _.RegisterFensUserInfo(request: FensUserInfoRequest) = inner.RegisterFensUserInfo(request)

    member _.ConnectAsync
        (
            [<Optional>] timeout: Nullable<TimeSpan>,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task
        =
        let comp =
            match CSharpHelpers.nullableToOption timeout with
            | Some t -> inner.Connect(timeout = t)
            | None -> inner.Connect()

        CSharpHelpers.startConnectAsync cancellationToken comp

    member _.Join() = inner.Join()

    member _.LoginAsync
        ([<Optional>] cancellationToken: CancellationToken)
        : Task<UserLoginResponse>
        =
        CSharpHelpers.startAsync cancellationToken (inner.LoginAsync())

    member _.LogoutAsync
        ([<Optional>] cancellationToken: CancellationToken)
        : Task<UserLogoutResponse>
        =
        CSharpHelpers.startAsync cancellationToken (inner.LogoutAsync())

    member _.SubscribeMarketDataAsync
        (instrumentIds: string seq, [<Optional>] cancellationToken: CancellationToken)
        : Task<IReadOnlyList<string>>
        =
        CSharpHelpers.startAsyncList cancellationToken (inner.SubscribeMarketDataAsync(instrumentIds))

    member _.UnsubscribeMarketDataAsync
        (instrumentIds: string seq, [<Optional>] cancellationToken: CancellationToken)
        : Task<IReadOnlyList<string>>
        =
        CSharpHelpers.startAsyncList cancellationToken (inner.UnsubscribeMarketDataAsync(instrumentIds))

    member _.SubscribeForQuoteRspAsync
        (instrumentIds: string seq, [<Optional>] cancellationToken: CancellationToken)
        : Task<IReadOnlyList<string>>
        =
        CSharpHelpers.startAsyncList cancellationToken (inner.SubscribeForQuoteRspAsync(instrumentIds))

    member _.UnsubscribeForQuoteRspAsync
        (instrumentIds: string seq, [<Optional>] cancellationToken: CancellationToken)
        : Task<IReadOnlyList<string>>
        =
        CSharpHelpers.startAsyncList cancellationToken (inner.UnsubscribeForQuoteRspAsync(instrumentIds))

    member _.QueryMulticastInstrumentAsync
        (
            topicId: int,
            [<Optional>] instrumentId: string,
            [<Optional>] cancellationToken: CancellationToken
        )
        : Task<IReadOnlyList<MulticastInstrumentResponse>>
        =
        CSharpHelpers.startAsyncList
            cancellationToken
            (inner.QueryMulticastInstrumentAsync(topicId, ?instrumentId = CSharpHelpers.valueToOption instrumentId))

    interface IDisposable with
        member _.Dispose() = (inner :> IDisposable).Dispose()
