[English](README.md) | [中文](README.zh-CN.md)

# Subscrption

## Purpose

`Demos/Subscrption` is a console demo based on `MdClient` that demonstrates:

- Reading `CtpOptions`, `CtpFlowControlOptions`, and a subscription list from `options.local.json`
- Initializing `MdClient`
- Completing connection and login
- Subscribing to market data for the configured instrument list
- Printing market data in `instrumentid@time: lastprice` format on each `DepthMarketData` callback
- Exiting when the user presses any key

This demo focuses on showing how `MdClient` subscribes to market data from configuration and performs lightweight real-time output in the callback path.

## Prerequisites

Before running this demo:

- .NET SDK 10 installed
- The repo can successfully build `Ctp.Net`
- The machine can reach the configured CTP MD Front
- A valid trading account, password, and required login fields
- The directory pointed to by `ctpOptions.flowPath` must already exist with read/write permissions

Configuration file:

- The demo reads `Demos/Subscrption/options.local.json` at runtime
- A template is provided at `Demos/Subscrption/options.template.json`
- Copy and modify the template to create your own `options.local.json`

For example:

```bash
cp Demos/Subscrption/options.template.json Demos/Subscrption/options.local.json
```

Then fill in your actual environment values:

- `ctpOptions.frontAddress`
- `ctpOptions.brokerId`
- `ctpOptions.userId`
- `ctpOptions.password`
- `ctpOptions.userProductInfo`
- `ctpOptions.flowPath`
- `instrumentIds`

`ctpFlowControlOptions` can also be adjusted to observe behavior under different subscription batch sizes and send intervals.

## Build

From the repository root:

```bash
dotnet build Demos/Subscrption/Subscrption.fsproj -m:1
```

Notes:

- This command also builds `Ctp.Net`
- The `Ctp.Net` MSBuild target automatically builds and copies NativeBridge artifacts
- This repo uses `-m:1` for `dotnet build`

## Run

From the repository root:

```bash
dotnet run --project Demos/Subscrption/Subscrption.fsproj
```

On success, the console outputs:

- Connection and login progress
- The list of instruments actually subscribed
- Real-time market data in the format: `instrumentid@time: lastprice`
- A prompt to press any key to exit

## Configuration

`options.local.json` has four sections:

- `ctpOptions` — CTP connection and login settings
- `ctpFlowControlOptions` — request/subscription flow-control settings
- `instrumentIds` — list of instruments to subscribe to
- `connectTimeoutMs` — connection timeout

Notes:

- `ctpOptions.frontAddress` must point to an MD Front, not a Trader Front
- `instrumentIds` must contain at least one non-empty instrument code
- `ctpFlowControlOptions.subscriptionBatchSize` and `subscriptionBatchDelayMs` directly affect the send pacing for large subscription batches
- Keep callback handlers lightweight to avoid blocking the underlying callback thread
