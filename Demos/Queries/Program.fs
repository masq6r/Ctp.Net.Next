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
    let private configFileName = "options.local.json"
    let private projectFileName = "Queries.fsproj"
    let private jsonOptions = JsonSerializerOptions(PropertyNameCaseInsensitive = true)

    let private isPresent (value: string) = not (String.IsNullOrWhiteSpace value)

    let private trimRequired fieldName (value: string) =
        if isPresent value then
            value.Trim()
        else
            invalidOp $"{configFileName} is missing required field CtpOptions.{fieldName}."

    let private trimToOption (value: string) = if isPresent value then Some(value.Trim()) else None

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
        [ configFileName; Path.Combine("Demos", "Queries", configFileName) ]
        |> tryFindFile

    let private tryGetProjectDirectory () =
        [ projectFileName; Path.Combine("Demos", "Queries", projectFileName) ]
        |> tryFindFile
        |> Option.bind (Path.GetDirectoryName >> Option.ofObj)

    let private configLocationMessage () =
        match tryGetProjectDirectory () with
        | Some directory -> Path.Combine(directory, configFileName)
        | None -> Path.Combine("Demos", "Queries", configFileName)

    let private tryLoadConfigFile () =
        try
            tryGetConfigPath ()
            |> Option.bind (fun path ->
                let json = File.ReadAllText path

                JsonSerializer.Deserialize<QueriesConfigFile>(json, jsonOptions) |> Option.ofObj)
        with _ ->
            None

    let private requireSection sectionName value =
        if obj.ReferenceEquals(value, null) then
            invalidOp $"{configFileName} must contain a {sectionName} section."

        value

    let private createCtpOptions (config: CtpOptionsFile) =
        let flowPath = trimToOption config.FlowPath

        let productionMode =
            if config.ProductionMode.HasValue then
                config.ProductionMode.Value
            else
                true

        let userProductInfo = trimToOption config.UserProductInfo |> Option.defaultValue ""
        let appId = trimToOption config.AppId |> Option.defaultValue ""
        let authCode = trimToOption config.AuthCode |> Option.defaultValue ""

        CtpOptions.Create(
            trimRequired "FrontAddress" config.FrontAddress,
            trimRequired "BrokerId" config.BrokerId,
            trimRequired "UserId" config.UserId,
            trimRequired "Password" config.Password,
            ?flowPath = flowPath,
            productionMode = productionMode,
            userProductInfo = userProductInfo,
            appId = appId,
            authCode = authCode
        )

    let private createFlowControlOptions (config: CtpFlowControlOptionsFile) =
        let defaults = CtpFlowControlOptions.Default

        let valueOrDefault (value: Nullable<int>) defaultValue = if value.HasValue then value.Value else defaultValue

        { MaxDispatchesPerSecond = valueOrDefault config.MaxDispatchesPerSecond defaults.MaxDispatchesPerSecond
          MaxQueriesPerSecond = valueOrDefault config.MaxQueriesPerSecond defaults.MaxQueriesPerSecond
          MaxNativeReturnCodeRetries =
            valueOrDefault config.MaxNativeReturnCodeRetries defaults.MaxNativeReturnCodeRetries
          NativeReturnCodeRetryDelay =
            valueOrDefault
                config.NativeReturnCodeRetryDelayMs
                (int defaults.NativeReturnCodeRetryDelay.TotalMilliseconds)
            |> float
            |> TimeSpan.FromMilliseconds
          MaxQueryRspErrorRetries = valueOrDefault config.MaxQueryRspErrorRetries defaults.MaxQueryRspErrorRetries
          QueryRspErrorRetryDelay =
            valueOrDefault config.QueryRspErrorRetryDelayMs (int defaults.QueryRspErrorRetryDelay.TotalMilliseconds)
            |> float
            |> TimeSpan.FromMilliseconds
          QueryCompletionTimeout =
            valueOrDefault config.QueryCompletionTimeoutMs (int defaults.QueryCompletionTimeout.TotalMilliseconds)
            |> float
            |> TimeSpan.FromMilliseconds
          SubscriptionBatchSize = valueOrDefault config.SubscriptionBatchSize defaults.SubscriptionBatchSize
          SubscriptionBatchDelay =
            valueOrDefault config.SubscriptionBatchDelayMs (int defaults.SubscriptionBatchDelay.TotalMilliseconds)
            |> float
            |> TimeSpan.FromMilliseconds }

    let load () =
        match tryLoadConfigFile () with
        | Some config ->
            { CtpOptions = config.CtpOptions |> requireSection "CtpOptions" |> createCtpOptions
              CtpFlowControlOptions =
                config.CtpFlowControlOptions
                |> requireSection "CtpFlowControlOptions"
                |> createFlowControlOptions
              ConnectTimeout =
                if config.ConnectTimeoutMs.HasValue then
                    config.ConnectTimeoutMs.Value
                else
                    15000
                |> float
                |> TimeSpan.FromMilliseconds }
        | None ->
            failwith
                $"Create a valid {configLocationMessage ()} with CtpOptions and CtpFlowControlOptions before running this demo."

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

    let run () = task {
        let config = Config.load ()
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
let main _ =
    try
        (Demo.run ()).GetAwaiter().GetResult()
        0
    with ex ->
        eprintfn "%s" ex.Message
        1
