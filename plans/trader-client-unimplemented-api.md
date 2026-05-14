# TraderClient 未覆盖的 Trader API 清单

## 口径

- 官方接口来源：`.claude/skills/ctp/docs/reference/ThostFtdcTraderApi.h`。
- 当前实现来源：`Ctp.Net/Trader.fs`、`Ctp.Net/Bridge/TraderBridge.fs`。
- 官方 `CThostFtdcTraderApi` 中可调用的 `int` 方法共 129 个；当前 `TraderClient`/`TraderBridge` 已覆盖其中 11 个请求接口：`ReqAuthenticate`, `ReqSettlementInfoConfirm`, `ReqUserLogin`, `ReqUserLogout`, `ReqQryTradingAccount`, `ReqQryInvestorPosition`, `ReqQryInstrumentMarginRate`, `ReqQryExchangeMarginRate`, `ReqQryInstrumentCommissionRate`, `ReqOrderInsert`, `ReqOrderAction`。
- 按上面的口径，**尚缺 118 个请求类/请求式接口**；另有 5 个常用非请求类方法尚未公开暴露。
- 下面表格只列 **尚未被 `TraderClient`/`TraderBridge` 暴露** 的官方接口；`CreateFtdcTraderApi` / `Release` / `Init` / `RegisterFront` / `RegisterSpi` / `SubscribePrivateTopic` / `SubscribePublicTopic` / `Join` 已被构造器、`Connect()`、`Dispose()`、`Join()` 间接覆盖，因此不再列入缺口。
- 处理模式说明：`FinalOnly` = 单次应答；`StreamUntilLast` = 流式查询直到 `bIsLast=true`；“返回 requestId” = 仿照现有 `InsertOrderAsync` / `CancelOrderAsync`，把真正状态变化交给回报流。

## 额外缺口：官方非请求类方法

| API | 类别 | 主要回调/回报 | 建议处理方式 | 备注 |
| --- | --- | --- | --- | --- |
| GetApiVersion | 工具 | 无 | 作为静态只读 API 暴露即可，直接返回 string。 |  |
| GetTradingDay | 会话 | 无 | 在 `TraderApi` 上直接透出同步方法；仅在登录成功后保证有效。 | 坑点文档明确要求以登录响应 / GetTradingDay 为准。 |
| GetFrontInfo | 会话 | 无 | 在 `TraderApi` 上直接透出同步方法，返回结构体或记录。 | 适合诊断当前 front / 名称服务解析结果。 |
| RegisterNameServer | 连接 | 无 | 在 `CtpOptions` 增加 NameServer 配置，并在 `Connect()` 前调用。 | 与 `RegisterFront` 互斥。 |
| RegisterFensUserInfo | 连接 | 无 | 扩展 `CtpOptions` 以支持 FENS 用户信息，并在 `Connect()` 前调用。 |  |

## 会话/认证/监管

| API | 类别 | 主要回调/回报 | 建议处理方式 | 备注 |
| --- | --- | --- | --- | --- |
| RegisterUserSystemInfo | 监管 | 无（仅返回 int） | 封装为同步 `Result<unit, RspInfo>`/`Result<int, RspInfo>`；必须在认证成功、登录前调用。 | relay/proxy 模式必需。 |
| SubmitUserSystemInfo | 监管 | 无（仅返回 int） | 封装为同步方法；管理员登录后可多次上报。 | relay/proxy 模式使用。 |
| RegisterWechatUserSystemInfo | 监管 | 无（仅返回 int） | 与 `RegisterUserSystemInfo` 相同，但面向微信小程序终端信息。 | 仅微信场景需要。 |
| SubmitWechatUserSystemInfo | 监管 | 无（仅返回 int） | 与 `SubmitUserSystemInfo` 相同。 | 仅微信场景需要。 |
| ReqUserPasswordUpdate | 认证 | OnRspUserPasswordUpdate | 按 `FinalOnly` 单次应答封装。 | 无推送回报。 |
| ReqTradingAccountPasswordUpdate | 认证 | OnRspTradingAccountPasswordUpdate | 按 `FinalOnly` 单次应答封装。 | 无推送回报。 |
| ReqUserAuthMethod | 认证 | OnRspUserAuthMethod | 按 `FinalOnly` 单次应答封装。 | 登录增强流程前置能力探测。 |
| ReqGenUserCaptcha | 认证 | OnRspGenUserCaptcha | 按 `FinalOnly` 单次应答封装。 | 返回图形验证码元数据。 |
| ReqGenUserText | 认证 | OnRspGenUserText | 按 `FinalOnly` 单次应答封装。 | 返回短信/文本验证码元数据。 |
| ReqUserLoginWithCaptcha | 认证 | OnRspUserLogin | 复用现有 `LoginAsync` 的 pending/解析逻辑，但请求结构改为 captcha 登录。 | 成功后仍由 `OnRspUserLogin` 返回登录结果。 |
| ReqUserLoginWithText | 认证 | OnRspUserLogin | 复用现有 `LoginAsync` 的 pending/解析逻辑。 | 成功后仍由 `OnRspUserLogin` 返回登录结果。 |
| ReqUserLoginWithOTP | 认证 | OnRspUserLogin | 复用现有 `LoginAsync` 的 pending/解析逻辑。 | 成功后仍由 `OnRspUserLogin` 返回登录结果。 |
| ReqGenSMSCode | 认证 | OnRspGenSMSCode | 按 `FinalOnly` 单次应答封装。 | v6.7.13+ 短信验证辅助接口。 |

## 报单/执行/报价/对冲类命令

| API | 类别 | 主要回调/回报 | 建议处理方式 | 备注 |
| --- | --- | --- | --- | --- |
| ReqParkedOrderInsert | 交易命令 | OnRspParkedOrderInsert | 按 `FinalOnly` 单次应答封装；成功仅表示预埋单录入成功。 | 后续实际触发需另查预埋单或等真实报单流。 |
| ReqParkedOrderAction | 交易命令 | OnRspParkedOrderAction | 按 `FinalOnly` 单次应答封装。 | 无直接 `OnRtn*`。 |
| ReqQryMaxOrderVolume | 交易辅助 | OnRspQryMaxOrderVolume | 可按单条查询封装，内部仍用 `StreamUntilLast`/`FinalOnly` 都可。 | 通常只返回一条。 |
| ReqRemoveParkedOrder | 交易命令 | OnRspRemoveParkedOrder | 按 `FinalOnly` 单次应答封装。 | 删除预埋单。 |
| ReqRemoveParkedOrderAction | 交易命令 | OnRspRemoveParkedOrderAction | 按 `FinalOnly` 单次应答封装。 | 删除预埋撤单。 |
| ReqExecOrderInsert | 期权执行 | OnRspExecOrderInsert / OnRtnExecOrder / OnErrRtnExecOrderInsert | 仿照 `InsertOrderAsync`：返回 `requestId`；`OnRsp*` 处理受理/同步错误，状态流靠 `OnRtnExecOrder`，异步错误补 `OnErrRtnExecOrderInsert`。 | 期权执行命令。 |
| ReqExecOrderAction | 期权执行 | OnRspExecOrderAction / OnRtnExecOrder / OnErrRtnExecOrderAction | 仿照 `CancelOrderAsync`；撤销/修改结果以 `OnRtnExecOrder` 为准。 |  |
| ReqForQuoteInsert | 询价 | OnRspForQuoteInsert / OnRtnForQuoteRsp / OnErrRtnForQuoteInsert | 返回 `requestId`；`OnRsp*` 仅做受理/同步错误，市场侧应答看 `OnRtnForQuoteRsp`。 | 询价响应是独立推送。 |
| ReqQuoteInsert | 报价 | OnRspQuoteInsert / OnRtnQuote / OnErrRtnQuoteInsert | 返回 `requestId`；`OnRtnQuote` 维护状态流。 | 做市/双边报价。 |
| ReqQuoteAction | 报价 | OnRspQuoteAction / OnRtnQuote / OnErrRtnQuoteAction | 返回 `requestId`；最终状态以 `OnRtnQuote` 为准。 |  |
| ReqBatchOrderAction | 交易命令 | OnRspBatchOrderAction / OnErrRtnBatchOrderAction / OnRtnOrder | 返回 `requestId`；`OnRsp*` 看受理，批量撤单后各单状态仍走 `OnRtnOrder`。 | 批量撤单。 |
| ReqOptionSelfCloseInsert | 期权自对冲 | OnRspOptionSelfCloseInsert / OnRtnOptionSelfClose / OnErrRtnOptionSelfCloseInsert | 返回 `requestId`；状态流走 `OnRtnOptionSelfClose`。 | 结算/自动对冲时段有禁用限制。 |
| ReqOptionSelfCloseAction | 期权自对冲 | OnRspOptionSelfCloseAction / OnRtnOptionSelfClose / OnErrRtnOptionSelfCloseAction | 返回 `requestId`；最终状态以 `OnRtnOptionSelfClose` 为准。 |  |
| ReqCombActionInsert | 组合 | OnRspCombActionInsert / OnRtnCombAction / OnErrRtnCombActionInsert | 返回 `requestId`；状态流走 `OnRtnCombAction`。 | 组合申请/拆分等。 |
| ReqOffsetSetting | 对销设置 | OnRspOffsetSetting / OnRtnOffsetSetting / OnErrRtnOffsetSetting | 返回 `requestId` 或按 `FinalOnly` 封装都可，但最终生效建议订阅 `OnRtnOffsetSetting`。 | 新接口。 |
| ReqCancelOffsetSetting | 对销设置 | OnRspCancelOffsetSetting / OnRtnOffsetSetting / OnErrRtnCancelOffsetSetting | 与 `ReqOffsetSetting` 对称处理。 | 新接口。 |
| ReqSpdApply | 展期确认 | OnRspSpdApply / OnRtnSpdApply / OnErrRtnSpdApply | 返回 `requestId`；状态流走 `OnRtnSpdApply`。 | 新接口。 |
| ReqSpdApplyAction | 展期确认 | OnRspSpdApplyAction / OnRtnSpdApply / OnErrRtnSpdApplyAction | 返回 `requestId`；最终状态看 `OnRtnSpdApply`。 | 新接口。 |
| ReqHedgeCfm | 套保确认 | OnRspHedgeCfm / OnRtnHedgeCfm / OnErrRtnHedgeCfm | 返回 `requestId`；状态流走 `OnRtnHedgeCfm`。 | 新接口。 |
| ReqHedgeCfmAction | 套保确认 | OnRspHedgeCfmAction / OnRtnHedgeCfm / OnErrRtnHedgeCfmAction | 返回 `requestId`；最终状态看 `OnRtnHedgeCfm`。 | 新接口。 |

## 查询类 API

| API | 类别 | 主要回调/回报 | 建议处理方式 | 备注 |
| --- | --- | --- | --- | --- |
| ReqQryOrder | 交易查询 | OnRspQryOrder | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryTrade | 交易查询 | OnRspQryTrade | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryInvestor | 基础查询 | OnRspQryInvestor | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryTradingCode | 基础查询 | OnRspQryTradingCode | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryUserSession | 基础查询 | OnRspQryUserSession | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryExchange | 基础查询 | OnRspQryExchange | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryProduct | 基础查询 | OnRspQryProduct | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryInstrument | 基础查询 | OnRspQryInstrument | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryDepthMarketData | 基础查询 | OnRspQryDepthMarketData | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryTraderOffer | 基础查询 | OnRspQryTraderOffer | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQrySettlementInfo | 基础查询 | OnRspQrySettlementInfo | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryTransferBank | 银期查询 | OnRspQryTransferBank | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryInvestorPositionDetail | 持仓查询 | OnRspQryInvestorPositionDetail | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryNotice | 基础查询 | OnRspQryNotice | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQrySettlementInfoConfirm | 基础查询 | OnRspQrySettlementInfoConfirm | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryInvestorPositionCombineDetail | 持仓查询 | OnRspQryInvestorPositionCombineDetail | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryCFMMCTradingAccountKey | 监管/银期 | OnRspQryCFMMCTradingAccountKey | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryEWarrantOffset | 持仓/仓单 | OnRspQryEWarrantOffset | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryInvestorProductGroupMargin | 保证金查询 | OnRspQryInvestorProductGroupMargin | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryExchangeMarginRateAdjust | 保证金查询 | OnRspQryExchangeMarginRateAdjust | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryExchangeRate | 基础查询 | OnRspQryExchangeRate | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQrySecAgentACIDMap | 资管/二级代理 | OnRspQrySecAgentACIDMap | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryProductExchRate | 基础查询 | OnRspQryProductExchRate | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryProductGroup | 基础查询 | OnRspQryProductGroup | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryMMInstrumentCommissionRate | 做市商查询 | OnRspQryMMInstrumentCommissionRate | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryMMOptionInstrCommRate | 做市商查询 | OnRspQryMMOptionInstrCommRate | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryInstrumentOrderCommRate | 手续费查询 | OnRspQryInstrumentOrderCommRate | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQrySecAgentTradingAccount | 资管/二级代理 | OnRspQrySecAgentTradingAccount | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQrySecAgentCheckMode | 资管/二级代理 | OnRspQrySecAgentCheckMode | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQrySecAgentTradeInfo | 资管/二级代理 | OnRspQrySecAgentTradeInfo | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryOptionInstrTradeCost | 期权查询 | OnRspQryOptionInstrTradeCost | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryOptionInstrCommRate | 期权查询 | OnRspQryOptionInstrCommRate | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryExecOrder | 期权查询 | OnRspQryExecOrder | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryForQuote | 询价查询 | OnRspQryForQuote | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryQuote | 报价查询 | OnRspQryQuote | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryOptionSelfClose | 期权查询 | OnRspQryOptionSelfClose | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryInvestUnit | 基础查询 | OnRspQryInvestUnit | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryCombInstrumentGuard | 组合查询 | OnRspQryCombInstrumentGuard | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryCombAction | 组合查询 | OnRspQryCombAction | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryTransferSerial | 银期查询 | OnRspQryTransferSerial | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryAccountregister | 银期查询 | OnRspQryAccountregister | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryContractBank | 银期查询 | OnRspQryContractBank | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryParkedOrder | 预埋单查询 | OnRspQryParkedOrder | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryParkedOrderAction | 预埋单查询 | OnRspQryParkedOrderAction | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryTradingNotice | 通知查询 | OnRspQryTradingNotice | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryBrokerTradingParams | 柜台参数查询 | OnRspQryBrokerTradingParams | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryBrokerTradingAlgos | 柜台参数查询 | OnRspQryBrokerTradingAlgos | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQueryCFMMCTradingAccountToken | 监管/银期 | OnRspQueryCFMMCTradingAccountToken | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryClassifiedInstrument | 基础查询 | OnRspQryClassifiedInstrument | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryCombPromotionParam | 组合查询 | OnRspQryCombPromotionParam | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryRiskSettleInvstPosition | 风控结算查询 | OnRspQryRiskSettleInvstPosition | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryRiskSettleProductStatus | 风控结算查询 | OnRspQryRiskSettleProductStatus | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQrySPBMFutureParameter | SPBM 查询 | OnRspQrySPBMFutureParameter | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQrySPBMOptionParameter | SPBM 查询 | OnRspQrySPBMOptionParameter | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQrySPBMIntraParameter | SPBM 查询 | OnRspQrySPBMIntraParameter | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQrySPBMInterParameter | SPBM 查询 | OnRspQrySPBMInterParameter | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQrySPBMPortfDefinition | SPBM 查询 | OnRspQrySPBMPortfDefinition | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQrySPBMInvestorPortfDef | SPBM 查询 | OnRspQrySPBMInvestorPortfDef | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryInvestorPortfMarginRatio | SPBM/组合保证金查询 | OnRspQryInvestorPortfMarginRatio | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryInvestorProdSPBMDetail | SPBM 查询 | OnRspQryInvestorProdSPBMDetail | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryInvestorCommoditySPMMMargin | SPMM 查询 | OnRspQryInvestorCommoditySPMMMargin | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryInvestorCommodityGroupSPMMMargin | SPMM 查询 | OnRspQryInvestorCommodityGroupSPMMMargin | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQrySPMMInstParam | SPMM 查询 | OnRspQrySPMMInstParam | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQrySPMMProductParam | SPMM 查询 | OnRspQrySPMMProductParam | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQrySPBMAddOnInterParameter | SPBM 查询 | OnRspQrySPBMAddOnInterParameter | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryRCAMSCombProductInfo | RCAMS 查询 | OnRspQryRCAMSCombProductInfo | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryRCAMSInstrParameter | RCAMS 查询 | OnRspQryRCAMSInstrParameter | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryRCAMSIntraParameter | RCAMS 查询 | OnRspQryRCAMSIntraParameter | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryRCAMSInterParameter | RCAMS 查询 | OnRspQryRCAMSInterParameter | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryRCAMSShortOptAdjustParam | RCAMS 查询 | OnRspQryRCAMSShortOptAdjustParam | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryRCAMSInvestorCombPosition | RCAMS 查询 | OnRspQryRCAMSInvestorCombPosition | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryInvestorProdRCAMSMargin | RCAMS 查询 | OnRspQryInvestorProdRCAMSMargin | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryRULEInstrParameter | RULE 查询 | OnRspQryRULEInstrParameter | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryRULEIntraParameter | RULE 查询 | OnRspQryRULEIntraParameter | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryRULEInterParameter | RULE 查询 | OnRspQryRULEInterParameter | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryInvestorProdRULEMargin | RULE 查询 | OnRspQryInvestorProdRULEMargin | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryInvestorPortfSetting | 组合保证金查询 | OnRspQryInvestorPortfSetting | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryInvestorInfoCommRec | 合规查询 | OnRspQryInvestorInfoCommRec | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryCombLeg | 组合查询 | OnRspQryCombLeg | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryOffsetSetting | 对销设置查询 | OnRspQryOffsetSetting | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQrySpdApply | 展期确认查询 | OnRspQrySpdApply | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |
| ReqQryHedgeCfm | 套保确认查询 | OnRspQryHedgeCfm | 按 `QueryAsync` + `PendingResponseCompletionPolicy.StreamUntilLast` 封装；累积多条响应直至 `bIsLast=true`。 | 若 `pRsp...` 为 null，也要允许空结果集正常完成。 |

## 银期转账主动请求

| API | 类别 | 主要回调/回报 | 建议处理方式 | 备注 |
| --- | --- | --- | --- | --- |
| ReqFromBankToFutureByFuture | 银期转账 | OnRspFromBankToFutureByFuture / OnRtnFromBankToFutureByFuture / OnErrRtnBankToFutureByFuture | 返回 `requestId`；`OnRsp*` 处理受理/同步错误，转账结果与后续状态依赖 `OnRtn*` / `OnErrRtn*`。 | 还要考虑撤销/冲正广播 `OnRtnRepeal*`。 |
| ReqFromFutureToBankByFuture | 银期转账 | OnRspFromFutureToBankByFuture / OnRtnFromFutureToBankByFuture / OnErrRtnFutureToBankByFuture | 与上面对称处理。 | 还要考虑撤销/冲正广播 `OnRtnRepeal*`。 |
| ReqQueryBankAccountMoneyByFuture | 银期查询 | OnRspQueryBankAccountMoneyByFuture / OnRtnQueryBankBalanceByFuture / OnErrRtnQueryBankBalanceByFuture | 请求相关结果可先按 `FinalOnly` 封装，但账户余额通知最好单独暴露事件。 | 查询银行余额通常伴随异步通知。 |

## 优先级建议

1. **高优先级**：`ReqQryInstrument` / `ReqQryClassifiedInstrument`、`ReqQrySettlementInfoConfirm` / `ReqQrySettlementInfo`、`ReqQryOrder` / `ReqQryTrade`、`ReqQryInvestorPositionDetail`、`ReqQryBrokerTradingParams`。这些接口能直接补齐交易前检查、基础行情/合约元数据、成交回溯与结算闭环。
2. **中优先级**：`ReqQryOptionInstrTradeCost`、`ReqQryOptionInstrCommRate`、`ReqExecOrder*`、`ReqOptionSelfClose*`、`ReqQryInvestorPositionCombineDetail`、`ReqQryCombLeg`。这些接口决定期权/组合合约能力是否完整。
3. **低优先级**：银期转账、二级代理、SPBM/SPMM/RCAMS/RULE、对销/展期/套保确认等新接口，除非项目准备显式支持对应柜台/风控能力。

## 实现层面的统一建议

- 先在 `NativeBridge/include/ctp_bridge.h` 与 `NativeBridge/src/trader_bridge.cpp` 补充 C ABI 与回调桥接，再同步到 `Ctp.Net/Bridge/TraderBridge.fs` 的 `DllImport`、native struct、mapper、`TraderCallbacks`。
- 单次应答类接口复用 `PendingResponseCompletionPolicy.FinalOnly`；查询类接口复用 `QueryAsync` + `StreamUntilLast`。
- 命令类接口不要把 `OnRsp*` 当最终状态；订单/报价/执行宣告/期权自对冲/组合/银期转账都要同时接住 `OnErrRtn*` 与 `OnRtn*`。
- 结合坑点文档，订单相关接口必须继续保留 `UserID`、`OrderSysID` 原样保存、`OnRtnOrder`/`OnRtnTrade` 广播语义，以及查询限流/错误 90/154 的重试策略。
