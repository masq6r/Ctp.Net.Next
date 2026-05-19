[English](README.md) | [中文](README.zh-CN.md)

# NativeBridge

`NativeBridge` 是本仓库的原生 C++ 部分。

它链接官方 CTP SDK，并在 [`include/ctp_bridge.h`](include/ctp_bridge.h) 中暴露稳定的 C ABI，使 [`../Ctp.Net/Bridge/`](../Ctp.Net/Bridge) 中的托管 F# 层可以使用 `DllImport`，而无需直接调用厂商的 C++ ABI。

## 目录内容

- `CMakeLists.txt` — 原生桥接构建定义
- `include/ctp_bridge.h` — 导出的 C ABI，与托管层共享
- `src/md_bridge.cpp` — 行情桥接
- `src/trader_bridge.cpp` — 交易桥接
- `build.sh` — Linux 构建入口
- `build.ps1` — Windows 构建入口
- `ctp-sdk/` — 内置 SDK 根目录，按版本子目录组织

## 本目录在仓库中的位置

如果你构建托管项目：

```bash
dotnet build Ctp.Net/Ctp.Net.fsproj -m:1
```

MSBuild 会自动调用 `NativeBridge/build.sh`（Linux）或 `NativeBridge/build.ps1`（Windows），然后将原生产物复制到托管输出目录。

当你需要直接在 C++ 桥接层上迭代、检查原生输出或调试仅涉及桥接层的问题时，请使用本文档中的独立原生构建命令。

## 内置 SDK 目录结构

`ctp-sdk` 是 SDK 的根目录。其下每个一级子目录是一个版本目录，例如：

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

- `CTP_SDK_ROOT` 应指向 `ctp-sdk` 根目录。
- `CTP_SDK_VERSION` 是可选的，用于固定该根目录下的某个版本目录。
- 如果省略 `CTP_SDK_VERSION`，CMake 会自动选择最新的匹配版本目录。
- 为了兼容，`CTP_SDK_ROOT` 也可以直接指向单个版本目录。
- `ctp-sdk/<version>/reference` 包含官方 CTP API 参考文档，在修改桥接结构体或回调约定之前应先查阅。

## 前置条件

### 工具链

#### Linux

- `cmake >= 3.20`
- C++17 编译器，如 `g++` 或 `clang++`
- `make` 或其他 CMake 后端

#### Windows

- Visual Studio 2022 Build Tools 或包含 C++ 工作负载的完整 Visual Studio
- CMake
- 64 位工具链

## 仅构建原生桥接层

除非另有说明，以下命令在仓库根目录执行。

### Linux

默认内置 SDK 根目录：

```bash
./NativeBridge/build.sh
```

自定义 SDK 根目录：

```bash
CTP_SDK_ROOT=/path/to/ctp-sdk ./NativeBridge/build.sh
```

固定特定内置版本目录：

```bash
CTP_SDK_ROOT=/path/to/ctp-sdk CTP_SDK_VERSION=v6.7.13_20260225 ./NativeBridge/build.sh
```

### Windows

显式指定 SDK 根目录：

```powershell
.\NativeBridge\build.ps1 -CtpSdkRoot C:\path\to\ctp-sdk
```

使用环境变量：

```powershell
$env:CTP_SDK_ROOT = 'C:\path\to\ctp-sdk'
.\NativeBridge\build.ps1
```

固定特定内置版本目录：

```powershell
.\NativeBridge\build.ps1 -CtpSdkRoot C:\path\to\ctp-sdk -CtpSdkVersion v6.7.13_20260225
```

## 构建做了什么

- 使用 CMake 配置 `NativeBridge/`
- 从 `CTP_SDK_ROOT` 和可选的 `CTP_SDK_VERSION` 解析 SDK 版本目录
- 构建两个共享库：
  - `ctpmd_bridge`
  - `ctptrader_bridge`
- 将官方 CTP 运行时库复制到同一输出目录

在 Linux 上，桥接目标还会设置 RPATH，使生成的库能够从构建输出目录结构中解析所依赖的 `.so` 文件。

## 预期输出

Linux 构建成功后，`NativeBridge/build` 应包含如下文件：

```text
libctpmd_bridge.so
libctptrader_bridge.so
thostmduserapi_se.so
thosttraderapi_se.so
```

Windows 构建成功后，`NativeBridge/build` 应包含如下等效文件：

```text
ctpmd_bridge.dll
ctptrader_bridge.dll
thostmduserapi_se.dll
thosttraderapi_se.dll
```

## 托管层如何查找桥接库

托管层按以下顺序解析原生桥接库：

1. `CTP_BRIDGE_DIR`
2. `AppContext.BaseDirectory`
3. `AppContext.BaseDirectory/native`

它不使用进程当前工作目录。

典型手动设置：

### Linux

```bash
export CTP_BRIDGE_DIR=/home/layez/repos/Ctp.Net.Next/NativeBridge/build
```

### Windows

```powershell
$env:CTP_BRIDGE_DIR = 'C:\path\to\Ctp.Net\NativeBridge\build'
```

## 相关示例和消费者

- [`../Ctp.Net/Ctp.Net.fsproj`](../Ctp.Net/Ctp.Net.fsproj) 在托管构建过程中自动构建本目录。
- [`../Demos/CtpDemo.Local.Native`](../Demos/CtpDemo.Local.Native) 是一个本地原生 C++ Trader 示例，用于直接的请求/回调实验。
- [`../Tests/Ctp.Net.SmokeTests`](../Tests/Ctp.Net.SmokeTests) 依赖桥接输出和有效的 `smoke.local.json` 配置。

## 常见问题

### `CTP_SDK_ROOT is not set`

当 SDK 不在 `NativeBridge/ctp-sdk` 下时，请显式设置 `CTP_SDK_ROOT`，或向 `build.ps1` 传递 `-CtpSdkRoot`。

### Required SDK path does not exist

确认所选 SDK 目录同时包含：

- `mduserapi/<platform>`
- `traderapi/<platform>`

如果存在多个版本，还需确认 `CTP_SDK_VERSION` 指向了预期的版本。

### 托管构建成功但运行时抛出 `DllNotFoundException`

按以下顺序检查：

1. 原生桥接库是否确实已构建
2. 桥接文件是否存在于 `NativeBridge/build`
3. 官方 CTP 运行时库是否已复制到桥接库旁边
4. 如果不从托管输出目录加载，`CTP_BRIDGE_DIR` 是否指向正确的目录

### Linux 库解析问题

使用以下命令检查生成的桥接文件：

```bash
ldd /home/layez/repos/Ctp.Net.Next/NativeBridge/build/libctpmd_bridge.so
ldd /home/layez/repos/Ctp.Net.Next/NativeBridge/build/libctptrader_bridge.so
```

两个桥接库都应能从 `NativeBridge/build` 或配置的运行时搜索路径中解析其依赖的 CTP `.so` 文件。
