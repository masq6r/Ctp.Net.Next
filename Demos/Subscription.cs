#:sdk Microsoft.NET.Sdk
#:property TargetFramework=net10.0
#:project ../Ctp.Net/Ctp.Net.fsproj

using Ctp.Net;

var ctpOptions = CtpOptions.Create(
    frontAddress: "tcp://182.254.243.31:30011",
    brokerId: "9999",
    userId: "user-id@simnow",
    password: "password@simnow",
    flowPath: "/tmp/ctp-flow-md",
    productionMode: true,
    userProductInfo: "eXp",
    appId: "simnow_client_test",
    authCode: "0000000000000000"
);

var connectTimeout = TimeSpan.FromSeconds(15);
var instrumentIds = new[] { "au2612", "m2609" };

using var md = new Ctp.Net.CSharp.MdClient(ctpOptions);

md.DepthMarketDataReceived += (_, depth) =>
    Console.WriteLine($"{depth.InstrumentId}@{depth.UpdateTime:HH:mm:ss.fff}: {depth.LastPrice}");

Console.WriteLine($"Connecting to {ctpOptions.FrontAddress}");
await md.ConnectAsync(timeout: connectTimeout);

Console.WriteLine("Logging in...");
await md.LoginAsync();

Console.WriteLine($"Subscribing to {string.Join(",", instrumentIds)}");
await md.SubscribeMarketDataAsync(instrumentIds);

Console.WriteLine("Receiving depth market data. Press any key to exit.");
Console.ReadKey(intercept: true);
