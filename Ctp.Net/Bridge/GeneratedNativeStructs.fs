namespace Ctp.Net.Next.Bridge

open System.Runtime.InteropServices

// Generated from NativeBridge/include/ctp_bridge.h and the pinned CTP SDK.
// Do not hand-edit this file; run tools/ctp_api.py generate.

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeAccountProperty =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable BankId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BankAccount: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 101)>]
    [<DefaultValue>]
    val mutable OpenName: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 101)>]
    [<DefaultValue>]
    val mutable OpenBank: byte array

    [<DefaultValue>]
    val mutable IsActive: int

    [<DefaultValue>]
    val mutable AccountSourceType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable OpenDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable CancelDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 65)>]
    [<DefaultValue>]
    val mutable OperatorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable OperateDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable OperateTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeAddrAppIdRelation =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)>]
    [<DefaultValue>]
    val mutable Address: byte array

    [<DefaultValue>]
    val mutable DrIdentityId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable AppId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeAppAuthenticationCode =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable AppId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable AuthCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable PreAuthCode: byte array

    [<DefaultValue>]
    val mutable AppType: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeAppIdAuthAssign =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable AppId: byte array

    [<DefaultValue>]
    val mutable DrIdentityId: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeAuthForbiddenIp =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeAuthIp =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable AppId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeAuthUserId =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable AppId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable AuthType: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeAuthenticationInfo =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable UserProductInfo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)>]
    [<DefaultValue>]
    val mutable AuthInfo: byte array

    [<DefaultValue>]
    val mutable IsResult: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable AppId: byte array

    [<DefaultValue>]
    val mutable AppType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable ClientIpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeBroker =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable BrokerAbbr: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable BrokerName: byte array

    [<DefaultValue>]
    val mutable IsActive: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeBrokerDeposit =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<DefaultValue>]
    val mutable PreBalance: float

    [<DefaultValue>]
    val mutable CurrMargin: float

    [<DefaultValue>]
    val mutable CloseProfit: float

    [<DefaultValue>]
    val mutable Balance: float

    [<DefaultValue>]
    val mutable Deposit: float

    [<DefaultValue>]
    val mutable Withdraw: float

    [<DefaultValue>]
    val mutable Available: float

    [<DefaultValue>]
    val mutable Reserve: float

    [<DefaultValue>]
    val mutable FrozenMargin: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeBrokerSync =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeBrokerUser =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable UserName: byte array

    [<DefaultValue>]
    val mutable UserType: byte

    [<DefaultValue>]
    val mutable IsActive: int

    [<DefaultValue>]
    val mutable IsUsingOtp: int

    [<DefaultValue>]
    val mutable IsAuthForce: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeBrokerUserEvent =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable UserEventType: byte

    [<DefaultValue>]
    val mutable EventSequenceNo: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable EventDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable EventTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 1025)>]
    [<DefaultValue>]
    val mutable UserEventInfo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<DefaultValue>]
    val mutable DrIdentityId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeBrokerUserFunction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable BrokerFunctionCode: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeBrokerUserOtpParam =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)>]
    [<DefaultValue>]
    val mutable OtpVendorsId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable SerialNumber: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable AuthKey: byte array

    [<DefaultValue>]
    val mutable LastDrift: int

    [<DefaultValue>]
    val mutable LastSuccess: int

    [<DefaultValue>]
    val mutable OtpType: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeBrokerUserPassword =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Password: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable LastUpdateTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable LastLoginTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExpireDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable WeakExpireDate: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeBrokerUserRightAssign =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<DefaultValue>]
    val mutable DrIdentityId: int

    [<DefaultValue>]
    val mutable Tradeable: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeBrokerWithdrawAlgorithm =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<DefaultValue>]
    val mutable WithdrawAlgorithm: byte

    [<DefaultValue>]
    val mutable UsingRatio: float

    [<DefaultValue>]
    val mutable IncludeCloseProfit: byte

    [<DefaultValue>]
    val mutable AllWithoutTrade: byte

    [<DefaultValue>]
    val mutable AvailIncludeCloseProfit: byte

    [<DefaultValue>]
    val mutable IsBrokerUserEvent: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<DefaultValue>]
    val mutable FundMortgageRatio: float

    [<DefaultValue>]
    val mutable BalanceAlgorithm: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeCfmmcBrokerKey =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable CreateDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable CreateTime: byte array

    [<DefaultValue>]
    val mutable KeyId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable CurrentKey: byte array

    [<DefaultValue>]
    val mutable KeyKind: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeCombinationLeg =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<DefaultValue>]
    val mutable LegId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<DefaultValue>]
    val mutable Direction: byte

    [<DefaultValue>]
    val mutable LegMultiple: int

    [<DefaultValue>]
    val mutable ImplyLevel: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable CombInstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable LegInstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeCommPhase =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<DefaultValue>]
    val mutable CommPhaseNo: int16

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable SystemId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeCommRateModel =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable CommModelId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 161)>]
    [<DefaultValue>]
    val mutable CommModelName: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeCurrDrIdentity =
    [<DefaultValue>]
    val mutable DrIdentityId: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeCurrTransferIdentity =
    [<DefaultValue>]
    val mutable IdentityId: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeCurrentTime =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable CurrDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable CurrTime: byte array

    [<DefaultValue>]
    val mutable CurrMillisec: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ActionDay: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeDepartmentUser =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable InvestorRange: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeDepositResultInform =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)>]
    [<DefaultValue>]
    val mutable DepositSeqNo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable Deposit: float

    [<DefaultValue>]
    val mutable RequestId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)>]
    [<DefaultValue>]
    val mutable ReturnCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)>]
    [<DefaultValue>]
    val mutable DescrInfoForReturnCode: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeDiscount =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<DefaultValue>]
    val mutable InvestorRange: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable Discount: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeDissemination =
    [<DefaultValue>]
    val mutable SequenceSeries: int16

    [<DefaultValue>]
    val mutable SequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeDrTransfer =
    [<DefaultValue>]
    val mutable OrigDrIdentityId: int

    [<DefaultValue>]
    val mutable DestDrIdentityId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable OrigBrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable DestBrokerId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeErrExecOrder =
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

    [<DefaultValue>]
    val mutable ErrorId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ErrorMsg: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeErrExecOrderAction =
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

    [<DefaultValue>]
    val mutable ErrorId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ErrorMsg: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeErrOrder =
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

    [<DefaultValue>]
    val mutable ErrorId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ErrorMsg: byte array

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
type private NativeErrOrderAction =
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

    [<DefaultValue>]
    val mutable ErrorId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ErrorMsg: byte array

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
type private NativeExchangeBatchOrderAction =
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
type private NativeExchangeCombAction =
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
    val mutable Reserve1: byte array

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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable ComTradeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable BranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeExchangeExecOrder =
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
    val mutable Reserve1: byte array

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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable BranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeExchangeExecOrderAction =
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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable BranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<DefaultValue>]
    val mutable Volume: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeExchangeForQuote =
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
    val mutable Reserve1: byte array

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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeExchangeOptionSelfClose =
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
    val mutable Reserve1: byte array

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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable BranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeExchangeOptionSelfCloseAction =
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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable BranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<DefaultValue>]
    val mutable OptSelfCloseFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeExchangeOrder =
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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

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
    val mutable OrderSysId: byte array

    [<DefaultValue>]
    val mutable OrderSource: byte

    [<DefaultValue>]
    val mutable OrderStatus: byte

    [<DefaultValue>]
    val mutable OrderType: byte

    [<DefaultValue>]
    val mutable VolumeTraded: int

    [<DefaultValue>]
    val mutable VolumeTotal: int

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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable ActiveTraderId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClearingPartId: byte array

    [<DefaultValue>]
    val mutable SequenceNo: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable BranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeExchangeOrderAction =
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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable BranchId: byte array

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
type private NativeExchangeOrderActionError =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable OrderSysId: byte array

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

    [<DefaultValue>]
    val mutable ErrorId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ErrorMsg: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeExchangeOrderInsertError =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

    [<DefaultValue>]
    val mutable InstallId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderLocalId: byte array

    [<DefaultValue>]
    val mutable ErrorId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ErrorMsg: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeExchangeQuote =
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
    val mutable Reserve1: byte array

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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable ForQuoteSysId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable BranchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

    [<DefaultValue>]
    val mutable TimeCondition: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeExchangeQuoteAction =
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
type private NativeExchangeSequence =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<DefaultValue>]
    val mutable SequenceNo: int

    [<DefaultValue>]
    val mutable MarketStatus: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeExchangeTrade =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TradeId: byte array

    [<DefaultValue>]
    val mutable Direction: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable OrderSysId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<DefaultValue>]
    val mutable TradingRole: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

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

    [<DefaultValue>]
    val mutable TradeType: byte

    [<DefaultValue>]
    val mutable PriceSource: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable OrderLocalId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClearingPartId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable BusinessUnit: byte array

    [<DefaultValue>]
    val mutable SequenceNo: int

    [<DefaultValue>]
    val mutable TradeSource: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeExitEmergency =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeForQuoteParam =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<DefaultValue>]
    val mutable LastPrice: float

    [<DefaultValue>]
    val mutable PriceInterval: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeForceUserLogout =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeFrontStatus =
    [<DefaultValue>]
    val mutable FrontId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable LastReportDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable LastReportTime: byte array

    [<DefaultValue>]
    val mutable IsActive: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeFutureLimitPosiParam =
    [<DefaultValue>]
    val mutable InvestorRange: byte

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
    val mutable SpecOpenVolume: int

    [<DefaultValue>]
    val mutable ArbiOpenVolume: int

    [<DefaultValue>]
    val mutable OpenVolume: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeFutureSignIo =
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

    [<DefaultValue>]
    val mutable InstallId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)>]
    [<DefaultValue>]
    val mutable Digest: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)>]
    [<DefaultValue>]
    val mutable DeviceId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable BrokerIdByBank: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable OperNo: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable Tid: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeIndexPrice =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<DefaultValue>]
    val mutable ClosePrice: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInstrumentMarginRateAdjust =
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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInstrumentMarginRateUl =
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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInstrumentTradingRight =
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
    val mutable TradingRight: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInvestorAccount =
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
type private NativeInvestorDepartmentFlat =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable DepartmentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInvestorGroup =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorGroupId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable InvestorGroupName: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInvestorInfoCntSetting =
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
    val mutable ProductId: byte array

    [<DefaultValue>]
    val mutable IsCalInfoComm: int

    [<DefaultValue>]
    val mutable IsLimitInfoMax: int

    [<DefaultValue>]
    val mutable InfoMaxLimit: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInvestorPortfMarginModel =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable MarginModelId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInvestorReserveInfo =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 65)>]
    [<DefaultValue>]
    val mutable ReserveInfo: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInvestorTradingRight =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable InvstTradingRight: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeInvestorWithdrawAlgorithm =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<DefaultValue>]
    val mutable InvestorRange: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable UsingRatio: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<DefaultValue>]
    val mutable FundMortgageRatio: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeIpAddrParam =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)>]
    [<DefaultValue>]
    val mutable Address: byte array

    [<DefaultValue>]
    val mutable DrIdentityId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 65)>]
    [<DefaultValue>]
    val mutable DrIdentityName: byte array

    [<DefaultValue>]
    val mutable AddrSrvMode: byte

    [<DefaultValue>]
    val mutable AddrVer: byte

    [<DefaultValue>]
    val mutable AddrNo: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 65)>]
    [<DefaultValue>]
    val mutable AddrName: byte array

    [<DefaultValue>]
    val mutable IsSm: int

    [<DefaultValue>]
    val mutable IsLocalAddr: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 161)>]
    [<DefaultValue>]
    val mutable Remark: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 51)>]
    [<DefaultValue>]
    val mutable Site: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable NetOperator: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 65)>]
    [<DefaultValue>]
    val mutable SysName: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeIpList =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<DefaultValue>]
    val mutable IsWhite: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeLinkMan =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable PersonType: byte

    [<DefaultValue>]
    val mutable IdentifiedCardType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 51)>]
    [<DefaultValue>]
    val mutable IdentifiedCardNo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable PersonName: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Telephone: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 101)>]
    [<DefaultValue>]
    val mutable Address: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)>]
    [<DefaultValue>]
    val mutable ZipCode: byte array

    [<DefaultValue>]
    val mutable Priority: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable UoaZipCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 101)>]
    [<DefaultValue>]
    val mutable PersonFullName: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeLoadSettlementInfo =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeLocalAddrConfig =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)>]
    [<DefaultValue>]
    val mutable PeerAddr: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)>]
    [<DefaultValue>]
    val mutable NetMask: byte array

    [<DefaultValue>]
    val mutable DrIdentityId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)>]
    [<DefaultValue>]
    val mutable LocalAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeLoginForbiddenIp =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeLoginForbiddenUser =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeLoginInfo =
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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable SystemName: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable PasswordDeprecated: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable MaxOrderRef: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ShfeTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable DceTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable CzceTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable FfexTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable OneTimePassword: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable IneTime: byte array

    [<DefaultValue>]
    val mutable IsQryControl: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)>]
    [<DefaultValue>]
    val mutable LoginRemark: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Password: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeLogoutAll =
    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable SystemName: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeManualSyncBrokerUserOtp =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable OtpType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable FirstOtp: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable SecondOtp: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeMarginModel =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable MarginModelId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 161)>]
    [<DefaultValue>]
    val mutable MarginModelName: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeMarketData =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ActionDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeMarketDataAsk23 =
    [<DefaultValue>]
    val mutable AskPrice2: float

    [<DefaultValue>]
    val mutable AskVolume2: int

    [<DefaultValue>]
    val mutable AskPrice3: float

    [<DefaultValue>]
    val mutable AskVolume3: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeMarketDataAsk45 =
    [<DefaultValue>]
    val mutable AskPrice4: float

    [<DefaultValue>]
    val mutable AskVolume4: int

    [<DefaultValue>]
    val mutable AskPrice5: float

    [<DefaultValue>]
    val mutable AskVolume5: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeMarketDataAveragePrice =
    [<DefaultValue>]
    val mutable AveragePrice: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeMarketDataBandingPrice =
    [<DefaultValue>]
    val mutable BandingUpperPrice: float

    [<DefaultValue>]
    val mutable BandingLowerPrice: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeMarketDataBase =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<DefaultValue>]
    val mutable PreSettlementPrice: float

    [<DefaultValue>]
    val mutable PreClosePrice: float

    [<DefaultValue>]
    val mutable PreOpenInterest: float

    [<DefaultValue>]
    val mutable PreDelta: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeMarketDataBestPrice =
    [<DefaultValue>]
    val mutable BidPrice1: float

    [<DefaultValue>]
    val mutable BidVolume1: int

    [<DefaultValue>]
    val mutable AskPrice1: float

    [<DefaultValue>]
    val mutable AskVolume1: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeMarketDataBid23 =
    [<DefaultValue>]
    val mutable BidPrice2: float

    [<DefaultValue>]
    val mutable BidVolume2: int

    [<DefaultValue>]
    val mutable BidPrice3: float

    [<DefaultValue>]
    val mutable BidVolume3: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeMarketDataBid45 =
    [<DefaultValue>]
    val mutable BidPrice4: float

    [<DefaultValue>]
    val mutable BidVolume4: int

    [<DefaultValue>]
    val mutable BidPrice5: float

    [<DefaultValue>]
    val mutable BidVolume5: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeMarketDataExchange =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeMarketDataLastMatch =
    [<DefaultValue>]
    val mutable LastPrice: float

    [<DefaultValue>]
    val mutable Volume: int

    [<DefaultValue>]
    val mutable Turnover: float

    [<DefaultValue>]
    val mutable OpenInterest: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeMarketDataStatic =
    [<DefaultValue>]
    val mutable OpenPrice: float

    [<DefaultValue>]
    val mutable HighestPrice: float

    [<DefaultValue>]
    val mutable LowestPrice: float

    [<DefaultValue>]
    val mutable ClosePrice: float

    [<DefaultValue>]
    val mutable UpperLimitPrice: float

    [<DefaultValue>]
    val mutable LowerLimitPrice: float

    [<DefaultValue>]
    val mutable SettlementPrice: float

    [<DefaultValue>]
    val mutable CurrDelta: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeMarketDataUpdateTime =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable UpdateTime: byte array

    [<DefaultValue>]
    val mutable UpdateMillisec: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ActionDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeMdTraderOffer =
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
type private NativeMortgageParam =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<DefaultValue>]
    val mutable MortgageBalance: float

    [<DefaultValue>]
    val mutable CheckMortgageRatio: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeNotifyFutureSignIn =
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

    [<DefaultValue>]
    val mutable InstallId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)>]
    [<DefaultValue>]
    val mutable Digest: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)>]
    [<DefaultValue>]
    val mutable DeviceId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable BrokerIdByBank: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable OperNo: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable Tid: int

    [<DefaultValue>]
    val mutable ErrorId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ErrorMsg: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)>]
    [<DefaultValue>]
    val mutable PinKey: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)>]
    [<DefaultValue>]
    val mutable MacKey: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeNotifyFutureSignOut =
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

    [<DefaultValue>]
    val mutable InstallId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)>]
    [<DefaultValue>]
    val mutable Digest: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)>]
    [<DefaultValue>]
    val mutable DeviceId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable BrokerIdByBank: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable OperNo: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable Tid: int

    [<DefaultValue>]
    val mutable ErrorId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ErrorMsg: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeNotifyQueryFutureAccountBySec =
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
    val mutable Tid: int

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

    [<DefaultValue>]
    val mutable DrIdentityId: int

    [<DefaultValue>]
    val mutable SecFutureSerial: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeNotifySyncKey =
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

    [<DefaultValue>]
    val mutable InstallId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)>]
    [<DefaultValue>]
    val mutable Message: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)>]
    [<DefaultValue>]
    val mutable DeviceId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable BrokerIdByBank: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable OperNo: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable Tid: int

    [<DefaultValue>]
    val mutable ErrorId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ErrorMsg: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeOptionInstrDelta =
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
    val mutable Delta: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeOptionInstrMarginAdjust =
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
    val mutable SShortMarginRatioByMoney: float

    [<DefaultValue>]
    val mutable SShortMarginRatioByVolume: float

    [<DefaultValue>]
    val mutable HShortMarginRatioByMoney: float

    [<DefaultValue>]
    val mutable HShortMarginRatioByVolume: float

    [<DefaultValue>]
    val mutable AShortMarginRatioByMoney: float

    [<DefaultValue>]
    val mutable AShortMarginRatioByVolume: float

    [<DefaultValue>]
    val mutable IsRelative: int

    [<DefaultValue>]
    val mutable MShortMarginRatioByMoney: float

    [<DefaultValue>]
    val mutable MShortMarginRatioByVolume: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeOptionInstrMiniMargin =
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
    val mutable MinMargin: float

    [<DefaultValue>]
    val mutable ValueMethod: byte

    [<DefaultValue>]
    val mutable IsRelative: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeOptionInstrTradingRight =
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
    val mutable Direction: byte

    [<DefaultValue>]
    val mutable TradingRight: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativePartBroker =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<DefaultValue>]
    val mutable IsActive: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativePortfTradeParamSetting =
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
    val mutable Portfolio: byte

    [<DefaultValue>]
    val mutable IsActionVerify: int

    [<DefaultValue>]
    val mutable IsCloseVerify: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativePositionProfitAlgorithm =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<DefaultValue>]
    val mutable Algorithm: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 161)>]
    [<DefaultValue>]
    val mutable Memo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryAddrAppIdRelation =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryAuthForbiddenIp =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryBatchOrderAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryBroker =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryBrokerUser =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryBrokerUserEvent =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable UserEventType: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryBrokerUserFunction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryBulletin =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<DefaultValue>]
    val mutable BulletinId: int

    [<DefaultValue>]
    val mutable SequenceNo: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)>]
    [<DefaultValue>]
    val mutable NewsType: byte array

    [<DefaultValue>]
    val mutable NewsUrgency: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryCfmmcBrokerKey =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryCombinationLeg =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<DefaultValue>]
    val mutable LegId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable CombInstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable LegInstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryCommRateModel =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable CommModelId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryCurrDrIdentity =
    [<DefaultValue>]
    val mutable DrIdentityId: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryDepartmentUser =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryErrExecOrder =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryErrExecOrderAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryErrOrder =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryErrOrderAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryExchangeCombAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryExchangeExecOrder =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryExchangeExecOrderAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryExchangeForQuote =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryExchangeOrder =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryExchangeOrderAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryExchangeQuote =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryExchangeQuoteAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ClientId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryExchangeSequence =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryExecOrderAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryForQuoteParam =
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
type private NativeQryFrontStatus =
    [<DefaultValue>]
    val mutable FrontId: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryHisOrder =
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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<DefaultValue>]
    val mutable SettlementId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryInstrumentStatus =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryInstrumentTradingRight =
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
type private NativeQryInvestorDepartmentFlat =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryInvestorGroup =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryIpAddrParam =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryIpList =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryLinkMan =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryLocalAddrConfig =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryLoginForbiddenIp =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryLoginForbiddenUser =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryMarginModel =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable MarginModelId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryMaxOrderVolumeWithPrice =
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

    [<DefaultValue>]
    val mutable Price: float

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
type private NativeQryMdTraderOffer =
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
type private NativeQryOptionInstrTradingRight =
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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryOptionSelfCloseAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryOrderAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryPartBroker =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryQuoteAction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryStrikeOffset =
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
type private NativeQrySuperUser =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQrySuperUserFunction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQrySyncDelaySwap =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)>]
    [<DefaultValue>]
    val mutable DelaySwapSeqNo: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQrySyncDeposit =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)>]
    [<DefaultValue>]
    val mutable DepositSeqNo: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQrySyncFundMortgage =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)>]
    [<DefaultValue>]
    val mutable MortgageSeqNo: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQrySyncStatus =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryTgIpAddrParam =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable AppId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryThostUserFunction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryTrader =
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
type private NativeQryTraderAssign =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQryUserRightsAssign =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQueryBrokerDeposit =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeQueryFreq =
    [<DefaultValue>]
    val mutable QueryFreq: int

    [<DefaultValue>]
    val mutable FtdPkgFreq: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeReqApiHandshake =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)>]
    [<DefaultValue>]
    val mutable CryptoKeyVersion: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeReqCancelAccount =
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
    val mutable Gender: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable CountryCode: byte array

    [<DefaultValue>]
    val mutable CustType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 101)>]
    [<DefaultValue>]
    val mutable Address: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)>]
    [<DefaultValue>]
    val mutable ZipCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Telephone: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MobilePhone: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Fax: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable EMail: byte array

    [<DefaultValue>]
    val mutable MoneyAccountStatus: byte

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
    val mutable VerifyCertNoFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<DefaultValue>]
    val mutable CashExchangeCode: byte

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
    val mutable Tid: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 161)>]
    [<DefaultValue>]
    val mutable LongCustomerName: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeReqChangeAccount =
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
    val mutable Gender: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable CountryCode: byte array

    [<DefaultValue>]
    val mutable CustType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 101)>]
    [<DefaultValue>]
    val mutable Address: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)>]
    [<DefaultValue>]
    val mutable ZipCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Telephone: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MobilePhone: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Fax: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable EMail: byte array

    [<DefaultValue>]
    val mutable MoneyAccountStatus: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BankAccount: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BankPassWord: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable NewBankAccount: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable NewBankPassWord: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Password: byte array

    [<DefaultValue>]
    val mutable BankAccType: byte

    [<DefaultValue>]
    val mutable InstallId: int

    [<DefaultValue>]
    val mutable VerifyCertNoFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable BrokerIdByBank: byte array

    [<DefaultValue>]
    val mutable BankPwdFlag: byte

    [<DefaultValue>]
    val mutable SecuPwdFlag: byte

    [<DefaultValue>]
    val mutable Tid: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)>]
    [<DefaultValue>]
    val mutable Digest: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 161)>]
    [<DefaultValue>]
    val mutable LongCustomerName: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeReqDayEndFileReady =
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

    [<DefaultValue>]
    val mutable FileBusinessCode: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)>]
    [<DefaultValue>]
    val mutable Digest: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeReqFutureSignOut =
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

    [<DefaultValue>]
    val mutable InstallId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)>]
    [<DefaultValue>]
    val mutable Digest: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)>]
    [<DefaultValue>]
    val mutable DeviceId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable BrokerIdByBank: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable OperNo: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable Tid: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeReqOpenAccount =
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
    val mutable Gender: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable CountryCode: byte array

    [<DefaultValue>]
    val mutable CustType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 101)>]
    [<DefaultValue>]
    val mutable Address: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)>]
    [<DefaultValue>]
    val mutable ZipCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Telephone: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MobilePhone: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Fax: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable EMail: byte array

    [<DefaultValue>]
    val mutable MoneyAccountStatus: byte

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
    val mutable VerifyCertNoFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<DefaultValue>]
    val mutable CashExchangeCode: byte

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
    val mutable Tid: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 161)>]
    [<DefaultValue>]
    val mutable LongCustomerName: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeReqQueryBankAccountBySec =
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
    val mutable Tid: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 161)>]
    [<DefaultValue>]
    val mutable LongCustomerName: byte array

    [<DefaultValue>]
    val mutable DrIdentityId: int

    [<DefaultValue>]
    val mutable SecFutureSerial: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeReqQueryTradeResultBySerial =
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

    [<DefaultValue>]
    val mutable Reference: int

    [<DefaultValue>]
    val mutable RefrenceIssureType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)>]
    [<DefaultValue>]
    val mutable RefrenceIssure: byte array

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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<DefaultValue>]
    val mutable TradeAmount: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)>]
    [<DefaultValue>]
    val mutable Digest: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 161)>]
    [<DefaultValue>]
    val mutable LongCustomerName: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeReqSyncKey =
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

    [<DefaultValue>]
    val mutable InstallId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)>]
    [<DefaultValue>]
    val mutable Message: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)>]
    [<DefaultValue>]
    val mutable DeviceId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable BrokerIdByBank: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable OperNo: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable Tid: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeReqTransferBySec =
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
    val mutable Tid: int

    [<DefaultValue>]
    val mutable TransferStatus: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 161)>]
    [<DefaultValue>]
    val mutable LongCustomerName: byte array

    [<DefaultValue>]
    val mutable DrIdentityId: int

    [<DefaultValue>]
    val mutable SecFutureSerial: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeReqUserLoginSm =
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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable OneTimePassword: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)>]
    [<DefaultValue>]
    val mutable LoginRemark: byte array

    [<DefaultValue>]
    val mutable ClientIpPort: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable ClientIpAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable SmsCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable BrokerName: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable AuthCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable AppId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Pin: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeReqVerifyApiKey =
    [<DefaultValue>]
    val mutable ApiHandshakeDataLen: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 301)>]
    [<DefaultValue>]
    val mutable ApiHandshakeData: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeReserveOpenAccount =
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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 161)>]
    [<DefaultValue>]
    val mutable CustomerName: byte array

    [<DefaultValue>]
    val mutable IdCardType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 51)>]
    [<DefaultValue>]
    val mutable IdentifiedCardNo: byte array

    [<DefaultValue>]
    val mutable Gender: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable CountryCode: byte array

    [<DefaultValue>]
    val mutable CustType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 101)>]
    [<DefaultValue>]
    val mutable Address: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)>]
    [<DefaultValue>]
    val mutable ZipCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Telephone: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MobilePhone: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Fax: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable EMail: byte array

    [<DefaultValue>]
    val mutable MoneyAccountStatus: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BankAccount: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BankPassWord: byte array

    [<DefaultValue>]
    val mutable InstallId: int

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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable BrokerIdByBank: byte array

    [<DefaultValue>]
    val mutable Tid: int

    [<DefaultValue>]
    val mutable ReserveOpenAccStas: byte

    [<DefaultValue>]
    val mutable ErrorId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ErrorMsg: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeReserveOpenAccountConfirm =
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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 161)>]
    [<DefaultValue>]
    val mutable CustomerName: byte array

    [<DefaultValue>]
    val mutable IdCardType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 51)>]
    [<DefaultValue>]
    val mutable IdentifiedCardNo: byte array

    [<DefaultValue>]
    val mutable Gender: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable CountryCode: byte array

    [<DefaultValue>]
    val mutable CustType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 101)>]
    [<DefaultValue>]
    val mutable Address: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)>]
    [<DefaultValue>]
    val mutable ZipCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Telephone: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MobilePhone: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Fax: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable EMail: byte array

    [<DefaultValue>]
    val mutable MoneyAccountStatus: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BankAccount: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BankPassWord: byte array

    [<DefaultValue>]
    val mutable InstallId: int

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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable BrokerIdByBank: byte array

    [<DefaultValue>]
    val mutable Tid: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Password: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable BankReserveOpenSeq: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable BookDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BookPsw: byte array

    [<DefaultValue>]
    val mutable ErrorId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ErrorMsg: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeReturnResult =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)>]
    [<DefaultValue>]
    val mutable ReturnCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)>]
    [<DefaultValue>]
    val mutable DescrInfoForReturnCode: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRiskForbiddenRight =
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

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRspApiHandshake =
    [<DefaultValue>]
    val mutable FrontHandshakeDataLen: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 301)>]
    [<DefaultValue>]
    val mutable FrontHandshakeData: byte array

    [<DefaultValue>]
    val mutable IsApiAuthEnabled: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRspFutureSignIn =
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

    [<DefaultValue>]
    val mutable InstallId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)>]
    [<DefaultValue>]
    val mutable Digest: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)>]
    [<DefaultValue>]
    val mutable DeviceId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable BrokerIdByBank: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable OperNo: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable Tid: int

    [<DefaultValue>]
    val mutable ErrorId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ErrorMsg: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)>]
    [<DefaultValue>]
    val mutable PinKey: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)>]
    [<DefaultValue>]
    val mutable MacKey: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRspFutureSignOut =
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

    [<DefaultValue>]
    val mutable InstallId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)>]
    [<DefaultValue>]
    val mutable Digest: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)>]
    [<DefaultValue>]
    val mutable DeviceId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable BrokerIdByBank: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable OperNo: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable Tid: int

    [<DefaultValue>]
    val mutable ErrorId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ErrorMsg: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRspQueryAccount =
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
    val mutable Tid: int

    [<DefaultValue>]
    val mutable BankUseAmount: float

    [<DefaultValue>]
    val mutable BankFetchAmount: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 161)>]
    [<DefaultValue>]
    val mutable LongCustomerName: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRspQueryBankAccountBySec =
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
    val mutable Tid: int

    [<DefaultValue>]
    val mutable BankUseAmount: float

    [<DefaultValue>]
    val mutable BankFetchAmount: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 161)>]
    [<DefaultValue>]
    val mutable LongCustomerName: byte array

    [<DefaultValue>]
    val mutable DrIdentityId: int

    [<DefaultValue>]
    val mutable SecFutureSerial: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRspQueryTradeResultBySerial =
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

    [<DefaultValue>]
    val mutable ErrorId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ErrorMsg: byte array

    [<DefaultValue>]
    val mutable Reference: int

    [<DefaultValue>]
    val mutable RefrenceIssureType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)>]
    [<DefaultValue>]
    val mutable RefrenceIssure: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)>]
    [<DefaultValue>]
    val mutable OriginReturnCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)>]
    [<DefaultValue>]
    val mutable OriginDescrInfoForReturnCode: byte array

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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<DefaultValue>]
    val mutable TradeAmount: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)>]
    [<DefaultValue>]
    val mutable Digest: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRspSyncKey =
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

    [<DefaultValue>]
    val mutable InstallId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)>]
    [<DefaultValue>]
    val mutable Message: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)>]
    [<DefaultValue>]
    val mutable DeviceId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable BrokerIdByBank: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable OperNo: byte array

    [<DefaultValue>]
    val mutable RequestId: int

    [<DefaultValue>]
    val mutable Tid: int

    [<DefaultValue>]
    val mutable ErrorId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ErrorMsg: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRspTransferBySec =
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
    val mutable Tid: int

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

    [<DefaultValue>]
    val mutable DrIdentityId: int

    [<DefaultValue>]
    val mutable SecFutureSerial: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeRspUserLogin2 =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable LoginTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable SystemName: byte array

    [<DefaultValue>]
    val mutable FrontId: int

    [<DefaultValue>]
    val mutable SessionId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable MaxOrderRef: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ShfeTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable DceTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable CzceTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable FfexTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable IneTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable RandomString: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSettlementInfoConfirmFromSec =
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
    val mutable FromSec: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSettlementRef =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<DefaultValue>]
    val mutable SettlementId: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSmsVerifyConfig =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable Mobile: byte array

    [<DefaultValue>]
    val mutable UseSmsVerify: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSmsVerifyInfo =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable CreateTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable Mobile: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)>]
    [<DefaultValue>]
    val mutable SmsContent: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSmsVerifyInfoFromSec =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable BrokerAbbr: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable Mobile: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable SmsCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable CreateDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable CreateTime: byte array

    [<DefaultValue>]
    val mutable IsUsed: int

    [<DefaultValue>]
    val mutable FromSec: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeStrikeOffset =
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
    val mutable Offset: float

    [<DefaultValue>]
    val mutable OffsetType: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSuperUser =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable UserName: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Password: byte array

    [<DefaultValue>]
    val mutable IsActive: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSuperUserFunction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable FunctionCode: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDelaySwap =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)>]
    [<DefaultValue>]
    val mutable DelaySwapSeqNo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable FromCurrencyId: byte array

    [<DefaultValue>]
    val mutable FromAmount: float

    [<DefaultValue>]
    val mutable FromFrozenSwap: float

    [<DefaultValue>]
    val mutable FromRemainSwap: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable ToCurrencyId: byte array

    [<DefaultValue>]
    val mutable ToAmount: float

    [<DefaultValue>]
    val mutable IsManualSwap: int

    [<DefaultValue>]
    val mutable IsAllRemainSetZero: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDelaySwapFrozen =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)>]
    [<DefaultValue>]
    val mutable DelaySwapSeqNo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable FromCurrencyId: byte array

    [<DefaultValue>]
    val mutable FromRemainSwap: float

    [<DefaultValue>]
    val mutable IsManualSwap: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaDceCombInstrument =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable CombInstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

    [<DefaultValue>]
    val mutable TradeGroupId: int

    [<DefaultValue>]
    val mutable CombHedgeFlag: byte

    [<DefaultValue>]
    val mutable CombinationType: byte

    [<DefaultValue>]
    val mutable Direction: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

    [<DefaultValue>]
    val mutable Xparameter: float

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaDepthMarketData =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

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

    [<DefaultValue>]
    val mutable BandingUpperPrice: float

    [<DefaultValue>]
    val mutable BandingLowerPrice: float

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaEWarrantOffset =
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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<DefaultValue>]
    val mutable Direction: byte

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<DefaultValue>]
    val mutable Volume: int

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaExchMarginRate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

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
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaIndexPrice =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<DefaultValue>]
    val mutable ClosePrice: float

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaInfo =
    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

    [<DefaultValue>]
    val mutable SyncDeltaStatus: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 257)>]
    [<DefaultValue>]
    val mutable SyncDescription: byte array

    [<DefaultValue>]
    val mutable IsOnlyTrdDelta: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaInitInvstMargin =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable LastRiskTotalInvstMargin: float

    [<DefaultValue>]
    val mutable LastRiskTotalExchMargin: float

    [<DefaultValue>]
    val mutable ThisSyncInvstMargin: float

    [<DefaultValue>]
    val mutable ThisSyncExchMargin: float

    [<DefaultValue>]
    val mutable RemainRiskInvstMargin: float

    [<DefaultValue>]
    val mutable RemainRiskExchMargin: float

    [<DefaultValue>]
    val mutable LastRiskSpecTotalInvstMargin: float

    [<DefaultValue>]
    val mutable LastRiskSpecTotalExchMargin: float

    [<DefaultValue>]
    val mutable ThisSyncSpecInvstMargin: float

    [<DefaultValue>]
    val mutable ThisSyncSpecExchMargin: float

    [<DefaultValue>]
    val mutable RemainRiskSpecInvstMargin: float

    [<DefaultValue>]
    val mutable RemainRiskSpecExchMargin: float

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaInvestorSpmmModel =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable SpmmModelId: byte array

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaInvstCommRate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

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
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaInvstMarginRate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

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

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaInvstMarginRateUl =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

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
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaInvstPosCombDtl =
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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

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

    [<DefaultValue>]
    val mutable TradeGroupId: int

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaInvstPosDtl =
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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable CombInstrumentId: byte array

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

    [<DefaultValue>]
    val mutable SpecPosiType: byte

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaOptExchMargin =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<DefaultValue>]
    val mutable SShortMarginRatioByMoney: float

    [<DefaultValue>]
    val mutable SShortMarginRatioByVolume: float

    [<DefaultValue>]
    val mutable HShortMarginRatioByMoney: float

    [<DefaultValue>]
    val mutable HShortMarginRatioByVolume: float

    [<DefaultValue>]
    val mutable AShortMarginRatioByMoney: float

    [<DefaultValue>]
    val mutable AShortMarginRatioByVolume: float

    [<DefaultValue>]
    val mutable MShortMarginRatioByMoney: float

    [<DefaultValue>]
    val mutable MShortMarginRatioByVolume: float

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaOptInvstCommRate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

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

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaOptInvstMargin =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

    [<DefaultValue>]
    val mutable InvestorRange: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable SShortMarginRatioByMoney: float

    [<DefaultValue>]
    val mutable SShortMarginRatioByVolume: float

    [<DefaultValue>]
    val mutable HShortMarginRatioByMoney: float

    [<DefaultValue>]
    val mutable HShortMarginRatioByVolume: float

    [<DefaultValue>]
    val mutable AShortMarginRatioByMoney: float

    [<DefaultValue>]
    val mutable AShortMarginRatioByVolume: float

    [<DefaultValue>]
    val mutable IsRelative: int

    [<DefaultValue>]
    val mutable MShortMarginRatioByMoney: float

    [<DefaultValue>]
    val mutable MShortMarginRatioByVolume: float

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaProductExchRate =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable QuoteCurrencyId: byte array

    [<DefaultValue>]
    val mutable ExchangeRate: float

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaProductStatus =
    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ProductId: byte array

    [<DefaultValue>]
    val mutable ProductStatus: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaRcamsCombProdInfo =
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

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaRcamsCombRuleDtl =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable ProdGroup: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 51)>]
    [<DefaultValue>]
    val mutable RuleId: byte array

    [<DefaultValue>]
    val mutable Priority: int

    [<DefaultValue>]
    val mutable HedgeFlag: byte

    [<DefaultValue>]
    val mutable CombMargin: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ExchangeInstId: byte array

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
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaRcamsInstrParameter =
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

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaRcamsInterParameter =
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

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaRcamsIntraParameter =
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

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaRcamsInvstCombPos =
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

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaRcamssOptAdjParam =
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

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaRuleInstrParameter =
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

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaRuleInterParameter =
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

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaRuleIntraParameter =
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

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaSpbmAddOnInterParam =
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

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaSpbmFutureParameter =
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

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaSpbmInterParameter =
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

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaSpbmIntraParameter =
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

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaSpbmInvstPortfDef =
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

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaSpbmOptionParameter =
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

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaSpbmPortfDefinition =
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

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaSpmmInstParam =
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

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaSpmmModelParam =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable SpmmModelId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable CommodityGroupId: byte array

    [<DefaultValue>]
    val mutable IntraCommodityRate: float

    [<DefaultValue>]
    val mutable InterCommodityRate: float

    [<DefaultValue>]
    val mutable OptionDiscountRate: float

    [<DefaultValue>]
    val mutable MiniMarginRatio: float

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaSpmmProductParam =
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

    [<DefaultValue>]
    val mutable ActionDirection: byte

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeltaTradingAccount =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<DefaultValue>]
    val mutable PreMortgage: float

    [<DefaultValue>]
    val mutable PreCredit: float

    [<DefaultValue>]
    val mutable PreDeposit: float

    [<DefaultValue>]
    val mutable PreBalance: float

    [<DefaultValue>]
    val mutable PreMargin: float

    [<DefaultValue>]
    val mutable InterestBase: float

    [<DefaultValue>]
    val mutable Interest: float

    [<DefaultValue>]
    val mutable Deposit: float

    [<DefaultValue>]
    val mutable Withdraw: float

    [<DefaultValue>]
    val mutable FrozenMargin: float

    [<DefaultValue>]
    val mutable FrozenCash: float

    [<DefaultValue>]
    val mutable FrozenCommission: float

    [<DefaultValue>]
    val mutable CurrMargin: float

    [<DefaultValue>]
    val mutable CashIn: float

    [<DefaultValue>]
    val mutable Commission: float

    [<DefaultValue>]
    val mutable CloseProfit: float

    [<DefaultValue>]
    val mutable PositionProfit: float

    [<DefaultValue>]
    val mutable Balance: float

    [<DefaultValue>]
    val mutable Available: float

    [<DefaultValue>]
    val mutable WithdrawQuota: float

    [<DefaultValue>]
    val mutable Reserve: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<DefaultValue>]
    val mutable SettlementId: int

    [<DefaultValue>]
    val mutable Credit: float

    [<DefaultValue>]
    val mutable Mortgage: float

    [<DefaultValue>]
    val mutable ExchangeMargin: float

    [<DefaultValue>]
    val mutable DeliveryMargin: float

    [<DefaultValue>]
    val mutable ExchangeDeliveryMargin: float

    [<DefaultValue>]
    val mutable ReserveBalance: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<DefaultValue>]
    val mutable PreFundMortgageIn: float

    [<DefaultValue>]
    val mutable PreFundMortgageOut: float

    [<DefaultValue>]
    val mutable FundMortgageIn: float

    [<DefaultValue>]
    val mutable FundMortgageOut: float

    [<DefaultValue>]
    val mutable FundMortgageAvailable: float

    [<DefaultValue>]
    val mutable MortgageableFund: float

    [<DefaultValue>]
    val mutable SpecProductMargin: float

    [<DefaultValue>]
    val mutable SpecProductFrozenMargin: float

    [<DefaultValue>]
    val mutable SpecProductCommission: float

    [<DefaultValue>]
    val mutable SpecProductFrozenCommission: float

    [<DefaultValue>]
    val mutable SpecProductPositionProfit: float

    [<DefaultValue>]
    val mutable SpecProductCloseProfit: float

    [<DefaultValue>]
    val mutable SpecProductPositionProfitByAlg: float

    [<DefaultValue>]
    val mutable SpecProductExchangeMargin: float

    [<DefaultValue>]
    val mutable FrozenSwap: float

    [<DefaultValue>]
    val mutable RemainSwap: float

    [<DefaultValue>]
    val mutable OptionValue: float

    [<DefaultValue>]
    val mutable SyncDeltaSequenceNo: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncDeposit =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)>]
    [<DefaultValue>]
    val mutable DepositSeqNo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<DefaultValue>]
    val mutable Deposit: float

    [<DefaultValue>]
    val mutable IsForce: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<DefaultValue>]
    val mutable IsFromSopt: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable TradingPassword: byte array

    [<DefaultValue>]
    val mutable IsSecAgentTranfer: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncFundMortgage =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)>]
    [<DefaultValue>]
    val mutable MortgageSeqNo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable FromCurrencyId: byte array

    [<DefaultValue>]
    val mutable MortgageAmount: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable ToCurrencyId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncSpbmParameterEnd =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncStatus =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<DefaultValue>]
    val mutable DataSyncStatus: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncingInstrumentCommissionRate =
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
type private NativeSyncingInstrumentMarginRate =
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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncingInstrumentTradingRight =
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
    val mutable TradingRight: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncingInvestor =
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
type private NativeSyncingInvestorGroup =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorGroupId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable InvestorGroupName: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncingInvestorPosition =
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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncingTradingAccount =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<DefaultValue>]
    val mutable PreMortgage: float

    [<DefaultValue>]
    val mutable PreCredit: float

    [<DefaultValue>]
    val mutable PreDeposit: float

    [<DefaultValue>]
    val mutable PreBalance: float

    [<DefaultValue>]
    val mutable PreMargin: float

    [<DefaultValue>]
    val mutable InterestBase: float

    [<DefaultValue>]
    val mutable Interest: float

    [<DefaultValue>]
    val mutable Deposit: float

    [<DefaultValue>]
    val mutable Withdraw: float

    [<DefaultValue>]
    val mutable FrozenMargin: float

    [<DefaultValue>]
    val mutable FrozenCash: float

    [<DefaultValue>]
    val mutable FrozenCommission: float

    [<DefaultValue>]
    val mutable CurrMargin: float

    [<DefaultValue>]
    val mutable CashIn: float

    [<DefaultValue>]
    val mutable Commission: float

    [<DefaultValue>]
    val mutable CloseProfit: float

    [<DefaultValue>]
    val mutable PositionProfit: float

    [<DefaultValue>]
    val mutable Balance: float

    [<DefaultValue>]
    val mutable Available: float

    [<DefaultValue>]
    val mutable WithdrawQuota: float

    [<DefaultValue>]
    val mutable Reserve: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradingDay: byte array

    [<DefaultValue>]
    val mutable SettlementId: int

    [<DefaultValue>]
    val mutable Credit: float

    [<DefaultValue>]
    val mutable Mortgage: float

    [<DefaultValue>]
    val mutable ExchangeMargin: float

    [<DefaultValue>]
    val mutable DeliveryMargin: float

    [<DefaultValue>]
    val mutable ExchangeDeliveryMargin: float

    [<DefaultValue>]
    val mutable ReserveBalance: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<DefaultValue>]
    val mutable PreFundMortgageIn: float

    [<DefaultValue>]
    val mutable PreFundMortgageOut: float

    [<DefaultValue>]
    val mutable FundMortgageIn: float

    [<DefaultValue>]
    val mutable FundMortgageOut: float

    [<DefaultValue>]
    val mutable FundMortgageAvailable: float

    [<DefaultValue>]
    val mutable MortgageableFund: float

    [<DefaultValue>]
    val mutable SpecProductMargin: float

    [<DefaultValue>]
    val mutable SpecProductFrozenMargin: float

    [<DefaultValue>]
    val mutable SpecProductCommission: float

    [<DefaultValue>]
    val mutable SpecProductFrozenCommission: float

    [<DefaultValue>]
    val mutable SpecProductPositionProfit: float

    [<DefaultValue>]
    val mutable SpecProductCloseProfit: float

    [<DefaultValue>]
    val mutable SpecProductPositionProfitByAlg: float

    [<DefaultValue>]
    val mutable SpecProductExchangeMargin: float

    [<DefaultValue>]
    val mutable FrozenSwap: float

    [<DefaultValue>]
    val mutable RemainSwap: float

    [<DefaultValue>]
    val mutable OptionValue: float

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeSyncingTradingCode =
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

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTgIpAddrParam =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)>]
    [<DefaultValue>]
    val mutable Address: byte array

    [<DefaultValue>]
    val mutable DrIdentityId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 65)>]
    [<DefaultValue>]
    val mutable DrIdentityName: byte array

    [<DefaultValue>]
    val mutable AddrSrvMode: byte

    [<DefaultValue>]
    val mutable AddrVer: byte

    [<DefaultValue>]
    val mutable AddrNo: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 65)>]
    [<DefaultValue>]
    val mutable AddrName: byte array

    [<DefaultValue>]
    val mutable IsSm: int

    [<DefaultValue>]
    val mutable IsLocalAddr: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 161)>]
    [<DefaultValue>]
    val mutable Remark: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 51)>]
    [<DefaultValue>]
    val mutable Site: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable NetOperator: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 65)>]
    [<DefaultValue>]
    val mutable SysName: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTgSessionQryStatus =
    [<DefaultValue>]
    val mutable LastQryFreq: int

    [<DefaultValue>]
    val mutable QryStatus: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeThostUserFunction =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable ThostFunctionCode: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTradeParam =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<DefaultValue>]
    val mutable TradeParamId: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)>]
    [<DefaultValue>]
    val mutable TradeParamValue: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 161)>]
    [<DefaultValue>]
    val mutable Memo: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTrader =
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
    val mutable InstallCount: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<DefaultValue>]
    val mutable OrderCancelAlg: byte

    [<DefaultValue>]
    val mutable TradeInstallCount: int

    [<DefaultValue>]
    val mutable MdInstallCount: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTraderAssign =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable ExchangeId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable TraderId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable ParticipantId: byte array

    [<DefaultValue>]
    val mutable DrIdentityId: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTradingAccountPassword =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Password: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTradingAccountPasswordUpdateFromSec =
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

    [<DefaultValue>]
    val mutable FromSec: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTradingAccountPasswordUpdateV1 =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable OldPassword: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable NewPassword: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTradingAccountReserve =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<DefaultValue>]
    val mutable Reserve: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTransferBankToFutureReq =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable FutureAccount: byte array

    [<DefaultValue>]
    val mutable FuturePwdFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable FutureAccPwd: byte array

    [<DefaultValue>]
    val mutable TradeAmt: float

    [<DefaultValue>]
    val mutable CustFee: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyCode: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTransferBankToFutureRsp =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)>]
    [<DefaultValue>]
    val mutable RetCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)>]
    [<DefaultValue>]
    val mutable RetInfo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable FutureAccount: byte array

    [<DefaultValue>]
    val mutable TradeAmt: float

    [<DefaultValue>]
    val mutable CustFee: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyCode: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTransferFutureToBankReq =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable FutureAccount: byte array

    [<DefaultValue>]
    val mutable FuturePwdFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable FutureAccPwd: byte array

    [<DefaultValue>]
    val mutable TradeAmt: float

    [<DefaultValue>]
    val mutable CustFee: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyCode: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTransferFutureToBankRsp =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)>]
    [<DefaultValue>]
    val mutable RetCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)>]
    [<DefaultValue>]
    val mutable RetInfo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable FutureAccount: byte array

    [<DefaultValue>]
    val mutable TradeAmt: float

    [<DefaultValue>]
    val mutable CustFee: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyCode: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTransferHeader =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable Version: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)>]
    [<DefaultValue>]
    val mutable TradeCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradeDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradeTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradeSerial: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable FutureId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable BankId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)>]
    [<DefaultValue>]
    val mutable BankBrchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable OperNo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)>]
    [<DefaultValue>]
    val mutable DeviceId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)>]
    [<DefaultValue>]
    val mutable RecordNum: byte array

    [<DefaultValue>]
    val mutable SessionId: int

    [<DefaultValue>]
    val mutable RequestId: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTransferQryBankReq =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable FutureAccount: byte array

    [<DefaultValue>]
    val mutable FuturePwdFlag: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable FutureAccPwd: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyCode: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTransferQryBankRsp =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)>]
    [<DefaultValue>]
    val mutable RetCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 129)>]
    [<DefaultValue>]
    val mutable RetInfo: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable FutureAccount: byte array

    [<DefaultValue>]
    val mutable TradeAmt: float

    [<DefaultValue>]
    val mutable UseAmt: float

    [<DefaultValue>]
    val mutable FetchAmt: float

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyCode: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTransferQryDetailReq =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable FutureAccount: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeTransferQryDetailRsp =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradeDate: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable TradeTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)>]
    [<DefaultValue>]
    val mutable TradeCode: byte array

    [<DefaultValue>]
    val mutable FutureSerial: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable FutureId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)>]
    [<DefaultValue>]
    val mutable FutureAccount: byte array

    [<DefaultValue>]
    val mutable BankSerial: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable BankId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)>]
    [<DefaultValue>]
    val mutable BankBrchId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BankAccount: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable CertCode: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyCode: byte array

    [<DefaultValue>]
    val mutable TxAmount: float

    [<DefaultValue>]
    val mutable Flag: byte

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeUserDriBypass =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable DrIdentityId: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeUserIp =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve1: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable Reserve2: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)>]
    [<DefaultValue>]
    val mutable MacAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable IpMask: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeUserPasswordUpdateFromSec =
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

    [<DefaultValue>]
    val mutable FromSec: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeUserRight =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable UserRightType: byte

    [<DefaultValue>]
    val mutable IsForbidden: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeUserRightsAssign =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

    [<DefaultValue>]
    val mutable DrIdentityId: int

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeVerifyCustInfo =
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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 161)>]
    [<DefaultValue>]
    val mutable LongCustomerName: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeVerifyFuturePassword =
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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Password: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BankAccount: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable BankPassWord: byte array

    [<DefaultValue>]
    val mutable InstallId: int

    [<DefaultValue>]
    val mutable Tid: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeVerifyFuturePasswordAndCustInfo =
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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Password: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)>]
    [<DefaultValue>]
    val mutable CurrencyId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 161)>]
    [<DefaultValue>]
    val mutable LongCustomerName: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeVerifyInvestorPassword =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable InvestorId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable Password: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeWithDrawParam =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)>]
    [<DefaultValue>]
    val mutable AccountId: byte array

    [<DefaultValue>]
    val mutable WithDrawParamId: byte

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable WithDrawParamValue: byte array
