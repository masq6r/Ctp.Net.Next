open System
open Ctp.Net
open System.IO
open Ctp.Net.Bridge
open System.Text.Json

[<CLIMutable>]
type CtpOptionsFile =
    { FrontAddress: string
      FlowPath: string
      ProductionMode: Nullable<bool>
      BrokerId: string
      UserId: string
      Password: string
      UserProductInfo: string
      AppId: string
      AuthCode: string }

[<CLIMutable>]
type CtpFlowControlOptionsFile =
    { MaxDispatchesPerSecond: Nullable<int>
      MaxQueriesPerSecond: Nullable<int>
      MaxNativeReturnCodeRetries: Nullable<int>
      NativeReturnCodeRetryDelayMs: Nullable<int>
      MaxQueryRspErrorRetries: Nullable<int>
      QueryRspErrorRetryDelayMs: Nullable<int>
      QueryCompletionTimeoutMs: Nullable<int>
      SubscriptionBatchSize: Nullable<int>
      SubscriptionBatchDelayMs: Nullable<int> }

[<CLIMutable>]
type SubscrptionConfigFile =
    { CtpOptions: CtpOptionsFile
      CtpFlowControlOptions: CtpFlowControlOptionsFile
      InstrumentIds: string array
      ConnectTimeoutMs: Nullable<int> }

type SubscrptionConfig =
    { CtpOptions: CtpOptions
      CtpFlowControlOptions: CtpFlowControlOptions
      InstrumentIds: string list
      ConnectTimeout: TimeSpan }

module Config =
    let private defaultConfigFileName = "options.local.json"
    let private jsonOptions = JsonSerializerOptions(PropertyNameCaseInsensitive = true)

    let private trimToOption (value: string) = if String.IsNullOrWhiteSpace value then None else Some(value.Trim())

    let private createCtpOptions (config: CtpOptionsFile) =
        let flowPath = trimToOption config.FlowPath

        CtpOptions.Create(
            config.FrontAddress.Trim(),
            config.BrokerId.Trim(),
            config.UserId.Trim(),
            config.Password.Trim(),
            ?flowPath = flowPath,
            productionMode = config.ProductionMode.Value,
            userProductInfo = config.UserProductInfo.Trim(),
            appId = config.AppId.Trim(),
            authCode = config.AuthCode.Trim()
        )

    let private createFlowControlOptions (config: CtpFlowControlOptionsFile) =
        { MaxDispatchesPerSecond = config.MaxDispatchesPerSecond.Value
          MaxQueriesPerSecond = config.MaxQueriesPerSecond.Value
          MaxNativeReturnCodeRetries = config.MaxNativeReturnCodeRetries.Value
          NativeReturnCodeRetryDelay = float config.NativeReturnCodeRetryDelayMs.Value |> TimeSpan.FromMilliseconds
          MaxQueryRspErrorRetries = config.MaxQueryRspErrorRetries.Value
          QueryRspErrorRetryDelay = float config.QueryRspErrorRetryDelayMs.Value |> TimeSpan.FromMilliseconds
          QueryCompletionTimeout = float config.QueryCompletionTimeoutMs.Value |> TimeSpan.FromMilliseconds
          SubscriptionBatchSize = config.SubscriptionBatchSize.Value
          SubscriptionBatchDelay = float config.SubscriptionBatchDelayMs.Value |> TimeSpan.FromMilliseconds }

    let private createInstrumentIds (instrumentIds: string array) = instrumentIds |> Array.map _.Trim() |> Array.toList

    let load (args: string[]) =
        let path = if args.Length > 0 then args[0] else defaultConfigFileName
        let json = File.ReadAllText path
        match JsonSerializer.Deserialize<SubscrptionConfigFile>(json, jsonOptions) |> Option.ofObj with
        | Some config ->
            { CtpOptions = createCtpOptions config.CtpOptions
              CtpFlowControlOptions = createFlowControlOptions config.CtpFlowControlOptions
              InstrumentIds = createInstrumentIds config.InstrumentIds
              ConnectTimeout = float config.ConnectTimeoutMs.Value |> TimeSpan.FromMilliseconds }
        | None -> failwith $"Failed to load config from {path}."

module Demo =
    let private expectConnected =
        function
        | Ok() -> ()
        | Error(ConnectError.Timeout timeout) ->
            failwith $"Connect failed: timeout after {timeout.TotalSeconds} seconds."
        | Error ConnectError.Cancelled -> failwith "Connect failed: cancelled."
        | Error(ConnectError.NativeOperationFailed message) -> failwith $"Connect failed: {message}"

    let private expectOk operationName (result: Result<'T, RspInfo>) =
        match result with
        | Ok value -> value
        | Error error -> failwith $"{operationName} failed: [{error.ErrorId}] {error.ErrorMessage}"

    let private printDepthMarketData (data: DepthMarketData) =
        printfn "%s@%s: %M" data.InstrumentId (data.UpdateTime.ToString("HH:mm:ss.fff")) data.LastPrice

    let run (args: string[]) = task {
        let config = Config.load args
        use client = new MdClient(config.CtpOptions, flowControl = config.CtpFlowControlOptions)

        client.DepthMarketDataReceived.Add printDepthMarketData

        printfn "Connecting to %s" config.CtpOptions.FrontAddress
        let! connectResult = client.Connect(timeout = config.ConnectTimeout) |> Async.StartAsTask
        connectResult |> expectConnected

        printfn "Logging in..."
        let! loginResult = client.LoginAsync() |> Async.StartAsTask
        loginResult |> expectOk "LoginAsync" |> ignore

        printfn "Subscribing to %s" (String.concat "," config.InstrumentIds)
        let! subscribedResult = client.SubscribeMarketDataAsync config.InstrumentIds |> Async.StartAsTask
        subscribedResult |> expectOk "SubscribeMarketDataAsync" |> ignore

        printfn "Receiving depth market data. Press any key to exit."
        Console.ReadKey true |> ignore
    }

[<EntryPoint>]
let main args =
    try
        (Demo.run args).GetAwaiter().GetResult()
        0
    with ex ->
        eprintfn "%s" ex.Message
        1
