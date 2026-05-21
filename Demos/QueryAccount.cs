#:sdk Microsoft.NET.Sdk
#:property TargetFramework=net10.0
#:project ../Ctp.Net/Ctp.Net.fsproj

using Ctp.Net;
using Ctp.Net.Bridge;
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Control;
using Microsoft.FSharp.Core;

var ctpOptions = CtpOptions.Create(
    frontAddress: "tcp://182.254.243.31:30001",
    brokerId: "9999",
    userId: "user-id@simnow",
    password: "password@simnow",
    flowPath: "/tmp/ctp-flow-trader",
    productionMode: true,
    userProductInfo: "eXp",
    appId: "simnow_client_test",
    authCode: "0000000000000000"
);

var connectTimeout = TimeSpan.FromSeconds(15);

using var trader = new TraderClient(ctpOptions, null, null, null, null, null, null);

Console.WriteLine($"Connecting to {ctpOptions.FrontAddress}");
await FSharpAsync.StartAsTask(
    trader.Connect(timeout: connectTimeout, cancellationToken: null),
    null,
    null
);

Console.WriteLine("Authenticating...");
await FSharpAsync.StartAsTask(trader.AuthenticateAsync(cancellationToken: null), null, null);

Console.WriteLine("Logging in...");
await FSharpAsync.StartAsTask(trader.LoginAsync(cancellationToken: null), null, null);

Console.WriteLine("Confirming settlement info...");
var confirmations = EnsureOk(
    await FSharpAsync.StartAsTask(
        trader.SettlementInfoConfirmAsync(cancellationToken: null),
        null,
        null
    ),
    error => $"Settlement confirmation failed: {error.ErrorId} {error.ErrorMessage}"
);
SummarizeSettlementConfirm(confirmations);

Console.WriteLine("Querying trading account...");
var accounts = EnsureOk(
    await FSharpAsync.StartAsTask(
        trader.QueryTradingAccountAsync(null, null, null, null),
        null,
        null
    ),
    error => $"QueryTradingAccount failed: {error.ErrorId} {error.ErrorMessage}"
);
SummarizeAccount(accounts);

static T EnsureOk<T, TError>(FSharpResult<T, TError> result, Func<TError, string> formatError)
{
    if (result.IsOk)
    {
        return result.ResultValue;
    }

    throw new InvalidOperationException(formatError(result.ErrorValue));
}

static void SummarizeSettlementConfirm(FSharpList<SettlementInfoConfirm> confirmations)
{
    var confirmation = confirmations.FirstOrDefault();

    if (confirmation is null)
    {
        Console.WriteLine("Settlement confirmed.");
        return;
    }

    Console.WriteLine(
        $"Settlement confirmed. ConfirmDate={confirmation.ConfirmDate}; ConfirmTime={confirmation.ConfirmTime}"
    );
}

static void SummarizeAccount(FSharpList<TradingAccountResponse> accounts)
{
    var account = accounts.FirstOrDefault();

    if (account is null)
    {
        Console.WriteLine("Trading account fetched. No trading account data returned.");
        return;
    }

    Console.WriteLine(
        $"Trading account fetched. BrokerId={account.BrokerId}, Balance={account.Balance}"
    );
}
