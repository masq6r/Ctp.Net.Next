#:sdk Microsoft.NET.Sdk
#:property TargetFramework=net10.0
#:project ../Ctp.Net/Ctp.Net.fsproj

using System.Diagnostics;
using Ctp.Net;
using Ctp.Net.Bridge;
using Ctp.Net.CSharp;

static string Now() => DateTime.Now.ToString("HH:mm:ss.fff");

var ctpOpt = CtpOptions.Create(
    frontAddress: "tcp://182.254.243.31:30001",
    brokerId: "9999",
    userId: "user@simnow",
    password: "passowrd@simnow",
    flowPath: "/tmp/ctp-flow-query-cancel-cs",
    productionMode: true,
    userProductInfo: "eXp",
    appId: "simnow_client_test",
    authCode: "0000000000000000"
);

var connectTimeout = TimeSpan.FromSeconds(15);

using var trader = new Ctp.Net.CSharp.TraderClient(ctpOpt);

Console.WriteLine($"[{Now()}] Connecting to {ctpOpt.FrontAddress}");
await trader.ConnectAsync(timeout: connectTimeout);

Console.WriteLine($"[{Now()}] Authenticating...");
await trader.AuthenticateAsync();

Console.WriteLine($"[{Now()}] Logging in...");
var login = await trader.LoginAsync();
Console.WriteLine($"[{Now()}] Logged in. TradingDay={login.TradingDay}");

Console.WriteLine($"[{Now()}] Confirming settlement info...");
var confirmation = await trader.SettlementInfoConfirmAsync();
Console.WriteLine($"[{Now()}] Settlement confirmed. ConfirmDate={confirmation.ConfirmDate}");

Console.WriteLine("\n=================================================================");
Console.WriteLine("  Query Cancellation Flow Control Demo");
Console.WriteLine("=================================================================\n");
Console.WriteLine("  CTP only allows ONE in-flight query at a time.");
Console.WriteLine("  Once a query request reaches the CTP server, it cannot be");
Console.WriteLine("  cancelled on the server side. Cancelling the client-side async");
Console.WriteLine("  does NOT free the in-flight slot — the native SDK still waits");
Console.WriteLine("  for the server response (or timeout).\n");
Console.WriteLine("  This demo:");
Console.WriteLine("    1. Fires QueryInstrumentAsync (broad query)");
Console.WriteLine("    2. Cancels it after dispatch");
Console.WriteLine("    3. Immediately fires QueryTradingAccountAsync");
Console.WriteLine("    4. Shows QueryTradingAccount BLOCKED until the first query");
Console.WriteLine("       completes at the native level");
Console.WriteLine("=================================================================\n");

// ---- Step 1: Start a broad query in background ----

using var queryCts = new CancellationTokenSource();
var q1Sw = Stopwatch.StartNew();

async Task<IReadOnlyList<InstrumentResponse>?> RunQueryInstrument()
{
    try
    {
        Console.WriteLine(
            $"[{Now()}] [Q1] QueryInstrumentAsync START — acquiring in-flight slot..."
        );
        var result = await trader.QueryInstrumentAsync("", "", "", "", queryCts.Token);
        q1Sw.Stop();
        Console.WriteLine(
            $"[{Now()}] [Q1] QueryInstrumentAsync COMPLETED (elapsed {q1Sw.ElapsedMilliseconds}ms)"
        );
        return result;
    }
    catch (OperationCanceledException)
    {
        q1Sw.Stop();
        Console.WriteLine(
            $"[{Now()}] [Q1] QueryInstrumentAsync client-side CANCELLED (elapsed {q1Sw.ElapsedMilliseconds}ms)"
        );
        return null;
    }
    catch (CtpException ex)
    {
        q1Sw.Stop();
        Console.WriteLine($"[{Now()}] [Q1] QueryInstrumentAsync FAILED: {ex.Message}");
        return null;
    }
}

var q1Task = RunQueryInstrument();

// Give the query enough time to pass rate gates and reach the native API call
Console.WriteLine($"[{Now()}] --- Waiting 500ms for Q1 to dispatch to CTP ---");
await Task.Delay(500);

// ---- Step 2: Cancel the first query ----

Console.WriteLine();
Console.WriteLine($"[{Now()}] >>> Cancelling Q1 (client-side async cancellation)");
Console.WriteLine(
    $"[{Now()}] >>> The native CTP query is still in flight — server cannot cancel it"
);
queryCts.Cancel();
await Task.Delay(100);

// ---- Step 3: Immediately fire the second query ----

Console.WriteLine();
Console.WriteLine($"[{Now()}] >>> Starting Q2: QueryTradingAccountAsync");
Console.WriteLine($"[{Now()}] >>> Q2 will BLOCK at acquiring the in-flight slot...");
Console.WriteLine($"[{Now()}] >>> ...until Q1's native operation finishes (response or timeout)");

var q2Sw = Stopwatch.StartNew();
var accounts = await trader.QueryTradingAccountAsync();
q2Sw.Stop();

Console.WriteLine();
Console.WriteLine(
    $"[{Now()}] <<< Q2: QueryTradingAccountAsync COMPLETED (elapsed {q2Sw.ElapsedMilliseconds}ms)"
);

var account = accounts.FirstOrDefault();
if (account is not null)
    Console.WriteLine($"[{Now()}]     Balance={account.Balance}, BrokerId={account.BrokerId}");
else
    Console.WriteLine($"[{Now()}]     No trading account data returned.");

// ---- Step 4: Report final state of the first query ----

Console.WriteLine();

try
{
    var q1Result = await q1Task;
    if (q1Result is not null)
        Console.WriteLine($"[{Now()}] [Q1] Final result: {q1Result.Count} instruments returned");
    else
        Console.WriteLine($"[{Now()}] [Q1] Final result: cancelled (returned null)");
}
catch (Exception ex)
{
    Console.WriteLine($"[{Now()}] [Q1] Final result: {ex.Message}");
}

// ---- Summary ----

Console.WriteLine();
Console.WriteLine("=================================================================");
Console.WriteLine("  Key observations:");
Console.WriteLine("  - Q1 was cancelled at t~+50ms, but its native operation lived on");
Console.WriteLine("  - Q2 started immediately after cancellation");
Console.WriteLine(
    $"  - Q2 was blocked for ~{q2Sw.ElapsedMilliseconds}ms waiting for Q1's in-flight slot"
);
Console.WriteLine("  - This confirms: CTP server-side queries cannot be cancelled;");
Console.WriteLine("    a new query must wait for the old one to finish");
Console.WriteLine("=================================================================");
