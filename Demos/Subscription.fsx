#r "nuget: Ctp.Net.Next"

open System
open Ctp.Net
open Ctp.Net.Bridge

let ctpOpt =
    CtpOptions.Create(
        "tcp://182.254.243.31:30011",
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
let instrumentIds = [ "au2612"; "m2609" ]

using (new MdClient(ctpOpt))
<| fun md ->
    md.DepthMarketDataReceived.Add
    <| fun depth -> printfn "%s@%s: %M" depth.InstrumentId (depth.UpdateTime.ToString "HH:mm:ss.fff") depth.LastPrice

    async {
        printfn "Connecting to %s" ctpOpt.FrontAddress
        let! _ = md.Connect(timeout = connectTimeout)
        printfn "Logging in..."
        let! _ = md.LoginAsync()
        printfn "Subscribing to %s" (String.concat "," instrumentIds)
        let! _ = md.SubscribeMarketDataAsync instrumentIds

        printfn "Receiving depth market data. Press any key to exit."
        Console.ReadKey true |> ignore
    }
    |> Async.RunSynchronously
