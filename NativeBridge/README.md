[English](README.md) | [中文](README.zh-CN.md)

# NativeBridge

`NativeBridge` is the native C++ half of the repository.

It links the official CTP SDK and exposes a stable C ABI in [`include/ctp_bridge.h`](include/ctp_bridge.h) so the managed F# layer in [`../Ctp.Net/Bridge/`](../Ctp.Net/Bridge) can use `DllImport` instead of calling the vendor C++ ABI directly.

## What lives here

- `CMakeLists.txt` — native bridge build definition
- `include/ctp_bridge.h` — exported C ABI shared by the managed layer
- `src/md_bridge.cpp` — market-data bridge
- `src/trader_bridge.cpp` — trader bridge
- `build.sh` — Linux build entrypoint
- `build.ps1` — Windows build entrypoint
- `ctp-sdk/` — bundled SDK root, versioned by subdirectory

## How this directory fits into the repo

If you build the managed project:

```bash
dotnet build Ctp.Net/Ctp.Net.fsproj -m:1
```

MSBuild automatically invokes `NativeBridge/build.sh` on Linux or `NativeBridge/build.ps1` on Windows, then copies the native artifacts into the managed output directory.

Use the standalone native commands in this document when you want to iterate on the C++ bridge directly, inspect the native output, or debug bridge-only issues.

## Bundled SDK layout

`ctp-sdk` stays as the SDK root. Under it, each first-level child is a version directory, for example:

```text
NativeBridge/ctp-sdk/
└── v6.7.13_20260225/
    ├── mduserapi/
    │   ├── linux-x64/
    │   └── win-x64/
    ├── traderapi/
    │   ├── linux-x64/
    │   └── win-x64/
    └── reference/
```

- `CTP_SDK_ROOT` should point to the `ctp-sdk` root directory.
- `CTP_SDK_VERSION` is optional and pins one version directory under that root.
- If `CTP_SDK_VERSION` is omitted, CMake selects the latest matching version directory automatically.
- For compatibility, `CTP_SDK_ROOT` may also point directly at a single version directory.
- `ctp-sdk/<version>/reference` contains the bundled official CTP API reference and should be consulted before changing bridge structs or callback contracts.

## Prerequisites

### Toolchain

#### Linux

- `cmake >= 3.20`
- a C++17 compiler such as `g++` or `clang++`
- `make` or another CMake backend

#### Windows

- Visual Studio 2022 Build Tools or full Visual Studio with the C++ workload
- CMake
- a 64-bit toolchain

## Build the native bridge only

Run these commands from the repository root unless noted otherwise.

### Linux

Default bundled SDK root:

```bash
./NativeBridge/build.sh
```

Custom SDK root:

```bash
CTP_SDK_ROOT=/path/to/ctp-sdk ./NativeBridge/build.sh
```

Pin a specific bundled version directory:

```bash
CTP_SDK_ROOT=/path/to/ctp-sdk CTP_SDK_VERSION=v6.7.13_20260225 ./NativeBridge/build.sh
```

### Windows

Explicit SDK root:

```powershell
.\NativeBridge\build.ps1 -CtpSdkRoot C:\path\to\ctp-sdk
```

Use environment variables:

```powershell
$env:CTP_SDK_ROOT = 'C:\path\to\ctp-sdk'
.\NativeBridge\build.ps1
```

Pin a specific bundled version directory:

```powershell
.\NativeBridge\build.ps1 -CtpSdkRoot C:\path\to\ctp-sdk -CtpSdkVersion v6.7.13_20260225
```

## What the build does

- configures `NativeBridge/` with CMake
- resolves the SDK version directory from `CTP_SDK_ROOT` and optional `CTP_SDK_VERSION`
- builds two shared libraries:
  - `ctpmd_bridge`
  - `ctptrader_bridge`
- copies the official CTP runtime libraries into the same output directory

On Linux, the bridge targets also set RPATH so the generated libraries can resolve the bundled `.so` files from the build output layout.

## Expected output

After a successful Linux build, `NativeBridge/build` should contain files such as:

```text
libctpmd_bridge.so
libctptrader_bridge.so
thostmduserapi_se.so
thosttraderapi_se.so
```

After a successful Windows build, `NativeBridge/build` should contain equivalents such as:

```text
ctpmd_bridge.dll
ctptrader_bridge.dll
thostmduserapi_se.dll
thosttraderapi_se.dll
```

## How the managed layer finds the bridge

The managed layer resolves native bridge libraries in this order:

1. `CTP_BRIDGE_DIR`
2. `AppContext.BaseDirectory`
3. `AppContext.BaseDirectory/native`

It does not use the process current working directory.

Typical manual setup:

### Linux

```bash
export CTP_BRIDGE_DIR=/home/layez/repos/Ctp.Net.Next/NativeBridge/build
```

### Windows

```powershell
$env:CTP_BRIDGE_DIR = 'C:\path\to\Ctp.Net\NativeBridge\build'
```

## Related demos and consumers

- [`../Ctp.Net/Ctp.Net.fsproj`](../Ctp.Net/Ctp.Net.fsproj) automatically builds this directory as part of the managed build.
- [`../Demos/CtpDemo.Local.Native`](../Demos/CtpDemo.Local.Native) is a local native C++ trader demo for direct request / callback experiments.
- [`../Tests/Ctp.Net.SmokeTests`](../Tests/Ctp.Net.SmokeTests) depends on the bridge output and a valid `smoke.local.json` configuration.

## Common problems

### `CTP_SDK_ROOT is not set`

Set `CTP_SDK_ROOT` explicitly when the SDK is not under `NativeBridge/ctp-sdk`, or pass `-CtpSdkRoot` to `build.ps1`.

### Required SDK path does not exist

Verify that the selected SDK directory contains both:

- `mduserapi/<platform>`
- `traderapi/<platform>`

If multiple versions exist, also verify `CTP_SDK_VERSION` points at the intended one.

### Managed build succeeds but runtime throws `DllNotFoundException`

Check these in order:

1. the native bridge was actually built
2. the bridge files exist in `NativeBridge/build`
3. the official CTP runtime libraries were copied beside the bridge
4. `CTP_BRIDGE_DIR` points at the correct directory when you are not loading from the managed output folder

### Linux library resolution problems

Inspect the generated bridge files with:

```bash
ldd /home/layez/repos/Ctp.Net.Next/NativeBridge/build/libctpmd_bridge.so
ldd /home/layez/repos/Ctp.Net.Next/NativeBridge/build/libctptrader_bridge.so
```

Both bridge libraries should resolve their dependent CTP `.so` files from `NativeBridge/build` or the configured runtime search path.
