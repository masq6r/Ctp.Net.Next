[English](README.md) | [中文](README.zh-CN.md)

[![Build](https://github.com/masq6r/Ctp.Net.Next/actions/workflows/build.yml/badge.svg?branch=master)](https://github.com/masq6r/Ctp.Net.Next/actions/workflows/build.yml)
[![Tests](https://img.shields.io/endpoint?url=https://raw.githubusercontent.com/masq6r/Ctp.Net.Next/master/badges/test-results.json)](https://github.com/masq6r/Ctp.Net.Next/actions/workflows/test.yml)

# Ctp.Net

CTP SDK 的 `.NET` 封装库。

本仓库将厂商 C++ SDK 置于一个轻量原生桥接层之后，使托管层可以在 Linux 和 Windows 上暴露 F# 优先的 `Async<Result<...>>` API，并提供 `Ctp.Net.CSharp` 中的 C# 风格 `Task<T>` 封装、.NET 事件以及托管领域类型。

作为个人自动化交易系统`eXp`的基础设施的重要组成部分，`Ctp.Net.Next`为其7x24小时不间断稳定运行长达5年不宕机，达成42%年复合收益率提供了坚实保障。这部分组件开源，希望为有志于投身自动化交易的个人提供帮助，愿你们能早日找到属于自己的圣杯。

## 免责声明

**本软件按「原样」提供，不作任何明示或默示的保证。** 使用风险自负。作者对因使用本软件而产生的任何财务损失、交易错误或其他损害不承担责任。本项目与 CTP SDK 厂商无任何关联。本仓库中的任何内容均不构成财务或投资建议。

### CTP SDK 原生库再分发提示

出于打包便利性的考虑，本项目的 NuGet 包中可能包含来源于 CTP SDK 的原生二进制文件。这些文件的相关权利仍归其各自权利人所有。

本仓库中的任何表述，均不应被解释为就上游 CTP SDK 的再分发提供单独许可、授权、背书或法律意见。在将本项目用于生产环境、内部发放、商业分发或任何进一步再分发场景之前，你有责任自行审查上游 SDK 条款、适用法律法规以及自身合规义务，并在必要时自行取得相应许可或授权。

如果你的预期用途、分发模式或所在司法辖区存在额外限制，请不要仅依赖随包提供的二进制文件。若法律、合规或上游政策方面出现要求，维护者保留在后续版本中调整、限制或移除这些原生文件的权利。

## 亮点

- 提供了开箱即用的开发体验，无须任何额外配置
- F# 优先的客户端提供 `Async<Result<...>>`，另有 **C# 风格的封装**（`Ctp.Net.CSharp.MdClient` / `TraderClient`）返回原生 `Task` / `Task<T>`，支持标准 `CancellationToken` 以及 `EventHandler<T>` 事件 — 无需引用 `Microsoft.FSharp.*`
- `CtpFlowControlOptions` 用于查询**节流、原生重试处理以及订阅批次控制**
- 使用`CancellationToken`随时**取消**在途查询
- 稳定的断线重连与订阅恢复机制
- 使用**托管类型**例如`DateOnly`, `DateTime`等代替CTP SDK的字符串
- 与常见 CTP 部署对齐的非对称编码策略：出站 `GBK`，入站 `GB18030`
- 紧跟最新CTP SDK更新

## 最小化示例
### 最小 F# 用法

```fsharp
open Ctp.Net

let options =
    CtpOptions.Create(
        frontAddress = "tcp://180.168.146.187:10211",
        brokerId = "9999",
        userId = "demo",
        password = "secret",
        flowPath = "/tmp/ctp-flow-md",
        productionMode = false
    )

async {
    use md = new MdClient(ctpOpt)
    let! _ = md.Connect()
    match! md.LoginAsync() with
    | Ok login -> printfn $"Logged in. TradingDay={login.TradingDay}"
    | _ -> ()
}
|> Async.RunSynchronously
```

运行客户端之前请先创建 flow 目录。

### 最小 C# 用法

```csharp
using Ctp.Net;
using Ctp.Net.CSharp;

var options = CtpOptions.Create(
    frontAddress: "tcp://180.168.146.187:10211",
    brokerId: "9999",
    userId: "demo",
    password: "secret",
    flowPath: "/tmp/ctp-flow-md"
);

using var md = new MdClient(options);
await md.ConnectAsync();
var login = await md.LoginAsync();
Console.WriteLine($"Logged in. TradingDay={login.TradingDay}");
```

`Ctp.Net.CSharp.MdClient` 和 `Ctp.Net.CSharp.TraderClient` 封装将 F# 层的 `Async<Result<_,_>>` 转换为原生 `Task<T>`，将失败映射为类型化异常（`CtpResponseException`、`CtpTimeoutException` 等），使用标准 `EventHandler<T>` 事件，并接受 C# 原生的可选参数——无需 `Microsoft.FSharp.*` 引用。

与 F# 优先 API 的主要差异：
- `md.LoginAsync()` 返回 `Task<UserLoginResponse>`，失败时抛出 `CtpResponseException`
- `trader.QueryTradingAccountAsync()` 返回 `Task<IReadOnlyList<TradingAccountResponse>>`
- `CancellationToken` 通过 `[Optional]` 默认参数传递
- 事件使用 `EventHandler<T>` 配合 `+=` / `-=` 语法
- `FSharpList<T>` 替换为 `IReadOnlyList<T>`

## 仓库结构

- `Ctp.Net` — 托管 F# 库；`Bridge/` 包含底层互操作，顶层文件暴露 F# 优先的公开客户端，`CSharp/` 提供 C# 风格的封装
- `NativeBridge` — C++ 桥接层，导出 C ABI，内置 `ctp-sdk`，以及原生构建入口
- `Demos/Subscription.fsx` — 用于登录和行情订阅的 F# 脚本版 `MdClient` 示例
- `Demos/Subscription.cs` — 使用 `Ctp.Net.CSharp.MdClient` 的 C# file-based app 示例，演示同样的登录与行情订阅流程
- `Demos/QueryAccount.fsx` — 用于认证、登录、结算确认和资金账户查询的 F# 脚本版 `TraderClient` 示例
- `Demos/QueryAccount.cs` — 使用 `Ctp.Net.CSharp.TraderClient` 的 C# file-based app 示例，演示同样的资金账户查询流程
- `Demos/QueryCancellation.fsx` — F# 脚本版 `TraderClient` 查询取消流控示例
- `Demos/QueryCancellation.cs` — 使用 `Ctp.Net.CSharp.TraderClient` 的 C# file-based app 查询取消流控示例
- `Demos/FlowControl` — 用于认证、结算确认以及托管流控下并发查询的 `TraderClient` 示例
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

### `Demos/Subscription.fsx`

这个 F# 脚本演示了 `MdClient` 的连接、登录和行情订阅流程。

运行前请先：

1. 编辑 `Demos/Subscription.fsx` 中的 `ctpOpt`，按你的环境修改以下字段：
   - `frontAddress`
   - `brokerId`
   - `userId`
   - `password`
   - `flowPath`
   - `productionMode`
   - `userProductInfo`
   - `appId`
   - `authCode`
2. 编辑同一文件中的 `instrumentIds`，改成你要订阅的合约。
3. 在 `Init()` 之前先创建 `flowPath` 指向的目录。

例如：

```bash
mkdir -p /tmp/ctp-flow-md
```

运行：

```bash
dotnet fsi Demos/Subscription.fsx
```

说明：

- 这个脚本是自包含的，不读取 `options.local.json`。
- 当前脚本通过 `#r "nuget: Ctp.Net.Next"` 引用已发布的 NuGet 包。

### `Demos/Subscription.cs`

这个 C# file-based app 使用 C# 风格的 `Ctp.Net.CSharp.MdClient` 演示了同样的连接、登录和行情订阅流程（无需 `Microsoft.FSharp.*` 引用）。

运行前请先：

1. 编辑 `Demos/Subscription.cs` 中的 `ctpOptions`，按你的环境修改以下字段：
   - `frontAddress`
   - `brokerId`
   - `userId`
   - `password`
   - `flowPath`
   - `productionMode`
   - `userProductInfo`
   - `appId`
   - `authCode`
2. 编辑同一文件中的 `instrumentIds`，改成你要订阅的合约。
3. 在 `Init()` 之前先创建 `flowPath` 指向的目录。

构建：

```bash
dotnet build -- Demos/Subscription.cs
```

运行：

```bash
dotnet run --file Demos/Subscription.cs
```

说明：

- 这个 file-based app 是自包含的，不读取 `options.local.json`。
- `dotnet build -- Demos/Subscription.cs` 里的 `--` 用于确保该文件被当作 file-based app，而不是 MSBuild 项目路径。

### `Demos/QueryAccount.fsx`

这个 F# 脚本演示了 `TraderClient` 的连接、认证、登录、结算确认和资金账户查询流程。

运行前请先：

1. 编辑 `Demos/QueryAccount.fsx` 中的 `ctpOpt`，按你的环境修改以下字段：
   - `frontAddress`
   - `brokerId`
   - `userId`
   - `password`
   - `flowPath`
   - `productionMode`
   - `userProductInfo`
   - `appId`
   - `authCode`
2. 在 `Init()` 之前先创建 `flowPath` 指向的目录。
3. 确认你的 Trader Front 是否要求认证；很多前置都需要 `userProductInfo`、`appId` 和 `authCode`。

例如：

```bash
mkdir -p /tmp/ctp-flow-trader
```

运行：

```bash
dotnet fsi Demos/QueryAccount.fsx
```

说明：

- 这个脚本是自包含的，不读取 `options.local.json`。
- 当前脚本通过 `#r "nuget: Ctp.Net.Next"` 引用已发布的 NuGet 包。
- 该流程会先调用 `SettlementInfoConfirmAsync()`，再调用 `QueryTradingAccountAsync()`，以保持与现有 Trader 示例一致。

### `Demos/QueryAccount.cs`

这个 C# file-based app 使用 C# 风格的 `Ctp.Net.CSharp.TraderClient` 演示了同样的连接、认证、登录、结算确认和资金账户查询流程（无需 `Microsoft.FSharp.*` 引用）。

运行前请先：

1. 编辑 `Demos/QueryAccount.cs` 中的 `ctpOptions`，按你的环境修改以下字段：
   - `frontAddress`
   - `brokerId`
   - `userId`
   - `password`
   - `flowPath`
   - `productionMode`
   - `userProductInfo`
   - `appId`
   - `authCode`
2. 在 `Init()` 之前先创建 `flowPath` 指向的目录。
3. 确认你的 Trader Front 是否要求认证；很多前置都需要 `userProductInfo`、`appId` 和 `authCode`。

构建：

```bash
dotnet build -- Demos/QueryAccount.cs
```

运行：

```bash
dotnet run --file Demos/QueryAccount.cs
```

说明：

- 这个 file-based app 是自包含的，不读取 `options.local.json`。
- `dotnet build -- Demos/QueryAccount.cs` 里的 `--` 用于确保该文件被当作 file-based app，而不是 MSBuild 项目路径。
- 该示例会打印简短的结算确认摘要，以及第一条资金账户余额信息。

### `Demos/QueryCancellation.fsx`

这个 F# 脚本演示了 `TraderClient` 的查询取消息控制制：CTP 同一时间只允许一个在途（in-flight）查询，且查询一旦到达 CTP 服务器便无法被服务侧取消。脚本首先发起一个宽泛的 `QueryInstrumentAsync` 查询，待其派发后取消客户端异步操作并立即发起 `QueryTradingAccountAsync`，第二个查询会被阻塞，直到第一个查询的原生操作完成或超时。每个关键事件都带有时间戳，阻塞时长清晰可见。

运行前请先：

1. 编辑 `Demos/QueryCancellation.fsx` 中的 `ctpOpt`，按你的环境修改以下字段：
   - `frontAddress`
   - `brokerId`
   - `userId`
   - `password`
   - `flowPath`
   - `productionMode`
   - `userProductInfo`
   - `appId`
   - `authCode`
2. 在 `Init()` 之前先创建 `flowPath` 指向的目录。
3. 确认你的 Trader Front 是否要求认证；很多前置都需要 `userProductInfo`、`appId` 和 `authCode`。

例如：

```bash
mkdir -p /tmp/ctp-flow-query-cancel
```

运行：

```bash
dotnet fsi Demos/QueryCancellation.fsx
```

说明：

- 这个脚本是自包含的，不读取 `options.local.json`。
- 当前脚本通过 `#r "nuget: Ctp.Net.Next"` 引用已发布的 NuGet 包。
- `flowControl` 参数将 `QueryCompletionTimeout` 设为 15 秒；默认值为 120 秒。
- 预期输出：Q1 在 ~50ms 时被取消但仍持有在途槽位；Q2 持续阻塞，直至 CTP 服务器返回响应（或超时）。

### `Demos/QueryCancellation.cs`

这个 C# file-based app 使用 C# 风格的 `Ctp.Net.CSharp.TraderClient` 演示了同样的查询取消流控机制（无需 `Microsoft.FSharp.*` 引用）。

运行前请先：

1. 编辑 `Demos/QueryCancellation.cs` 中的 `ctpOptions`，按你的环境修改以下字段：
   - `frontAddress`
   - `brokerId`
   - `userId`
   - `password`
   - `flowPath`
   - `productionMode`
   - `userProductInfo`
   - `appId`
   - `authCode`
2. 在 `Init()` 之前先创建 `flowPath` 指向的目录。
3. 确认你的 Trader Front 是否要求认证；很多前置都需要 `userProductInfo`、`appId` 和 `authCode`。

构建：

```bash
dotnet build -- Demos/QueryCancellation.cs
```

运行：

```bash
dotnet run --file Demos/QueryCancellation.cs
```

说明：

- 这个 file-based app 是自包含的，不读取 `options.local.json`。
- `dotnet build -- Demos/QueryCancellation.cs` 里的 `--` 用于确保该文件被当作 file-based app，而不是 MSBuild 项目路径。
- 该示例利用了 `Ctp.Net.CSharp.TraderClient` 的 `CancellationToken` 支持进行客户端取消；`catch (OperationCanceledException)` 替代了 F# 中的模式匹配方式。
- 预期输出与 F# 脚本一致：Q1 被取消，Q2 阻塞，直至 Q1 的原生操作完成。

### `Demos/FlowControl`

构建：

```bash
dotnet build Demos/FlowControl/FlowControl.fsproj -m:1
```

运行：

```bash
dotnet run --project Demos/FlowControl/FlowControl.fsproj
```

此示例读取 `Demos/FlowControl/options.local.json`，通过 `TraderClient` 进行认证和登录，在需要时确认结算，然后发起并发查询以演示托管流控行为。

### `Demos/CtpDemo.Local.Native`

此原生 C++ 示例复用 `Tests/Ctp.Net.SmokeTests/smoke.local.json`，适用于希望在不经过托管封装的情况下检查原生 Trader API 请求/回调行为。

## 原生桥接架构

本仓库是围绕官方 CTP SDK 的两层封装：

1. `NativeBridge` 链接厂商 SDK，并在 `NativeBridge/include/ctp_bridge.h` 中暴露稳定的 C ABI。
2. `Ctp.Net/Bridge` 通过 `DllImport`、原生结构体、回调注册、安全句柄和编组来镜像该 ABI。
3. `Ctp.Net/Md.fs` 和 `Ctp.Net/Trader.fs` 将回调驱动的原生 API 封装为 F# 优先的公开客户端类，提供 `Async<Result<...>>` 和 `IEvent`。
4. `Ctp.Net/CSharp/` 提供 C# 风格的封装，将 `Async<Result<_,_>>` → `Task<T>` + 类型化异常，`IEvent` → `EventHandler<T>`，`FSharpOption` → C# 原生可选参数。

## 编码策略

- .NET 字符串在托管层保持 Unicode
- 出站请求默认使用 `GBK`
- 入站响应和事件默认使用 `GB18030`
- 编码可按 `MdClient` / `TraderClient` 实例覆盖

这比在 SDK 边界强制使用 UTF-8 更符合常见的 CTP 部署实际情况。
