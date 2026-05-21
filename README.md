[English](README.md) | [中文](README.zh-CN.md)

[![Build](https://github.com/masq6r/Ctp.Net.Next/actions/workflows/build.yml/badge.svg?branch=master)](https://github.com/masq6r/Ctp.Net.Next/actions/workflows/build.yml)
[![Tests](https://img.shields.io/endpoint?url=https://raw.githubusercontent.com/masq6r/Ctp.Net.Next/master/badges/test-results.json)](https://github.com/masq6r/Ctp.Net.Next/actions/workflows/test.yml)

# Ctp.Net

A `.NET` wrapper for the CTP SDK.

This repository keeps the vendor C++ SDK behind a thin native bridge so the managed layer can expose `Async<Result<...>>` APIs, .NET events, and managed domain types on Linux and Windows.

As a core infrastructure component of the personal automated trading system `eXp`, `Ctp.Net.Next` has powered its 24/7 uninterrupted stable operation for 5 years without downtime, achieving a 42% CAGR. This component is open-sourced with the hope of helping individuals who aspire to engage in automated trading — may you find your own holy grail.

## Disclaimer

**This software is provided "as is", without warranty of any kind.** Use it at your own risk. The authors are not responsible for any financial losses, trading errors, or damages arising from its use. This project is not affiliated with the CTP SDK vendor. Nothing in this repository constitutes financial or investment advice.

### CTP SDK native library redistribution notice

For packaging convenience, the NuGet package for this project may include native binary files originating from the CTP SDK. Those files remain the property of their respective rights holders.

No statement in this repository should be interpreted as granting any separate license, authorization, endorsement, or legal opinion regarding redistribution of the upstream CTP SDK. Before using this project in production, internal distribution, commercial distribution, or any onward redistribution scenario, you are responsible for independently reviewing the upstream SDK terms, applicable laws and regulations, and your own compliance obligations, and for obtaining any permissions that may be required.

If your intended use, distribution model, or jurisdiction imposes additional restrictions, do not rely solely on the bundled binaries. The maintainers reserve the right to change, limit, or remove bundled native files in future releases if legal, compliance, or upstream policy considerations require it.

## Highlights

- OOTB experience, just download and trade, no extra setups
- Query with **async workflows** (C# `async`/`await`, F# `async` computation expressions) to avoid callback hell
- `CtpFlowControlOptions` for query **throttling, native retry handling, and subscription batching**
- Cancel in-flight queries at any time with `CancellationToken`
- Stable disconnection/reconnection with subscription recovery
- Use **managed types** such as `DateOnly`, `DateTime` etc. instead of strings
- Asymmetric encoding policy aligned with common CTP deployments: outbound `GBK`, inbound `GB18030`
- Keeping up with the latest CTP SDK releases

## Related demos and consumers

- `Demos/Subscription.fsx` — F# script `MdClient` example covering login and market-data subscription
- `Demos/Subscription.cs` — C# file-based app `MdClient` example covering the same login and market-data subscription flow against the local project
- `Demos/FlowControl` — `TraderClient` example covering authentication, settlement confirmation, and concurrent queries under managed flow control
- `Demos/CtpDemo.Local.Native` — native C++ Trader example for inspecting request/callback behavior against the official API
- `eXp` — a personal automated trading system that has been running on `Ctp.Net.Next` for 5 years

## Repository layout

- `Ctp.Net` — managed F# library; `Bridge/` contains low-level interop and the top-level files expose public clients
- `NativeBridge` — C++ bridge, exported C ABI, bundled `ctp-sdk`, and native-only build entrypoints
- `Demos/Subscription.fsx` — F# script `MdClient` demo for login and market-data subscription
- `Demos/Subscription.cs` — C# file-based app `MdClient` demo for the same market-data flow
- `Demos/FlowControl` — `TraderClient` demo for authentication, settlement confirmation, and concurrent queries under managed flow control
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

### `Demos/Subscription.fsx`

This F# script demonstrates the `MdClient` login and market-data subscription flow.

Before running it:

1. Edit `Demos/Subscription.fsx` and update `ctpOpt` to match your environment:
   - `frontAddress`
   - `brokerId`
   - `userId`
   - `password`
   - `flowPath`
   - `productionMode`
   - `userProductInfo`
   - `appId`
   - `authCode`
2. Update `instrumentIds` in the same file to the contracts you want to subscribe to.
3. Create the directory referenced by `flowPath` before `Init()` runs.

Example:

```bash
mkdir -p /tmp/ctp-flow-md
```

Run:

```bash
dotnet fsi Demos/Subscription.fsx
```

Notes:

- The script is self-contained and does not use `options.local.json`.
- The script currently references the published `Ctp.Net.Next` NuGet package via `#r "nuget: Ctp.Net.Next"`.

### `Demos/Subscription.cs`

This C# file-based app demonstrates the same `MdClient` login and market-data subscription flow, but references the local `Ctp.Net` project from this repository.

Before running it:

1. Edit `Demos/Subscription.cs` and update `ctpOptions` to match your environment:
   - `frontAddress`
   - `brokerId`
   - `userId`
   - `password`
   - `flowPath`
   - `productionMode`
   - `userProductInfo`
   - `appId`
   - `authCode`
2. Update `instrumentIds` in the same file to the contracts you want to subscribe to.
3. Create the directory referenced by `flowPath` before `Init()` runs.

Build:

```bash
dotnet build -- Demos/Subscription.cs
```

Run:

```bash
dotnet run --file Demos/Subscription.cs
```

Notes:

- The file-based app is self-contained and does not use `options.local.json`.
- `dotnet build -- Demos/Subscription.cs` uses `--` so the file is treated as a file-based app instead of an MSBuild project path.

### `Demos/FlowControl`

Build:

```bash
dotnet build Demos/FlowControl/FlowControl.fsproj -m:1
```

Run:

```bash
dotnet run --project Demos/FlowControl/FlowControl.fsproj
```

This demo reads `Demos/FlowControl/options.local.json`, authenticates and logs in with `TraderClient`, confirms settlement when needed, then issues concurrent queries to demonstrate managed flow-control behavior.

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
