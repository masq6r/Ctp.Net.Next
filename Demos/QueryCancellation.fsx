#r "nuget: Ctp.Net.Next"

open System
open Ctp.Net
open Ctp.Net.Bridge
open System.Threading
open System.Diagnostics
open System.Threading.Tasks

let now () = DateTime.Now.ToString "HH:mm:ss.fff"

// Use a shorter query timeout so the demo finishes in a reasonable time
let flowControl =
    { CtpFlowControlOptions.Default with
        QueryCompletionTimeout = TimeSpan.FromSeconds 15.0 }

let ctpOpt =
    CtpOptions.Create(
        "tcp://182.254.243.31:40001",
        "9999",
        "106839",
        "I#mVkm&jw*9J&e",
        // "user@simnow",
        // "passowrd@simnow",
        flowPath = "/tmp/ctp-flow-query-cancel",
        productionMode = true,
        userProductInfo = "eXp",
        appId = "simnow_client_test",
        authCode = "0000000000000000"
    )

let connectTimeout = TimeSpan.FromSeconds 15.0

using (new TraderClient(ctpOpt, flowControl = flowControl))
<| fun t ->
    async {
        printfn $"[{now ()}] Connecting to {ctpOpt.FrontAddress}"
        let! _ = t.Connect(timeout = connectTimeout)

        printfn $"[{now ()}] Authenticating..."
        let! _ = t.AuthenticateAsync()

        printfn $"[{now ()}] Logging in..."
        let! _ = t.LoginAsync()

        printfn $"[{now ()}] Confirming settlement info..."
        let! _ = t.SettlementInfoConfirmAsync()

        printfn "\n================================================================="
        printfn "  Query Cancellation Flow Control Demo"
        printfn "================================================================="
        printfn ""
        printfn "  CTP only allows ONE in-flight query at a time."
        printfn "  Once a query request reaches the CTP server, it cannot be"
        printfn "  cancelled on the server side. Cancelling the client-side async"
        printfn "  does NOT free the in-flight slot — the native SDK still waits"
        printfn "  for the server response (or timeout)."
        printfn ""
        printfn "  This demo:"
        printfn "    1. Fires QueryInstrumentAsync (broad query)"
        printfn "    2. Cancels it after dispatch"
        printfn "    3. Immediately fires QueryTradingAccountAsync"
        printfn "    4. Shows QueryTradingAccount BLOCKED until the first query"
        printfn "       completes at the native level"
        printfn "=================================================================\n"

        // ---- Step 1: Start a broad query in background ----

        use queryCts = new CancellationTokenSource()
        let query1Sw = Stopwatch.StartNew()

        let query1 = async {
            try
                printfn $"[{now ()}] [Q1] QueryInstrumentAsync START — acquiring in-flight slot..."
                let! result = t.QueryInstrumentAsync("", "", "", "")
                query1Sw.Stop()
                printfn $"[{now ()}] [Q1] QueryInstrumentAsync COMPLETED (elapsed %d{query1Sw.ElapsedMilliseconds}ms)"

                return Some result
            with
            | :? OperationCanceledException ->
                query1Sw.Stop()

                printfn
                    $"[{now ()}] [Q1] QueryInstrumentAsync client-side CANCELLED (elapsed %d{query1Sw.ElapsedMilliseconds}ms)"

                return None
            | ex ->
                query1Sw.Stop()
                printfn $"[{now ()}] [Q1] QueryInstrumentAsync FAILED: {ex.Message}"
                return None
        }

        let query1Task = Async.StartAsTask(query1, cancellationToken = queryCts.Token)

        // Give the query enough time to pass rate gates and reach the native API call
        printfn $"[{now ()}] --- Waiting 500ms for Q1 to dispatch to CTP ---"
        do! Async.Sleep 500

        // ---- Step 2: Cancel the first query ----

        printfn ""
        printfn $"[{now ()}] >>> Cancelling Q1 (client-side async cancellation)"
        printfn $"[{now ()}] >>> The native CTP query is still in flight — server cannot cancel it"
        queryCts.Cancel()
        do! Async.Sleep 100

        // ---- Step 3: Immediately fire the second query ----

        printfn ""
        printfn $"[{now ()}] >>> Starting Q2: QueryTradingAccountAsync"
        printfn $"[{now ()}] >>> Q2 will BLOCK at acquiring the in-flight slot..."

        printfn
            $"[{now ()}] >>> ...until Q1's native operation finishes (response or %.1f{flowControl.QueryCompletionTimeout.TotalSeconds}s timeout)"

        let q2Sw = Stopwatch.StartNew()
        let! accountResult = t.QueryTradingAccountAsync()

        q2Sw.Stop()
        printfn ""
        printfn $"[{now ()}] <<< Q2: QueryTradingAccountAsync COMPLETED (elapsed %d{q2Sw.ElapsedMilliseconds}ms)"

        match accountResult with
        | Ok acco ->
            match acco with
            | a :: _ -> printfn "[%s]     Balance=%f, BrokerId=%s" (now ()) a.Balance a.BrokerId
            | [] -> printfn "[%s]     No trading account data returned." (now ())
        | Error e -> printfn "[%s]     Error: [%d] %s" (now ()) e.ErrorId e.ErrorMessage

        // ---- Step 4: Report final state of the first query ----

        printfn ""

        match query1Task.Status with
        | TaskStatus.RanToCompletion ->
            match query1Task.Result with
            | Some(Ok insts) -> printfn $"[{now ()}] [Q1] Final result: {insts.Length} instruments returned"
            | Some(Error e) -> printfn $"[{now ()}] [Q1] Final result: error [{e.ErrorId}] {e.ErrorMessage}"
            | None -> printfn $"[{now ()}] [Q1] Final result: cancelled (returned None)"
        | TaskStatus.Canceled -> printfn $"[{now ()}] [Q1] Task status: Canceled"
        | TaskStatus.Faulted ->
            printfn "[%s] [Q1] Task faulted: %s" (now ()) query1Task.Exception.InnerException.Message
        | _ -> printfn "[%s] [Q1] Task status: %A" (now ()) query1Task.Status

        // ---- Summary ----

        printfn ""
        printfn "================================================================="
        printfn "  Key observations:"
        printfn "  - Q1 was cancelled at t~+1s, but its native operation lived on"
        printfn "  - Q2 started immediately after cancellation"
        printfn "  - Q2 was blocked for ~%dms waiting for Q1's in-flight slot" q2Sw.ElapsedMilliseconds
        printfn "  - This confirms: CTP server-side queries cannot be cancelled;"
        printfn "    a new query must wait for the old one to finish"
        printfn "================================================================="
    }
    |> Async.RunSynchronously
