# CLAUDE.md

This file gives Claude Code the repo-specific facts that are worth loading every session. Keep it short, operational, and biased toward mistake prevention. Put general walkthroughs in `README.md`, and native-only deep detail in `NativeBridge/README.md`.

## Fast path commands

- Prefer project-file commands; do not assume a solution file exists.
- Main managed build: `dotnet build Ctp.Net/Ctp.Net.fsproj -m:1`
- Build unit tests: `dotnet build Tests/Ctp.Net.Tests/Ctp.Net.Tests.fsproj -m:1`
- Run unit tests: `dotnet run --project Tests/Ctp.Net.Tests/Ctp.Net.Tests.fsproj --no-restore`
- List tests: `dotnet run --project Tests/Ctp.Net.Tests/Ctp.Net.Tests.fsproj --no-build -- -list tests`
- Run one unit test: `dotnet run --project Tests/Ctp.Net.Tests/Ctp.Net.Tests.fsproj --no-build -- -method "Ctp.Net.Tests.EncodingTests.outbound encoding defaults to GBK"`
- Run smoke tests: `dotnet run --project Tests/Ctp.Net.SmokeTests/Ctp.Net.SmokeTests.fsproj`
- Run one smoke test: `dotnet run --project Tests/Ctp.Net.SmokeTests/Ctp.Net.SmokeTests.fsproj --no-build -- -method "Ctp.Net.SmokeTests.MdSmokeTests.md client can connect login subscribe and receive market data"`
- Build native bridge only on Linux: `./NativeBridge/build.sh`
- Build native bridge only on Windows: `./NativeBridge/build.ps1 -CtpSdkRoot <path-to-ctp-sdk>`
- Build market-data demo: `dotnet build Demos/Subscrption/Subscrption.fsproj -m:1`
- Run market-data demo: `dotnet run --project Demos/Subscrption/Subscrption.fsproj`
- Build trader query demo: `dotnet build Demos/Queries/Queries.fsproj -m:1`
- Run trader query demo: `dotnet run --project Demos/Queries/Queries.fsproj`

## Command caveats

- Use `-m:1` for `dotnet build`; multi-node builds can fail in MSBuild's `CombineTargetFrameworkInfoProperties` task.
- Building `Ctp.Net` also builds `NativeBridge` and copies native artifacts into the managed output directory.
- Test projects use pure Microsoft.Testing.Platform via `Tests/*/global.json`. Prefer `dotnet run --project ... -- <xUnit args>` over VSTest compatibility mode.
- Smoke tests are live integration tests. They stay skipped until `Tests/Ctp.Net.SmokeTests/smoke.local.json` exists and contains valid fronts and credentials.
- Demo projects read local config from `Demos/*/options.local.json`.
- `flowPath` directories must already exist before `Init()`.
- There is no repo-specific lint or formatting command checked in here. Do not invent one in automation.

## Mental model

- `NativeBridge` is the native C++ layer. It links the official CTP SDK and exposes a stable C ABI in `NativeBridge/include/ctp_bridge.h`.
- `Ctp.Net/Bridge/` is the interop layer: `DllImport`, native structs, callback delegates, safe handles, and marshalling.
- `Ctp.Net/Md.fs` and `Ctp.Net/Trader.fs` wrap callback-driven APIs into `Async<Result<...>>` methods and .NET events.
- `Ctp.Net/Common.fs` contains shared public options, `CtpFlowControlOptions`, connection coordination, and managed flow-control primitives.
- `Tests/Ctp.Net.Tests/Program.fs` is the fast behavioral suite.
- `Tests/Ctp.Net.SmokeTests/Program.fs` is the end-to-end suite against real fronts.
- `Demos/Subscrption` and `Demos/Queries` are the fastest examples of current managed usage.

## Hard invariants

- Before changing a CTP field, callback contract, or request/response semantic, consult `NativeBridge/ctp-sdk/<version>/reference` first.
- The market-data and trader stacks are intentionally parallel. Most field/API changes require coordinated edits in:
  1. `NativeBridge/include/ctp_bridge.h`
  2. `NativeBridge/src/`
  3. `Ctp.Net/Bridge/`
  4. `Ctp.Net/`
  5. tests
- Managed runtime loading does not use the process working directory. Native lookup order is:
  1. `CTP_BRIDGE_DIR`
  2. `AppContext.BaseDirectory`
  3. `AppContext.BaseDirectory/native`
- Encoding is asymmetric by design: outbound requests default to `GBK`; inbound responses and events default to `GB18030`.
- Managed domain types normalize price/currency-like values to `decimal` even though the native SDK uses floating-point values.
- `ConnectionCoordinator` ensures native `Init()` starts once, shares the connect wait across concurrent callers, and resets waiting only after a front disconnect.
- `TraderClient.Connect()` must subscribe private/public topics before `Init()`.
- `productionMode` defaults to `true` across the library, smoke tests, and demos. Test or simulation flows must opt out explicitly.
- `LogoutAsync` in both clients intentionally completes after a successful native logout request instead of waiting for `OnRspUserLogout`.
- Native callbacks must stay lightweight; expensive work should be handed off away from the SDK callback thread.
- Do not share one `flowPath` across multiple API instances.

## Where to edit first

- Shared interop and native loading: `Ctp.Net/Bridge/Common.fs`
- MD interop surface: `Ctp.Net/Bridge/MdBridge.fs`
- Trader interop surface: `Ctp.Net/Bridge/TraderBridge.fs`
- Shared managed options and flow control: `Ctp.Net/Common.fs`
- Managed MD client flow: `Ctp.Net/Md.fs`
- Managed trader client flow: `Ctp.Net/Trader.fs`

## Keep out of this file

- Long tutorials
- Repeated README content
- Deep native build explanation better kept in `NativeBridge/README.md`
- Temporary task notes or branch-specific status
