# Native Bridge Build Guide

This document covers only the native C++ bridge under `NativeBridge/` for `Ctp.Net`.

## Purpose

The official CTP SDK ships as C++ libraries. `Ctp.Net` does not call those libraries directly from .NET. Instead, it builds two thin native bridge libraries:

- `ctpmd_bridge`
- `ctptrader_bridge`

These expose a stable C ABI for the F# interop layer in `Ctp.Net/Bridge/`.

## Directory Layout

- `NativeBridge/CMakeLists.txt`: CMake project for the native bridge
- `NativeBridge/src/md_bridge.cpp`: market data bridge
- `NativeBridge/src/trader_bridge.cpp`: trader bridge
- `NativeBridge/include/ctp_bridge.h`: exported C ABI header
- `NativeBridge/build.sh`: Linux build entrypoint
- `NativeBridge/build.ps1`: Windows build entrypoint
- `NativeBridge/ctp-sdk`: bundled SDK root

## Prerequisites

## SDK

`ctp-sdk` remains the SDK root. Under it, each first-level child is a version directory, for example:

```text
NativeBridge/ctp-sdk/
└── v6.7.13_20260225/
    ├── mduserapi/
    │   ├── linux-x64/
    │   └── win-x64/
    └── traderapi/
        ├── linux-x64/
        └── win-x64/
```

`CTP_SDK_ROOT` should point to the `ctp-sdk` root directory. `CTP_SDK_VERSION` is optional and can be used to pin a specific version directory. If omitted, CMake selects the latest matching version directory automatically.

For backward compatibility, `CTP_SDK_ROOT` may also point directly at a single version directory.

## Toolchain

### Linux

- `cmake >= 3.20`
- a C++17 compiler such as `g++` or `clang++`
- `make` or another backend supported by CMake

### Windows

- Visual Studio 2022 Build Tools or full Visual Studio with C++ workload
- CMake
- a 64-bit toolchain

## Build on Linux

From the project root:

```bash
cd /home/layez/repos/Ctp.Net.Next
./NativeBridge/build.sh
```

If the SDK root is elsewhere:

```bash
cd /home/layez/repos/Ctp.Net.Next
CTP_SDK_ROOT=/path/to/ctp-sdk ./NativeBridge/build.sh
```

To pin a specific version directory:

```bash
cd /home/layez/repos/Ctp.Net.Next
CTP_SDK_ROOT=/path/to/ctp-sdk CTP_SDK_VERSION=v6.7.13_20260225 ./NativeBridge/build.sh
```

What `build.sh` does:

1. configures `NativeBridge/` with CMake
2. uses `CTP_SDK_ROOT` if provided
3. optionally uses `CTP_SDK_VERSION` if provided
4. writes build output to `NativeBridge/build`
5. builds both bridge shared libraries

## Build on Windows

From the project root in PowerShell:

```powershell
cd C:\path\to\Ctp.Net
.\NativeBridge\build.ps1 -CtpSdkRoot C:\path\to\ctp-sdk
```

If `CTP_SDK_ROOT` is already set:

```powershell
cd C:\path\to\Ctp.Net
$env:CTP_SDK_ROOT = 'C:\path\to\ctp-sdk'
.\NativeBridge\build.ps1
```

To pin a specific version directory:

```powershell
cd C:\path\to\Ctp.Net
.\NativeBridge\build.ps1 -CtpSdkRoot C:\path\to\ctp-sdk -CtpSdkVersion v6.7.13_20260225
```

The PowerShell script:

1. configures `NativeBridge/` with CMake
2. writes output to `NativeBridge/build`
3. builds the bridge in `Release`

## Expected Output

After a successful Linux build, `NativeBridge/build` should contain:

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

The official CTP runtime libraries are copied into the build output directory by CMake so that the managed layer can load everything from one place.

## How the Managed Layer Finds the Bridge

The F# interop layer checks `CTP_BRIDGE_DIR` first.

Typical usage:

### Linux

```bash
export CTP_BRIDGE_DIR=/home/layez/repos/Ctp.Net.Next/NativeBridge/build
```

### Windows

```powershell
$env:CTP_BRIDGE_DIR = 'C:\path\to\Ctp.Net\NativeBridge\build'
```

## Rebuild From Scratch

If you need a clean rebuild:

### Linux

```bash
rm -rf /home/layez/repos/Ctp.Net.Next/NativeBridge/build
cd /home/layez/repos/Ctp.Net.Next
./NativeBridge/build.sh
```

### Windows

Delete `NativeBridge/build`, then run:

```powershell
cd C:\path\to\Ctp.Net
.\NativeBridge\build.ps1 -CtpSdkRoot C:\path\to\ctp-sdk
```

## Common Problems

## `CTP_SDK_ROOT is not set`

Cause:

- the SDK path is not in the default location
- `CTP_SDK_ROOT` was not provided

Fix:

- set `CTP_SDK_ROOT` explicitly

## Linker cannot find `thostmduserapi_se` or `thosttraderapi_se`

Cause:

- wrong SDK directory
- SDK directory structure does not match `ctp-sdk/<version>/{mduserapi,traderapi}/{linux-x64,win-x64}`

Fix:

- verify the extracted SDK path
- verify the library files exist under the expected Linux or Windows subdirectories
- if multiple versions exist, verify `CTP_SDK_VERSION` points to the intended one

## .NET can build but runtime throws `DllNotFoundException`

Cause:

- `CTP_BRIDGE_DIR` is not set
- bridge libraries were not built
- runtime libraries were not copied into `NativeBridge/build`

Fix:

1. rebuild `NativeBridge`
2. confirm the bridge files exist in `NativeBridge/build`
3. set `CTP_BRIDGE_DIR` to `NativeBridge/build`

## Linux runtime library resolution issues

Check the generated output with:

```bash
ldd /home/layez/repos/Ctp.Net.Next/NativeBridge/build/libctpmd_bridge.so
ldd /home/layez/repos/Ctp.Net.Next/NativeBridge/build/libctptrader_bridge.so
```

Both bridge libraries should resolve their dependent CTP `.so` files from `NativeBridge/build` or the configured RPATH/load path.

## Related Files

- [NativeBridge/CMakeLists.txt](NativeBridge/CMakeLists.txt)
- [NativeBridge/build.sh](NativeBridge/build.sh)
- [NativeBridge/build.ps1](NativeBridge/build.ps1)
- [README.md](README.md)
