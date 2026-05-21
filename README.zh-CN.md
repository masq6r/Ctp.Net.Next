[English](README.md) | [中文](README.zh-CN.md)

[![Build](https://github.com/masq6r/Ctp.Net.Next/actions/workflows/build.yml/badge.svg?branch=master)](https://github.com/masq6r/Ctp.Net.Next/actions/workflows/build.yml)
[![Tests](https://img.shields.io/endpoint?url=https://raw.githubusercontent.com/masq6r/Ctp.Net.Next/master/badges/test-results.json)](https://github.com/masq6r/Ctp.Net.Next/actions/workflows/test.yml)

# Ctp.Net

CTP SDK 的 `.NET` 封装库。

本仓库将厂商 C++ SDK 置于一个轻量原生桥接层之后，使托管层可以在 Linux 和 Windows 上暴露 `Async<Result<...>>` API、.NET 事件以及托管领域类型。

作为个人自动化交易系统`eXp`的基础设施的重要组成部分，`Ctp.Net.Next`为其7x24小时不间断稳定运行长达5年不宕机，达成42%年复合收益率提供了坚实保障。这部分组件开源，希望为有志于投身自动化交易的个人提供帮助，愿你们能早日找到属于自己的圣杯。

## 免责声明

**本软件按「原样」提供，不作任何明示或默示的保证。** 使用风险自负。作者对因使用本软件而产生的任何财务损失、交易错误或其他损害不承担责任。本项目与 CTP SDK 厂商无任何关联。本仓库中的任何内容均不构成财务或投资建议。

### CTP SDK 原生库再分发提示

出于打包便利性的考虑，本项目的 NuGet 包中可能包含来源于 CTP SDK 的原生二进制文件。这些文件的相关权利仍归其各自权利人所有。

本仓库中的任何表述，均不应被解释为就上游 CTP SDK 的再分发提供单独许可、授权、背书或法律意见。在将本项目用于生产环境、内部发放、商业分发或任何进一步再分发场景之前，你有责任自行审查上游 SDK 条款、适用法律法规以及自身合规义务，并在必要时自行取得相应许可或授权。

如果你的预期用途、分发模式或所在司法辖区存在额外限制，请不要仅依赖随包提供的二进制文件。若法律、合规或上游政策方面出现要求，维护者保留在后续版本中调整、限制或移除这些原生文件的权利。

## 亮点

- 提供了开箱即用的开发体验，无须任何额外配置
- 使用**异步工作流**（C#的`async`与`await`，F#的`async`计算表达式）进行查询，避免回调地狱
- `CtpFlowControlOptions` 用于查询**节流、原生重试处理以及订阅批次控制**
- 使用`CancellationToken`随时**取消**在途查询
- 稳定的断线重连与订阅恢复机制
- 使用**托管类型**例如`DateOnly`, `DateTime`等代替CTP SDK的字符串
- 与常见 CTP 部署对齐的非对称编码策略：出站 `GBK`，入站 `GB18030`
- 紧跟最新CTP SDK更新

## 仓库结构

- `Ctp.Net` — 托管 F# 库；`Bridge/` 包含底层互操作，顶层文件暴露公开客户端
- `NativeBridge` — C++ 桥接层，导出 C ABI，内置 `ctp-sdk`，以及原生构建入口
- `Demos/Subscription` — 用于登录和行情订阅的 `MdClient` 示例
- `Demos/Queries` — 用于认证、结算确认以及托管流控下并发查询的 `TraderClient` 示例
- `Demos/CtpDemo.Local.Native` — 原生 C++ Trader 示例，用于对照官方 API 检查请求/回调行为
- `Tests/Ctp.Net.Tests` — 快速单元测试
- `Tests/Ctp.Net.SmokeTests` — 针对真实 CTP 前置的集成测试

## 构建

### 托管库

在仓库根目录执行：

```bash
dotnet build Ctp.Net/Ctp.Net.fsproj -m:1
```

说明：

- 此构建会自动在 Linux 上运行 `NativeBridge/build.sh`，在 Windows 上运行 `NativeBridge/build.ps1`。
- 原生桥接产物会被复制到托管输出目录。
- 请使用 `-m:1`；多节点构建可能在本仓库的 MSBuild `CombineTargetFrameworkInfoProperties` 任务中失败。

### 仅构建原生桥接层

如果你直接在 C++ 桥接层上迭代，可以单独构建：

#### Linux

```bash
./NativeBridge/build.sh
```

#### Windows

```powershell
.\NativeBridge\build.ps1 -CtpSdkRoot C:\path\to\ctp-sdk
```

有关原生工作流和 SDK 目录结构的详细信息，请参见 [NativeBridge/README.md](NativeBridge/README.md)。

## 测试

### 单元测试

```bash
dotnet run --project Tests/Ctp.Net.Tests/Ctp.Net.Tests.fsproj --no-restore
```

列出测试：

```bash
dotnet run --project Tests/Ctp.Net.Tests/Ctp.Net.Tests.fsproj --no-build -- -list tests
```

运行单个测试：

```bash
dotnet run --project Tests/Ctp.Net.Tests/Ctp.Net.Tests.fsproj --no-build -- -method "Ctp.Net.Tests.EncodingTests.outbound encoding defaults to GBK"
```

### 冒烟测试

冒烟测试默认跳过，直到 `Tests/Ctp.Net.SmokeTests/smoke.local.json` 存在并包含有效的前置地址和凭据设置。

运行冒烟测试项目：

```bash
dotnet run --project Tests/Ctp.Net.SmokeTests/Ctp.Net.SmokeTests.fsproj
```

运行单个冒烟测试：

```bash
dotnet run --project Tests/Ctp.Net.SmokeTests/Ctp.Net.SmokeTests.fsproj --no-build -- -method "Ctp.Net.SmokeTests.MdSmokeTests.md client can connect login subscribe and receive market data"
```

## 运行时说明

- 原生桥接库查找顺序为：
  1. `CTP_BRIDGE_DIR`
  2. `AppContext.BaseDirectory`
  3. `AppContext.BaseDirectory/native`
- 托管层不使用进程当前工作目录来解析原生库。
- `flowPath` 必须在 `Init()` 之前已存在；库不会自动创建。
- 不要在不同 API 实例之间共享同一个 `flowPath`。
- `productionMode` 默认为 `true`。对于测试或模拟环境，当前置需要时请显式设置为 `false`。
- Trader 认证在 API 层面是可选的，但许多前置要求提供；当你的环境需要时，请使用 `userProductInfo`、`appId` 和 `authCode`。

## 示例

### `Demos/Subscription`

构建：

```bash
dotnet build Demos/Subscription/Subscription.fsproj -m:1
```

运行：

```bash
dotnet run --project Demos/Subscription/Subscription.fsproj
```

此示例读取 `Demos/Subscription/options.local.json`，连接 `MdClient`，登录，订阅配置的合约，并打印简短的行情更新。

### `Demos/Queries`

构建：

```bash
dotnet build Demos/Queries/Queries.fsproj -m:1
```

运行：

```bash
dotnet run --project Demos/Queries/Queries.fsproj
```

此示例读取 `Demos/Queries/options.local.json`，通过 `TraderClient` 进行认证和登录，在需要时确认结算，然后发起并发查询以演示托管流控行为。

### `Demos/CtpDemo.Local.Native`

此原生 C++ 示例复用 `Tests/Ctp.Net.SmokeTests/smoke.local.json`，适用于希望在不经过托管封装的情况下检查原生 Trader API 请求/回调行为。

## 最小 F# 用法

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

运行客户端之前请先创建 flow 目录。

## 原生桥接架构

本仓库是围绕官方 CTP SDK 的两层封装：

1. `NativeBridge` 链接厂商 SDK，并在 `NativeBridge/include/ctp_bridge.h` 中暴露稳定的 C ABI。
2. `Ctp.Net/Bridge` 通过 `DllImport`、原生结构体、回调注册、安全句柄和编组来镜像该 ABI。
3. `Ctp.Net/Md.fs` 和 `Ctp.Net/Trader.fs` 将回调驱动的原生 API 封装为公开客户端类、异步工作流和 .NET 事件。

## 编码策略

- .NET 字符串在托管层保持 Unicode
- 出站请求默认使用 `GBK`
- 入站响应和事件默认使用 `GB18030`
- 编码可按 `MdClient` / `TraderClient` 实例覆盖

这比在 SDK 边界强制使用 UTF-8 更符合常见的 CTP 部署实际情况。
