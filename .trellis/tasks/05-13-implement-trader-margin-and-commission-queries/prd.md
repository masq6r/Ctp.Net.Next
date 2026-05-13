# implement trader margin and commission queries

## Goal

在 `TraderClient` 中实现 CTP SDK 提供的 `ReqQryInstrumentMarginRate`、`ReqQryExchangeMarginRate` 和 `ReqQryInstrumentCommissionRate`，沿用当前 Trader 查询链路，把 native bridge、F# interop、public wrapper 和必要测试补齐，让调用方可以像现有 `QueryTradingAccountAsync` / `QueryInvestorPositionAsync` 一样发起异步查询并拿到列表结果。

## What I already know

* 用户明确要求在 `TraderClient` 中实现这三个查询接口。
* 当前 `TraderClient` 已有通用 `QueryAsync` 模式，`QueryTradingAccountAsync` 与 `QueryInvestorPositionAsync` 通过 `PendingQueryDict` 聚合 `OnRspQry*` 回调结果。
* `Ctp.Net/Bridge/TraderBridge.fs` 已形成固定扩展模式：请求 record → native struct → DllImport → callback delegate → mapping → `TraderApi.Req*` 成员。
* `NativeBridge/include/ctp_bridge.h` 与 `NativeBridge/src/trader_bridge.cpp` 当前已覆盖 trading account / investor position 的请求与回调桥接，可作为模板扩展。
* SDK 头文件中已确认以下请求与响应结构存在：
  * `CThostFtdcQryInstrumentMarginRateField`
  * `CThostFtdcQryExchangeMarginRateField`
  * `CThostFtdcQryInstrumentCommissionRateField`
  * `CThostFtdcInstrumentMarginRateField`
  * `CThostFtdcExchangeMarginRateField`
  * `CThostFtdcInstrumentCommissionRateField`
* 三组请求都含有 `BrokerID`，其中：
  * instrument margin rate 额外含 `InvestorID`、`HedgeFlag`、`ExchangeID`、`InvestUnitID`、`InstrumentID`
  * exchange margin rate 含 `HedgeFlag`、`ExchangeID`、`InstrumentID`
  * instrument commission rate 含 `InvestorID`、`ExchangeID`、`InvestUnitID`、`InstrumentID`
* 响应结构中的价格/费率字段仍应遵循仓库既有约定，映射为 managed 侧 `decimal`。

## Assumptions (temporary)

* 这次只做最小可用实现：新增 3 个公开异步查询方法，返回 `Async<Result<... list, RspInfo>>`。
* 公开方法会延续当前 `TraderClient` 风格：自动填充 `BrokerId`，需要 `InvestorId` 的接口自动使用 `options.UserId`。
* 不新增额外事件流，也不扩展到其他相近但未要求的查询接口，例如 `ExchangeMarginRateAdjust`。

## Requirements (evolving)

* 在 `NativeBridge` 中新增三组请求/响应 C ABI struct、SPI 回调槽位和 `ctp_trader_req_*` 导出函数。
* 在 `Ctp.Net/Bridge/TraderBridge.fs` 中新增对应请求/响应 record、native struct、mapping、delegate、DllImport 和 `TraderApi` 成员。
* 在 `Ctp.Net/Trader.fs` 中把三组 `OnRspQry*` 回调接入 agent / pending 聚合，并公开对应 `Query*Async` 方法。
* 三个公开方法采用与现有查询一致的最小可选参数风格；`BrokerId` 自动来自 `options`，需要 `InvestorId` 的查询自动使用 `options.UserId`。
* `QueryExchangeMarginRateAsync` 只暴露 `hedgeFlag`、`exchangeId`、`instrumentId` 这组常用筛选项，并保持可选参数形式。
* 新接口应复用现有 `QueryAsync` 行为，包括 native return code 错误路径。
* 更新测试覆盖新增的请求/结果映射或 Trader 查询路径中的核心逻辑。

## Acceptance Criteria (evolving)

* [ ] `TraderClient` 暴露 `QueryInstrumentMarginRateAsync`。
* [ ] `TraderClient` 暴露 `QueryExchangeMarginRateAsync`。
* [ ] `TraderClient` 暴露 `QueryInstrumentCommissionRateAsync`。
* [ ] 三个接口均通过现有 pending query 聚合正确返回 `Async<Result<... list, RspInfo>>`。
* [ ] Native bridge、managed bridge、public wrapper 三层定义保持一致。
* [ ] 相关测试通过。

## Definition of Done (team quality bar)

* Tests added/updated (unit/integration where appropriate)
* Lint / typecheck / CI green
* Docs/notes updated if behavior changes
* Rollout/rollback considered if risky

## Technical Approach

沿用现有 `QueryTradingAccountAsync` / `QueryInvestorPositionAsync` 的查询链路，在 `NativeBridge/include/ctp_bridge.h`、`NativeBridge/src/trader_bridge.cpp`、`Ctp.Net/Bridge/TraderBridge.fs`、`Ctp.Net/Trader.fs` 四处平行扩展三组请求/响应定义与回调处理；managed 侧继续以 `PendingQueryDict` 聚合 `OnRspQry*` 多条结果，并把费率/保证金数值映射为 `decimal`。

## Decision (ADR-lite)

**Context**: 这三个接口可以做成“完整 request record 暴露到底层”或“沿用现有 TraderClient 的高层便捷方法”。
**Decision**: 采用现有 `TraderClient` 风格的最小可选参数 API，自动填充 `BrokerId` / `InvestorId`，只暴露调用方常用筛选项。
**Consequences**: public API 与现有查询保持一致、调用更轻；代价是公开面不会 1:1 暴露底层全部字段，但这次需求不需要更底层的形状。

## Out of Scope (explicit)

* `ReqQryExchangeMarginRateAdjust` 或其他未明确要求的相邻查询
* 改造现有 `TraderClient` 查询抽象
* 新增文档或 README 说明

## Technical Notes

* 现有查询模式参考：
  * `Ctp.Net/Trader.fs`
  * `Ctp.Net/Bridge/TraderBridge.fs`
  * `NativeBridge/include/ctp_bridge.h`
  * `NativeBridge/src/trader_bridge.cpp`
* SDK 结构参考：
  * `NativeBridge/ctp-sdk/v6.7.13_20260225/traderapi/linux-x64/ThostFtdcTraderApi.h`
  * `NativeBridge/ctp-sdk/v6.7.13_20260225/traderapi/linux-x64/ThostFtdcUserApiStruct.h`
* 约束：Managed 层 native lookup、编码策略、decimal 归一化、TraderClient 查询聚合方式需保持与现有实现一致。
