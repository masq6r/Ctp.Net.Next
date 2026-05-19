# Queries

## 项目目的

`Demos/Queries` 是一个基于 `TraderClient` 的控制台示例，用来演示：

- 从 `options.local.json` 读取 `CtpOptions` 和 `CtpFlowControlOptions`
- 初始化 `TraderClient`
- 完成连接、认证、登录与结算信息确认
- 在结算确认后并发发起两个查询：
  - `QueryInstrumentAsync("", "", "", "")`
  - `QueryTradingAccountAsync()`
- 在控制台打印两个查询的发起时间，以及各自完成后的摘要信息

这个示例的重点是展示 `TraderClient` 在并发查询场景下如何按照 `CtpFlowControlOptions` 执行流控。

## 运行条件

运行此示例前，需要满足以下条件：

- 已安装 .NET SDK 10
- 当前仓库可以正常构建 `Ctp.Net`
- 本机能够访问配置中的 CTP Trader Front
- 已准备可用的交易账号、密码，以及认证所需的 `AppId` / `AuthCode`（如果柜台要求）
- `ctpOptions.flowPath` 指向的目录已存在，并且当前用户有读写权限

配置文件要求：

- 本项目运行时读取 `Demos/Queries/options.local.json`
- 目录中已提供 `Demos/Queries/options.template.json`
- 请以 `options.template.json` 为基础复制并修改，生成自己的 `options.local.json`

例如：

```bash
cp Demos/Queries/options.template.json Demos/Queries/options.local.json
```

然后按实际环境填写：

- `ctpOptions.frontAddress`
- `ctpOptions.brokerId`
- `ctpOptions.userId`
- `ctpOptions.password`
- `ctpOptions.userProductInfo`
- `ctpOptions.appId`
- `ctpOptions.authCode`
- `ctpOptions.flowPath`

`ctpFlowControlOptions` 也可以按需调整，用于观察并发查询在不同流控参数下的行为。

## 构建方法

在仓库根目录执行：

```bash
dotnet build Demos/Queries/Queries.fsproj -m:1
```

说明：

- 该命令会同时构建 `Ctp.Net`
- `Ctp.Net` 的 MSBuild 目标会自动构建并复制 NativeBridge 产物
- 本仓库约定 `dotnet build` 使用 `-m:1`

## 运行方法

在仓库根目录执行：

```bash
dotnet run --project Demos/Queries/Queries.fsproj
```

运行成功后，控制台会依次输出：

- 连接、认证、登录、结算确认过程
- 两个查询各自的发起时间
- 查询完成后的摘要结果：
  - 合约查询：返回的 instrument 总数，以及用 `,` 拼接后的 `InstrumentId`
  - 资金账户查询：返回的 `BrokerId` 与 `Balance`

## 配置说明

`options.local.json` 包含三部分：

- `ctpOptions`：CTP 连接与认证配置
- `ctpFlowControlOptions`：请求/查询流控配置
- `connectTimeoutMs`：连接超时时间

其中：

- `ctpOptions.frontAddress` 应填写 Trader Front 地址，而不是 MD Front 地址
- `ctpOptions.productionMode` 需要与目标环境匹配
- `ctpFlowControlOptions.maxQueriesPerSecond`、`queryCompletionTimeoutMs` 等参数会直接影响并发查询的节奏与等待行为
