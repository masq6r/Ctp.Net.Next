# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build and test commands

- Prefer project-file commands; do not assume a solution file exists in the checkout.
- Native bridge only on Linux: `./NativeBridge/build.sh`
- Native bridge only on Windows: `./NativeBridge/build.ps1 -CtpSdkRoot <path-to-ctp-sdk>`
- Main managed build: `dotnet build Ctp.Net/Ctp.Net.fsproj -m:1`
- Build the main test project: `dotnet build Tests/Ctp.Net.Tests/Ctp.Net.Tests.fsproj -m:1`
- Run the main test suite (xUnit v3 on Microsoft.Testing.Platform): `dotnet run --project Tests/Ctp.Net.Tests/Ctp.Net.Tests.fsproj --no-restore`
- List tests: `dotnet run --project Tests/Ctp.Net.Tests/Ctp.Net.Tests.fsproj --no-build -- -list tests`
- Run one test: `dotnet run --project Tests/Ctp.Net.Tests/Ctp.Net.Tests.fsproj --no-build -- -method "Ctp.Net.Tests.EncodingTests.outbound encoding defaults to GBK"`
- Run the smoke tests: `dotnet run --project Tests/Ctp.Net.SmokeTests/Ctp.Net.SmokeTests.fsproj`
- Run one smoke test: `dotnet run --project Tests/Ctp.Net.SmokeTests/Ctp.Net.SmokeTests.fsproj --no-build -- -method "Ctp.Net.SmokeTests.MdSmokeTests.md client can connect login"`

Notes:

- `Ctp.Net` triggers `NativeBridge/build.sh` or `build.ps1` from its MSBuild targets, so building managed projects also rebuilds and copies the native bridge artifacts.
- Use `-m:1` for `dotnet build`; the repo README notes multi-node builds can fail in MSBuild's `CombineTargetFrameworkInfoProperties` task.
- Test projects are configured for pure Microsoft.Testing.Platform via `Tests/*/global.json`. Prefer `dotnet run --project ... -- <xUnit runner args>` over VSTest compatibility mode.
- There is no repo-specific lint or formatting command checked in here. Do not invent one in automation.
- Smoke tests are live integration tests. They become active when `Tests/Ctp.Net.SmokeTests/smoke.local.json` exists, and they can fail with real network timeouts if local credentials or fronts are unavailable.

## Architecture

This repo is a two-layer wrapper around the official CTP SDK:

1. `NativeBridge` is the native C++ layer. It links the vendor SDK and exposes a stable C ABI in `include/ctp_bridge.h`, implemented by `src/md_bridge.cpp` and `src/trader_bridge.cpp`.
2. `Ctp.Net` is the managed F# layer. `Ctp.Net/Bridge/` owns the low-level interop (`DllImport` declarations, native struct layouts, callback delegate registration, `SafeHandle` lifetimes, and marshalling), while the top-level files wrap callback-driven native APIs into `Async<Result<...>>` methods and .NET events for market data and trader workflows.

## Bundled CTP SDK layout

- `NativeBridge/ctp-sdk/<version_date>/mduserapi` contains the market-data SDK headers and native libraries.
- `NativeBridge/ctp-sdk/<version_date>/traderapi` contains the trader SDK headers and native libraries.
- `NativeBridge/ctp-sdk/<version_date>/reference` contains the official CTP API reference documentation bundled with that SDK version.
- The native build selects a version directory under `ctp-sdk`, so bridge changes must stay aligned with the structs, callback contracts, and request semantics of the selected SDK version.

## Key code paths

- `Ctp.Net/Bridge/Common.fs` is the shared interop spine: native library resolution, encoding defaults, fixed-width string marshalling, and shared request/response/native struct helpers.
- `Ctp.Net/Bridge/MdBridge.fs` and `Ctp.Net/Bridge/TraderBridge.fs` mirror the C ABI for the market-data and trader APIs separately. They map native callbacks into F# records and expose thin `MdApi` / `TraderApi` wrappers.
- `Ctp.Net/Common.fs` contains shared public options plus the connection/request coordination primitives (`SinglePendingResult` and `ConnectionCoordinator`) used by both clients.
- `Ctp.Net/Md.fs` and `Ctp.Net/Trader.fs` wrap the low-level APIs with `MailboxProcessor` agents. Those agents serialize native callbacks, complete pending async requests, and raise user-facing events.
- `Tests/Ctp.Net.Tests/Program.fs` focuses on fast behavioral coverage for encoding defaults, request helpers, and connection coordination.
- `Tests/Ctp.Net.SmokeTests/Program.fs` is the real end-to-end suite against CTP fronts.

## Invariants that matter when changing code

- The market-data and trader stacks are intentionally parallel across all layers. Adding or changing a CTP field or API usually requires coordinated edits in:
  1. the official CTP API reference under `NativeBridge/ctp-sdk/<version_date>/reference` should be consulted first to confirm field sizes, callback contracts, request/response semantics, and API-specific constraints
  2. `NativeBridge/include/ctp_bridge.h`
  3. the corresponding C++ bridge implementation in `NativeBridge/src/`
  4. the matching F# native structs/imports/mappers in `Ctp.Net/Bridge/`
  5. the public wrapper types and async flow in `Ctp.Net/`
  6. tests
- Managed runtime loading does not use the process working directory. Native lookup order is `CTP_BRIDGE_DIR`, then `AppContext.BaseDirectory`, then `AppContext.BaseDirectory/native`.
- Encoding policy is asymmetric by design: outbound requests default to `GBK`, inbound responses/events default to `GB18030`.
- Managed domain types normalize price/currency-like values to `decimal` even though the native SDK uses floating-point values.
- `ConnectionCoordinator` ensures native init starts once, lets concurrent callers share the same connect wait, and resets waiting only after a front disconnect.
- `TraderClient.Connect()` subscribes private/public topics before `Init()`. Keep that sequencing aligned with any future connection changes.
- `LogoutAsync` in both clients intentionally completes after a successful native logout request instead of waiting for `OnRspUserLogout`, because the current SDK does not reliably invoke that callback.
