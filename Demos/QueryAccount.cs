#:sdk Microsoft.NET.Sdk
#:property TargetFramework=net10.0
#:project ../Ctp.Net/Ctp.Net.fsproj

using Ctp.Net;

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

using var trader = new Ctp.Net.CSharp.TraderClient(ctpOptions);

Console.WriteLine($"Connecting to {ctpOptions.FrontAddress}");
await trader.ConnectAsync(timeout: connectTimeout);

Console.WriteLine("Authenticating...");
var auth = await trader.AuthenticateAsync();
Console.WriteLine($"Authenticated. AppType={auth.AppType}");

Console.WriteLine("Logging in...");
var login = await trader.LoginAsync();
Console.WriteLine($"Logged in. TradingDay={login.TradingDay}");

Console.WriteLine("Confirming settlement info...");
var confirmation = await trader.SettlementInfoConfirmAsync();
Console.WriteLine($"Settlement confirmed. ConfirmDate={confirmation.ConfirmDate}");

Console.WriteLine("Querying trading account...");
var accounts = await trader.QueryTradingAccountAsync();
var account = accounts.FirstOrDefault();
if (account is not null)
    Console.WriteLine(
        $"Trading account fetched. BrokerId={account.BrokerId}, Balance={account.Balance}"
    );
