open Ctp.Net
open System
open System.IO
open FSharpPlus
open Ctp.Net.Bridge
open System.Text.Json
open System.Threading.Tasks

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
type QueriesConfigFile =
    { CtpOptions: CtpOptionsFile
      CtpFlowControlOptions: CtpFlowControlOptionsFile
      ConnectTimeoutMs: Nullable<int> }

type QueriesConfig =
    { CtpOptions: CtpOptions
      CtpFlowControlOptions: CtpFlowControlOptions
      ConnectTimeout: TimeSpan }

type QueryOutcome = { Name: string; Summary: string; Succeeded: bool }

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

    let load (args: string[]) =
        let path = if args.Length > 0 then args[0] else defaultConfigFileName
        let json = File.ReadAllText path
        match JsonSerializer.Deserialize<QueriesConfigFile>(json, jsonOptions) |> Option.ofObj with
        | Some config ->
            { CtpOptions = createCtpOptions config.CtpOptions
              CtpFlowControlOptions = createFlowControlOptions config.CtpFlowControlOptions
              ConnectTimeout = float config.ConnectTimeoutMs.Value |> TimeSpan.FromMilliseconds }
        | None -> failwith $"Failed to load config from {path}."

module Demo =
    let private timestamp () = DateTimeOffset.Now.ToString("HH:mm:ss.fff")

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

    let private summarizeSettlementConfirm (confirmations: SettlementInfoConfirmResponse list) =
        match confirmations with
        | confirmation :: _ ->
            $"Settlement confirmed. ConfirmDate={confirmation.ConfirmDate}; ConfirmTime={confirmation.ConfirmTime}"
        | [] -> "Settlement confirmed."

    let private startInstrumentQuery (client: TraderClient) = task {
        printfn "[%s] QueryInstrumentAsync started." (timestamp ())

        let! result = client.QueryInstrumentAsync("", "", "", "") |> Async.StartAsTask

        return
            match result with
            | Ok instruments ->
                let instrumentIds =
                    let str = instruments |>> _.InstrumentId |> String.concat ","

                    if str.Length > 500 then
                        $"{String.take 500 str}..."
                    else
                        str

                { Name = "QueryInstrumentAsync"
                  Summary =
                    $"QueryInstrumentAsync completed. Count={List.length instruments}; InstrumentIds={instrumentIds}"
                  Succeeded = true }
            | Error error ->
                { Name = "QueryInstrumentAsync"
                  Summary = $"QueryInstrumentAsync failed. ErrorId={error.ErrorId}; ErrorMessage={error.ErrorMessage}"
                  Succeeded = false }
    }

    let private startTradingAccountQuery (client: TraderClient) = task {
        printfn "[%s] QueryTradingAccountAsync started." (timestamp ())

        let! result = client.QueryTradingAccountAsync() |> Async.StartAsTask

        return
            match result with
            | Ok [] ->
                { Name = "QueryTradingAccountAsync"
                  Summary = "QueryTradingAccountAsync completed. No trading account data returned."
                  Succeeded = true }
            | Ok accounts ->
                let accountSummary =
                    accounts
                    |> List.map (fun account -> $"BrokerId={account.BrokerId}, Balance={account.Balance}")
                    |> String.concat "; "

                { Name = "QueryTradingAccountAsync"
                  Summary = $"QueryTradingAccountAsync completed. {accountSummary}"
                  Succeeded = true }
            | Error error ->
                { Name = "QueryTradingAccountAsync"
                  Summary =
                    $"QueryTradingAccountAsync failed. ErrorId={error.ErrorId}; ErrorMessage={error.ErrorMessage}"
                  Succeeded = false }
    }

    let run (args: string[]) = task {
        let config = Config.load args
        use client = new TraderClient(config.CtpOptions, flowControl = config.CtpFlowControlOptions)

        printfn "Connecting to %s" config.CtpOptions.FrontAddress
        let! connectResult = client.Connect(timeout = config.ConnectTimeout) |> Async.StartAsTask
        connectResult |> expectConnected

        printfn "Authenticating..."
        let! authenticateResult = client.AuthenticateAsync() |> Async.StartAsTask
        authenticateResult |> expectOk "AuthenticateAsync" |> ignore

        printfn "Logging in..."
        let! loginResult = client.LoginAsync() |> Async.StartAsTask
        loginResult |> expectOk "LoginAsync" |> ignore

        printfn "Confirming settlement info..."
        let! settlementConfirmResult = client.SettlementInfoConfirmAsync() |> Async.StartAsTask

        settlementConfirmResult
        |> expectOk "SettlementInfoConfirmAsync"
        |> summarizeSettlementConfirm
        |> printfn "%s"

        let instrumentTask = startInstrumentQuery client
        let tradingAccountTask = startTradingAccountQuery client
        let pending = ResizeArray<Task<QueryOutcome>>([ instrumentTask; tradingAccountTask ])
        let outcomes = ResizeArray<QueryOutcome>()

        while pending.Count > 0 do
            let! completed = Task.WhenAny pending
            pending.Remove completed |> ignore

            let! outcome = completed
            outcomes.Add outcome
            printfn "[%s] %s" (timestamp ()) outcome.Summary

        outcomes
        |> Seq.tryFind (fun outcome -> not outcome.Succeeded)
        |> Option.iter (fun outcome -> failwith $"{outcome.Name} did not complete successfully.")
    }

[<EntryPoint>]
let main args =
    try
        (Demo.run args).GetAwaiter().GetResult()
        0
    with ex ->
        eprintfn "%s" ex.Message
        1
