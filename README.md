# Ctp.Net

F#-first `.NET 10` wrapper for the CTP SDK.

This repository keeps the vendor C++ SDK behind a thin native bridge so the managed layer can expose `Async<Result<...>>` APIs, .NET events, and managed domain types on Linux and Windows.

## Highlights

- `MdClient` for market-data connect / login / subscribe workflows
- `TraderClient` for authenticate / login / settlement confirmation, trading, and a broad query surface
- `CtpFlowControlOptions` for query throttling, native retry handling, and subscription batching
- managed `decimal`-based price and money fields instead of raw native floating-point values
- asymmetric encoding policy aligned with common CTP deployments: outbound `GBK`, inbound `GB18030`

## Repository layout

- `Ctp.Net` — managed F# library; `Bridge/` contains low-level interop and the top-level files expose public clients
- `NativeBridge` — C++ bridge, exported C ABI, bundled `ctp-sdk`, and native-only build entrypoints
- `Demos/Subscrption` — `MdClient` demo for login and market-data subscription
- `Demos/Queries` — `TraderClient` demo for authentication, settlement confirmation, and concurrent queries under managed flow control
- `Demos/CtpDemo.Local.Native` — native C++ trader demo for inspecting request / callback behavior against the official API
- `Tests/Ctp.Net.Tests` — fast unit tests
- `Tests/Ctp.Net.SmokeTests` — live integration tests against real CTP fronts

## Build

### Managed library

From the repository root:

```bash
dotnet build Ctp.Net/Ctp.Net.fsproj -m:1
```

Notes:

- This build automatically runs `NativeBridge/build.sh` on Linux or `NativeBridge/build.ps1` on Windows.
- The native bridge artifacts are copied into the managed output directory.
- Use `-m:1`; multi-node builds can fail in MSBuild's `CombineTargetFrameworkInfoProperties` task on this repo.

### Native bridge only

If you are iterating on the C++ bridge directly, build it separately:

#### Linux

```bash
./NativeBridge/build.sh
```

#### Windows

```powershell
.\NativeBridge\build.ps1 -CtpSdkRoot C:\path\to\ctp-sdk
```

See [NativeBridge/README.md](NativeBridge/README.md) for the native-only workflow and SDK layout details.

## Test

### Unit tests

```bash
dotnet run --project Tests/Ctp.Net.Tests/Ctp.Net.Tests.fsproj --no-restore
```

List tests:

```bash
dotnet run --project Tests/Ctp.Net.Tests/Ctp.Net.Tests.fsproj --no-build -- -list tests
```

Run one test:

```bash
dotnet run --project Tests/Ctp.Net.Tests/Ctp.Net.Tests.fsproj --no-build -- -method "Ctp.Net.Tests.EncodingTests.outbound encoding defaults to GBK"
```

### Smoke tests

Smoke tests stay skipped until `Tests/Ctp.Net.SmokeTests/smoke.local.json` exists and contains valid front / credential settings.

Run the smoke test project:

```bash
dotnet run --project Tests/Ctp.Net.SmokeTests/Ctp.Net.SmokeTests.fsproj
```

Run one smoke test:

```bash
dotnet run --project Tests/Ctp.Net.SmokeTests/Ctp.Net.SmokeTests.fsproj --no-build -- -method "Ctp.Net.SmokeTests.MdSmokeTests.md client can connect login subscribe and receive market data"
```

## Runtime notes

- Native bridge lookup order is:
  1. `CTP_BRIDGE_DIR`
  2. `AppContext.BaseDirectory`
  3. `AppContext.BaseDirectory/native`
- The managed layer does not use the process current working directory to resolve native libraries.
- `flowPath` must already exist before `Init()`; the library does not create it for you.
- Do not share the same `flowPath` between multiple API instances.
- `productionMode` defaults to `true`. For test or simulation environments, set it explicitly to `false` when required by the front.
- Trader authentication is optional at the API surface but required by many fronts; use `userProductInfo`, `appId`, and `authCode` when your environment expects them.

## Demos

### `Demos/Subscrption`

Build:

```bash
dotnet build Demos/Subscrption/Subscrption.fsproj -m:1
```

Run:

```bash
dotnet run --project Demos/Subscrption/Subscrption.fsproj
```

This demo reads `Demos/Subscrption/options.local.json`, connects an `MdClient`, logs in, subscribes to configured instruments, and prints lightweight market-data updates.

### `Demos/Queries`

Build:

```bash
dotnet build Demos/Queries/Queries.fsproj -m:1
```

Run:

```bash
dotnet run --project Demos/Queries/Queries.fsproj
```

This demo reads `Demos/Queries/options.local.json`, authenticates and logs in with `TraderClient`, confirms settlement when needed, then issues concurrent queries to demonstrate managed flow-control behavior.

### `Demos/CtpDemo.Local.Native`

This native C++ demo reuses `Tests/Ctp.Net.SmokeTests/smoke.local.json` and is useful when you want to inspect raw Trader API request / callback request-id behavior without going through the managed wrapper.

## Minimal F# usage

```fsharp
open Ctp.Net

let run () =
    let options =
        CtpOptions.Create(
            frontAddress = "tcp://180.168.146.187:10211",
            brokerId = "9999",
            userId = "demo",
            password = "secret",
            flowPath = "/tmp/ctp-flow-md",
            productionMode = false
        )

    use md = new MdClient(options)

    async {
        let! connectResult = md.Connect()

        match connectResult with
        | Error error ->
            return Error error
        | Ok () ->
            let! loginResult = md.LoginAsync()
            return loginResult
    }
    |> Async.RunSynchronously
```

Create the flow directory before running the client.

## Native bridge architecture

The repository is a two-layer wrapper around the official CTP SDK:

1. `NativeBridge` links the vendor SDK and exposes a stable C ABI in `NativeBridge/include/ctp_bridge.h`.
2. `Ctp.Net/Bridge` mirrors that ABI with `DllImport`, native structs, callback registration, safe handles, and marshalling.
3. `Ctp.Net/Md.fs` and `Ctp.Net/Trader.fs` wrap the callback-driven native APIs into public client classes, async workflows, and .NET events.

## Encoding policy

- .NET strings remain Unicode in the managed surface
- outbound requests default to `GBK`
- inbound responses and events default to `GB18030`
- encoding can be overridden per `MdClient` / `TraderClient`

This matches common CTP deployment reality better than forcing UTF-8 at the SDK boundary.
