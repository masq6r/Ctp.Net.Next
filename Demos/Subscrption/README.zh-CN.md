[English](README.md) | [中文](README.zh-CN.md)

# Subscrption

## 项目目的

`Demos/Subscrption` 是一个基于 `MdClient` 的控制台示例，用来演示：

- 从 `options.local.json` 读取 `CtpOptions`、`CtpFlowControlOptions` 和订阅列表
- 初始化 `MdClient`
- 完成连接与登录
- 按配置中的合约列表订阅行情
- 在收到 `DepthMarketData` 时，按 `instrumentid@time: lastprice` 的格式打印行情
- 用户按下任意键后退出程序

这个示例的重点是展示 `MdClient` 如何基于配置完成行情订阅，以及如何在接收回调时做最轻量的实时输出。

## 运行条件

运行此示例前，需要满足以下条件：

- 已安装 .NET SDK 10
- 当前仓库可以正常构建 `Ctp.Net`
- 本机能够访问配置中的 CTP MD Front
- 已准备可用的交易账号、密码，以及登录所需字段
- `ctpOptions.flowPath` 指向的目录已存在，并且当前用户有读写权限

配置文件要求：

- 本项目运行时读取 `Demos/Subscrption/options.local.json`
- 目录中已提供 `Demos/Subscrption/options.template.json`
- 请以 `options.template.json` 为基础复制并修改，生成自己的 `options.local.json`

例如：

```bash
cp Demos/Subscrption/options.template.json Demos/Subscrption/options.local.json
```

然后按实际环境填写：

- `ctpOptions.frontAddress`
- `ctpOptions.brokerId`
- `ctpOptions.userId`
- `ctpOptions.password`
- `ctpOptions.userProductInfo`
- `ctpOptions.flowPath`
- `instrumentIds`

`ctpFlowControlOptions` 也可以按需调整，用于观察不同订阅批次和发送节奏下的行为。

## 构建方法

在仓库根目录执行：

```bash
dotnet build Demos/Subscrption/Subscrption.fsproj -m:1
```

说明：

- 该命令会同时构建 `Ctp.Net`
- `Ctp.Net` 的 MSBuild 目标会自动构建并复制 NativeBridge 产物
- 本仓库约定 `dotnet build` 使用 `-m:1`

## 运行方法

在仓库根目录执行：

```bash
dotnet run --project Demos/Subscrption/Subscrption.fsproj
```

运行成功后，控制台会依次输出：

- 连接与登录过程
- 实际订阅的合约列表
- 收到的实时行情，格式为：`instrumentid@time: lastprice`
- 提示用户按任意键退出

## 配置说明

`options.local.json` 包含四部分：

- `ctpOptions`：CTP 连接与登录配置
- `ctpFlowControlOptions`：请求/订阅流控配置
- `instrumentIds`：需要订阅的合约列表
- `connectTimeoutMs`：连接超时时间

其中：

- `ctpOptions.frontAddress` 应填写 MD Front 地址，而不是 Trader Front 地址
- `instrumentIds` 至少要包含一个非空合约代码
- `ctpFlowControlOptions.subscriptionBatchSize`、`subscriptionBatchDelayMs` 会直接影响大批量订阅时的发送节奏
- 订阅回调里只做轻量输出，避免阻塞底层回调线程
