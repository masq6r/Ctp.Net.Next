namespace Ctp.Net.Bridge

open System
open Microsoft.Win32.SafeHandles
open Microsoft.FSharp.Reflection
open System.Runtime.InteropServices

type AuthenticateResponse =
    { BrokerId: string
      UserId: string
      UserProductInfo: string
      AppId: string
      AppType: char option }

type SettlementInfoConfirm =
    { BrokerId: string
      InvestorId: string
      ConfirmDate: string
      ConfirmTime: string
      SettlementId: int
      AccountId: string
      CurrencyId: string }

type TradingAccount =
    { BrokerId: string
      AccountId: string
      CurrencyId: string
      TradingDay: string
      Deposit: decimal
      Withdraw: decimal
      Balance: decimal
      Available: decimal
      CurrMargin: decimal
      FrozenMargin: decimal
      FrozenCash: decimal
      FrozenCommission: decimal
      Commission: decimal
      CloseProfit: decimal
      PositionProfit: decimal
      WithdrawQuota: decimal
      Reserve: decimal }

type InvestorPosition =
    { BrokerId: string
      InvestorId: string
      InstrumentId: string
      ExchangeId: string
      PosiDirection: char option
      HedgeFlag: char option
      PositionDate: char option
      YdPosition: int
      Position: int
      TodayPosition: int
      LongFrozen: int
      ShortFrozen: int
      OpenVolume: int
      CloseVolume: int
      PositionProfit: decimal
      CloseProfit: decimal
      UseMargin: decimal
      PositionCost: decimal
      OpenCost: decimal }

type InstrumentMarginRate =
    { InvestorRange: char option
      BrokerId: string
      InvestorId: string
      HedgeFlag: char option
      LongMarginRatioByMoney: decimal
      LongMarginRatioByVolume: decimal
      ShortMarginRatioByMoney: decimal
      ShortMarginRatioByVolume: decimal
      IsRelative: bool
      ExchangeId: string
      InvestUnitId: string
      InstrumentId: string }

type ExchangeMarginRate =
    { BrokerId: string
      HedgeFlag: char option
      LongMarginRatioByMoney: decimal
      LongMarginRatioByVolume: decimal
      ShortMarginRatioByMoney: decimal
      ShortMarginRatioByVolume: decimal
      ExchangeId: string
      InstrumentId: string }

type InstrumentCommissionRate =
    { InvestorRange: char option
      BrokerId: string
      InvestorId: string
      OpenRatioByMoney: decimal
      OpenRatioByVolume: decimal
      CloseRatioByMoney: decimal
      CloseRatioByVolume: decimal
      CloseTodayRatioByMoney: decimal
      CloseTodayRatioByVolume: decimal
      ExchangeId: string
      BizType: char option
      InvestUnitId: string
      InstrumentId: string }

type InputOrderRequest =
    { BrokerId: string
      InvestorId: string
      InstrumentId: string
      OrderRef: string
      UserId: string
      OrderPriceType: char
      Direction: char
      CombOffsetFlag: string
      CombHedgeFlag: string
      LimitPrice: decimal
      VolumeTotalOriginal: int
      TimeCondition: char
      GtdDate: string option
      VolumeCondition: char
      MinVolume: int
      ContingentCondition: char
      StopPrice: decimal
      ForceCloseReason: char
      IsAutoSuspend: bool
      BusinessUnit: string option
      UserForceClose: bool
      IsSwapOrder: bool
      ExchangeId: string option
      InvestUnitId: string option
      AccountId: string option
      CurrencyId: string option
      ClientId: string option
      MacAddress: string option
      IpAddress: string option
      OrderMemo: string option }

type InputOrderActionRequest =
    { BrokerId: string
      InvestorId: string
      OrderActionRef: int
      OrderRef: string
      FrontId: int
      SessionId: int
      ExchangeId: string option
      OrderSysId: string option
      ActionFlag: char
      LimitPrice: decimal
      VolumeChange: int
      UserId: string option
      InvestUnitId: string option
      InstrumentId: string option
      MacAddress: string option
      IpAddress: string option
      OrderMemo: string option }

type OrderUpdate =
    { BrokerId: string
      InvestorId: string
      InstrumentId: string
      ExchangeId: string
      OrderRef: string
      OrderSysId: string
      UserId: string
      OrderPriceType: char option
      Direction: char option
      CombOffsetFlag: string
      CombHedgeFlag: string
      LimitPrice: decimal
      VolumeTotalOriginal: int
      VolumeTraded: int
      VolumeTotal: int
      FrontId: int
      SessionId: int
      OrderStatus: char option
      OrderSubmitStatus: char option
      StatusMessage: string
      InsertDate: string
      InsertTime: string
      ActiveTime: string
      SuspendTime: string
      UpdateTime: string
      CancelTime: string }

type TradeUpdate =
    { BrokerId: string
      InvestorId: string
      InstrumentId: string
      ExchangeId: string
      OrderRef: string
      OrderSysId: string
      TradeId: string
      UserId: string
      Direction: char option
      OffsetFlag: char option
      HedgeFlag: char option
      Price: decimal
      Volume: int
      TradeDate: string
      TradeTime: string
      TradingDay: string }

type AuthenticateRequest =
    { BrokerId: string
      UserId: string
      UserProductInfo: string option
      AuthCode: string
      AppId: string }

type QueryTradingAccountRequest =
    { BrokerId: string
      InvestorId: string
      CurrencyId: string option
      BizType: char option
      AccountId: string option }

type QueryInvestorPositionRequest =
    { BrokerId: string
      InvestorId: string
      ExchangeId: string option
      InvestUnitId: string option
      InstrumentId: string option }

type QueryInstrumentMarginRateRequest =
    { BrokerId: string
      InvestorId: string
      HedgeFlag: char
      ExchangeId: string option
      InvestUnitId: string option
      InstrumentId: string }

type QueryExchangeMarginRateRequest =
    { BrokerId: string
      HedgeFlag: char
      ExchangeId: string option
      InstrumentId: string }

type QueryInstrumentCommissionRateRequest =
    { BrokerId: string
      InvestorId: string
      ExchangeId: string option
      InvestUnitId: string option
      InstrumentId: string }

type Accountregister =
    { 
      TradeDay: string;
      BankId: string;
      BankBranchId: string;
      BankAccount: string;
      BrokerId: string;
      BrokerBranchId: string;
      AccountId: string;
      IdCardType: char option;
      IdentifiedCardNo: string;
      CustomerName: string;
      CurrencyId: string;
      OpenOrDestroy: char option;
      RegDate: string;
      OutDate: string;
      TId: int;
      CustType: char option;
      BankAccType: char option;
      LongCustomerName: string }

type BatchOrderAction =
    { 
      BrokerId: string;
      InvestorId: string;
      OrderActionRef: int;
      RequestId: int;
      FrontId: int;
      SessionId: int;
      ExchangeId: string;
      ActionDate: string;
      ActionTime: string;
      TraderId: string;
      InstallId: int;
      ActionLocalId: string;
      ParticipantId: string;
      ClientId: string;
      BusinessUnit: string;
      OrderActionStatus: char option;
      UserId: string;
      StatusMsg: string;
      InvestUnitId: string;
      Reserve1: string;
      MacAddress: string;
      IpAddress: string }

type BrokerTradingAlgos =
    { 
      BrokerId: string;
      ExchangeId: string;
      Reserve1: string;
      HandlePositionAlgoId: char option;
      FindMarginRateAlgoId: char option;
      HandleTradingAccountAlgoId: char option;
      InstrumentId: string }

type BrokerTradingParams =
    { 
      BrokerId: string;
      InvestorId: string;
      MarginPriceType: char option;
      Algorithm: char option;
      AvailIncludeCloseProfit: char option;
      CurrencyId: string;
      OptionRoyaltyPriceType: char option;
      AccountId: string }

type CancelOffsetSetting =
    { 
      BrokerId: string;
      InvestorId: string;
      InstrumentId: string;
      UnderlyingInstrId: string;
      ProductId: string;
      OffsetType: char option;
      Volume: int;
      IsOffset: int;
      RequestId: int;
      UserId: string;
      ExchangeId: string;
      IpAddress: string;
      MacAddress: string;
      ExchangeInstId: string;
      ExchangeSerialNo: string;
      ExchangeProductId: string;
      TraderId: string;
      InstallId: int;
      ParticipantId: string;
      ClientId: string;
      OrderActionStatus: char option;
      StatusMsg: string;
      ActionLocalId: string;
      ActionDate: string;
      ActionTime: string }

type CfmmcTradingAccountKey =
    { 
      BrokerId: string;
      ParticipantId: string;
      AccountId: string;
      KeyId: int;
      CurrentKey: string }

type CombAction =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      CombActionRef: string;
      UserId: string;
      Direction: char option;
      Volume: int;
      CombDirection: char option;
      HedgeFlag: char option;
      ActionLocalId: string;
      ExchangeId: string;
      ParticipantId: string;
      ClientId: string;
      Reserve2: string;
      TraderId: string;
      InstallId: int;
      ActionStatus: char option;
      NotifySequence: int;
      TradingDay: string;
      SettlementId: int;
      SequenceNo: int;
      FrontId: int;
      SessionId: int;
      UserProductInfo: string;
      StatusMsg: string;
      Reserve3: string;
      MacAddress: string;
      ComTradeId: string;
      BranchId: string;
      InvestUnitId: string;
      InstrumentId: string;
      ExchangeInstId: string;
      IpAddress: string }

type CombInstrumentGuard =
    { 
      BrokerId: string;
      Reserve1: string;
      GuarantRatio: decimal;
      ExchangeId: string;
      InstrumentId: string }

type CombLeg =
    { 
      CombInstrumentId: string;
      LegId: int;
      LegInstrumentId: string;
      Direction: char option;
      LegMultiple: int;
      ImplyLevel: int }

type CombPromotionParam =
    { 
      ExchangeId: string;
      InstrumentId: string;
      CombHedgeFlag: string;
      Xparameter: decimal }

type ContractBank =
    { 
      BrokerId: string;
      BankId: string;
      BankBrchId: string;
      BankName: string;
      CsrcBankId: string }

type EWarrantOffset =
    { 
      TradingDay: string;
      BrokerId: string;
      InvestorId: string;
      ExchangeId: string;
      Reserve1: string;
      Direction: char option;
      HedgeFlag: char option;
      Volume: int;
      InvestUnitId: string;
      InstrumentId: string }

type Exchange =
    { 
      ExchangeId: string;
      ExchangeName: string;
      ExchangeProperty: char option }

type ExchangeMarginRateAdjust =
    { 
      BrokerId: string;
      Reserve1: string;
      HedgeFlag: char option;
      LongMarginRatioByMoney: decimal;
      LongMarginRatioByVolume: decimal;
      ShortMarginRatioByMoney: decimal;
      ShortMarginRatioByVolume: decimal;
      ExchLongMarginRatioByMoney: decimal;
      ExchLongMarginRatioByVolume: decimal;
      ExchShortMarginRatioByMoney: decimal;
      ExchShortMarginRatioByVolume: decimal;
      NoLongMarginRatioByMoney: decimal;
      NoLongMarginRatioByVolume: decimal;
      NoShortMarginRatioByMoney: decimal;
      NoShortMarginRatioByVolume: decimal;
      InstrumentId: string }

type ExchangeRate =
    { 
      BrokerId: string;
      FromCurrencyId: string;
      FromCurrencyUnit: decimal;
      ToCurrencyId: string;
      ExchangeRate: decimal }

type ExecOrder =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      ExecOrderRef: string;
      UserId: string;
      Volume: int;
      RequestId: int;
      BusinessUnit: string;
      OffsetFlag: char option;
      HedgeFlag: char option;
      ActionType: char option;
      PosiDirection: char option;
      ReservePositionFlag: char option;
      CloseFlag: char option;
      ExecOrderLocalId: string;
      ExchangeId: string;
      ParticipantId: string;
      ClientId: string;
      Reserve2: string;
      TraderId: string;
      InstallId: int;
      OrderSubmitStatus: char option;
      NotifySequence: int;
      TradingDay: string;
      SettlementId: int;
      ExecOrderSysId: string;
      InsertDate: string;
      InsertTime: string;
      CancelTime: string;
      ExecResult: char option;
      ClearingPartId: string;
      SequenceNo: int;
      FrontId: int;
      SessionId: int;
      UserProductInfo: string;
      StatusMsg: string;
      ActiveUserId: string;
      BrokerExecOrderSeq: int;
      BranchId: string;
      InvestUnitId: string;
      AccountId: string;
      CurrencyId: string;
      Reserve3: string;
      MacAddress: string;
      InstrumentId: string;
      ExchangeInstId: string;
      IpAddress: string }

type ExecOrderAction =
    { 
      BrokerId: string;
      InvestorId: string;
      ExecOrderActionRef: int;
      ExecOrderRef: string;
      RequestId: int;
      FrontId: int;
      SessionId: int;
      ExchangeId: string;
      ExecOrderSysId: string;
      ActionFlag: char option;
      ActionDate: string;
      ActionTime: string;
      TraderId: string;
      InstallId: int;
      ExecOrderLocalId: string;
      ActionLocalId: string;
      ParticipantId: string;
      ClientId: string;
      BusinessUnit: string;
      OrderActionStatus: char option;
      UserId: string;
      ActionType: char option;
      StatusMsg: string;
      Reserve1: string;
      BranchId: string;
      InvestUnitId: string;
      Reserve2: string;
      MacAddress: string;
      InstrumentId: string;
      IpAddress: string }

type FensUserInfo =
    { 
      BrokerId: string;
      UserId: string;
      LoginMode: char option }

type ForQuote =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      ForQuoteRef: string;
      UserId: string;
      ForQuoteLocalId: string;
      ExchangeId: string;
      ParticipantId: string;
      ClientId: string;
      Reserve2: string;
      TraderId: string;
      InstallId: int;
      InsertDate: string;
      InsertTime: string;
      ForQuoteStatus: char option;
      FrontId: int;
      SessionId: int;
      StatusMsg: string;
      ActiveUserId: string;
      BrokerForQutoSeq: int;
      InvestUnitId: string;
      Reserve3: string;
      MacAddress: string;
      InstrumentId: string;
      ExchangeInstId: string;
      IpAddress: string }

type ForQuoteRsp =
    { 
      TradingDay: string;
      Reserve1: string;
      ForQuoteSysId: string;
      ForQuoteTime: string;
      ActionDay: string;
      ExchangeId: string;
      InstrumentId: string }

type FrontInfo =
    { 
      FrontAddr: string;
      QryFreq: int;
      FtdPkgFreq: int }

type HedgeCfm =
    { 
      BrokerId: string;
      InvestorId: string;
      InstrumentId: string;
      UserId: string;
      Volume: int;
      Direction: char option;
      RequestId: int;
      FrontId: int;
      SessionId: int;
      OrderRef: string;
      ActiveUserId: string;
      BrokerOrderSeq: int;
      OrderSysId: string;
      ApplyStatus: char option;
      SequenceNo: int;
      DealVolume: int;
      InsertDate: string;
      InsertTime: string;
      CancelTime: string;
      ReqDate: string;
      OrderLocalId: string;
      ExchangeId: string;
      ParticipantId: string;
      ClientId: string;
      ExchangeInstId: string;
      TraderId: string;
      InstallId: int;
      OrderSubmitStatus: char option;
      NotifySequence: int;
      TradingDay: string;
      SettlementId: int;
      StatusMsg: string;
      IpAddress: string;
      MacAddress: string }

type HedgeCfmAction =
    { 
      BrokerId: string;
      InvestorId: string;
      ActionDate: string;
      ActionTime: string;
      TraderId: string;
      InstallId: int;
      OrderLocalId: string;
      ActionLocalId: string;
      ParticipantId: string;
      ClientId: string;
      OrderActionStatus: char option;
      UserId: string;
      ExchangeId: string;
      OrderSysId: string;
      RequestId: int;
      StatusMsg: string;
      OrderRef: string;
      FrontId: int;
      SessionId: int;
      IpAddress: string;
      MacAddress: string }

type InputBatchOrderActionRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      OrderActionRef: int;
      RequestId: int;
      FrontId: int;
      SessionId: int;
      ExchangeId: string;
      UserId: string;
      InvestUnitId: string;
      Reserve1: string;
      MacAddress: string;
      IpAddress: string }

type InputCombActionRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      CombActionRef: string;
      UserId: string;
      Direction: char option;
      Volume: int;
      CombDirection: char option;
      HedgeFlag: char option;
      ExchangeId: string;
      Reserve2: string;
      MacAddress: string;
      InvestUnitId: string;
      FrontId: int;
      SessionId: int;
      InstrumentId: string;
      IpAddress: string }

type InputExecOrderRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      ExecOrderRef: string;
      UserId: string;
      Volume: int;
      RequestId: int;
      BusinessUnit: string;
      OffsetFlag: char option;
      HedgeFlag: char option;
      ActionType: char option;
      PosiDirection: char option;
      ReservePositionFlag: char option;
      CloseFlag: char option;
      ExchangeId: string;
      InvestUnitId: string;
      AccountId: string;
      CurrencyId: string;
      ClientId: string;
      Reserve2: string;
      MacAddress: string;
      InstrumentId: string;
      IpAddress: string }

type InputExecOrderActionRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      ExecOrderActionRef: int;
      ExecOrderRef: string;
      RequestId: int;
      FrontId: int;
      SessionId: int;
      ExchangeId: string;
      ExecOrderSysId: string;
      ActionFlag: char option;
      UserId: string;
      Reserve1: string;
      InvestUnitId: string;
      Reserve2: string;
      MacAddress: string;
      InstrumentId: string;
      IpAddress: string }

type InputForQuoteRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      ForQuoteRef: string;
      UserId: string;
      ExchangeId: string;
      InvestUnitId: string;
      Reserve2: string;
      MacAddress: string;
      InstrumentId: string;
      IpAddress: string }

type InputHedgeCfmRequest =
    { 
      BrokerId: string;
      UserId: string;
      InvestorId: string;
      ExchangeId: string;
      InstrumentId: string;
      Volume: int;
      Direction: char option;
      RequestId: int;
      OrderRef: string;
      IpAddress: string;
      MacAddress: string }

type InputHedgeCfmActionRequest =
    { 
      BrokerId: string;
      UserId: string;
      InvestorId: string;
      ExchangeId: string;
      OrderSysId: string;
      OrderRef: string;
      FrontId: int;
      SessionId: int;
      RequestId: int;
      IpAddress: string;
      MacAddress: string }

type InputOffsetSettingRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      InstrumentId: string;
      UnderlyingInstrId: string;
      ProductId: string;
      OffsetType: char option;
      Volume: int;
      IsOffset: int;
      RequestId: int;
      UserId: string;
      ExchangeId: string;
      IpAddress: string;
      MacAddress: string }

type InputOptionSelfCloseRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      OptionSelfCloseRef: string;
      UserId: string;
      Volume: int;
      RequestId: int;
      BusinessUnit: string;
      HedgeFlag: char option;
      OptSelfCloseFlag: char option;
      ExchangeId: string;
      InvestUnitId: string;
      AccountId: string;
      CurrencyId: string;
      ClientId: string;
      Reserve2: string;
      MacAddress: string;
      InstrumentId: string;
      IpAddress: string }

type InputOptionSelfCloseActionRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      OptionSelfCloseActionRef: int;
      OptionSelfCloseRef: string;
      RequestId: int;
      FrontId: int;
      SessionId: int;
      ExchangeId: string;
      OptionSelfCloseSysId: string;
      ActionFlag: char option;
      UserId: string;
      Reserve1: string;
      InvestUnitId: string;
      Reserve2: string;
      MacAddress: string;
      InstrumentId: string;
      IpAddress: string }

type InputQuoteRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      QuoteRef: string;
      UserId: string;
      AskPrice: decimal;
      BidPrice: decimal;
      AskVolume: int;
      BidVolume: int;
      RequestId: int;
      BusinessUnit: string;
      AskOffsetFlag: char option;
      BidOffsetFlag: char option;
      AskHedgeFlag: char option;
      BidHedgeFlag: char option;
      AskOrderRef: string;
      BidOrderRef: string;
      ForQuoteSysId: string;
      ExchangeId: string;
      InvestUnitId: string;
      ClientId: string;
      Reserve2: string;
      MacAddress: string;
      InstrumentId: string;
      IpAddress: string;
      ReplaceSysId: string;
      TimeCondition: char option;
      OrderMemo: string;
      SessionReqSeq: int }

type InputQuoteActionRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      QuoteActionRef: int;
      QuoteRef: string;
      RequestId: int;
      FrontId: int;
      SessionId: int;
      ExchangeId: string;
      QuoteSysId: string;
      ActionFlag: char option;
      UserId: string;
      Reserve1: string;
      InvestUnitId: string;
      ClientId: string;
      Reserve2: string;
      MacAddress: string;
      InstrumentId: string;
      IpAddress: string;
      OrderMemo: string;
      SessionReqSeq: int }

type InputSpdApplyRequest =
    { 
      BrokerId: string;
      UserId: string;
      InvestorId: string;
      ExchangeId: string;
      FirstLegInstrumentId: string;
      SecondLegInstrumentId: string;
      Volume: int;
      Direction: char option;
      CmbType: char option;
      RequestId: int;
      OrderRef: string;
      IpAddress: string;
      MacAddress: string }

type InputSpdApplyActionRequest =
    { 
      BrokerId: string;
      UserId: string;
      InvestorId: string;
      ExchangeId: string;
      OrderSysId: string;
      OrderRef: string;
      FrontId: int;
      SessionId: int;
      RequestId: int;
      IpAddress: string;
      MacAddress: string }

type Instrument =
    { 
      Reserve1: string;
      ExchangeId: string;
      InstrumentName: string;
      Reserve2: string;
      Reserve3: string;
      ProductClass: char option;
      DeliveryYear: int;
      DeliveryMonth: int;
      MaxMarketOrderVolume: int;
      MinMarketOrderVolume: int;
      MaxLimitOrderVolume: int;
      MinLimitOrderVolume: int;
      VolumeMultiple: int;
      PriceTick: decimal;
      CreateDate: string;
      OpenDate: string;
      ExpireDate: string;
      StartDelivDate: string;
      EndDelivDate: string;
      InstLifePhase: char option;
      IsTrading: int;
      PositionType: char option;
      PositionDateType: char option;
      LongMarginRatio: decimal;
      ShortMarginRatio: decimal;
      MaxMarginSideAlgorithm: char option;
      Reserve4: string;
      StrikePrice: decimal;
      OptionsType: char option;
      UnderlyingMultiple: decimal;
      CombinationType: char option;
      InstrumentId: string;
      ExchangeInstId: string;
      ProductId: string;
      UnderlyingInstrId: string }

type InstrumentOrderCommRate =
    { 
      Reserve1: string;
      InvestorRange: char option;
      BrokerId: string;
      InvestorId: string;
      HedgeFlag: char option;
      OrderCommByVolume: decimal;
      OrderActionCommByVolume: decimal;
      ExchangeId: string;
      InvestUnitId: string;
      InstrumentId: string;
      OrderCommByTrade: decimal;
      OrderActionCommByTrade: decimal }

type InvestUnit =
    { 
      BrokerId: string;
      InvestorId: string;
      InvestUnitId: string;
      InvestorUnitName: string;
      InvestorGroupId: string;
      CommModelId: string;
      MarginModelId: string;
      AccountId: string;
      CurrencyId: string }

type Investor =
    { 
      InvestorId: string;
      BrokerId: string;
      InvestorGroupId: string;
      InvestorName: string;
      IdentifiedCardType: char option;
      IdentifiedCardNo: string;
      IsActive: int;
      Telephone: string;
      Address: string;
      OpenDate: string;
      Mobile: string;
      CommModelId: string;
      MarginModelId: string;
      IsOrderFreq: char option;
      IsOpenVolLimit: char option }

type InvestorCommodityGroupSpmmMargin =
    { 
      ExchangeId: string;
      BrokerId: string;
      InvestorId: string;
      CommodityGroupId: string;
      MarginBeforeDiscount: decimal;
      MarginNoDiscount: decimal;
      LongRisk: decimal;
      ShortRisk: decimal;
      CloseFrozenMargin: decimal;
      InterCommodityRate: decimal;
      MiniMarginRatio: decimal;
      AdjustRatio: decimal;
      IntraCommodityDiscount: decimal;
      InterCommodityDiscount: decimal;
      ExchMargin: decimal;
      InvestorMargin: decimal;
      FrozenCommission: decimal;
      Commission: decimal;
      FrozenCash: decimal;
      CashIn: decimal;
      StrikeFrozenMargin: decimal }

type InvestorCommoditySpmmMargin =
    { 
      ExchangeId: string;
      BrokerId: string;
      InvestorId: string;
      CommodityId: string;
      MarginBeforeDiscount: decimal;
      MarginNoDiscount: decimal;
      LongPosRisk: decimal;
      LongOpenFrozenRisk: decimal;
      LongCloseFrozenRisk: decimal;
      ShortPosRisk: decimal;
      ShortOpenFrozenRisk: decimal;
      ShortCloseFrozenRisk: decimal;
      IntraCommodityRate: decimal;
      OptionDiscountRate: decimal;
      PosDiscount: decimal;
      OpenFrozenDiscount: decimal;
      NetRisk: decimal;
      CloseFrozenMargin: decimal;
      FrozenCommission: decimal;
      Commission: decimal;
      FrozenCash: decimal;
      CashIn: decimal;
      StrikeFrozenMargin: decimal }

type InvestorInfoCommRec =
    { 
      ExchangeId: string;
      BrokerId: string;
      InvestorId: string;
      InstrumentId: string;
      OrderCount: int;
      OrderActionCount: int;
      ForQuoteCnt: int;
      InfoComm: decimal;
      IsOptSeries: int;
      ProductId: string;
      InfoCnt: int }

type InvestorPortfMarginRatio =
    { 
      InvestorRange: char option;
      BrokerId: string;
      InvestorId: string;
      ExchangeId: string;
      MarginRatio: decimal;
      ProductGroupId: string }

type InvestorPortfSetting =
    { 
      ExchangeId: string;
      BrokerId: string;
      InvestorId: string;
      HedgeFlag: char option;
      UsePortf: int }

type InvestorPositionCombineDetail =
    { 
      TradingDay: string;
      OpenDate: string;
      ExchangeId: string;
      SettlementId: int;
      BrokerId: string;
      InvestorId: string;
      ComTradeId: string;
      TradeId: string;
      Reserve1: string;
      HedgeFlag: char option;
      Direction: char option;
      TotalAmt: int;
      Margin: decimal;
      ExchMargin: decimal;
      MarginRateByMoney: decimal;
      MarginRateByVolume: decimal;
      LegId: int;
      LegMultiple: int;
      Reserve2: string;
      TradeGroupId: int;
      InvestUnitId: string;
      InstrumentId: string;
      CombInstrumentId: string }

type InvestorPositionDetail =
    { 
      Reserve1: string;
      BrokerId: string;
      InvestorId: string;
      HedgeFlag: char option;
      Direction: char option;
      OpenDate: string;
      TradeId: string;
      Volume: int;
      OpenPrice: decimal;
      TradingDay: string;
      SettlementId: int;
      TradeType: char option;
      Reserve2: string;
      ExchangeId: string;
      CloseProfitByDate: decimal;
      CloseProfitByTrade: decimal;
      PositionProfitByDate: decimal;
      PositionProfitByTrade: decimal;
      Margin: decimal;
      ExchMargin: decimal;
      MarginRateByMoney: decimal;
      MarginRateByVolume: decimal;
      LastSettlementPrice: decimal;
      SettlementPrice: decimal;
      CloseVolume: int;
      CloseAmount: decimal;
      TimeFirstVolume: int;
      InvestUnitId: string;
      SpecPosiType: char option;
      InstrumentId: string;
      CombInstrumentId: string }

type InvestorProdRcamsMargin =
    { 
      ExchangeId: string;
      BrokerId: string;
      InvestorId: string;
      CombProductId: string;
      HedgeFlag: char option;
      ProductGroupId: string;
      RiskBeforeDiscount: decimal;
      IntraInstrRisk: decimal;
      BPosRisk: decimal;
      SPosRisk: decimal;
      IntraProdRisk: decimal;
      NetRisk: decimal;
      InterProdRisk: decimal;
      ShortOptRiskAdj: decimal;
      OptionRoyalty: decimal;
      MmsaCloseFrozenMargin: decimal;
      CloseCombFrozenMargin: decimal;
      CloseFrozenMargin: decimal;
      MmsaOpenFrozenMargin: decimal;
      DeliveryOpenFrozenMargin: decimal;
      OpenFrozenMargin: decimal;
      UseFrozenMargin: decimal;
      MmsaExchMargin: decimal;
      DeliveryExchMargin: decimal;
      CombExchMargin: decimal;
      ExchMargin: decimal;
      UseMargin: decimal }

type InvestorProdRuleMargin =
    { 
      ExchangeId: string;
      BrokerId: string;
      InvestorId: string;
      ProdFamilyCode: string;
      InstrumentClass: char option;
      CommodityGroupId: int;
      BStdPosition: decimal;
      SStdPosition: decimal;
      BStdOpenFrozen: decimal;
      SStdOpenFrozen: decimal;
      BStdCloseFrozen: decimal;
      SStdCloseFrozen: decimal;
      IntraProdStdPosition: decimal;
      NetStdPosition: decimal;
      InterProdStdPosition: decimal;
      SingleStdPosition: decimal;
      IntraProdMargin: decimal;
      InterProdMargin: decimal;
      SingleMargin: decimal;
      NonCombMargin: decimal;
      AddOnMargin: decimal;
      ExchMargin: decimal;
      AddOnFrozenMargin: decimal;
      OpenFrozenMargin: decimal;
      CloseFrozenMargin: decimal;
      Margin: decimal;
      FrozenMargin: decimal }

type InvestorProdSpbmDetail =
    { 
      ExchangeId: string;
      BrokerId: string;
      InvestorId: string;
      ProdFamilyCode: string;
      IntraInstrMargin: decimal;
      BCollectingMargin: decimal;
      SCollectingMargin: decimal;
      IntraProdMargin: decimal;
      NetMargin: decimal;
      InterProdMargin: decimal;
      SingleMargin: decimal;
      AddOnMargin: decimal;
      DeliveryMargin: decimal;
      CallOptionMinRisk: decimal;
      PutOptionMinRisk: decimal;
      OptionMinRisk: decimal;
      OptionValueOffset: decimal;
      OptionRoyalty: decimal;
      RealOptionValueOffset: decimal;
      Margin: decimal;
      ExchMargin: decimal }

type InvestorProductGroupMargin =
    { 
      Reserve1: string;
      BrokerId: string;
      InvestorId: string;
      TradingDay: string;
      SettlementId: int;
      FrozenMargin: decimal;
      LongFrozenMargin: decimal;
      ShortFrozenMargin: decimal;
      UseMargin: decimal;
      LongUseMargin: decimal;
      ShortUseMargin: decimal;
      ExchMargin: decimal;
      LongExchMargin: decimal;
      ShortExchMargin: decimal;
      CloseProfit: decimal;
      FrozenCommission: decimal;
      Commission: decimal;
      FrozenCash: decimal;
      CashIn: decimal;
      PositionProfit: decimal;
      OffsetAmount: decimal;
      LongOffsetAmount: decimal;
      ShortOffsetAmount: decimal;
      ExchOffsetAmount: decimal;
      LongExchOffsetAmount: decimal;
      ShortExchOffsetAmount: decimal;
      HedgeFlag: char option;
      ExchangeId: string;
      InvestUnitId: string;
      ProductGroupId: string }

type MmInstrumentCommissionRate =
    { 
      Reserve1: string;
      InvestorRange: char option;
      BrokerId: string;
      InvestorId: string;
      OpenRatioByMoney: decimal;
      OpenRatioByVolume: decimal;
      CloseRatioByMoney: decimal;
      CloseRatioByVolume: decimal;
      CloseTodayRatioByMoney: decimal;
      CloseTodayRatioByVolume: decimal;
      InstrumentId: string }

type MmOptionInstrCommRate =
    { 
      Reserve1: string;
      InvestorRange: char option;
      BrokerId: string;
      InvestorId: string;
      OpenRatioByMoney: decimal;
      OpenRatioByVolume: decimal;
      CloseRatioByMoney: decimal;
      CloseRatioByVolume: decimal;
      CloseTodayRatioByMoney: decimal;
      CloseTodayRatioByVolume: decimal;
      StrikeRatioByMoney: decimal;
      StrikeRatioByVolume: decimal;
      InstrumentId: string }

type Notice =
    { 
      BrokerId: string;
      Content: string;
      SequenceLabel: string }

type NotifyQueryAccount =
    { 
      TradeCode: string;
      BankId: string;
      BankBranchId: string;
      BrokerId: string;
      BrokerBranchId: string;
      TradeDate: string;
      TradeTime: string;
      BankSerial: string;
      TradingDay: string;
      PlateSerial: int;
      LastFragment: char option;
      SessionId: int;
      CustomerName: string;
      IdCardType: char option;
      IdentifiedCardNo: string;
      CustType: char option;
      BankAccount: string;
      BankPassWord: string;
      AccountId: string;
      Password: string;
      FutureSerial: int;
      InstallId: int;
      UserId: string;
      VerifyCertNoFlag: char option;
      CurrencyId: string;
      Digest: string;
      BankAccType: char option;
      DeviceId: string;
      BankSecuAccType: char option;
      BrokerIdByBank: string;
      BankSecuAcc: string;
      BankPwdFlag: char option;
      SecuPwdFlag: char option;
      OperNo: string;
      RequestId: int;
      TId: int;
      BankUseAmount: decimal;
      BankFetchAmount: decimal;
      ErrorId: int;
      ErrorMsg: string;
      LongCustomerName: string }

type OffsetSetting =
    { 
      BrokerId: string;
      InvestorId: string;
      InstrumentId: string;
      UnderlyingInstrId: string;
      ProductId: string;
      OffsetType: char option;
      Volume: int;
      IsOffset: int;
      RequestId: int;
      UserId: string;
      ExchangeId: string;
      IpAddress: string;
      MacAddress: string;
      ExchangeInstId: string;
      ExchangeSerialNo: string;
      ExchangeProductId: string;
      ParticipantId: string;
      ClientId: string;
      TraderId: string;
      InstallId: int;
      OrderSubmitStatus: char option;
      TradingDay: string;
      SettlementId: int;
      InsertDate: string;
      InsertTime: string;
      CancelTime: string;
      ExecResult: char option;
      SequenceNo: int;
      FrontId: int;
      SessionId: int;
      StatusMsg: string;
      ActiveUserId: string;
      BrokerOffsetSettingSeq: int;
      ApplySrc: char option }

type OptionInstrCommRate =
    { 
      Reserve1: string;
      InvestorRange: char option;
      BrokerId: string;
      InvestorId: string;
      OpenRatioByMoney: decimal;
      OpenRatioByVolume: decimal;
      CloseRatioByMoney: decimal;
      CloseRatioByVolume: decimal;
      CloseTodayRatioByMoney: decimal;
      CloseTodayRatioByVolume: decimal;
      StrikeRatioByMoney: decimal;
      StrikeRatioByVolume: decimal;
      ExchangeId: string;
      InvestUnitId: string;
      InstrumentId: string }

type OptionInstrTradeCost =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      HedgeFlag: char option;
      FixedMargin: decimal;
      MiniMargin: decimal;
      Royalty: decimal;
      ExchFixedMargin: decimal;
      ExchMiniMargin: decimal;
      ExchangeId: string;
      InvestUnitId: string;
      InstrumentId: string }

type OptionSelfClose =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      OptionSelfCloseRef: string;
      UserId: string;
      Volume: int;
      RequestId: int;
      BusinessUnit: string;
      HedgeFlag: char option;
      OptSelfCloseFlag: char option;
      OptionSelfCloseLocalId: string;
      ExchangeId: string;
      ParticipantId: string;
      ClientId: string;
      Reserve2: string;
      TraderId: string;
      InstallId: int;
      OrderSubmitStatus: char option;
      NotifySequence: int;
      TradingDay: string;
      SettlementId: int;
      OptionSelfCloseSysId: string;
      InsertDate: string;
      InsertTime: string;
      CancelTime: string;
      ExecResult: char option;
      ClearingPartId: string;
      SequenceNo: int;
      FrontId: int;
      SessionId: int;
      UserProductInfo: string;
      StatusMsg: string;
      ActiveUserId: string;
      BrokerOptionSelfCloseSeq: int;
      BranchId: string;
      InvestUnitId: string;
      AccountId: string;
      CurrencyId: string;
      Reserve3: string;
      MacAddress: string;
      InstrumentId: string;
      ExchangeInstId: string;
      IpAddress: string }

type OptionSelfCloseAction =
    { 
      BrokerId: string;
      InvestorId: string;
      OptionSelfCloseActionRef: int;
      OptionSelfCloseRef: string;
      RequestId: int;
      FrontId: int;
      SessionId: int;
      ExchangeId: string;
      OptionSelfCloseSysId: string;
      ActionFlag: char option;
      ActionDate: string;
      ActionTime: string;
      TraderId: string;
      InstallId: int;
      OptionSelfCloseLocalId: string;
      ActionLocalId: string;
      ParticipantId: string;
      ClientId: string;
      BusinessUnit: string;
      OrderActionStatus: char option;
      UserId: string;
      StatusMsg: string;
      Reserve1: string;
      BranchId: string;
      InvestUnitId: string;
      Reserve2: string;
      MacAddress: string;
      InstrumentId: string;
      IpAddress: string }

type OrderAction =
    { 
      BrokerId: string;
      InvestorId: string;
      OrderActionRef: int;
      OrderRef: string;
      RequestId: int;
      FrontId: int;
      SessionId: int;
      ExchangeId: string;
      OrderSysId: string;
      ActionFlag: char option;
      LimitPrice: decimal;
      VolumeChange: int;
      ActionDate: string;
      ActionTime: string;
      TraderId: string;
      InstallId: int;
      OrderLocalId: string;
      ActionLocalId: string;
      ParticipantId: string;
      ClientId: string;
      BusinessUnit: string;
      OrderActionStatus: char option;
      UserId: string;
      StatusMsg: string;
      Reserve1: string;
      BranchId: string;
      InvestUnitId: string;
      Reserve2: string;
      MacAddress: string;
      InstrumentId: string;
      IpAddress: string;
      OrderMemo: string;
      SessionReqSeq: int }

type ParkedOrder =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      OrderRef: string;
      UserId: string;
      OrderPriceType: char option;
      Direction: char option;
      CombOffsetFlag: string;
      CombHedgeFlag: string;
      LimitPrice: decimal;
      VolumeTotalOriginal: int;
      TimeCondition: char option;
      GtdDate: string;
      VolumeCondition: char option;
      MinVolume: int;
      ContingentCondition: char option;
      StopPrice: decimal;
      ForceCloseReason: char option;
      IsAutoSuspend: int;
      BusinessUnit: string;
      RequestId: int;
      UserForceClose: int;
      ExchangeId: string;
      ParkedOrderId: string;
      UserType: char option;
      Status: char option;
      ErrorId: int;
      ErrorMsg: string;
      IsSwapOrder: int;
      AccountId: string;
      CurrencyId: string;
      ClientId: string;
      InvestUnitId: string;
      Reserve2: string;
      MacAddress: string;
      InstrumentId: string;
      IpAddress: string }

type ParkedOrderAction =
    { 
      BrokerId: string;
      InvestorId: string;
      OrderActionRef: int;
      OrderRef: string;
      RequestId: int;
      FrontId: int;
      SessionId: int;
      ExchangeId: string;
      OrderSysId: string;
      ActionFlag: char option;
      LimitPrice: decimal;
      VolumeChange: int;
      UserId: string;
      Reserve1: string;
      ParkedOrderActionId: string;
      UserType: char option;
      Status: char option;
      ErrorId: int;
      ErrorMsg: string;
      InvestUnitId: string;
      Reserve2: string;
      MacAddress: string;
      InstrumentId: string;
      IpAddress: string }

type Product =
    { 
      Reserve1: string;
      ProductName: string;
      ExchangeId: string;
      ProductClass: char option;
      VolumeMultiple: int;
      PriceTick: decimal;
      MaxMarketOrderVolume: int;
      MinMarketOrderVolume: int;
      MaxLimitOrderVolume: int;
      MinLimitOrderVolume: int;
      PositionType: char option;
      PositionDateType: char option;
      CloseDealType: char option;
      TradeCurrencyId: string;
      MortgageFundUseRange: char option;
      Reserve2: string;
      UnderlyingMultiple: decimal;
      ProductId: string;
      ExchangeProductId: string;
      OpenLimitControlLevel: char option;
      OrderFreqControlLevel: char option }

type ProductExchRate =
    { 
      Reserve1: string;
      QuoteCurrencyId: string;
      ExchangeRate: decimal;
      ExchangeId: string;
      ProductId: string }

type ProductGroup =
    { 
      Reserve1: string;
      ExchangeId: string;
      Reserve2: string;
      ProductId: string;
      ProductGroupId: string }

type QryAccountregisterRequest =
    { 
      BrokerId: string;
      AccountId: string;
      BankId: string;
      BankBranchId: string;
      CurrencyId: string }

type QryBrokerTradingAlgosRequest =
    { 
      BrokerId: string;
      ExchangeId: string;
      Reserve1: string;
      InstrumentId: string }

type QryBrokerTradingParamsRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      CurrencyId: string;
      AccountId: string }

type QryCfmmcTradingAccountKeyRequest =
    { 
      BrokerId: string;
      InvestorId: string }

type QryClassifiedInstrumentRequest =
    { 
      InstrumentId: string;
      ExchangeId: string;
      ExchangeInstId: string;
      ProductId: string;
      TradingType: char option;
      ClassType: char option }

type QryCombActionRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      ExchangeId: string;
      InvestUnitId: string;
      InstrumentId: string }

type QryCombInstrumentGuardRequest =
    { 
      BrokerId: string;
      Reserve1: string;
      ExchangeId: string;
      InstrumentId: string }

type QryCombLeg =
    { 
      LegInstrumentId: string }

type QryCombPromotionParamRequest =
    { 
      ExchangeId: string;
      InstrumentId: string }

type QryContractBankRequest =
    { 
      BrokerId: string;
      BankId: string;
      BankBrchId: string }

type QryDepthMarketDataRequest =
    { 
      Reserve1: string;
      ExchangeId: string;
      InstrumentId: string;
      ProductClass: char option }

type QryEWarrantOffsetRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      ExchangeId: string;
      Reserve1: string;
      InvestUnitId: string;
      InstrumentId: string }

type QryExchangeRequest =
    { 
      ExchangeId: string }

type QryExchangeMarginRateAdjustRequest =
    { 
      BrokerId: string;
      Reserve1: string;
      HedgeFlag: char option;
      InstrumentId: string }

type QryExchangeRate =
    { 
      BrokerId: string;
      FromCurrencyId: string;
      ToCurrencyId: string }

type QryExecOrderRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      ExchangeId: string;
      ExecOrderSysId: string;
      InsertTimeStart: string;
      InsertTimeEnd: string;
      InstrumentId: string }

type QryForQuoteRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      ExchangeId: string;
      InsertTimeStart: string;
      InsertTimeEnd: string;
      InvestUnitId: string;
      InstrumentId: string }

type QryHedgeCfmRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      ExchangeId: string;
      OrderSysId: string;
      InstrumentId: string }

type QryInstrumentRequest =
    { 
      Reserve1: string;
      ExchangeId: string;
      Reserve2: string;
      Reserve3: string;
      InstrumentId: string;
      ExchangeInstId: string;
      ProductId: string }

type QryInstrumentOrderCommRateRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      InstrumentId: string }

type QryInvestUnitRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      InvestUnitId: string }

type QryInvestorRequest =
    { 
      BrokerId: string;
      InvestorId: string }

type QryInvestorCommodityGroupSpmmMarginRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      CommodityGroupId: string }

type QryInvestorCommoditySpmmMarginRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      CommodityId: string }

type QryInvestorInfoCommRecRequest =
    { 
      InvestorId: string;
      InstrumentId: string;
      BrokerId: string }

type QryInvestorPortfMarginRatioRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      ExchangeId: string;
      ProductGroupId: string }

type QryInvestorPortfSettingRequest =
    { 
      ExchangeId: string;
      BrokerId: string;
      InvestorId: string }

type QryInvestorPositionCombineDetailRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      ExchangeId: string;
      InvestUnitId: string;
      CombInstrumentId: string }

type QryInvestorPositionDetailRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      ExchangeId: string;
      InvestUnitId: string;
      InstrumentId: string }

type QryInvestorProdRcamsMarginRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      CombProductId: string;
      ProductGroupId: string }

type QryInvestorProdRuleMarginRequest =
    { 
      ExchangeId: string;
      BrokerId: string;
      InvestorId: string;
      ProdFamilyCode: string;
      CommodityGroupId: int }

type QryInvestorProdSpbmDetailRequest =
    { 
      ExchangeId: string;
      BrokerId: string;
      InvestorId: string;
      ProdFamilyCode: string }

type QryInvestorProductGroupMarginRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      HedgeFlag: char option;
      ExchangeId: string;
      InvestUnitId: string;
      ProductGroupId: string }

type QryMaxOrderVolumeRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      Direction: char option;
      OffsetFlag: char option;
      HedgeFlag: char option;
      MaxVolume: int;
      ExchangeId: string;
      InvestUnitId: string;
      InstrumentId: string }

type QryMmInstrumentCommissionRateRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      InstrumentId: string }

type QryMmOptionInstrCommRateRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      InstrumentId: string }

type QryNoticeRequest =
    { 
      BrokerId: string }

type QryOffsetSettingRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      ProductId: string;
      OffsetType: char option }

type QryOptionInstrCommRateRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      ExchangeId: string;
      InvestUnitId: string;
      InstrumentId: string }

type QryOptionInstrTradeCostRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      HedgeFlag: char option;
      InputPrice: decimal;
      UnderlyingPrice: decimal;
      ExchangeId: string;
      InvestUnitId: string;
      InstrumentId: string }

type QryOptionSelfCloseRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      ExchangeId: string;
      OptionSelfCloseSysId: string;
      InsertTimeStart: string;
      InsertTimeEnd: string;
      InstrumentId: string }

type QryOrderRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      ExchangeId: string;
      OrderSysId: string;
      InsertTimeStart: string;
      InsertTimeEnd: string;
      InvestUnitId: string;
      InstrumentId: string }

type QryParkedOrderRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      ExchangeId: string;
      InvestUnitId: string;
      InstrumentId: string }

type QryParkedOrderActionRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      ExchangeId: string;
      InvestUnitId: string;
      InstrumentId: string }

type QryProductRequest =
    { 
      Reserve1: string;
      ProductClass: char option;
      ExchangeId: string;
      ProductId: string }

type QryProductExchRateRequest =
    { 
      Reserve1: string;
      ExchangeId: string;
      ProductId: string }

type QryProductGroupRequest =
    { 
      Reserve1: string;
      ExchangeId: string;
      ProductId: string }

type QryQuoteRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      ExchangeId: string;
      QuoteSysId: string;
      InsertTimeStart: string;
      InsertTimeEnd: string;
      InvestUnitId: string;
      InstrumentId: string }

type QryRcamsCombProductInfoRequest =
    { 
      ProductId: string;
      CombProductId: string;
      ProductGroupId: string }

type QryRcamsInstrParameterRequest =
    { 
      ProductId: string }

type QryRcamsInterParameterRequest =
    { 
      ProductGroupId: string;
      CombProduct1: string;
      CombProduct2: string }

type QryRcamsIntraParameterRequest =
    { 
      CombProductId: string }

type QryRcamsInvestorCombPositionRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      InstrumentId: string;
      CombInstrumentId: string }

type QryRcamsShortOptAdjustParamRequest =
    { 
      CombProductId: string }

type QryRiskSettleInvstPositionRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      InstrumentId: string }

type QryRiskSettleProductStatusRequest =
    { 
      ProductId: string }

type QryRuleInstrParameterRequest =
    { 
      ExchangeId: string;
      InstrumentId: string }

type QryRuleInterParameterRequest =
    { 
      ExchangeId: string;
      Leg1ProdFamilyCode: string;
      Leg2ProdFamilyCode: string;
      CommodityGroupId: int }

type QryRuleIntraParameterRequest =
    { 
      ExchangeId: string;
      ProdFamilyCode: string }

type QrySecAgentAcIdMapRequest =
    { 
      BrokerId: string;
      UserId: string;
      AccountId: string;
      CurrencyId: string }

type QrySecAgentCheckModeRequest =
    { 
      BrokerId: string;
      InvestorId: string }

type QrySecAgentTradeInfoRequest =
    { 
      BrokerId: string;
      BrokerSecAgentId: string }

type QrySettlementInfoRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      TradingDay: string;
      AccountId: string;
      CurrencyId: string }

type QrySettlementInfoConfirmRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      AccountId: string;
      CurrencyId: string }

type QrySpbmAddOnInterParameterRequest =
    { 
      ExchangeId: string;
      Leg1ProdFamilyCode: string;
      Leg2ProdFamilyCode: string }

type QrySpbmFutureParameterRequest =
    { 
      ExchangeId: string;
      InstrumentId: string;
      ProdFamilyCode: string }

type QrySpbmInterParameterRequest =
    { 
      ExchangeId: string;
      Leg1ProdFamilyCode: string;
      Leg2ProdFamilyCode: string }

type QrySpbmIntraParameterRequest =
    { 
      ExchangeId: string;
      ProdFamilyCode: string }

type QrySpbmInvestorPortfDefRequest =
    { 
      ExchangeId: string;
      BrokerId: string;
      InvestorId: string }

type QrySpbmOptionParameterRequest =
    { 
      ExchangeId: string;
      InstrumentId: string;
      ProdFamilyCode: string }

type QrySpbmPortfDefinitionRequest =
    { 
      ExchangeId: string;
      PortfolioDefId: int;
      ProdFamilyCode: string }

type QrySpdApplyRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      ExchangeId: string;
      OrderSysId: string;
      FirstLegInstrumentId: string;
      SecondLegInstrumentId: string }

type QrySpmmInstParamRequest =
    { 
      InstrumentId: string }

type QrySpmmProductParamRequest =
    { 
      ProductId: string }

type QryTradeRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      ExchangeId: string;
      TradeId: string;
      TradeTimeStart: string;
      TradeTimeEnd: string;
      InvestUnitId: string;
      InstrumentId: string }

type QryTraderOfferRequest =
    { 
      ExchangeId: string;
      ParticipantId: string;
      TraderId: string }

type QryTradingCodeRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      ExchangeId: string;
      ClientId: string;
      ClientIdType: char option;
      InvestUnitId: string }

type QryTradingNoticeRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      InvestUnitId: string }

type QryTransferBankRequest =
    { 
      BankId: string;
      BankBrchId: string }

type QryTransferSerialRequest =
    { 
      BrokerId: string;
      AccountId: string;
      BankId: string;
      CurrencyId: string }

type QryUserSessionRequest =
    { 
      FrontId: int;
      SessionId: int;
      BrokerId: string;
      UserId: string }

type QueryCfmmcTradingAccountTokenRequest =
    { 
      BrokerId: string;
      InvestorId: string;
      InvestUnitId: string }

type Quote =
    { 
      BrokerId: string;
      InvestorId: string;
      Reserve1: string;
      QuoteRef: string;
      UserId: string;
      AskPrice: decimal;
      BidPrice: decimal;
      AskVolume: int;
      BidVolume: int;
      RequestId: int;
      BusinessUnit: string;
      AskOffsetFlag: char option;
      BidOffsetFlag: char option;
      AskHedgeFlag: char option;
      BidHedgeFlag: char option;
      QuoteLocalId: string;
      ExchangeId: string;
      ParticipantId: string;
      ClientId: string;
      Reserve2: string;
      TraderId: string;
      InstallId: int;
      NotifySequence: int;
      OrderSubmitStatus: char option;
      TradingDay: string;
      SettlementId: int;
      QuoteSysId: string;
      InsertDate: string;
      InsertTime: string;
      CancelTime: string;
      QuoteStatus: char option;
      ClearingPartId: string;
      SequenceNo: int;
      AskOrderSysId: string;
      BidOrderSysId: string;
      FrontId: int;
      SessionId: int;
      UserProductInfo: string;
      StatusMsg: string;
      ActiveUserId: string;
      BrokerQuoteSeq: int;
      AskOrderRef: string;
      BidOrderRef: string;
      ForQuoteSysId: string;
      BranchId: string;
      InvestUnitId: string;
      AccountId: string;
      CurrencyId: string;
      Reserve3: string;
      MacAddress: string;
      InstrumentId: string;
      ExchangeInstId: string;
      IpAddress: string;
      ReplaceSysId: string;
      TimeCondition: char option;
      OrderMemo: string;
      SessionReqSeq: int }

type QuoteAction =
    { 
      BrokerId: string;
      InvestorId: string;
      QuoteActionRef: int;
      QuoteRef: string;
      RequestId: int;
      FrontId: int;
      SessionId: int;
      ExchangeId: string;
      QuoteSysId: string;
      ActionFlag: char option;
      ActionDate: string;
      ActionTime: string;
      TraderId: string;
      InstallId: int;
      QuoteLocalId: string;
      ActionLocalId: string;
      ParticipantId: string;
      ClientId: string;
      BusinessUnit: string;
      OrderActionStatus: char option;
      UserId: string;
      StatusMsg: string;
      Reserve1: string;
      BranchId: string;
      InvestUnitId: string;
      Reserve2: string;
      MacAddress: string;
      InstrumentId: string;
      IpAddress: string;
      OrderMemo: string;
      SessionReqSeq: int }

type RcamsCombProductInfo =
    { 
      TradingDay: string;
      ExchangeId: string;
      ProductId: string;
      CombProductId: string;
      ProductGroupId: string }

type RcamsInstrParameter =
    { 
      TradingDay: string;
      ExchangeId: string;
      ProductId: string;
      HedgeRate: decimal }

type RcamsInterParameter =
    { 
      TradingDay: string;
      ExchangeId: string;
      ProductGroupId: string;
      Priority: int;
      CreditRate: decimal;
      CombProduct1: string;
      CombProduct2: string }

type RcamsIntraParameter =
    { 
      TradingDay: string;
      ExchangeId: string;
      CombProductId: string;
      HedgeRate: decimal }

type RcamsInvestorCombPosition =
    { 
      ExchangeId: string;
      BrokerId: string;
      InvestorId: string;
      InstrumentId: string;
      HedgeFlag: char option;
      PosiDirection: char option;
      CombInstrumentId: string;
      LegId: int;
      ExchangeInstId: string;
      TotalAmt: int;
      ExchMargin: decimal;
      Margin: decimal }

type RcamsShortOptAdjustParam =
    { 
      TradingDay: string;
      ExchangeId: string;
      CombProductId: string;
      HedgeFlag: char option;
      AdjustValue: decimal }

type RemoveParkedOrder =
    { 
      BrokerId: string;
      InvestorId: string;
      ParkedOrderId: string;
      InvestUnitId: string }

type RemoveParkedOrderAction =
    { 
      BrokerId: string;
      InvestorId: string;
      ParkedOrderActionId: string;
      InvestUnitId: string }

type GenSmsCodeRequest =
    { 
      BrokerId: string;
      UserId: string;
      Mobile: string }

type GenUserCaptchaRequest =
    { 
      TradingDay: string;
      BrokerId: string;
      UserId: string }

type GenUserTextRequest =
    { 
      TradingDay: string;
      BrokerId: string;
      UserId: string }

type ReqQueryAccount =
    { 
      TradeCode: string;
      BankId: string;
      BankBranchId: string;
      BrokerId: string;
      BrokerBranchId: string;
      TradeDate: string;
      TradeTime: string;
      BankSerial: string;
      TradingDay: string;
      PlateSerial: int;
      LastFragment: char option;
      SessionId: int;
      CustomerName: string;
      IdCardType: char option;
      IdentifiedCardNo: string;
      CustType: char option;
      BankAccount: string;
      BankPassWord: string;
      AccountId: string;
      Password: string;
      FutureSerial: int;
      InstallId: int;
      UserId: string;
      VerifyCertNoFlag: char option;
      CurrencyId: string;
      Digest: string;
      BankAccType: char option;
      DeviceId: string;
      BankSecuAccType: char option;
      BrokerIdByBank: string;
      BankSecuAcc: string;
      BankPwdFlag: char option;
      SecuPwdFlag: char option;
      OperNo: string;
      RequestId: int;
      TId: int;
      LongCustomerName: string }

type TransferRequest =
    { 
      TradeCode: string;
      BankId: string;
      BankBranchId: string;
      BrokerId: string;
      BrokerBranchId: string;
      TradeDate: string;
      TradeTime: string;
      BankSerial: string;
      TradingDay: string;
      PlateSerial: int;
      LastFragment: char option;
      SessionId: int;
      CustomerName: string;
      IdCardType: char option;
      IdentifiedCardNo: string;
      CustType: char option;
      BankAccount: string;
      BankPassWord: string;
      AccountId: string;
      Password: string;
      InstallId: int;
      FutureSerial: int;
      UserId: string;
      VerifyCertNoFlag: char option;
      CurrencyId: string;
      TradeAmount: decimal;
      FutureFetchAmount: decimal;
      FeePayFlag: char option;
      CustFee: decimal;
      BrokerFee: decimal;
      Message: string;
      Digest: string;
      BankAccType: char option;
      DeviceId: string;
      BankSecuAccType: char option;
      BrokerIdByBank: string;
      BankSecuAcc: string;
      BankPwdFlag: char option;
      SecuPwdFlag: char option;
      OperNo: string;
      RequestId: int;
      TId: int;
      TransferStatus: char option;
      LongCustomerName: string }

type UserAuthMethodRequest =
    { 
      TradingDay: string;
      BrokerId: string;
      UserId: string }

type UserLoginWithCaptchaRequest =
    { 
      TradingDay: string;
      BrokerId: string;
      UserId: string;
      Password: string;
      UserProductInfo: string;
      InterfaceProductInfo: string;
      ProtocolInfo: string;
      MacAddress: string;
      Reserve1: string;
      LoginRemark: string;
      Captcha: string;
      ClientIpPort: int;
      ClientIpAddress: string }

type UserLoginWithOtpRequest =
    { 
      TradingDay: string;
      BrokerId: string;
      UserId: string;
      Password: string;
      UserProductInfo: string;
      InterfaceProductInfo: string;
      ProtocolInfo: string;
      MacAddress: string;
      Reserve1: string;
      LoginRemark: string;
      OtpPassword: string;
      ClientIpPort: int;
      ClientIpAddress: string }

type UserLoginWithTextRequest =
    { 
      TradingDay: string;
      BrokerId: string;
      UserId: string;
      Password: string;
      UserProductInfo: string;
      InterfaceProductInfo: string;
      ProtocolInfo: string;
      MacAddress: string;
      Reserve1: string;
      LoginRemark: string;
      Text: string;
      ClientIpPort: int;
      ClientIpAddress: string }

type RiskSettleInvstPosition =
    { 
      InstrumentId: string;
      BrokerId: string;
      InvestorId: string;
      PosiDirection: char option;
      HedgeFlag: char option;
      PositionDate: char option;
      YdPosition: int;
      Position: int;
      LongFrozen: int;
      ShortFrozen: int;
      LongFrozenAmount: decimal;
      ShortFrozenAmount: decimal;
      OpenVolume: int;
      CloseVolume: int;
      OpenAmount: decimal;
      CloseAmount: decimal;
      PositionCost: decimal;
      PreMargin: decimal;
      UseMargin: decimal;
      FrozenMargin: decimal;
      FrozenCash: decimal;
      FrozenCommission: decimal;
      CashIn: decimal;
      Commission: decimal;
      CloseProfit: decimal;
      PositionProfit: decimal;
      PreSettlementPrice: decimal;
      SettlementPrice: decimal;
      TradingDay: string;
      SettlementId: int;
      OpenCost: decimal;
      ExchangeMargin: decimal;
      CombPosition: int;
      CombLongFrozen: int;
      CombShortFrozen: int;
      CloseProfitByDate: decimal;
      CloseProfitByTrade: decimal;
      TodayPosition: int;
      MarginRateByMoney: decimal;
      MarginRateByVolume: decimal;
      StrikeFrozen: int;
      StrikeFrozenAmount: decimal;
      AbandonFrozen: int;
      ExchangeId: string;
      YdStrikeFrozen: int;
      InvestUnitId: string;
      PositionCostOffset: decimal;
      TasPosition: int;
      TasPositionCost: decimal }

type RiskSettleProductStatus =
    { 
      ExchangeId: string;
      ProductId: string;
      ProductStatus: char option }

type RspGenSmsCode =
    { 
      BrokerId: string;
      UserId: string;
      GenTime: string }

type RspGenUserCaptcha =
    { 
      BrokerId: string;
      UserId: string;
      CaptchaInfoLen: int;
      CaptchaInfo: string }

type RspGenUserText =
    { 
      UserTextSeq: int }

type RspTransfer =
    { 
      TradeCode: string;
      BankId: string;
      BankBranchId: string;
      BrokerId: string;
      BrokerBranchId: string;
      TradeDate: string;
      TradeTime: string;
      BankSerial: string;
      TradingDay: string;
      PlateSerial: int;
      LastFragment: char option;
      SessionId: int;
      CustomerName: string;
      IdCardType: char option;
      IdentifiedCardNo: string;
      CustType: char option;
      BankAccount: string;
      BankPassWord: string;
      AccountId: string;
      Password: string;
      InstallId: int;
      FutureSerial: int;
      UserId: string;
      VerifyCertNoFlag: char option;
      CurrencyId: string;
      TradeAmount: decimal;
      FutureFetchAmount: decimal;
      FeePayFlag: char option;
      CustFee: decimal;
      BrokerFee: decimal;
      Message: string;
      Digest: string;
      BankAccType: char option;
      DeviceId: string;
      BankSecuAccType: char option;
      BrokerIdByBank: string;
      BankSecuAcc: string;
      BankPwdFlag: char option;
      SecuPwdFlag: char option;
      OperNo: string;
      RequestId: int;
      TId: int;
      TransferStatus: char option;
      ErrorId: int;
      ErrorMsg: string;
      LongCustomerName: string }

type RspUserAuthMethod =
    { 
      UsableAuthMethod: int }

type RuleInstrParameter =
    { 
      TradingDay: string;
      ExchangeId: string;
      InstrumentId: string;
      InstrumentClass: char option;
      StdInstrumentId: string;
      BSpecRatio: decimal;
      SSpecRatio: decimal;
      BHedgeRatio: decimal;
      SHedgeRatio: decimal;
      BAddOnMargin: decimal;
      SAddOnMargin: decimal;
      CommodityGroupId: int }

type RuleInterParameter =
    { 
      TradingDay: string;
      ExchangeId: string;
      SpreadId: int;
      InterRate: decimal;
      Leg1ProdFamilyCode: string;
      Leg2ProdFamilyCode: string;
      Leg1PropFactor: int;
      Leg2PropFactor: int;
      CommodityGroupId: int;
      CommodityGroupName: string }

type RuleIntraParameter =
    { 
      TradingDay: string;
      ExchangeId: string;
      ProdFamilyCode: string;
      StdInstrumentId: string;
      StdInstrMargin: decimal;
      UsualIntraRate: decimal;
      DeliveryIntraRate: decimal }

type SecAgentAcIdMap =
    { 
      BrokerId: string;
      UserId: string;
      AccountId: string;
      CurrencyId: string;
      BrokerSecAgentId: string }

type SecAgentCheckMode =
    { 
      InvestorId: string;
      BrokerId: string;
      CurrencyId: string;
      BrokerSecAgentId: string;
      CheckSelfAccount: int }

type SecAgentTradeInfo =
    { 
      BrokerId: string;
      BrokerSecAgentId: string;
      InvestorId: string;
      LongCustomerName: string }

type SettlementInfo =
    { 
      TradingDay: string;
      SettlementId: int;
      BrokerId: string;
      InvestorId: string;
      SequenceNo: int;
      Content: string;
      AccountId: string;
      CurrencyId: string }

type SpbmAddOnInterParameter =
    { 
      TradingDay: string;
      ExchangeId: string;
      SpreadId: int;
      AddOnInterRateZ2: decimal;
      Leg1ProdFamilyCode: string;
      Leg2ProdFamilyCode: string }

type SpbmFutureParameter =
    { 
      TradingDay: string;
      ExchangeId: string;
      InstrumentId: string;
      ProdFamilyCode: string;
      Cvf: int;
      TimeRange: char option;
      MarginRate: decimal;
      LockRateX: decimal;
      AddOnRate: decimal;
      PreSettlementPrice: decimal;
      AddOnLockRateX2: decimal }

type SpbmInterParameter =
    { 
      TradingDay: string;
      ExchangeId: string;
      SpreadId: int;
      InterRateZ: decimal;
      Leg1ProdFamilyCode: string;
      Leg2ProdFamilyCode: string }

type SpbmIntraParameter =
    { 
      TradingDay: string;
      ExchangeId: string;
      ProdFamilyCode: string;
      IntraRateY: decimal;
      AddOnIntraRateY2: decimal }

type SpbmInvestorPortfDef =
    { 
      ExchangeId: string;
      BrokerId: string;
      InvestorId: string;
      PortfolioDefId: int }

type SpbmOptionParameter =
    { 
      TradingDay: string;
      ExchangeId: string;
      InstrumentId: string;
      ProdFamilyCode: string;
      Cvf: int;
      DownPrice: decimal;
      Delta: decimal;
      SlimiDelta: decimal;
      PreSettlementPrice: decimal }

type SpbmPortfDefinition =
    { 
      ExchangeId: string;
      PortfolioDefId: int;
      ProdFamilyCode: string;
      IsSpbm: int }

type SpdApply =
    { 
      BrokerId: string;
      InvestorId: string;
      FirstLegInstrumentId: string;
      SecondLegInstrumentId: string;
      UserId: string;
      Volume: int;
      Direction: char option;
      RequestId: int;
      FrontId: int;
      SessionId: int;
      OrderRef: string;
      ActiveUserId: string;
      BrokerOrderSeq: int;
      OrderSysId: string;
      ApplyStatus: char option;
      SequenceNo: int;
      InsertDate: string;
      InsertTime: string;
      CancelTime: string;
      OrderLocalId: string;
      ExchangeId: string;
      ParticipantId: string;
      ClientId: string;
      ExchangeInstId: string;
      TraderId: string;
      InstallId: int;
      OrderSubmitStatus: char option;
      NotifySequence: int;
      TradingDay: string;
      SettlementId: int;
      IpAddress: string;
      MacAddress: string;
      CmbType: char option;
      StatusMsg: string }

type SpdApplyAction =
    { 
      BrokerId: string;
      InvestorId: string;
      ActionDate: string;
      ActionTime: string;
      TraderId: string;
      InstallId: int;
      OrderLocalId: string;
      ActionLocalId: string;
      ParticipantId: string;
      ClientId: string;
      OrderActionStatus: char option;
      UserId: string;
      ExchangeId: string;
      OrderSysId: string;
      RequestId: int;
      StatusMsg: string;
      OrderRef: string;
      FrontId: int;
      SessionId: int;
      IpAddress: string;
      MacAddress: string }

type SpmmInstParam =
    { 
      ExchangeId: string;
      InstrumentId: string;
      InstMarginCalId: char option;
      CommodityId: string;
      CommodityGroupId: string }

type SpmmProductParam =
    { 
      ExchangeId: string;
      ProductId: string;
      CommodityId: string;
      CommodityGroupId: string }

type TraderOffer =
    { 
      ExchangeId: string;
      TraderId: string;
      ParticipantId: string;
      Password: string;
      InstallId: int;
      OrderLocalId: string;
      TraderConnectStatus: char option;
      ConnectRequestDate: string;
      ConnectRequestTime: string;
      LastReportDate: string;
      LastReportTime: string;
      ConnectDate: string;
      ConnectTime: string;
      StartDate: string;
      StartTime: string;
      TradingDay: string;
      BrokerId: string;
      MaxTradeId: string;
      MaxOrderMessageReference: string;
      OrderCancelAlg: char option }

type TradingAccountPasswordUpdate =
    { 
      BrokerId: string;
      AccountId: string;
      OldPassword: string;
      NewPassword: string;
      CurrencyId: string }

type TradingCode =
    { 
      InvestorId: string;
      BrokerId: string;
      ExchangeId: string;
      ClientId: string;
      IsActive: int;
      ClientIdType: char option;
      BranchId: string;
      BizType: char option;
      InvestUnitId: string }

type TradingNotice =
    { 
      BrokerId: string;
      InvestorRange: char option;
      InvestorId: string;
      SequenceSeries: int;
      UserId: string;
      SendTime: string;
      SequenceNo: int;
      FieldContent: string;
      InvestUnitId: string }

type TransferBank =
    { 
      BankId: string;
      BankBrchId: string;
      BankName: string;
      IsActive: int }

type TransferSerial =
    { 
      PlateSerial: int;
      TradeDate: string;
      TradingDay: string;
      TradeTime: string;
      TradeCode: string;
      SessionId: int;
      BankId: string;
      BankBranchId: string;
      BankAccType: char option;
      BankAccount: string;
      BankSerial: string;
      BrokerId: string;
      BrokerBranchId: string;
      FutureAccType: char option;
      AccountId: string;
      InvestorId: string;
      FutureSerial: int;
      IdCardType: char option;
      IdentifiedCardNo: string;
      CurrencyId: string;
      TradeAmount: decimal;
      CustFee: decimal;
      BrokerFee: decimal;
      AvailabilityFlag: char option;
      OperatorCode: string;
      BankNewAccount: string;
      ErrorId: int;
      ErrorMsg: string }

type UserPasswordUpdate =
    { 
      BrokerId: string;
      UserId: string;
      OldPassword: string;
      NewPassword: string }

type UserSession =
    { 
      FrontId: int;
      SessionId: int;
      BrokerId: string;
      UserId: string;
      LoginDate: string;
      LoginTime: string;
      Reserve1: string;
      UserProductInfo: string;
      InterfaceProductInfo: string;
      ProtocolInfo: string;
      MacAddress: string;
      LoginRemark: string;
      IpAddress: string }

type UserSystemInfo =
    { 
      BrokerId: string;
      UserId: string;
      ClientSystemInfoLen: int;
      ClientSystemInfo: string;
      Reserve1: string;
      ClientIpPort: int;
      ClientLoginTime: string;
      ClientAppId: string;
      ClientPublicIp: string;
      ClientLoginRemark: string;
      Mac: string }

type WechatUserSystemInfo =
    { 
      BrokerId: string;
      UserId: string;
      WechatCltSysInfoLen: int;
      WechatCltSysInfo: string;
      ClientIpPort: int;
      ClientLoginTime: string;
      ClientAppId: string;
      ClientPublicIp: string;
      ClientLoginRemark: string }

type TraderCallbacks =
    {
      FrontConnected: (unit -> unit) option
      FrontDisconnected: (int -> unit) option
      HeartBeatWarning: (int -> unit) option
      RtnPrivateSeqNo: (int -> unit) option
      RspAuthenticate: (AuthenticateResponse option -> RspInfo option -> int -> bool -> unit) option
      RspSettlementInfoConfirm: (SettlementInfoConfirm option -> RspInfo option -> int -> bool -> unit) option
      RspUserLogin: (UserLoginResponse option -> RspInfo option -> int -> bool -> unit) option
      RspUserLogout: (UserLogoutResponse option -> RspInfo option -> int -> bool -> unit) option
      RspError: (RspInfo option -> int -> bool -> unit) option
      RspQryTradingAccount: (TradingAccount option -> RspInfo option -> int -> bool -> unit) option
      RspQryInvestorPosition: (InvestorPosition option -> RspInfo option -> int -> bool -> unit) option
      RspQryInstrumentMarginRate: (InstrumentMarginRate option -> RspInfo option -> int -> bool -> unit) option
      RspQryExchangeMarginRate: (ExchangeMarginRate option -> RspInfo option -> int -> bool -> unit) option
      RspQryInstrumentCommissionRate: (InstrumentCommissionRate option -> RspInfo option -> int -> bool -> unit) option
      RspOrderInsert: (InputOrderRequest option -> RspInfo option -> int -> bool -> unit) option
      RspOrderAction: (InputOrderActionRequest option -> RspInfo option -> int -> bool -> unit) option
      RtnOrder: (OrderUpdate -> unit) option
      RtnTrade: (TradeUpdate -> unit) option
      ErrRtnBankToFutureByFuture: (TransferRequest option -> RspInfo option -> unit) option
      ErrRtnBatchOrderAction: (BatchOrderAction option -> RspInfo option -> unit) option
      ErrRtnCancelOffsetSetting: (CancelOffsetSetting option -> RspInfo option -> unit) option
      ErrRtnCombActionInsert: (InputCombActionRequest option -> RspInfo option -> unit) option
      ErrRtnExecOrderAction: (ExecOrderAction option -> RspInfo option -> unit) option
      ErrRtnExecOrderInsert: (InputExecOrderRequest option -> RspInfo option -> unit) option
      ErrRtnForQuoteInsert: (InputForQuoteRequest option -> RspInfo option -> unit) option
      ErrRtnFutureToBankByFuture: (TransferRequest option -> RspInfo option -> unit) option
      ErrRtnHedgeCfm: (InputHedgeCfmRequest option -> RspInfo option -> unit) option
      ErrRtnHedgeCfmAction: (HedgeCfmAction option -> RspInfo option -> unit) option
      ErrRtnOffsetSetting: (InputOffsetSettingRequest option -> RspInfo option -> unit) option
      ErrRtnOptionSelfCloseAction: (OptionSelfCloseAction option -> RspInfo option -> unit) option
      ErrRtnOptionSelfCloseInsert: (InputOptionSelfCloseRequest option -> RspInfo option -> unit) option
      ErrRtnOrderAction: (OrderAction option -> RspInfo option -> unit) option
      ErrRtnOrderInsert: (InputOrderRequest option -> RspInfo option -> unit) option
      ErrRtnQueryBankBalanceByFuture: (ReqQueryAccount option -> RspInfo option -> unit) option
      ErrRtnQuoteAction: (QuoteAction option -> RspInfo option -> unit) option
      ErrRtnQuoteInsert: (InputQuoteRequest option -> RspInfo option -> unit) option
      ErrRtnSpdApply: (InputSpdApplyRequest option -> RspInfo option -> unit) option
      ErrRtnSpdApplyAction: (SpdApplyAction option -> RspInfo option -> unit) option
      RspBatchOrderAction: (InputBatchOrderActionRequest option -> RspInfo option -> int -> bool -> unit) option
      RspCancelOffsetSetting: (InputOffsetSettingRequest option -> RspInfo option -> int -> bool -> unit) option
      RspCombActionInsert: (InputCombActionRequest option -> RspInfo option -> int -> bool -> unit) option
      RspExecOrderAction: (InputExecOrderActionRequest option -> RspInfo option -> int -> bool -> unit) option
      RspExecOrderInsert: (InputExecOrderRequest option -> RspInfo option -> int -> bool -> unit) option
      RspForQuoteInsert: (InputForQuoteRequest option -> RspInfo option -> int -> bool -> unit) option
      RspFromBankToFutureByFuture: (TransferRequest option -> RspInfo option -> int -> bool -> unit) option
      RspFromFutureToBankByFuture: (TransferRequest option -> RspInfo option -> int -> bool -> unit) option
      RspGenSmsCode: (RspGenSmsCode option -> RspInfo option -> int -> bool -> unit) option
      RspGenUserCaptcha: (RspGenUserCaptcha option -> RspInfo option -> int -> bool -> unit) option
      RspGenUserText: (RspGenUserText option -> RspInfo option -> int -> bool -> unit) option
      RspHedgeCfm: (InputHedgeCfmRequest option -> RspInfo option -> int -> bool -> unit) option
      RspHedgeCfmAction: (InputHedgeCfmActionRequest option -> RspInfo option -> int -> bool -> unit) option
      RspOffsetSetting: (InputOffsetSettingRequest option -> RspInfo option -> int -> bool -> unit) option
      RspOptionSelfCloseAction: (InputOptionSelfCloseActionRequest option -> RspInfo option -> int -> bool -> unit) option
      RspOptionSelfCloseInsert: (InputOptionSelfCloseRequest option -> RspInfo option -> int -> bool -> unit) option
      RspParkedOrderAction: (ParkedOrderAction option -> RspInfo option -> int -> bool -> unit) option
      RspParkedOrderInsert: (ParkedOrder option -> RspInfo option -> int -> bool -> unit) option
      RspQryAccountregister: (Accountregister option -> RspInfo option -> int -> bool -> unit) option
      RspQryBrokerTradingAlgos: (BrokerTradingAlgos option -> RspInfo option -> int -> bool -> unit) option
      RspQryBrokerTradingParams: (BrokerTradingParams option -> RspInfo option -> int -> bool -> unit) option
      RspQryCfmmcTradingAccountKey: (CfmmcTradingAccountKey option -> RspInfo option -> int -> bool -> unit) option
      RspQryClassifiedInstrument: (Instrument option -> RspInfo option -> int -> bool -> unit) option
      RspQryCombAction: (CombAction option -> RspInfo option -> int -> bool -> unit) option
      RspQryCombInstrumentGuard: (CombInstrumentGuard option -> RspInfo option -> int -> bool -> unit) option
      RspQryCombLeg: (CombLeg option -> RspInfo option -> int -> bool -> unit) option
      RspQryCombPromotionParam: (CombPromotionParam option -> RspInfo option -> int -> bool -> unit) option
      RspQryContractBank: (ContractBank option -> RspInfo option -> int -> bool -> unit) option
      RspQryDepthMarketData: (DepthMarketData option -> RspInfo option -> int -> bool -> unit) option
      RspQryEWarrantOffset: (EWarrantOffset option -> RspInfo option -> int -> bool -> unit) option
      RspQryExchange: (Exchange option -> RspInfo option -> int -> bool -> unit) option
      RspQryExchangeMarginRateAdjust: (ExchangeMarginRateAdjust option -> RspInfo option -> int -> bool -> unit) option
      RspQryExchangeRate: (ExchangeRate option -> RspInfo option -> int -> bool -> unit) option
      RspQryExecOrder: (ExecOrder option -> RspInfo option -> int -> bool -> unit) option
      RspQryForQuote: (ForQuote option -> RspInfo option -> int -> bool -> unit) option
      RspQryHedgeCfm: (HedgeCfm option -> RspInfo option -> int -> bool -> unit) option
      RspQryInstrument: (Instrument option -> RspInfo option -> int -> bool -> unit) option
      RspQryInstrumentOrderCommRate: (InstrumentOrderCommRate option -> RspInfo option -> int -> bool -> unit) option
      RspQryInvestUnit: (InvestUnit option -> RspInfo option -> int -> bool -> unit) option
      RspQryInvestor: (Investor option -> RspInfo option -> int -> bool -> unit) option
      RspQryInvestorCommodityGroupSpmmMargin: (InvestorCommodityGroupSpmmMargin option -> RspInfo option -> int -> bool -> unit) option
      RspQryInvestorCommoditySpmmMargin: (InvestorCommoditySpmmMargin option -> RspInfo option -> int -> bool -> unit) option
      RspQryInvestorInfoCommRec: (InvestorInfoCommRec option -> RspInfo option -> int -> bool -> unit) option
      RspQryInvestorPortfMarginRatio: (InvestorPortfMarginRatio option -> RspInfo option -> int -> bool -> unit) option
      RspQryInvestorPortfSetting: (InvestorPortfSetting option -> RspInfo option -> int -> bool -> unit) option
      RspQryInvestorPositionCombineDetail: (InvestorPositionCombineDetail option -> RspInfo option -> int -> bool -> unit) option
      RspQryInvestorPositionDetail: (InvestorPositionDetail option -> RspInfo option -> int -> bool -> unit) option
      RspQryInvestorProdRcamsMargin: (InvestorProdRcamsMargin option -> RspInfo option -> int -> bool -> unit) option
      RspQryInvestorProdRuleMargin: (InvestorProdRuleMargin option -> RspInfo option -> int -> bool -> unit) option
      RspQryInvestorProdSpbmDetail: (InvestorProdSpbmDetail option -> RspInfo option -> int -> bool -> unit) option
      RspQryInvestorProductGroupMargin: (InvestorProductGroupMargin option -> RspInfo option -> int -> bool -> unit) option
      RspQryMmInstrumentCommissionRate: (MmInstrumentCommissionRate option -> RspInfo option -> int -> bool -> unit) option
      RspQryMmOptionInstrCommRate: (MmOptionInstrCommRate option -> RspInfo option -> int -> bool -> unit) option
      RspQryMaxOrderVolume: (QryMaxOrderVolumeRequest option -> RspInfo option -> int -> bool -> unit) option
      RspQryNotice: (Notice option -> RspInfo option -> int -> bool -> unit) option
      RspQryOffsetSetting: (OffsetSetting option -> RspInfo option -> int -> bool -> unit) option
      RspQryOptionInstrCommRate: (OptionInstrCommRate option -> RspInfo option -> int -> bool -> unit) option
      RspQryOptionInstrTradeCost: (OptionInstrTradeCost option -> RspInfo option -> int -> bool -> unit) option
      RspQryOptionSelfClose: (OptionSelfClose option -> RspInfo option -> int -> bool -> unit) option
      RspQryOrder: (OrderUpdate option -> RspInfo option -> int -> bool -> unit) option
      RspQryParkedOrder: (ParkedOrder option -> RspInfo option -> int -> bool -> unit) option
      RspQryParkedOrderAction: (ParkedOrderAction option -> RspInfo option -> int -> bool -> unit) option
      RspQryProduct: (Product option -> RspInfo option -> int -> bool -> unit) option
      RspQryProductExchRate: (ProductExchRate option -> RspInfo option -> int -> bool -> unit) option
      RspQryProductGroup: (ProductGroup option -> RspInfo option -> int -> bool -> unit) option
      RspQryQuote: (Quote option -> RspInfo option -> int -> bool -> unit) option
      RspQryRcamsCombProductInfo: (RcamsCombProductInfo option -> RspInfo option -> int -> bool -> unit) option
      RspQryRcamsInstrParameter: (RcamsInstrParameter option -> RspInfo option -> int -> bool -> unit) option
      RspQryRcamsInterParameter: (RcamsInterParameter option -> RspInfo option -> int -> bool -> unit) option
      RspQryRcamsIntraParameter: (RcamsIntraParameter option -> RspInfo option -> int -> bool -> unit) option
      RspQryRcamsInvestorCombPosition: (RcamsInvestorCombPosition option -> RspInfo option -> int -> bool -> unit) option
      RspQryRcamsShortOptAdjustParam: (RcamsShortOptAdjustParam option -> RspInfo option -> int -> bool -> unit) option
      RspQryRuleInstrParameter: (RuleInstrParameter option -> RspInfo option -> int -> bool -> unit) option
      RspQryRuleInterParameter: (RuleInterParameter option -> RspInfo option -> int -> bool -> unit) option
      RspQryRuleIntraParameter: (RuleIntraParameter option -> RspInfo option -> int -> bool -> unit) option
      RspQryRiskSettleInvstPosition: (RiskSettleInvstPosition option -> RspInfo option -> int -> bool -> unit) option
      RspQryRiskSettleProductStatus: (RiskSettleProductStatus option -> RspInfo option -> int -> bool -> unit) option
      RspQrySpbmAddOnInterParameter: (SpbmAddOnInterParameter option -> RspInfo option -> int -> bool -> unit) option
      RspQrySpbmFutureParameter: (SpbmFutureParameter option -> RspInfo option -> int -> bool -> unit) option
      RspQrySpbmInterParameter: (SpbmInterParameter option -> RspInfo option -> int -> bool -> unit) option
      RspQrySpbmIntraParameter: (SpbmIntraParameter option -> RspInfo option -> int -> bool -> unit) option
      RspQrySpbmInvestorPortfDef: (SpbmInvestorPortfDef option -> RspInfo option -> int -> bool -> unit) option
      RspQrySpbmOptionParameter: (SpbmOptionParameter option -> RspInfo option -> int -> bool -> unit) option
      RspQrySpbmPortfDefinition: (SpbmPortfDefinition option -> RspInfo option -> int -> bool -> unit) option
      RspQrySpmmInstParam: (SpmmInstParam option -> RspInfo option -> int -> bool -> unit) option
      RspQrySpmmProductParam: (SpmmProductParam option -> RspInfo option -> int -> bool -> unit) option
      RspQrySecAgentAcIdMap: (SecAgentAcIdMap option -> RspInfo option -> int -> bool -> unit) option
      RspQrySecAgentCheckMode: (SecAgentCheckMode option -> RspInfo option -> int -> bool -> unit) option
      RspQrySecAgentTradeInfo: (SecAgentTradeInfo option -> RspInfo option -> int -> bool -> unit) option
      RspQrySecAgentTradingAccount: (TradingAccount option -> RspInfo option -> int -> bool -> unit) option
      RspQrySettlementInfo: (SettlementInfo option -> RspInfo option -> int -> bool -> unit) option
      RspQrySettlementInfoConfirm: (SettlementInfoConfirm option -> RspInfo option -> int -> bool -> unit) option
      RspQrySpdApply: (SpdApply option -> RspInfo option -> int -> bool -> unit) option
      RspQryTrade: (TradeUpdate option -> RspInfo option -> int -> bool -> unit) option
      RspQryTraderOffer: (TraderOffer option -> RspInfo option -> int -> bool -> unit) option
      RspQryTradingCode: (TradingCode option -> RspInfo option -> int -> bool -> unit) option
      RspQryTradingNotice: (TradingNotice option -> RspInfo option -> int -> bool -> unit) option
      RspQryTransferBank: (TransferBank option -> RspInfo option -> int -> bool -> unit) option
      RspQryTransferSerial: (TransferSerial option -> RspInfo option -> int -> bool -> unit) option
      RspQryUserSession: (UserSession option -> RspInfo option -> int -> bool -> unit) option
      RspQueryBankAccountMoneyByFuture: (ReqQueryAccount option -> RspInfo option -> int -> bool -> unit) option
      RspQueryCfmmcTradingAccountToken: (QueryCfmmcTradingAccountTokenRequest option -> RspInfo option -> int -> bool -> unit) option
      RspQuoteAction: (InputQuoteActionRequest option -> RspInfo option -> int -> bool -> unit) option
      RspQuoteInsert: (InputQuoteRequest option -> RspInfo option -> int -> bool -> unit) option
      RspRemoveParkedOrder: (RemoveParkedOrder option -> RspInfo option -> int -> bool -> unit) option
      RspRemoveParkedOrderAction: (RemoveParkedOrderAction option -> RspInfo option -> int -> bool -> unit) option
      RspSpdApply: (InputSpdApplyRequest option -> RspInfo option -> int -> bool -> unit) option
      RspSpdApplyAction: (InputSpdApplyActionRequest option -> RspInfo option -> int -> bool -> unit) option
      RspTradingAccountPasswordUpdate: (TradingAccountPasswordUpdate option -> RspInfo option -> int -> bool -> unit) option
      RspUserAuthMethod: (RspUserAuthMethod option -> RspInfo option -> int -> bool -> unit) option
      RspUserPasswordUpdate: (UserPasswordUpdate option -> RspInfo option -> int -> bool -> unit) option
      RtnCombAction: (CombAction -> unit) option
      RtnExecOrder: (ExecOrder -> unit) option
      RtnForQuoteRsp: (ForQuoteRsp -> unit) option
      RtnFromBankToFutureByFuture: (RspTransfer -> unit) option
      RtnFromFutureToBankByFuture: (RspTransfer -> unit) option
      RtnHedgeCfm: (HedgeCfm -> unit) option
      RtnOffsetSetting: (OffsetSetting -> unit) option
      RtnOptionSelfClose: (OptionSelfClose -> unit) option
      RtnQueryBankBalanceByFuture: (NotifyQueryAccount -> unit) option
      RtnQuote: (Quote -> unit) option
      RtnSpdApply: (SpdApply -> unit) option
    }

    static member Empty =
        { FrontConnected = None
          FrontDisconnected = None
          HeartBeatWarning = None
          RtnPrivateSeqNo = None
          RspAuthenticate = None
          RspSettlementInfoConfirm = None
          RspUserLogin = None
          RspUserLogout = None
          RspError = None
          RspQryTradingAccount = None
          RspQryInvestorPosition = None
          RspQryInstrumentMarginRate = None
          RspQryExchangeMarginRate = None
          RspQryInstrumentCommissionRate = None
          RspOrderInsert = None
          RspOrderAction = None
          RtnOrder = None
          RtnTrade = None
          ErrRtnBankToFutureByFuture = None
          ErrRtnBatchOrderAction = None
          ErrRtnCancelOffsetSetting = None
          ErrRtnCombActionInsert = None
          ErrRtnExecOrderAction = None
          ErrRtnExecOrderInsert = None
          ErrRtnForQuoteInsert = None
          ErrRtnFutureToBankByFuture = None
          ErrRtnHedgeCfm = None
          ErrRtnHedgeCfmAction = None
          ErrRtnOffsetSetting = None
          ErrRtnOptionSelfCloseAction = None
          ErrRtnOptionSelfCloseInsert = None
          ErrRtnOrderAction = None
          ErrRtnOrderInsert = None
          ErrRtnQueryBankBalanceByFuture = None
          ErrRtnQuoteAction = None
          ErrRtnQuoteInsert = None
          ErrRtnSpdApply = None
          ErrRtnSpdApplyAction = None
          RspBatchOrderAction = None
          RspCancelOffsetSetting = None
          RspCombActionInsert = None
          RspExecOrderAction = None
          RspExecOrderInsert = None
          RspForQuoteInsert = None
          RspFromBankToFutureByFuture = None
          RspFromFutureToBankByFuture = None
          RspGenSmsCode = None
          RspGenUserCaptcha = None
          RspGenUserText = None
          RspHedgeCfm = None
          RspHedgeCfmAction = None
          RspOffsetSetting = None
          RspOptionSelfCloseAction = None
          RspOptionSelfCloseInsert = None
          RspParkedOrderAction = None
          RspParkedOrderInsert = None
          RspQryAccountregister = None
          RspQryBrokerTradingAlgos = None
          RspQryBrokerTradingParams = None
          RspQryCfmmcTradingAccountKey = None
          RspQryClassifiedInstrument = None
          RspQryCombAction = None
          RspQryCombInstrumentGuard = None
          RspQryCombLeg = None
          RspQryCombPromotionParam = None
          RspQryContractBank = None
          RspQryDepthMarketData = None
          RspQryEWarrantOffset = None
          RspQryExchange = None
          RspQryExchangeMarginRateAdjust = None
          RspQryExchangeRate = None
          RspQryExecOrder = None
          RspQryForQuote = None
          RspQryHedgeCfm = None
          RspQryInstrument = None
          RspQryInstrumentOrderCommRate = None
          RspQryInvestUnit = None
          RspQryInvestor = None
          RspQryInvestorCommodityGroupSpmmMargin = None
          RspQryInvestorCommoditySpmmMargin = None
          RspQryInvestorInfoCommRec = None
          RspQryInvestorPortfMarginRatio = None
          RspQryInvestorPortfSetting = None
          RspQryInvestorPositionCombineDetail = None
          RspQryInvestorPositionDetail = None
          RspQryInvestorProdRcamsMargin = None
          RspQryInvestorProdRuleMargin = None
          RspQryInvestorProdSpbmDetail = None
          RspQryInvestorProductGroupMargin = None
          RspQryMmInstrumentCommissionRate = None
          RspQryMmOptionInstrCommRate = None
          RspQryMaxOrderVolume = None
          RspQryNotice = None
          RspQryOffsetSetting = None
          RspQryOptionInstrCommRate = None
          RspQryOptionInstrTradeCost = None
          RspQryOptionSelfClose = None
          RspQryOrder = None
          RspQryParkedOrder = None
          RspQryParkedOrderAction = None
          RspQryProduct = None
          RspQryProductExchRate = None
          RspQryProductGroup = None
          RspQryQuote = None
          RspQryRcamsCombProductInfo = None
          RspQryRcamsInstrParameter = None
          RspQryRcamsInterParameter = None
          RspQryRcamsIntraParameter = None
          RspQryRcamsInvestorCombPosition = None
          RspQryRcamsShortOptAdjustParam = None
          RspQryRuleInstrParameter = None
          RspQryRuleInterParameter = None
          RspQryRuleIntraParameter = None
          RspQryRiskSettleInvstPosition = None
          RspQryRiskSettleProductStatus = None
          RspQrySpbmAddOnInterParameter = None
          RspQrySpbmFutureParameter = None
          RspQrySpbmInterParameter = None
          RspQrySpbmIntraParameter = None
          RspQrySpbmInvestorPortfDef = None
          RspQrySpbmOptionParameter = None
          RspQrySpbmPortfDefinition = None
          RspQrySpmmInstParam = None
          RspQrySpmmProductParam = None
          RspQrySecAgentAcIdMap = None
          RspQrySecAgentCheckMode = None
          RspQrySecAgentTradeInfo = None
          RspQrySecAgentTradingAccount = None
          RspQrySettlementInfo = None
          RspQrySettlementInfoConfirm = None
          RspQrySpdApply = None
          RspQryTrade = None
          RspQryTraderOffer = None
          RspQryTradingCode = None
          RspQryTradingNotice = None
          RspQryTransferBank = None
          RspQryTransferSerial = None
          RspQryUserSession = None
          RspQueryBankAccountMoneyByFuture = None
          RspQueryCfmmcTradingAccountToken = None
          RspQuoteAction = None
          RspQuoteInsert = None
          RspRemoveParkedOrder = None
          RspRemoveParkedOrderAction = None
          RspSpdApply = None
          RspSpdApplyAction = None
          RspTradingAccountPasswordUpdate = None
          RspUserAuthMethod = None
          RspUserPasswordUpdate = None
          RtnCombAction = None
          RtnExecOrder = None
          RtnForQuoteRsp = None
          RtnFromBankToFutureByFuture = None
          RtnFromFutureToBankByFuture = None
          RtnHedgeCfm = None
          RtnOffsetSetting = None
          RtnOptionSelfClose = None
          RtnQueryBankBalanceByFuture = None
          RtnQuote = None
          RtnSpdApply = None }

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRspAuthenticate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable UserProductInfo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable AppId: byte array

    [<DefaultValue>]
    val mutable AppType: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeReqAuthenticate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable UserProductInfo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable AuthCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable AppId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSettlementInfoConfirm =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ConfirmDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ConfirmTime: byte array

    [<DefaultValue>]
    val mutable SettlementId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryTradingAccount =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<DefaultValue>]
    val mutable BizType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTradingAccount =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<DefaultValue>]
    val mutable Deposit: float

    [<DefaultValue>]
    val mutable Withdraw: float

    [<DefaultValue>]
    val mutable Balance: float

    [<DefaultValue>]
    val mutable Available: float

    [<DefaultValue>]
    val mutable CurrMargin: float

    [<DefaultValue>]
    val mutable FrozenMargin: float

    [<DefaultValue>]
    val mutable FrozenCash: float

    [<DefaultValue>]
    val mutable FrozenCommission: float

    [<DefaultValue>]
    val mutable Commission: float

    [<DefaultValue>]
    val mutable CloseProfit: float

    [<DefaultValue>]
    val mutable PositionProfit: float

    [<DefaultValue>]
    val mutable WithdrawQuota: float

    [<DefaultValue>]
    val mutable Reserve: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryInvestorPosition =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInvestorPosition =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<DefaultValue>]
    val mutable PosiDirection: byte

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<DefaultValue>]
    val mutable PositionDate: byte

    [<DefaultValue>]
    val mutable YdPosition: int

    [<DefaultValue>]
    val mutable Position: int

    [<DefaultValue>]
    val mutable TodayPosition: int

    [<DefaultValue>]
    val mutable LongFrozen: int

    [<DefaultValue>]
    val mutable ShortFrozen: int

    [<DefaultValue>]
    val mutable OpenVolume: int

    [<DefaultValue>]
    val mutable CloseVolume: int

    [<DefaultValue>]
    val mutable PositionProfit: float

    [<DefaultValue>]
    val mutable CloseProfit: float

    [<DefaultValue>]
    val mutable UseMargin: float

    [<DefaultValue>]
    val mutable PositionCost: float

    [<DefaultValue>]
    val mutable OpenCost: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryInstrumentMarginRate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInstrumentMarginRate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<DefaultValue>]
    val mutable InvestorRange: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<DefaultValue>]
    val mutable LongMarginRatioByMoney: float

    [<DefaultValue>]
    val mutable LongMarginRatioByVolume: float

    [<DefaultValue>]
    val mutable ShortMarginRatioByMoney: float

    [<DefaultValue>]
    val mutable ShortMarginRatioByVolume: float

    [<DefaultValue>]
    val mutable IsRelative: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryExchangeMarginRate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeExchangeMarginRate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<DefaultValue>]
    val mutable LongMarginRatioByMoney: float

    [<DefaultValue>]
    val mutable LongMarginRatioByVolume: float

    [<DefaultValue>]
    val mutable ShortMarginRatioByMoney: float

    [<DefaultValue>]
    val mutable ShortMarginRatioByVolume: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryInstrumentCommissionRate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInstrumentCommissionRate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<DefaultValue>]
    val mutable InvestorRange: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable OpenRatioByMoney: float

    [<DefaultValue>]
    val mutable OpenRatioByVolume: float

    [<DefaultValue>]
    val mutable CloseRatioByMoney: float

    [<DefaultValue>]
    val mutable CloseRatioByVolume: float

    [<DefaultValue>]
    val mutable CloseTodayRatioByMoney: float

    [<DefaultValue>]
    val mutable CloseTodayRatioByVolume: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<DefaultValue>]
    val mutable BizType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInputOrder =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderRef: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable OrderPriceType: byte

    [<DefaultValue>]
    val mutable Direction: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)>]
    [<DefaultValue>]
    val mutable CombOffsetFlag: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)>]
    [<DefaultValue>]
    val mutable CombHedgeFlag: byte array

    [<DefaultValue>]
    val mutable LimitPrice: float

    [<DefaultValue>]
    val mutable VolumeTotalOriginal: int

    [<DefaultValue>]
    val mutable TimeCondition: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable GtdDate: byte array

    [<DefaultValue>]
    val mutable VolumeCondition: byte

    [<DefaultValue>]
    val mutable MinVolume: int

    [<DefaultValue>]
    val mutable ContingentCondition: byte

    [<DefaultValue>]
    val mutable StopPrice: float

    [<DefaultValue>]
    val mutable ForceCloseReason: byte

    [<DefaultValue>]
    val mutable IsAutoSuspend: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable BusinessUnit: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable UserForceClose: int

    [<DefaultValue>]
    val mutable IsSwapOrder: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderMemo: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInputOrderAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable OrderActionRef: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderRef: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable OrderSysId: byte array

    [<DefaultValue>]
    val mutable ActionFlag: byte

    [<DefaultValue>]
    val mutable LimitPrice: float

    [<DefaultValue>]
    val mutable VolumeChange: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderMemo: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeOrder =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderRef: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable OrderSysId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable OrderPriceType: byte

    [<DefaultValue>]
    val mutable Direction: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)>]
    [<DefaultValue>]
    val mutable CombOffsetFlag: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)>]
    [<DefaultValue>]
    val mutable CombHedgeFlag: byte array

    [<DefaultValue>]
    val mutable LimitPrice: float

    [<DefaultValue>]
    val mutable VolumeTotalOriginal: int

    [<DefaultValue>]
    val mutable VolumeTraded: int

    [<DefaultValue>]
    val mutable VolumeTotal: int

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<DefaultValue>]
    val mutable OrderStatus: byte

    [<DefaultValue>]
    val mutable OrderSubmitStatus: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable StatusMsg: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ActiveTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable SuspendTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable UpdateTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable CancelTime: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTrade =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderRef: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable OrderSysId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TradeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable Direction: byte

    [<DefaultValue>]
    val mutable OffsetFlag: byte

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<DefaultValue>]
    val mutable Price: float

    [<DefaultValue>]
    val mutable Volume: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradeDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradeTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeAccountregister =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradeDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable BankId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)>]
    [<DefaultValue>]
    val mutable BankBranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BankAccount: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable BrokerBranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<DefaultValue>]
    val mutable IdCardType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 51)>]
    [<DefaultValue>]
    val mutable IdentifiedCardNo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 51)>]
    [<DefaultValue>]
    val mutable CustomerName: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<DefaultValue>]
    val mutable OpenOrDestroy: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable RegDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable OutDate: byte array

    [<DefaultValue>]
    val mutable TId: int

    [<DefaultValue>]
    val mutable CustType: byte

    [<DefaultValue>]
    val mutable BankAccType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 161)>]
    [<DefaultValue>]
    val mutable LongCustomerName: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeBatchOrderAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable OrderActionRef: int

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ActionDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ActionTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

    [<DefaultValue>]
    val mutable InstallId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable ActionLocalId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable BusinessUnit: byte array

    [<DefaultValue>]
    val mutable OrderActionStatus: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable StatusMsg: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeBrokerTradingAlgos =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<DefaultValue>]
    val mutable HandlePositionAlgoId: byte

    [<DefaultValue>]
    val mutable FindMarginRateAlgoId: byte

    [<DefaultValue>]
    val mutable HandleTradingAccountAlgoId: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeBrokerTradingParams =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable MarginPriceType: byte

    [<DefaultValue>]
    val mutable Algorithm: byte

    [<DefaultValue>]
    val mutable AvailIncludeCloseProfit: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<DefaultValue>]
    val mutable OptionRoyaltyPriceType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeCancelOffsetSetting =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable UnderlyingInstrId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

    [<DefaultValue>]
    val mutable OffsetType: byte

    [<DefaultValue>]
    val mutable Volume: int

    [<DefaultValue>]
    val mutable IsOffset: int

    [<DefaultValue>]
    val mutable RequestId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeSerialNo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable ExchangeProductId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

    [<DefaultValue>]
    val mutable InstallId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<DefaultValue>]
    val mutable OrderActionStatus: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable StatusMsg: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable ActionLocalId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ActionDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ActionTime: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeCfmmcTradingAccountKey =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<DefaultValue>]
    val mutable KeyId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable CurrentKey: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeCombAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable CombActionRef: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable Direction: byte

    [<DefaultValue>]
    val mutable Volume: int

    [<DefaultValue>]
    val mutable CombDirection: byte

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable ActionLocalId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

    [<DefaultValue>]
    val mutable InstallId: int

    [<DefaultValue>]
    val mutable ActionStatus: byte

    [<DefaultValue>]
    val mutable NotifySequence: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<DefaultValue>]
    val mutable SettlementId: int

    [<DefaultValue>]
    val mutable SequenceNo: int

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable UserProductInfo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable StatusMsg: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve3: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable ComTradeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable BranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeCombInstrumentGuard =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<DefaultValue>]
    val mutable GuarantRatio: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeCombLeg =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable CombInstrumentId: byte array

    [<DefaultValue>]
    val mutable LegId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable LegInstrumentId: byte array

    [<DefaultValue>]
    val mutable Direction: byte

    [<DefaultValue>]
    val mutable LegMultiple: int

    [<DefaultValue>]
    val mutable ImplyLevel: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeCombPromotionParam =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)>]
    [<DefaultValue>]
    val mutable CombHedgeFlag: byte array

    [<DefaultValue>]
    val mutable Xparameter: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeContractBank =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable BankId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)>]
    [<DefaultValue>]
    val mutable BankBrchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 101)>]
    [<DefaultValue>]
    val mutable BankName: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CsrcBankId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTraderDepthMarketData =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<DefaultValue>]
    val mutable LastPrice: float

    [<DefaultValue>]
    val mutable PreSettlementPrice: float

    [<DefaultValue>]
    val mutable PreClosePrice: float

    [<DefaultValue>]
    val mutable PreOpenInterest: float

    [<DefaultValue>]
    val mutable OpenPrice: float

    [<DefaultValue>]
    val mutable HighestPrice: float

    [<DefaultValue>]
    val mutable LowestPrice: float

    [<DefaultValue>]
    val mutable Volume: int

    [<DefaultValue>]
    val mutable Turnover: float

    [<DefaultValue>]
    val mutable OpenInterest: float

    [<DefaultValue>]
    val mutable ClosePrice: float

    [<DefaultValue>]
    val mutable SettlementPrice: float

    [<DefaultValue>]
    val mutable UpperLimitPrice: float

    [<DefaultValue>]
    val mutable LowerLimitPrice: float

    [<DefaultValue>]
    val mutable PreDelta: float

    [<DefaultValue>]
    val mutable CurrDelta: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable UpdateTime: byte array

    [<DefaultValue>]
    val mutable UpdateMillisec: int

    [<DefaultValue>]
    val mutable BidPrice1: float

    [<DefaultValue>]
    val mutable BidVolume1: int

    [<DefaultValue>]
    val mutable AskPrice1: float

    [<DefaultValue>]
    val mutable AskVolume1: int

    [<DefaultValue>]
    val mutable BidPrice2: float

    [<DefaultValue>]
    val mutable BidVolume2: int

    [<DefaultValue>]
    val mutable AskPrice2: float

    [<DefaultValue>]
    val mutable AskVolume2: int

    [<DefaultValue>]
    val mutable BidPrice3: float

    [<DefaultValue>]
    val mutable BidVolume3: int

    [<DefaultValue>]
    val mutable AskPrice3: float

    [<DefaultValue>]
    val mutable AskVolume3: int

    [<DefaultValue>]
    val mutable BidPrice4: float

    [<DefaultValue>]
    val mutable BidVolume4: int

    [<DefaultValue>]
    val mutable AskPrice4: float

    [<DefaultValue>]
    val mutable AskVolume4: int

    [<DefaultValue>]
    val mutable BidPrice5: float

    [<DefaultValue>]
    val mutable BidVolume5: int

    [<DefaultValue>]
    val mutable AskPrice5: float

    [<DefaultValue>]
    val mutable AskVolume5: int

    [<DefaultValue>]
    val mutable AveragePrice: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ActionDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

    [<DefaultValue>]
    val mutable BandingUpperPrice: float

    [<DefaultValue>]
    val mutable BandingLowerPrice: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeEWarrantOffset =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<DefaultValue>]
    val mutable Direction: byte

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<DefaultValue>]
    val mutable Volume: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeExchange =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 61)>]
    [<DefaultValue>]
    val mutable ExchangeName: byte array

    [<DefaultValue>]
    val mutable ExchangeProperty: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeExchangeMarginRateAdjust =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<DefaultValue>]
    val mutable LongMarginRatioByMoney: float

    [<DefaultValue>]
    val mutable LongMarginRatioByVolume: float

    [<DefaultValue>]
    val mutable ShortMarginRatioByMoney: float

    [<DefaultValue>]
    val mutable ShortMarginRatioByVolume: float

    [<DefaultValue>]
    val mutable ExchLongMarginRatioByMoney: float

    [<DefaultValue>]
    val mutable ExchLongMarginRatioByVolume: float

    [<DefaultValue>]
    val mutable ExchShortMarginRatioByMoney: float

    [<DefaultValue>]
    val mutable ExchShortMarginRatioByVolume: float

    [<DefaultValue>]
    val mutable NoLongMarginRatioByMoney: float

    [<DefaultValue>]
    val mutable NoLongMarginRatioByVolume: float

    [<DefaultValue>]
    val mutable NoShortMarginRatioByMoney: float

    [<DefaultValue>]
    val mutable NoShortMarginRatioByVolume: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeExchangeRate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable FromCurrencyId: byte array

    [<DefaultValue>]
    val mutable FromCurrencyUnit: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable ToCurrencyId: byte array

    [<DefaultValue>]
    val mutable ExchangeRate: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeExecOrder =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable ExecOrderRef: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable Volume: int

    [<DefaultValue>]
    val mutable RequestId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable BusinessUnit: byte array

    [<DefaultValue>]
    val mutable OffsetFlag: byte

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<DefaultValue>]
    val mutable ActionType: byte

    [<DefaultValue>]
    val mutable PosiDirection: byte

    [<DefaultValue>]
    val mutable ReservePositionFlag: byte

    [<DefaultValue>]
    val mutable CloseFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable ExecOrderLocalId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

    [<DefaultValue>]
    val mutable InstallId: int

    [<DefaultValue>]
    val mutable OrderSubmitStatus: byte

    [<DefaultValue>]
    val mutable NotifySequence: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<DefaultValue>]
    val mutable SettlementId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable ExecOrderSysId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable CancelTime: byte array

    [<DefaultValue>]
    val mutable ExecResult: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClearingPartId: byte array

    [<DefaultValue>]
    val mutable SequenceNo: int

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable UserProductInfo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable StatusMsg: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable ActiveUserId: byte array

    [<DefaultValue>]
    val mutable BrokerExecOrderSeq: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable BranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve3: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeExecOrderAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable ExecOrderActionRef: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable ExecOrderRef: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable ExecOrderSysId: byte array

    [<DefaultValue>]
    val mutable ActionFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ActionDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ActionTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

    [<DefaultValue>]
    val mutable InstallId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable ExecOrderLocalId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable ActionLocalId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable BusinessUnit: byte array

    [<DefaultValue>]
    val mutable OrderActionStatus: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable ActionType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable StatusMsg: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable BranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeFensUserInfo =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable LoginMode: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeForQuote =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable ForQuoteRef: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable ForQuoteLocalId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

    [<DefaultValue>]
    val mutable InstallId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertTime: byte array

    [<DefaultValue>]
    val mutable ForQuoteStatus: byte

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable StatusMsg: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable ActiveUserId: byte array

    [<DefaultValue>]
    val mutable BrokerForQutoSeq: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve3: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeForQuoteRsp =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable ForQuoteSysId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ForQuoteTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ActionDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeFrontInfo =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 101)>]
    [<DefaultValue>]
    val mutable FrontAddr: byte array

    [<DefaultValue>]
    val mutable QryFreq: int

    [<DefaultValue>]
    val mutable FtdPkgFreq: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeHedgeCfm =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable Volume: int

    [<DefaultValue>]
    val mutable Direction: byte

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderRef: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable ActiveUserId: byte array

    [<DefaultValue>]
    val mutable BrokerOrderSeq: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable OrderSysId: byte array

    [<DefaultValue>]
    val mutable ApplyStatus: byte

    [<DefaultValue>]
    val mutable SequenceNo: int

    [<DefaultValue>]
    val mutable DealVolume: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable CancelTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ReqDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderLocalId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

    [<DefaultValue>]
    val mutable InstallId: int

    [<DefaultValue>]
    val mutable OrderSubmitStatus: byte

    [<DefaultValue>]
    val mutable NotifySequence: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<DefaultValue>]
    val mutable SettlementId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable StatusMsg: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeHedgeCfmAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ActionDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ActionTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

    [<DefaultValue>]
    val mutable InstallId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderLocalId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable ActionLocalId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<DefaultValue>]
    val mutable OrderActionStatus: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable OrderSysId: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable StatusMsg: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderRef: byte array

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInputBatchOrderAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable OrderActionRef: int

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInputCombAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable CombActionRef: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable Direction: byte

    [<DefaultValue>]
    val mutable Volume: int

    [<DefaultValue>]
    val mutable CombDirection: byte

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInputExecOrder =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable ExecOrderRef: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable Volume: int

    [<DefaultValue>]
    val mutable RequestId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable BusinessUnit: byte array

    [<DefaultValue>]
    val mutable OffsetFlag: byte

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<DefaultValue>]
    val mutable ActionType: byte

    [<DefaultValue>]
    val mutable PosiDirection: byte

    [<DefaultValue>]
    val mutable ReservePositionFlag: byte

    [<DefaultValue>]
    val mutable CloseFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInputExecOrderAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable ExecOrderActionRef: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable ExecOrderRef: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable ExecOrderSysId: byte array

    [<DefaultValue>]
    val mutable ActionFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInputForQuote =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable ForQuoteRef: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInputHedgeCfm =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<DefaultValue>]
    val mutable Volume: int

    [<DefaultValue>]
    val mutable Direction: byte

    [<DefaultValue>]
    val mutable RequestId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderRef: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInputHedgeCfmAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable OrderSysId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderRef: byte array

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<DefaultValue>]
    val mutable RequestId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInputOffsetSetting =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable UnderlyingInstrId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

    [<DefaultValue>]
    val mutable OffsetType: byte

    [<DefaultValue>]
    val mutable Volume: int

    [<DefaultValue>]
    val mutable IsOffset: int

    [<DefaultValue>]
    val mutable RequestId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInputOptionSelfClose =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OptionSelfCloseRef: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable Volume: int

    [<DefaultValue>]
    val mutable RequestId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable BusinessUnit: byte array

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<DefaultValue>]
    val mutable OptSelfCloseFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInputOptionSelfCloseAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable OptionSelfCloseActionRef: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OptionSelfCloseRef: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable OptionSelfCloseSysId: byte array

    [<DefaultValue>]
    val mutable ActionFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInputQuote =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable QuoteRef: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable AskPrice: float

    [<DefaultValue>]
    val mutable BidPrice: float

    [<DefaultValue>]
    val mutable AskVolume: int

    [<DefaultValue>]
    val mutable BidVolume: int

    [<DefaultValue>]
    val mutable RequestId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable BusinessUnit: byte array

    [<DefaultValue>]
    val mutable AskOffsetFlag: byte

    [<DefaultValue>]
    val mutable BidOffsetFlag: byte

    [<DefaultValue>]
    val mutable AskHedgeFlag: byte

    [<DefaultValue>]
    val mutable BidHedgeFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AskOrderRef: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable BidOrderRef: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable ForQuoteSysId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable ReplaceSysId: byte array

    [<DefaultValue>]
    val mutable TimeCondition: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderMemo: byte array

    [<DefaultValue>]
    val mutable SessionReqSeq: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInputQuoteAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable QuoteActionRef: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable QuoteRef: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable QuoteSysId: byte array

    [<DefaultValue>]
    val mutable ActionFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderMemo: byte array

    [<DefaultValue>]
    val mutable SessionReqSeq: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInputSpdApply =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable FirstLegInstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable SecondLegInstrumentId: byte array

    [<DefaultValue>]
    val mutable Volume: int

    [<DefaultValue>]
    val mutable Direction: byte

    [<DefaultValue>]
    val mutable CmbType: byte

    [<DefaultValue>]
    val mutable RequestId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderRef: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInputSpdApplyAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable OrderSysId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderRef: byte array

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<DefaultValue>]
    val mutable RequestId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInstrument =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable InstrumentName: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve3: byte array

    [<DefaultValue>]
    val mutable ProductClass: byte

    [<DefaultValue>]
    val mutable DeliveryYear: int

    [<DefaultValue>]
    val mutable DeliveryMonth: int

    [<DefaultValue>]
    val mutable MaxMarketOrderVolume: int

    [<DefaultValue>]
    val mutable MinMarketOrderVolume: int

    [<DefaultValue>]
    val mutable MaxLimitOrderVolume: int

    [<DefaultValue>]
    val mutable MinLimitOrderVolume: int

    [<DefaultValue>]
    val mutable VolumeMultiple: int

    [<DefaultValue>]
    val mutable PriceTick: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable CreateDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable OpenDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExpireDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable StartDelivDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable EndDelivDate: byte array

    [<DefaultValue>]
    val mutable InstLifePhase: byte

    [<DefaultValue>]
    val mutable IsTrading: int

    [<DefaultValue>]
    val mutable PositionType: byte

    [<DefaultValue>]
    val mutable PositionDateType: byte

    [<DefaultValue>]
    val mutable LongMarginRatio: float

    [<DefaultValue>]
    val mutable ShortMarginRatio: float

    [<DefaultValue>]
    val mutable MaxMarginSideAlgorithm: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve4: byte array

    [<DefaultValue>]
    val mutable StrikePrice: float

    [<DefaultValue>]
    val mutable OptionsType: byte

    [<DefaultValue>]
    val mutable UnderlyingMultiple: float

    [<DefaultValue>]
    val mutable CombinationType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable UnderlyingInstrId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInstrumentOrderCommRate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<DefaultValue>]
    val mutable InvestorRange: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<DefaultValue>]
    val mutable OrderCommByVolume: float

    [<DefaultValue>]
    val mutable OrderActionCommByVolume: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<DefaultValue>]
    val mutable OrderCommByTrade: float

    [<DefaultValue>]
    val mutable OrderActionCommByTrade: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInvestUnit =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InvestorUnitName: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorGroupId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable CommModelId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable MarginModelId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInvestor =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorGroupId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InvestorName: byte array

    [<DefaultValue>]
    val mutable IdentifiedCardType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 51)>]
    [<DefaultValue>]
    val mutable IdentifiedCardNo: byte array

    [<DefaultValue>]
    val mutable IsActive: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Telephone: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 101)>]
    [<DefaultValue>]
    val mutable Address: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable OpenDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Mobile: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable CommModelId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable MarginModelId: byte array

    [<DefaultValue>]
    val mutable IsOrderFreq: byte

    [<DefaultValue>]
    val mutable IsOpenVolLimit: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInvestorCommodityGroupSpmmMargin =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable CommodityGroupId: byte array

    [<DefaultValue>]
    val mutable MarginBeforeDiscount: float

    [<DefaultValue>]
    val mutable MarginNoDiscount: float

    [<DefaultValue>]
    val mutable LongRisk: float

    [<DefaultValue>]
    val mutable ShortRisk: float

    [<DefaultValue>]
    val mutable CloseFrozenMargin: float

    [<DefaultValue>]
    val mutable InterCommodityRate: float

    [<DefaultValue>]
    val mutable MiniMarginRatio: float

    [<DefaultValue>]
    val mutable AdjustRatio: float

    [<DefaultValue>]
    val mutable IntraCommodityDiscount: float

    [<DefaultValue>]
    val mutable InterCommodityDiscount: float

    [<DefaultValue>]
    val mutable ExchMargin: float

    [<DefaultValue>]
    val mutable InvestorMargin: float

    [<DefaultValue>]
    val mutable FrozenCommission: float

    [<DefaultValue>]
    val mutable Commission: float

    [<DefaultValue>]
    val mutable FrozenCash: float

    [<DefaultValue>]
    val mutable CashIn: float

    [<DefaultValue>]
    val mutable StrikeFrozenMargin: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInvestorCommoditySpmmMargin =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable CommodityId: byte array

    [<DefaultValue>]
    val mutable MarginBeforeDiscount: float

    [<DefaultValue>]
    val mutable MarginNoDiscount: float

    [<DefaultValue>]
    val mutable LongPosRisk: float

    [<DefaultValue>]
    val mutable LongOpenFrozenRisk: float

    [<DefaultValue>]
    val mutable LongCloseFrozenRisk: float

    [<DefaultValue>]
    val mutable ShortPosRisk: float

    [<DefaultValue>]
    val mutable ShortOpenFrozenRisk: float

    [<DefaultValue>]
    val mutable ShortCloseFrozenRisk: float

    [<DefaultValue>]
    val mutable IntraCommodityRate: float

    [<DefaultValue>]
    val mutable OptionDiscountRate: float

    [<DefaultValue>]
    val mutable PosDiscount: float

    [<DefaultValue>]
    val mutable OpenFrozenDiscount: float

    [<DefaultValue>]
    val mutable NetRisk: float

    [<DefaultValue>]
    val mutable CloseFrozenMargin: float

    [<DefaultValue>]
    val mutable FrozenCommission: float

    [<DefaultValue>]
    val mutable Commission: float

    [<DefaultValue>]
    val mutable FrozenCash: float

    [<DefaultValue>]
    val mutable CashIn: float

    [<DefaultValue>]
    val mutable StrikeFrozenMargin: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInvestorInfoCommRec =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<DefaultValue>]
    val mutable OrderCount: int

    [<DefaultValue>]
    val mutable OrderActionCount: int

    [<DefaultValue>]
    val mutable ForQuoteCnt: int

    [<DefaultValue>]
    val mutable InfoComm: float

    [<DefaultValue>]
    val mutable IsOptSeries: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

    [<DefaultValue>]
    val mutable InfoCnt: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInvestorPortfMarginRatio =
    [<DefaultValue>]
    val mutable InvestorRange: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<DefaultValue>]
    val mutable MarginRatio: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable ProductGroupId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInvestorPortfSetting =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<DefaultValue>]
    val mutable UsePortf: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInvestorPositionCombineDetail =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable OpenDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<DefaultValue>]
    val mutable SettlementId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable ComTradeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TradeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<DefaultValue>]
    val mutable Direction: byte

    [<DefaultValue>]
    val mutable TotalAmt: int

    [<DefaultValue>]
    val mutable Margin: float

    [<DefaultValue>]
    val mutable ExchMargin: float

    [<DefaultValue>]
    val mutable MarginRateByMoney: float

    [<DefaultValue>]
    val mutable MarginRateByVolume: float

    [<DefaultValue>]
    val mutable LegId: int

    [<DefaultValue>]
    val mutable LegMultiple: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<DefaultValue>]
    val mutable TradeGroupId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable CombInstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInvestorPositionDetail =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<DefaultValue>]
    val mutable Direction: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable OpenDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TradeId: byte array

    [<DefaultValue>]
    val mutable Volume: int

    [<DefaultValue>]
    val mutable OpenPrice: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<DefaultValue>]
    val mutable SettlementId: int

    [<DefaultValue>]
    val mutable TradeType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<DefaultValue>]
    val mutable CloseProfitByDate: float

    [<DefaultValue>]
    val mutable CloseProfitByTrade: float

    [<DefaultValue>]
    val mutable PositionProfitByDate: float

    [<DefaultValue>]
    val mutable PositionProfitByTrade: float

    [<DefaultValue>]
    val mutable Margin: float

    [<DefaultValue>]
    val mutable ExchMargin: float

    [<DefaultValue>]
    val mutable MarginRateByMoney: float

    [<DefaultValue>]
    val mutable MarginRateByVolume: float

    [<DefaultValue>]
    val mutable LastSettlementPrice: float

    [<DefaultValue>]
    val mutable SettlementPrice: float

    [<DefaultValue>]
    val mutable CloseVolume: int

    [<DefaultValue>]
    val mutable CloseAmount: float

    [<DefaultValue>]
    val mutable TimeFirstVolume: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<DefaultValue>]
    val mutable SpecPosiType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable CombInstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInvestorProdRcamsMargin =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable CombProductId: byte array

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable ProductGroupId: byte array

    [<DefaultValue>]
    val mutable RiskBeforeDiscount: float

    [<DefaultValue>]
    val mutable IntraInstrRisk: float

    [<DefaultValue>]
    val mutable BPosRisk: float

    [<DefaultValue>]
    val mutable SPosRisk: float

    [<DefaultValue>]
    val mutable IntraProdRisk: float

    [<DefaultValue>]
    val mutable NetRisk: float

    [<DefaultValue>]
    val mutable InterProdRisk: float

    [<DefaultValue>]
    val mutable ShortOptRiskAdj: float

    [<DefaultValue>]
    val mutable OptionRoyalty: float

    [<DefaultValue>]
    val mutable MmsaCloseFrozenMargin: float

    [<DefaultValue>]
    val mutable CloseCombFrozenMargin: float

    [<DefaultValue>]
    val mutable CloseFrozenMargin: float

    [<DefaultValue>]
    val mutable MmsaOpenFrozenMargin: float

    [<DefaultValue>]
    val mutable DeliveryOpenFrozenMargin: float

    [<DefaultValue>]
    val mutable OpenFrozenMargin: float

    [<DefaultValue>]
    val mutable UseFrozenMargin: float

    [<DefaultValue>]
    val mutable MmsaExchMargin: float

    [<DefaultValue>]
    val mutable DeliveryExchMargin: float

    [<DefaultValue>]
    val mutable CombExchMargin: float

    [<DefaultValue>]
    val mutable ExchMargin: float

    [<DefaultValue>]
    val mutable UseMargin: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInvestorProdRuleMargin =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProdFamilyCode: byte array

    [<DefaultValue>]
    val mutable InstrumentClass: byte

    [<DefaultValue>]
    val mutable CommodityGroupId: int

    [<DefaultValue>]
    val mutable BStdPosition: float

    [<DefaultValue>]
    val mutable SStdPosition: float

    [<DefaultValue>]
    val mutable BStdOpenFrozen: float

    [<DefaultValue>]
    val mutable SStdOpenFrozen: float

    [<DefaultValue>]
    val mutable BStdCloseFrozen: float

    [<DefaultValue>]
    val mutable SStdCloseFrozen: float

    [<DefaultValue>]
    val mutable IntraProdStdPosition: float

    [<DefaultValue>]
    val mutable NetStdPosition: float

    [<DefaultValue>]
    val mutable InterProdStdPosition: float

    [<DefaultValue>]
    val mutable SingleStdPosition: float

    [<DefaultValue>]
    val mutable IntraProdMargin: float

    [<DefaultValue>]
    val mutable InterProdMargin: float

    [<DefaultValue>]
    val mutable SingleMargin: float

    [<DefaultValue>]
    val mutable NonCombMargin: float

    [<DefaultValue>]
    val mutable AddOnMargin: float

    [<DefaultValue>]
    val mutable ExchMargin: float

    [<DefaultValue>]
    val mutable AddOnFrozenMargin: float

    [<DefaultValue>]
    val mutable OpenFrozenMargin: float

    [<DefaultValue>]
    val mutable CloseFrozenMargin: float

    [<DefaultValue>]
    val mutable Margin: float

    [<DefaultValue>]
    val mutable FrozenMargin: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInvestorProdSpbmDetail =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProdFamilyCode: byte array

    [<DefaultValue>]
    val mutable IntraInstrMargin: float

    [<DefaultValue>]
    val mutable BCollectingMargin: float

    [<DefaultValue>]
    val mutable SCollectingMargin: float

    [<DefaultValue>]
    val mutable IntraProdMargin: float

    [<DefaultValue>]
    val mutable NetMargin: float

    [<DefaultValue>]
    val mutable InterProdMargin: float

    [<DefaultValue>]
    val mutable SingleMargin: float

    [<DefaultValue>]
    val mutable AddOnMargin: float

    [<DefaultValue>]
    val mutable DeliveryMargin: float

    [<DefaultValue>]
    val mutable CallOptionMinRisk: float

    [<DefaultValue>]
    val mutable PutOptionMinRisk: float

    [<DefaultValue>]
    val mutable OptionMinRisk: float

    [<DefaultValue>]
    val mutable OptionValueOffset: float

    [<DefaultValue>]
    val mutable OptionRoyalty: float

    [<DefaultValue>]
    val mutable RealOptionValueOffset: float

    [<DefaultValue>]
    val mutable Margin: float

    [<DefaultValue>]
    val mutable ExchMargin: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInvestorProductGroupMargin =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<DefaultValue>]
    val mutable SettlementId: int

    [<DefaultValue>]
    val mutable FrozenMargin: float

    [<DefaultValue>]
    val mutable LongFrozenMargin: float

    [<DefaultValue>]
    val mutable ShortFrozenMargin: float

    [<DefaultValue>]
    val mutable UseMargin: float

    [<DefaultValue>]
    val mutable LongUseMargin: float

    [<DefaultValue>]
    val mutable ShortUseMargin: float

    [<DefaultValue>]
    val mutable ExchMargin: float

    [<DefaultValue>]
    val mutable LongExchMargin: float

    [<DefaultValue>]
    val mutable ShortExchMargin: float

    [<DefaultValue>]
    val mutable CloseProfit: float

    [<DefaultValue>]
    val mutable FrozenCommission: float

    [<DefaultValue>]
    val mutable Commission: float

    [<DefaultValue>]
    val mutable FrozenCash: float

    [<DefaultValue>]
    val mutable CashIn: float

    [<DefaultValue>]
    val mutable PositionProfit: float

    [<DefaultValue>]
    val mutable OffsetAmount: float

    [<DefaultValue>]
    val mutable LongOffsetAmount: float

    [<DefaultValue>]
    val mutable ShortOffsetAmount: float

    [<DefaultValue>]
    val mutable ExchOffsetAmount: float

    [<DefaultValue>]
    val mutable LongExchOffsetAmount: float

    [<DefaultValue>]
    val mutable ShortExchOffsetAmount: float

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProductGroupId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeMmInstrumentCommissionRate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<DefaultValue>]
    val mutable InvestorRange: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable OpenRatioByMoney: float

    [<DefaultValue>]
    val mutable OpenRatioByVolume: float

    [<DefaultValue>]
    val mutable CloseRatioByMoney: float

    [<DefaultValue>]
    val mutable CloseRatioByVolume: float

    [<DefaultValue>]
    val mutable CloseTodayRatioByMoney: float

    [<DefaultValue>]
    val mutable CloseTodayRatioByVolume: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeMmOptionInstrCommRate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<DefaultValue>]
    val mutable InvestorRange: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable OpenRatioByMoney: float

    [<DefaultValue>]
    val mutable OpenRatioByVolume: float

    [<DefaultValue>]
    val mutable CloseRatioByMoney: float

    [<DefaultValue>]
    val mutable CloseRatioByVolume: float

    [<DefaultValue>]
    val mutable CloseTodayRatioByMoney: float

    [<DefaultValue>]
    val mutable CloseTodayRatioByVolume: float

    [<DefaultValue>]
    val mutable StrikeRatioByMoney: float

    [<DefaultValue>]
    val mutable StrikeRatioByVolume: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeNotice =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 501)>]
    [<DefaultValue>]
    val mutable Content: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)>]
    [<DefaultValue>]
    val mutable SequenceLabel: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeNotifyQueryAccount =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)>]
    [<DefaultValue>]
    val mutable TradeCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable BankId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)>]
    [<DefaultValue>]
    val mutable BankBranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable BrokerBranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradeDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradeTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable BankSerial: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<DefaultValue>]
    val mutable PlateSerial: int

    [<DefaultValue>]
    val mutable LastFragment: byte

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 51)>]
    [<DefaultValue>]
    val mutable CustomerName: byte array

    [<DefaultValue>]
    val mutable IdCardType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 51)>]
    [<DefaultValue>]
    val mutable IdentifiedCardNo: byte array

    [<DefaultValue>]
    val mutable CustType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BankAccount: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BankPassWord: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Password: byte array

    [<DefaultValue>]
    val mutable FutureSerial: int

    [<DefaultValue>]
    val mutable InstallId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable VerifyCertNoFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)>]
    [<DefaultValue>]
    val mutable Digest: byte array

    [<DefaultValue>]
    val mutable BankAccType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)>]
    [<DefaultValue>]
    val mutable DeviceId: byte array

    [<DefaultValue>]
    val mutable BankSecuAccType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable BrokerIdByBank: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BankSecuAcc: byte array

    [<DefaultValue>]
    val mutable BankPwdFlag: byte

    [<DefaultValue>]
    val mutable SecuPwdFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable OperNo: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable TId: int

    [<DefaultValue>]
    val mutable BankUseAmount: float

    [<DefaultValue>]
    val mutable BankFetchAmount: float

    [<DefaultValue>]
    val mutable ErrorId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ErrorMsg: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 161)>]
    [<DefaultValue>]
    val mutable LongCustomerName: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeOffsetSetting =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable UnderlyingInstrId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

    [<DefaultValue>]
    val mutable OffsetType: byte

    [<DefaultValue>]
    val mutable Volume: int

    [<DefaultValue>]
    val mutable IsOffset: int

    [<DefaultValue>]
    val mutable RequestId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeSerialNo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable ExchangeProductId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

    [<DefaultValue>]
    val mutable InstallId: int

    [<DefaultValue>]
    val mutable OrderSubmitStatus: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<DefaultValue>]
    val mutable SettlementId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable CancelTime: byte array

    [<DefaultValue>]
    val mutable ExecResult: byte

    [<DefaultValue>]
    val mutable SequenceNo: int

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable StatusMsg: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable ActiveUserId: byte array

    [<DefaultValue>]
    val mutable BrokerOffsetSettingSeq: int

    [<DefaultValue>]
    val mutable ApplySrc: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeOptionInstrCommRate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<DefaultValue>]
    val mutable InvestorRange: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable OpenRatioByMoney: float

    [<DefaultValue>]
    val mutable OpenRatioByVolume: float

    [<DefaultValue>]
    val mutable CloseRatioByMoney: float

    [<DefaultValue>]
    val mutable CloseRatioByVolume: float

    [<DefaultValue>]
    val mutable CloseTodayRatioByMoney: float

    [<DefaultValue>]
    val mutable CloseTodayRatioByVolume: float

    [<DefaultValue>]
    val mutable StrikeRatioByMoney: float

    [<DefaultValue>]
    val mutable StrikeRatioByVolume: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeOptionInstrTradeCost =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<DefaultValue>]
    val mutable FixedMargin: float

    [<DefaultValue>]
    val mutable MiniMargin: float

    [<DefaultValue>]
    val mutable Royalty: float

    [<DefaultValue>]
    val mutable ExchFixedMargin: float

    [<DefaultValue>]
    val mutable ExchMiniMargin: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeOptionSelfClose =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OptionSelfCloseRef: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable Volume: int

    [<DefaultValue>]
    val mutable RequestId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable BusinessUnit: byte array

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<DefaultValue>]
    val mutable OptSelfCloseFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OptionSelfCloseLocalId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

    [<DefaultValue>]
    val mutable InstallId: int

    [<DefaultValue>]
    val mutable OrderSubmitStatus: byte

    [<DefaultValue>]
    val mutable NotifySequence: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<DefaultValue>]
    val mutable SettlementId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable OptionSelfCloseSysId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable CancelTime: byte array

    [<DefaultValue>]
    val mutable ExecResult: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClearingPartId: byte array

    [<DefaultValue>]
    val mutable SequenceNo: int

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable UserProductInfo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable StatusMsg: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable ActiveUserId: byte array

    [<DefaultValue>]
    val mutable BrokerOptionSelfCloseSeq: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable BranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve3: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeOptionSelfCloseAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable OptionSelfCloseActionRef: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OptionSelfCloseRef: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable OptionSelfCloseSysId: byte array

    [<DefaultValue>]
    val mutable ActionFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ActionDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ActionTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

    [<DefaultValue>]
    val mutable InstallId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OptionSelfCloseLocalId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable ActionLocalId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable BusinessUnit: byte array

    [<DefaultValue>]
    val mutable OrderActionStatus: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable StatusMsg: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable BranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeOrderAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable OrderActionRef: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderRef: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable OrderSysId: byte array

    [<DefaultValue>]
    val mutable ActionFlag: byte

    [<DefaultValue>]
    val mutable LimitPrice: float

    [<DefaultValue>]
    val mutable VolumeChange: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ActionDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ActionTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

    [<DefaultValue>]
    val mutable InstallId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderLocalId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable ActionLocalId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable BusinessUnit: byte array

    [<DefaultValue>]
    val mutable OrderActionStatus: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable StatusMsg: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable BranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderMemo: byte array

    [<DefaultValue>]
    val mutable SessionReqSeq: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeParkedOrder =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderRef: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable OrderPriceType: byte

    [<DefaultValue>]
    val mutable Direction: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)>]
    [<DefaultValue>]
    val mutable CombOffsetFlag: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)>]
    [<DefaultValue>]
    val mutable CombHedgeFlag: byte array

    [<DefaultValue>]
    val mutable LimitPrice: float

    [<DefaultValue>]
    val mutable VolumeTotalOriginal: int

    [<DefaultValue>]
    val mutable TimeCondition: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable GtdDate: byte array

    [<DefaultValue>]
    val mutable VolumeCondition: byte

    [<DefaultValue>]
    val mutable MinVolume: int

    [<DefaultValue>]
    val mutable ContingentCondition: byte

    [<DefaultValue>]
    val mutable StopPrice: float

    [<DefaultValue>]
    val mutable ForceCloseReason: byte

    [<DefaultValue>]
    val mutable IsAutoSuspend: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable BusinessUnit: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable UserForceClose: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable ParkedOrderId: byte array

    [<DefaultValue>]
    val mutable UserType: byte

    [<DefaultValue>]
    val mutable Status: byte

    [<DefaultValue>]
    val mutable ErrorId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ErrorMsg: byte array

    [<DefaultValue>]
    val mutable IsSwapOrder: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeParkedOrderAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable OrderActionRef: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderRef: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable OrderSysId: byte array

    [<DefaultValue>]
    val mutable ActionFlag: byte

    [<DefaultValue>]
    val mutable LimitPrice: float

    [<DefaultValue>]
    val mutable VolumeChange: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable ParkedOrderActionId: byte array

    [<DefaultValue>]
    val mutable UserType: byte

    [<DefaultValue>]
    val mutable Status: byte

    [<DefaultValue>]
    val mutable ErrorId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ErrorMsg: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeProduct =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable ProductName: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<DefaultValue>]
    val mutable ProductClass: byte

    [<DefaultValue>]
    val mutable VolumeMultiple: int

    [<DefaultValue>]
    val mutable PriceTick: float

    [<DefaultValue>]
    val mutable MaxMarketOrderVolume: int

    [<DefaultValue>]
    val mutable MinMarketOrderVolume: int

    [<DefaultValue>]
    val mutable MaxLimitOrderVolume: int

    [<DefaultValue>]
    val mutable MinLimitOrderVolume: int

    [<DefaultValue>]
    val mutable PositionType: byte

    [<DefaultValue>]
    val mutable PositionDateType: byte

    [<DefaultValue>]
    val mutable CloseDealType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable TradeCurrencyId: byte array

    [<DefaultValue>]
    val mutable MortgageFundUseRange: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<DefaultValue>]
    val mutable UnderlyingMultiple: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeProductId: byte array

    [<DefaultValue>]
    val mutable OpenLimitControlLevel: byte

    [<DefaultValue>]
    val mutable OrderFreqControlLevel: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeProductExchRate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable QuoteCurrencyId: byte array

    [<DefaultValue>]
    val mutable ExchangeRate: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeProductGroup =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProductGroupId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryAccountregister =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable BankId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)>]
    [<DefaultValue>]
    val mutable BankBranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryBrokerTradingAlgos =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryBrokerTradingParams =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryCfmmcTradingAccountKey =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryClassifiedInstrument =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

    [<DefaultValue>]
    val mutable TradingType: byte

    [<DefaultValue>]
    val mutable ClassType: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryCombAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryCombInstrumentGuard =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryCombLeg =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable LegInstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryCombPromotionParam =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryContractBank =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable BankId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)>]
    [<DefaultValue>]
    val mutable BankBrchId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryDepthMarketData =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<DefaultValue>]
    val mutable ProductClass: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryEWarrantOffset =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryExchange =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryExchangeMarginRateAdjust =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryExchangeRate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable FromCurrencyId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable ToCurrencyId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryExecOrder =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable ExecOrderSysId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertTimeStart: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertTimeEnd: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryForQuote =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertTimeStart: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertTimeEnd: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryHedgeCfm =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable OrderSysId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryInstrument =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve3: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryInstrumentOrderCommRate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryInvestUnit =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryInvestor =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryInvestorCommodityGroupSpmmMargin =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable CommodityGroupId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryInvestorCommoditySpmmMargin =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable CommodityId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryInvestorInfoCommRec =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryInvestorPortfMarginRatio =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable ProductGroupId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryInvestorPortfSetting =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryInvestorPositionCombineDetail =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable CombInstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryInvestorPositionDetail =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryInvestorProdRcamsMargin =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable CombProductId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable ProductGroupId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryInvestorProdRuleMargin =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProdFamilyCode: byte array

    [<DefaultValue>]
    val mutable CommodityGroupId: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryInvestorProdSpbmDetail =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProdFamilyCode: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryInvestorProductGroupMargin =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProductGroupId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryMaxOrderVolume =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<DefaultValue>]
    val mutable Direction: byte

    [<DefaultValue>]
    val mutable OffsetFlag: byte

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<DefaultValue>]
    val mutable MaxVolume: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryMmInstrumentCommissionRate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryMmOptionInstrCommRate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryNotice =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryOffsetSetting =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

    [<DefaultValue>]
    val mutable OffsetType: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryOptionInstrCommRate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryOptionInstrTradeCost =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<DefaultValue>]
    val mutable InputPrice: float

    [<DefaultValue>]
    val mutable UnderlyingPrice: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryOptionSelfClose =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable OptionSelfCloseSysId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertTimeStart: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertTimeEnd: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryOrder =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable OrderSysId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertTimeStart: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertTimeEnd: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryParkedOrder =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryParkedOrderAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryProduct =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<DefaultValue>]
    val mutable ProductClass: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryProductExchRate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryProductGroup =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryQuote =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable QuoteSysId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertTimeStart: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertTimeEnd: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryRcamsCombProductInfo =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable CombProductId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable ProductGroupId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryRcamsInstrParameter =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryRcamsInterParameter =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable ProductGroupId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable CombProduct1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable CombProduct2: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryRcamsIntraParameter =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable CombProductId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryRcamsInvestorCombPosition =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable CombInstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryRcamsShortOptAdjustParam =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable CombProductId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryRiskSettleInvstPosition =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryRiskSettleProductStatus =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryRuleInstrParameter =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryRuleInterParameter =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable Leg1ProdFamilyCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable Leg2ProdFamilyCode: byte array

    [<DefaultValue>]
    val mutable CommodityGroupId: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryRuleIntraParameter =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProdFamilyCode: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQrySecAgentAcIdMap =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQrySecAgentCheckMode =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQrySecAgentTradeInfo =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable BrokerSecAgentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQrySettlementInfo =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQrySettlementInfoConfirm =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQrySpbmAddOnInterParameter =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable Leg1ProdFamilyCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable Leg2ProdFamilyCode: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQrySpbmFutureParameter =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProdFamilyCode: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQrySpbmInterParameter =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable Leg1ProdFamilyCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable Leg2ProdFamilyCode: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQrySpbmIntraParameter =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProdFamilyCode: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQrySpbmInvestorPortfDef =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQrySpbmOptionParameter =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProdFamilyCode: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQrySpbmPortfDefinition =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<DefaultValue>]
    val mutable PortfolioDefId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProdFamilyCode: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQrySpdApply =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable OrderSysId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable FirstLegInstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable SecondLegInstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQrySpmmInstParam =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQrySpmmProductParam =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryTrade =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TradeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradeTimeStart: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradeTimeEnd: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryTraderOffer =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryTradingCode =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<DefaultValue>]
    val mutable ClientIdType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryTradingNotice =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryTransferBank =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable BankId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)>]
    [<DefaultValue>]
    val mutable BankBrchId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryTransferSerial =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable BankId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryUserSession =
    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQueryCfmmcTradingAccountToken =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQuote =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable QuoteRef: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable AskPrice: float

    [<DefaultValue>]
    val mutable BidPrice: float

    [<DefaultValue>]
    val mutable AskVolume: int

    [<DefaultValue>]
    val mutable BidVolume: int

    [<DefaultValue>]
    val mutable RequestId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable BusinessUnit: byte array

    [<DefaultValue>]
    val mutable AskOffsetFlag: byte

    [<DefaultValue>]
    val mutable BidOffsetFlag: byte

    [<DefaultValue>]
    val mutable AskHedgeFlag: byte

    [<DefaultValue>]
    val mutable BidHedgeFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable QuoteLocalId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

    [<DefaultValue>]
    val mutable InstallId: int

    [<DefaultValue>]
    val mutable NotifySequence: int

    [<DefaultValue>]
    val mutable OrderSubmitStatus: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<DefaultValue>]
    val mutable SettlementId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable QuoteSysId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable CancelTime: byte array

    [<DefaultValue>]
    val mutable QuoteStatus: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClearingPartId: byte array

    [<DefaultValue>]
    val mutable SequenceNo: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable AskOrderSysId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable BidOrderSysId: byte array

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable UserProductInfo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable StatusMsg: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable ActiveUserId: byte array

    [<DefaultValue>]
    val mutable BrokerQuoteSeq: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AskOrderRef: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable BidOrderRef: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable ForQuoteSysId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable BranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve3: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable ReplaceSysId: byte array

    [<DefaultValue>]
    val mutable TimeCondition: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderMemo: byte array

    [<DefaultValue>]
    val mutable SessionReqSeq: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQuoteAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable QuoteActionRef: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable QuoteRef: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable QuoteSysId: byte array

    [<DefaultValue>]
    val mutable ActionFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ActionDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ActionTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

    [<DefaultValue>]
    val mutable InstallId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable QuoteLocalId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable ActionLocalId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable BusinessUnit: byte array

    [<DefaultValue>]
    val mutable OrderActionStatus: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable StatusMsg: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable BranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderMemo: byte array

    [<DefaultValue>]
    val mutable SessionReqSeq: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRcamsCombProductInfo =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable CombProductId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable ProductGroupId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRcamsInstrParameter =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

    [<DefaultValue>]
    val mutable HedgeRate: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRcamsInterParameter =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable ProductGroupId: byte array

    [<DefaultValue>]
    val mutable Priority: int

    [<DefaultValue>]
    val mutable CreditRate: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable CombProduct1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable CombProduct2: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRcamsIntraParameter =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable CombProductId: byte array

    [<DefaultValue>]
    val mutable HedgeRate: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRcamsInvestorCombPosition =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<DefaultValue>]
    val mutable PosiDirection: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable CombInstrumentId: byte array

    [<DefaultValue>]
    val mutable LegId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

    [<DefaultValue>]
    val mutable TotalAmt: int

    [<DefaultValue>]
    val mutable ExchMargin: float

    [<DefaultValue>]
    val mutable Margin: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRcamsShortOptAdjustParam =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable CombProductId: byte array

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<DefaultValue>]
    val mutable AdjustValue: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRemoveParkedOrder =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable ParkedOrderId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRemoveParkedOrderAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable ParkedOrderActionId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeReqGenSmsCode =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable Mobile: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeReqGenUserCaptcha =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeReqGenUserText =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeReqQueryAccount =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)>]
    [<DefaultValue>]
    val mutable TradeCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable BankId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)>]
    [<DefaultValue>]
    val mutable BankBranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable BrokerBranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradeDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradeTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable BankSerial: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<DefaultValue>]
    val mutable PlateSerial: int

    [<DefaultValue>]
    val mutable LastFragment: byte

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 51)>]
    [<DefaultValue>]
    val mutable CustomerName: byte array

    [<DefaultValue>]
    val mutable IdCardType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 51)>]
    [<DefaultValue>]
    val mutable IdentifiedCardNo: byte array

    [<DefaultValue>]
    val mutable CustType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BankAccount: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BankPassWord: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Password: byte array

    [<DefaultValue>]
    val mutable FutureSerial: int

    [<DefaultValue>]
    val mutable InstallId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable VerifyCertNoFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)>]
    [<DefaultValue>]
    val mutable Digest: byte array

    [<DefaultValue>]
    val mutable BankAccType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)>]
    [<DefaultValue>]
    val mutable DeviceId: byte array

    [<DefaultValue>]
    val mutable BankSecuAccType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable BrokerIdByBank: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BankSecuAcc: byte array

    [<DefaultValue>]
    val mutable BankPwdFlag: byte

    [<DefaultValue>]
    val mutable SecuPwdFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable OperNo: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable TId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 161)>]
    [<DefaultValue>]
    val mutable LongCustomerName: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeReqTransfer =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)>]
    [<DefaultValue>]
    val mutable TradeCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable BankId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)>]
    [<DefaultValue>]
    val mutable BankBranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable BrokerBranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradeDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradeTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable BankSerial: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<DefaultValue>]
    val mutable PlateSerial: int

    [<DefaultValue>]
    val mutable LastFragment: byte

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 51)>]
    [<DefaultValue>]
    val mutable CustomerName: byte array

    [<DefaultValue>]
    val mutable IdCardType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 51)>]
    [<DefaultValue>]
    val mutable IdentifiedCardNo: byte array

    [<DefaultValue>]
    val mutable CustType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BankAccount: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BankPassWord: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Password: byte array

    [<DefaultValue>]
    val mutable InstallId: int

    [<DefaultValue>]
    val mutable FutureSerial: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable VerifyCertNoFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<DefaultValue>]
    val mutable TradeAmount: float

    [<DefaultValue>]
    val mutable FutureFetchAmount: float

    [<DefaultValue>]
    val mutable FeePayFlag: byte

    [<DefaultValue>]
    val mutable CustFee: float

    [<DefaultValue>]
    val mutable BrokerFee: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)>]
    [<DefaultValue>]
    val mutable Message: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)>]
    [<DefaultValue>]
    val mutable Digest: byte array

    [<DefaultValue>]
    val mutable BankAccType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)>]
    [<DefaultValue>]
    val mutable DeviceId: byte array

    [<DefaultValue>]
    val mutable BankSecuAccType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable BrokerIdByBank: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BankSecuAcc: byte array

    [<DefaultValue>]
    val mutable BankPwdFlag: byte

    [<DefaultValue>]
    val mutable SecuPwdFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable OperNo: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable TId: int

    [<DefaultValue>]
    val mutable TransferStatus: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 161)>]
    [<DefaultValue>]
    val mutable LongCustomerName: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeReqUserAuthMethod =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeReqUserLoginWithCaptcha =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Password: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable UserProductInfo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable InterfaceProductInfo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ProtocolInfo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)>]
    [<DefaultValue>]
    val mutable LoginRemark: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Captcha: byte array

    [<DefaultValue>]
    val mutable ClientIpPort: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable ClientIpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeReqUserLoginWithOtp =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Password: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable UserProductInfo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable InterfaceProductInfo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ProtocolInfo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)>]
    [<DefaultValue>]
    val mutable LoginRemark: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable OtpPassword: byte array

    [<DefaultValue>]
    val mutable ClientIpPort: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable ClientIpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeReqUserLoginWithText =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Password: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable UserProductInfo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable InterfaceProductInfo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ProtocolInfo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)>]
    [<DefaultValue>]
    val mutable LoginRemark: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Text: byte array

    [<DefaultValue>]
    val mutable ClientIpPort: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable ClientIpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRiskSettleInvstPosition =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable PosiDirection: byte

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<DefaultValue>]
    val mutable PositionDate: byte

    [<DefaultValue>]
    val mutable YdPosition: int

    [<DefaultValue>]
    val mutable Position: int

    [<DefaultValue>]
    val mutable LongFrozen: int

    [<DefaultValue>]
    val mutable ShortFrozen: int

    [<DefaultValue>]
    val mutable LongFrozenAmount: float

    [<DefaultValue>]
    val mutable ShortFrozenAmount: float

    [<DefaultValue>]
    val mutable OpenVolume: int

    [<DefaultValue>]
    val mutable CloseVolume: int

    [<DefaultValue>]
    val mutable OpenAmount: float

    [<DefaultValue>]
    val mutable CloseAmount: float

    [<DefaultValue>]
    val mutable PositionCost: float

    [<DefaultValue>]
    val mutable PreMargin: float

    [<DefaultValue>]
    val mutable UseMargin: float

    [<DefaultValue>]
    val mutable FrozenMargin: float

    [<DefaultValue>]
    val mutable FrozenCash: float

    [<DefaultValue>]
    val mutable FrozenCommission: float

    [<DefaultValue>]
    val mutable CashIn: float

    [<DefaultValue>]
    val mutable Commission: float

    [<DefaultValue>]
    val mutable CloseProfit: float

    [<DefaultValue>]
    val mutable PositionProfit: float

    [<DefaultValue>]
    val mutable PreSettlementPrice: float

    [<DefaultValue>]
    val mutable SettlementPrice: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<DefaultValue>]
    val mutable SettlementId: int

    [<DefaultValue>]
    val mutable OpenCost: float

    [<DefaultValue>]
    val mutable ExchangeMargin: float

    [<DefaultValue>]
    val mutable CombPosition: int

    [<DefaultValue>]
    val mutable CombLongFrozen: int

    [<DefaultValue>]
    val mutable CombShortFrozen: int

    [<DefaultValue>]
    val mutable CloseProfitByDate: float

    [<DefaultValue>]
    val mutable CloseProfitByTrade: float

    [<DefaultValue>]
    val mutable TodayPosition: int

    [<DefaultValue>]
    val mutable MarginRateByMoney: float

    [<DefaultValue>]
    val mutable MarginRateByVolume: float

    [<DefaultValue>]
    val mutable StrikeFrozen: int

    [<DefaultValue>]
    val mutable StrikeFrozenAmount: float

    [<DefaultValue>]
    val mutable AbandonFrozen: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<DefaultValue>]
    val mutable YdStrikeFrozen: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

    [<DefaultValue>]
    val mutable PositionCostOffset: float

    [<DefaultValue>]
    val mutable TasPosition: int

    [<DefaultValue>]
    val mutable TasPositionCost: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRiskSettleProductStatus =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

    [<DefaultValue>]
    val mutable ProductStatus: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRspGenSmsCode =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable GenTime: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRspGenUserCaptcha =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable CaptchaInfoLen: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 2561)>]
    [<DefaultValue>]
    val mutable CaptchaInfo: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRspGenUserText =
    [<DefaultValue>]
    val mutable UserTextSeq: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRspTransfer =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)>]
    [<DefaultValue>]
    val mutable TradeCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable BankId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)>]
    [<DefaultValue>]
    val mutable BankBranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable BrokerBranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradeDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradeTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable BankSerial: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<DefaultValue>]
    val mutable PlateSerial: int

    [<DefaultValue>]
    val mutable LastFragment: byte

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 51)>]
    [<DefaultValue>]
    val mutable CustomerName: byte array

    [<DefaultValue>]
    val mutable IdCardType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 51)>]
    [<DefaultValue>]
    val mutable IdentifiedCardNo: byte array

    [<DefaultValue>]
    val mutable CustType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BankAccount: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BankPassWord: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Password: byte array

    [<DefaultValue>]
    val mutable InstallId: int

    [<DefaultValue>]
    val mutable FutureSerial: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable VerifyCertNoFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<DefaultValue>]
    val mutable TradeAmount: float

    [<DefaultValue>]
    val mutable FutureFetchAmount: float

    [<DefaultValue>]
    val mutable FeePayFlag: byte

    [<DefaultValue>]
    val mutable CustFee: float

    [<DefaultValue>]
    val mutable BrokerFee: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)>]
    [<DefaultValue>]
    val mutable Message: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)>]
    [<DefaultValue>]
    val mutable Digest: byte array

    [<DefaultValue>]
    val mutable BankAccType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)>]
    [<DefaultValue>]
    val mutable DeviceId: byte array

    [<DefaultValue>]
    val mutable BankSecuAccType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable BrokerIdByBank: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BankSecuAcc: byte array

    [<DefaultValue>]
    val mutable BankPwdFlag: byte

    [<DefaultValue>]
    val mutable SecuPwdFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable OperNo: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable TId: int

    [<DefaultValue>]
    val mutable TransferStatus: byte

    [<DefaultValue>]
    val mutable ErrorId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ErrorMsg: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 161)>]
    [<DefaultValue>]
    val mutable LongCustomerName: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRspUserAuthMethod =
    [<DefaultValue>]
    val mutable UsableAuthMethod: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRuleInstrParameter =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<DefaultValue>]
    val mutable InstrumentClass: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable StdInstrumentId: byte array

    [<DefaultValue>]
    val mutable BSpecRatio: float

    [<DefaultValue>]
    val mutable SSpecRatio: float

    [<DefaultValue>]
    val mutable BHedgeRatio: float

    [<DefaultValue>]
    val mutable SHedgeRatio: float

    [<DefaultValue>]
    val mutable BAddOnMargin: float

    [<DefaultValue>]
    val mutable SAddOnMargin: float

    [<DefaultValue>]
    val mutable CommodityGroupId: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRuleInterParameter =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<DefaultValue>]
    val mutable SpreadId: int

    [<DefaultValue>]
    val mutable InterRate: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable Leg1ProdFamilyCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable Leg2ProdFamilyCode: byte array

    [<DefaultValue>]
    val mutable Leg1PropFactor: int

    [<DefaultValue>]
    val mutable Leg2PropFactor: int

    [<DefaultValue>]
    val mutable CommodityGroupId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable CommodityGroupName: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRuleIntraParameter =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProdFamilyCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable StdInstrumentId: byte array

    [<DefaultValue>]
    val mutable StdInstrMargin: float

    [<DefaultValue>]
    val mutable UsualIntraRate: float

    [<DefaultValue>]
    val mutable DeliveryIntraRate: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSecAgentAcIdMap =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable BrokerSecAgentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSecAgentCheckMode =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable BrokerSecAgentId: byte array

    [<DefaultValue>]
    val mutable CheckSelfAccount: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSecAgentTradeInfo =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable BrokerSecAgentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 161)>]
    [<DefaultValue>]
    val mutable LongCustomerName: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSettlementInfo =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<DefaultValue>]
    val mutable SettlementId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable SequenceNo: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 501)>]
    [<DefaultValue>]
    val mutable Content: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSpbmAddOnInterParameter =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<DefaultValue>]
    val mutable SpreadId: int

    [<DefaultValue>]
    val mutable AddOnInterRateZ2: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable Leg1ProdFamilyCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable Leg2ProdFamilyCode: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSpbmFutureParameter =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProdFamilyCode: byte array

    [<DefaultValue>]
    val mutable Cvf: int

    [<DefaultValue>]
    val mutable TimeRange: byte

    [<DefaultValue>]
    val mutable MarginRate: float

    [<DefaultValue>]
    val mutable LockRateX: float

    [<DefaultValue>]
    val mutable AddOnRate: float

    [<DefaultValue>]
    val mutable PreSettlementPrice: float

    [<DefaultValue>]
    val mutable AddOnLockRateX2: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSpbmInterParameter =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<DefaultValue>]
    val mutable SpreadId: int

    [<DefaultValue>]
    val mutable InterRateZ: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable Leg1ProdFamilyCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable Leg2ProdFamilyCode: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSpbmIntraParameter =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProdFamilyCode: byte array

    [<DefaultValue>]
    val mutable IntraRateY: float

    [<DefaultValue>]
    val mutable AddOnIntraRateY2: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSpbmInvestorPortfDef =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable PortfolioDefId: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSpbmOptionParameter =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProdFamilyCode: byte array

    [<DefaultValue>]
    val mutable Cvf: int

    [<DefaultValue>]
    val mutable DownPrice: float

    [<DefaultValue>]
    val mutable Delta: float

    [<DefaultValue>]
    val mutable SlimiDelta: float

    [<DefaultValue>]
    val mutable PreSettlementPrice: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSpbmPortfDefinition =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<DefaultValue>]
    val mutable PortfolioDefId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProdFamilyCode: byte array

    [<DefaultValue>]
    val mutable IsSpbm: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSpdApply =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable FirstLegInstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable SecondLegInstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable Volume: int

    [<DefaultValue>]
    val mutable Direction: byte

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderRef: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable ActiveUserId: byte array

    [<DefaultValue>]
    val mutable BrokerOrderSeq: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable OrderSysId: byte array

    [<DefaultValue>]
    val mutable ApplyStatus: byte

    [<DefaultValue>]
    val mutable SequenceNo: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable InsertTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable CancelTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderLocalId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

    [<DefaultValue>]
    val mutable InstallId: int

    [<DefaultValue>]
    val mutable OrderSubmitStatus: byte

    [<DefaultValue>]
    val mutable NotifySequence: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<DefaultValue>]
    val mutable SettlementId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<DefaultValue>]
    val mutable CmbType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable StatusMsg: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSpdApplyAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ActionDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ActionTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

    [<DefaultValue>]
    val mutable InstallId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderLocalId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable ActionLocalId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<DefaultValue>]
    val mutable OrderActionStatus: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable OrderSysId: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable StatusMsg: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderRef: byte array

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSpmmInstParam =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<DefaultValue>]
    val mutable InstMarginCalId: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable CommodityId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable CommodityGroupId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSpmmProductParam =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable CommodityId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable CommodityGroupId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTraderOffer =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Password: byte array

    [<DefaultValue>]
    val mutable InstallId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderLocalId: byte array

    [<DefaultValue>]
    val mutable TraderConnectStatus: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ConnectRequestDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ConnectRequestTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable LastReportDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable LastReportTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ConnectDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ConnectTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable StartDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable StartTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MaxTradeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)>]
    [<DefaultValue>]
    val mutable MaxOrderMessageReference: byte array

    [<DefaultValue>]
    val mutable OrderCancelAlg: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTradingAccountPasswordUpdate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable OldPassword: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable NewPassword: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTradingCode =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<DefaultValue>]
    val mutable IsActive: int

    [<DefaultValue>]
    val mutable ClientIdType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable BranchId: byte array

    [<DefaultValue>]
    val mutable BizType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTradingNotice =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<DefaultValue>]
    val mutable InvestorRange: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable SequenceSeries: int16

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable SendTime: byte array

    [<DefaultValue>]
    val mutable SequenceNo: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 501)>]
    [<DefaultValue>]
    val mutable FieldContent: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable InvestUnitId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTransferBank =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable BankId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)>]
    [<DefaultValue>]
    val mutable BankBrchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 101)>]
    [<DefaultValue>]
    val mutable BankName: byte array

    [<DefaultValue>]
    val mutable IsActive: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTransferSerial =
    [<DefaultValue>]
    val mutable PlateSerial: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradeDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradeTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)>]
    [<DefaultValue>]
    val mutable TradeCode: byte array

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable BankId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)>]
    [<DefaultValue>]
    val mutable BankBranchId: byte array

    [<DefaultValue>]
    val mutable BankAccType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BankAccount: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable BankSerial: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable BrokerBranchId: byte array

    [<DefaultValue>]
    val mutable FutureAccType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable FutureSerial: int

    [<DefaultValue>]
    val mutable IdCardType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 51)>]
    [<DefaultValue>]
    val mutable IdentifiedCardNo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<DefaultValue>]
    val mutable TradeAmount: float

    [<DefaultValue>]
    val mutable CustFee: float

    [<DefaultValue>]
    val mutable BrokerFee: float

    [<DefaultValue>]
    val mutable AvailabilityFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable OperatorCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BankNewAccount: byte array

    [<DefaultValue>]
    val mutable ErrorId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ErrorMsg: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeUserPasswordUpdate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable OldPassword: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable NewPassword: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeUserSession =
    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable LoginDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable LoginTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable UserProductInfo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable InterfaceProductInfo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ProtocolInfo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)>]
    [<DefaultValue>]
    val mutable LoginRemark: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeUserSystemInfo =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable ClientSystemInfoLen: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 273)>]
    [<DefaultValue>]
    val mutable ClientSystemInfo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<DefaultValue>]
    val mutable ClientIpPort: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ClientLoginTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable ClientAppId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable ClientPublicIp: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 151)>]
    [<DefaultValue>]
    val mutable ClientLoginRemark: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Mac: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeWechatUserSystemInfo =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable WechatCltSysInfoLen: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 273)>]
    [<DefaultValue>]
    val mutable WechatCltSysInfo: byte array

    [<DefaultValue>]
    val mutable ClientIpPort: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ClientLoginTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable ClientAppId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable ClientPublicIp: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 151)>]
    [<DefaultValue>]
    val mutable ClientLoginRemark: byte array

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderFrontConnectedDelegate = delegate of nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderFrontDisconnectedDelegate = delegate of int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderHeartBeatWarningDelegate = delegate of int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRtnPrivateSeqNoDelegate = delegate of int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspAuthenticateDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspSettlementInfoConfirmDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspUserLoginDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspUserLogoutDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspErrorDelegate = delegate of nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryTradingAccountDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryInvestorPositionDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryInstrumentMarginRateDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryExchangeMarginRateDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryInstrumentCommissionRateDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspOrderInsertDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspOrderActionDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRtnOrderDelegate = delegate of nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRtnTradeDelegate = delegate of nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderErrRtnBankToFutureByFutureDelegate = delegate of nativeint * nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderErrRtnBatchOrderActionDelegate = delegate of nativeint * nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderErrRtnCancelOffsetSettingDelegate = delegate of nativeint * nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderErrRtnCombActionInsertDelegate = delegate of nativeint * nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderErrRtnExecOrderActionDelegate = delegate of nativeint * nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderErrRtnExecOrderInsertDelegate = delegate of nativeint * nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderErrRtnForQuoteInsertDelegate = delegate of nativeint * nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderErrRtnFutureToBankByFutureDelegate = delegate of nativeint * nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderErrRtnHedgeCfmDelegate = delegate of nativeint * nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderErrRtnHedgeCfmActionDelegate = delegate of nativeint * nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderErrRtnOffsetSettingDelegate = delegate of nativeint * nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderErrRtnOptionSelfCloseActionDelegate = delegate of nativeint * nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderErrRtnOptionSelfCloseInsertDelegate = delegate of nativeint * nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderErrRtnOrderActionDelegate = delegate of nativeint * nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderErrRtnOrderInsertDelegate = delegate of nativeint * nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderErrRtnQueryBankBalanceByFutureDelegate = delegate of nativeint * nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderErrRtnQuoteActionDelegate = delegate of nativeint * nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderErrRtnQuoteInsertDelegate = delegate of nativeint * nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderErrRtnSpdApplyDelegate = delegate of nativeint * nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderErrRtnSpdApplyActionDelegate = delegate of nativeint * nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspBatchOrderActionDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspCancelOffsetSettingDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspCombActionInsertDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspExecOrderActionDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspExecOrderInsertDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspForQuoteInsertDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspFromBankToFutureByFutureDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspFromFutureToBankByFutureDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspGenSmsCodeDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspGenUserCaptchaDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspGenUserTextDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspHedgeCfmDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspHedgeCfmActionDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspOffsetSettingDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspOptionSelfCloseActionDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspOptionSelfCloseInsertDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspParkedOrderActionDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspParkedOrderInsertDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryAccountregisterDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryBrokerTradingAlgosDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryBrokerTradingParamsDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryCfmmcTradingAccountKeyDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryClassifiedInstrumentDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryCombActionDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryCombInstrumentGuardDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryCombLegDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryCombPromotionParamDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryContractBankDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryDepthMarketDataDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryEWarrantOffsetDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryExchangeDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryExchangeMarginRateAdjustDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryExchangeRateDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryExecOrderDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryForQuoteDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryHedgeCfmDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryInstrumentDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryInstrumentOrderCommRateDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryInvestUnitDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryInvestorDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryInvestorCommodityGroupSpmmMarginDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryInvestorCommoditySpmmMarginDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryInvestorInfoCommRecDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryInvestorPortfMarginRatioDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryInvestorPortfSettingDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryInvestorPositionCombineDetailDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryInvestorPositionDetailDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryInvestorProdRcamsMarginDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryInvestorProdRuleMarginDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryInvestorProdSpbmDetailDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryInvestorProductGroupMarginDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryMmInstrumentCommissionRateDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryMmOptionInstrCommRateDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryMaxOrderVolumeDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryNoticeDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryOffsetSettingDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryOptionInstrCommRateDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryOptionInstrTradeCostDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryOptionSelfCloseDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryOrderDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryParkedOrderDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryParkedOrderActionDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryProductDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryProductExchRateDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryProductGroupDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryQuoteDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryRcamsCombProductInfoDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryRcamsInstrParameterDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryRcamsInterParameterDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryRcamsIntraParameterDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryRcamsInvestorCombPositionDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryRcamsShortOptAdjustParamDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryRuleInstrParameterDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryRuleInterParameterDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryRuleIntraParameterDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryRiskSettleInvstPositionDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryRiskSettleProductStatusDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQrySpbmAddOnInterParameterDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQrySpbmFutureParameterDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQrySpbmInterParameterDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQrySpbmIntraParameterDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQrySpbmInvestorPortfDefDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQrySpbmOptionParameterDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQrySpbmPortfDefinitionDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQrySpmmInstParamDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQrySpmmProductParamDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQrySecAgentAcIdMapDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQrySecAgentCheckModeDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQrySecAgentTradeInfoDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQrySecAgentTradingAccountDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQrySettlementInfoDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQrySettlementInfoConfirmDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQrySpdApplyDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryTradeDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryTraderOfferDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryTradingCodeDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryTradingNoticeDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryTransferBankDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryTransferSerialDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQryUserSessionDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQueryBankAccountMoneyByFutureDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQueryCfmmcTradingAccountTokenDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQuoteActionDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspQuoteInsertDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspRemoveParkedOrderDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspRemoveParkedOrderActionDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspSpdApplyDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspSpdApplyActionDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspTradingAccountPasswordUpdateDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspUserAuthMethodDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspUserPasswordUpdateDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRtnCombActionDelegate = delegate of nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRtnExecOrderDelegate = delegate of nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRtnForQuoteRspDelegate = delegate of nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRtnFromBankToFutureByFutureDelegate = delegate of nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRtnFromFutureToBankByFutureDelegate = delegate of nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRtnHedgeCfmDelegate = delegate of nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRtnOffsetSettingDelegate = delegate of nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRtnOptionSelfCloseDelegate = delegate of nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRtnQueryBankBalanceByFutureDelegate = delegate of nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRtnQuoteDelegate = delegate of nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRtnSpdApplyDelegate = delegate of nativeint * nativeint -> unit

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTraderSpi =
    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable FrontConnected: TraderFrontConnectedDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable FrontDisconnected: TraderFrontDisconnectedDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable HeartBeatWarning: TraderHeartBeatWarningDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RtnPrivateSeqNo: TraderRtnPrivateSeqNoDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspAuthenticate: TraderRspAuthenticateDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspSettlementInfoConfirm: TraderRspSettlementInfoConfirmDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspUserLogin: TraderRspUserLoginDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspUserLogout: TraderRspUserLogoutDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspError: TraderRspErrorDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryTradingAccount: TraderRspQryTradingAccountDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryInvestorPosition: TraderRspQryInvestorPositionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryInstrumentMarginRate: TraderRspQryInstrumentMarginRateDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryExchangeMarginRate: TraderRspQryExchangeMarginRateDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryInstrumentCommissionRate: TraderRspQryInstrumentCommissionRateDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspOrderInsert: TraderRspOrderInsertDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspOrderAction: TraderRspOrderActionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RtnOrder: TraderRtnOrderDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RtnTrade: TraderRtnTradeDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable ErrRtnBankToFutureByFuture: TraderErrRtnBankToFutureByFutureDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable ErrRtnBatchOrderAction: TraderErrRtnBatchOrderActionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable ErrRtnCancelOffsetSetting: TraderErrRtnCancelOffsetSettingDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable ErrRtnCombActionInsert: TraderErrRtnCombActionInsertDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable ErrRtnExecOrderAction: TraderErrRtnExecOrderActionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable ErrRtnExecOrderInsert: TraderErrRtnExecOrderInsertDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable ErrRtnForQuoteInsert: TraderErrRtnForQuoteInsertDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable ErrRtnFutureToBankByFuture: TraderErrRtnFutureToBankByFutureDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable ErrRtnHedgeCfm: TraderErrRtnHedgeCfmDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable ErrRtnHedgeCfmAction: TraderErrRtnHedgeCfmActionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable ErrRtnOffsetSetting: TraderErrRtnOffsetSettingDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable ErrRtnOptionSelfCloseAction: TraderErrRtnOptionSelfCloseActionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable ErrRtnOptionSelfCloseInsert: TraderErrRtnOptionSelfCloseInsertDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable ErrRtnOrderAction: TraderErrRtnOrderActionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable ErrRtnOrderInsert: TraderErrRtnOrderInsertDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable ErrRtnQueryBankBalanceByFuture: TraderErrRtnQueryBankBalanceByFutureDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable ErrRtnQuoteAction: TraderErrRtnQuoteActionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable ErrRtnQuoteInsert: TraderErrRtnQuoteInsertDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable ErrRtnSpdApply: TraderErrRtnSpdApplyDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable ErrRtnSpdApplyAction: TraderErrRtnSpdApplyActionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspBatchOrderAction: TraderRspBatchOrderActionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspCancelOffsetSetting: TraderRspCancelOffsetSettingDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspCombActionInsert: TraderRspCombActionInsertDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspExecOrderAction: TraderRspExecOrderActionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspExecOrderInsert: TraderRspExecOrderInsertDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspForQuoteInsert: TraderRspForQuoteInsertDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspFromBankToFutureByFuture: TraderRspFromBankToFutureByFutureDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspFromFutureToBankByFuture: TraderRspFromFutureToBankByFutureDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspGenSmsCode: TraderRspGenSmsCodeDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspGenUserCaptcha: TraderRspGenUserCaptchaDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspGenUserText: TraderRspGenUserTextDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspHedgeCfm: TraderRspHedgeCfmDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspHedgeCfmAction: TraderRspHedgeCfmActionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspOffsetSetting: TraderRspOffsetSettingDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspOptionSelfCloseAction: TraderRspOptionSelfCloseActionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspOptionSelfCloseInsert: TraderRspOptionSelfCloseInsertDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspParkedOrderAction: TraderRspParkedOrderActionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspParkedOrderInsert: TraderRspParkedOrderInsertDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryAccountregister: TraderRspQryAccountregisterDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryBrokerTradingAlgos: TraderRspQryBrokerTradingAlgosDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryBrokerTradingParams: TraderRspQryBrokerTradingParamsDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryCfmmcTradingAccountKey: TraderRspQryCfmmcTradingAccountKeyDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryClassifiedInstrument: TraderRspQryClassifiedInstrumentDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryCombAction: TraderRspQryCombActionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryCombInstrumentGuard: TraderRspQryCombInstrumentGuardDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryCombLeg: TraderRspQryCombLegDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryCombPromotionParam: TraderRspQryCombPromotionParamDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryContractBank: TraderRspQryContractBankDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryDepthMarketData: TraderRspQryDepthMarketDataDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryEWarrantOffset: TraderRspQryEWarrantOffsetDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryExchange: TraderRspQryExchangeDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryExchangeMarginRateAdjust: TraderRspQryExchangeMarginRateAdjustDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryExchangeRate: TraderRspQryExchangeRateDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryExecOrder: TraderRspQryExecOrderDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryForQuote: TraderRspQryForQuoteDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryHedgeCfm: TraderRspQryHedgeCfmDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryInstrument: TraderRspQryInstrumentDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryInstrumentOrderCommRate: TraderRspQryInstrumentOrderCommRateDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryInvestUnit: TraderRspQryInvestUnitDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryInvestor: TraderRspQryInvestorDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryInvestorCommodityGroupSpmmMargin: TraderRspQryInvestorCommodityGroupSpmmMarginDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryInvestorCommoditySpmmMargin: TraderRspQryInvestorCommoditySpmmMarginDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryInvestorInfoCommRec: TraderRspQryInvestorInfoCommRecDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryInvestorPortfMarginRatio: TraderRspQryInvestorPortfMarginRatioDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryInvestorPortfSetting: TraderRspQryInvestorPortfSettingDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryInvestorPositionCombineDetail: TraderRspQryInvestorPositionCombineDetailDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryInvestorPositionDetail: TraderRspQryInvestorPositionDetailDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryInvestorProdRcamsMargin: TraderRspQryInvestorProdRcamsMarginDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryInvestorProdRuleMargin: TraderRspQryInvestorProdRuleMarginDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryInvestorProdSpbmDetail: TraderRspQryInvestorProdSpbmDetailDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryInvestorProductGroupMargin: TraderRspQryInvestorProductGroupMarginDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryMmInstrumentCommissionRate: TraderRspQryMmInstrumentCommissionRateDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryMmOptionInstrCommRate: TraderRspQryMmOptionInstrCommRateDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryMaxOrderVolume: TraderRspQryMaxOrderVolumeDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryNotice: TraderRspQryNoticeDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryOffsetSetting: TraderRspQryOffsetSettingDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryOptionInstrCommRate: TraderRspQryOptionInstrCommRateDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryOptionInstrTradeCost: TraderRspQryOptionInstrTradeCostDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryOptionSelfClose: TraderRspQryOptionSelfCloseDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryOrder: TraderRspQryOrderDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryParkedOrder: TraderRspQryParkedOrderDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryParkedOrderAction: TraderRspQryParkedOrderActionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryProduct: TraderRspQryProductDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryProductExchRate: TraderRspQryProductExchRateDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryProductGroup: TraderRspQryProductGroupDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryQuote: TraderRspQryQuoteDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryRcamsCombProductInfo: TraderRspQryRcamsCombProductInfoDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryRcamsInstrParameter: TraderRspQryRcamsInstrParameterDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryRcamsInterParameter: TraderRspQryRcamsInterParameterDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryRcamsIntraParameter: TraderRspQryRcamsIntraParameterDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryRcamsInvestorCombPosition: TraderRspQryRcamsInvestorCombPositionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryRcamsShortOptAdjustParam: TraderRspQryRcamsShortOptAdjustParamDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryRuleInstrParameter: TraderRspQryRuleInstrParameterDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryRuleInterParameter: TraderRspQryRuleInterParameterDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryRuleIntraParameter: TraderRspQryRuleIntraParameterDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryRiskSettleInvstPosition: TraderRspQryRiskSettleInvstPositionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryRiskSettleProductStatus: TraderRspQryRiskSettleProductStatusDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQrySpbmAddOnInterParameter: TraderRspQrySpbmAddOnInterParameterDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQrySpbmFutureParameter: TraderRspQrySpbmFutureParameterDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQrySpbmInterParameter: TraderRspQrySpbmInterParameterDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQrySpbmIntraParameter: TraderRspQrySpbmIntraParameterDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQrySpbmInvestorPortfDef: TraderRspQrySpbmInvestorPortfDefDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQrySpbmOptionParameter: TraderRspQrySpbmOptionParameterDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQrySpbmPortfDefinition: TraderRspQrySpbmPortfDefinitionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQrySpmmInstParam: TraderRspQrySpmmInstParamDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQrySpmmProductParam: TraderRspQrySpmmProductParamDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQrySecAgentAcIdMap: TraderRspQrySecAgentAcIdMapDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQrySecAgentCheckMode: TraderRspQrySecAgentCheckModeDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQrySecAgentTradeInfo: TraderRspQrySecAgentTradeInfoDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQrySecAgentTradingAccount: TraderRspQrySecAgentTradingAccountDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQrySettlementInfo: TraderRspQrySettlementInfoDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQrySettlementInfoConfirm: TraderRspQrySettlementInfoConfirmDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQrySpdApply: TraderRspQrySpdApplyDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryTrade: TraderRspQryTradeDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryTraderOffer: TraderRspQryTraderOfferDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryTradingCode: TraderRspQryTradingCodeDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryTradingNotice: TraderRspQryTradingNoticeDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryTransferBank: TraderRspQryTransferBankDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryTransferSerial: TraderRspQryTransferSerialDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQryUserSession: TraderRspQryUserSessionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQueryBankAccountMoneyByFuture: TraderRspQueryBankAccountMoneyByFutureDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQueryCfmmcTradingAccountToken: TraderRspQueryCfmmcTradingAccountTokenDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQuoteAction: TraderRspQuoteActionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspQuoteInsert: TraderRspQuoteInsertDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspRemoveParkedOrder: TraderRspRemoveParkedOrderDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspRemoveParkedOrderAction: TraderRspRemoveParkedOrderActionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspSpdApply: TraderRspSpdApplyDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspSpdApplyAction: TraderRspSpdApplyActionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspTradingAccountPasswordUpdate: TraderRspTradingAccountPasswordUpdateDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspUserAuthMethod: TraderRspUserAuthMethodDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RspUserPasswordUpdate: TraderRspUserPasswordUpdateDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RtnCombAction: TraderRtnCombActionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RtnExecOrder: TraderRtnExecOrderDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RtnForQuoteRsp: TraderRtnForQuoteRspDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RtnFromBankToFutureByFuture: TraderRtnFromBankToFutureByFutureDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RtnFromFutureToBankByFuture: TraderRtnFromFutureToBankByFutureDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RtnHedgeCfm: TraderRtnHedgeCfmDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RtnOffsetSetting: TraderRtnOffsetSettingDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RtnOptionSelfClose: TraderRtnOptionSelfCloseDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RtnQueryBankBalanceByFuture: TraderRtnQueryBankBalanceByFutureDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RtnQuote: TraderRtnQuoteDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable RtnSpdApply: TraderRtnSpdApplyDelegate

module private TraderNativeInterop =
    [<Literal>]
    let Library = "ctptrader_bridge"

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_get_api_version")>]
    extern nativeint getApiVersion()

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_create")>]
    extern nativeint create(nativeint flowPath, int productionMode)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_destroy")>]
    extern void destroy(nativeint handle)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_set_spi")>]
    extern int setSpi(nativeint handle, NativeTraderSpi& spi, nativeint userData)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_register_front")>]
    extern int registerFront(nativeint handle, nativeint frontAddress)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_subscribe_private_topic")>]
    extern int subscribePrivateTopic(nativeint handle, int resumeType, int seqNo)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_subscribe_public_topic")>]
    extern int subscribePublicTopic(nativeint handle, int resumeType)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_init")>]
    extern void init(nativeint handle)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_join")>]
    extern int join(nativeint handle)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_authenticate")>]
    extern int reqAuthenticate(nativeint handle, NativeReqAuthenticate& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_settlement_info_confirm")>]
    extern int reqSettlementInfoConfirm(nativeint handle, NativeSettlementInfoConfirm& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_user_login")>]
    extern int reqUserLogin(nativeint handle, NativeReqUserLogin& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_user_logout")>]
    extern int reqUserLogout(nativeint handle, NativeUserLogout& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_trading_account")>]
    extern int reqQryTradingAccount(nativeint handle, NativeQryTradingAccount& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_investor_position")>]
    extern int reqQryInvestorPosition(nativeint handle, NativeQryInvestorPosition& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_instrument_margin_rate")>]
    extern int reqQryInstrumentMarginRate(nativeint handle, NativeQryInstrumentMarginRate& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_exchange_margin_rate")>]
    extern int reqQryExchangeMarginRate(nativeint handle, NativeQryExchangeMarginRate& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_instrument_commission_rate")>]
    extern int reqQryInstrumentCommissionRate(nativeint handle, NativeQryInstrumentCommissionRate& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_order_insert")>]
    extern int reqOrderInsert(nativeint handle, NativeInputOrder& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_order_action")>]
    extern int reqOrderAction(nativeint handle, NativeInputOrderAction& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_get_trading_day")>]
    extern nativeint getTradingDay(nativeint handle)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_get_front_info")>]
    extern int getFrontInfo(nativeint handle, NativeFrontInfo& frontInfo)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_register_name_server")>]
    extern int registerNameServer(nativeint handle, nativeint nsAddress)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_register_fens_user_info")>]
    extern int registerFensUserInfo(nativeint handle, NativeFensUserInfo& request)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_register_user_system_info")>]
    extern int registerUserSystemInfo(nativeint handle, NativeUserSystemInfo& request)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_submit_user_system_info")>]
    extern int submitUserSystemInfo(nativeint handle, NativeUserSystemInfo& request)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_register_wechat_user_system_info")>]
    extern int registerWechatUserSystemInfo(nativeint handle, NativeWechatUserSystemInfo& request)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_submit_wechat_user_system_info")>]
    extern int submitWechatUserSystemInfo(nativeint handle, NativeWechatUserSystemInfo& request)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_user_password_update")>]
    extern int reqUserPasswordUpdate(nativeint handle, NativeUserPasswordUpdate& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_trading_account_password_update")>]
    extern int reqTradingAccountPasswordUpdate(nativeint handle, NativeTradingAccountPasswordUpdate& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_user_auth_method")>]
    extern int reqUserAuthMethod(nativeint handle, NativeReqUserAuthMethod& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_gen_user_captcha")>]
    extern int reqGenUserCaptcha(nativeint handle, NativeReqGenUserCaptcha& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_gen_user_text")>]
    extern int reqGenUserText(nativeint handle, NativeReqGenUserText& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_user_login_with_captcha")>]
    extern int reqUserLoginWithCaptcha(nativeint handle, NativeReqUserLoginWithCaptcha& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_user_login_with_text")>]
    extern int reqUserLoginWithText(nativeint handle, NativeReqUserLoginWithText& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_user_login_with_otp")>]
    extern int reqUserLoginWithOtp(nativeint handle, NativeReqUserLoginWithOtp& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_gen_sms_code")>]
    extern int reqGenSmsCode(nativeint handle, NativeReqGenSmsCode& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_parked_order_insert")>]
    extern int reqParkedOrderInsert(nativeint handle, NativeParkedOrder& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_parked_order_action")>]
    extern int reqParkedOrderAction(nativeint handle, NativeParkedOrderAction& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_max_order_volume")>]
    extern int reqQryMaxOrderVolume(nativeint handle, NativeQryMaxOrderVolume& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_remove_parked_order")>]
    extern int reqRemoveParkedOrder(nativeint handle, NativeRemoveParkedOrder& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_remove_parked_order_action")>]
    extern int reqRemoveParkedOrderAction(nativeint handle, NativeRemoveParkedOrderAction& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_exec_order_insert")>]
    extern int reqExecOrderInsert(nativeint handle, NativeInputExecOrder& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_exec_order_action")>]
    extern int reqExecOrderAction(nativeint handle, NativeInputExecOrderAction& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_for_quote_insert")>]
    extern int reqForQuoteInsert(nativeint handle, NativeInputForQuote& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_quote_insert")>]
    extern int reqQuoteInsert(nativeint handle, NativeInputQuote& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_quote_action")>]
    extern int reqQuoteAction(nativeint handle, NativeInputQuoteAction& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_batch_order_action")>]
    extern int reqBatchOrderAction(nativeint handle, NativeInputBatchOrderAction& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_option_self_close_insert")>]
    extern int reqOptionSelfCloseInsert(nativeint handle, NativeInputOptionSelfClose& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_option_self_close_action")>]
    extern int reqOptionSelfCloseAction(nativeint handle, NativeInputOptionSelfCloseAction& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_comb_action_insert")>]
    extern int reqCombActionInsert(nativeint handle, NativeInputCombAction& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_offset_setting")>]
    extern int reqOffsetSetting(nativeint handle, NativeInputOffsetSetting& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_cancel_offset_setting")>]
    extern int reqCancelOffsetSetting(nativeint handle, NativeInputOffsetSetting& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_spd_apply")>]
    extern int reqSpdApply(nativeint handle, NativeInputSpdApply& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_spd_apply_action")>]
    extern int reqSpdApplyAction(nativeint handle, NativeInputSpdApplyAction& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_hedge_cfm")>]
    extern int reqHedgeCfm(nativeint handle, NativeInputHedgeCfm& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_hedge_cfm_action")>]
    extern int reqHedgeCfmAction(nativeint handle, NativeInputHedgeCfmAction& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_order")>]
    extern int reqQryOrder(nativeint handle, NativeQryOrder& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_trade")>]
    extern int reqQryTrade(nativeint handle, NativeQryTrade& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_investor")>]
    extern int reqQryInvestor(nativeint handle, NativeQryInvestor& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_trading_code")>]
    extern int reqQryTradingCode(nativeint handle, NativeQryTradingCode& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_user_session")>]
    extern int reqQryUserSession(nativeint handle, NativeQryUserSession& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_exchange")>]
    extern int reqQryExchange(nativeint handle, NativeQryExchange& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_product")>]
    extern int reqQryProduct(nativeint handle, NativeQryProduct& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_instrument")>]
    extern int reqQryInstrument(nativeint handle, NativeQryInstrument& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_depth_market_data")>]
    extern int reqQryDepthMarketData(nativeint handle, NativeQryDepthMarketData& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_trader_offer")>]
    extern int reqQryTraderOffer(nativeint handle, NativeQryTraderOffer& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_settlement_info")>]
    extern int reqQrySettlementInfo(nativeint handle, NativeQrySettlementInfo& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_transfer_bank")>]
    extern int reqQryTransferBank(nativeint handle, NativeQryTransferBank& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_investor_position_detail")>]
    extern int reqQryInvestorPositionDetail(nativeint handle, NativeQryInvestorPositionDetail& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_notice")>]
    extern int reqQryNotice(nativeint handle, NativeQryNotice& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_settlement_info_confirm")>]
    extern int reqQrySettlementInfoConfirm(nativeint handle, NativeQrySettlementInfoConfirm& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_investor_position_combine_detail")>]
    extern int reqQryInvestorPositionCombineDetail(nativeint handle, NativeQryInvestorPositionCombineDetail& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_cfmmc_trading_account_key")>]
    extern int reqQryCfmmcTradingAccountKey(nativeint handle, NativeQryCfmmcTradingAccountKey& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_e_warrant_offset")>]
    extern int reqQryEWarrantOffset(nativeint handle, NativeQryEWarrantOffset& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_investor_product_group_margin")>]
    extern int reqQryInvestorProductGroupMargin(nativeint handle, NativeQryInvestorProductGroupMargin& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_exchange_margin_rate_adjust")>]
    extern int reqQryExchangeMarginRateAdjust(nativeint handle, NativeQryExchangeMarginRateAdjust& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_exchange_rate")>]
    extern int reqQryExchangeRate(nativeint handle, NativeQryExchangeRate& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_sec_agent_ac_id_map")>]
    extern int reqQrySecAgentAcIdMap(nativeint handle, NativeQrySecAgentAcIdMap& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_product_exch_rate")>]
    extern int reqQryProductExchRate(nativeint handle, NativeQryProductExchRate& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_product_group")>]
    extern int reqQryProductGroup(nativeint handle, NativeQryProductGroup& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_mm_instrument_commission_rate")>]
    extern int reqQryMmInstrumentCommissionRate(nativeint handle, NativeQryMmInstrumentCommissionRate& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_mm_option_instr_comm_rate")>]
    extern int reqQryMmOptionInstrCommRate(nativeint handle, NativeQryMmOptionInstrCommRate& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_instrument_order_comm_rate")>]
    extern int reqQryInstrumentOrderCommRate(nativeint handle, NativeQryInstrumentOrderCommRate& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_sec_agent_trading_account")>]
    extern int reqQrySecAgentTradingAccount(nativeint handle, NativeQryTradingAccount& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_sec_agent_check_mode")>]
    extern int reqQrySecAgentCheckMode(nativeint handle, NativeQrySecAgentCheckMode& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_sec_agent_trade_info")>]
    extern int reqQrySecAgentTradeInfo(nativeint handle, NativeQrySecAgentTradeInfo& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_option_instr_trade_cost")>]
    extern int reqQryOptionInstrTradeCost(nativeint handle, NativeQryOptionInstrTradeCost& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_option_instr_comm_rate")>]
    extern int reqQryOptionInstrCommRate(nativeint handle, NativeQryOptionInstrCommRate& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_exec_order")>]
    extern int reqQryExecOrder(nativeint handle, NativeQryExecOrder& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_for_quote")>]
    extern int reqQryForQuote(nativeint handle, NativeQryForQuote& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_quote")>]
    extern int reqQryQuote(nativeint handle, NativeQryQuote& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_option_self_close")>]
    extern int reqQryOptionSelfClose(nativeint handle, NativeQryOptionSelfClose& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_invest_unit")>]
    extern int reqQryInvestUnit(nativeint handle, NativeQryInvestUnit& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_comb_instrument_guard")>]
    extern int reqQryCombInstrumentGuard(nativeint handle, NativeQryCombInstrumentGuard& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_comb_action")>]
    extern int reqQryCombAction(nativeint handle, NativeQryCombAction& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_transfer_serial")>]
    extern int reqQryTransferSerial(nativeint handle, NativeQryTransferSerial& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_accountregister")>]
    extern int reqQryAccountregister(nativeint handle, NativeQryAccountregister& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_contract_bank")>]
    extern int reqQryContractBank(nativeint handle, NativeQryContractBank& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_parked_order")>]
    extern int reqQryParkedOrder(nativeint handle, NativeQryParkedOrder& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_parked_order_action")>]
    extern int reqQryParkedOrderAction(nativeint handle, NativeQryParkedOrderAction& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_trading_notice")>]
    extern int reqQryTradingNotice(nativeint handle, NativeQryTradingNotice& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_broker_trading_params")>]
    extern int reqQryBrokerTradingParams(nativeint handle, NativeQryBrokerTradingParams& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_broker_trading_algos")>]
    extern int reqQryBrokerTradingAlgos(nativeint handle, NativeQryBrokerTradingAlgos& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_query_cfmmc_trading_account_token")>]
    extern int reqQueryCfmmcTradingAccountToken(nativeint handle, NativeQueryCfmmcTradingAccountToken& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_classified_instrument")>]
    extern int reqQryClassifiedInstrument(nativeint handle, NativeQryClassifiedInstrument& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_comb_promotion_param")>]
    extern int reqQryCombPromotionParam(nativeint handle, NativeQryCombPromotionParam& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_risk_settle_invst_position")>]
    extern int reqQryRiskSettleInvstPosition(nativeint handle, NativeQryRiskSettleInvstPosition& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_risk_settle_product_status")>]
    extern int reqQryRiskSettleProductStatus(nativeint handle, NativeQryRiskSettleProductStatus& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_spbm_future_parameter")>]
    extern int reqQrySpbmFutureParameter(nativeint handle, NativeQrySpbmFutureParameter& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_spbm_option_parameter")>]
    extern int reqQrySpbmOptionParameter(nativeint handle, NativeQrySpbmOptionParameter& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_spbm_intra_parameter")>]
    extern int reqQrySpbmIntraParameter(nativeint handle, NativeQrySpbmIntraParameter& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_spbm_inter_parameter")>]
    extern int reqQrySpbmInterParameter(nativeint handle, NativeQrySpbmInterParameter& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_spbm_portf_definition")>]
    extern int reqQrySpbmPortfDefinition(nativeint handle, NativeQrySpbmPortfDefinition& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_spbm_investor_portf_def")>]
    extern int reqQrySpbmInvestorPortfDef(nativeint handle, NativeQrySpbmInvestorPortfDef& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_investor_portf_margin_ratio")>]
    extern int reqQryInvestorPortfMarginRatio(nativeint handle, NativeQryInvestorPortfMarginRatio& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_investor_prod_spbm_detail")>]
    extern int reqQryInvestorProdSpbmDetail(nativeint handle, NativeQryInvestorProdSpbmDetail& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_investor_commodity_spmm_margin")>]
    extern int reqQryInvestorCommoditySpmmMargin(nativeint handle, NativeQryInvestorCommoditySpmmMargin& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_investor_commodity_group_spmm_margin")>]
    extern int reqQryInvestorCommodityGroupSpmmMargin(nativeint handle, NativeQryInvestorCommodityGroupSpmmMargin& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_spmm_inst_param")>]
    extern int reqQrySpmmInstParam(nativeint handle, NativeQrySpmmInstParam& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_spmm_product_param")>]
    extern int reqQrySpmmProductParam(nativeint handle, NativeQrySpmmProductParam& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_spbm_add_on_inter_parameter")>]
    extern int reqQrySpbmAddOnInterParameter(nativeint handle, NativeQrySpbmAddOnInterParameter& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_rcams_comb_product_info")>]
    extern int reqQryRcamsCombProductInfo(nativeint handle, NativeQryRcamsCombProductInfo& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_rcams_instr_parameter")>]
    extern int reqQryRcamsInstrParameter(nativeint handle, NativeQryRcamsInstrParameter& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_rcams_intra_parameter")>]
    extern int reqQryRcamsIntraParameter(nativeint handle, NativeQryRcamsIntraParameter& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_rcams_inter_parameter")>]
    extern int reqQryRcamsInterParameter(nativeint handle, NativeQryRcamsInterParameter& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_rcams_short_opt_adjust_param")>]
    extern int reqQryRcamsShortOptAdjustParam(nativeint handle, NativeQryRcamsShortOptAdjustParam& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_rcams_investor_comb_position")>]
    extern int reqQryRcamsInvestorCombPosition(nativeint handle, NativeQryRcamsInvestorCombPosition& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_investor_prod_rcams_margin")>]
    extern int reqQryInvestorProdRcamsMargin(nativeint handle, NativeQryInvestorProdRcamsMargin& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_rule_instr_parameter")>]
    extern int reqQryRuleInstrParameter(nativeint handle, NativeQryRuleInstrParameter& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_rule_intra_parameter")>]
    extern int reqQryRuleIntraParameter(nativeint handle, NativeQryRuleIntraParameter& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_rule_inter_parameter")>]
    extern int reqQryRuleInterParameter(nativeint handle, NativeQryRuleInterParameter& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_investor_prod_rule_margin")>]
    extern int reqQryInvestorProdRuleMargin(nativeint handle, NativeQryInvestorProdRuleMargin& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_investor_portf_setting")>]
    extern int reqQryInvestorPortfSetting(nativeint handle, NativeQryInvestorPortfSetting& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_investor_info_comm_rec")>]
    extern int reqQryInvestorInfoCommRec(nativeint handle, NativeQryInvestorInfoCommRec& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_comb_leg")>]
    extern int reqQryCombLeg(nativeint handle, NativeQryCombLeg& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_offset_setting")>]
    extern int reqQryOffsetSetting(nativeint handle, NativeQryOffsetSetting& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_spd_apply")>]
    extern int reqQrySpdApply(nativeint handle, NativeQrySpdApply& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_hedge_cfm")>]
    extern int reqQryHedgeCfm(nativeint handle, NativeQryHedgeCfm& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_from_bank_to_future_by_future")>]
    extern int reqFromBankToFutureByFuture(nativeint handle, NativeReqTransfer& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_from_future_to_bank_by_future")>]
    extern int reqFromFutureToBankByFuture(nativeint handle, NativeReqTransfer& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_query_bank_account_money_by_future")>]
    extern int reqQueryBankAccountMoneyByFuture(nativeint handle, NativeReqQueryAccount& request, int requestId)

type private TraderApiSafeHandle private () =
    inherit SafeHandleZeroOrMinusOneIsInvalid(true)

    static member FromNative(handle: nativeint) =
        let safeHandle = new TraderApiSafeHandle()
        safeHandle.SetHandle(handle)
        safeHandle

    override this.ReleaseHandle() =
        BridgeResolver.ensureRegistered ()
        TraderNativeInterop.destroy this.handle
        true

module private TraderBridgeGenerated =
    let private getFieldSize (fieldInfo: System.Reflection.FieldInfo) =
        fieldInfo.GetCustomAttributes(typeof<MarshalAsAttribute>, false)
        |> Array.tryHead
        |> Option.map (fun attribute -> (attribute :?> MarshalAsAttribute).SizeConst)

    let private nativeFieldMap<'T> =
        typeof<'T>.GetFields(System.Reflection.BindingFlags.Instance ||| System.Reflection.BindingFlags.Public ||| System.Reflection.BindingFlags.NonPublic)
        |> Array.map (fun field -> field.Name, field)
        |> dict

    let private encodeStringField encoding size value =
        if isNull value then
            EncodingHelpers.encodeFixed encoding size None
        else
            EncodingHelpers.encodeFixed encoding size (Some value)

    let mapNative<'Record, 'Native> encoding (value: 'Native) : 'Record =
        let nativeFields = nativeFieldMap<'Native>
        let values =
            FSharpType.GetRecordFields typeof<'Record>
            |> Array.map (fun field ->
                let nativeField = nativeFields[field.Name]
                let nativeValue = nativeField.GetValue(box value)

                if field.PropertyType = typeof<string> then
                    box (EncodingHelpers.decodeFixed encoding (nativeValue :?> byte array))
                elif field.PropertyType = typeof<char option> then
                    box (EncodingHelpers.byteToChar (unbox<byte> nativeValue))
                elif field.PropertyType = typeof<decimal> then
                    box (NumericHelpers.priceOrInvalid (unbox<float> nativeValue))
                elif field.PropertyType = typeof<int> then
                    if nativeField.FieldType = typeof<int16> then
                        box (int (unbox<int16> nativeValue))
                    else
                        nativeValue
                else
                    invalidOp $"Unsupported record field type '{field.PropertyType.FullName}' for generated trader bridge mapping."
            )

        FSharpValue.MakeRecord(typeof<'Record>, values) :?> 'Record

    let private setFieldValue (boxedNative: obj) (fieldInfo: System.Reflection.FieldInfo) value =
        fieldInfo.SetValue(boxedNative, value)

    let private buildNativeInternal<'Record, 'Native when 'Native: struct> encoding requestIdOpt (record: 'Record) : 'Native =
        let nativeFields = nativeFieldMap<'Native>
        let recordFields =
            FSharpType.GetRecordFields typeof<'Record>
            |> Array.map (fun field -> field.Name, field)
            |> dict

        let boxedNative = box (System.Activator.CreateInstance<'Native>())

        for KeyValue(_, nativeField) in nativeFields do
            match recordFields.TryGetValue nativeField.Name with
            | true, recordField ->
                let recordValue = recordField.GetValue(box record)
                let nativeValue =
                    if nativeField.FieldType = typeof<byte array> then
                        let size = getFieldSize nativeField |> Option.defaultWith (fun () -> invalidOp $"Missing MarshalAs size for field '{nativeField.Name}'.")
                        box (encodeStringField encoding size (recordValue :?> string))
                    elif nativeField.FieldType = typeof<byte> && recordField.PropertyType = typeof<char option> then
                        box (EncodingHelpers.charToByte (recordValue :?> char option))
                    elif nativeField.FieldType = typeof<float> && recordField.PropertyType = typeof<decimal> then
                        box (float (unbox<decimal> recordValue))
                    elif nativeField.FieldType = typeof<int16> && recordField.PropertyType = typeof<int> then
                        box (int16 (unbox<int> recordValue))
                    else
                        recordValue
                setFieldValue boxedNative nativeField nativeValue
            | _ -> ()

        match requestIdOpt with
        | Some requestId ->
            match nativeFields.TryGetValue "RequestId" with
            | true, nativeField when nativeField.FieldType = typeof<int> -> setFieldValue boxedNative nativeField (box requestId)
            | _ -> ()
        | None -> ()

        unbox<'Native> boxedNative

    let buildNative<'Record, 'Native when 'Native: struct> encoding (record: 'Record) : 'Native =
        buildNativeInternal<'Record, 'Native> encoding None record

    let buildNativeWithRequestId<'Record, 'Native when 'Native: struct> encoding requestId (record: 'Record) : 'Native =
        buildNativeInternal<'Record, 'Native> encoding (Some requestId) record

module private TraderBridgeMapping =
    let private toDecimal value = NumericHelpers.priceOrInvalid value

    let authenticate encoding (value: NativeRspAuthenticate) =
        { BrokerId = EncodingHelpers.decodeFixed encoding value.BrokerId
          UserId = EncodingHelpers.decodeFixed encoding value.UserId
          UserProductInfo = EncodingHelpers.decodeFixed encoding value.UserProductInfo
          AppId = EncodingHelpers.decodeFixed encoding value.AppId
          AppType = EncodingHelpers.byteToChar value.AppType }

    let settlementInfoConfirm encoding (value: NativeSettlementInfoConfirm) =
        { BrokerId = EncodingHelpers.decodeFixed encoding value.BrokerId
          InvestorId = EncodingHelpers.decodeFixed encoding value.InvestorId
          ConfirmDate = EncodingHelpers.decodeFixed encoding value.ConfirmDate
          ConfirmTime = EncodingHelpers.decodeFixed encoding value.ConfirmTime
          SettlementId = value.SettlementId
          AccountId = EncodingHelpers.decodeFixed encoding value.AccountId
          CurrencyId = EncodingHelpers.decodeFixed encoding value.CurrencyId }

    let tradingAccount encoding (value: NativeTradingAccount) =
        { BrokerId = EncodingHelpers.decodeFixed encoding value.BrokerId
          AccountId = EncodingHelpers.decodeFixed encoding value.AccountId
          CurrencyId = EncodingHelpers.decodeFixed encoding value.CurrencyId
          TradingDay = EncodingHelpers.decodeFixed encoding value.TradingDay
          Deposit = toDecimal value.Deposit
          Withdraw = toDecimal value.Withdraw
          Balance = toDecimal value.Balance
          Available = toDecimal value.Available
          CurrMargin = toDecimal value.CurrMargin
          FrozenMargin = toDecimal value.FrozenMargin
          FrozenCash = toDecimal value.FrozenCash
          FrozenCommission = toDecimal value.FrozenCommission
          Commission = toDecimal value.Commission
          CloseProfit = toDecimal value.CloseProfit
          PositionProfit = toDecimal value.PositionProfit
          WithdrawQuota = toDecimal value.WithdrawQuota
          Reserve = toDecimal value.Reserve }

    let investorPosition encoding (value: NativeInvestorPosition) =
        { BrokerId = EncodingHelpers.decodeFixed encoding value.BrokerId
          InvestorId = EncodingHelpers.decodeFixed encoding value.InvestorId
          InstrumentId = EncodingHelpers.decodeFixed encoding value.InstrumentId
          ExchangeId = EncodingHelpers.decodeFixed encoding value.ExchangeId
          PosiDirection = EncodingHelpers.byteToChar value.PosiDirection
          HedgeFlag = EncodingHelpers.byteToChar value.HedgeFlag
          PositionDate = EncodingHelpers.byteToChar value.PositionDate
          YdPosition = value.YdPosition
          Position = value.Position
          TodayPosition = value.TodayPosition
          LongFrozen = value.LongFrozen
          ShortFrozen = value.ShortFrozen
          OpenVolume = value.OpenVolume
          CloseVolume = value.CloseVolume
          PositionProfit = toDecimal value.PositionProfit
          CloseProfit = toDecimal value.CloseProfit
          UseMargin = toDecimal value.UseMargin
          PositionCost = toDecimal value.PositionCost
          OpenCost = toDecimal value.OpenCost }

    let instrumentMarginRate encoding (value: NativeInstrumentMarginRate) =
        { InvestorRange = EncodingHelpers.byteToChar value.InvestorRange
          BrokerId = EncodingHelpers.decodeFixed encoding value.BrokerId
          InvestorId = EncodingHelpers.decodeFixed encoding value.InvestorId
          HedgeFlag = EncodingHelpers.byteToChar value.HedgeFlag
          LongMarginRatioByMoney = toDecimal value.LongMarginRatioByMoney
          LongMarginRatioByVolume = toDecimal value.LongMarginRatioByVolume
          ShortMarginRatioByMoney = toDecimal value.ShortMarginRatioByMoney
          ShortMarginRatioByVolume = toDecimal value.ShortMarginRatioByVolume
          IsRelative = value.IsRelative <> 0
          ExchangeId = EncodingHelpers.decodeFixed encoding value.ExchangeId
          InvestUnitId = EncodingHelpers.decodeFixed encoding value.InvestUnitId
          InstrumentId = EncodingHelpers.decodeFixed encoding value.InstrumentId }

    let exchangeMarginRate encoding (value: NativeExchangeMarginRate) =
        { BrokerId = EncodingHelpers.decodeFixed encoding value.BrokerId
          HedgeFlag = EncodingHelpers.byteToChar value.HedgeFlag
          LongMarginRatioByMoney = toDecimal value.LongMarginRatioByMoney
          LongMarginRatioByVolume = toDecimal value.LongMarginRatioByVolume
          ShortMarginRatioByMoney = toDecimal value.ShortMarginRatioByMoney
          ShortMarginRatioByVolume = toDecimal value.ShortMarginRatioByVolume
          ExchangeId = EncodingHelpers.decodeFixed encoding value.ExchangeId
          InstrumentId = EncodingHelpers.decodeFixed encoding value.InstrumentId }

    let instrumentCommissionRate encoding (value: NativeInstrumentCommissionRate) =
        { InvestorRange = EncodingHelpers.byteToChar value.InvestorRange
          BrokerId = EncodingHelpers.decodeFixed encoding value.BrokerId
          InvestorId = EncodingHelpers.decodeFixed encoding value.InvestorId
          OpenRatioByMoney = toDecimal value.OpenRatioByMoney
          OpenRatioByVolume = toDecimal value.OpenRatioByVolume
          CloseRatioByMoney = toDecimal value.CloseRatioByMoney
          CloseRatioByVolume = toDecimal value.CloseRatioByVolume
          CloseTodayRatioByMoney = toDecimal value.CloseTodayRatioByMoney
          CloseTodayRatioByVolume = toDecimal value.CloseTodayRatioByVolume
          ExchangeId = EncodingHelpers.decodeFixed encoding value.ExchangeId
          BizType = EncodingHelpers.byteToChar value.BizType
          InvestUnitId = EncodingHelpers.decodeFixed encoding value.InvestUnitId
          InstrumentId = EncodingHelpers.decodeFixed encoding value.InstrumentId }

    let inputOrderRequest encoding (value: NativeInputOrder) =
        { BrokerId = EncodingHelpers.decodeFixed encoding value.BrokerId
          InvestorId = EncodingHelpers.decodeFixed encoding value.InvestorId
          InstrumentId = EncodingHelpers.decodeFixed encoding value.InstrumentId
          OrderRef = EncodingHelpers.decodeFixed encoding value.OrderRef
          UserId = EncodingHelpers.decodeFixed encoding value.UserId
          OrderPriceType = char value.OrderPriceType
          Direction = char value.Direction
          CombOffsetFlag = EncodingHelpers.decodeFixed encoding value.CombOffsetFlag
          CombHedgeFlag = EncodingHelpers.decodeFixed encoding value.CombHedgeFlag
          LimitPrice = toDecimal value.LimitPrice
          VolumeTotalOriginal = value.VolumeTotalOriginal
          TimeCondition = char value.TimeCondition
          GtdDate =
            Some(EncodingHelpers.decodeFixed encoding value.GtdDate)
            |> EncodingHelpers.normalize
          VolumeCondition = char value.VolumeCondition
          MinVolume = value.MinVolume
          ContingentCondition = char value.ContingentCondition
          StopPrice = toDecimal value.StopPrice
          ForceCloseReason = char value.ForceCloseReason
          IsAutoSuspend = value.IsAutoSuspend <> 0
          BusinessUnit =
            Some(EncodingHelpers.decodeFixed encoding value.BusinessUnit)
            |> EncodingHelpers.normalize
          UserForceClose = value.UserForceClose <> 0
          IsSwapOrder = value.IsSwapOrder <> 0
          ExchangeId =
            Some(EncodingHelpers.decodeFixed encoding value.ExchangeId)
            |> EncodingHelpers.normalize
          InvestUnitId =
            Some(EncodingHelpers.decodeFixed encoding value.InvestUnitId)
            |> EncodingHelpers.normalize
          AccountId =
            Some(EncodingHelpers.decodeFixed encoding value.AccountId)
            |> EncodingHelpers.normalize
          CurrencyId =
            Some(EncodingHelpers.decodeFixed encoding value.CurrencyId)
            |> EncodingHelpers.normalize
          ClientId =
            Some(EncodingHelpers.decodeFixed encoding value.ClientId)
            |> EncodingHelpers.normalize
          MacAddress =
            Some(EncodingHelpers.decodeFixed encoding value.MacAddress)
            |> EncodingHelpers.normalize
          IpAddress =
            Some(EncodingHelpers.decodeFixed encoding value.IpAddress)
            |> EncodingHelpers.normalize
          OrderMemo =
            Some(EncodingHelpers.decodeFixed encoding value.OrderMemo)
            |> EncodingHelpers.normalize }

    let inputOrderActionRequest encoding (value: NativeInputOrderAction) =
        { BrokerId = EncodingHelpers.decodeFixed encoding value.BrokerId
          InvestorId = EncodingHelpers.decodeFixed encoding value.InvestorId
          OrderActionRef = value.OrderActionRef
          OrderRef = EncodingHelpers.decodeFixed encoding value.OrderRef
          FrontId = value.FrontId
          SessionId = value.SessionId
          ExchangeId =
            Some(EncodingHelpers.decodeFixed encoding value.ExchangeId)
            |> EncodingHelpers.normalize
          OrderSysId =
            Some(EncodingHelpers.decodeFixed encoding value.OrderSysId)
            |> EncodingHelpers.normalize
          ActionFlag = char value.ActionFlag
          LimitPrice = toDecimal value.LimitPrice
          VolumeChange = value.VolumeChange
          UserId =
            Some(EncodingHelpers.decodeFixed encoding value.UserId)
            |> EncodingHelpers.normalize
          InvestUnitId =
            Some(EncodingHelpers.decodeFixed encoding value.InvestUnitId)
            |> EncodingHelpers.normalize
          InstrumentId =
            Some(EncodingHelpers.decodeFixed encoding value.InstrumentId)
            |> EncodingHelpers.normalize
          MacAddress =
            Some(EncodingHelpers.decodeFixed encoding value.MacAddress)
            |> EncodingHelpers.normalize
          IpAddress =
            Some(EncodingHelpers.decodeFixed encoding value.IpAddress)
            |> EncodingHelpers.normalize
          OrderMemo =
            Some(EncodingHelpers.decodeFixed encoding value.OrderMemo)
            |> EncodingHelpers.normalize }

    let orderUpdate encoding (value: NativeOrder) =
        { BrokerId = EncodingHelpers.decodeFixed encoding value.BrokerId
          InvestorId = EncodingHelpers.decodeFixed encoding value.InvestorId
          InstrumentId = EncodingHelpers.decodeFixed encoding value.InstrumentId
          ExchangeId = EncodingHelpers.decodeFixed encoding value.ExchangeId
          OrderRef = EncodingHelpers.decodeFixed encoding value.OrderRef
          OrderSysId = EncodingHelpers.decodeFixed encoding value.OrderSysId
          UserId = EncodingHelpers.decodeFixed encoding value.UserId
          OrderPriceType = EncodingHelpers.byteToChar value.OrderPriceType
          Direction = EncodingHelpers.byteToChar value.Direction
          CombOffsetFlag = EncodingHelpers.decodeFixed encoding value.CombOffsetFlag
          CombHedgeFlag = EncodingHelpers.decodeFixed encoding value.CombHedgeFlag
          LimitPrice = toDecimal value.LimitPrice
          VolumeTotalOriginal = value.VolumeTotalOriginal
          VolumeTraded = value.VolumeTraded
          VolumeTotal = value.VolumeTotal
          FrontId = value.FrontId
          SessionId = value.SessionId
          OrderStatus = EncodingHelpers.byteToChar value.OrderStatus
          OrderSubmitStatus = EncodingHelpers.byteToChar value.OrderSubmitStatus
          StatusMessage = EncodingHelpers.decodeFixed encoding value.StatusMsg
          InsertDate = EncodingHelpers.decodeFixed encoding value.InsertDate
          InsertTime = EncodingHelpers.decodeFixed encoding value.InsertTime
          ActiveTime = EncodingHelpers.decodeFixed encoding value.ActiveTime
          SuspendTime = EncodingHelpers.decodeFixed encoding value.SuspendTime
          UpdateTime = EncodingHelpers.decodeFixed encoding value.UpdateTime
          CancelTime = EncodingHelpers.decodeFixed encoding value.CancelTime }

    let tradeUpdate encoding (value: NativeTrade) =
        { BrokerId = EncodingHelpers.decodeFixed encoding value.BrokerId
          InvestorId = EncodingHelpers.decodeFixed encoding value.InvestorId
          InstrumentId = EncodingHelpers.decodeFixed encoding value.InstrumentId
          ExchangeId = EncodingHelpers.decodeFixed encoding value.ExchangeId
          OrderRef = EncodingHelpers.decodeFixed encoding value.OrderRef
          OrderSysId = EncodingHelpers.decodeFixed encoding value.OrderSysId
          TradeId = EncodingHelpers.decodeFixed encoding value.TradeId
          UserId = EncodingHelpers.decodeFixed encoding value.UserId
          Direction = EncodingHelpers.byteToChar value.Direction
          OffsetFlag = EncodingHelpers.byteToChar value.OffsetFlag
          HedgeFlag = EncodingHelpers.byteToChar value.HedgeFlag
          Price = toDecimal value.Price
          Volume = value.Volume
          TradeDate = EncodingHelpers.decodeFixed encoding value.TradeDate
          TradeTime = EncodingHelpers.decodeFixed encoding value.TradeTime
          TradingDay = EncodingHelpers.decodeFixed encoding value.TradingDay }

    let accountregister encoding (value: NativeAccountregister) : Accountregister =
        TraderBridgeGenerated.mapNative<Accountregister, NativeAccountregister> encoding value

    let batchOrderAction encoding (value: NativeBatchOrderAction) : BatchOrderAction =
        TraderBridgeGenerated.mapNative<BatchOrderAction, NativeBatchOrderAction> encoding value

    let brokerTradingAlgos encoding (value: NativeBrokerTradingAlgos) : BrokerTradingAlgos =
        TraderBridgeGenerated.mapNative<BrokerTradingAlgos, NativeBrokerTradingAlgos> encoding value

    let brokerTradingParams encoding (value: NativeBrokerTradingParams) : BrokerTradingParams =
        TraderBridgeGenerated.mapNative<BrokerTradingParams, NativeBrokerTradingParams> encoding value

    let cancelOffsetSetting encoding (value: NativeCancelOffsetSetting) : CancelOffsetSetting =
        TraderBridgeGenerated.mapNative<CancelOffsetSetting, NativeCancelOffsetSetting> encoding value

    let cfmmcTradingAccountKey encoding (value: NativeCfmmcTradingAccountKey) : CfmmcTradingAccountKey =
        TraderBridgeGenerated.mapNative<CfmmcTradingAccountKey, NativeCfmmcTradingAccountKey> encoding value

    let combAction encoding (value: NativeCombAction) : CombAction =
        TraderBridgeGenerated.mapNative<CombAction, NativeCombAction> encoding value

    let combInstrumentGuard encoding (value: NativeCombInstrumentGuard) : CombInstrumentGuard =
        TraderBridgeGenerated.mapNative<CombInstrumentGuard, NativeCombInstrumentGuard> encoding value

    let combLeg encoding (value: NativeCombLeg) : CombLeg =
        TraderBridgeGenerated.mapNative<CombLeg, NativeCombLeg> encoding value

    let combPromotionParam encoding (value: NativeCombPromotionParam) : CombPromotionParam =
        TraderBridgeGenerated.mapNative<CombPromotionParam, NativeCombPromotionParam> encoding value

    let contractBank encoding (value: NativeContractBank) : ContractBank =
        TraderBridgeGenerated.mapNative<ContractBank, NativeContractBank> encoding value

    let depthMarketData encoding (value: NativeTraderDepthMarketData) : DepthMarketData =
        TraderBridgeGenerated.mapNative<DepthMarketData, NativeTraderDepthMarketData> encoding value

    let eWarrantOffset encoding (value: NativeEWarrantOffset) : EWarrantOffset =
        TraderBridgeGenerated.mapNative<EWarrantOffset, NativeEWarrantOffset> encoding value

    let exchange encoding (value: NativeExchange) : Exchange =
        TraderBridgeGenerated.mapNative<Exchange, NativeExchange> encoding value

    let exchangeMarginRateAdjust encoding (value: NativeExchangeMarginRateAdjust) : ExchangeMarginRateAdjust =
        TraderBridgeGenerated.mapNative<ExchangeMarginRateAdjust, NativeExchangeMarginRateAdjust> encoding value

    let exchangeRate encoding (value: NativeExchangeRate) : ExchangeRate =
        TraderBridgeGenerated.mapNative<ExchangeRate, NativeExchangeRate> encoding value

    let execOrder encoding (value: NativeExecOrder) : ExecOrder =
        TraderBridgeGenerated.mapNative<ExecOrder, NativeExecOrder> encoding value

    let execOrderAction encoding (value: NativeExecOrderAction) : ExecOrderAction =
        TraderBridgeGenerated.mapNative<ExecOrderAction, NativeExecOrderAction> encoding value

    let fensUserInfo encoding (value: NativeFensUserInfo) : FensUserInfo =
        TraderBridgeGenerated.mapNative<FensUserInfo, NativeFensUserInfo> encoding value

    let forQuote encoding (value: NativeForQuote) : ForQuote =
        TraderBridgeGenerated.mapNative<ForQuote, NativeForQuote> encoding value

    let forQuoteRsp encoding (value: NativeForQuoteRsp) : ForQuoteRsp =
        TraderBridgeGenerated.mapNative<ForQuoteRsp, NativeForQuoteRsp> encoding value

    let frontInfo encoding (value: NativeFrontInfo) : FrontInfo =
        TraderBridgeGenerated.mapNative<FrontInfo, NativeFrontInfo> encoding value

    let hedgeCfm encoding (value: NativeHedgeCfm) : HedgeCfm =
        TraderBridgeGenerated.mapNative<HedgeCfm, NativeHedgeCfm> encoding value

    let hedgeCfmAction encoding (value: NativeHedgeCfmAction) : HedgeCfmAction =
        TraderBridgeGenerated.mapNative<HedgeCfmAction, NativeHedgeCfmAction> encoding value

    let inputBatchOrderAction encoding (value: NativeInputBatchOrderAction) : InputBatchOrderActionRequest =
        TraderBridgeGenerated.mapNative<InputBatchOrderActionRequest, NativeInputBatchOrderAction> encoding value

    let inputCombAction encoding (value: NativeInputCombAction) : InputCombActionRequest =
        TraderBridgeGenerated.mapNative<InputCombActionRequest, NativeInputCombAction> encoding value

    let inputExecOrder encoding (value: NativeInputExecOrder) : InputExecOrderRequest =
        TraderBridgeGenerated.mapNative<InputExecOrderRequest, NativeInputExecOrder> encoding value

    let inputExecOrderAction encoding (value: NativeInputExecOrderAction) : InputExecOrderActionRequest =
        TraderBridgeGenerated.mapNative<InputExecOrderActionRequest, NativeInputExecOrderAction> encoding value

    let inputForQuote encoding (value: NativeInputForQuote) : InputForQuoteRequest =
        TraderBridgeGenerated.mapNative<InputForQuoteRequest, NativeInputForQuote> encoding value

    let inputHedgeCfm encoding (value: NativeInputHedgeCfm) : InputHedgeCfmRequest =
        TraderBridgeGenerated.mapNative<InputHedgeCfmRequest, NativeInputHedgeCfm> encoding value

    let inputHedgeCfmAction encoding (value: NativeInputHedgeCfmAction) : InputHedgeCfmActionRequest =
        TraderBridgeGenerated.mapNative<InputHedgeCfmActionRequest, NativeInputHedgeCfmAction> encoding value

    let inputOffsetSetting encoding (value: NativeInputOffsetSetting) : InputOffsetSettingRequest =
        TraderBridgeGenerated.mapNative<InputOffsetSettingRequest, NativeInputOffsetSetting> encoding value

    let inputOptionSelfClose encoding (value: NativeInputOptionSelfClose) : InputOptionSelfCloseRequest =
        TraderBridgeGenerated.mapNative<InputOptionSelfCloseRequest, NativeInputOptionSelfClose> encoding value

    let inputOptionSelfCloseAction encoding (value: NativeInputOptionSelfCloseAction) : InputOptionSelfCloseActionRequest =
        TraderBridgeGenerated.mapNative<InputOptionSelfCloseActionRequest, NativeInputOptionSelfCloseAction> encoding value

    let inputQuote encoding (value: NativeInputQuote) : InputQuoteRequest =
        TraderBridgeGenerated.mapNative<InputQuoteRequest, NativeInputQuote> encoding value

    let inputQuoteAction encoding (value: NativeInputQuoteAction) : InputQuoteActionRequest =
        TraderBridgeGenerated.mapNative<InputQuoteActionRequest, NativeInputQuoteAction> encoding value

    let inputSpdApply encoding (value: NativeInputSpdApply) : InputSpdApplyRequest =
        TraderBridgeGenerated.mapNative<InputSpdApplyRequest, NativeInputSpdApply> encoding value

    let inputSpdApplyAction encoding (value: NativeInputSpdApplyAction) : InputSpdApplyActionRequest =
        TraderBridgeGenerated.mapNative<InputSpdApplyActionRequest, NativeInputSpdApplyAction> encoding value

    let instrument encoding (value: NativeInstrument) : Instrument =
        TraderBridgeGenerated.mapNative<Instrument, NativeInstrument> encoding value

    let instrumentOrderCommRate encoding (value: NativeInstrumentOrderCommRate) : InstrumentOrderCommRate =
        TraderBridgeGenerated.mapNative<InstrumentOrderCommRate, NativeInstrumentOrderCommRate> encoding value

    let investUnit encoding (value: NativeInvestUnit) : InvestUnit =
        TraderBridgeGenerated.mapNative<InvestUnit, NativeInvestUnit> encoding value

    let investor encoding (value: NativeInvestor) : Investor =
        TraderBridgeGenerated.mapNative<Investor, NativeInvestor> encoding value

    let investorCommodityGroupSpmmMargin encoding (value: NativeInvestorCommodityGroupSpmmMargin) : InvestorCommodityGroupSpmmMargin =
        TraderBridgeGenerated.mapNative<InvestorCommodityGroupSpmmMargin, NativeInvestorCommodityGroupSpmmMargin> encoding value

    let investorCommoditySpmmMargin encoding (value: NativeInvestorCommoditySpmmMargin) : InvestorCommoditySpmmMargin =
        TraderBridgeGenerated.mapNative<InvestorCommoditySpmmMargin, NativeInvestorCommoditySpmmMargin> encoding value

    let investorInfoCommRec encoding (value: NativeInvestorInfoCommRec) : InvestorInfoCommRec =
        TraderBridgeGenerated.mapNative<InvestorInfoCommRec, NativeInvestorInfoCommRec> encoding value

    let investorPortfMarginRatio encoding (value: NativeInvestorPortfMarginRatio) : InvestorPortfMarginRatio =
        TraderBridgeGenerated.mapNative<InvestorPortfMarginRatio, NativeInvestorPortfMarginRatio> encoding value

    let investorPortfSetting encoding (value: NativeInvestorPortfSetting) : InvestorPortfSetting =
        TraderBridgeGenerated.mapNative<InvestorPortfSetting, NativeInvestorPortfSetting> encoding value

    let investorPositionCombineDetail encoding (value: NativeInvestorPositionCombineDetail) : InvestorPositionCombineDetail =
        TraderBridgeGenerated.mapNative<InvestorPositionCombineDetail, NativeInvestorPositionCombineDetail> encoding value

    let investorPositionDetail encoding (value: NativeInvestorPositionDetail) : InvestorPositionDetail =
        TraderBridgeGenerated.mapNative<InvestorPositionDetail, NativeInvestorPositionDetail> encoding value

    let investorProdRcamsMargin encoding (value: NativeInvestorProdRcamsMargin) : InvestorProdRcamsMargin =
        TraderBridgeGenerated.mapNative<InvestorProdRcamsMargin, NativeInvestorProdRcamsMargin> encoding value

    let investorProdRuleMargin encoding (value: NativeInvestorProdRuleMargin) : InvestorProdRuleMargin =
        TraderBridgeGenerated.mapNative<InvestorProdRuleMargin, NativeInvestorProdRuleMargin> encoding value

    let investorProdSpbmDetail encoding (value: NativeInvestorProdSpbmDetail) : InvestorProdSpbmDetail =
        TraderBridgeGenerated.mapNative<InvestorProdSpbmDetail, NativeInvestorProdSpbmDetail> encoding value

    let investorProductGroupMargin encoding (value: NativeInvestorProductGroupMargin) : InvestorProductGroupMargin =
        TraderBridgeGenerated.mapNative<InvestorProductGroupMargin, NativeInvestorProductGroupMargin> encoding value

    let mmInstrumentCommissionRate encoding (value: NativeMmInstrumentCommissionRate) : MmInstrumentCommissionRate =
        TraderBridgeGenerated.mapNative<MmInstrumentCommissionRate, NativeMmInstrumentCommissionRate> encoding value

    let mmOptionInstrCommRate encoding (value: NativeMmOptionInstrCommRate) : MmOptionInstrCommRate =
        TraderBridgeGenerated.mapNative<MmOptionInstrCommRate, NativeMmOptionInstrCommRate> encoding value

    let notice encoding (value: NativeNotice) : Notice =
        TraderBridgeGenerated.mapNative<Notice, NativeNotice> encoding value

    let notifyQueryAccount encoding (value: NativeNotifyQueryAccount) : NotifyQueryAccount =
        TraderBridgeGenerated.mapNative<NotifyQueryAccount, NativeNotifyQueryAccount> encoding value

    let offsetSetting encoding (value: NativeOffsetSetting) : OffsetSetting =
        TraderBridgeGenerated.mapNative<OffsetSetting, NativeOffsetSetting> encoding value

    let optionInstrCommRate encoding (value: NativeOptionInstrCommRate) : OptionInstrCommRate =
        TraderBridgeGenerated.mapNative<OptionInstrCommRate, NativeOptionInstrCommRate> encoding value

    let optionInstrTradeCost encoding (value: NativeOptionInstrTradeCost) : OptionInstrTradeCost =
        TraderBridgeGenerated.mapNative<OptionInstrTradeCost, NativeOptionInstrTradeCost> encoding value

    let optionSelfClose encoding (value: NativeOptionSelfClose) : OptionSelfClose =
        TraderBridgeGenerated.mapNative<OptionSelfClose, NativeOptionSelfClose> encoding value

    let optionSelfCloseAction encoding (value: NativeOptionSelfCloseAction) : OptionSelfCloseAction =
        TraderBridgeGenerated.mapNative<OptionSelfCloseAction, NativeOptionSelfCloseAction> encoding value

    let orderAction encoding (value: NativeOrderAction) : OrderAction =
        TraderBridgeGenerated.mapNative<OrderAction, NativeOrderAction> encoding value

    let parkedOrder encoding (value: NativeParkedOrder) : ParkedOrder =
        TraderBridgeGenerated.mapNative<ParkedOrder, NativeParkedOrder> encoding value

    let parkedOrderAction encoding (value: NativeParkedOrderAction) : ParkedOrderAction =
        TraderBridgeGenerated.mapNative<ParkedOrderAction, NativeParkedOrderAction> encoding value

    let product encoding (value: NativeProduct) : Product =
        TraderBridgeGenerated.mapNative<Product, NativeProduct> encoding value

    let productExchRate encoding (value: NativeProductExchRate) : ProductExchRate =
        TraderBridgeGenerated.mapNative<ProductExchRate, NativeProductExchRate> encoding value

    let productGroup encoding (value: NativeProductGroup) : ProductGroup =
        TraderBridgeGenerated.mapNative<ProductGroup, NativeProductGroup> encoding value

    let qryAccountregister encoding (value: NativeQryAccountregister) : QryAccountregisterRequest =
        TraderBridgeGenerated.mapNative<QryAccountregisterRequest, NativeQryAccountregister> encoding value

    let qryBrokerTradingAlgos encoding (value: NativeQryBrokerTradingAlgos) : QryBrokerTradingAlgosRequest =
        TraderBridgeGenerated.mapNative<QryBrokerTradingAlgosRequest, NativeQryBrokerTradingAlgos> encoding value

    let qryBrokerTradingParams encoding (value: NativeQryBrokerTradingParams) : QryBrokerTradingParamsRequest =
        TraderBridgeGenerated.mapNative<QryBrokerTradingParamsRequest, NativeQryBrokerTradingParams> encoding value

    let qryCfmmcTradingAccountKey encoding (value: NativeQryCfmmcTradingAccountKey) : QryCfmmcTradingAccountKeyRequest =
        TraderBridgeGenerated.mapNative<QryCfmmcTradingAccountKeyRequest, NativeQryCfmmcTradingAccountKey> encoding value

    let qryClassifiedInstrument encoding (value: NativeQryClassifiedInstrument) : QryClassifiedInstrumentRequest =
        TraderBridgeGenerated.mapNative<QryClassifiedInstrumentRequest, NativeQryClassifiedInstrument> encoding value

    let qryCombAction encoding (value: NativeQryCombAction) : QryCombActionRequest =
        TraderBridgeGenerated.mapNative<QryCombActionRequest, NativeQryCombAction> encoding value

    let qryCombInstrumentGuard encoding (value: NativeQryCombInstrumentGuard) : QryCombInstrumentGuardRequest =
        TraderBridgeGenerated.mapNative<QryCombInstrumentGuardRequest, NativeQryCombInstrumentGuard> encoding value

    let qryCombLeg encoding (value: NativeQryCombLeg) : QryCombLeg =
        TraderBridgeGenerated.mapNative<QryCombLeg, NativeQryCombLeg> encoding value

    let qryCombPromotionParam encoding (value: NativeQryCombPromotionParam) : QryCombPromotionParamRequest =
        TraderBridgeGenerated.mapNative<QryCombPromotionParamRequest, NativeQryCombPromotionParam> encoding value

    let qryContractBank encoding (value: NativeQryContractBank) : QryContractBankRequest =
        TraderBridgeGenerated.mapNative<QryContractBankRequest, NativeQryContractBank> encoding value

    let qryDepthMarketData encoding (value: NativeQryDepthMarketData) : QryDepthMarketDataRequest =
        TraderBridgeGenerated.mapNative<QryDepthMarketDataRequest, NativeQryDepthMarketData> encoding value

    let qryEWarrantOffset encoding (value: NativeQryEWarrantOffset) : QryEWarrantOffsetRequest =
        TraderBridgeGenerated.mapNative<QryEWarrantOffsetRequest, NativeQryEWarrantOffset> encoding value

    let qryExchange encoding (value: NativeQryExchange) : QryExchangeRequest =
        TraderBridgeGenerated.mapNative<QryExchangeRequest, NativeQryExchange> encoding value

    let qryExchangeMarginRateAdjust encoding (value: NativeQryExchangeMarginRateAdjust) : QryExchangeMarginRateAdjustRequest =
        TraderBridgeGenerated.mapNative<QryExchangeMarginRateAdjustRequest, NativeQryExchangeMarginRateAdjust> encoding value

    let qryExchangeRate encoding (value: NativeQryExchangeRate) : QryExchangeRate =
        TraderBridgeGenerated.mapNative<QryExchangeRate, NativeQryExchangeRate> encoding value

    let qryExecOrder encoding (value: NativeQryExecOrder) : QryExecOrderRequest =
        TraderBridgeGenerated.mapNative<QryExecOrderRequest, NativeQryExecOrder> encoding value

    let qryForQuote encoding (value: NativeQryForQuote) : QryForQuoteRequest =
        TraderBridgeGenerated.mapNative<QryForQuoteRequest, NativeQryForQuote> encoding value

    let qryHedgeCfm encoding (value: NativeQryHedgeCfm) : QryHedgeCfmRequest =
        TraderBridgeGenerated.mapNative<QryHedgeCfmRequest, NativeQryHedgeCfm> encoding value

    let qryInstrument encoding (value: NativeQryInstrument) : QryInstrumentRequest =
        TraderBridgeGenerated.mapNative<QryInstrumentRequest, NativeQryInstrument> encoding value

    let qryInstrumentOrderCommRate encoding (value: NativeQryInstrumentOrderCommRate) : QryInstrumentOrderCommRateRequest =
        TraderBridgeGenerated.mapNative<QryInstrumentOrderCommRateRequest, NativeQryInstrumentOrderCommRate> encoding value

    let qryInvestUnit encoding (value: NativeQryInvestUnit) : QryInvestUnitRequest =
        TraderBridgeGenerated.mapNative<QryInvestUnitRequest, NativeQryInvestUnit> encoding value

    let qryInvestor encoding (value: NativeQryInvestor) : QryInvestorRequest =
        TraderBridgeGenerated.mapNative<QryInvestorRequest, NativeQryInvestor> encoding value

    let qryInvestorCommodityGroupSpmmMargin encoding (value: NativeQryInvestorCommodityGroupSpmmMargin) : QryInvestorCommodityGroupSpmmMarginRequest =
        TraderBridgeGenerated.mapNative<QryInvestorCommodityGroupSpmmMarginRequest, NativeQryInvestorCommodityGroupSpmmMargin> encoding value

    let qryInvestorCommoditySpmmMargin encoding (value: NativeQryInvestorCommoditySpmmMargin) : QryInvestorCommoditySpmmMarginRequest =
        TraderBridgeGenerated.mapNative<QryInvestorCommoditySpmmMarginRequest, NativeQryInvestorCommoditySpmmMargin> encoding value

    let qryInvestorInfoCommRec encoding (value: NativeQryInvestorInfoCommRec) : QryInvestorInfoCommRecRequest =
        TraderBridgeGenerated.mapNative<QryInvestorInfoCommRecRequest, NativeQryInvestorInfoCommRec> encoding value

    let qryInvestorPortfMarginRatio encoding (value: NativeQryInvestorPortfMarginRatio) : QryInvestorPortfMarginRatioRequest =
        TraderBridgeGenerated.mapNative<QryInvestorPortfMarginRatioRequest, NativeQryInvestorPortfMarginRatio> encoding value

    let qryInvestorPortfSetting encoding (value: NativeQryInvestorPortfSetting) : QryInvestorPortfSettingRequest =
        TraderBridgeGenerated.mapNative<QryInvestorPortfSettingRequest, NativeQryInvestorPortfSetting> encoding value

    let qryInvestorPositionCombineDetail encoding (value: NativeQryInvestorPositionCombineDetail) : QryInvestorPositionCombineDetailRequest =
        TraderBridgeGenerated.mapNative<QryInvestorPositionCombineDetailRequest, NativeQryInvestorPositionCombineDetail> encoding value

    let qryInvestorPositionDetail encoding (value: NativeQryInvestorPositionDetail) : QryInvestorPositionDetailRequest =
        TraderBridgeGenerated.mapNative<QryInvestorPositionDetailRequest, NativeQryInvestorPositionDetail> encoding value

    let qryInvestorProdRcamsMargin encoding (value: NativeQryInvestorProdRcamsMargin) : QryInvestorProdRcamsMarginRequest =
        TraderBridgeGenerated.mapNative<QryInvestorProdRcamsMarginRequest, NativeQryInvestorProdRcamsMargin> encoding value

    let qryInvestorProdRuleMargin encoding (value: NativeQryInvestorProdRuleMargin) : QryInvestorProdRuleMarginRequest =
        TraderBridgeGenerated.mapNative<QryInvestorProdRuleMarginRequest, NativeQryInvestorProdRuleMargin> encoding value

    let qryInvestorProdSpbmDetail encoding (value: NativeQryInvestorProdSpbmDetail) : QryInvestorProdSpbmDetailRequest =
        TraderBridgeGenerated.mapNative<QryInvestorProdSpbmDetailRequest, NativeQryInvestorProdSpbmDetail> encoding value

    let qryInvestorProductGroupMargin encoding (value: NativeQryInvestorProductGroupMargin) : QryInvestorProductGroupMarginRequest =
        TraderBridgeGenerated.mapNative<QryInvestorProductGroupMarginRequest, NativeQryInvestorProductGroupMargin> encoding value

    let qryMaxOrderVolume encoding (value: NativeQryMaxOrderVolume) : QryMaxOrderVolumeRequest =
        TraderBridgeGenerated.mapNative<QryMaxOrderVolumeRequest, NativeQryMaxOrderVolume> encoding value

    let qryMmInstrumentCommissionRate encoding (value: NativeQryMmInstrumentCommissionRate) : QryMmInstrumentCommissionRateRequest =
        TraderBridgeGenerated.mapNative<QryMmInstrumentCommissionRateRequest, NativeQryMmInstrumentCommissionRate> encoding value

    let qryMmOptionInstrCommRate encoding (value: NativeQryMmOptionInstrCommRate) : QryMmOptionInstrCommRateRequest =
        TraderBridgeGenerated.mapNative<QryMmOptionInstrCommRateRequest, NativeQryMmOptionInstrCommRate> encoding value

    let qryNotice encoding (value: NativeQryNotice) : QryNoticeRequest =
        TraderBridgeGenerated.mapNative<QryNoticeRequest, NativeQryNotice> encoding value

    let qryOffsetSetting encoding (value: NativeQryOffsetSetting) : QryOffsetSettingRequest =
        TraderBridgeGenerated.mapNative<QryOffsetSettingRequest, NativeQryOffsetSetting> encoding value

    let qryOptionInstrCommRate encoding (value: NativeQryOptionInstrCommRate) : QryOptionInstrCommRateRequest =
        TraderBridgeGenerated.mapNative<QryOptionInstrCommRateRequest, NativeQryOptionInstrCommRate> encoding value

    let qryOptionInstrTradeCost encoding (value: NativeQryOptionInstrTradeCost) : QryOptionInstrTradeCostRequest =
        TraderBridgeGenerated.mapNative<QryOptionInstrTradeCostRequest, NativeQryOptionInstrTradeCost> encoding value

    let qryOptionSelfClose encoding (value: NativeQryOptionSelfClose) : QryOptionSelfCloseRequest =
        TraderBridgeGenerated.mapNative<QryOptionSelfCloseRequest, NativeQryOptionSelfClose> encoding value

    let qryOrder encoding (value: NativeQryOrder) : QryOrderRequest =
        TraderBridgeGenerated.mapNative<QryOrderRequest, NativeQryOrder> encoding value

    let qryParkedOrder encoding (value: NativeQryParkedOrder) : QryParkedOrderRequest =
        TraderBridgeGenerated.mapNative<QryParkedOrderRequest, NativeQryParkedOrder> encoding value

    let qryParkedOrderAction encoding (value: NativeQryParkedOrderAction) : QryParkedOrderActionRequest =
        TraderBridgeGenerated.mapNative<QryParkedOrderActionRequest, NativeQryParkedOrderAction> encoding value

    let qryProduct encoding (value: NativeQryProduct) : QryProductRequest =
        TraderBridgeGenerated.mapNative<QryProductRequest, NativeQryProduct> encoding value

    let qryProductExchRate encoding (value: NativeQryProductExchRate) : QryProductExchRateRequest =
        TraderBridgeGenerated.mapNative<QryProductExchRateRequest, NativeQryProductExchRate> encoding value

    let qryProductGroup encoding (value: NativeQryProductGroup) : QryProductGroupRequest =
        TraderBridgeGenerated.mapNative<QryProductGroupRequest, NativeQryProductGroup> encoding value

    let qryQuote encoding (value: NativeQryQuote) : QryQuoteRequest =
        TraderBridgeGenerated.mapNative<QryQuoteRequest, NativeQryQuote> encoding value

    let qryRcamsCombProductInfo encoding (value: NativeQryRcamsCombProductInfo) : QryRcamsCombProductInfoRequest =
        TraderBridgeGenerated.mapNative<QryRcamsCombProductInfoRequest, NativeQryRcamsCombProductInfo> encoding value

    let qryRcamsInstrParameter encoding (value: NativeQryRcamsInstrParameter) : QryRcamsInstrParameterRequest =
        TraderBridgeGenerated.mapNative<QryRcamsInstrParameterRequest, NativeQryRcamsInstrParameter> encoding value

    let qryRcamsInterParameter encoding (value: NativeQryRcamsInterParameter) : QryRcamsInterParameterRequest =
        TraderBridgeGenerated.mapNative<QryRcamsInterParameterRequest, NativeQryRcamsInterParameter> encoding value

    let qryRcamsIntraParameter encoding (value: NativeQryRcamsIntraParameter) : QryRcamsIntraParameterRequest =
        TraderBridgeGenerated.mapNative<QryRcamsIntraParameterRequest, NativeQryRcamsIntraParameter> encoding value

    let qryRcamsInvestorCombPosition encoding (value: NativeQryRcamsInvestorCombPosition) : QryRcamsInvestorCombPositionRequest =
        TraderBridgeGenerated.mapNative<QryRcamsInvestorCombPositionRequest, NativeQryRcamsInvestorCombPosition> encoding value

    let qryRcamsShortOptAdjustParam encoding (value: NativeQryRcamsShortOptAdjustParam) : QryRcamsShortOptAdjustParamRequest =
        TraderBridgeGenerated.mapNative<QryRcamsShortOptAdjustParamRequest, NativeQryRcamsShortOptAdjustParam> encoding value

    let qryRiskSettleInvstPosition encoding (value: NativeQryRiskSettleInvstPosition) : QryRiskSettleInvstPositionRequest =
        TraderBridgeGenerated.mapNative<QryRiskSettleInvstPositionRequest, NativeQryRiskSettleInvstPosition> encoding value

    let qryRiskSettleProductStatus encoding (value: NativeQryRiskSettleProductStatus) : QryRiskSettleProductStatusRequest =
        TraderBridgeGenerated.mapNative<QryRiskSettleProductStatusRequest, NativeQryRiskSettleProductStatus> encoding value

    let qryRuleInstrParameter encoding (value: NativeQryRuleInstrParameter) : QryRuleInstrParameterRequest =
        TraderBridgeGenerated.mapNative<QryRuleInstrParameterRequest, NativeQryRuleInstrParameter> encoding value

    let qryRuleInterParameter encoding (value: NativeQryRuleInterParameter) : QryRuleInterParameterRequest =
        TraderBridgeGenerated.mapNative<QryRuleInterParameterRequest, NativeQryRuleInterParameter> encoding value

    let qryRuleIntraParameter encoding (value: NativeQryRuleIntraParameter) : QryRuleIntraParameterRequest =
        TraderBridgeGenerated.mapNative<QryRuleIntraParameterRequest, NativeQryRuleIntraParameter> encoding value

    let qrySecAgentAcIdMap encoding (value: NativeQrySecAgentAcIdMap) : QrySecAgentAcIdMapRequest =
        TraderBridgeGenerated.mapNative<QrySecAgentAcIdMapRequest, NativeQrySecAgentAcIdMap> encoding value

    let qrySecAgentCheckMode encoding (value: NativeQrySecAgentCheckMode) : QrySecAgentCheckModeRequest =
        TraderBridgeGenerated.mapNative<QrySecAgentCheckModeRequest, NativeQrySecAgentCheckMode> encoding value

    let qrySecAgentTradeInfo encoding (value: NativeQrySecAgentTradeInfo) : QrySecAgentTradeInfoRequest =
        TraderBridgeGenerated.mapNative<QrySecAgentTradeInfoRequest, NativeQrySecAgentTradeInfo> encoding value

    let qrySettlementInfo encoding (value: NativeQrySettlementInfo) : QrySettlementInfoRequest =
        TraderBridgeGenerated.mapNative<QrySettlementInfoRequest, NativeQrySettlementInfo> encoding value

    let qrySettlementInfoConfirm encoding (value: NativeQrySettlementInfoConfirm) : QrySettlementInfoConfirmRequest =
        TraderBridgeGenerated.mapNative<QrySettlementInfoConfirmRequest, NativeQrySettlementInfoConfirm> encoding value

    let qrySpbmAddOnInterParameter encoding (value: NativeQrySpbmAddOnInterParameter) : QrySpbmAddOnInterParameterRequest =
        TraderBridgeGenerated.mapNative<QrySpbmAddOnInterParameterRequest, NativeQrySpbmAddOnInterParameter> encoding value

    let qrySpbmFutureParameter encoding (value: NativeQrySpbmFutureParameter) : QrySpbmFutureParameterRequest =
        TraderBridgeGenerated.mapNative<QrySpbmFutureParameterRequest, NativeQrySpbmFutureParameter> encoding value

    let qrySpbmInterParameter encoding (value: NativeQrySpbmInterParameter) : QrySpbmInterParameterRequest =
        TraderBridgeGenerated.mapNative<QrySpbmInterParameterRequest, NativeQrySpbmInterParameter> encoding value

    let qrySpbmIntraParameter encoding (value: NativeQrySpbmIntraParameter) : QrySpbmIntraParameterRequest =
        TraderBridgeGenerated.mapNative<QrySpbmIntraParameterRequest, NativeQrySpbmIntraParameter> encoding value

    let qrySpbmInvestorPortfDef encoding (value: NativeQrySpbmInvestorPortfDef) : QrySpbmInvestorPortfDefRequest =
        TraderBridgeGenerated.mapNative<QrySpbmInvestorPortfDefRequest, NativeQrySpbmInvestorPortfDef> encoding value

    let qrySpbmOptionParameter encoding (value: NativeQrySpbmOptionParameter) : QrySpbmOptionParameterRequest =
        TraderBridgeGenerated.mapNative<QrySpbmOptionParameterRequest, NativeQrySpbmOptionParameter> encoding value

    let qrySpbmPortfDefinition encoding (value: NativeQrySpbmPortfDefinition) : QrySpbmPortfDefinitionRequest =
        TraderBridgeGenerated.mapNative<QrySpbmPortfDefinitionRequest, NativeQrySpbmPortfDefinition> encoding value

    let qrySpdApply encoding (value: NativeQrySpdApply) : QrySpdApplyRequest =
        TraderBridgeGenerated.mapNative<QrySpdApplyRequest, NativeQrySpdApply> encoding value

    let qrySpmmInstParam encoding (value: NativeQrySpmmInstParam) : QrySpmmInstParamRequest =
        TraderBridgeGenerated.mapNative<QrySpmmInstParamRequest, NativeQrySpmmInstParam> encoding value

    let qrySpmmProductParam encoding (value: NativeQrySpmmProductParam) : QrySpmmProductParamRequest =
        TraderBridgeGenerated.mapNative<QrySpmmProductParamRequest, NativeQrySpmmProductParam> encoding value

    let qryTrade encoding (value: NativeQryTrade) : QryTradeRequest =
        TraderBridgeGenerated.mapNative<QryTradeRequest, NativeQryTrade> encoding value

    let qryTraderOffer encoding (value: NativeQryTraderOffer) : QryTraderOfferRequest =
        TraderBridgeGenerated.mapNative<QryTraderOfferRequest, NativeQryTraderOffer> encoding value

    let qryTradingCode encoding (value: NativeQryTradingCode) : QryTradingCodeRequest =
        TraderBridgeGenerated.mapNative<QryTradingCodeRequest, NativeQryTradingCode> encoding value

    let qryTradingNotice encoding (value: NativeQryTradingNotice) : QryTradingNoticeRequest =
        TraderBridgeGenerated.mapNative<QryTradingNoticeRequest, NativeQryTradingNotice> encoding value

    let qryTransferBank encoding (value: NativeQryTransferBank) : QryTransferBankRequest =
        TraderBridgeGenerated.mapNative<QryTransferBankRequest, NativeQryTransferBank> encoding value

    let qryTransferSerial encoding (value: NativeQryTransferSerial) : QryTransferSerialRequest =
        TraderBridgeGenerated.mapNative<QryTransferSerialRequest, NativeQryTransferSerial> encoding value

    let qryUserSession encoding (value: NativeQryUserSession) : QryUserSessionRequest =
        TraderBridgeGenerated.mapNative<QryUserSessionRequest, NativeQryUserSession> encoding value

    let queryCfmmcTradingAccountToken encoding (value: NativeQueryCfmmcTradingAccountToken) : QueryCfmmcTradingAccountTokenRequest =
        TraderBridgeGenerated.mapNative<QueryCfmmcTradingAccountTokenRequest, NativeQueryCfmmcTradingAccountToken> encoding value

    let quote encoding (value: NativeQuote) : Quote =
        TraderBridgeGenerated.mapNative<Quote, NativeQuote> encoding value

    let quoteAction encoding (value: NativeQuoteAction) : QuoteAction =
        TraderBridgeGenerated.mapNative<QuoteAction, NativeQuoteAction> encoding value

    let rcamsCombProductInfo encoding (value: NativeRcamsCombProductInfo) : RcamsCombProductInfo =
        TraderBridgeGenerated.mapNative<RcamsCombProductInfo, NativeRcamsCombProductInfo> encoding value

    let rcamsInstrParameter encoding (value: NativeRcamsInstrParameter) : RcamsInstrParameter =
        TraderBridgeGenerated.mapNative<RcamsInstrParameter, NativeRcamsInstrParameter> encoding value

    let rcamsInterParameter encoding (value: NativeRcamsInterParameter) : RcamsInterParameter =
        TraderBridgeGenerated.mapNative<RcamsInterParameter, NativeRcamsInterParameter> encoding value

    let rcamsIntraParameter encoding (value: NativeRcamsIntraParameter) : RcamsIntraParameter =
        TraderBridgeGenerated.mapNative<RcamsIntraParameter, NativeRcamsIntraParameter> encoding value

    let rcamsInvestorCombPosition encoding (value: NativeRcamsInvestorCombPosition) : RcamsInvestorCombPosition =
        TraderBridgeGenerated.mapNative<RcamsInvestorCombPosition, NativeRcamsInvestorCombPosition> encoding value

    let rcamsShortOptAdjustParam encoding (value: NativeRcamsShortOptAdjustParam) : RcamsShortOptAdjustParam =
        TraderBridgeGenerated.mapNative<RcamsShortOptAdjustParam, NativeRcamsShortOptAdjustParam> encoding value

    let removeParkedOrder encoding (value: NativeRemoveParkedOrder) : RemoveParkedOrder =
        TraderBridgeGenerated.mapNative<RemoveParkedOrder, NativeRemoveParkedOrder> encoding value

    let removeParkedOrderAction encoding (value: NativeRemoveParkedOrderAction) : RemoveParkedOrderAction =
        TraderBridgeGenerated.mapNative<RemoveParkedOrderAction, NativeRemoveParkedOrderAction> encoding value

    let reqGenSmsCode encoding (value: NativeReqGenSmsCode) : GenSmsCodeRequest =
        TraderBridgeGenerated.mapNative<GenSmsCodeRequest, NativeReqGenSmsCode> encoding value

    let reqGenUserCaptcha encoding (value: NativeReqGenUserCaptcha) : GenUserCaptchaRequest =
        TraderBridgeGenerated.mapNative<GenUserCaptchaRequest, NativeReqGenUserCaptcha> encoding value

    let reqGenUserText encoding (value: NativeReqGenUserText) : GenUserTextRequest =
        TraderBridgeGenerated.mapNative<GenUserTextRequest, NativeReqGenUserText> encoding value

    let reqQueryAccount encoding (value: NativeReqQueryAccount) : ReqQueryAccount =
        TraderBridgeGenerated.mapNative<ReqQueryAccount, NativeReqQueryAccount> encoding value

    let reqTransfer encoding (value: NativeReqTransfer) : TransferRequest =
        TraderBridgeGenerated.mapNative<TransferRequest, NativeReqTransfer> encoding value

    let reqUserAuthMethod encoding (value: NativeReqUserAuthMethod) : UserAuthMethodRequest =
        TraderBridgeGenerated.mapNative<UserAuthMethodRequest, NativeReqUserAuthMethod> encoding value

    let reqUserLoginWithCaptcha encoding (value: NativeReqUserLoginWithCaptcha) : UserLoginWithCaptchaRequest =
        TraderBridgeGenerated.mapNative<UserLoginWithCaptchaRequest, NativeReqUserLoginWithCaptcha> encoding value

    let reqUserLoginWithOtp encoding (value: NativeReqUserLoginWithOtp) : UserLoginWithOtpRequest =
        TraderBridgeGenerated.mapNative<UserLoginWithOtpRequest, NativeReqUserLoginWithOtp> encoding value

    let reqUserLoginWithText encoding (value: NativeReqUserLoginWithText) : UserLoginWithTextRequest =
        TraderBridgeGenerated.mapNative<UserLoginWithTextRequest, NativeReqUserLoginWithText> encoding value

    let riskSettleInvstPosition encoding (value: NativeRiskSettleInvstPosition) : RiskSettleInvstPosition =
        TraderBridgeGenerated.mapNative<RiskSettleInvstPosition, NativeRiskSettleInvstPosition> encoding value

    let riskSettleProductStatus encoding (value: NativeRiskSettleProductStatus) : RiskSettleProductStatus =
        TraderBridgeGenerated.mapNative<RiskSettleProductStatus, NativeRiskSettleProductStatus> encoding value

    let rspGenSmsCode encoding (value: NativeRspGenSmsCode) : RspGenSmsCode =
        TraderBridgeGenerated.mapNative<RspGenSmsCode, NativeRspGenSmsCode> encoding value

    let rspGenUserCaptcha encoding (value: NativeRspGenUserCaptcha) : RspGenUserCaptcha =
        TraderBridgeGenerated.mapNative<RspGenUserCaptcha, NativeRspGenUserCaptcha> encoding value

    let rspGenUserText encoding (value: NativeRspGenUserText) : RspGenUserText =
        TraderBridgeGenerated.mapNative<RspGenUserText, NativeRspGenUserText> encoding value

    let rspTransfer encoding (value: NativeRspTransfer) : RspTransfer =
        TraderBridgeGenerated.mapNative<RspTransfer, NativeRspTransfer> encoding value

    let rspUserAuthMethod encoding (value: NativeRspUserAuthMethod) : RspUserAuthMethod =
        TraderBridgeGenerated.mapNative<RspUserAuthMethod, NativeRspUserAuthMethod> encoding value

    let ruleInstrParameter encoding (value: NativeRuleInstrParameter) : RuleInstrParameter =
        TraderBridgeGenerated.mapNative<RuleInstrParameter, NativeRuleInstrParameter> encoding value

    let ruleInterParameter encoding (value: NativeRuleInterParameter) : RuleInterParameter =
        TraderBridgeGenerated.mapNative<RuleInterParameter, NativeRuleInterParameter> encoding value

    let ruleIntraParameter encoding (value: NativeRuleIntraParameter) : RuleIntraParameter =
        TraderBridgeGenerated.mapNative<RuleIntraParameter, NativeRuleIntraParameter> encoding value

    let secAgentAcIdMap encoding (value: NativeSecAgentAcIdMap) : SecAgentAcIdMap =
        TraderBridgeGenerated.mapNative<SecAgentAcIdMap, NativeSecAgentAcIdMap> encoding value

    let secAgentCheckMode encoding (value: NativeSecAgentCheckMode) : SecAgentCheckMode =
        TraderBridgeGenerated.mapNative<SecAgentCheckMode, NativeSecAgentCheckMode> encoding value

    let secAgentTradeInfo encoding (value: NativeSecAgentTradeInfo) : SecAgentTradeInfo =
        TraderBridgeGenerated.mapNative<SecAgentTradeInfo, NativeSecAgentTradeInfo> encoding value

    let settlementInfo encoding (value: NativeSettlementInfo) : SettlementInfo =
        TraderBridgeGenerated.mapNative<SettlementInfo, NativeSettlementInfo> encoding value

    let spbmAddOnInterParameter encoding (value: NativeSpbmAddOnInterParameter) : SpbmAddOnInterParameter =
        TraderBridgeGenerated.mapNative<SpbmAddOnInterParameter, NativeSpbmAddOnInterParameter> encoding value

    let spbmFutureParameter encoding (value: NativeSpbmFutureParameter) : SpbmFutureParameter =
        TraderBridgeGenerated.mapNative<SpbmFutureParameter, NativeSpbmFutureParameter> encoding value

    let spbmInterParameter encoding (value: NativeSpbmInterParameter) : SpbmInterParameter =
        TraderBridgeGenerated.mapNative<SpbmInterParameter, NativeSpbmInterParameter> encoding value

    let spbmIntraParameter encoding (value: NativeSpbmIntraParameter) : SpbmIntraParameter =
        TraderBridgeGenerated.mapNative<SpbmIntraParameter, NativeSpbmIntraParameter> encoding value

    let spbmInvestorPortfDef encoding (value: NativeSpbmInvestorPortfDef) : SpbmInvestorPortfDef =
        TraderBridgeGenerated.mapNative<SpbmInvestorPortfDef, NativeSpbmInvestorPortfDef> encoding value

    let spbmOptionParameter encoding (value: NativeSpbmOptionParameter) : SpbmOptionParameter =
        TraderBridgeGenerated.mapNative<SpbmOptionParameter, NativeSpbmOptionParameter> encoding value

    let spbmPortfDefinition encoding (value: NativeSpbmPortfDefinition) : SpbmPortfDefinition =
        TraderBridgeGenerated.mapNative<SpbmPortfDefinition, NativeSpbmPortfDefinition> encoding value

    let spdApply encoding (value: NativeSpdApply) : SpdApply =
        TraderBridgeGenerated.mapNative<SpdApply, NativeSpdApply> encoding value

    let spdApplyAction encoding (value: NativeSpdApplyAction) : SpdApplyAction =
        TraderBridgeGenerated.mapNative<SpdApplyAction, NativeSpdApplyAction> encoding value

    let spmmInstParam encoding (value: NativeSpmmInstParam) : SpmmInstParam =
        TraderBridgeGenerated.mapNative<SpmmInstParam, NativeSpmmInstParam> encoding value

    let spmmProductParam encoding (value: NativeSpmmProductParam) : SpmmProductParam =
        TraderBridgeGenerated.mapNative<SpmmProductParam, NativeSpmmProductParam> encoding value

    let traderOffer encoding (value: NativeTraderOffer) : TraderOffer =
        TraderBridgeGenerated.mapNative<TraderOffer, NativeTraderOffer> encoding value

    let tradingAccountPasswordUpdate encoding (value: NativeTradingAccountPasswordUpdate) : TradingAccountPasswordUpdate =
        TraderBridgeGenerated.mapNative<TradingAccountPasswordUpdate, NativeTradingAccountPasswordUpdate> encoding value

    let tradingCode encoding (value: NativeTradingCode) : TradingCode =
        TraderBridgeGenerated.mapNative<TradingCode, NativeTradingCode> encoding value

    let tradingNotice encoding (value: NativeTradingNotice) : TradingNotice =
        TraderBridgeGenerated.mapNative<TradingNotice, NativeTradingNotice> encoding value

    let transferBank encoding (value: NativeTransferBank) : TransferBank =
        TraderBridgeGenerated.mapNative<TransferBank, NativeTransferBank> encoding value

    let transferSerial encoding (value: NativeTransferSerial) : TransferSerial =
        TraderBridgeGenerated.mapNative<TransferSerial, NativeTransferSerial> encoding value

    let userPasswordUpdate encoding (value: NativeUserPasswordUpdate) : UserPasswordUpdate =
        TraderBridgeGenerated.mapNative<UserPasswordUpdate, NativeUserPasswordUpdate> encoding value

    let userSession encoding (value: NativeUserSession) : UserSession =
        TraderBridgeGenerated.mapNative<UserSession, NativeUserSession> encoding value

    let userSystemInfo encoding (value: NativeUserSystemInfo) : UserSystemInfo =
        TraderBridgeGenerated.mapNative<UserSystemInfo, NativeUserSystemInfo> encoding value

    let wechatUserSystemInfo encoding (value: NativeWechatUserSystemInfo) : WechatUserSystemInfo =
        TraderBridgeGenerated.mapNative<WechatUserSystemInfo, NativeWechatUserSystemInfo> encoding value

module private TraderBridgeBuilders =
    let reqAuthenticate encoding (request: AuthenticateRequest) =
        let mutable native = NativeReqAuthenticate()
        native.BrokerId <- EncodingHelpers.encodeFixed encoding 11 (Some request.BrokerId)
        native.UserId <- EncodingHelpers.encodeFixed encoding 16 (Some request.UserId)
        native.UserProductInfo <- EncodingHelpers.encodeFixed encoding 11 request.UserProductInfo
        native.AuthCode <- EncodingHelpers.encodeFixed encoding 17 (Some request.AuthCode)
        native.AppId <- EncodingHelpers.encodeFixed encoding 33 (Some request.AppId)
        native

    let settlementInfoConfirm encoding (request: SettlementInfoConfirm) =
        let mutable native = NativeSettlementInfoConfirm()
        native.BrokerId <- EncodingHelpers.encodeFixed encoding 11 (Some request.BrokerId)
        native.InvestorId <- EncodingHelpers.encodeFixed encoding 13 (Some request.InvestorId)
        native.ConfirmDate <- EncodingHelpers.encodeFixed encoding 9 (Some request.ConfirmDate)
        native.ConfirmTime <- EncodingHelpers.encodeFixed encoding 9 (Some request.ConfirmTime)
        native.SettlementId <- request.SettlementId
        native.AccountId <- EncodingHelpers.encodeFixed encoding 13 (Some request.AccountId)
        native.CurrencyId <- EncodingHelpers.encodeFixed encoding 4 (Some request.CurrencyId)
        native

    let qryTradingAccount encoding (request: QueryTradingAccountRequest) : NativeQryTradingAccount =
        let mutable native = NativeQryTradingAccount()
        native.BrokerId <- EncodingHelpers.encodeFixed encoding 11 (Some request.BrokerId)
        native.InvestorId <- EncodingHelpers.encodeFixed encoding 13 (Some request.InvestorId)
        native.CurrencyId <- EncodingHelpers.encodeFixed encoding 4 request.CurrencyId
        native.BizType <- EncodingHelpers.charToByte request.BizType
        native.AccountId <- EncodingHelpers.encodeFixed encoding 13 request.AccountId
        native

    let qryInvestorPosition encoding (request: QueryInvestorPositionRequest) : NativeQryInvestorPosition =
        let mutable native = NativeQryInvestorPosition()
        native.BrokerId <- EncodingHelpers.encodeFixed encoding 11 (Some request.BrokerId)
        native.InvestorId <- EncodingHelpers.encodeFixed encoding 13 (Some request.InvestorId)
        native.ExchangeId <- EncodingHelpers.encodeFixed encoding 9 request.ExchangeId
        native.InvestUnitId <- EncodingHelpers.encodeFixed encoding 17 request.InvestUnitId
        native.InstrumentId <- EncodingHelpers.encodeFixed encoding 81 request.InstrumentId
        native

    let qryInstrumentMarginRate encoding (request: QueryInstrumentMarginRateRequest) : NativeQryInstrumentMarginRate =
        let mutable native = NativeQryInstrumentMarginRate()
        native.BrokerId <- EncodingHelpers.encodeFixed encoding 11 (Some request.BrokerId)
        native.InvestorId <- EncodingHelpers.encodeFixed encoding 13 (Some request.InvestorId)
        native.Reserve1 <- EncodingHelpers.encodeFixed encoding 31 None
        native.HedgeFlag <- EncodingHelpers.charToByte (Some request.HedgeFlag)
        native.ExchangeId <- EncodingHelpers.encodeFixed encoding 9 request.ExchangeId
        native.InvestUnitId <- EncodingHelpers.encodeFixed encoding 17 request.InvestUnitId
        native.InstrumentId <- EncodingHelpers.encodeFixed encoding 81 (Some request.InstrumentId)
        native

    let qryExchangeMarginRate encoding (request: QueryExchangeMarginRateRequest) : NativeQryExchangeMarginRate =
        let mutable native = NativeQryExchangeMarginRate()
        native.BrokerId <- EncodingHelpers.encodeFixed encoding 11 (Some request.BrokerId)
        native.Reserve1 <- EncodingHelpers.encodeFixed encoding 31 None
        native.HedgeFlag <- EncodingHelpers.charToByte (Some request.HedgeFlag)
        native.ExchangeId <- EncodingHelpers.encodeFixed encoding 9 request.ExchangeId
        native.InstrumentId <- EncodingHelpers.encodeFixed encoding 81 (Some request.InstrumentId)
        native

    let qryInstrumentCommissionRate encoding (request: QueryInstrumentCommissionRateRequest) : NativeQryInstrumentCommissionRate =
        let mutable native = NativeQryInstrumentCommissionRate()
        native.BrokerId <- EncodingHelpers.encodeFixed encoding 11 (Some request.BrokerId)
        native.InvestorId <- EncodingHelpers.encodeFixed encoding 13 (Some request.InvestorId)
        native.Reserve1 <- EncodingHelpers.encodeFixed encoding 31 None
        native.ExchangeId <- EncodingHelpers.encodeFixed encoding 9 request.ExchangeId
        native.InvestUnitId <- EncodingHelpers.encodeFixed encoding 17 request.InvestUnitId
        native.InstrumentId <- EncodingHelpers.encodeFixed encoding 81 (Some request.InstrumentId)
        native

    let inputOrder encoding requestId (request: InputOrderRequest) =
        let mutable native = NativeInputOrder()
        native.BrokerId <- EncodingHelpers.encodeFixed encoding 11 (Some request.BrokerId)
        native.InvestorId <- EncodingHelpers.encodeFixed encoding 13 (Some request.InvestorId)
        native.InstrumentId <- EncodingHelpers.encodeFixed encoding 81 (Some request.InstrumentId)
        native.OrderRef <- EncodingHelpers.encodeFixed encoding 13 (Some request.OrderRef)
        native.UserId <- EncodingHelpers.encodeFixed encoding 16 (Some request.UserId)
        native.OrderPriceType <- byte request.OrderPriceType
        native.Direction <- byte request.Direction
        native.CombOffsetFlag <- EncodingHelpers.encodeFixed encoding 5 (Some request.CombOffsetFlag)
        native.CombHedgeFlag <- EncodingHelpers.encodeFixed encoding 5 (Some request.CombHedgeFlag)
        native.LimitPrice <- float request.LimitPrice
        native.VolumeTotalOriginal <- request.VolumeTotalOriginal
        native.TimeCondition <- byte request.TimeCondition
        native.GtdDate <- EncodingHelpers.encodeFixed encoding 9 request.GtdDate
        native.VolumeCondition <- byte request.VolumeCondition
        native.MinVolume <- request.MinVolume
        native.ContingentCondition <- byte request.ContingentCondition
        native.StopPrice <- float request.StopPrice
        native.ForceCloseReason <- byte request.ForceCloseReason
        native.IsAutoSuspend <- if request.IsAutoSuspend then 1 else 0
        native.BusinessUnit <- EncodingHelpers.encodeFixed encoding 21 request.BusinessUnit
        native.RequestId <- requestId
        native.UserForceClose <- if request.UserForceClose then 1 else 0
        native.IsSwapOrder <- if request.IsSwapOrder then 1 else 0
        native.ExchangeId <- EncodingHelpers.encodeFixed encoding 9 request.ExchangeId
        native.InvestUnitId <- EncodingHelpers.encodeFixed encoding 17 request.InvestUnitId
        native.AccountId <- EncodingHelpers.encodeFixed encoding 13 request.AccountId
        native.CurrencyId <- EncodingHelpers.encodeFixed encoding 4 request.CurrencyId
        native.ClientId <- EncodingHelpers.encodeFixed encoding 11 request.ClientId
        native.MacAddress <- EncodingHelpers.encodeFixed encoding 21 request.MacAddress
        native.IpAddress <- EncodingHelpers.encodeFixed encoding 33 request.IpAddress
        native.OrderMemo <- EncodingHelpers.encodeFixed encoding 13 request.OrderMemo
        native

    let inputOrderAction encoding requestId (request: InputOrderActionRequest) =
        let mutable native = NativeInputOrderAction()
        native.BrokerId <- EncodingHelpers.encodeFixed encoding 11 (Some request.BrokerId)
        native.InvestorId <- EncodingHelpers.encodeFixed encoding 13 (Some request.InvestorId)
        native.OrderActionRef <- request.OrderActionRef
        native.OrderRef <- EncodingHelpers.encodeFixed encoding 13 (Some request.OrderRef)
        native.RequestId <- requestId
        native.FrontId <- request.FrontId
        native.SessionId <- request.SessionId
        native.ExchangeId <- EncodingHelpers.encodeFixed encoding 9 request.ExchangeId
        native.OrderSysId <- EncodingHelpers.encodeFixed encoding 21 request.OrderSysId
        native.ActionFlag <- byte request.ActionFlag
        native.LimitPrice <- float request.LimitPrice
        native.VolumeChange <- request.VolumeChange
        native.UserId <- EncodingHelpers.encodeFixed encoding 16 request.UserId
        native.InvestUnitId <- EncodingHelpers.encodeFixed encoding 17 request.InvestUnitId
        native.MacAddress <- EncodingHelpers.encodeFixed encoding 21 request.MacAddress
        native.InstrumentId <- EncodingHelpers.encodeFixed encoding 81 request.InstrumentId
        native.IpAddress <- EncodingHelpers.encodeFixed encoding 33 request.IpAddress
        native.OrderMemo <- EncodingHelpers.encodeFixed encoding 13 request.OrderMemo
        native

    let fensUserInfo encoding (request: FensUserInfo) : NativeFensUserInfo =
        TraderBridgeGenerated.buildNative<FensUserInfo, NativeFensUserInfo> encoding request

    let inputBatchOrderAction encoding requestId (request: InputBatchOrderActionRequest) : NativeInputBatchOrderAction =
        TraderBridgeGenerated.buildNativeWithRequestId<InputBatchOrderActionRequest, NativeInputBatchOrderAction> encoding requestId request

    let inputCombAction encoding (request: InputCombActionRequest) : NativeInputCombAction =
        TraderBridgeGenerated.buildNative<InputCombActionRequest, NativeInputCombAction> encoding request

    let inputExecOrder encoding requestId (request: InputExecOrderRequest) : NativeInputExecOrder =
        TraderBridgeGenerated.buildNativeWithRequestId<InputExecOrderRequest, NativeInputExecOrder> encoding requestId request

    let inputExecOrderAction encoding requestId (request: InputExecOrderActionRequest) : NativeInputExecOrderAction =
        TraderBridgeGenerated.buildNativeWithRequestId<InputExecOrderActionRequest, NativeInputExecOrderAction> encoding requestId request

    let inputForQuote encoding (request: InputForQuoteRequest) : NativeInputForQuote =
        TraderBridgeGenerated.buildNative<InputForQuoteRequest, NativeInputForQuote> encoding request

    let inputHedgeCfm encoding requestId (request: InputHedgeCfmRequest) : NativeInputHedgeCfm =
        TraderBridgeGenerated.buildNativeWithRequestId<InputHedgeCfmRequest, NativeInputHedgeCfm> encoding requestId request

    let inputHedgeCfmAction encoding requestId (request: InputHedgeCfmActionRequest) : NativeInputHedgeCfmAction =
        TraderBridgeGenerated.buildNativeWithRequestId<InputHedgeCfmActionRequest, NativeInputHedgeCfmAction> encoding requestId request

    let inputOffsetSetting encoding requestId (request: InputOffsetSettingRequest) : NativeInputOffsetSetting =
        TraderBridgeGenerated.buildNativeWithRequestId<InputOffsetSettingRequest, NativeInputOffsetSetting> encoding requestId request

    let inputOptionSelfClose encoding requestId (request: InputOptionSelfCloseRequest) : NativeInputOptionSelfClose =
        TraderBridgeGenerated.buildNativeWithRequestId<InputOptionSelfCloseRequest, NativeInputOptionSelfClose> encoding requestId request

    let inputOptionSelfCloseAction encoding requestId (request: InputOptionSelfCloseActionRequest) : NativeInputOptionSelfCloseAction =
        TraderBridgeGenerated.buildNativeWithRequestId<InputOptionSelfCloseActionRequest, NativeInputOptionSelfCloseAction> encoding requestId request

    let inputQuote encoding requestId (request: InputQuoteRequest) : NativeInputQuote =
        TraderBridgeGenerated.buildNativeWithRequestId<InputQuoteRequest, NativeInputQuote> encoding requestId request

    let inputQuoteAction encoding requestId (request: InputQuoteActionRequest) : NativeInputQuoteAction =
        TraderBridgeGenerated.buildNativeWithRequestId<InputQuoteActionRequest, NativeInputQuoteAction> encoding requestId request

    let inputSpdApply encoding requestId (request: InputSpdApplyRequest) : NativeInputSpdApply =
        TraderBridgeGenerated.buildNativeWithRequestId<InputSpdApplyRequest, NativeInputSpdApply> encoding requestId request

    let inputSpdApplyAction encoding requestId (request: InputSpdApplyActionRequest) : NativeInputSpdApplyAction =
        TraderBridgeGenerated.buildNativeWithRequestId<InputSpdApplyActionRequest, NativeInputSpdApplyAction> encoding requestId request

    let parkedOrder encoding requestId (request: ParkedOrder) : NativeParkedOrder =
        TraderBridgeGenerated.buildNativeWithRequestId<ParkedOrder, NativeParkedOrder> encoding requestId request

    let parkedOrderAction encoding requestId (request: ParkedOrderAction) : NativeParkedOrderAction =
        TraderBridgeGenerated.buildNativeWithRequestId<ParkedOrderAction, NativeParkedOrderAction> encoding requestId request

    let qryAccountregister encoding (request: QryAccountregisterRequest) : NativeQryAccountregister =
        TraderBridgeGenerated.buildNative<QryAccountregisterRequest, NativeQryAccountregister> encoding request

    let qryBrokerTradingAlgos encoding (request: QryBrokerTradingAlgosRequest) : NativeQryBrokerTradingAlgos =
        TraderBridgeGenerated.buildNative<QryBrokerTradingAlgosRequest, NativeQryBrokerTradingAlgos> encoding request

    let qryBrokerTradingParams encoding (request: QryBrokerTradingParamsRequest) : NativeQryBrokerTradingParams =
        TraderBridgeGenerated.buildNative<QryBrokerTradingParamsRequest, NativeQryBrokerTradingParams> encoding request

    let qryCfmmcTradingAccountKey encoding (request: QryCfmmcTradingAccountKeyRequest) : NativeQryCfmmcTradingAccountKey =
        TraderBridgeGenerated.buildNative<QryCfmmcTradingAccountKeyRequest, NativeQryCfmmcTradingAccountKey> encoding request

    let qryClassifiedInstrument encoding (request: QryClassifiedInstrumentRequest) : NativeQryClassifiedInstrument =
        TraderBridgeGenerated.buildNative<QryClassifiedInstrumentRequest, NativeQryClassifiedInstrument> encoding request

    let qryCombAction encoding (request: QryCombActionRequest) : NativeQryCombAction =
        TraderBridgeGenerated.buildNative<QryCombActionRequest, NativeQryCombAction> encoding request

    let qryCombInstrumentGuard encoding (request: QryCombInstrumentGuardRequest) : NativeQryCombInstrumentGuard =
        TraderBridgeGenerated.buildNative<QryCombInstrumentGuardRequest, NativeQryCombInstrumentGuard> encoding request

    let qryCombLeg encoding (request: QryCombLeg) : NativeQryCombLeg =
        TraderBridgeGenerated.buildNative<QryCombLeg, NativeQryCombLeg> encoding request

    let qryCombPromotionParam encoding (request: QryCombPromotionParamRequest) : NativeQryCombPromotionParam =
        TraderBridgeGenerated.buildNative<QryCombPromotionParamRequest, NativeQryCombPromotionParam> encoding request

    let qryContractBank encoding (request: QryContractBankRequest) : NativeQryContractBank =
        TraderBridgeGenerated.buildNative<QryContractBankRequest, NativeQryContractBank> encoding request

    let qryDepthMarketData encoding (request: QryDepthMarketDataRequest) : NativeQryDepthMarketData =
        TraderBridgeGenerated.buildNative<QryDepthMarketDataRequest, NativeQryDepthMarketData> encoding request

    let qryEWarrantOffset encoding (request: QryEWarrantOffsetRequest) : NativeQryEWarrantOffset =
        TraderBridgeGenerated.buildNative<QryEWarrantOffsetRequest, NativeQryEWarrantOffset> encoding request

    let qryExchange encoding (request: QryExchangeRequest) : NativeQryExchange =
        TraderBridgeGenerated.buildNative<QryExchangeRequest, NativeQryExchange> encoding request

    let qryExchangeMarginRateAdjust encoding (request: QryExchangeMarginRateAdjustRequest) : NativeQryExchangeMarginRateAdjust =
        TraderBridgeGenerated.buildNative<QryExchangeMarginRateAdjustRequest, NativeQryExchangeMarginRateAdjust> encoding request

    let qryExchangeRate encoding (request: QryExchangeRate) : NativeQryExchangeRate =
        TraderBridgeGenerated.buildNative<QryExchangeRate, NativeQryExchangeRate> encoding request

    let qryExecOrder encoding (request: QryExecOrderRequest) : NativeQryExecOrder =
        TraderBridgeGenerated.buildNative<QryExecOrderRequest, NativeQryExecOrder> encoding request

    let qryForQuote encoding (request: QryForQuoteRequest) : NativeQryForQuote =
        TraderBridgeGenerated.buildNative<QryForQuoteRequest, NativeQryForQuote> encoding request

    let qryHedgeCfm encoding (request: QryHedgeCfmRequest) : NativeQryHedgeCfm =
        TraderBridgeGenerated.buildNative<QryHedgeCfmRequest, NativeQryHedgeCfm> encoding request

    let qryInstrument encoding (request: QryInstrumentRequest) : NativeQryInstrument =
        TraderBridgeGenerated.buildNative<QryInstrumentRequest, NativeQryInstrument> encoding request

    let qryInstrumentOrderCommRate encoding (request: QryInstrumentOrderCommRateRequest) : NativeQryInstrumentOrderCommRate =
        TraderBridgeGenerated.buildNative<QryInstrumentOrderCommRateRequest, NativeQryInstrumentOrderCommRate> encoding request

    let qryInvestUnit encoding (request: QryInvestUnitRequest) : NativeQryInvestUnit =
        TraderBridgeGenerated.buildNative<QryInvestUnitRequest, NativeQryInvestUnit> encoding request

    let qryInvestor encoding (request: QryInvestorRequest) : NativeQryInvestor =
        TraderBridgeGenerated.buildNative<QryInvestorRequest, NativeQryInvestor> encoding request

    let qryInvestorCommodityGroupSpmmMargin encoding (request: QryInvestorCommodityGroupSpmmMarginRequest) : NativeQryInvestorCommodityGroupSpmmMargin =
        TraderBridgeGenerated.buildNative<QryInvestorCommodityGroupSpmmMarginRequest, NativeQryInvestorCommodityGroupSpmmMargin> encoding request

    let qryInvestorCommoditySpmmMargin encoding (request: QryInvestorCommoditySpmmMarginRequest) : NativeQryInvestorCommoditySpmmMargin =
        TraderBridgeGenerated.buildNative<QryInvestorCommoditySpmmMarginRequest, NativeQryInvestorCommoditySpmmMargin> encoding request

    let qryInvestorInfoCommRec encoding (request: QryInvestorInfoCommRecRequest) : NativeQryInvestorInfoCommRec =
        TraderBridgeGenerated.buildNative<QryInvestorInfoCommRecRequest, NativeQryInvestorInfoCommRec> encoding request

    let qryInvestorPortfMarginRatio encoding (request: QryInvestorPortfMarginRatioRequest) : NativeQryInvestorPortfMarginRatio =
        TraderBridgeGenerated.buildNative<QryInvestorPortfMarginRatioRequest, NativeQryInvestorPortfMarginRatio> encoding request

    let qryInvestorPortfSetting encoding (request: QryInvestorPortfSettingRequest) : NativeQryInvestorPortfSetting =
        TraderBridgeGenerated.buildNative<QryInvestorPortfSettingRequest, NativeQryInvestorPortfSetting> encoding request

    let qryInvestorPositionCombineDetail encoding (request: QryInvestorPositionCombineDetailRequest) : NativeQryInvestorPositionCombineDetail =
        TraderBridgeGenerated.buildNative<QryInvestorPositionCombineDetailRequest, NativeQryInvestorPositionCombineDetail> encoding request

    let qryInvestorPositionDetail encoding (request: QryInvestorPositionDetailRequest) : NativeQryInvestorPositionDetail =
        TraderBridgeGenerated.buildNative<QryInvestorPositionDetailRequest, NativeQryInvestorPositionDetail> encoding request

    let qryInvestorProdRcamsMargin encoding (request: QryInvestorProdRcamsMarginRequest) : NativeQryInvestorProdRcamsMargin =
        TraderBridgeGenerated.buildNative<QryInvestorProdRcamsMarginRequest, NativeQryInvestorProdRcamsMargin> encoding request

    let qryInvestorProdRuleMargin encoding (request: QryInvestorProdRuleMarginRequest) : NativeQryInvestorProdRuleMargin =
        TraderBridgeGenerated.buildNative<QryInvestorProdRuleMarginRequest, NativeQryInvestorProdRuleMargin> encoding request

    let qryInvestorProdSpbmDetail encoding (request: QryInvestorProdSpbmDetailRequest) : NativeQryInvestorProdSpbmDetail =
        TraderBridgeGenerated.buildNative<QryInvestorProdSpbmDetailRequest, NativeQryInvestorProdSpbmDetail> encoding request

    let qryInvestorProductGroupMargin encoding (request: QryInvestorProductGroupMarginRequest) : NativeQryInvestorProductGroupMargin =
        TraderBridgeGenerated.buildNative<QryInvestorProductGroupMarginRequest, NativeQryInvestorProductGroupMargin> encoding request

    let qryMaxOrderVolume encoding (request: QryMaxOrderVolumeRequest) : NativeQryMaxOrderVolume =
        TraderBridgeGenerated.buildNative<QryMaxOrderVolumeRequest, NativeQryMaxOrderVolume> encoding request

    let qryMmInstrumentCommissionRate encoding (request: QryMmInstrumentCommissionRateRequest) : NativeQryMmInstrumentCommissionRate =
        TraderBridgeGenerated.buildNative<QryMmInstrumentCommissionRateRequest, NativeQryMmInstrumentCommissionRate> encoding request

    let qryMmOptionInstrCommRate encoding (request: QryMmOptionInstrCommRateRequest) : NativeQryMmOptionInstrCommRate =
        TraderBridgeGenerated.buildNative<QryMmOptionInstrCommRateRequest, NativeQryMmOptionInstrCommRate> encoding request

    let qryNotice encoding (request: QryNoticeRequest) : NativeQryNotice =
        TraderBridgeGenerated.buildNative<QryNoticeRequest, NativeQryNotice> encoding request

    let qryOffsetSetting encoding (request: QryOffsetSettingRequest) : NativeQryOffsetSetting =
        TraderBridgeGenerated.buildNative<QryOffsetSettingRequest, NativeQryOffsetSetting> encoding request

    let qryOptionInstrCommRate encoding (request: QryOptionInstrCommRateRequest) : NativeQryOptionInstrCommRate =
        TraderBridgeGenerated.buildNative<QryOptionInstrCommRateRequest, NativeQryOptionInstrCommRate> encoding request

    let qryOptionInstrTradeCost encoding (request: QryOptionInstrTradeCostRequest) : NativeQryOptionInstrTradeCost =
        TraderBridgeGenerated.buildNative<QryOptionInstrTradeCostRequest, NativeQryOptionInstrTradeCost> encoding request

    let qryOptionSelfClose encoding (request: QryOptionSelfCloseRequest) : NativeQryOptionSelfClose =
        TraderBridgeGenerated.buildNative<QryOptionSelfCloseRequest, NativeQryOptionSelfClose> encoding request

    let qryOrder encoding (request: QryOrderRequest) : NativeQryOrder =
        TraderBridgeGenerated.buildNative<QryOrderRequest, NativeQryOrder> encoding request

    let qryParkedOrder encoding (request: QryParkedOrderRequest) : NativeQryParkedOrder =
        TraderBridgeGenerated.buildNative<QryParkedOrderRequest, NativeQryParkedOrder> encoding request

    let qryParkedOrderAction encoding (request: QryParkedOrderActionRequest) : NativeQryParkedOrderAction =
        TraderBridgeGenerated.buildNative<QryParkedOrderActionRequest, NativeQryParkedOrderAction> encoding request

    let qryProduct encoding (request: QryProductRequest) : NativeQryProduct =
        TraderBridgeGenerated.buildNative<QryProductRequest, NativeQryProduct> encoding request

    let qryProductExchRate encoding (request: QryProductExchRateRequest) : NativeQryProductExchRate =
        TraderBridgeGenerated.buildNative<QryProductExchRateRequest, NativeQryProductExchRate> encoding request

    let qryProductGroup encoding (request: QryProductGroupRequest) : NativeQryProductGroup =
        TraderBridgeGenerated.buildNative<QryProductGroupRequest, NativeQryProductGroup> encoding request

    let qryQuote encoding (request: QryQuoteRequest) : NativeQryQuote =
        TraderBridgeGenerated.buildNative<QryQuoteRequest, NativeQryQuote> encoding request

    let qryRcamsCombProductInfo encoding (request: QryRcamsCombProductInfoRequest) : NativeQryRcamsCombProductInfo =
        TraderBridgeGenerated.buildNative<QryRcamsCombProductInfoRequest, NativeQryRcamsCombProductInfo> encoding request

    let qryRcamsInstrParameter encoding (request: QryRcamsInstrParameterRequest) : NativeQryRcamsInstrParameter =
        TraderBridgeGenerated.buildNative<QryRcamsInstrParameterRequest, NativeQryRcamsInstrParameter> encoding request

    let qryRcamsInterParameter encoding (request: QryRcamsInterParameterRequest) : NativeQryRcamsInterParameter =
        TraderBridgeGenerated.buildNative<QryRcamsInterParameterRequest, NativeQryRcamsInterParameter> encoding request

    let qryRcamsIntraParameter encoding (request: QryRcamsIntraParameterRequest) : NativeQryRcamsIntraParameter =
        TraderBridgeGenerated.buildNative<QryRcamsIntraParameterRequest, NativeQryRcamsIntraParameter> encoding request

    let qryRcamsInvestorCombPosition encoding (request: QryRcamsInvestorCombPositionRequest) : NativeQryRcamsInvestorCombPosition =
        TraderBridgeGenerated.buildNative<QryRcamsInvestorCombPositionRequest, NativeQryRcamsInvestorCombPosition> encoding request

    let qryRcamsShortOptAdjustParam encoding (request: QryRcamsShortOptAdjustParamRequest) : NativeQryRcamsShortOptAdjustParam =
        TraderBridgeGenerated.buildNative<QryRcamsShortOptAdjustParamRequest, NativeQryRcamsShortOptAdjustParam> encoding request

    let qryRiskSettleInvstPosition encoding (request: QryRiskSettleInvstPositionRequest) : NativeQryRiskSettleInvstPosition =
        TraderBridgeGenerated.buildNative<QryRiskSettleInvstPositionRequest, NativeQryRiskSettleInvstPosition> encoding request

    let qryRiskSettleProductStatus encoding (request: QryRiskSettleProductStatusRequest) : NativeQryRiskSettleProductStatus =
        TraderBridgeGenerated.buildNative<QryRiskSettleProductStatusRequest, NativeQryRiskSettleProductStatus> encoding request

    let qryRuleInstrParameter encoding (request: QryRuleInstrParameterRequest) : NativeQryRuleInstrParameter =
        TraderBridgeGenerated.buildNative<QryRuleInstrParameterRequest, NativeQryRuleInstrParameter> encoding request

    let qryRuleInterParameter encoding (request: QryRuleInterParameterRequest) : NativeQryRuleInterParameter =
        TraderBridgeGenerated.buildNative<QryRuleInterParameterRequest, NativeQryRuleInterParameter> encoding request

    let qryRuleIntraParameter encoding (request: QryRuleIntraParameterRequest) : NativeQryRuleIntraParameter =
        TraderBridgeGenerated.buildNative<QryRuleIntraParameterRequest, NativeQryRuleIntraParameter> encoding request

    let qrySecAgentAcIdMap encoding (request: QrySecAgentAcIdMapRequest) : NativeQrySecAgentAcIdMap =
        TraderBridgeGenerated.buildNative<QrySecAgentAcIdMapRequest, NativeQrySecAgentAcIdMap> encoding request

    let qrySecAgentCheckMode encoding (request: QrySecAgentCheckModeRequest) : NativeQrySecAgentCheckMode =
        TraderBridgeGenerated.buildNative<QrySecAgentCheckModeRequest, NativeQrySecAgentCheckMode> encoding request

    let qrySecAgentTradeInfo encoding (request: QrySecAgentTradeInfoRequest) : NativeQrySecAgentTradeInfo =
        TraderBridgeGenerated.buildNative<QrySecAgentTradeInfoRequest, NativeQrySecAgentTradeInfo> encoding request

    let qrySettlementInfo encoding (request: QrySettlementInfoRequest) : NativeQrySettlementInfo =
        TraderBridgeGenerated.buildNative<QrySettlementInfoRequest, NativeQrySettlementInfo> encoding request

    let qrySettlementInfoConfirm encoding (request: QrySettlementInfoConfirmRequest) : NativeQrySettlementInfoConfirm =
        TraderBridgeGenerated.buildNative<QrySettlementInfoConfirmRequest, NativeQrySettlementInfoConfirm> encoding request

    let qrySpbmAddOnInterParameter encoding (request: QrySpbmAddOnInterParameterRequest) : NativeQrySpbmAddOnInterParameter =
        TraderBridgeGenerated.buildNative<QrySpbmAddOnInterParameterRequest, NativeQrySpbmAddOnInterParameter> encoding request

    let qrySpbmFutureParameter encoding (request: QrySpbmFutureParameterRequest) : NativeQrySpbmFutureParameter =
        TraderBridgeGenerated.buildNative<QrySpbmFutureParameterRequest, NativeQrySpbmFutureParameter> encoding request

    let qrySpbmInterParameter encoding (request: QrySpbmInterParameterRequest) : NativeQrySpbmInterParameter =
        TraderBridgeGenerated.buildNative<QrySpbmInterParameterRequest, NativeQrySpbmInterParameter> encoding request

    let qrySpbmIntraParameter encoding (request: QrySpbmIntraParameterRequest) : NativeQrySpbmIntraParameter =
        TraderBridgeGenerated.buildNative<QrySpbmIntraParameterRequest, NativeQrySpbmIntraParameter> encoding request

    let qrySpbmInvestorPortfDef encoding (request: QrySpbmInvestorPortfDefRequest) : NativeQrySpbmInvestorPortfDef =
        TraderBridgeGenerated.buildNative<QrySpbmInvestorPortfDefRequest, NativeQrySpbmInvestorPortfDef> encoding request

    let qrySpbmOptionParameter encoding (request: QrySpbmOptionParameterRequest) : NativeQrySpbmOptionParameter =
        TraderBridgeGenerated.buildNative<QrySpbmOptionParameterRequest, NativeQrySpbmOptionParameter> encoding request

    let qrySpbmPortfDefinition encoding (request: QrySpbmPortfDefinitionRequest) : NativeQrySpbmPortfDefinition =
        TraderBridgeGenerated.buildNative<QrySpbmPortfDefinitionRequest, NativeQrySpbmPortfDefinition> encoding request

    let qrySpdApply encoding (request: QrySpdApplyRequest) : NativeQrySpdApply =
        TraderBridgeGenerated.buildNative<QrySpdApplyRequest, NativeQrySpdApply> encoding request

    let qrySpmmInstParam encoding (request: QrySpmmInstParamRequest) : NativeQrySpmmInstParam =
        TraderBridgeGenerated.buildNative<QrySpmmInstParamRequest, NativeQrySpmmInstParam> encoding request

    let qrySpmmProductParam encoding (request: QrySpmmProductParamRequest) : NativeQrySpmmProductParam =
        TraderBridgeGenerated.buildNative<QrySpmmProductParamRequest, NativeQrySpmmProductParam> encoding request

    let qryTrade encoding (request: QryTradeRequest) : NativeQryTrade =
        TraderBridgeGenerated.buildNative<QryTradeRequest, NativeQryTrade> encoding request

    let qryTraderOffer encoding (request: QryTraderOfferRequest) : NativeQryTraderOffer =
        TraderBridgeGenerated.buildNative<QryTraderOfferRequest, NativeQryTraderOffer> encoding request

    let qryTradingCode encoding (request: QryTradingCodeRequest) : NativeQryTradingCode =
        TraderBridgeGenerated.buildNative<QryTradingCodeRequest, NativeQryTradingCode> encoding request

    let qryTradingNotice encoding (request: QryTradingNoticeRequest) : NativeQryTradingNotice =
        TraderBridgeGenerated.buildNative<QryTradingNoticeRequest, NativeQryTradingNotice> encoding request

    let qryTransferBank encoding (request: QryTransferBankRequest) : NativeQryTransferBank =
        TraderBridgeGenerated.buildNative<QryTransferBankRequest, NativeQryTransferBank> encoding request

    let qryTransferSerial encoding (request: QryTransferSerialRequest) : NativeQryTransferSerial =
        TraderBridgeGenerated.buildNative<QryTransferSerialRequest, NativeQryTransferSerial> encoding request

    let qryUserSession encoding (request: QryUserSessionRequest) : NativeQryUserSession =
        TraderBridgeGenerated.buildNative<QryUserSessionRequest, NativeQryUserSession> encoding request

    let queryCfmmcTradingAccountToken encoding (request: QueryCfmmcTradingAccountTokenRequest) : NativeQueryCfmmcTradingAccountToken =
        TraderBridgeGenerated.buildNative<QueryCfmmcTradingAccountTokenRequest, NativeQueryCfmmcTradingAccountToken> encoding request

    let removeParkedOrder encoding (request: RemoveParkedOrder) : NativeRemoveParkedOrder =
        TraderBridgeGenerated.buildNative<RemoveParkedOrder, NativeRemoveParkedOrder> encoding request

    let removeParkedOrderAction encoding (request: RemoveParkedOrderAction) : NativeRemoveParkedOrderAction =
        TraderBridgeGenerated.buildNative<RemoveParkedOrderAction, NativeRemoveParkedOrderAction> encoding request

    let reqGenSmsCode encoding (request: GenSmsCodeRequest) : NativeReqGenSmsCode =
        TraderBridgeGenerated.buildNative<GenSmsCodeRequest, NativeReqGenSmsCode> encoding request

    let reqGenUserCaptcha encoding (request: GenUserCaptchaRequest) : NativeReqGenUserCaptcha =
        TraderBridgeGenerated.buildNative<GenUserCaptchaRequest, NativeReqGenUserCaptcha> encoding request

    let reqGenUserText encoding (request: GenUserTextRequest) : NativeReqGenUserText =
        TraderBridgeGenerated.buildNative<GenUserTextRequest, NativeReqGenUserText> encoding request

    let reqQueryAccount encoding requestId (request: ReqQueryAccount) : NativeReqQueryAccount =
        TraderBridgeGenerated.buildNativeWithRequestId<ReqQueryAccount, NativeReqQueryAccount> encoding requestId request

    let reqTransfer encoding requestId (request: TransferRequest) : NativeReqTransfer =
        TraderBridgeGenerated.buildNativeWithRequestId<TransferRequest, NativeReqTransfer> encoding requestId request

    let reqUserAuthMethod encoding (request: UserAuthMethodRequest) : NativeReqUserAuthMethod =
        TraderBridgeGenerated.buildNative<UserAuthMethodRequest, NativeReqUserAuthMethod> encoding request

    let reqUserLoginWithCaptcha encoding (request: UserLoginWithCaptchaRequest) : NativeReqUserLoginWithCaptcha =
        TraderBridgeGenerated.buildNative<UserLoginWithCaptchaRequest, NativeReqUserLoginWithCaptcha> encoding request

    let reqUserLoginWithOtp encoding (request: UserLoginWithOtpRequest) : NativeReqUserLoginWithOtp =
        TraderBridgeGenerated.buildNative<UserLoginWithOtpRequest, NativeReqUserLoginWithOtp> encoding request

    let reqUserLoginWithText encoding (request: UserLoginWithTextRequest) : NativeReqUserLoginWithText =
        TraderBridgeGenerated.buildNative<UserLoginWithTextRequest, NativeReqUserLoginWithText> encoding request

    let tradingAccountPasswordUpdate encoding (request: TradingAccountPasswordUpdate) : NativeTradingAccountPasswordUpdate =
        TraderBridgeGenerated.buildNative<TradingAccountPasswordUpdate, NativeTradingAccountPasswordUpdate> encoding request

    let userLogoutResponse encoding (request: UserLogoutResponse) : NativeUserLogout =
        TraderBridgeGenerated.buildNative<UserLogoutResponse, NativeUserLogout> encoding request

    let userPasswordUpdate encoding (request: UserPasswordUpdate) : NativeUserPasswordUpdate =
        TraderBridgeGenerated.buildNative<UserPasswordUpdate, NativeUserPasswordUpdate> encoding request

    let userSystemInfo encoding (request: UserSystemInfo) : NativeUserSystemInfo =
        TraderBridgeGenerated.buildNative<UserSystemInfo, NativeUserSystemInfo> encoding request

    let wechatUserSystemInfo encoding (request: WechatUserSystemInfo) : NativeWechatUserSystemInfo =
        TraderBridgeGenerated.buildNative<WechatUserSystemInfo, NativeWechatUserSystemInfo> encoding request

module private TraderBridgeHelpers =
    let toTraderHandle (ptr: nativeint) =
        if ptr = 0n then
            invalidOp "Failed to create trader bridge handle."

        TraderApiSafeHandle.FromNative ptr

type private TraderSpiRegistration(callbacks: TraderCallbacks, encodings: EncodingPair) =
    let onFrontConnected =
        TraderFrontConnectedDelegate(fun _ -> callbacks.FrontConnected |> Option.iter (fun handler -> handler ()))

    let onFrontDisconnected =
        TraderFrontDisconnectedDelegate(fun reason _ ->
            callbacks.FrontDisconnected |> Option.iter (fun handler -> handler reason))

    let onHeartBeatWarning =
        TraderHeartBeatWarningDelegate(fun lapse _ ->
            callbacks.HeartBeatWarning |> Option.iter (fun handler -> handler lapse))

    let onRtnPrivateSeqNo =
        TraderRtnPrivateSeqNoDelegate(fun seqNo _ ->
            callbacks.RtnPrivateSeqNo |> Option.iter (fun handler -> handler seqNo))

    let rspAuthenticate =
        TraderRspAuthenticateDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspAuthenticate
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeRspAuthenticate>
                    |> Option.map (TraderBridgeMapping.authenticate encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspSettlementInfoConfirm =
        TraderRspSettlementInfoConfirmDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspSettlementInfoConfirm
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeSettlementInfoConfirm>
                    |> Option.map (TraderBridgeMapping.settlementInfoConfirm encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspUserLogin =
        TraderRspUserLoginDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspUserLogin
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeRspUserLogin>
                    |> Option.map (BridgeMapping.userLogin encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspUserLogout =
        TraderRspUserLogoutDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspUserLogout
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeUserLogout>
                    |> Option.map (BridgeMapping.userLogout encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspError =
        TraderRspErrorDelegate(fun rspInfoPtr requestId isLast _ ->
            callbacks.RspError
            |> Option.iter (fun handler ->
                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler rspInfo requestId (isLast <> 0)))

    let rspQryTradingAccount =
        TraderRspQryTradingAccountDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryTradingAccount
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeTradingAccount>
                    |> Option.map (TraderBridgeMapping.tradingAccount encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryInvestorPosition =
        TraderRspQryInvestorPositionDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryInvestorPosition
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInvestorPosition>
                    |> Option.map (TraderBridgeMapping.investorPosition encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryInstrumentMarginRate =
        TraderRspQryInstrumentMarginRateDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryInstrumentMarginRate
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInstrumentMarginRate>
                    |> Option.map (TraderBridgeMapping.instrumentMarginRate encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryExchangeMarginRate =
        TraderRspQryExchangeMarginRateDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryExchangeMarginRate
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeExchangeMarginRate>
                    |> Option.map (TraderBridgeMapping.exchangeMarginRate encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryInstrumentCommissionRate =
        TraderRspQryInstrumentCommissionRateDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryInstrumentCommissionRate
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInstrumentCommissionRate>
                    |> Option.map (TraderBridgeMapping.instrumentCommissionRate encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspOrderInsert =
        TraderRspOrderInsertDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspOrderInsert
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputOrder>
                    |> Option.map (TraderBridgeMapping.inputOrderRequest encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspOrderAction =
        TraderRspOrderActionDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspOrderAction
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputOrderAction>
                    |> Option.map (TraderBridgeMapping.inputOrderActionRequest encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rtnOrder =
        TraderRtnOrderDelegate(fun itemPtr _ ->
            callbacks.RtnOrder
            |> Option.iter (fun handler ->
                match EncodingHelpers.ptrToOption<NativeOrder> itemPtr with
                | Some item -> handler (TraderBridgeMapping.orderUpdate encodings.InboundEncoding item)
                | None -> ()))

    let rtnTrade =
        TraderRtnTradeDelegate(fun itemPtr _ ->
            callbacks.RtnTrade
            |> Option.iter (fun handler ->
                match EncodingHelpers.ptrToOption<NativeTrade> itemPtr with
                | Some item -> handler (TraderBridgeMapping.tradeUpdate encodings.InboundEncoding item)
                | None -> ()))

    let errRtnBankToFutureByFuture =
        TraderErrRtnBankToFutureByFutureDelegate(fun itemPtr rspInfoPtr _ ->
            callbacks.ErrRtnBankToFutureByFuture
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeReqTransfer>
                    |> Option.map (TraderBridgeMapping.reqTransfer encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo))

    let errRtnBatchOrderAction =
        TraderErrRtnBatchOrderActionDelegate(fun itemPtr rspInfoPtr _ ->
            callbacks.ErrRtnBatchOrderAction
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeBatchOrderAction>
                    |> Option.map (TraderBridgeMapping.batchOrderAction encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo))

    let errRtnCancelOffsetSetting =
        TraderErrRtnCancelOffsetSettingDelegate(fun itemPtr rspInfoPtr _ ->
            callbacks.ErrRtnCancelOffsetSetting
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeCancelOffsetSetting>
                    |> Option.map (TraderBridgeMapping.cancelOffsetSetting encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo))

    let errRtnCombActionInsert =
        TraderErrRtnCombActionInsertDelegate(fun itemPtr rspInfoPtr _ ->
            callbacks.ErrRtnCombActionInsert
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputCombAction>
                    |> Option.map (TraderBridgeMapping.inputCombAction encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo))

    let errRtnExecOrderAction =
        TraderErrRtnExecOrderActionDelegate(fun itemPtr rspInfoPtr _ ->
            callbacks.ErrRtnExecOrderAction
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeExecOrderAction>
                    |> Option.map (TraderBridgeMapping.execOrderAction encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo))

    let errRtnExecOrderInsert =
        TraderErrRtnExecOrderInsertDelegate(fun itemPtr rspInfoPtr _ ->
            callbacks.ErrRtnExecOrderInsert
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputExecOrder>
                    |> Option.map (TraderBridgeMapping.inputExecOrder encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo))

    let errRtnForQuoteInsert =
        TraderErrRtnForQuoteInsertDelegate(fun itemPtr rspInfoPtr _ ->
            callbacks.ErrRtnForQuoteInsert
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputForQuote>
                    |> Option.map (TraderBridgeMapping.inputForQuote encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo))

    let errRtnFutureToBankByFuture =
        TraderErrRtnFutureToBankByFutureDelegate(fun itemPtr rspInfoPtr _ ->
            callbacks.ErrRtnFutureToBankByFuture
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeReqTransfer>
                    |> Option.map (TraderBridgeMapping.reqTransfer encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo))

    let errRtnHedgeCfm =
        TraderErrRtnHedgeCfmDelegate(fun itemPtr rspInfoPtr _ ->
            callbacks.ErrRtnHedgeCfm
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputHedgeCfm>
                    |> Option.map (TraderBridgeMapping.inputHedgeCfm encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo))

    let errRtnHedgeCfmAction =
        TraderErrRtnHedgeCfmActionDelegate(fun itemPtr rspInfoPtr _ ->
            callbacks.ErrRtnHedgeCfmAction
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeHedgeCfmAction>
                    |> Option.map (TraderBridgeMapping.hedgeCfmAction encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo))

    let errRtnOffsetSetting =
        TraderErrRtnOffsetSettingDelegate(fun itemPtr rspInfoPtr _ ->
            callbacks.ErrRtnOffsetSetting
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputOffsetSetting>
                    |> Option.map (TraderBridgeMapping.inputOffsetSetting encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo))

    let errRtnOptionSelfCloseAction =
        TraderErrRtnOptionSelfCloseActionDelegate(fun itemPtr rspInfoPtr _ ->
            callbacks.ErrRtnOptionSelfCloseAction
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeOptionSelfCloseAction>
                    |> Option.map (TraderBridgeMapping.optionSelfCloseAction encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo))

    let errRtnOptionSelfCloseInsert =
        TraderErrRtnOptionSelfCloseInsertDelegate(fun itemPtr rspInfoPtr _ ->
            callbacks.ErrRtnOptionSelfCloseInsert
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputOptionSelfClose>
                    |> Option.map (TraderBridgeMapping.inputOptionSelfClose encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo))

    let errRtnOrderAction =
        TraderErrRtnOrderActionDelegate(fun itemPtr rspInfoPtr _ ->
            callbacks.ErrRtnOrderAction
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeOrderAction>
                    |> Option.map (TraderBridgeMapping.orderAction encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo))

    let errRtnOrderInsert =
        TraderErrRtnOrderInsertDelegate(fun itemPtr rspInfoPtr _ ->
            callbacks.ErrRtnOrderInsert
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputOrder>
                    |> Option.map (TraderBridgeMapping.inputOrderRequest encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo))

    let errRtnQueryBankBalanceByFuture =
        TraderErrRtnQueryBankBalanceByFutureDelegate(fun itemPtr rspInfoPtr _ ->
            callbacks.ErrRtnQueryBankBalanceByFuture
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeReqQueryAccount>
                    |> Option.map (TraderBridgeMapping.reqQueryAccount encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo))

    let errRtnQuoteAction =
        TraderErrRtnQuoteActionDelegate(fun itemPtr rspInfoPtr _ ->
            callbacks.ErrRtnQuoteAction
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeQuoteAction>
                    |> Option.map (TraderBridgeMapping.quoteAction encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo))

    let errRtnQuoteInsert =
        TraderErrRtnQuoteInsertDelegate(fun itemPtr rspInfoPtr _ ->
            callbacks.ErrRtnQuoteInsert
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputQuote>
                    |> Option.map (TraderBridgeMapping.inputQuote encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo))

    let errRtnSpdApply =
        TraderErrRtnSpdApplyDelegate(fun itemPtr rspInfoPtr _ ->
            callbacks.ErrRtnSpdApply
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputSpdApply>
                    |> Option.map (TraderBridgeMapping.inputSpdApply encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo))

    let errRtnSpdApplyAction =
        TraderErrRtnSpdApplyActionDelegate(fun itemPtr rspInfoPtr _ ->
            callbacks.ErrRtnSpdApplyAction
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeSpdApplyAction>
                    |> Option.map (TraderBridgeMapping.spdApplyAction encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo))

    let rspBatchOrderAction =
        TraderRspBatchOrderActionDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspBatchOrderAction
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputBatchOrderAction>
                    |> Option.map (TraderBridgeMapping.inputBatchOrderAction encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspCancelOffsetSetting =
        TraderRspCancelOffsetSettingDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspCancelOffsetSetting
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputOffsetSetting>
                    |> Option.map (TraderBridgeMapping.inputOffsetSetting encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspCombActionInsert =
        TraderRspCombActionInsertDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspCombActionInsert
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputCombAction>
                    |> Option.map (TraderBridgeMapping.inputCombAction encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspExecOrderAction =
        TraderRspExecOrderActionDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspExecOrderAction
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputExecOrderAction>
                    |> Option.map (TraderBridgeMapping.inputExecOrderAction encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspExecOrderInsert =
        TraderRspExecOrderInsertDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspExecOrderInsert
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputExecOrder>
                    |> Option.map (TraderBridgeMapping.inputExecOrder encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspForQuoteInsert =
        TraderRspForQuoteInsertDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspForQuoteInsert
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputForQuote>
                    |> Option.map (TraderBridgeMapping.inputForQuote encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspFromBankToFutureByFuture =
        TraderRspFromBankToFutureByFutureDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspFromBankToFutureByFuture
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeReqTransfer>
                    |> Option.map (TraderBridgeMapping.reqTransfer encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspFromFutureToBankByFuture =
        TraderRspFromFutureToBankByFutureDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspFromFutureToBankByFuture
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeReqTransfer>
                    |> Option.map (TraderBridgeMapping.reqTransfer encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspGenSmsCode =
        TraderRspGenSmsCodeDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspGenSmsCode
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeRspGenSmsCode>
                    |> Option.map (TraderBridgeMapping.rspGenSmsCode encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspGenUserCaptcha =
        TraderRspGenUserCaptchaDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspGenUserCaptcha
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeRspGenUserCaptcha>
                    |> Option.map (TraderBridgeMapping.rspGenUserCaptcha encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspGenUserText =
        TraderRspGenUserTextDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspGenUserText
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeRspGenUserText>
                    |> Option.map (TraderBridgeMapping.rspGenUserText encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspHedgeCfm =
        TraderRspHedgeCfmDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspHedgeCfm
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputHedgeCfm>
                    |> Option.map (TraderBridgeMapping.inputHedgeCfm encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspHedgeCfmAction =
        TraderRspHedgeCfmActionDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspHedgeCfmAction
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputHedgeCfmAction>
                    |> Option.map (TraderBridgeMapping.inputHedgeCfmAction encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspOffsetSetting =
        TraderRspOffsetSettingDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspOffsetSetting
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputOffsetSetting>
                    |> Option.map (TraderBridgeMapping.inputOffsetSetting encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspOptionSelfCloseAction =
        TraderRspOptionSelfCloseActionDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspOptionSelfCloseAction
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputOptionSelfCloseAction>
                    |> Option.map (TraderBridgeMapping.inputOptionSelfCloseAction encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspOptionSelfCloseInsert =
        TraderRspOptionSelfCloseInsertDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspOptionSelfCloseInsert
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputOptionSelfClose>
                    |> Option.map (TraderBridgeMapping.inputOptionSelfClose encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspParkedOrderAction =
        TraderRspParkedOrderActionDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspParkedOrderAction
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeParkedOrderAction>
                    |> Option.map (TraderBridgeMapping.parkedOrderAction encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspParkedOrderInsert =
        TraderRspParkedOrderInsertDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspParkedOrderInsert
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeParkedOrder>
                    |> Option.map (TraderBridgeMapping.parkedOrder encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryAccountregister =
        TraderRspQryAccountregisterDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryAccountregister
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeAccountregister>
                    |> Option.map (TraderBridgeMapping.accountregister encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryBrokerTradingAlgos =
        TraderRspQryBrokerTradingAlgosDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryBrokerTradingAlgos
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeBrokerTradingAlgos>
                    |> Option.map (TraderBridgeMapping.brokerTradingAlgos encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryBrokerTradingParams =
        TraderRspQryBrokerTradingParamsDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryBrokerTradingParams
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeBrokerTradingParams>
                    |> Option.map (TraderBridgeMapping.brokerTradingParams encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryCfmmcTradingAccountKey =
        TraderRspQryCfmmcTradingAccountKeyDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryCfmmcTradingAccountKey
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeCfmmcTradingAccountKey>
                    |> Option.map (TraderBridgeMapping.cfmmcTradingAccountKey encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryClassifiedInstrument =
        TraderRspQryClassifiedInstrumentDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryClassifiedInstrument
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInstrument>
                    |> Option.map (TraderBridgeMapping.instrument encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryCombAction =
        TraderRspQryCombActionDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryCombAction
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeCombAction>
                    |> Option.map (TraderBridgeMapping.combAction encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryCombInstrumentGuard =
        TraderRspQryCombInstrumentGuardDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryCombInstrumentGuard
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeCombInstrumentGuard>
                    |> Option.map (TraderBridgeMapping.combInstrumentGuard encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryCombLeg =
        TraderRspQryCombLegDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryCombLeg
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeCombLeg>
                    |> Option.map (TraderBridgeMapping.combLeg encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryCombPromotionParam =
        TraderRspQryCombPromotionParamDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryCombPromotionParam
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeCombPromotionParam>
                    |> Option.map (TraderBridgeMapping.combPromotionParam encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryContractBank =
        TraderRspQryContractBankDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryContractBank
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeContractBank>
                    |> Option.map (TraderBridgeMapping.contractBank encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryDepthMarketData =
        TraderRspQryDepthMarketDataDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryDepthMarketData
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeTraderDepthMarketData>
                    |> Option.map (TraderBridgeMapping.depthMarketData encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryEWarrantOffset =
        TraderRspQryEWarrantOffsetDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryEWarrantOffset
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeEWarrantOffset>
                    |> Option.map (TraderBridgeMapping.eWarrantOffset encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryExchange =
        TraderRspQryExchangeDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryExchange
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeExchange>
                    |> Option.map (TraderBridgeMapping.exchange encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryExchangeMarginRateAdjust =
        TraderRspQryExchangeMarginRateAdjustDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryExchangeMarginRateAdjust
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeExchangeMarginRateAdjust>
                    |> Option.map (TraderBridgeMapping.exchangeMarginRateAdjust encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryExchangeRate =
        TraderRspQryExchangeRateDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryExchangeRate
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeExchangeRate>
                    |> Option.map (TraderBridgeMapping.exchangeRate encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryExecOrder =
        TraderRspQryExecOrderDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryExecOrder
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeExecOrder>
                    |> Option.map (TraderBridgeMapping.execOrder encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryForQuote =
        TraderRspQryForQuoteDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryForQuote
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeForQuote>
                    |> Option.map (TraderBridgeMapping.forQuote encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryHedgeCfm =
        TraderRspQryHedgeCfmDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryHedgeCfm
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeHedgeCfm>
                    |> Option.map (TraderBridgeMapping.hedgeCfm encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryInstrument =
        TraderRspQryInstrumentDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryInstrument
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInstrument>
                    |> Option.map (TraderBridgeMapping.instrument encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryInstrumentOrderCommRate =
        TraderRspQryInstrumentOrderCommRateDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryInstrumentOrderCommRate
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInstrumentOrderCommRate>
                    |> Option.map (TraderBridgeMapping.instrumentOrderCommRate encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryInvestUnit =
        TraderRspQryInvestUnitDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryInvestUnit
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInvestUnit>
                    |> Option.map (TraderBridgeMapping.investUnit encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryInvestor =
        TraderRspQryInvestorDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryInvestor
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInvestor>
                    |> Option.map (TraderBridgeMapping.investor encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryInvestorCommodityGroupSpmmMargin =
        TraderRspQryInvestorCommodityGroupSpmmMarginDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryInvestorCommodityGroupSpmmMargin
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInvestorCommodityGroupSpmmMargin>
                    |> Option.map (TraderBridgeMapping.investorCommodityGroupSpmmMargin encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryInvestorCommoditySpmmMargin =
        TraderRspQryInvestorCommoditySpmmMarginDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryInvestorCommoditySpmmMargin
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInvestorCommoditySpmmMargin>
                    |> Option.map (TraderBridgeMapping.investorCommoditySpmmMargin encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryInvestorInfoCommRec =
        TraderRspQryInvestorInfoCommRecDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryInvestorInfoCommRec
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInvestorInfoCommRec>
                    |> Option.map (TraderBridgeMapping.investorInfoCommRec encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryInvestorPortfMarginRatio =
        TraderRspQryInvestorPortfMarginRatioDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryInvestorPortfMarginRatio
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInvestorPortfMarginRatio>
                    |> Option.map (TraderBridgeMapping.investorPortfMarginRatio encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryInvestorPortfSetting =
        TraderRspQryInvestorPortfSettingDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryInvestorPortfSetting
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInvestorPortfSetting>
                    |> Option.map (TraderBridgeMapping.investorPortfSetting encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryInvestorPositionCombineDetail =
        TraderRspQryInvestorPositionCombineDetailDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryInvestorPositionCombineDetail
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInvestorPositionCombineDetail>
                    |> Option.map (TraderBridgeMapping.investorPositionCombineDetail encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryInvestorPositionDetail =
        TraderRspQryInvestorPositionDetailDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryInvestorPositionDetail
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInvestorPositionDetail>
                    |> Option.map (TraderBridgeMapping.investorPositionDetail encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryInvestorProdRcamsMargin =
        TraderRspQryInvestorProdRcamsMarginDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryInvestorProdRcamsMargin
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInvestorProdRcamsMargin>
                    |> Option.map (TraderBridgeMapping.investorProdRcamsMargin encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryInvestorProdRuleMargin =
        TraderRspQryInvestorProdRuleMarginDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryInvestorProdRuleMargin
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInvestorProdRuleMargin>
                    |> Option.map (TraderBridgeMapping.investorProdRuleMargin encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryInvestorProdSpbmDetail =
        TraderRspQryInvestorProdSpbmDetailDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryInvestorProdSpbmDetail
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInvestorProdSpbmDetail>
                    |> Option.map (TraderBridgeMapping.investorProdSpbmDetail encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryInvestorProductGroupMargin =
        TraderRspQryInvestorProductGroupMarginDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryInvestorProductGroupMargin
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInvestorProductGroupMargin>
                    |> Option.map (TraderBridgeMapping.investorProductGroupMargin encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryMmInstrumentCommissionRate =
        TraderRspQryMmInstrumentCommissionRateDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryMmInstrumentCommissionRate
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeMmInstrumentCommissionRate>
                    |> Option.map (TraderBridgeMapping.mmInstrumentCommissionRate encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryMmOptionInstrCommRate =
        TraderRspQryMmOptionInstrCommRateDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryMmOptionInstrCommRate
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeMmOptionInstrCommRate>
                    |> Option.map (TraderBridgeMapping.mmOptionInstrCommRate encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryMaxOrderVolume =
        TraderRspQryMaxOrderVolumeDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryMaxOrderVolume
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeQryMaxOrderVolume>
                    |> Option.map (TraderBridgeMapping.qryMaxOrderVolume encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryNotice =
        TraderRspQryNoticeDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryNotice
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeNotice>
                    |> Option.map (TraderBridgeMapping.notice encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryOffsetSetting =
        TraderRspQryOffsetSettingDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryOffsetSetting
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeOffsetSetting>
                    |> Option.map (TraderBridgeMapping.offsetSetting encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryOptionInstrCommRate =
        TraderRspQryOptionInstrCommRateDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryOptionInstrCommRate
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeOptionInstrCommRate>
                    |> Option.map (TraderBridgeMapping.optionInstrCommRate encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryOptionInstrTradeCost =
        TraderRspQryOptionInstrTradeCostDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryOptionInstrTradeCost
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeOptionInstrTradeCost>
                    |> Option.map (TraderBridgeMapping.optionInstrTradeCost encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryOptionSelfClose =
        TraderRspQryOptionSelfCloseDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryOptionSelfClose
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeOptionSelfClose>
                    |> Option.map (TraderBridgeMapping.optionSelfClose encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryOrder =
        TraderRspQryOrderDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryOrder
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeOrder>
                    |> Option.map (TraderBridgeMapping.orderUpdate encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryParkedOrder =
        TraderRspQryParkedOrderDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryParkedOrder
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeParkedOrder>
                    |> Option.map (TraderBridgeMapping.parkedOrder encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryParkedOrderAction =
        TraderRspQryParkedOrderActionDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryParkedOrderAction
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeParkedOrderAction>
                    |> Option.map (TraderBridgeMapping.parkedOrderAction encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryProduct =
        TraderRspQryProductDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryProduct
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeProduct>
                    |> Option.map (TraderBridgeMapping.product encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryProductExchRate =
        TraderRspQryProductExchRateDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryProductExchRate
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeProductExchRate>
                    |> Option.map (TraderBridgeMapping.productExchRate encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryProductGroup =
        TraderRspQryProductGroupDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryProductGroup
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeProductGroup>
                    |> Option.map (TraderBridgeMapping.productGroup encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryQuote =
        TraderRspQryQuoteDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryQuote
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeQuote>
                    |> Option.map (TraderBridgeMapping.quote encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryRcamsCombProductInfo =
        TraderRspQryRcamsCombProductInfoDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryRcamsCombProductInfo
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeRcamsCombProductInfo>
                    |> Option.map (TraderBridgeMapping.rcamsCombProductInfo encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryRcamsInstrParameter =
        TraderRspQryRcamsInstrParameterDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryRcamsInstrParameter
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeRcamsInstrParameter>
                    |> Option.map (TraderBridgeMapping.rcamsInstrParameter encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryRcamsInterParameter =
        TraderRspQryRcamsInterParameterDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryRcamsInterParameter
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeRcamsInterParameter>
                    |> Option.map (TraderBridgeMapping.rcamsInterParameter encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryRcamsIntraParameter =
        TraderRspQryRcamsIntraParameterDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryRcamsIntraParameter
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeRcamsIntraParameter>
                    |> Option.map (TraderBridgeMapping.rcamsIntraParameter encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryRcamsInvestorCombPosition =
        TraderRspQryRcamsInvestorCombPositionDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryRcamsInvestorCombPosition
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeRcamsInvestorCombPosition>
                    |> Option.map (TraderBridgeMapping.rcamsInvestorCombPosition encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryRcamsShortOptAdjustParam =
        TraderRspQryRcamsShortOptAdjustParamDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryRcamsShortOptAdjustParam
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeRcamsShortOptAdjustParam>
                    |> Option.map (TraderBridgeMapping.rcamsShortOptAdjustParam encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryRuleInstrParameter =
        TraderRspQryRuleInstrParameterDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryRuleInstrParameter
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeRuleInstrParameter>
                    |> Option.map (TraderBridgeMapping.ruleInstrParameter encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryRuleInterParameter =
        TraderRspQryRuleInterParameterDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryRuleInterParameter
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeRuleInterParameter>
                    |> Option.map (TraderBridgeMapping.ruleInterParameter encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryRuleIntraParameter =
        TraderRspQryRuleIntraParameterDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryRuleIntraParameter
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeRuleIntraParameter>
                    |> Option.map (TraderBridgeMapping.ruleIntraParameter encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryRiskSettleInvstPosition =
        TraderRspQryRiskSettleInvstPositionDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryRiskSettleInvstPosition
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeRiskSettleInvstPosition>
                    |> Option.map (TraderBridgeMapping.riskSettleInvstPosition encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryRiskSettleProductStatus =
        TraderRspQryRiskSettleProductStatusDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryRiskSettleProductStatus
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeRiskSettleProductStatus>
                    |> Option.map (TraderBridgeMapping.riskSettleProductStatus encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQrySpbmAddOnInterParameter =
        TraderRspQrySpbmAddOnInterParameterDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQrySpbmAddOnInterParameter
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeSpbmAddOnInterParameter>
                    |> Option.map (TraderBridgeMapping.spbmAddOnInterParameter encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQrySpbmFutureParameter =
        TraderRspQrySpbmFutureParameterDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQrySpbmFutureParameter
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeSpbmFutureParameter>
                    |> Option.map (TraderBridgeMapping.spbmFutureParameter encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQrySpbmInterParameter =
        TraderRspQrySpbmInterParameterDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQrySpbmInterParameter
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeSpbmInterParameter>
                    |> Option.map (TraderBridgeMapping.spbmInterParameter encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQrySpbmIntraParameter =
        TraderRspQrySpbmIntraParameterDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQrySpbmIntraParameter
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeSpbmIntraParameter>
                    |> Option.map (TraderBridgeMapping.spbmIntraParameter encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQrySpbmInvestorPortfDef =
        TraderRspQrySpbmInvestorPortfDefDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQrySpbmInvestorPortfDef
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeSpbmInvestorPortfDef>
                    |> Option.map (TraderBridgeMapping.spbmInvestorPortfDef encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQrySpbmOptionParameter =
        TraderRspQrySpbmOptionParameterDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQrySpbmOptionParameter
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeSpbmOptionParameter>
                    |> Option.map (TraderBridgeMapping.spbmOptionParameter encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQrySpbmPortfDefinition =
        TraderRspQrySpbmPortfDefinitionDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQrySpbmPortfDefinition
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeSpbmPortfDefinition>
                    |> Option.map (TraderBridgeMapping.spbmPortfDefinition encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQrySpmmInstParam =
        TraderRspQrySpmmInstParamDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQrySpmmInstParam
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeSpmmInstParam>
                    |> Option.map (TraderBridgeMapping.spmmInstParam encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQrySpmmProductParam =
        TraderRspQrySpmmProductParamDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQrySpmmProductParam
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeSpmmProductParam>
                    |> Option.map (TraderBridgeMapping.spmmProductParam encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQrySecAgentAcIdMap =
        TraderRspQrySecAgentAcIdMapDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQrySecAgentAcIdMap
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeSecAgentAcIdMap>
                    |> Option.map (TraderBridgeMapping.secAgentAcIdMap encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQrySecAgentCheckMode =
        TraderRspQrySecAgentCheckModeDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQrySecAgentCheckMode
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeSecAgentCheckMode>
                    |> Option.map (TraderBridgeMapping.secAgentCheckMode encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQrySecAgentTradeInfo =
        TraderRspQrySecAgentTradeInfoDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQrySecAgentTradeInfo
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeSecAgentTradeInfo>
                    |> Option.map (TraderBridgeMapping.secAgentTradeInfo encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQrySecAgentTradingAccount =
        TraderRspQrySecAgentTradingAccountDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQrySecAgentTradingAccount
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeTradingAccount>
                    |> Option.map (TraderBridgeMapping.tradingAccount encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQrySettlementInfo =
        TraderRspQrySettlementInfoDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQrySettlementInfo
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeSettlementInfo>
                    |> Option.map (TraderBridgeMapping.settlementInfo encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQrySettlementInfoConfirm =
        TraderRspQrySettlementInfoConfirmDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQrySettlementInfoConfirm
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeSettlementInfoConfirm>
                    |> Option.map (TraderBridgeMapping.settlementInfoConfirm encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQrySpdApply =
        TraderRspQrySpdApplyDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQrySpdApply
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeSpdApply>
                    |> Option.map (TraderBridgeMapping.spdApply encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryTrade =
        TraderRspQryTradeDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryTrade
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeTrade>
                    |> Option.map (TraderBridgeMapping.tradeUpdate encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryTraderOffer =
        TraderRspQryTraderOfferDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryTraderOffer
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeTraderOffer>
                    |> Option.map (TraderBridgeMapping.traderOffer encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryTradingCode =
        TraderRspQryTradingCodeDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryTradingCode
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeTradingCode>
                    |> Option.map (TraderBridgeMapping.tradingCode encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryTradingNotice =
        TraderRspQryTradingNoticeDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryTradingNotice
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeTradingNotice>
                    |> Option.map (TraderBridgeMapping.tradingNotice encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryTransferBank =
        TraderRspQryTransferBankDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryTransferBank
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeTransferBank>
                    |> Option.map (TraderBridgeMapping.transferBank encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryTransferSerial =
        TraderRspQryTransferSerialDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryTransferSerial
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeTransferSerial>
                    |> Option.map (TraderBridgeMapping.transferSerial encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQryUserSession =
        TraderRspQryUserSessionDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryUserSession
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeUserSession>
                    |> Option.map (TraderBridgeMapping.userSession encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQueryBankAccountMoneyByFuture =
        TraderRspQueryBankAccountMoneyByFutureDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQueryBankAccountMoneyByFuture
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeReqQueryAccount>
                    |> Option.map (TraderBridgeMapping.reqQueryAccount encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQueryCfmmcTradingAccountToken =
        TraderRspQueryCfmmcTradingAccountTokenDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQueryCfmmcTradingAccountToken
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeQueryCfmmcTradingAccountToken>
                    |> Option.map (TraderBridgeMapping.queryCfmmcTradingAccountToken encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQuoteAction =
        TraderRspQuoteActionDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQuoteAction
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputQuoteAction>
                    |> Option.map (TraderBridgeMapping.inputQuoteAction encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspQuoteInsert =
        TraderRspQuoteInsertDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQuoteInsert
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputQuote>
                    |> Option.map (TraderBridgeMapping.inputQuote encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspRemoveParkedOrder =
        TraderRspRemoveParkedOrderDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspRemoveParkedOrder
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeRemoveParkedOrder>
                    |> Option.map (TraderBridgeMapping.removeParkedOrder encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspRemoveParkedOrderAction =
        TraderRspRemoveParkedOrderActionDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspRemoveParkedOrderAction
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeRemoveParkedOrderAction>
                    |> Option.map (TraderBridgeMapping.removeParkedOrderAction encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspSpdApply =
        TraderRspSpdApplyDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspSpdApply
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputSpdApply>
                    |> Option.map (TraderBridgeMapping.inputSpdApply encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspSpdApplyAction =
        TraderRspSpdApplyActionDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspSpdApplyAction
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeInputSpdApplyAction>
                    |> Option.map (TraderBridgeMapping.inputSpdApplyAction encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspTradingAccountPasswordUpdate =
        TraderRspTradingAccountPasswordUpdateDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspTradingAccountPasswordUpdate
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeTradingAccountPasswordUpdate>
                    |> Option.map (TraderBridgeMapping.tradingAccountPasswordUpdate encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspUserAuthMethod =
        TraderRspUserAuthMethodDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspUserAuthMethod
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeRspUserAuthMethod>
                    |> Option.map (TraderBridgeMapping.rspUserAuthMethod encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rspUserPasswordUpdate =
        TraderRspUserPasswordUpdateDelegate(fun itemPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspUserPasswordUpdate
            |> Option.iter (fun handler ->
                let item =
                    itemPtr
                    |> EncodingHelpers.ptrToOption<NativeUserPasswordUpdate>
                    |> Option.map (TraderBridgeMapping.userPasswordUpdate encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler item rspInfo requestId (isLast <> 0)))

    let rtnCombAction =
        TraderRtnCombActionDelegate(fun itemPtr _ ->
            callbacks.RtnCombAction
            |> Option.iter (fun handler ->
                match EncodingHelpers.ptrToOption<NativeCombAction> itemPtr with
                | Some item -> handler (TraderBridgeMapping.combAction encodings.InboundEncoding item)
                | None -> ()))

    let rtnExecOrder =
        TraderRtnExecOrderDelegate(fun itemPtr _ ->
            callbacks.RtnExecOrder
            |> Option.iter (fun handler ->
                match EncodingHelpers.ptrToOption<NativeExecOrder> itemPtr with
                | Some item -> handler (TraderBridgeMapping.execOrder encodings.InboundEncoding item)
                | None -> ()))

    let rtnForQuoteRsp =
        TraderRtnForQuoteRspDelegate(fun itemPtr _ ->
            callbacks.RtnForQuoteRsp
            |> Option.iter (fun handler ->
                match EncodingHelpers.ptrToOption<NativeForQuoteRsp> itemPtr with
                | Some item -> handler (TraderBridgeMapping.forQuoteRsp encodings.InboundEncoding item)
                | None -> ()))

    let rtnFromBankToFutureByFuture =
        TraderRtnFromBankToFutureByFutureDelegate(fun itemPtr _ ->
            callbacks.RtnFromBankToFutureByFuture
            |> Option.iter (fun handler ->
                match EncodingHelpers.ptrToOption<NativeRspTransfer> itemPtr with
                | Some item -> handler (TraderBridgeMapping.rspTransfer encodings.InboundEncoding item)
                | None -> ()))

    let rtnFromFutureToBankByFuture =
        TraderRtnFromFutureToBankByFutureDelegate(fun itemPtr _ ->
            callbacks.RtnFromFutureToBankByFuture
            |> Option.iter (fun handler ->
                match EncodingHelpers.ptrToOption<NativeRspTransfer> itemPtr with
                | Some item -> handler (TraderBridgeMapping.rspTransfer encodings.InboundEncoding item)
                | None -> ()))

    let rtnHedgeCfm =
        TraderRtnHedgeCfmDelegate(fun itemPtr _ ->
            callbacks.RtnHedgeCfm
            |> Option.iter (fun handler ->
                match EncodingHelpers.ptrToOption<NativeHedgeCfm> itemPtr with
                | Some item -> handler (TraderBridgeMapping.hedgeCfm encodings.InboundEncoding item)
                | None -> ()))

    let rtnOffsetSetting =
        TraderRtnOffsetSettingDelegate(fun itemPtr _ ->
            callbacks.RtnOffsetSetting
            |> Option.iter (fun handler ->
                match EncodingHelpers.ptrToOption<NativeOffsetSetting> itemPtr with
                | Some item -> handler (TraderBridgeMapping.offsetSetting encodings.InboundEncoding item)
                | None -> ()))

    let rtnOptionSelfClose =
        TraderRtnOptionSelfCloseDelegate(fun itemPtr _ ->
            callbacks.RtnOptionSelfClose
            |> Option.iter (fun handler ->
                match EncodingHelpers.ptrToOption<NativeOptionSelfClose> itemPtr with
                | Some item -> handler (TraderBridgeMapping.optionSelfClose encodings.InboundEncoding item)
                | None -> ()))

    let rtnQueryBankBalanceByFuture =
        TraderRtnQueryBankBalanceByFutureDelegate(fun itemPtr _ ->
            callbacks.RtnQueryBankBalanceByFuture
            |> Option.iter (fun handler ->
                match EncodingHelpers.ptrToOption<NativeNotifyQueryAccount> itemPtr with
                | Some item -> handler (TraderBridgeMapping.notifyQueryAccount encodings.InboundEncoding item)
                | None -> ()))

    let rtnQuote =
        TraderRtnQuoteDelegate(fun itemPtr _ ->
            callbacks.RtnQuote
            |> Option.iter (fun handler ->
                match EncodingHelpers.ptrToOption<NativeQuote> itemPtr with
                | Some item -> handler (TraderBridgeMapping.quote encodings.InboundEncoding item)
                | None -> ()))

    let rtnSpdApply =
        TraderRtnSpdApplyDelegate(fun itemPtr _ ->
            callbacks.RtnSpdApply
            |> Option.iter (fun handler ->
                match EncodingHelpers.ptrToOption<NativeSpdApply> itemPtr with
                | Some item -> handler (TraderBridgeMapping.spdApply encodings.InboundEncoding item)
                | None -> ()))

    let mutable native = NativeTraderSpi()

    do
        native.FrontConnected <- onFrontConnected
        native.FrontDisconnected <- onFrontDisconnected
        native.HeartBeatWarning <- onHeartBeatWarning
        native.RtnPrivateSeqNo <- onRtnPrivateSeqNo
        native.RspAuthenticate <- rspAuthenticate
        native.RspSettlementInfoConfirm <- rspSettlementInfoConfirm
        native.RspUserLogin <- rspUserLogin
        native.RspUserLogout <- rspUserLogout
        native.RspError <- rspError
        native.RspQryTradingAccount <- rspQryTradingAccount
        native.RspQryInvestorPosition <- rspQryInvestorPosition
        native.RspQryInstrumentMarginRate <- rspQryInstrumentMarginRate
        native.RspQryExchangeMarginRate <- rspQryExchangeMarginRate
        native.RspQryInstrumentCommissionRate <- rspQryInstrumentCommissionRate
        native.RspOrderInsert <- rspOrderInsert
        native.RspOrderAction <- rspOrderAction
        native.RtnOrder <- rtnOrder
        native.RtnTrade <- rtnTrade
        native.ErrRtnBankToFutureByFuture <- errRtnBankToFutureByFuture
        native.ErrRtnBatchOrderAction <- errRtnBatchOrderAction
        native.ErrRtnCancelOffsetSetting <- errRtnCancelOffsetSetting
        native.ErrRtnCombActionInsert <- errRtnCombActionInsert
        native.ErrRtnExecOrderAction <- errRtnExecOrderAction
        native.ErrRtnExecOrderInsert <- errRtnExecOrderInsert
        native.ErrRtnForQuoteInsert <- errRtnForQuoteInsert
        native.ErrRtnFutureToBankByFuture <- errRtnFutureToBankByFuture
        native.ErrRtnHedgeCfm <- errRtnHedgeCfm
        native.ErrRtnHedgeCfmAction <- errRtnHedgeCfmAction
        native.ErrRtnOffsetSetting <- errRtnOffsetSetting
        native.ErrRtnOptionSelfCloseAction <- errRtnOptionSelfCloseAction
        native.ErrRtnOptionSelfCloseInsert <- errRtnOptionSelfCloseInsert
        native.ErrRtnOrderAction <- errRtnOrderAction
        native.ErrRtnOrderInsert <- errRtnOrderInsert
        native.ErrRtnQueryBankBalanceByFuture <- errRtnQueryBankBalanceByFuture
        native.ErrRtnQuoteAction <- errRtnQuoteAction
        native.ErrRtnQuoteInsert <- errRtnQuoteInsert
        native.ErrRtnSpdApply <- errRtnSpdApply
        native.ErrRtnSpdApplyAction <- errRtnSpdApplyAction
        native.RspBatchOrderAction <- rspBatchOrderAction
        native.RspCancelOffsetSetting <- rspCancelOffsetSetting
        native.RspCombActionInsert <- rspCombActionInsert
        native.RspExecOrderAction <- rspExecOrderAction
        native.RspExecOrderInsert <- rspExecOrderInsert
        native.RspForQuoteInsert <- rspForQuoteInsert
        native.RspFromBankToFutureByFuture <- rspFromBankToFutureByFuture
        native.RspFromFutureToBankByFuture <- rspFromFutureToBankByFuture
        native.RspGenSmsCode <- rspGenSmsCode
        native.RspGenUserCaptcha <- rspGenUserCaptcha
        native.RspGenUserText <- rspGenUserText
        native.RspHedgeCfm <- rspHedgeCfm
        native.RspHedgeCfmAction <- rspHedgeCfmAction
        native.RspOffsetSetting <- rspOffsetSetting
        native.RspOptionSelfCloseAction <- rspOptionSelfCloseAction
        native.RspOptionSelfCloseInsert <- rspOptionSelfCloseInsert
        native.RspParkedOrderAction <- rspParkedOrderAction
        native.RspParkedOrderInsert <- rspParkedOrderInsert
        native.RspQryAccountregister <- rspQryAccountregister
        native.RspQryBrokerTradingAlgos <- rspQryBrokerTradingAlgos
        native.RspQryBrokerTradingParams <- rspQryBrokerTradingParams
        native.RspQryCfmmcTradingAccountKey <- rspQryCfmmcTradingAccountKey
        native.RspQryClassifiedInstrument <- rspQryClassifiedInstrument
        native.RspQryCombAction <- rspQryCombAction
        native.RspQryCombInstrumentGuard <- rspQryCombInstrumentGuard
        native.RspQryCombLeg <- rspQryCombLeg
        native.RspQryCombPromotionParam <- rspQryCombPromotionParam
        native.RspQryContractBank <- rspQryContractBank
        native.RspQryDepthMarketData <- rspQryDepthMarketData
        native.RspQryEWarrantOffset <- rspQryEWarrantOffset
        native.RspQryExchange <- rspQryExchange
        native.RspQryExchangeMarginRateAdjust <- rspQryExchangeMarginRateAdjust
        native.RspQryExchangeRate <- rspQryExchangeRate
        native.RspQryExecOrder <- rspQryExecOrder
        native.RspQryForQuote <- rspQryForQuote
        native.RspQryHedgeCfm <- rspQryHedgeCfm
        native.RspQryInstrument <- rspQryInstrument
        native.RspQryInstrumentOrderCommRate <- rspQryInstrumentOrderCommRate
        native.RspQryInvestUnit <- rspQryInvestUnit
        native.RspQryInvestor <- rspQryInvestor
        native.RspQryInvestorCommodityGroupSpmmMargin <- rspQryInvestorCommodityGroupSpmmMargin
        native.RspQryInvestorCommoditySpmmMargin <- rspQryInvestorCommoditySpmmMargin
        native.RspQryInvestorInfoCommRec <- rspQryInvestorInfoCommRec
        native.RspQryInvestorPortfMarginRatio <- rspQryInvestorPortfMarginRatio
        native.RspQryInvestorPortfSetting <- rspQryInvestorPortfSetting
        native.RspQryInvestorPositionCombineDetail <- rspQryInvestorPositionCombineDetail
        native.RspQryInvestorPositionDetail <- rspQryInvestorPositionDetail
        native.RspQryInvestorProdRcamsMargin <- rspQryInvestorProdRcamsMargin
        native.RspQryInvestorProdRuleMargin <- rspQryInvestorProdRuleMargin
        native.RspQryInvestorProdSpbmDetail <- rspQryInvestorProdSpbmDetail
        native.RspQryInvestorProductGroupMargin <- rspQryInvestorProductGroupMargin
        native.RspQryMmInstrumentCommissionRate <- rspQryMmInstrumentCommissionRate
        native.RspQryMmOptionInstrCommRate <- rspQryMmOptionInstrCommRate
        native.RspQryMaxOrderVolume <- rspQryMaxOrderVolume
        native.RspQryNotice <- rspQryNotice
        native.RspQryOffsetSetting <- rspQryOffsetSetting
        native.RspQryOptionInstrCommRate <- rspQryOptionInstrCommRate
        native.RspQryOptionInstrTradeCost <- rspQryOptionInstrTradeCost
        native.RspQryOptionSelfClose <- rspQryOptionSelfClose
        native.RspQryOrder <- rspQryOrder
        native.RspQryParkedOrder <- rspQryParkedOrder
        native.RspQryParkedOrderAction <- rspQryParkedOrderAction
        native.RspQryProduct <- rspQryProduct
        native.RspQryProductExchRate <- rspQryProductExchRate
        native.RspQryProductGroup <- rspQryProductGroup
        native.RspQryQuote <- rspQryQuote
        native.RspQryRcamsCombProductInfo <- rspQryRcamsCombProductInfo
        native.RspQryRcamsInstrParameter <- rspQryRcamsInstrParameter
        native.RspQryRcamsInterParameter <- rspQryRcamsInterParameter
        native.RspQryRcamsIntraParameter <- rspQryRcamsIntraParameter
        native.RspQryRcamsInvestorCombPosition <- rspQryRcamsInvestorCombPosition
        native.RspQryRcamsShortOptAdjustParam <- rspQryRcamsShortOptAdjustParam
        native.RspQryRuleInstrParameter <- rspQryRuleInstrParameter
        native.RspQryRuleInterParameter <- rspQryRuleInterParameter
        native.RspQryRuleIntraParameter <- rspQryRuleIntraParameter
        native.RspQryRiskSettleInvstPosition <- rspQryRiskSettleInvstPosition
        native.RspQryRiskSettleProductStatus <- rspQryRiskSettleProductStatus
        native.RspQrySpbmAddOnInterParameter <- rspQrySpbmAddOnInterParameter
        native.RspQrySpbmFutureParameter <- rspQrySpbmFutureParameter
        native.RspQrySpbmInterParameter <- rspQrySpbmInterParameter
        native.RspQrySpbmIntraParameter <- rspQrySpbmIntraParameter
        native.RspQrySpbmInvestorPortfDef <- rspQrySpbmInvestorPortfDef
        native.RspQrySpbmOptionParameter <- rspQrySpbmOptionParameter
        native.RspQrySpbmPortfDefinition <- rspQrySpbmPortfDefinition
        native.RspQrySpmmInstParam <- rspQrySpmmInstParam
        native.RspQrySpmmProductParam <- rspQrySpmmProductParam
        native.RspQrySecAgentAcIdMap <- rspQrySecAgentAcIdMap
        native.RspQrySecAgentCheckMode <- rspQrySecAgentCheckMode
        native.RspQrySecAgentTradeInfo <- rspQrySecAgentTradeInfo
        native.RspQrySecAgentTradingAccount <- rspQrySecAgentTradingAccount
        native.RspQrySettlementInfo <- rspQrySettlementInfo
        native.RspQrySettlementInfoConfirm <- rspQrySettlementInfoConfirm
        native.RspQrySpdApply <- rspQrySpdApply
        native.RspQryTrade <- rspQryTrade
        native.RspQryTraderOffer <- rspQryTraderOffer
        native.RspQryTradingCode <- rspQryTradingCode
        native.RspQryTradingNotice <- rspQryTradingNotice
        native.RspQryTransferBank <- rspQryTransferBank
        native.RspQryTransferSerial <- rspQryTransferSerial
        native.RspQryUserSession <- rspQryUserSession
        native.RspQueryBankAccountMoneyByFuture <- rspQueryBankAccountMoneyByFuture
        native.RspQueryCfmmcTradingAccountToken <- rspQueryCfmmcTradingAccountToken
        native.RspQuoteAction <- rspQuoteAction
        native.RspQuoteInsert <- rspQuoteInsert
        native.RspRemoveParkedOrder <- rspRemoveParkedOrder
        native.RspRemoveParkedOrderAction <- rspRemoveParkedOrderAction
        native.RspSpdApply <- rspSpdApply
        native.RspSpdApplyAction <- rspSpdApplyAction
        native.RspTradingAccountPasswordUpdate <- rspTradingAccountPasswordUpdate
        native.RspUserAuthMethod <- rspUserAuthMethod
        native.RspUserPasswordUpdate <- rspUserPasswordUpdate
        native.RtnCombAction <- rtnCombAction
        native.RtnExecOrder <- rtnExecOrder
        native.RtnForQuoteRsp <- rtnForQuoteRsp
        native.RtnFromBankToFutureByFuture <- rtnFromBankToFutureByFuture
        native.RtnFromFutureToBankByFuture <- rtnFromFutureToBankByFuture
        native.RtnHedgeCfm <- rtnHedgeCfm
        native.RtnOffsetSetting <- rtnOffsetSetting
        native.RtnOptionSelfClose <- rtnOptionSelfClose
        native.RtnQueryBankBalanceByFuture <- rtnQueryBankBalanceByFuture
        native.RtnQuote <- rtnQuote
        native.RtnSpdApply <- rtnSpdApply

    member _.Native = native

type TraderApi(flowPath: string option, productionMode: bool, ?encodings: EncodingPair) =
    let encodings = defaultArg encodings EncodingPair.Default

    let handle =
        BridgeResolver.ensureRegistered ()
        let productionFlag = if productionMode then 1 else 0

        BridgeHelpers.withEncodedCString encodings.OutboundEncoding flowPath (fun ptr ->
            TraderNativeInterop.create (ptr, productionFlag))
        |> TraderBridgeHelpers.toTraderHandle

    let mutable callbackRegistrationLifetime: TraderSpiRegistration option = None

    member private _.Handle =
        if handle.IsInvalid then
            invalidOp "The trader API handle is invalid."

        handle.DangerousGetHandle()

    static member GetApiVersion() =
        BridgeResolver.ensureRegistered ()
        TraderNativeInterop.getApiVersion () |> BridgeHelpers.ptrToAnsiString

    member this.SetCallbacks(callbacks: TraderCallbacks) =
        let registration = TraderSpiRegistration(callbacks, encodings)
        let mutable native = registration.Native
        let result = TraderNativeInterop.setSpi (this.Handle, &native, 0n)
        BridgeHelpers.throwOnNonZero result "ctp_trader_set_spi"
        callbackRegistrationLifetime <- Some registration

    member this.RegisterFront(frontAddress: string) =
        BridgeHelpers.withEncodedCString encodings.OutboundEncoding (Some frontAddress) (fun ptr ->
            TraderNativeInterop.registerFront (this.Handle, ptr))
        |> BridgeHelpers.throwOnNonZero
        <| "ctp_trader_register_front"

    member this.SubscribePrivateTopic(resumeType: int, seqNo: int) =
        TraderNativeInterop.subscribePrivateTopic (this.Handle, resumeType, seqNo)
        |> BridgeHelpers.throwOnNonZero
        <| "ctp_trader_subscribe_private_topic"

    member this.SubscribePublicTopic(resumeType: int) =
        TraderNativeInterop.subscribePublicTopic (this.Handle, resumeType)
        |> BridgeHelpers.throwOnNonZero
        <| "ctp_trader_subscribe_public_topic"

    member this.Init() = TraderNativeInterop.init (this.Handle)

    member this.Join() = TraderNativeInterop.join (this.Handle)

    member this.ReqAuthenticate(request: AuthenticateRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.reqAuthenticate encodings.OutboundEncoding request
        TraderNativeInterop.reqAuthenticate (this.Handle, &native, requestId)

    member this.ReqSettlementInfoConfirm(request: SettlementInfoConfirm, requestId: int) =
        let mutable native = TraderBridgeBuilders.settlementInfoConfirm encodings.OutboundEncoding request
        TraderNativeInterop.reqSettlementInfoConfirm (this.Handle, &native, requestId)

    member this.ReqUserLogin(request: UserLoginRequest, requestId: int) =
        let mutable native = BridgeBuilders.reqUserLogin encodings.OutboundEncoding request
        TraderNativeInterop.reqUserLogin (this.Handle, &native, requestId)

    member this.ReqUserLogout(request: UserLogoutRequest, requestId: int) =
        let mutable native = BridgeBuilders.reqUserLogout encodings.OutboundEncoding request
        TraderNativeInterop.reqUserLogout (this.Handle, &native, requestId)

    member this.ReqQryTradingAccount(request: QueryTradingAccountRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryTradingAccount encodings.OutboundEncoding request
        TraderNativeInterop.reqQryTradingAccount (this.Handle, &native, requestId)

    member this.ReqQryInvestorPosition(request: QueryInvestorPositionRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryInvestorPosition encodings.OutboundEncoding request
        TraderNativeInterop.reqQryInvestorPosition (this.Handle, &native, requestId)

    member this.ReqQryInstrumentMarginRate(request: QueryInstrumentMarginRateRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryInstrumentMarginRate encodings.OutboundEncoding request
        TraderNativeInterop.reqQryInstrumentMarginRate (this.Handle, &native, requestId)

    member this.ReqQryExchangeMarginRate(request: QueryExchangeMarginRateRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryExchangeMarginRate encodings.OutboundEncoding request
        TraderNativeInterop.reqQryExchangeMarginRate (this.Handle, &native, requestId)

    member this.ReqQryInstrumentCommissionRate(request: QueryInstrumentCommissionRateRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryInstrumentCommissionRate encodings.OutboundEncoding request
        TraderNativeInterop.reqQryInstrumentCommissionRate (this.Handle, &native, requestId)

    member this.ReqOrderInsert(request: InputOrderRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.inputOrder encodings.OutboundEncoding requestId request
        TraderNativeInterop.reqOrderInsert (this.Handle, &native, requestId)

    member this.ReqOrderAction(request: InputOrderActionRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.inputOrderAction encodings.OutboundEncoding requestId request
        TraderNativeInterop.reqOrderAction (this.Handle, &native, requestId)

    member this.GetTradingDay() =
        TraderNativeInterop.getTradingDay this.Handle |> BridgeHelpers.ptrToAnsiString

    member this.GetFrontInfo() =
        let mutable native = NativeFrontInfo()
        TraderNativeInterop.getFrontInfo (this.Handle, &native)
        |> BridgeHelpers.throwOnNonZero
        <| "ctp_trader_get_front_info"
        TraderBridgeMapping.frontInfo encodings.InboundEncoding native

    member this.RegisterNameServer(nsAddress: string) =
        BridgeHelpers.withEncodedCString encodings.OutboundEncoding (Some nsAddress) (fun ptr ->
            TraderNativeInterop.registerNameServer (this.Handle, ptr))
        |> BridgeHelpers.throwOnNonZero
        <| "ctp_trader_register_name_server"

    member this.RegisterFensUserInfo(request: FensUserInfo) =
        let mutable native = TraderBridgeBuilders.fensUserInfo encodings.OutboundEncoding request
        TraderNativeInterop.registerFensUserInfo (this.Handle, &native)

    member this.RegisterUserSystemInfo(request: UserSystemInfo) =
        let mutable native = TraderBridgeBuilders.userSystemInfo encodings.OutboundEncoding request
        TraderNativeInterop.registerUserSystemInfo (this.Handle, &native)

    member this.SubmitUserSystemInfo(request: UserSystemInfo) =
        let mutable native = TraderBridgeBuilders.userSystemInfo encodings.OutboundEncoding request
        TraderNativeInterop.submitUserSystemInfo (this.Handle, &native)

    member this.RegisterWechatUserSystemInfo(request: WechatUserSystemInfo) =
        let mutable native = TraderBridgeBuilders.wechatUserSystemInfo encodings.OutboundEncoding request
        TraderNativeInterop.registerWechatUserSystemInfo (this.Handle, &native)

    member this.SubmitWechatUserSystemInfo(request: WechatUserSystemInfo) =
        let mutable native = TraderBridgeBuilders.wechatUserSystemInfo encodings.OutboundEncoding request
        TraderNativeInterop.submitWechatUserSystemInfo (this.Handle, &native)

    member this.ReqUserPasswordUpdate(request: UserPasswordUpdate, requestId: int) =
        let mutable native = TraderBridgeBuilders.userPasswordUpdate encodings.OutboundEncoding request
        TraderNativeInterop.reqUserPasswordUpdate (this.Handle, &native, requestId)

    member this.ReqTradingAccountPasswordUpdate(request: TradingAccountPasswordUpdate, requestId: int) =
        let mutable native = TraderBridgeBuilders.tradingAccountPasswordUpdate encodings.OutboundEncoding request
        TraderNativeInterop.reqTradingAccountPasswordUpdate (this.Handle, &native, requestId)

    member this.ReqUserAuthMethod(request: UserAuthMethodRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.reqUserAuthMethod encodings.OutboundEncoding request
        TraderNativeInterop.reqUserAuthMethod (this.Handle, &native, requestId)

    member this.ReqGenUserCaptcha(request: GenUserCaptchaRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.reqGenUserCaptcha encodings.OutboundEncoding request
        TraderNativeInterop.reqGenUserCaptcha (this.Handle, &native, requestId)

    member this.ReqGenUserText(request: GenUserTextRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.reqGenUserText encodings.OutboundEncoding request
        TraderNativeInterop.reqGenUserText (this.Handle, &native, requestId)

    member this.ReqUserLoginWithCaptcha(request: UserLoginWithCaptchaRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.reqUserLoginWithCaptcha encodings.OutboundEncoding request
        TraderNativeInterop.reqUserLoginWithCaptcha (this.Handle, &native, requestId)

    member this.ReqUserLoginWithText(request: UserLoginWithTextRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.reqUserLoginWithText encodings.OutboundEncoding request
        TraderNativeInterop.reqUserLoginWithText (this.Handle, &native, requestId)

    member this.ReqUserLoginWithOtp(request: UserLoginWithOtpRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.reqUserLoginWithOtp encodings.OutboundEncoding request
        TraderNativeInterop.reqUserLoginWithOtp (this.Handle, &native, requestId)

    member this.ReqGenSmsCode(request: GenSmsCodeRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.reqGenSmsCode encodings.OutboundEncoding request
        TraderNativeInterop.reqGenSmsCode (this.Handle, &native, requestId)

    member this.ReqParkedOrderInsert(request: ParkedOrder, requestId: int) =
        let mutable native = TraderBridgeBuilders.parkedOrder encodings.OutboundEncoding requestId request
        TraderNativeInterop.reqParkedOrderInsert (this.Handle, &native, requestId)

    member this.ReqParkedOrderAction(request: ParkedOrderAction, requestId: int) =
        let mutable native = TraderBridgeBuilders.parkedOrderAction encodings.OutboundEncoding requestId request
        TraderNativeInterop.reqParkedOrderAction (this.Handle, &native, requestId)

    member this.ReqQryMaxOrderVolume(request: QryMaxOrderVolumeRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryMaxOrderVolume encodings.OutboundEncoding request
        TraderNativeInterop.reqQryMaxOrderVolume (this.Handle, &native, requestId)

    member this.ReqRemoveParkedOrder(request: RemoveParkedOrder, requestId: int) =
        let mutable native = TraderBridgeBuilders.removeParkedOrder encodings.OutboundEncoding request
        TraderNativeInterop.reqRemoveParkedOrder (this.Handle, &native, requestId)

    member this.ReqRemoveParkedOrderAction(request: RemoveParkedOrderAction, requestId: int) =
        let mutable native = TraderBridgeBuilders.removeParkedOrderAction encodings.OutboundEncoding request
        TraderNativeInterop.reqRemoveParkedOrderAction (this.Handle, &native, requestId)

    member this.ReqExecOrderInsert(request: InputExecOrderRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.inputExecOrder encodings.OutboundEncoding requestId request
        TraderNativeInterop.reqExecOrderInsert (this.Handle, &native, requestId)

    member this.ReqExecOrderAction(request: InputExecOrderActionRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.inputExecOrderAction encodings.OutboundEncoding requestId request
        TraderNativeInterop.reqExecOrderAction (this.Handle, &native, requestId)

    member this.ReqForQuoteInsert(request: InputForQuoteRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.inputForQuote encodings.OutboundEncoding request
        TraderNativeInterop.reqForQuoteInsert (this.Handle, &native, requestId)

    member this.ReqQuoteInsert(request: InputQuoteRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.inputQuote encodings.OutboundEncoding requestId request
        TraderNativeInterop.reqQuoteInsert (this.Handle, &native, requestId)

    member this.ReqQuoteAction(request: InputQuoteActionRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.inputQuoteAction encodings.OutboundEncoding requestId request
        TraderNativeInterop.reqQuoteAction (this.Handle, &native, requestId)

    member this.ReqBatchOrderAction(request: InputBatchOrderActionRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.inputBatchOrderAction encodings.OutboundEncoding requestId request
        TraderNativeInterop.reqBatchOrderAction (this.Handle, &native, requestId)

    member this.ReqOptionSelfCloseInsert(request: InputOptionSelfCloseRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.inputOptionSelfClose encodings.OutboundEncoding requestId request
        TraderNativeInterop.reqOptionSelfCloseInsert (this.Handle, &native, requestId)

    member this.ReqOptionSelfCloseAction(request: InputOptionSelfCloseActionRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.inputOptionSelfCloseAction encodings.OutboundEncoding requestId request
        TraderNativeInterop.reqOptionSelfCloseAction (this.Handle, &native, requestId)

    member this.ReqCombActionInsert(request: InputCombActionRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.inputCombAction encodings.OutboundEncoding request
        TraderNativeInterop.reqCombActionInsert (this.Handle, &native, requestId)

    member this.ReqOffsetSetting(request: InputOffsetSettingRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.inputOffsetSetting encodings.OutboundEncoding requestId request
        TraderNativeInterop.reqOffsetSetting (this.Handle, &native, requestId)

    member this.ReqCancelOffsetSetting(request: InputOffsetSettingRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.inputOffsetSetting encodings.OutboundEncoding requestId request
        TraderNativeInterop.reqCancelOffsetSetting (this.Handle, &native, requestId)

    member this.ReqSpdApply(request: InputSpdApplyRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.inputSpdApply encodings.OutboundEncoding requestId request
        TraderNativeInterop.reqSpdApply (this.Handle, &native, requestId)

    member this.ReqSpdApplyAction(request: InputSpdApplyActionRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.inputSpdApplyAction encodings.OutboundEncoding requestId request
        TraderNativeInterop.reqSpdApplyAction (this.Handle, &native, requestId)

    member this.ReqHedgeCfm(request: InputHedgeCfmRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.inputHedgeCfm encodings.OutboundEncoding requestId request
        TraderNativeInterop.reqHedgeCfm (this.Handle, &native, requestId)

    member this.ReqHedgeCfmAction(request: InputHedgeCfmActionRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.inputHedgeCfmAction encodings.OutboundEncoding requestId request
        TraderNativeInterop.reqHedgeCfmAction (this.Handle, &native, requestId)

    member this.ReqQryOrder(request: QryOrderRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryOrder encodings.OutboundEncoding request
        TraderNativeInterop.reqQryOrder (this.Handle, &native, requestId)

    member this.ReqQryTrade(request: QryTradeRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryTrade encodings.OutboundEncoding request
        TraderNativeInterop.reqQryTrade (this.Handle, &native, requestId)

    member this.ReqQryInvestor(request: QryInvestorRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryInvestor encodings.OutboundEncoding request
        TraderNativeInterop.reqQryInvestor (this.Handle, &native, requestId)

    member this.ReqQryTradingCode(request: QryTradingCodeRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryTradingCode encodings.OutboundEncoding request
        TraderNativeInterop.reqQryTradingCode (this.Handle, &native, requestId)

    member this.ReqQryUserSession(request: QryUserSessionRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryUserSession encodings.OutboundEncoding request
        TraderNativeInterop.reqQryUserSession (this.Handle, &native, requestId)

    member this.ReqQryExchange(request: QryExchangeRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryExchange encodings.OutboundEncoding request
        TraderNativeInterop.reqQryExchange (this.Handle, &native, requestId)

    member this.ReqQryProduct(request: QryProductRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryProduct encodings.OutboundEncoding request
        TraderNativeInterop.reqQryProduct (this.Handle, &native, requestId)

    member this.ReqQryInstrument(request: QryInstrumentRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryInstrument encodings.OutboundEncoding request
        TraderNativeInterop.reqQryInstrument (this.Handle, &native, requestId)

    member this.ReqQryDepthMarketData(request: QryDepthMarketDataRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryDepthMarketData encodings.OutboundEncoding request
        TraderNativeInterop.reqQryDepthMarketData (this.Handle, &native, requestId)

    member this.ReqQryTraderOffer(request: QryTraderOfferRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryTraderOffer encodings.OutboundEncoding request
        TraderNativeInterop.reqQryTraderOffer (this.Handle, &native, requestId)

    member this.ReqQrySettlementInfo(request: QrySettlementInfoRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qrySettlementInfo encodings.OutboundEncoding request
        TraderNativeInterop.reqQrySettlementInfo (this.Handle, &native, requestId)

    member this.ReqQryTransferBank(request: QryTransferBankRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryTransferBank encodings.OutboundEncoding request
        TraderNativeInterop.reqQryTransferBank (this.Handle, &native, requestId)

    member this.ReqQryInvestorPositionDetail(request: QryInvestorPositionDetailRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryInvestorPositionDetail encodings.OutboundEncoding request
        TraderNativeInterop.reqQryInvestorPositionDetail (this.Handle, &native, requestId)

    member this.ReqQryNotice(request: QryNoticeRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryNotice encodings.OutboundEncoding request
        TraderNativeInterop.reqQryNotice (this.Handle, &native, requestId)

    member this.ReqQrySettlementInfoConfirm(request: QrySettlementInfoConfirmRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qrySettlementInfoConfirm encodings.OutboundEncoding request
        TraderNativeInterop.reqQrySettlementInfoConfirm (this.Handle, &native, requestId)

    member this.ReqQryInvestorPositionCombineDetail(request: QryInvestorPositionCombineDetailRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryInvestorPositionCombineDetail encodings.OutboundEncoding request
        TraderNativeInterop.reqQryInvestorPositionCombineDetail (this.Handle, &native, requestId)

    member this.ReqQryCfmmcTradingAccountKey(request: QryCfmmcTradingAccountKeyRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryCfmmcTradingAccountKey encodings.OutboundEncoding request
        TraderNativeInterop.reqQryCfmmcTradingAccountKey (this.Handle, &native, requestId)

    member this.ReqQryEWarrantOffset(request: QryEWarrantOffsetRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryEWarrantOffset encodings.OutboundEncoding request
        TraderNativeInterop.reqQryEWarrantOffset (this.Handle, &native, requestId)

    member this.ReqQryInvestorProductGroupMargin(request: QryInvestorProductGroupMarginRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryInvestorProductGroupMargin encodings.OutboundEncoding request
        TraderNativeInterop.reqQryInvestorProductGroupMargin (this.Handle, &native, requestId)

    member this.ReqQryExchangeMarginRateAdjust(request: QryExchangeMarginRateAdjustRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryExchangeMarginRateAdjust encodings.OutboundEncoding request
        TraderNativeInterop.reqQryExchangeMarginRateAdjust (this.Handle, &native, requestId)

    member this.ReqQryExchangeRate(request: QryExchangeRate, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryExchangeRate encodings.OutboundEncoding request
        TraderNativeInterop.reqQryExchangeRate (this.Handle, &native, requestId)

    member this.ReqQrySecAgentAcIdMap(request: QrySecAgentAcIdMapRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qrySecAgentAcIdMap encodings.OutboundEncoding request
        TraderNativeInterop.reqQrySecAgentAcIdMap (this.Handle, &native, requestId)

    member this.ReqQryProductExchRate(request: QryProductExchRateRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryProductExchRate encodings.OutboundEncoding request
        TraderNativeInterop.reqQryProductExchRate (this.Handle, &native, requestId)

    member this.ReqQryProductGroup(request: QryProductGroupRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryProductGroup encodings.OutboundEncoding request
        TraderNativeInterop.reqQryProductGroup (this.Handle, &native, requestId)

    member this.ReqQryMmInstrumentCommissionRate(request: QryMmInstrumentCommissionRateRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryMmInstrumentCommissionRate encodings.OutboundEncoding request
        TraderNativeInterop.reqQryMmInstrumentCommissionRate (this.Handle, &native, requestId)

    member this.ReqQryMmOptionInstrCommRate(request: QryMmOptionInstrCommRateRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryMmOptionInstrCommRate encodings.OutboundEncoding request
        TraderNativeInterop.reqQryMmOptionInstrCommRate (this.Handle, &native, requestId)

    member this.ReqQryInstrumentOrderCommRate(request: QryInstrumentOrderCommRateRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryInstrumentOrderCommRate encodings.OutboundEncoding request
        TraderNativeInterop.reqQryInstrumentOrderCommRate (this.Handle, &native, requestId)

    member this.ReqQrySecAgentTradingAccount(request: QueryTradingAccountRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryTradingAccount encodings.OutboundEncoding request
        TraderNativeInterop.reqQrySecAgentTradingAccount (this.Handle, &native, requestId)

    member this.ReqQrySecAgentCheckMode(request: QrySecAgentCheckModeRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qrySecAgentCheckMode encodings.OutboundEncoding request
        TraderNativeInterop.reqQrySecAgentCheckMode (this.Handle, &native, requestId)

    member this.ReqQrySecAgentTradeInfo(request: QrySecAgentTradeInfoRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qrySecAgentTradeInfo encodings.OutboundEncoding request
        TraderNativeInterop.reqQrySecAgentTradeInfo (this.Handle, &native, requestId)

    member this.ReqQryOptionInstrTradeCost(request: QryOptionInstrTradeCostRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryOptionInstrTradeCost encodings.OutboundEncoding request
        TraderNativeInterop.reqQryOptionInstrTradeCost (this.Handle, &native, requestId)

    member this.ReqQryOptionInstrCommRate(request: QryOptionInstrCommRateRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryOptionInstrCommRate encodings.OutboundEncoding request
        TraderNativeInterop.reqQryOptionInstrCommRate (this.Handle, &native, requestId)

    member this.ReqQryExecOrder(request: QryExecOrderRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryExecOrder encodings.OutboundEncoding request
        TraderNativeInterop.reqQryExecOrder (this.Handle, &native, requestId)

    member this.ReqQryForQuote(request: QryForQuoteRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryForQuote encodings.OutboundEncoding request
        TraderNativeInterop.reqQryForQuote (this.Handle, &native, requestId)

    member this.ReqQryQuote(request: QryQuoteRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryQuote encodings.OutboundEncoding request
        TraderNativeInterop.reqQryQuote (this.Handle, &native, requestId)

    member this.ReqQryOptionSelfClose(request: QryOptionSelfCloseRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryOptionSelfClose encodings.OutboundEncoding request
        TraderNativeInterop.reqQryOptionSelfClose (this.Handle, &native, requestId)

    member this.ReqQryInvestUnit(request: QryInvestUnitRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryInvestUnit encodings.OutboundEncoding request
        TraderNativeInterop.reqQryInvestUnit (this.Handle, &native, requestId)

    member this.ReqQryCombInstrumentGuard(request: QryCombInstrumentGuardRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryCombInstrumentGuard encodings.OutboundEncoding request
        TraderNativeInterop.reqQryCombInstrumentGuard (this.Handle, &native, requestId)

    member this.ReqQryCombAction(request: QryCombActionRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryCombAction encodings.OutboundEncoding request
        TraderNativeInterop.reqQryCombAction (this.Handle, &native, requestId)

    member this.ReqQryTransferSerial(request: QryTransferSerialRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryTransferSerial encodings.OutboundEncoding request
        TraderNativeInterop.reqQryTransferSerial (this.Handle, &native, requestId)

    member this.ReqQryAccountregister(request: QryAccountregisterRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryAccountregister encodings.OutboundEncoding request
        TraderNativeInterop.reqQryAccountregister (this.Handle, &native, requestId)

    member this.ReqQryContractBank(request: QryContractBankRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryContractBank encodings.OutboundEncoding request
        TraderNativeInterop.reqQryContractBank (this.Handle, &native, requestId)

    member this.ReqQryParkedOrder(request: QryParkedOrderRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryParkedOrder encodings.OutboundEncoding request
        TraderNativeInterop.reqQryParkedOrder (this.Handle, &native, requestId)

    member this.ReqQryParkedOrderAction(request: QryParkedOrderActionRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryParkedOrderAction encodings.OutboundEncoding request
        TraderNativeInterop.reqQryParkedOrderAction (this.Handle, &native, requestId)

    member this.ReqQryTradingNotice(request: QryTradingNoticeRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryTradingNotice encodings.OutboundEncoding request
        TraderNativeInterop.reqQryTradingNotice (this.Handle, &native, requestId)

    member this.ReqQryBrokerTradingParams(request: QryBrokerTradingParamsRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryBrokerTradingParams encodings.OutboundEncoding request
        TraderNativeInterop.reqQryBrokerTradingParams (this.Handle, &native, requestId)

    member this.ReqQryBrokerTradingAlgos(request: QryBrokerTradingAlgosRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryBrokerTradingAlgos encodings.OutboundEncoding request
        TraderNativeInterop.reqQryBrokerTradingAlgos (this.Handle, &native, requestId)

    member this.ReqQueryCfmmcTradingAccountToken(request: QueryCfmmcTradingAccountTokenRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.queryCfmmcTradingAccountToken encodings.OutboundEncoding request
        TraderNativeInterop.reqQueryCfmmcTradingAccountToken (this.Handle, &native, requestId)

    member this.ReqQryClassifiedInstrument(request: QryClassifiedInstrumentRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryClassifiedInstrument encodings.OutboundEncoding request
        TraderNativeInterop.reqQryClassifiedInstrument (this.Handle, &native, requestId)

    member this.ReqQryCombPromotionParam(request: QryCombPromotionParamRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryCombPromotionParam encodings.OutboundEncoding request
        TraderNativeInterop.reqQryCombPromotionParam (this.Handle, &native, requestId)

    member this.ReqQryRiskSettleInvstPosition(request: QryRiskSettleInvstPositionRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryRiskSettleInvstPosition encodings.OutboundEncoding request
        TraderNativeInterop.reqQryRiskSettleInvstPosition (this.Handle, &native, requestId)

    member this.ReqQryRiskSettleProductStatus(request: QryRiskSettleProductStatusRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryRiskSettleProductStatus encodings.OutboundEncoding request
        TraderNativeInterop.reqQryRiskSettleProductStatus (this.Handle, &native, requestId)

    member this.ReqQrySpbmFutureParameter(request: QrySpbmFutureParameterRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qrySpbmFutureParameter encodings.OutboundEncoding request
        TraderNativeInterop.reqQrySpbmFutureParameter (this.Handle, &native, requestId)

    member this.ReqQrySpbmOptionParameter(request: QrySpbmOptionParameterRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qrySpbmOptionParameter encodings.OutboundEncoding request
        TraderNativeInterop.reqQrySpbmOptionParameter (this.Handle, &native, requestId)

    member this.ReqQrySpbmIntraParameter(request: QrySpbmIntraParameterRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qrySpbmIntraParameter encodings.OutboundEncoding request
        TraderNativeInterop.reqQrySpbmIntraParameter (this.Handle, &native, requestId)

    member this.ReqQrySpbmInterParameter(request: QrySpbmInterParameterRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qrySpbmInterParameter encodings.OutboundEncoding request
        TraderNativeInterop.reqQrySpbmInterParameter (this.Handle, &native, requestId)

    member this.ReqQrySpbmPortfDefinition(request: QrySpbmPortfDefinitionRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qrySpbmPortfDefinition encodings.OutboundEncoding request
        TraderNativeInterop.reqQrySpbmPortfDefinition (this.Handle, &native, requestId)

    member this.ReqQrySpbmInvestorPortfDef(request: QrySpbmInvestorPortfDefRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qrySpbmInvestorPortfDef encodings.OutboundEncoding request
        TraderNativeInterop.reqQrySpbmInvestorPortfDef (this.Handle, &native, requestId)

    member this.ReqQryInvestorPortfMarginRatio(request: QryInvestorPortfMarginRatioRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryInvestorPortfMarginRatio encodings.OutboundEncoding request
        TraderNativeInterop.reqQryInvestorPortfMarginRatio (this.Handle, &native, requestId)

    member this.ReqQryInvestorProdSpbmDetail(request: QryInvestorProdSpbmDetailRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryInvestorProdSpbmDetail encodings.OutboundEncoding request
        TraderNativeInterop.reqQryInvestorProdSpbmDetail (this.Handle, &native, requestId)

    member this.ReqQryInvestorCommoditySpmmMargin(request: QryInvestorCommoditySpmmMarginRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryInvestorCommoditySpmmMargin encodings.OutboundEncoding request
        TraderNativeInterop.reqQryInvestorCommoditySpmmMargin (this.Handle, &native, requestId)

    member this.ReqQryInvestorCommodityGroupSpmmMargin(request: QryInvestorCommodityGroupSpmmMarginRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryInvestorCommodityGroupSpmmMargin encodings.OutboundEncoding request
        TraderNativeInterop.reqQryInvestorCommodityGroupSpmmMargin (this.Handle, &native, requestId)

    member this.ReqQrySpmmInstParam(request: QrySpmmInstParamRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qrySpmmInstParam encodings.OutboundEncoding request
        TraderNativeInterop.reqQrySpmmInstParam (this.Handle, &native, requestId)

    member this.ReqQrySpmmProductParam(request: QrySpmmProductParamRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qrySpmmProductParam encodings.OutboundEncoding request
        TraderNativeInterop.reqQrySpmmProductParam (this.Handle, &native, requestId)

    member this.ReqQrySpbmAddOnInterParameter(request: QrySpbmAddOnInterParameterRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qrySpbmAddOnInterParameter encodings.OutboundEncoding request
        TraderNativeInterop.reqQrySpbmAddOnInterParameter (this.Handle, &native, requestId)

    member this.ReqQryRcamsCombProductInfo(request: QryRcamsCombProductInfoRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryRcamsCombProductInfo encodings.OutboundEncoding request
        TraderNativeInterop.reqQryRcamsCombProductInfo (this.Handle, &native, requestId)

    member this.ReqQryRcamsInstrParameter(request: QryRcamsInstrParameterRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryRcamsInstrParameter encodings.OutboundEncoding request
        TraderNativeInterop.reqQryRcamsInstrParameter (this.Handle, &native, requestId)

    member this.ReqQryRcamsIntraParameter(request: QryRcamsIntraParameterRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryRcamsIntraParameter encodings.OutboundEncoding request
        TraderNativeInterop.reqQryRcamsIntraParameter (this.Handle, &native, requestId)

    member this.ReqQryRcamsInterParameter(request: QryRcamsInterParameterRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryRcamsInterParameter encodings.OutboundEncoding request
        TraderNativeInterop.reqQryRcamsInterParameter (this.Handle, &native, requestId)

    member this.ReqQryRcamsShortOptAdjustParam(request: QryRcamsShortOptAdjustParamRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryRcamsShortOptAdjustParam encodings.OutboundEncoding request
        TraderNativeInterop.reqQryRcamsShortOptAdjustParam (this.Handle, &native, requestId)

    member this.ReqQryRcamsInvestorCombPosition(request: QryRcamsInvestorCombPositionRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryRcamsInvestorCombPosition encodings.OutboundEncoding request
        TraderNativeInterop.reqQryRcamsInvestorCombPosition (this.Handle, &native, requestId)

    member this.ReqQryInvestorProdRcamsMargin(request: QryInvestorProdRcamsMarginRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryInvestorProdRcamsMargin encodings.OutboundEncoding request
        TraderNativeInterop.reqQryInvestorProdRcamsMargin (this.Handle, &native, requestId)

    member this.ReqQryRuleInstrParameter(request: QryRuleInstrParameterRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryRuleInstrParameter encodings.OutboundEncoding request
        TraderNativeInterop.reqQryRuleInstrParameter (this.Handle, &native, requestId)

    member this.ReqQryRuleIntraParameter(request: QryRuleIntraParameterRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryRuleIntraParameter encodings.OutboundEncoding request
        TraderNativeInterop.reqQryRuleIntraParameter (this.Handle, &native, requestId)

    member this.ReqQryRuleInterParameter(request: QryRuleInterParameterRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryRuleInterParameter encodings.OutboundEncoding request
        TraderNativeInterop.reqQryRuleInterParameter (this.Handle, &native, requestId)

    member this.ReqQryInvestorProdRuleMargin(request: QryInvestorProdRuleMarginRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryInvestorProdRuleMargin encodings.OutboundEncoding request
        TraderNativeInterop.reqQryInvestorProdRuleMargin (this.Handle, &native, requestId)

    member this.ReqQryInvestorPortfSetting(request: QryInvestorPortfSettingRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryInvestorPortfSetting encodings.OutboundEncoding request
        TraderNativeInterop.reqQryInvestorPortfSetting (this.Handle, &native, requestId)

    member this.ReqQryInvestorInfoCommRec(request: QryInvestorInfoCommRecRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryInvestorInfoCommRec encodings.OutboundEncoding request
        TraderNativeInterop.reqQryInvestorInfoCommRec (this.Handle, &native, requestId)

    member this.ReqQryCombLeg(request: QryCombLeg, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryCombLeg encodings.OutboundEncoding request
        TraderNativeInterop.reqQryCombLeg (this.Handle, &native, requestId)

    member this.ReqQryOffsetSetting(request: QryOffsetSettingRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryOffsetSetting encodings.OutboundEncoding request
        TraderNativeInterop.reqQryOffsetSetting (this.Handle, &native, requestId)

    member this.ReqQrySpdApply(request: QrySpdApplyRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qrySpdApply encodings.OutboundEncoding request
        TraderNativeInterop.reqQrySpdApply (this.Handle, &native, requestId)

    member this.ReqQryHedgeCfm(request: QryHedgeCfmRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryHedgeCfm encodings.OutboundEncoding request
        TraderNativeInterop.reqQryHedgeCfm (this.Handle, &native, requestId)

    member this.ReqFromBankToFutureByFuture(request: TransferRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.reqTransfer encodings.OutboundEncoding requestId request
        TraderNativeInterop.reqFromBankToFutureByFuture (this.Handle, &native, requestId)

    member this.ReqFromFutureToBankByFuture(request: TransferRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.reqTransfer encodings.OutboundEncoding requestId request
        TraderNativeInterop.reqFromFutureToBankByFuture (this.Handle, &native, requestId)

    member this.ReqQueryBankAccountMoneyByFuture(request: ReqQueryAccount, requestId: int) =
        let mutable native = TraderBridgeBuilders.reqQueryAccount encodings.OutboundEncoding requestId request
        TraderNativeInterop.reqQueryBankAccountMoneyByFuture (this.Handle, &native, requestId)

    interface IDisposable with
        member _.Dispose() = handle.Dispose()
