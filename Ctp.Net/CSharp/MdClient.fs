namespace Ctp.Net.CSharp

open System
open System.Runtime.InteropServices
open System.Threading
open System.Threading.Tasks
open System.Collections.Generic
open Ctp.Net
open Ctp.Net.Bridge
open Microsoft.Extensions.Logging

type MdClient private (inner: Ctp.Net.MdClient) =

    let frontConnectedEvent = Event<EventHandler, EventArgs>()
    let frontDisconnectedEvent = Event<EventHandler<int>, int>()
    let heartBeatWarningEvent = Event<EventHandler<int>, int>()
    let rspErrorEvent = Event<EventHandler<RspInfo>, RspInfo>()
    let depthMarketDataEvent = Event<EventHandler<DepthMarketData>, DepthMarketData>()

    do
        inner.FrontConnected.Add(fun () -> frontConnectedEvent.Trigger(null, EventArgs.Empty))
        inner.FrontDisconnected.Add(fun r -> frontDisconnectedEvent.Trigger(null, r))
        inner.HeartBeatWarning.Add(fun l -> heartBeatWarningEvent.Trigger(null, l))
        inner.RspError.Add(fun i -> rspErrorEvent.Trigger(null, i))
        inner.DepthMarketDataReceived.Add(fun d -> depthMarketDataEvent.Trigger(null, d))

    new(options: CtpOptions, [<Optional>] autoResubscribe: bool) =
        new MdClient(new Ctp.Net.MdClient(options, ?autoResubscribe = (if autoResubscribe then Some true else None)))

    new
        (
            options: CtpOptions,
            encodings: CtpEncodingOptions,
            [<Optional>] useUdp: bool,
            [<Optional>] useMulticast: bool,
            [<Optional>] loggerFactory: ILoggerFactory,
            flowControl: CtpFlowControlOptions,
            [<Optional>] autoResubscribe: bool
        )
        =
        let nullToOpt (v: 'T) = if obj.ReferenceEquals(box v, null) then None else Some v

        new MdClient(
            new Ctp.Net.MdClient(
                options,
                ?encodings = nullToOpt encodings,
                ?useUdp = (if useUdp then Some useUdp else None),
                ?useMulticast = (if useMulticast then Some useMulticast else None),
                ?loggerFactory = CSharpHelpers.nullToOption loggerFactory,
                ?flowControl = nullToOpt flowControl,
                ?autoResubscribe = (if autoResubscribe then Some true else None)
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

    interface IDisposable with
        member _.Dispose() = (inner :> IDisposable).Dispose()
