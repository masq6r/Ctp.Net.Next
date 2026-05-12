# Ctp.Net

F#-first `.NET 10` wrapper for the CTP 6.7.13 SDK.

This repository uses a thin native C++ bridge to translate the vendor C++ ABI into a stable C ABI that can be called from managed code on Linux and Windows.

## Layout

- `Ctp.Bridge.Net`: low-level F# interop layer and native bridge loader
- `Ctp.Net`: higher-level F# client API for `mduserapi` and `traderapi`
- `Ctp.Bridge.C`: C++ bridge, exported C ABI, CMake build, and bundled `ctp-sdk`
- `Tests/Ctp.Net.Tests`: smoke runner that validates encoding defaults and bridge version lookup

## Encoding policy

- .NET strings stay Unicode end-to-end
- outbound requests to CTP default to `GBK`
- inbound responses/events from CTP default to `GB18030`
- encoding can be overridden per `MdClient` / `TraderClient`

This matches the usual CTP deployment reality more closely than forcing UTF-8 at the SDK boundary.

## Build

### Linux

```bash
cd /home/layez/Projects/Ctp.Net
./Ctp.Bridge.C/build.sh
dotnet build Ctp.Net.sln -m:1
```

`build.sh` defaults `CTP_SDK_ROOT` to `./Ctp.Bridge.C/ctp-sdk` and automatically selects the latest version directory under it, such as `v6.7.13_20260225`. Override `CTP_SDK_ROOT` if your SDK root is elsewhere, or set `CTP_SDK_VERSION` to pin a specific version directory.

### Windows

```powershell
cd C:\path\to\Ctp.Net
.\Ctp.Bridge.C\build.ps1 -CtpSdkRoot C:\path\to\ctp-sdk
dotnet build .\Ctp.Net.sln -m:1
```

The native build copies the official CTP runtime libraries into `Ctp.Bridge.C/build`, so setting `CTP_BRIDGE_DIR` to that directory is enough for smoke tests and local development.

`Ctp.Bridge.Net` resolves native bridge libraries in this order:

1. `CTP_BRIDGE_DIR`
2. `AppContext.BaseDirectory`
3. `AppContext.BaseDirectory/native`

It does not read from the process current working directory.

## Smoke test

### Linux

```bash
cd /home/layez/Projects/Ctp.Net
CTP_BRIDGE_DIR=/home/layez/Projects/Ctp.Net/Ctp.Bridge.C/build dotnet run --project Tests/Ctp.Net.Tests/Ctp.Net.Tests.fsproj --no-restore
```

### Windows

```powershell
$env:CTP_BRIDGE_DIR = "C:\path\to\Ctp.Net\Ctp.Bridge.C\build"
dotnet run --project .\Tests\Ctp.Net.Tests\Ctp.Net.Tests.fsproj --no-restore
```

## Build note

With the current `.NET 10.0.201` SDK on this machine, `ProjectReference` builds are stable with `-m:1`. Plain multi-node `dotnet build` can fail inside MSBuild's `CombineTargetFrameworkInfoProperties` task even though the projects themselves are valid.

Use `dotnet build -m:1` for now.

## Minimal F# usage

```fsharp
open Ctp.Net

let connection =
    CtpConnectionOptions.Create(
        frontAddress = "tcp://180.168.146.187:10211",
        flowPath = "./flow"
    )

use md = new MdClient(connection)
match md.Connect() |> Async.RunSynchronously with
| Ok () -> ()
| Error error -> failwithf "Connect failed: %A" error

let login =
    UserLoginRequest.Create("9999", "demo", "secret")

let result = md.LoginAsync(login) |> Async.RunSynchronously
printfn "%A" result
```
