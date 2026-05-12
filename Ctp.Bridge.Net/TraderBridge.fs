namespace Ctp.Bridge.Net

open System
open System.Runtime.InteropServices
open Microsoft.Win32.SafeHandles

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

type TraderCallbacks =
    { FrontConnected: (unit -> unit) option
      FrontDisconnected: (int -> unit) option
      HeartBeatWarning: (int -> unit) option
      RspAuthenticate: (AuthenticateResponse option -> RspInfo option -> int -> bool -> unit) option
      RspSettlementInfoConfirm: (SettlementInfoConfirm option -> RspInfo option -> int -> bool -> unit) option
      RspUserLogin: (UserLoginResponse option -> RspInfo option -> int -> bool -> unit) option
      RspUserLogout: (UserLogoutResponse option -> RspInfo option -> int -> bool -> unit) option
      RspError: (RspInfo option -> int -> bool -> unit) option
      RspQryTradingAccount: (TradingAccount option -> RspInfo option -> int -> bool -> unit) option
      RspQryInvestorPosition: (InvestorPosition option -> RspInfo option -> int -> bool -> unit) option
      RspOrderInsert: (InputOrderRequest option -> RspInfo option -> int -> bool -> unit) option
      RspOrderAction: (InputOrderActionRequest option -> RspInfo option -> int -> bool -> unit) option
      RtnOrder: (OrderUpdate -> unit) option
      RtnTrade: (TradeUpdate -> unit) option }

    static member Empty =
        { FrontConnected = None
          FrontDisconnected = None
          HeartBeatWarning = None
          RspAuthenticate = None
          RspSettlementInfoConfirm = None
          RspUserLogin = None
          RspUserLogout = None
          RspError = None
          RspQryTradingAccount = None
          RspQryInvestorPosition = None
          RspOrderInsert = None
          RspOrderAction = None
          RtnOrder = None
          RtnTrade = None }

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

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderFrontConnectedDelegate = delegate of nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderFrontDisconnectedDelegate = delegate of int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderHeartBeatWarningDelegate = delegate of int * nativeint -> unit

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
type private TraderRspOrderInsertDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRspOrderActionDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRtnOrderDelegate = delegate of nativeint * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private TraderRtnTradeDelegate = delegate of nativeint * nativeint -> unit

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTraderSpi =
    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable OnFrontConnected: TraderFrontConnectedDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable OnFrontDisconnected: TraderFrontDisconnectedDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable OnHeartBeatWarning: TraderHeartBeatWarningDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable OnRspAuthenticate: TraderRspAuthenticateDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable OnRspSettlementInfoConfirm: TraderRspSettlementInfoConfirmDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable OnRspUserLogin: TraderRspUserLoginDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable OnRspUserLogout: TraderRspUserLogoutDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable OnRspError: TraderRspErrorDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable OnRspQryTradingAccount: TraderRspQryTradingAccountDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable OnRspQryInvestorPosition: TraderRspQryInvestorPositionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable OnRspOrderInsert: TraderRspOrderInsertDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable OnRspOrderAction: TraderRspOrderActionDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable OnRtnOrder: TraderRtnOrderDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable OnRtnTrade: TraderRtnTradeDelegate

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

    [<DllImport(Library,
                CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "ctp_trader_req_settlement_info_confirm")>]
    extern int reqSettlementInfoConfirm(nativeint handle, NativeSettlementInfoConfirm& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_user_login")>]
    extern int reqUserLogin(nativeint handle, NativeReqUserLogin& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_user_logout")>]
    extern int reqUserLogout(nativeint handle, NativeUserLogout& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_qry_trading_account")>]
    extern int reqQryTradingAccount(nativeint handle, NativeQryTradingAccount& request, int requestId)

    [<DllImport(Library,
                CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "ctp_trader_req_qry_investor_position")>]
    extern int reqQryInvestorPosition(nativeint handle, NativeQryInvestorPosition& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_order_insert")>]
    extern int reqOrderInsert(nativeint handle, NativeInputOrder& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_trader_req_order_action")>]
    extern int reqOrderAction(nativeint handle, NativeInputOrderAction& request, int requestId)

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

    let onRspAuthenticate =
        TraderRspAuthenticateDelegate(fun authPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspAuthenticate
            |> Option.iter (fun handler ->
                let auth =
                    authPtr
                    |> EncodingHelpers.ptrToOption<NativeRspAuthenticate>
                    |> Option.map (TraderBridgeMapping.authenticate encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler auth rspInfo requestId (isLast <> 0)))

    let onRspSettlementInfoConfirm =
        TraderRspSettlementInfoConfirmDelegate(fun confirmPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspSettlementInfoConfirm
            |> Option.iter (fun handler ->
                let confirm =
                    confirmPtr
                    |> EncodingHelpers.ptrToOption<NativeSettlementInfoConfirm>
                    |> Option.map (TraderBridgeMapping.settlementInfoConfirm encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler confirm rspInfo requestId (isLast <> 0)))

    let onRspUserLogin =
        TraderRspUserLoginDelegate(fun loginPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspUserLogin
            |> Option.iter (fun handler ->
                let login =
                    loginPtr
                    |> EncodingHelpers.ptrToOption<NativeRspUserLogin>
                    |> Option.map (BridgeMapping.userLogin encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler login rspInfo requestId (isLast <> 0)))

    let onRspUserLogout =
        TraderRspUserLogoutDelegate(fun logoutPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspUserLogout
            |> Option.iter (fun handler ->
                let logout =
                    logoutPtr
                    |> EncodingHelpers.ptrToOption<NativeUserLogout>
                    |> Option.map (BridgeMapping.userLogout encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler logout rspInfo requestId (isLast <> 0)))

    let onRspError =
        TraderRspErrorDelegate(fun rspInfoPtr requestId isLast _ ->
            callbacks.RspError
            |> Option.iter (fun handler ->
                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler rspInfo requestId (isLast <> 0)))

    let onRspQryTradingAccount =
        TraderRspQryTradingAccountDelegate(fun accountPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryTradingAccount
            |> Option.iter (fun handler ->
                let account =
                    accountPtr
                    |> EncodingHelpers.ptrToOption<NativeTradingAccount>
                    |> Option.map (TraderBridgeMapping.tradingAccount encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler account rspInfo requestId (isLast <> 0)))

    let onRspQryInvestorPosition =
        TraderRspQryInvestorPositionDelegate(fun positionPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspQryInvestorPosition
            |> Option.iter (fun handler ->
                let position =
                    positionPtr
                    |> EncodingHelpers.ptrToOption<NativeInvestorPosition>
                    |> Option.map (TraderBridgeMapping.investorPosition encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler position rspInfo requestId (isLast <> 0)))

    let onRspOrderInsert =
        TraderRspOrderInsertDelegate(fun orderPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspOrderInsert
            |> Option.iter (fun handler ->
                let order =
                    orderPtr
                    |> EncodingHelpers.ptrToOption<NativeInputOrder>
                    |> Option.map (TraderBridgeMapping.inputOrderRequest encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler order rspInfo requestId (isLast <> 0)))

    let onRspOrderAction =
        TraderRspOrderActionDelegate(fun actionPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspOrderAction
            |> Option.iter (fun handler ->
                let action =
                    actionPtr
                    |> EncodingHelpers.ptrToOption<NativeInputOrderAction>
                    |> Option.map (TraderBridgeMapping.inputOrderActionRequest encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler action rspInfo requestId (isLast <> 0)))

    let onRtnOrder =
        TraderRtnOrderDelegate(fun orderPtr _ ->
            callbacks.RtnOrder
            |> Option.iter (fun handler ->
                match EncodingHelpers.ptrToOption<NativeOrder> orderPtr with
                | Some order -> handler (TraderBridgeMapping.orderUpdate encodings.InboundEncoding order)
                | None -> ()))

    let onRtnTrade =
        TraderRtnTradeDelegate(fun tradePtr _ ->
            callbacks.RtnTrade
            |> Option.iter (fun handler ->
                match EncodingHelpers.ptrToOption<NativeTrade> tradePtr with
                | Some trade -> handler (TraderBridgeMapping.tradeUpdate encodings.InboundEncoding trade)
                | None -> ()))

    let mutable native = NativeTraderSpi()

    do
        native.OnFrontConnected <- onFrontConnected
        native.OnFrontDisconnected <- onFrontDisconnected
        native.OnHeartBeatWarning <- onHeartBeatWarning
        native.OnRspAuthenticate <- onRspAuthenticate
        native.OnRspSettlementInfoConfirm <- onRspSettlementInfoConfirm
        native.OnRspUserLogin <- onRspUserLogin
        native.OnRspUserLogout <- onRspUserLogout
        native.OnRspError <- onRspError
        native.OnRspQryTradingAccount <- onRspQryTradingAccount
        native.OnRspQryInvestorPosition <- onRspQryInvestorPosition
        native.OnRspOrderInsert <- onRspOrderInsert
        native.OnRspOrderAction <- onRspOrderAction
        native.OnRtnOrder <- onRtnOrder
        native.OnRtnTrade <- onRtnTrade

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

    member this.ReqUserLogin(request: RequestUserLogin, requestId: int) =
        let mutable native = BridgeBuilders.reqUserLogin encodings.OutboundEncoding request
        TraderNativeInterop.reqUserLogin (this.Handle, &native, requestId)

    member this.ReqUserLogout(request: RequestUserLogout, requestId: int) =
        let mutable native = BridgeBuilders.reqUserLogout encodings.OutboundEncoding request
        TraderNativeInterop.reqUserLogout (this.Handle, &native, requestId)

    member this.ReqQryTradingAccount(request: QueryTradingAccountRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryTradingAccount encodings.OutboundEncoding request
        TraderNativeInterop.reqQryTradingAccount (this.Handle, &native, requestId)

    member this.ReqQryInvestorPosition(request: QueryInvestorPositionRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.qryInvestorPosition encodings.OutboundEncoding request
        TraderNativeInterop.reqQryInvestorPosition (this.Handle, &native, requestId)

    member this.ReqOrderInsert(request: InputOrderRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.inputOrder encodings.OutboundEncoding requestId request
        TraderNativeInterop.reqOrderInsert (this.Handle, &native, requestId)

    member this.ReqOrderAction(request: InputOrderActionRequest, requestId: int) =
        let mutable native = TraderBridgeBuilders.inputOrderAction encodings.OutboundEncoding requestId request
        TraderNativeInterop.reqOrderAction (this.Handle, &native, requestId)

    interface IDisposable with
        member _.Dispose() = handle.Dispose()
