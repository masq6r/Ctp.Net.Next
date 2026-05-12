namespace Ctp.Net.SmokeTests

open Xunit
open System
open Ctp.Net
open System.IO
open Ctp.Net.Bridge
open System.Text.Json
open System.Threading.Tasks

[<CLIMutable>]
type SmokeTestConfigFile =
    { BrokerId: string
      UserId: string
      Password: string
      MdFront: string
      TraderFront: string
      InstrumentId: string
      AppId: string
      AuthCode: string
      UserProductInfo: string
      FlowPath: string
      ProductionMode: Nullable<bool>
      ConnectTimeoutMs: Nullable<int>
      MarketDataTimeoutMs: Nullable<int> }

type AuthenticationConfig = { AppId: string; AuthCode: string }

type SharedSmokeConfig =
    { BrokerId: string
      UserId: string
      Password: string
      UserProductInfo: string
      FlowPath: string option
      ProductionMode: bool
      ConnectTimeout: TimeSpan }

type MdSmokeConfig =
    { Shared: SharedSmokeConfig
      FrontAddress: string
      InstrumentId: string
      MarketDataTimeout: TimeSpan }

type TraderSmokeConfig =
    { Shared: SharedSmokeConfig
      FrontAddress: string
      Authentication: AuthenticationConfig option }

module Helper =
    let expectOk result =
        match result with
        | Ok value -> value
        | Error error -> failwith $"Expected Ok(_) but got {error}."

    let createOptions (shared: SharedSmokeConfig) frontAddress authentication =
        let appId, authCode =
            match authentication with
            | Some value -> value.AppId, value.AuthCode
            | None -> "", ""

        CtpOptions.Create(
            frontAddress,
            shared.BrokerId,
            shared.UserId,
            shared.Password,
            ?flowPath = shared.FlowPath,
            productionMode = shared.ProductionMode,
            userProductInfo = shared.UserProductInfo,
            appId = appId,
            authCode = authCode
        )

module SmokeConfig =
    let private configFileName = "smoke.local.json"
    let private projectFileName = "Ctp.Net.SmokeTests.fsproj"

    let private jsonOptions = JsonSerializerOptions(PropertyNameCaseInsensitive = true)

    let private isPresent (value: string) = not (String.IsNullOrWhiteSpace value)

    let private trimToOption (value: string) = if isPresent value then Some(value.Trim()) else None

    let private trimRequired (value: string) = value.Trim()

    let private allAncestorDirectories startPath =
        let rec loop (current: DirectoryInfo option) = seq {
            match current with
            | Some directory ->
                yield directory.FullName
                yield! loop (Option.ofObj directory.Parent)
            | None -> ()
        }

        loop (Some(DirectoryInfo startPath))

    let private searchStartDirectories () =
        [ Directory.GetCurrentDirectory(); AppContext.BaseDirectory ] |> Seq.distinct

    let private tryFindFile relativePaths =
        searchStartDirectories ()
        |> Seq.collect allAncestorDirectories
        |> Seq.distinct
        |> Seq.tryPick (fun directory ->
            relativePaths
            |> Seq.tryPick (fun relativePath ->
                let path = Path.Combine(directory, relativePath)
                if File.Exists path then Some path else None))

    let private tryGetConfigPath () =
        [ configFileName; Path.Combine("Tests", "Ctp.Net.SmokeTests", configFileName) ]
        |> tryFindFile

    let private tryGetProjectDirectory () =
        [ projectFileName
          Path.Combine("Tests", "Ctp.Net.SmokeTests", projectFileName) ]
        |> tryFindFile
        |> Option.bind (Path.GetDirectoryName >> Option.ofObj)

    let private tryLoadConfigFile () =
        try
            tryGetConfigPath ()
            |> Option.bind (fun path ->
                let json = File.ReadAllText path

                JsonSerializer.Deserialize<SmokeTestConfigFile>(json, jsonOptions)
                |> Option.ofObj)
        with _ ->
            None

    let private createSharedConfig (config: SmokeTestConfigFile) =
        if
            isPresent config.BrokerId
            && isPresent config.UserId
            && isPresent config.Password
        then
            Some
                { BrokerId = trimRequired config.BrokerId
                  UserId = trimRequired config.UserId
                  Password = trimRequired config.Password
                  UserProductInfo = trimToOption config.UserProductInfo |> Option.defaultValue ""
                  FlowPath = trimToOption config.FlowPath
                  ProductionMode =
                    if config.ProductionMode.HasValue then
                        config.ProductionMode.Value
                    else
                        true
                  ConnectTimeout =
                    if config.ConnectTimeoutMs.HasValue then
                        config.ConnectTimeoutMs.Value
                    else
                        15000
                    |> float
                    |> TimeSpan.FromMilliseconds }
        else
            None

    let tryLoadMd () : MdSmokeConfig option =
        match tryLoadConfigFile () with
        | Some config ->
            match createSharedConfig config with
            | Some shared when isPresent config.MdFront && isPresent config.InstrumentId ->
                Some
                    { Shared = shared
                      FrontAddress = trimRequired config.MdFront
                      InstrumentId = trimRequired config.InstrumentId
                      MarketDataTimeout =
                        if config.MarketDataTimeoutMs.HasValue then
                            config.MarketDataTimeoutMs.Value
                        else
                            20000
                        |> float
                        |> TimeSpan.FromMilliseconds }
            | _ -> None
        | None -> None

    let tryLoadTrader () : TraderSmokeConfig option =
        match tryLoadConfigFile () with
        | Some config ->
            match createSharedConfig config with
            | Some shared when isPresent config.TraderFront ->
                let authentication =
                    match trimToOption config.AppId, trimToOption config.AuthCode with
                    | None, None -> Some None
                    | Some appId, Some authCode -> Some(Some { AppId = appId; AuthCode = authCode })
                    | _ -> None

                authentication
                |> Option.map (fun auth ->
                    { Shared = shared
                      FrontAddress = trimRequired config.TraderFront
                      Authentication = auth })
            | _ -> None
        | None -> None

    let private configLocationMessage () =
        match tryGetProjectDirectory () with
        | Some directory -> Path.Combine(directory, configFileName)
        | None -> Path.Combine("Tests", "Ctp.Net.SmokeTests", configFileName)

    let loadMd () =
        match tryLoadMd () with
        | Some config -> config
        | None -> failwith $"Create a valid {configLocationMessage ()} to enable this smoke test."

    let loadTrader () =
        match tryLoadTrader () with
        | Some config -> config
        | None -> failwith $"Create a valid {configLocationMessage ()} to enable this smoke test."

type SmokeTestAvailability =
    static member IsMdConfigured = SmokeConfig.tryLoadMd () |> Option.isSome

    static member IsTraderConfigured = SmokeConfig.tryLoadTrader () |> Option.isSome

type MdSmokeTests() =
    [<Fact(Skip = "Create Tests/Ctp.Net.SmokeTests/smoke.local.json to enable this smoke test.",
           SkipUnless = "IsMdConfigured",
           SkipType = typeof<SmokeTestAvailability>)>]
    member _.``md client can connect login subscribe and receive market data``() = task {
        let config = SmokeConfig.loadMd ()
        let options = Helper.createOptions config.Shared config.FrontAddress None
        use client = new MdClient(options)

        let dataReceived =
            TaskCompletionSource<DepthMarketData> TaskCreationOptions.RunContinuationsAsynchronously

        client.DepthMarketDataReceived.Add(fun data ->
            if String.Equals(data.InstrumentId, config.InstrumentId, StringComparison.OrdinalIgnoreCase) then
                dataReceived.TrySetResult data |> ignore)

        let! connectResult = client.Connect(timeout = config.Shared.ConnectTimeout) |> Async.StartAsTask
        connectResult |> Helper.expectOk |> ignore

        let! loginResult = client.LoginAsync() |> Async.StartAsTask
        loginResult |> Helper.expectOk |> ignore

        let! subscribed = client.SubscribeMarketDataAsync [ config.InstrumentId ] |> Async.StartAsTask
        subscribed |> Helper.expectOk |> ignore

        let! data = dataReceived.Task.WaitAsync config.MarketDataTimeout
        Assert.Equal(config.InstrumentId, data.InstrumentId)

        let! unsubscribed = client.UnsubscribeMarketDataAsync [ config.InstrumentId ] |> Async.StartAsTask
        unsubscribed |> Helper.expectOk |> ignore
    }

type TraderSmokeTests() =
    [<Fact(Skip = "Create Tests/Ctp.Net.SmokeTests/smoke.local.json to enable this smoke test.",
           SkipUnless = "IsTraderConfigured",
           SkipType = typeof<SmokeTestAvailability>)>]
    member _.``trader client can connect optionally authenticate login and query trading account``() = task {
        let config = SmokeConfig.loadTrader ()
        let options = Helper.createOptions config.Shared config.FrontAddress config.Authentication
        use client = new TraderClient(options)

        let! connectResult = client.Connect(timeout = config.Shared.ConnectTimeout) |> Async.StartAsTask
        connectResult |> Helper.expectOk |> ignore

        let! authenticateResult = client.AuthenticateAsync() |> Async.StartAsTask
        authenticateResult |> Helper.expectOk |> ignore

        let! loginResult = client.LoginAsync() |> Async.StartAsTask
        loginResult |> Helper.expectOk |> ignore

        let! confirmed = client.SettlementInfoConfirmAsync() |> Async.StartAsTask
        confirmed |> Helper.expectOk |> ignore

    // let queryRequest: QueryTradingAccountRequest =
    //     { BrokerId = config.Shared.BrokerId
    //       InvestorId = config.Shared.UserId
    //       CurrencyId = None
    //       BizType = None
    //       AccountId = None }

    // let! accountResult = client.QueryTradingAccountAsync(queryRequest) |> Async.StartAsTask
    // accountResult |> Helper.expectOk |> ignore
    }
