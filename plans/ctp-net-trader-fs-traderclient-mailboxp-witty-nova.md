# TraderClient mailboxprocessor simplification plan

## Context
当前 `Ctp.Net/Trader.fs` 的 `TraderClient` 只包装了 Trader API 的一个很小子集，但它已经暴露出一个趋势：同一类回调会在消息 DU、mailbox match、`api.SetCallbacks` wiring 三处重复展开。如果直接基于这 17 个已实现 callback 来设计重构，会把“当前偶然长相”误当成“整个 Trader SPI 的真实结构”。

官方 `CThostFtdcTraderSpi` 显示，Trader 回调面远大于当前实现：
- 当前 bridge 只接了 `ctp_trader_spi` 的 17 个 callback：`NativeBridge/include/ctp_bridge.h:391`
- 官方 Trader SPI 则包含大批 `OnRsp*`、`OnRtn*`、`OnErrRtn*` 以及少量标量系统回调：`NativeBridge/ctp-sdk/v6.7.13_20260225/reference/ThostFtdcTraderApi.h:29`
- 尤其值得注意的是：当前 bridge 已经支持 `SubscribePrivateTopic(resumeType, seqNo)`，但还没有接 `OnRtnPrivateSeqNo`：`NativeBridge/include/ctp_bridge.h:431` vs `NativeBridge/ctp-sdk/v6.7.13_20260225/reference/ThostFtdcTraderApi.h:51`

因此，这次计划的目标不应是“把今天的 mailbox 写得更短”，而应是先按官方 callback 家族建立一个稳定分类，再据此设计 managed 层的路由抽象，让未来扩展全量 Trader API 时不会再次塌缩成一个巨型 mailbox。

## Recommended approach
### 1. 先按官方 Trader SPI 建立 callback taxonomy，再让代码结构服从 taxonomy
重构的设计基线应来自官方 SPI，而不是当前 `TraderClient` 子集。建议先把 Trader callbacks 分成下面四大类：

1. **Correlated responses**
   - 形状：`OnRspXxx(T* payload, RspInfo*, nRequestID, bIsLast)`
   - 代表：`OnRspAuthenticate`、`OnRspUserLogin`、所有 `OnRspQry*`、以及大量业务响应族
   - 参考：`NativeBridge/ctp-sdk/v6.7.13_20260225/reference/ThostFtdcTraderApi.h:48`, `:55`, `:127`, `:265`, `:406`

2. **Generic correlated error**
   - 形状：`OnRspError(RspInfo*, nRequestID, bIsLast)`
   - 它和 `OnRsp*` 共享 request correlation，但没有 typed payload，且承担全局错误语义
   - 参考：`NativeBridge/ctp-sdk/v6.7.13_20260225/reference/ThostFtdcTraderApi.h:265`

3. **Push notifications**
   - 形状：`OnRtnXxx(T* payload)`
   - 代表：`OnRtnOrder`、`OnRtnTrade`、`OnRtnQuote`、`OnRtnExecOrder`、`OnRtnInstrumentStatus`、`OnRtnTradingNotice` 等
   - 参考：`NativeBridge/ctp-sdk/v6.7.13_20260225/reference/ThostFtdcTraderApi.h:268`, `:280`, `:304`, `:523`, `:547`, `:565`

4. **Async error pushes**
   - 形状：`OnErrRtnXxx(T* payload, RspInfo*)`
   - 代表：`OnErrRtnOrderInsert`、`OnErrRtnOrderAction`、`OnErrRtnQuoteInsert`、`OnErrRtnExecOrderInsert` 等
   - 这类回调当前一个都没接，但它是完整 Trader 封装里绝不能忽略的一大家族
   - 参考：`NativeBridge/ctp-sdk/v6.7.13_20260225/reference/ThostFtdcTraderApi.h:274`, `:295`, `:307`, `:319`, `:385`, `:526`, `:550`, `:568`

此外保留一个小类：
- **Scalar/system callbacks**
  - 形状：无 payload 或简单标量，如 `OnFrontConnected`、`OnFrontDisconnected(int)`、`OnHeartBeatWarning(int)`、`OnRtnPrivateSeqNo(int)`
  - 参考：`NativeBridge/ctp-sdk/v6.7.13_20260225/reference/ThostFtdcTraderApi.h:33`, `:42`, `:46`, `:53`

后续所有 managed 抽象都应该围绕这几个家族设计，而不是围绕“Authenticate / Login / QryTradingAccount / OrderInsert”这些当前碰巧已实现的 API 名字设计。

### 2. 在 managed 层把“消息种类”改成“回调家族 + 完成语义”
在 `Ctp.Net/Trader.fs` 里，不再维持“一条 `OnRsp*` 对应一个 mailbox case”的模型，而改成少量按家族组织的消息：
- `CorrelatedResponse`：承载所有 `OnRsp*` typed 响应
- `CorrelatedError`：承载 `OnRspError`
- `PushNotification`：承载 `OnRtn*`
- `AsyncErrorPush`：承载 `OnErrRtn*`
- `SystemEvent`：承载连接、心跳、私有流 seqNo 等标量事件

其中 `CorrelatedResponse` 再通过 completion policy 区分：
- **FinalOnly**：登录、认证、确认等单结果响应
- **StreamUntilLast**：`OnRspQry*` 这类流式查询响应

当前 `TraderClient` 已有的 `PendingQueryDict` 正好是这个方向的起点，因为它已经把 request-id keyed accumulation 做出来了：
- `Ctp.Net/Common.fs:183`
- `Ctp.Net/Common.fs:238`
- `Ctp.Net/Common.fs:224`

但这次不应只做“把现有 9 个 `TryAccumulate` case 合并成 1 个”这么窄的动作；而应该把它升级成“官方 `OnRsp*` 家族的统一 completion engine”。

### 3. 保留 order family 的独立语义，不把它错误并入普通 request/response 模型
官方资料和本地 pitfalls 都说明，订单相关回调不能被误建模成“一个请求 -> 一个成功响应”：
- `ReqOrderInsert` 返回 0 只表示写 socket 成功，不表示业务成功：`.claude/skills/ctp/docs/pitfalls.md:57`
- `OnRspOrderInsert` 不是“下单成功回执”，而是请求级响应；拒单还可能走 `OnErrRtnOrderInsert`：`.claude/skills/ctp/docs/pitfalls.md:63`
- `OnRtnOrder` 是真实的订单状态流，`OnRtnTrade` 是成交流，而且两者的关联键也不同：`.claude/skills/ctp/docs/pitfalls.md:32`, `:37`

因此计划里要明确：
- `OnRspOrderInsert` / `OnRspOrderAction` 不应作为“order success completion”
- 未来补齐 `OnErrRtnOrderInsert` / `OnErrRtnOrderAction` 后，它们应归入 `AsyncErrorPush` 家族
- `OnRtnOrder` / `OnRtnTrade` 继续作为独立 push stream 暴露，不进入 `PendingQueryDict`

这不仅适用于订单，也适用于 quote / exec-order / option-self-close / comb-action / spd-apply / hedge-cfm 等整族 API，因为官方 header 里这些家族普遍都呈现：
- `OnRspXxx`
- `OnRtnXxx`
- `OnErrRtnXxx`

也就是说，mailbox 的抽象边界应该面向“家族模式”，而不是面向“订单是特殊例外”。

### 4. 先补 callback 家族的骨架，再决定是否继续消解 request 侧模板代码
当前 `TraderClient` 请求侧已经有 `QueryAsync<'TItem, 'TRequest>`：`Ctp.Net/Trader.fs:265`。它对今天的 `ReqQry*` 子集够用，但不足以代表整个 Trader API，因为：
- 还有大量 `OnRsp*` 不是 query，但仍是 correlated responses
- 还有整族 `OnErrRtn*` 和 `OnRtn*` 完全不走这个模型

因此推荐顺序是：
1. **先** 在 bridge + managed 层建立 callback family 骨架，让新增 callback 时不再扩散出新的 mailbox 形状
2. **再** 在这个骨架稳定后，视需要把 `AuthenticateAsync` / `LoginAsync` / `SettlementInfoConfirmAsync` 等 request-side 模板代码进一步统一

这样可以避免先把 `TraderClient` 请求接口抽象死，后面又因为 `OnErrRtn*` / `OnRtn*` 家族接入而推翻。

### 5. 以“扩全量 Trader SPI 时是否还稳定”为标准评估设计
通过官方资料可以看到，未来高概率会扩进来的不只是更多 `OnRspQry*`，而是整批新家族：
- `OnErrRtnOrderInsert` / `OnErrRtnOrderAction`
- `OnRtnInstrumentStatus` / `OnRtnTradingNotice`
- `OnRspQrySettlementInfo` / `OnRspQryClassifiedInstrument` / `OnRspQryBrokerTradingParams`
- `OnRtnQuote` / `OnErrRtnQuoteInsert`
- `OnRtnExecOrder` / `OnErrRtnExecOrderInsert`
- `OnRtnOptionSelfClose` / `OnErrRtnOptionSelfCloseInsert`
- `OnRtnCombAction` / `OnErrRtnCombActionInsert`
- `OnRtnSpdApply` / `OnErrRtnSpdApply`
- `OnRtnHedgeCfm` / `OnErrRtnHedgeCfm`

所以最终要采用的重构方案，必须满足这个检验：
- 新增一个 `OnRsp*` 不需要新增一类 mailbox 结构
- 新增一个 `OnRtn*` 不需要硬塞进 request completion 体系
- 新增一个 `OnErrRtn*` 不需要临时拼一个第三种错误通道
- 新增一个 scalar/system callback（如 `OnRtnPrivateSeqNo`）也有明确落点

## Target code skeleton
### `Ctp.Net/Common.fs`
目标不是重写 `PendingQueryDict`，而是在它现有的 request-id keyed accumulation 之上，补一个更贴近官方 `OnRsp*` 家族的 completion policy 层。

建议新增一个内部 completion policy：
- `FinalOnly`
- `StreamUntilLast`

并给 `PendingQueryDict` 增加一个统一入口，语义上等价于：
- 输入：`requestId`、`payload option`、`rspInfo`、`isLast`、`completionPolicy`
- 行为：
  - `FinalOnly`：忽略非 final 响应，只在 `isLast = true` 时完成
  - `StreamUntilLast`：非 final 响应持续累积，`isLast = true` 时完成
- 保留现有：
  - `Register<'a>`
  - `TryFail`
  - 最终结果类型仍是 `Result<'a list, RspInfo>`

这一层的目的，是把“什么时候完成 pending 请求”从 mailbox 分支里拿走。

### `Ctp.Net/Trader.fs` 顶部消息骨架
当前 `TraderAgentMessage` 在 `Ctp.Net/Trader.fs:8` 以“每个 callback 一个 case”的方式展开。目标骨架应改成按家族组织：

1. `SystemEvent`
   - `FrontConnected`
   - `FrontDisconnected of int`
   - `HeartBeatWarning of int`
   - 未来落点：`PrivateSeqNo of int`

2. `CorrelatedError`
   - 只承载 `RspInfo option * requestId * isLast`
   - 对应 `OnRspError`

3. `CorrelatedResponse`
   - 承载：`completionPolicy * requestId * payload option * rspInfo option * isLast`
   - 对应所有 typed `OnRsp*`
   - 这里的 payload 可以在 mailbox 层保持为 `obj option`，因为 mailbox 本身不需要理解类型，只需要把它交还给 `PendingQueryDict`

4. `OrderCommandResponse`
   - 承载：`operationName * rspInfo option * requestId * isLast`
   - 当前覆盖 `RspOrderInsert` / `RspOrderAction`
   - 未来可以容纳同族 command-style 响应，但不进入 pending completion

5. `PushNotification`
   - `OrderReceived of OrderUpdate`
   - `TradeReceived of TradeUpdate`
   - 未来落点：`QuoteReceived`、`ExecOrderReceived`、`InstrumentStatusReceived`、`TradingNoticeReceived` 等

6. `AsyncErrorPush`
   - 当前 bridge 尚未实现，但目标骨架里应预留
   - 未来承载 `OnErrRtnOrderInsert` / `OnErrRtnOrderAction` / `OnErrRtnQuoteInsert` 等

目标不是现在一次性把所有未来 callback 都写出来，而是把消息形状收敛到这几个稳定桶里。

### `Ctp.Net/Trader.fs` mailbox match 骨架
`Ctp.Net/Trader.fs:74` 附近的 mailbox 最终应只保留少量稳定分支：
- `SystemEvent`：连接协调和公开事件触发
- `CorrelatedError`：日志 + `RspError` 事件 + `isLast` 时 `pending.TryFail`
- `CorrelatedResponse`：统一委托给 `PendingQueryDict` 的新入口
- `OrderCommandResponse`：仅在 final 回调上处理非 0 错误，并 fan-out 到错误事件
- `PushNotification`：直接触发用户事件
- `AsyncErrorPush`：与 request completion 分离，走独立错误/状态通道

这样 mailbox 就不再随着 `OnRspQry*` 或新业务响应族的数量线性增长。

### `Ctp.Net/Trader.fs` callback adapter 骨架
`api.SetCallbacks` 仍然必须显式列出每个 Trader callback，但每一行应该只做“映射到哪个消息桶”的选择，而不是重新构造一套消息类型。

建议在 `api.SetCallbacks` 附近放一组局部 adapter：
- `postSystemEvent`
- `postCorrelatedError`
- `postFinalOnlyResponse`
- `postStreamingResponse`
- `postOrderCommandResponse`
- `postPushNotification`
- 未来：`postAsyncErrorPush`

当前 `Ctp.Net/Trader.fs:143` 到 `:184` 的 callback wiring 可以据此收敛成：
- `RspAuthenticate` / `RspSettlementInfoConfirm` / `RspUserLogin` / `RspUserLogout` -> `postFinalOnlyResponse`
- `RspQryTradingAccount` / `RspQryInvestorPosition` / `RspQryInstrumentMarginRate` / `RspQryExchangeMarginRate` / `RspQryInstrumentCommissionRate` -> `postStreamingResponse`
- `RspOrderInsert` / `RspOrderAction` -> `postOrderCommandResponse`
- `RtnOrder` / `RtnTrade` -> `postPushNotification`

这一步是压缩重复的核心，因为未来新增一个 callback 时，新增的是“一行映射 + 一个 family 归类”，而不是“DU case + mailbox arm + wiring lambda”三连扩散。

### request-side helper 的边界
`Ctp.Net/Trader.fs:197` 之后的请求侧方法这次不作为主战场。

建议边界如下：
- 保留 `QueryAsync<'TItem, 'TRequest>` 作为 query 子集 helper
- 先不急着把所有 non-query `Req*` 强行并进统一 helper
- 等 callback family 骨架稳定后，再评估是否新增一个与 `FinalOnly` 匹配的 request helper，用来收敛 `AuthenticateAsync` / `SettlementInfoConfirmAsync` / `LoginAsync`
- `LogoutAsync` 继续显式保留特殊完成语义，不隐藏到通用 helper 里

### 最小落地顺序
1. 在 `Ctp.Net/Common.fs` 给 `PendingQueryDict` 增加 completion policy 支撑
2. 在 `Ctp.Net/Trader.fs` 收缩 `TraderAgentMessage` 为稳定消息桶
3. 用 adapter 函数重写 `api.SetCallbacks`
4. 保持 `InsertOrderAsync` / `CancelOrderAsync` / `LogoutAsync` 现有语义不变
5. 用一个未来样本家族验证骨架可扩展，例如 `OnRtnPrivateSeqNo` 或 `OnErrRtnOrderInsert`

## Callback inventory / checklist
### Official Trader SPI taxonomy baseline
| Family | Official shape | Approx. official count | Current bridge coverage | Managed target bucket | Notes |
| --- | --- | ---: | --- | --- | --- |
| Correlated typed responses | `OnRspXxx(T*, RspInfo*, nRequestID, bIsLast)` | 123 (`OnRspError` included) | Partial | `CorrelatedResponse` + completion policy | Includes query and non-query responses. Source: `NativeBridge/ctp-sdk/v6.7.13_20260225/reference/ThostFtdcTraderApi.h`. |
| Generic correlated error | `OnRspError(RspInfo*, nRequestID, bIsLast)` | 1 | Yes | `CorrelatedError` | Must stay distinct from typed `OnRsp*` because it has no payload but still correlates by `nRequestID`. |
| Push notifications | `OnRtnXxx(T*)` | 30 | Very partial | `PushNotification` | Includes order/trade, quote, exec-order, instrument-status, bank-futures, hedge/spd families. |
| Async error pushes | `OnErrRtnXxx(T*, RspInfo*)` | 22 | None | `AsyncErrorPush` | Entire family currently absent from bridge; must be first-class in future design. |
| Scalar/system callbacks | `OnFrontConnected()` / `OnFrontDisconnected(int)` / `OnHeartBeatWarning(int)` / `OnRtnPrivateSeqNo(int)` | 4 | 3/4 | `SystemEvent` | `OnRtnPrivateSeqNo` is the notable current gap. |

### Current bridged Trader subset
| Callback family | Implemented now | File references | Implication for refactor |
| --- | --- | --- | --- |
| System callbacks | `OnFrontConnected`, `OnFrontDisconnected`, `OnHeartBeatWarning` | `NativeBridge/include/ctp_bridge.h:391`, `Ctp.Net/Bridge/TraderBridge.fs:242`, `Ctp.Net/Trader.fs:75` | Keep on a small dedicated path; no need to force through pending completion. |
| Final-only correlated responses | `OnRspAuthenticate`, `OnRspSettlementInfoConfirm`, `OnRspUserLogin`, `OnRspUserLogout` | `NativeBridge/include/ctp_bridge.h:395`, `Ctp.Net/Trader.fs:94` | These should share one `FinalOnly` completion policy instead of one mailbox branch each. |
| Streamed correlated query responses | `OnRspQryTradingAccount`, `OnRspQryInvestorPosition`, `OnRspQryInstrumentMarginRate`, `OnRspQryExchangeMarginRate`, `OnRspQryInstrumentCommissionRate` | `NativeBridge/include/ctp_bridge.h:400`, `Ctp.Net/Trader.fs:102` | These should share one `StreamUntilLast` completion policy. |
| Generic correlated error | `OnRspError` | `NativeBridge/include/ctp_bridge.h:399`, `Ctp.Net/Trader.fs:82` | Keep special handling: immediate error fan-out plus request-correlated fail path. |
| Order request responses | `OnRspOrderInsert`, `OnRspOrderAction` | `NativeBridge/include/ctp_bridge.h:405`, `Ctp.Net/Trader.fs:112` | Do not treat as order success completion. Preserve independent order semantics. |
| Push streams | `OnRtnOrder`, `OnRtnTrade` | `NativeBridge/include/ctp_bridge.h:407`, `Ctp.Net/Trader.fs:134` | Keep as push/event streams, outside pending response completion. |

### High-priority missing families to test the abstraction against
| Sample gap | Official family | Why it matters |
| --- | --- | --- |
| `OnRtnPrivateSeqNo` | Scalar/system callback | Current API already exposes `SubscribePrivateTopic(..., seqNo)` but not the callback that makes sequence-based resume observable. |
| `OnErrRtnOrderInsert`, `OnErrRtnOrderAction` | Async error push | Official and local pitfalls both indicate order rejection cannot be modeled by `OnRspOrderInsert` alone. |
| `OnRspQrySettlementInfo`, `OnRspQryClassifiedInstrument`, `OnRspQryBrokerTradingParams` | Correlated typed response | These verify that the response abstraction scales beyond today's five query wrappers. |
| `OnRtnInstrumentStatus`, `OnRtnTradingNotice` | Push notification | These verify that non-order push events have a natural home. |
| `OnRtnQuote` / `OnErrRtnQuoteInsert` / `OnRtnExecOrder` / `OnErrRtnExecOrderInsert` | Family-pattern expansion | These validate that the design works for other Trader subdomains, not just orders. |

### Pre-refactor checklist
- [ ] Confirm every official callback can be assigned to exactly one managed bucket: `CorrelatedResponse`, `CorrelatedError`, `PushNotification`, `AsyncErrorPush`, or `SystemEvent`.
- [ ] Split `CorrelatedResponse` completion semantics into at least `FinalOnly` and `StreamUntilLast`.
- [ ] Keep `OnRspError` independent from typed `OnRsp*` completion logic.
- [ ] Keep `OnRtnOrder` / `OnRtnTrade` outside `PendingQueryDict` and outside request-success semantics.
- [ ] Reserve a first-class path for `OnErrRtn*`; do not postpone it into ad-hoc special cases.
- [ ] Ensure adding a new `OnRsp*` does not require adding a new mailbox shape.
- [ ] Ensure adding a new `OnRtn*` does not require routing through request completion.
- [ ] Ensure adding a new scalar/system callback such as `OnRtnPrivateSeqNo` has an obvious landing zone.
- [ ] Validate that current `LogoutAsync` special completion behavior remains explicit and unaffected.
- [ ] Use at least one future sample from each family (`OnRspQry*`, non-query `OnRsp*`, `OnRtn*`, `OnErrRtn*`, scalar/system) to pressure-test the design before broad rollout.

## Critical files
- `NativeBridge/ctp-sdk/v6.7.13_20260225/reference/ThostFtdcTraderApi.h`
- `.claude/skills/ctp/docs/pitfalls.md`
- `NativeBridge/include/ctp_bridge.h`
- `NativeBridge/src/trader_bridge.cpp`
- `Ctp.Net/Bridge/TraderBridge.fs`
- `Ctp.Net/Common.fs`
- `Ctp.Net/Trader.fs`
- `Tests/Ctp.Net.Tests/Program.fs`
- `Tests/Ctp.Net.SmokeTests/Program.fs`

## Verification
### Design verification before coding
在开始实现前，先做一张 callback inventory / taxonomy 对照表，至少覆盖：
- 官方 `OnRsp*` 是否全部能落到 `CorrelatedResponse + completion policy`
- 官方 `OnRspError` 是否独立落到 `CorrelatedError`
- 官方 `OnRtn*` 是否全部能落到 `PushNotification`
- 官方 `OnErrRtn*` 是否全部能落到 `AsyncErrorPush`
- `OnFrontConnected` / `OnFrontDisconnected` / `OnHeartBeatWarning` / `OnRtnPrivateSeqNo` 是否能落到 `SystemEvent`

只有当这张对照表没有明显漏斗，才进入编码阶段。

### Unit verification after implementation
补充 `Tests/Ctp.Net.Tests/Program.fs`，重点验证新的 completion engine：
- FinalOnly 响应只在 `isLast = true` 时完成
- StreamUntilLast 响应会在多次回调后聚合完成
- `OnRspError` 仍能按 `nRequestID` 失败对应 pending 请求
- `OnErrRtn*` 通道不会误影响 `PendingQueryDict`
- `OnRtn*` 通道不会被 request completion 逻辑吞掉

执行命令：
- `dotnet build Ctp.Net/Ctp.Net.fsproj -m:1`
- `dotnet build Tests/Ctp.Net.Tests/Ctp.Net.Tests.fsproj -m:1`
- `dotnet run --project Tests/Ctp.Net.Tests/Ctp.Net.Tests.fsproj --no-restore`

### Smoke verification after implementation
复用现有 Trader smoke tests 验证当前已包装子集没有回归：
- `Tests/Ctp.Net.SmokeTests/Program.fs:253`
- `Tests/Ctp.Net.SmokeTests/Program.fs:300`

执行命令：
- `dotnet build Tests/Ctp.Net.SmokeTests/Ctp.Net.SmokeTests.fsproj -m:1`
- `dotnet run --project Tests/Ctp.Net.SmokeTests/Ctp.Net.SmokeTests.fsproj --no-build`

重点观察：
- authenticate / login / settlement confirm / query trading account / margin / commission 仍能完成
- 错误密码路径仍通过 `RspInfo` 正确失败
- `LogoutAsync` 的现有特殊完成语义不回归

### Structural verification for future expansion
如果开始扩 callback 覆盖面，优先用一小组“跨家族样本”验证抽象是否站得住：
- 一个新的 `OnRspQry*`
- 一个新的非 query `OnRsp*`
- 一个新的 `OnRtn*`
- 一个新的 `OnErrRtn*`
- 一个新的 scalar/system callback（建议 `OnRtnPrivateSeqNo`）

只有这五类样本都能无额外 mailbox 形状扩散地接入，才说明这次抽象真的对全量 Trader SPI 生效。