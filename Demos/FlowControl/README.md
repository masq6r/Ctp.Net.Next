[English](README.md) | [中文](README.zh-CN.md)

# FlowControl

## Purpose

`Demos/FlowControl` is a console demo based on `TraderClient` that demonstrates:

- Reading `CtpOptions` and `CtpFlowControlOptions` from `options.local.json`
- Initializing `TraderClient`
- Completing connection, authentication, login, and settlement confirmation
- Issuing two concurrent queries after settlement confirmation:
  - `QueryInstrumentAsync("", "", "", "")`
  - `QueryTradingAccountAsync()`
- Printing the start time of each query and summary results after completion

This demo focuses on showing how `TraderClient` applies flow control per `CtpFlowControlOptions` under concurrent query scenarios.

## Prerequisites

Before running this demo:

- .NET SDK 10 installed
- The repo can successfully build `Ctp.Net`
- The machine can reach the configured CTP Trader Front
- A valid trading account, password, and `AppId` / `AuthCode` (if required by the front)
- The directory pointed to by `ctpOptions.flowPath` must already exist with read/write permissions

Configuration file:

- The demo reads `Demos/FlowControl/options.local.json` at runtime
- A template is provided at `Demos/FlowControl/options.template.json`
- Copy and modify the template to create your own `options.local.json`

For example:

```bash
cp Demos/FlowControl/options.template.json Demos/FlowControl/options.local.json
```

Then fill in your actual environment values:

- `ctpOptions.frontAddress`
- `ctpOptions.brokerId`
- `ctpOptions.userId`
- `ctpOptions.password`
- `ctpOptions.userProductInfo`
- `ctpOptions.appId`
- `ctpOptions.authCode`
- `ctpOptions.flowPath`

`ctpFlowControlOptions` can also be adjusted to observe concurrent query behavior under different flow-control parameters.

## Build

From the repository root:

```bash
dotnet build Demos/FlowControl/FlowControl.fsproj -m:1
```

Notes:

- This command also builds `Ctp.Net`
- The `Ctp.Net` MSBuild target automatically builds and copies NativeBridge artifacts
- This repo uses `-m:1` for `dotnet build`

## Run

From the repository root:

```bash
dotnet run --project Demos/FlowControl/FlowControl.fsproj
```

On success, the console outputs:

- Connection, authentication, login, and settlement confirmation progress
- The start time of each query
- Summary results after queries complete:
  - Instrument query: total count of returned instruments, and `InstrumentId` values joined by `,`
  - Trading account query: the returned `BrokerId` and `Balance`

## Configuration

`options.local.json` has three sections:

- `ctpOptions` — CTP connection and authentication settings
- `ctpFlowControlOptions` — request/query flow-control settings
- `connectTimeoutMs` — connection timeout

Notes:

- `ctpOptions.frontAddress` must point to a Trader Front, not an MD Front
- `ctpOptions.productionMode` must match the target environment
- `ctpFlowControlOptions.maxQueriesPerSecond`, `queryCompletionTimeoutMs`, and similar parameters directly affect the pacing and waiting behavior of concurrent queries
