namespace Ctp.Net.Tests

open Xunit
open System
open Ctp.Net
open System.Text
open Ctp.Net.Bridge
open System.Threading
open System.Collections.Generic
open Microsoft.Extensions.Logging

module Helper =

    let waitFor predicate (timeoutMs: int) =
        Assert.True(SpinWait.SpinUntil(Func<bool>(predicate), timeoutMs))

    let assertOk result =
        match result with
        | Ok() -> ()
        | Error error -> failwith $"Expected Ok() but got {error}."

    let assertConnectError expected result =
        match expected, result with
        | ConnectError.Timeout expectedTimeout, Error(ConnectError.Timeout actualTimeout) ->
            Assert.Equal(expectedTimeout, actualTimeout)
        | ConnectError.Cancelled, Error ConnectError.Cancelled -> ()
        | ConnectError.NativeOperationFailed expectedMessage, Error(ConnectError.NativeOperationFailed actualMessage) ->
            Assert.Equal(expectedMessage, actualMessage)
        | _, Ok() -> failwith $"Expected Error({expected}) but got Ok()."
        | _, Error actual -> failwith $"Expected Error({expected}) but got Error({actual})."

type EncodingTests() =

    [<Fact>]
    member _.``outbound encoding defaults to GBK``() =
        let defaults = CtpEncodingOptions.Default

        Assert.Equal(Encoding.GetEncoding("GBK").WebName, defaults.OutboundEncoding.WebName)

    [<Fact>]
    member _.``inbound encoding defaults to GB18030``() =
        let defaults = CtpEncodingOptions.Default

        Assert.Equal(Encoding.GetEncoding("GB18030").WebName, defaults.InboundEncoding.WebName)

    [<Fact>]
    member _.``user login request keeps broker and user ids``() =
        let login = RequestUserLogin.Create("9999", "demo", "secret")
        Assert.Equal("9999", login.BrokerId)
        Assert.Equal("demo", login.UserId)

    [<Fact>]
    member _.``user logout request keeps broker and user ids``() =
        let logout = RequestUserLogout.Create("9999", "demo")
        Assert.Equal("9999", logout.BrokerId)
        Assert.Equal("demo", logout.UserId)

type OptionHelperTests() =

    [<Fact>]
    member _.``options create login request with broker user and password``() =
        let options = CtpOptions.Create("tcp://front", "9999", "demo", "secret")
        let login = OptionHelpers.createUserLoginRequest options

        Assert.Equal("9999", login.BrokerId)
        Assert.Equal("demo", login.UserId)
        Assert.Equal("secret", login.Password)
        Assert.Equal("", login.UserProductInfo)

    [<Fact>]
    member _.``options login request keeps user product info``() =
        let options = CtpOptions.Create("tcp://front", "9999", "demo", "secret", userProductInfo = "strategy")
        let login = OptionHelpers.createUserLoginRequest options

        Assert.Equal("strategy", login.UserProductInfo)

    [<Fact>]
    member _.``options create logout request with broker and user ids``() =
        let options = CtpOptions.Create("tcp://front", "9999", "demo", "secret")
        let logout = OptionHelpers.createUserLogoutRequest options

        Assert.Equal("9999", logout.BrokerId)
        Assert.Equal("demo", logout.UserId)

    [<Fact>]
    member _.``options create authenticate request with user product app id and auth code``() =
        let options =
            CtpOptions.Create(
                "tcp://front",
                "9999",
                "demo",
                "secret",
                userProductInfo = "strategy",
                appId = "app",
                authCode = "auth"
            )

        let request = OptionHelpers.createAuthenticateRequest options

        Assert.Equal("9999", request.BrokerId)
        Assert.Equal("demo", request.UserId)
        Assert.Equal("strategy", request.UserProductInfo.Value)
        Assert.Equal("app", request.AppId)
        Assert.Equal("auth", request.AuthCode)

    [<Fact>]
    member _.``options create authenticate request with empty defaults``() =
        let options = CtpOptions.Create("tcp://front", "9999", "demo", "secret")
        let request = OptionHelpers.createAuthenticateRequest options

        Assert.True(request.UserProductInfo.IsNone)
        Assert.Equal("", request.AppId)
        Assert.Equal("", request.AuthCode)

    [<Fact>]
    member _.``options create settlement confirm request with broker and investor ids``() =
        let options = CtpOptions.Create("tcp://front", "9999", "demo", "secret")
        let request = OptionHelpers.createSettlementInfoConfirmRequest options

        Assert.Equal("9999", request.BrokerId)
        Assert.Equal("demo", request.InvestorId)
        Assert.Equal("", request.ConfirmDate)
        Assert.Equal("", request.ConfirmTime)
        Assert.Equal(0, request.SettlementId)
        Assert.Equal("", request.AccountId)
        Assert.Equal("", request.CurrencyId)


type NumericHelperTests() =

    [<Fact>]
    member _.``float in decimal range converts successfully``() =
        let actual = NumericHelpers.tryDecimal 123.45

        Assert.Equal(Some 123.45m, actual)

    [<Fact>]
    member _.``float above decimal max returns none``() =
        let actual = NumericHelpers.tryDecimal Double.MaxValue

        Assert.Equal(None, actual)

    [<Fact>]
    member _.``nan returns none``() =
        let actual = NumericHelpers.tryDecimal Double.NaN

        Assert.Equal(None, actual)

    [<Fact>]
    member _.``infinity returns none``() =
        let actual = NumericHelpers.tryDecimal Double.PositiveInfinity

        Assert.Equal(None, actual)

    [<Fact>]
    member _.``price above decimal max returns invalid sentinel``() =
        let actual = NumericHelpers.priceOrInvalid Double.MaxValue

        Assert.Equal(-1m, actual)

    [<Fact>]
    member _.``nan price returns invalid sentinel``() =
        let actual = NumericHelpers.priceOrInvalid Double.NaN

        Assert.Equal(-1m, actual)


type SinglePendingResultTests() =

    [<Fact>]
    member _.``pending result completes when set``() =
        let pending = SinglePendingResult<int>()
        let completion = pending.Begin()

        pending.TrySetResult 42

        Assert.True(completion.Task.Wait(1000))
        Assert.Equal(42, completion.Task.Result)

    [<Fact>]
    member _.``only one pending operation is allowed``() =
        let pending = SinglePendingResult<int>()
        pending.Begin() |> ignore

        Assert.Throws<InvalidOperationException>(fun () -> pending.Begin() |> ignore)
        |> ignore

    [<Fact>]
    member _.``taken pending is cleared``() =
        let pending = SinglePendingResult<int>()
        let completion = pending.Begin()

        let taken = pending.TryTake()

        Assert.True(taken.IsSome)
        Assert.Same(completion, taken.Value)
        Assert.True((pending.TryTake()).IsNone)

    [<Fact>]
    member _.``completed pending allows next operation``() =
        let pending = SinglePendingResult<int>()
        let first = pending.Begin()

        pending.TrySetResult 1

        Assert.True(first.Task.Wait(1000))
        Assert.Equal(1, first.Task.Result)

        let second = pending.Begin()

        pending.TrySetResult 2

        Assert.True(second.Task.Wait(1000))
        Assert.Equal(2, second.Task.Result)


type SinglePendingRequestTests() =

    [<Fact>]
    member _.``pending request completes with mapped request``() =
        let pending = SinglePendingRequest<string list, Result<string list, RspInfo>>()
        let completion = pending.Begin([ "au2506"; "ag2506" ])

        pending.TrySetResultFromRequest Ok

        Assert.True(completion.Task.Wait(1000))

        match completion.Task.Result with
        | Ok value -> Assert.Equal<string list>([ "au2506"; "ag2506" ], value)
        | Error error -> failwith $"Expected Ok but got Error({error})."

    [<Fact>]
    member _.``pending request can complete with explicit error``() =
        let pending = SinglePendingRequest<string list, Result<string list, RspInfo>>()
        let completion = pending.Begin([ "au2506" ])
        let error = ClientHelpers.apiReturnError 7

        pending.TrySetResult(Error error)

        Assert.True(completion.Task.Wait(1000))

        match completion.Task.Result with
        | Error actual -> Assert.Equal(error.ErrorId, actual.ErrorId)
        | Ok value -> failwith $"Expected error but got Ok({value})."

    [<Fact>]
    member _.``only one pending request is allowed``() =
        let pending = SinglePendingRequest<string list, Result<string list, RspInfo>>()
        pending.Begin([ "au2506" ]) |> ignore

        Assert.Throws<InvalidOperationException>(fun () -> pending.Begin([ "ag2506" ]) |> ignore)
        |> ignore


type TraderBridgeSurfaceTests() =

    [<Fact>]
    member _.``trader callbacks include private seq no slot``() =
        Assert.True(TraderCallbacks.Empty.RtnPrivateSeqNo.IsNone)


type PendingQueryDictTests() =

    [<Fact>]
    member _.``stream until last accumulates all responses``() =
        let pending = PendingQueryDict()
        let completion = ClientHelpers.createCompletionSource<Result<int list, RspInfo>>()

        pending.Register(1, "QueryNumbers", completion)
        pending.TryHandleResponse(1, Some(box 1), None, false, PendingResponseCompletionPolicy.StreamUntilLast)

        Assert.False(completion.Task.IsCompleted)

        pending.TryHandleResponse(1, Some(box 2), None, true, PendingResponseCompletionPolicy.StreamUntilLast)

        Assert.True(completion.Task.Wait(1000))

        match completion.Task.Result with
        | Ok values -> Assert.Equal<int list>([ 1; 2 ], values)
        | Error error -> failwith $"Expected Ok but got Error({error})."

    [<Fact>]
    member _.``final only ignores non final responses``() =
        let pending = PendingQueryDict()
        let completion = ClientHelpers.createCompletionSource<Result<int list, RspInfo>>()

        pending.Register(1, "Login", completion)
        pending.TryHandleResponse(1, Some(box 1), None, false, PendingResponseCompletionPolicy.FinalOnly)

        Assert.False(completion.Task.IsCompleted)

        pending.TryHandleResponse(1, Some(box 2), None, true, PendingResponseCompletionPolicy.FinalOnly)

        Assert.True(completion.Task.Wait(1000))

        match completion.Task.Result with
        | Ok values -> Assert.Equal<int list>([ 2 ], values)
        | Error error -> failwith $"Expected Ok but got Error({error})."

    [<Fact>]
    member _.``final only returns error from final response``() =
        let pending = PendingQueryDict()
        let completion = ClientHelpers.createCompletionSource<Result<int list, RspInfo>>()
        let error = ClientHelpers.apiReturnError 7

        pending.Register(1, "Authenticate", completion)
        pending.TryHandleResponse(1, Some(box 1), Some error, true, PendingResponseCompletionPolicy.FinalOnly)

        Assert.True(completion.Task.Wait(1000))

        match completion.Task.Result with
        | Error actual -> Assert.Equal(error.ErrorId, actual.ErrorId)
        | Ok value -> failwith $"Expected Error but got Ok({value})."

    [<Fact>]
    member _.``rsp error failure clears pending request``() =
        let pending = PendingQueryDict()
        let completion = ClientHelpers.createCompletionSource<Result<int list, RspInfo>>()
        let error = ClientHelpers.apiReturnError 9

        pending.Register(1, "QueryNumbers", completion)
        pending.TryHandleResponse(1, Some(box 1), None, false, PendingResponseCompletionPolicy.StreamUntilLast)
        pending.TryFail(1, error)
        pending.TryHandleResponse(1, Some(box 2), None, true, PendingResponseCompletionPolicy.StreamUntilLast)

        Assert.True(completion.Task.Wait(1000))

        match completion.Task.Result with
        | Error actual -> Assert.Equal(error.ErrorId, actual.ErrorId)
        | Ok value -> failwith $"Expected Error but got Ok({value})."


type ConnectionCoordinatorTests() =

    [<Fact>]
    member _.``connect waits for first front connected``() =
        let starts = ref 0
        let coordinator = ConnectionCoordinator(fun () -> Interlocked.Increment(starts) |> ignore)
        let task = Async.StartAsTask(coordinator.Connect())

        Helper.waitFor (fun () -> !starts = 1) 1000
        Assert.False(task.IsCompleted)

        coordinator.HandleFrontConnected()
        task.GetAwaiter().GetResult() |> Helper.assertOk

    [<Fact>]
    member _.``front disconnected before first connect does not fail``() =
        let starts = ref 0
        let coordinator = ConnectionCoordinator(fun () -> Interlocked.Increment(starts) |> ignore)
        let task = Async.StartAsTask(coordinator.Connect())

        Helper.waitFor (fun () -> !starts = 1) 1000
        coordinator.HandleFrontDisconnected()
        Assert.False(task.IsCompleted)

        coordinator.HandleFrontConnected()
        task.GetAwaiter().GetResult() |> Helper.assertOk

    [<Fact>]
    member _.``timeout does not restart connection flow``() =
        let starts = ref 0
        let coordinator = ConnectionCoordinator(fun () -> Interlocked.Increment(starts) |> ignore)
        let timeout = TimeSpan.FromMilliseconds 50.0

        coordinator.Connect(timeout = timeout)
        |> Async.RunSynchronously
        |> Helper.assertConnectError (ConnectError.Timeout timeout)

        Assert.Equal(1, !starts)

        let secondTask = Async.StartAsTask(coordinator.Connect())
        Thread.Sleep 20
        Assert.Equal(1, !starts)

        coordinator.HandleFrontConnected()
        secondTask.GetAwaiter().GetResult() |> Helper.assertOk

    [<Fact>]
    member _.``cancellation returns cancelled``() =
        let starts = ref 0
        let coordinator = ConnectionCoordinator(fun () -> Interlocked.Increment(starts) |> ignore)
        use cts = new CancellationTokenSource()

        cts.CancelAfter 50

        let result = coordinator.ConnectTask(cancellationToken = cts.Token).GetAwaiter().GetResult()

        Assert.Equal(1, !starts)
        result |> Helper.assertConnectError ConnectError.Cancelled

    [<Fact>]
    member _.``concurrent connect shares one start``() =
        let starts = ref 0
        let coordinator = ConnectionCoordinator(fun () -> Interlocked.Increment(starts) |> ignore)
        let firstTask = Async.StartAsTask(coordinator.Connect())
        let secondTask = Async.StartAsTask(coordinator.Connect())

        Helper.waitFor (fun () -> !starts = 1) 1000
        coordinator.HandleFrontConnected()

        firstTask.GetAwaiter().GetResult() |> Helper.assertOk
        secondTask.GetAwaiter().GetResult() |> Helper.assertOk
        Assert.Equal(1, !starts)

    [<Fact>]
    member _.``reconnect does not reinit``() =
        let starts = ref 0
        let coordinator = ConnectionCoordinator(fun () -> Interlocked.Increment(starts) |> ignore)
        let firstTask = Async.StartAsTask(coordinator.Connect())

        Helper.waitFor (fun () -> !starts = 1) 1000
        coordinator.HandleFrontConnected()
        firstTask.GetAwaiter().GetResult() |> Helper.assertOk

        coordinator.Connect() |> Async.RunSynchronously |> Helper.assertOk
        Assert.Equal(1, !starts)

        coordinator.HandleFrontDisconnected()

        let secondTask = Async.StartAsTask(coordinator.Connect())
        Thread.Sleep 20
        Assert.Equal(1, !starts)

        coordinator.HandleFrontConnected()
        secondTask.GetAwaiter().GetResult() |> Helper.assertOk

    [<Fact>]
    member _.``native start failure is mapped``() =
        let coordinator = ConnectionCoordinator(fun () -> invalidOp "boom")

        coordinator.Connect()
        |> Async.RunSynchronously
        |> Helper.assertConnectError (ConnectError.NativeOperationFailed "boom")


module FakeLogger =

    type LogEntry = { Level: LogLevel; Message: string }

    type FakeLogger() =
        let entries = ResizeArray<LogEntry>()

        member _.Entries = entries :> IReadOnlyList<LogEntry>

        interface ILogger with
            member _.BeginScope _ = null
            member _.IsEnabled _ = true

            member _.Log(logLevel, _eventId, state, exn, formatter: Func<_, _, _>) =
                let message = formatter.Invoke(state, exn) |> string
                entries.Add({ Level = logLevel; Message = message })

    type FakeLoggerProvider() =
        let logger = FakeLogger()

        member _.Logger = logger

        interface ILoggerProvider with
            member _.CreateLogger _ = logger :> ILogger
            member _.Dispose() = ()


type LoggingTests() =

    let factoryWithProvider (provider: FakeLogger.FakeLoggerProvider) =
        { new ILoggerFactory with
            member _.CreateLogger(category) =
                provider :> ILoggerProvider |> fun p -> p.CreateLogger(category)

            member _.AddProvider _ = ()
            member _.Dispose() = () }

    [<Fact>]
    member _.``native failure logs error``() =
        use provider = new FakeLogger.FakeLoggerProvider()
        use factory = factoryWithProvider provider
        let logger = factory.CreateLogger("Ctp.Net.ConnectionCoordinator")
        let coordinator = ConnectionCoordinator((fun () -> invalidOp "boom"), logger = logger)

        coordinator.Connect()
        |> Async.RunSynchronously
        |> Helper.assertConnectError (ConnectError.NativeOperationFailed "boom")

        let errors =
            provider.Logger.Entries
            |> Seq.filter (fun e -> e.Level = LogLevel.Error)
            |> List.ofSeq

        Assert.Single(errors) |> ignore
        Assert.Contains("Native connection initiation failed", errors[0].Message)
        Assert.Contains("boom", errors[0].Message)

    [<Fact>]
    member _.``connect timeout logs warning``() =
        use provider = new FakeLogger.FakeLoggerProvider()
        use factory = factoryWithProvider provider
        let logger = factory.CreateLogger("Ctp.Net.ConnectionCoordinator")
        let coordinator = ConnectionCoordinator((fun () -> ()), logger = logger)
        let timeout = TimeSpan.FromMilliseconds 50.0

        coordinator.Connect(timeout = timeout)
        |> Async.RunSynchronously
        |> Helper.assertConnectError (ConnectError.Timeout timeout)

        let warnings =
            provider.Logger.Entries
            |> Seq.filter (fun e -> e.Level = LogLevel.Warning)
            |> List.ofSeq

        Assert.Single(warnings) |> ignore
        Assert.Contains("timed out", warnings[0].Message)

    [<Fact>]
    member _.``front connected logs debug``() =
        use provider = new FakeLogger.FakeLoggerProvider()
        use factory = factoryWithProvider provider
        let logger = factory.CreateLogger("Ctp.Net.ConnectionCoordinator")
        let coordinator = ConnectionCoordinator((fun () -> ()), logger = logger)

        let task = Async.StartAsTask(coordinator.Connect())
        Helper.waitFor (fun () -> task.IsCompleted = false) 1000
        coordinator.HandleFrontConnected()
        task.GetAwaiter().GetResult() |> Helper.assertOk

        let debugs =
            provider.Logger.Entries
            |> Seq.filter (fun e -> e.Level = LogLevel.Debug)
            |> List.ofSeq

        Assert.Contains(debugs, fun e -> e.Message.Contains("Front connected"))

    [<Fact>]
    member _.``front disconnected logs info``() =
        use provider = new FakeLogger.FakeLoggerProvider()
        use factory = factoryWithProvider provider
        let logger = factory.CreateLogger("Ctp.Net.ConnectionCoordinator")
        let coordinator = ConnectionCoordinator((fun () -> ()), logger = logger)

        let task = Async.StartAsTask(coordinator.Connect())
        coordinator.HandleFrontConnected()
        task.GetAwaiter().GetResult() |> Helper.assertOk
        coordinator.HandleFrontDisconnected()

        let infos =
            provider.Logger.Entries
            |> Seq.filter (fun e -> e.Level = LogLevel.Information)
            |> List.ofSeq

        Assert.Contains(infos, fun e -> e.Message.Contains("Front disconnected"))

    [<Fact>]
    member _.``no logger means no logging``() =
        let coordinator = ConnectionCoordinator(fun () -> ())
        let task = Async.StartAsTask(coordinator.Connect())

        coordinator.HandleFrontConnected()
        task.GetAwaiter().GetResult() |> Helper.assertOk
// No exception thrown = pass
