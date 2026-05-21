#r "nuget: Ctp.Net.Next"

open System
open Ctp.Net
open Ctp.Net.Bridge

let ctpOpt =
    CtpOptions.Create(
        "tcp://182.254.243.31:30001",
        "9999",
        "user-id@simnow",
        "password@simnow",
        flowPath = "/tmp/ctp-flow-md",
        productionMode = true,
        userProductInfo = "eXp",
        appId = "simnow_client_test",
        authCode = "0000000000000000"
    )

let connectTimeout = TimeSpan.FromSeconds 15.0

let summarizeSettlementConfirm =
    function
    | confirmation: SettlementInfoConfirmResponse :: _ ->
        printfn $"Settlement confirmed. ConfirmDate={confirmation.ConfirmDate}; ConfirmTime={confirmation.ConfirmTime}"
    | [] -> printfn "Settlement confirmed."

let summarizeAccount (acco: TradingAccountResponse list) =
    match acco with
    | acco :: _ -> printfn $"Trading account fetched. BrokerId={acco.BrokerId}, Balance={acco.Balance}"
    | [] -> printfn "Trading account fetched. No trading account data returned."

using (new TraderClient(ctpOpt))
<| fun t ->
    async {
        printfn "Connecting to %s" ctpOpt.FrontAddress
        let! _ = t.Connect(timeout = connectTimeout)

        printfn "Authenticating..."
        let! _ = t.AuthenticateAsync()

        printfn "Logging in..."
        let! _ = t.LoginAsync()

        printfn "Confirming settlement info..."

        match! t.SettlementInfoConfirmAsync() with
        | Ok s -> summarizeSettlementConfirm s
        | _ -> failwith "Failed to confirm settlement info."

        match! t.QueryTradingAccountAsync() with
        | Ok acco -> summarizeAccount acco
        | _ -> failwith "Failed to query trading account."
    }
    |> Async.RunSynchronously
