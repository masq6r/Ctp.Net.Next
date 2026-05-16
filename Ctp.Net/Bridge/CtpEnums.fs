namespace Ctp.Net.Bridge

open System

// ---- Trading direction ----

/// TThostFtdcDirectionType: Buy='0', Sell='1'
type Direction =
    | Buy
    | Sell
    static member FromChar(c: char) =
        match c with
        | '0' -> Buy
        | '1' -> Sell
        | c -> invalidArg (nameof c) $"Invalid Direction: %c{c}"
    static member ToChar(d: Direction) =
        match d with
        | Buy -> '0'
        | Sell -> '1'

// ---- Position direction ----

/// TThostFtdcPosiDirectionType: Net='1', Long='2', Short='3'
type PosiDirection =
    | Net
    | Long
    | Short
    static member FromChar(c: char) =
        match c with
        | '1' -> Net
        | '2' -> Long
        | '3' -> Short
        | c -> invalidArg (nameof c) $"Invalid PosiDirection: %c{c}"
    static member ToChar(d: PosiDirection) =
        match d with
        | Net -> '1'
        | Long -> '2'
        | Short -> '3'

/// TThostFtdcCombDirectionType: Comb='0', UnComb='1', DelComb='2'
type CombDirection =
    | Comb
    | UnComb
    | DelComb
    static member FromChar(c: char) =
        match c with
        | '0' -> Comb
        | '1' -> UnComb
        | '2' -> DelComb
        | c -> invalidArg (nameof c) $"Invalid CombDirection: %c{c}"
    static member ToChar(d: CombDirection) =
        match d with
        | Comb -> '0'
        | UnComb -> '1'
        | DelComb -> '2'

// ---- Hedge flags ----

/// TThostFtdcHedgeFlagType: Speculation='1', Arbitrage='2', Hedge='3', MarketMaker='5', SpecHedge='6', HedgeSpec='7'
type HedgeFlag =
    | Speculation
    | Arbitrage
    | Hedge
    | MarketMaker
    | SpecHedge
    | HedgeSpec
    static member FromChar(c: char) =
        match c with
        | '1' -> Speculation
        | '2' -> Arbitrage
        | '3' -> Hedge
        | '5' -> MarketMaker
        | '6' -> SpecHedge
        | '7' -> HedgeSpec
        | c -> invalidArg (nameof c) $"Invalid HedgeFlag: %c{c}"
    static member ToChar(d: HedgeFlag) =
        match d with
        | Speculation -> '1'
        | Arbitrage -> '2'
        | Hedge -> '3'
        | MarketMaker -> '5'
        | SpecHedge -> '6'
        | HedgeSpec -> '7'

// ---- Offset flags ----

/// TThostFtdcOffsetFlagType: Open='0', Close='1', ForceClose='2', CloseToday='3', CloseYesterday='4', ForceOff='5', LocalForceClose='6'
type OffsetFlag =
    | Open
    | Close
    | ForceClose
    | CloseToday
    | CloseYesterday
    | ForceOff
    | LocalForceClose
    static member FromChar(c: char) =
        match c with
        | '0' -> Open
        | '1' -> Close
        | '2' -> ForceClose
        | '3' -> CloseToday
        | '4' -> CloseYesterday
        | '5' -> ForceOff
        | '6' -> LocalForceClose
        | c -> invalidArg (nameof c) $"Invalid OffsetFlag: %c{c}"
    static member ToChar(d: OffsetFlag) =
        match d with
        | Open -> '0'
        | Close -> '1'
        | ForceClose -> '2'
        | CloseToday -> '3'
        | CloseYesterday -> '4'
        | ForceOff -> '5'
        | LocalForceClose -> '6'

/// TThostFtdcOffsetTypeType: OPTOFFSET='0', FUTOFFSET='1', EXECOFFSET='2', PERFORMOFFSET='3'
type OffsetType =
    | OptOffset
    | FutOffset
    | ExecOffset
    | PerformOffset
    static member FromChar(c: char) =
        match c with
        | '0' -> OptOffset
        | '1' -> FutOffset
        | '2' -> ExecOffset
        | '3' -> PerformOffset
        | c -> invalidArg (nameof c) $"Invalid OffsetType: %c{c}"
    static member ToChar(d: OffsetType) =
        match d with
        | OptOffset -> '0'
        | FutOffset -> '1'
        | ExecOffset -> '2'
        | PerformOffset -> '3'

// ---- Order price type ----

/// TThostFtdcOrderPriceTypeType: AnyPrice='1', LimitPrice='2', BestPrice='3', LastPrice='4',
/// LastPricePlusOneTicks='5', LastPricePlusTwoTicks='6', LastPricePlusThreeTicks='7',
/// AskPrice1='8', AskPrice1PlusOneTicks='9', AskPrice1PlusTwoTicks='A', AskPrice1PlusThreeTicks='B',
/// BidPrice1='C', BidPrice1PlusOneTicks='D', BidPrice1PlusTwoTicks='E', BidPrice1PlusThreeTicks='F',
/// FiveLevelPrice='G'
type OrderPriceType =
    | AnyPrice
    | LimitPrice
    | BestPrice
    | LastPrice
    | LastPricePlusOneTicks
    | LastPricePlusTwoTicks
    | LastPricePlusThreeTicks
    | AskPrice1
    | AskPrice1PlusOneTicks
    | AskPrice1PlusTwoTicks
    | AskPrice1PlusThreeTicks
    | BidPrice1
    | BidPrice1PlusOneTicks
    | BidPrice1PlusTwoTicks
    | BidPrice1PlusThreeTicks
    | FiveLevelPrice
    static member FromChar(c: char) =
        match c with
        | '1' -> AnyPrice
        | '2' -> LimitPrice
        | '3' -> BestPrice
        | '4' -> LastPrice
        | '5' -> LastPricePlusOneTicks
        | '6' -> LastPricePlusTwoTicks
        | '7' -> LastPricePlusThreeTicks
        | '8' -> AskPrice1
        | '9' -> AskPrice1PlusOneTicks
        | 'A' -> AskPrice1PlusTwoTicks
        | 'B' -> AskPrice1PlusThreeTicks
        | 'C' -> BidPrice1
        | 'D' -> BidPrice1PlusOneTicks
        | 'E' -> BidPrice1PlusTwoTicks
        | 'F' -> BidPrice1PlusThreeTicks
        | 'G' -> FiveLevelPrice
        | c -> invalidArg (nameof c) $"Invalid OrderPriceType: %c{c}"
    static member ToChar(d: OrderPriceType) =
        match d with
        | AnyPrice -> '1'
        | LimitPrice -> '2'
        | BestPrice -> '3'
        | LastPrice -> '4'
        | LastPricePlusOneTicks -> '5'
        | LastPricePlusTwoTicks -> '6'
        | LastPricePlusThreeTicks -> '7'
        | AskPrice1 -> '8'
        | AskPrice1PlusOneTicks -> '9'
        | AskPrice1PlusTwoTicks -> 'A'
        | AskPrice1PlusThreeTicks -> 'B'
        | BidPrice1 -> 'C'
        | BidPrice1PlusOneTicks -> 'D'
        | BidPrice1PlusTwoTicks -> 'E'
        | BidPrice1PlusThreeTicks -> 'F'
        | FiveLevelPrice -> 'G'

// ---- Time / volume conditions ----

/// TThostFtdcTimeConditionType: IOC='1', GFS='2', GFD='3', GTD='4', GTC='5', GFA='6'
type TimeCondition =
    | IOC
    | GFS
    | GFD
    | GTD
    | GTC
    | GFA
    static member FromChar(c: char) =
        match c with
        | '1' -> IOC
        | '2' -> GFS
        | '3' -> GFD
        | '4' -> GTD
        | '5' -> GTC
        | '6' -> GFA
        | c -> invalidArg (nameof c) $"Invalid TimeCondition: %c{c}"
    static member ToChar(d: TimeCondition) =
        match d with
        | IOC -> '1'
        | GFS -> '2'
        | GFD -> '3'
        | GTD -> '4'
        | GTC -> '5'
        | GFA -> '6'

/// TThostFtdcVolumeConditionType: AV='1', MV='2', CV='3'
type VolumeCondition =
    | AV
    | MV
    | CV
    static member FromChar(c: char) =
        match c with
        | '1' -> AV
        | '2' -> MV
        | '3' -> CV
        | c -> invalidArg (nameof c) $"Invalid VolumeCondition: %c{c}"
    static member ToChar(d: VolumeCondition) =
        match d with
        | AV -> '1'
        | MV -> '2'
        | CV -> '3'

/// TThostFtdcContingentConditionType: Immediately='1', Touch='2', TouchProfit='3', ParkedOrder='4',
/// LastPriceGreaterThanStopPrice='5', LastPriceGreaterEqualStopPrice='6',
/// LastPriceLesserThanStopPrice='7', LastPriceLesserEqualStopPrice='8',
/// AskPriceGreaterThanStopPrice='9', AskPriceGreaterEqualStopPrice='A',
/// AskPriceLesserThanStopPrice='B', AskPriceLesserEqualStopPrice='C',
/// BidPriceGreaterThanStopPrice='D', BidPriceGreaterEqualStopPrice='E',
/// BidPriceLesserThanStopPrice='F', BidPriceLesserEqualStopPrice='H'
type ContingentCondition =
    | Immediately
    | Touch
    | TouchProfit
    | ParkedOrder
    | LastPriceGreaterThanStopPrice
    | LastPriceGreaterEqualStopPrice
    | LastPriceLesserThanStopPrice
    | LastPriceLesserEqualStopPrice
    | AskPriceGreaterThanStopPrice
    | AskPriceGreaterEqualStopPrice
    | AskPriceLesserThanStopPrice
    | AskPriceLesserEqualStopPrice
    | BidPriceGreaterThanStopPrice
    | BidPriceGreaterEqualStopPrice
    | BidPriceLesserThanStopPrice
    | BidPriceLesserEqualStopPrice
    static member FromChar(c: char) =
        match c with
        | '1' -> Immediately
        | '2' -> Touch
        | '3' -> TouchProfit
        | '4' -> ParkedOrder
        | '5' -> LastPriceGreaterThanStopPrice
        | '6' -> LastPriceGreaterEqualStopPrice
        | '7' -> LastPriceLesserThanStopPrice
        | '8' -> LastPriceLesserEqualStopPrice
        | '9' -> AskPriceGreaterThanStopPrice
        | 'A' -> AskPriceGreaterEqualStopPrice
        | 'B' -> AskPriceLesserThanStopPrice
        | 'C' -> AskPriceLesserEqualStopPrice
        | 'D' -> BidPriceGreaterThanStopPrice
        | 'E' -> BidPriceGreaterEqualStopPrice
        | 'F' -> BidPriceLesserThanStopPrice
        | 'H' -> BidPriceLesserEqualStopPrice
        | c -> invalidArg (nameof c) $"Invalid ContingentCondition: %c{c}"
    static member ToChar(d: ContingentCondition) =
        match d with
        | Immediately -> '1'
        | Touch -> '2'
        | TouchProfit -> '3'
        | ParkedOrder -> '4'
        | LastPriceGreaterThanStopPrice -> '5'
        | LastPriceGreaterEqualStopPrice -> '6'
        | LastPriceLesserThanStopPrice -> '7'
        | LastPriceLesserEqualStopPrice -> '8'
        | AskPriceGreaterThanStopPrice -> '9'
        | AskPriceGreaterEqualStopPrice -> 'A'
        | AskPriceLesserThanStopPrice -> 'B'
        | AskPriceLesserEqualStopPrice -> 'C'
        | BidPriceGreaterThanStopPrice -> 'D'
        | BidPriceGreaterEqualStopPrice -> 'E'
        | BidPriceLesserThanStopPrice -> 'F'
        | BidPriceLesserEqualStopPrice -> 'H'

// ---- Force close ----

/// TThostFtdcForceCloseReasonType: NotForceClose='0', LackDeposit='1', ClientOverPositionLimit='2',
/// MemberOverPositionLimit='3', NotMultiple='4', Violation='5', Other='6', PersonDeliv='7',
/// Notverifycapital='8', LocalLackDeposit='9', LocalViolationNocheck='a', LocalViolation='b'
type ForceCloseReason =
    | NotForceClose
    | LackDeposit
    | ClientOverPositionLimit
    | MemberOverPositionLimit
    | NotMultiple
    | Violation
    | Other
    | PersonDeliv
    | NotVerifyCapital
    | LocalLackDeposit
    | LocalViolationNocheck
    | LocalViolation
    static member FromChar(c: char) =
        match c with
        | '0' -> NotForceClose
        | '1' -> LackDeposit
        | '2' -> ClientOverPositionLimit
        | '3' -> MemberOverPositionLimit
        | '4' -> NotMultiple
        | '5' -> Violation
        | '6' -> Other
        | '7' -> PersonDeliv
        | '8' -> NotVerifyCapital
        | '9' -> LocalLackDeposit
        | 'a' -> LocalViolationNocheck
        | 'b' -> LocalViolation
        | c -> invalidArg (nameof c) $"Invalid ForceCloseReason: %c{c}"
    static member ToChar(d: ForceCloseReason) =
        match d with
        | NotForceClose -> '0'
        | LackDeposit -> '1'
        | ClientOverPositionLimit -> '2'
        | MemberOverPositionLimit -> '3'
        | NotMultiple -> '4'
        | Violation -> '5'
        | Other -> '6'
        | PersonDeliv -> '7'
        | NotVerifyCapital -> '8'
        | LocalLackDeposit -> '9'
        | LocalViolationNocheck -> 'a'
        | LocalViolation -> 'b'

// ---- Order status ----

/// TThostFtdcOrderActionFlagType: Delete='0', Modify='3'
type ActionFlag =
    | Delete
    | Modify
    static member FromChar(c: char) =
        match c with
        | '0' -> Delete
        | '3' -> Modify
        | c -> invalidArg (nameof c) $"Invalid ActionFlag: %c{c}"
    static member ToChar(d: ActionFlag) =
        match d with
        | Delete -> '0'
        | Modify -> '3'

/// TThostFtdcOrderStatusType: AllTraded='0', PartTradedQueueing='1', PartTradedNotQueueing='2',
/// NoTradeQueueing='3', NoTradeNotQueueing='4', Canceled='5', Unknown='a', NotTouched='b', Touched='c'
type OrderStatus =
    | AllTraded
    | PartTradedQueueing
    | PartTradedNotQueueing
    | NoTradeQueueing
    | NoTradeNotQueueing
    | Canceled
    | Unknown
    | NotTouched
    | Touched
    static member FromChar(c: char) =
        match c with
        | '0' -> AllTraded
        | '1' -> PartTradedQueueing
        | '2' -> PartTradedNotQueueing
        | '3' -> NoTradeQueueing
        | '4' -> NoTradeNotQueueing
        | '5' -> Canceled
        | 'a' -> Unknown
        | 'b' -> NotTouched
        | 'c' -> Touched
        | c -> invalidArg (nameof c) $"Invalid OrderStatus: %c{c}"
    static member ToChar(d: OrderStatus) =
        match d with
        | AllTraded -> '0'
        | PartTradedQueueing -> '1'
        | PartTradedNotQueueing -> '2'
        | NoTradeQueueing -> '3'
        | NoTradeNotQueueing -> '4'
        | Canceled -> '5'
        | Unknown -> 'a'
        | NotTouched -> 'b'
        | Touched -> 'c'

/// TThostFtdcOrderSubmitStatusType: InsertSubmitted='0', CancelSubmitted='1', ModifySubmitted='2',
/// Accepted='3', InsertRejected='4', CancelRejected='5', ModifyRejected='6'
type OrderSubmitStatus =
    | InsertSubmitted
    | CancelSubmitted
    | ModifySubmitted
    | Accepted
    | InsertRejected
    | CancelRejected
    | ModifyRejected
    static member FromChar(c: char) =
        match c with
        | '0' -> InsertSubmitted
        | '1' -> CancelSubmitted
        | '2' -> ModifySubmitted
        | '3' -> Accepted
        | '4' -> InsertRejected
        | '5' -> CancelRejected
        | '6' -> ModifyRejected
        | c -> invalidArg (nameof c) $"Invalid OrderSubmitStatus: %c{c}"
    static member ToChar(d: OrderSubmitStatus) =
        match d with
        | InsertSubmitted -> '0'
        | CancelSubmitted -> '1'
        | ModifySubmitted -> '2'
        | Accepted -> '3'
        | InsertRejected -> '4'
        | CancelRejected -> '5'
        | ModifyRejected -> '6'

/// TThostFtdcOrderActionStatusType: Submitted='a', Accepted='b', Rejected='c'
type OrderActionStatus =
    | Submitted
    | Accepted
    | Rejected
    static member FromChar(c: char) =
        match c with
        | 'a' -> Submitted
        | 'b' -> Accepted
        | 'c' -> Rejected
        | c -> invalidArg (nameof c) $"Invalid OrderActionStatus: %c{c}"
    static member ToChar(d: OrderActionStatus) =
        match d with
        | Submitted -> 'a'
        | Accepted -> 'b'
        | Rejected -> 'c'

/// TThostFtdcOrderCancelAlgType: Balance='1', OrigFirst='2'
type OrderCancelAlg =
    | Balance
    | OrigFirst
    static member FromChar(c: char) =
        match c with
        | '1' -> Balance
        | '2' -> OrigFirst
        | c -> invalidArg (nameof c) $"Invalid OrderCancelAlg: %c{c}"
    static member ToChar(d: OrderCancelAlg) =
        match d with
        | Balance -> '1'
        | OrigFirst -> '2'

// ---- Position ----

/// TThostFtdcPositionDateType: Today='1', History='2'
type PositionDate =
    | Today
    | History
    static member FromChar(c: char) =
        match c with
        | '1' -> Today
        | '2' -> History
        | c -> invalidArg (nameof c) $"Invalid PositionDate: %c{c}"
    static member ToChar(d: PositionDate) =
        match d with
        | Today -> '1'
        | History -> '2'

/// TThostFtdcPositionDateTypeType: UseHistory='1', NoUseHistory='2'
type PositionDateType =
    | UseHistory
    | NoUseHistory
    static member FromChar(c: char) =
        match c with
        | '1' -> UseHistory
        | '2' -> NoUseHistory
        | c -> invalidArg (nameof c) $"Invalid PositionDateType: %c{c}"
    static member ToChar(d: PositionDateType) =
        match d with
        | UseHistory -> '1'
        | NoUseHistory -> '2'

/// TThostFtdcPositionTypeType: Net='1', Gross='2'
type PositionType =
    | Net
    | Gross
    static member FromChar(c: char) =
        match c with
        | '1' -> Net
        | '2' -> Gross
        | c -> invalidArg (nameof c) $"Invalid PositionType: %c{c}"
    static member ToChar(d: PositionType) =
        match d with
        | Net -> '1'
        | Gross -> '2'

/// TThostFtdcExecOrderPositionFlagType: Reserve='0', UnReserve='1'
type ReservePositionFlag =
    | Reserve
    | UnReserve
    static member FromChar(c: char) =
        match c with
        | '0' -> Reserve
        | '1' -> UnReserve
        | c -> invalidArg (nameof c) $"Invalid ReservePositionFlag: %c{c}"
    static member ToChar(d: ReservePositionFlag) =
        match d with
        | Reserve -> '0'
        | UnReserve -> '1'

/// TThostFtdcExecOrderCloseFlagType: AutoClose='0', NotToClose='1'
type CloseFlag =
    | AutoClose
    | NotToClose
    static member FromChar(c: char) =
        match c with
        | '0' -> AutoClose
        | '1' -> NotToClose
        | c -> invalidArg (nameof c) $"Invalid CloseFlag: %c{c}"
    static member ToChar(d: CloseFlag) =
        match d with
        | AutoClose -> '0'
        | NotToClose -> '1'

/// TThostFtdcOptSelfCloseFlagType: CloseSelfOptionPosition='1', ReserveOptionPosition='2',
/// SellCloseSelfFuturePosition='3', ReserveFuturePosition='4'
type OptSelfCloseFlag =
    | CloseSelfOptionPosition
    | ReserveOptionPosition
    | SellCloseSelfFuturePosition
    | ReserveFuturePosition
    static member FromChar(c: char) =
        match c with
        | '1' -> CloseSelfOptionPosition
        | '2' -> ReserveOptionPosition
        | '3' -> SellCloseSelfFuturePosition
        | '4' -> ReserveFuturePosition
        | c -> invalidArg (nameof c) $"Invalid OptSelfCloseFlag: %c{c}"
    static member ToChar(d: OptSelfCloseFlag) =
        match d with
        | CloseSelfOptionPosition -> '1'
        | ReserveOptionPosition -> '2'
        | SellCloseSelfFuturePosition -> '3'
        | ReserveFuturePosition -> '4'

/// TThostFtdcCloseDealTypeType: Normal='0', SpecFirst='1'
type CloseDealType =
    | Normal
    | SpecFirst
    static member FromChar(c: char) =
        match c with
        | '0' -> Normal
        | '1' -> SpecFirst
        | c -> invalidArg (nameof c) $"Invalid CloseDealType: %c{c}"
    static member ToChar(d: CloseDealType) =
        match d with
        | Normal -> '0'
        | SpecFirst -> '1'

// ---- Product / Instrument ----

/// TThostFtdcProductClassType: Futures='1', Options='2', Combination='3', Spot='4',
/// EFP='5', SpotOption='6', TAS='7', MI='I'
type ProductClass =
    | Futures
    | Options
    | Combination
    | Spot
    | EFP
    | SpotOption
    | TAS
    | MI
    static member FromChar(c: char) =
        match c with
        | '1' -> Futures
        | '2' -> Options
        | '3' -> Combination
        | '4' -> Spot
        | '5' -> EFP
        | '6' -> SpotOption
        | '7' -> TAS
        | 'I' -> MI
        | c -> invalidArg (nameof c) $"Invalid ProductClass: %c{c}"
    static member ToChar(d: ProductClass) =
        match d with
        | Futures -> '1'
        | Options -> '2'
        | Combination -> '3'
        | Spot -> '4'
        | EFP -> '5'
        | SpotOption -> '6'
        | TAS -> '7'
        | MI -> 'I'

/// TThostFtdcInstLifePhaseType: NotStart='0', Started='1', Pause='2', Expired='3'
type InstLifePhase =
    | NotStart
    | Started
    | Pause
    | Expired
    static member FromChar(c: char) =
        match c with
        | '0' -> NotStart
        | '1' -> Started
        | '2' -> Pause
        | '3' -> Expired
        | c -> invalidArg (nameof c) $"Invalid InstLifePhase: %c{c}"
    static member ToChar(d: InstLifePhase) =
        match d with
        | NotStart -> '0'
        | Started -> '1'
        | Pause -> '2'
        | Expired -> '3'

/// TThostFtdcProductStatusType: Tradeable='1', Untradeable='2'
type ProductStatus =
    | Tradeable
    | Untradeable
    static member FromChar(c: char) =
        match c with
        | '1' -> Tradeable
        | '2' -> Untradeable
        | c -> invalidArg (nameof c) $"Invalid ProductStatus: %c{c}"
    static member ToChar(d: ProductStatus) =
        match d with
        | Tradeable -> '1'
        | Untradeable -> '2'

/// TThostFtdcCombinationTypeType: STD='3', STG='4', PRT='5', CAS='6', OPL='7', BFO='8', BLS='9', BES='a'
type CombinationType =
    | STD
    | STG
    | PRT
    | CAS
    | OPL
    | BFO
    | BLS
    | BES
    static member FromChar(c: char) =
        match c with
        | '3' -> STD
        | '4' -> STG
        | '5' -> PRT
        | '6' -> CAS
        | '7' -> OPL
        | '8' -> BFO
        | '9' -> BLS
        | 'a' -> BES
        | c -> invalidArg (nameof c) $"Invalid CombinationType: %c{c}"
    static member ToChar(d: CombinationType) =
        match d with
        | STD -> '3'
        | STG -> '4'
        | PRT -> '5'
        | CAS -> '6'
        | OPL -> '7'
        | BFO -> '8'
        | BLS -> '9'
        | BES -> 'a'

/// TThostFtdcCmbTypeType (ZCECmbType): SPZ='0', SPD='1', IPS='2', BUL='3', BER='4', BLT='5', BRT='6', STD='7', STG='8', PRT='9'
type CmbType =
    | SPZ
    | SPD
    | IPS
    | BUL
    | BER
    | BLT
    | BRT
    | STD
    | STG
    | PRT
    static member FromChar(c: char) =
        match c with
        | '0' -> SPZ
        | '1' -> SPD
        | '2' -> IPS
        | '3' -> BUL
        | '4' -> BER
        | '5' -> BLT
        | '6' -> BRT
        | '7' -> STD
        | '8' -> STG
        | '9' -> PRT
        | c -> invalidArg (nameof c) $"Invalid CmbType: %c{c}"
    static member ToChar(d: CmbType) =
        match d with
        | SPZ -> '0'
        | SPD -> '1'
        | IPS -> '2'
        | BUL -> '3'
        | BER -> '4'
        | BLT -> '5'
        | BRT -> '6'
        | STD -> '7'
        | STG -> '8'
        | PRT -> '9'

/// TThostFtdcOptionsTypeType: CallOptions='1', PutOptions='2'
type OptionsType =
    | CallOptions
    | PutOptions
    static member FromChar(c: char) =
        match c with
        | '1' -> CallOptions
        | '2' -> PutOptions
        | c -> invalidArg (nameof c) $"Invalid OptionsType: %c{c}"
    static member ToChar(d: OptionsType) =
        match d with
        | CallOptions -> '1'
        | PutOptions -> '2'

/// TThostFtdcOptionRoyaltyPriceTypeType: PreSettlementPrice='1', OpenPrice='4', MaxPreSettlementPrice='5'
type OptionRoyaltyPriceType =
    | PreSettlementPrice
    | OpenPrice
    | MaxPreSettlementPrice
    static member FromChar(c: char) =
        match c with
        | '1' -> PreSettlementPrice
        | '4' -> OpenPrice
        | '5' -> MaxPreSettlementPrice
        | c -> invalidArg (nameof c) $"Invalid OptionRoyaltyPriceType: %c{c}"
    static member ToChar(d: OptionRoyaltyPriceType) =
        match d with
        | PreSettlementPrice -> '1'
        | OpenPrice -> '4'
        | MaxPreSettlementPrice -> '5'

/// TThostFtdcTradingTypeType: All='0', Trade='1', UnTrade='2'
type TradingType =
    | All
    | Trade
    | UnTrade
    static member FromChar(c: char) =
        match c with
        | '0' -> All
        | '1' -> Trade
        | '2' -> UnTrade
        | c -> invalidArg (nameof c) $"Invalid TradingType: %c{c}"
    static member ToChar(d: TradingType) =
        match d with
        | All -> '0'
        | Trade -> '1'
        | UnTrade -> '2'

/// TThostFtdcClassTypeType: All='0', Future='1', Option='2', Comb='3'
type ClassType =
    | All
    | Future
    | Option
    | Comb
    static member FromChar(c: char) =
        match c with
        | '0' -> All
        | '1' -> Future
        | '2' -> Option
        | '3' -> Comb
        | c -> invalidArg (nameof c) $"Invalid ClassType: %c{c}"
    static member ToChar(d: ClassType) =
        match d with
        | All -> '0'
        | Future -> '1'
        | Option -> '2'
        | Comb -> '3'

/// TThostFtdcInstrumentClassType: Usual='1', Delivery='2', NonComb='3'
type InstrumentClass =
    | Usual
    | Delivery
    | NonComb
    static member FromChar(c: char) =
        match c with
        | '1' -> Usual
        | '2' -> Delivery
        | '3' -> NonComb
        | c -> invalidArg (nameof c) $"Invalid InstrumentClass: %c{c}"
    static member ToChar(d: InstrumentClass) =
        match d with
        | Usual -> '1'
        | Delivery -> '2'
        | NonComb -> '3'

/// TThostFtdcInstMarginCalIDType: BothSide='1', MMSA='2', SPMM='3'
type InstMarginCalId =
    | BothSide
    | MMSA
    | SPMM
    static member FromChar(c: char) =
        match c with
        | '1' -> BothSide
        | '2' -> MMSA
        | '3' -> SPMM
        | c -> invalidArg (nameof c) $"Invalid InstMarginCalId: %c{c}"
    static member ToChar(d: InstMarginCalId) =
        match d with
        | BothSide -> '1'
        | MMSA -> '2'
        | SPMM -> '3'

/// TThostFtdcMaxMarginSideAlgorithmType: No='0', Yes='1'
type MaxMarginSideAlgorithm =
    | No
    | Yes
    static member FromChar(c: char) =
        match c with
        | '0' -> No
        | '1' -> Yes
        | c -> invalidArg (nameof c) $"Invalid MaxMarginSideAlgorithm: %c{c}"
    static member ToChar(d: MaxMarginSideAlgorithm) =
        match d with
        | No -> '0'
        | Yes -> '1'

// ---- Trade ----

/// TThostFtdcTradeTypeType: SplitCombination='#', Common='0', OptionsExecution='1',
/// OTC='2', EFPDerived='3', CombinationDerived='4', BlockTrade='5'
type TradeType =
    | SplitCombination
    | Common
    | OptionsExecution
    | OTC
    | EFPDerived
    | CombinationDerived
    | BlockTrade
    static member FromChar(c: char) =
        match c with
        | '#' -> SplitCombination
        | '0' -> Common
        | '1' -> OptionsExecution
        | '2' -> OTC
        | '3' -> EFPDerived
        | '4' -> CombinationDerived
        | '5' -> BlockTrade
        | c -> invalidArg (nameof c) $"Invalid TradeType: %c{c}"
    static member ToChar(d: TradeType) =
        match d with
        | SplitCombination -> '#'
        | Common -> '0'
        | OptionsExecution -> '1'
        | OTC -> '2'
        | EFPDerived -> '3'
        | CombinationDerived -> '4'
        | BlockTrade -> '5'

/// TThostFtdcSpecPosiTypeType: Common='#', Tas='0'
type SpecPosiType =
    | Common
    | Tas
    static member FromChar(c: char) =
        match c with
        | '#' -> Common
        | '0' -> Tas
        | c -> invalidArg (nameof c) $"Invalid SpecPosiType: %c{c}"
    static member ToChar(d: SpecPosiType) =
        match d with
        | Common -> '#'
        | Tas -> '0'

/// TThostFtdcExecResultType: NoExec='n', Canceled='c', OK='0', NoPosition='1', NoDeposit='2',
/// NoParticipant='3', NoClient='4', NoInstrument='6', NoRight='7', InvalidVolume='8',
/// NoEnoughHistoryTrade='9', Unknown='a'
type ExecResult =
    | NoExec
    | Canceled
    | OK
    | NoPosition
    | NoDeposit
    | NoParticipant
    | NoClient
    | NoInstrument
    | NoRight
    | InvalidVolume
    | NoEnoughHistoryTrade
    | Unknown
    static member FromChar(c: char) =
        match c with
        | 'n' -> NoExec
        | 'c' -> Canceled
        | '0' -> OK
        | '1' -> NoPosition
        | '2' -> NoDeposit
        | '3' -> NoParticipant
        | '4' -> NoClient
        | '6' -> NoInstrument
        | '7' -> NoRight
        | '8' -> InvalidVolume
        | '9' -> NoEnoughHistoryTrade
        | 'a' -> Unknown
        | c -> invalidArg (nameof c) $"Invalid ExecResult: %c{c}"
    static member ToChar(d: ExecResult) =
        match d with
        | NoExec -> 'n'
        | Canceled -> 'c'
        | OK -> '0'
        | NoPosition -> '1'
        | NoDeposit -> '2'
        | NoParticipant -> '3'
        | NoClient -> '4'
        | NoInstrument -> '6'
        | NoRight -> '7'
        | InvalidVolume -> '8'
        | NoEnoughHistoryTrade -> '9'
        | Unknown -> 'a'

// ---- Client / User / Account ----

/// TThostFtdcClientIDTypeType: Speculation='1', Arbitrage='2', Hedge='3', MarketMaker='5'
type ClientIdType =
    | Speculation
    | Arbitrage
    | Hedge
    | MarketMaker
    static member FromChar(c: char) =
        match c with
        | '1' -> Speculation
        | '2' -> Arbitrage
        | '3' -> Hedge
        | '5' -> MarketMaker
        | c -> invalidArg (nameof c) $"Invalid ClientIdType: %c{c}"
    static member ToChar(d: ClientIdType) =
        match d with
        | Speculation -> '1'
        | Arbitrage -> '2'
        | Hedge -> '3'
        | MarketMaker -> '5'

/// TThostFtdcUserTypeType: Investor='0', Operator='1', SuperUser='2'
type UserType =
    | Investor
    | Operator
    | SuperUser
    static member FromChar(c: char) =
        match c with
        | '0' -> Investor
        | '1' -> Operator
        | '2' -> SuperUser
        | c -> invalidArg (nameof c) $"Invalid UserType: %c{c}"
    static member ToChar(d: UserType) =
        match d with
        | Investor -> '0'
        | Operator -> '1'
        | SuperUser -> '2'

/// TThostFtdcAppTypeType: Investor='1', InvestorRelay='2', OperatorRelay='3', UnKnown='4'
type AppType =
    | Investor
    | InvestorRelay
    | OperatorRelay
    | UnKnown
    static member FromChar(c: char) =
        match c with
        | '1' -> Investor
        | '2' -> InvestorRelay
        | '3' -> OperatorRelay
        | '4' -> UnKnown
        | c -> invalidArg (nameof c) $"Invalid AppType: %c{c}"
    static member ToChar(d: AppType) =
        match d with
        | Investor -> '1'
        | InvestorRelay -> '2'
        | OperatorRelay -> '3'
        | UnKnown -> '4'

/// TThostFtdcLoginModeType: Trade='0', Transfer='1'
type LoginMode =
    | Trade
    | Transfer
    static member FromChar(c: char) =
        match c with
        | '0' -> Trade
        | '1' -> Transfer
        | c -> invalidArg (nameof c) $"Invalid LoginMode: %c{c}"
    static member ToChar(d: LoginMode) =
        match d with
        | Trade -> '0'
        | Transfer -> '1'

/// TThostFtdcIdCardTypeType: EID='0', IDCard='1', OfficerIDCard='2', PoliceIDCard='3',
/// SoldierIDCard='4', HouseholdRegister='5', Passport='6', TaiwanCompatriotIDCard='7',
/// HomeComingCard='8', LicenseNo='9', TaxNo='A', HMMainlandTravelPermit='B',
/// TwMainlandTravelPermit='C', DrivingLicense='D', SocialID='F', LocalID='G',
/// BusinessRegistration='H', HKMCIDCard='I', AccountsPermits='J', FrgPrmtRdCard='K',
/// CptMngPrdLetter='L', HKMCTwResidencePermit='M', UniformSocialCreditCode='N',
/// CorporationCertNo='O', OtherCard='x'
type IdCardType =
    | EID
    | IDCard
    | OfficerIDCard
    | PoliceIDCard
    | SoldierIDCard
    | HouseholdRegister
    | Passport
    | TaiwanCompatriotIDCard
    | HomeComingCard
    | LicenseNo
    | TaxNo
    | HMMainlandTravelPermit
    | TwMainlandTravelPermit
    | DrivingLicense
    | SocialID
    | LocalID
    | BusinessRegistration
    | HKMCIDCard
    | AccountsPermits
    | FrgPrmtRdCard
    | CptMngPrdLetter
    | HKMCTwResidencePermit
    | UniformSocialCreditCode
    | CorporationCertNo
    | OtherCard
    static member FromChar(c: char) =
        match c with
        | '0' -> EID
        | '1' -> IDCard
        | '2' -> OfficerIDCard
        | '3' -> PoliceIDCard
        | '4' -> SoldierIDCard
        | '5' -> HouseholdRegister
        | '6' -> Passport
        | '7' -> TaiwanCompatriotIDCard
        | '8' -> HomeComingCard
        | '9' -> LicenseNo
        | 'A' -> TaxNo
        | 'B' -> HMMainlandTravelPermit
        | 'C' -> TwMainlandTravelPermit
        | 'D' -> DrivingLicense
        | 'F' -> SocialID
        | 'G' -> LocalID
        | 'H' -> BusinessRegistration
        | 'I' -> HKMCIDCard
        | 'J' -> AccountsPermits
        | 'K' -> FrgPrmtRdCard
        | 'L' -> CptMngPrdLetter
        | 'M' -> HKMCTwResidencePermit
        | 'N' -> UniformSocialCreditCode
        | 'O' -> CorporationCertNo
        | 'x' -> OtherCard
        | c -> invalidArg (nameof c) $"Invalid IdCardType: %c{c}"
    static member ToChar(d: IdCardType) =
        match d with
        | EID -> '0'
        | IDCard -> '1'
        | OfficerIDCard -> '2'
        | PoliceIDCard -> '3'
        | SoldierIDCard -> '4'
        | HouseholdRegister -> '5'
        | Passport -> '6'
        | TaiwanCompatriotIDCard -> '7'
        | HomeComingCard -> '8'
        | LicenseNo -> '9'
        | TaxNo -> 'A'
        | HMMainlandTravelPermit -> 'B'
        | TwMainlandTravelPermit -> 'C'
        | DrivingLicense -> 'D'
        | SocialID -> 'F'
        | LocalID -> 'G'
        | BusinessRegistration -> 'H'
        | HKMCIDCard -> 'I'
        | AccountsPermits -> 'J'
        | FrgPrmtRdCard -> 'K'
        | CptMngPrdLetter -> 'L'
        | HKMCTwResidencePermit -> 'M'
        | UniformSocialCreditCode -> 'N'
        | CorporationCertNo -> 'O'
        | OtherCard -> 'x'

/// TThostFtdcCustTypeType: Person='0', Institution='1'
type CustType =
    | Person
    | Institution
    static member FromChar(c: char) =
        match c with
        | '0' -> Person
        | '1' -> Institution
        | c -> invalidArg (nameof c) $"Invalid CustType: %c{c}"
    static member ToChar(d: CustType) =
        match d with
        | Person -> '0'
        | Institution -> '1'

/// TThostFtdcBankAccTypeType: BankBook='1', SavingCard='2', CreditCard='3'
type BankAccType =
    | BankBook
    | SavingCard
    | CreditCard
    static member FromChar(c: char) =
        match c with
        | '1' -> BankBook
        | '2' -> SavingCard
        | '3' -> CreditCard
        | c -> invalidArg (nameof c) $"Invalid BankAccType: %c{c}"
    static member ToChar(d: BankAccType) =
        match d with
        | BankBook -> '1'
        | SavingCard -> '2'
        | CreditCard -> '3'

/// TThostFtdcFutureAccTypeType: BankBook='1', SavingCard='2', CreditCard='3'
type FutureAccType =
    | BankBook
    | SavingCard
    | CreditCard
    static member FromChar(c: char) =
        match c with
        | '1' -> BankBook
        | '2' -> SavingCard
        | '3' -> CreditCard
        | c -> invalidArg (nameof c) $"Invalid FutureAccType: %c{c}"
    static member ToChar(d: FutureAccType) =
        match d with
        | BankBook -> '1'
        | SavingCard -> '2'
        | CreditCard -> '3'

/// TThostFtdcPwdFlagType: NoCheck='0', BlankCheck='1', EncryptCheck='2'
type PwdFlag =
    | NoCheck
    | BlankCheck
    | EncryptCheck
    static member FromChar(c: char) =
        match c with
        | '0' -> NoCheck
        | '1' -> BlankCheck
        | '2' -> EncryptCheck
        | c -> invalidArg (nameof c) $"Invalid PwdFlag: %c{c}"
    static member ToChar(d: PwdFlag) =
        match d with
        | NoCheck -> '0'
        | BlankCheck -> '1'
        | EncryptCheck -> '2'

/// TThostFtdcSecuAccTypeType: AccountID='1', CardID='2', SHStockholderID='3', SZStockholderID='4'
type SecuAccType =
    | AccountID
    | CardID
    | SHStockholderID
    | SZStockholderID
    static member FromChar(c: char) =
        match c with
        | '1' -> AccountID
        | '2' -> CardID
        | '3' -> SHStockholderID
        | '4' -> SZStockholderID
        | c -> invalidArg (nameof c) $"Invalid SecuAccType: %c{c}"
    static member ToChar(d: SecuAccType) =
        match d with
        | AccountID -> '1'
        | CardID -> '2'
        | SHStockholderID -> '3'
        | SZStockholderID -> '4'

/// TThostFtdcTransferStatusType: Normal='0', Repealed='1'
type TransferStatus =
    | Normal
    | Repealed
    static member FromChar(c: char) =
        match c with
        | '0' -> Normal
        | '1' -> Repealed
        | c -> invalidArg (nameof c) $"Invalid TransferStatus: %c{c}"
    static member ToChar(d: TransferStatus) =
        match d with
        | Normal -> '0'
        | Repealed -> '1'

/// TThostFtdcOpenOrDestroyType: Open='1', Destroy='0'
type OpenOrDestroy =
    | Open
    | Destroy
    static member FromChar(c: char) =
        match c with
        | '1' -> Open
        | '0' -> Destroy
        | c -> invalidArg (nameof c) $"Invalid OpenOrDestroy: %c{c}"
    static member ToChar(d: OpenOrDestroy) =
        match d with
        | Open -> '1'
        | Destroy -> '0'

/// TThostFtdcAvailabilityFlagType: Invalid='0', Valid='1', Repeal='2'
type AvailabilityFlag =
    | Invalid
    | Valid
    | Repeal
    static member FromChar(c: char) =
        match c with
        | '0' -> Invalid
        | '1' -> Valid
        | '2' -> Repeal
        | c -> invalidArg (nameof c) $"Invalid AvailabilityFlag: %c{c}"
    static member ToChar(d: AvailabilityFlag) =
        match d with
        | Invalid -> '0'
        | Valid -> '1'
        | Repeal -> '2'

/// TThostFtdcFeePayFlagType: BEN='0', OUR='1', SHA='2'
type FeePayFlag =
    | BEN
    | OUR
    | SHA
    static member FromChar(c: char) =
        match c with
        | '0' -> BEN
        | '1' -> OUR
        | '2' -> SHA
        | c -> invalidArg (nameof c) $"Invalid FeePayFlag: %c{c}"
    static member ToChar(d: FeePayFlag) =
        match d with
        | BEN -> '0'
        | OUR -> '1'
        | SHA -> '2'

/// TThostFtdcLastFragmentType: Yes='0', No='1'
type LastFragment =
    | Yes
    | No
    static member FromChar(c: char) =
        match c with
        | '0' -> Yes
        | '1' -> No
        | c -> invalidArg (nameof c) $"Invalid LastFragment: %c{c}"
    static member ToChar(d: LastFragment) =
        match d with
        | Yes -> '0'
        | No -> '1'

// ---- Exchange ----

/// TThostFtdcExchangePropertyType: Normal='0', GenOrderByTrade='1'
type ExchangeProperty =
    | Normal
    | GenOrderByTrade
    static member FromChar(c: char) =
        match c with
        | '0' -> Normal
        | '1' -> GenOrderByTrade
        | c -> invalidArg (nameof c) $"Invalid ExchangeProperty: %c{c}"
    static member ToChar(d: ExchangeProperty) =
        match d with
        | Normal -> '0'
        | GenOrderByTrade -> '1'

/// TThostFtdcTraderConnectStatusType: NotConnected='1', Connected='2', QryInstrumentSent='3', SubPrivateFlow='4'
type TraderConnectStatus =
    | NotConnected
    | Connected
    | QryInstrumentSent
    | SubPrivateFlow
    static member FromChar(c: char) =
        match c with
        | '1' -> NotConnected
        | '2' -> Connected
        | '3' -> QryInstrumentSent
        | '4' -> SubPrivateFlow
        | c -> invalidArg (nameof c) $"Invalid TraderConnectStatus: %c{c}"
    static member ToChar(d: TraderConnectStatus) =
        match d with
        | NotConnected -> '1'
        | Connected -> '2'
        | QryInstrumentSent -> '3'
        | SubPrivateFlow -> '4'

// ---- Business types ----

/// TThostFtdcBizTypeType: Future='1', Stock='2'
type BizType =
    | Future
    | Stock
    static member FromChar(c: char) =
        match c with
        | '1' -> Future
        | '2' -> Stock
        | c -> invalidArg (nameof c) $"Invalid BizType: %c{c}"
    static member ToChar(d: BizType) =
        match d with
        | Future -> '1'
        | Stock -> '2'

/// TThostFtdcBusinessTypeType: Request='1', Response='2', Notice='3'
type BusinessType =
    | Request
    | Response
    | Notice
    static member FromChar(c: char) =
        match c with
        | '1' -> Request
        | '2' -> Response
        | '3' -> Notice
        | c -> invalidArg (nameof c) $"Invalid BusinessType: %c{c}"
    static member ToChar(d: BusinessType) =
        match d with
        | Request -> '1'
        | Response -> '2'
        | Notice -> '3'

/// TThostFtdcInvestorRangeType: All='1', Group='2', Single='3'
type InvestorRange =
    | All
    | Group
    | Single
    static member FromChar(c: char) =
        match c with
        | '1' -> All
        | '2' -> Group
        | '3' -> Single
        | c -> invalidArg (nameof c) $"Invalid InvestorRange: %c{c}"
    static member ToChar(d: InvestorRange) =
        match d with
        | All -> '1'
        | Group -> '2'
        | Single -> '3'

// ---- Algorithm / Price type ----

/// TThostFtdcAlgorithmType: All='1', OnlyLost='2', OnlyGain='3', None='4'
type Algorithm =
    | All
    | OnlyLost
    | OnlyGain
    | NoAlgorithm
    static member FromChar(c: char) =
        match c with
        | '1' -> All
        | '2' -> OnlyLost
        | '3' -> OnlyGain
        | '4' -> NoAlgorithm
        | c -> invalidArg (nameof c) $"Invalid Algorithm: %c{c}"
    static member ToChar(d: Algorithm) =
        match d with
        | All -> '1'
        | OnlyLost -> '2'
        | OnlyGain -> '3'
        | NoAlgorithm -> '4'

/// TThostFtdcIncludeCloseProfitType: Include='0', NotInclude='2'
type IncludeCloseProfit =
    | Include
    | NotInclude
    static member FromChar(c: char) =
        match c with
        | '0' -> Include
        | '2' -> NotInclude
        | c -> invalidArg (nameof c) $"Invalid IncludeCloseProfit: %c{c}"
    static member ToChar(d: IncludeCloseProfit) =
        match d with
        | Include -> '0'
        | NotInclude -> '2'

/// TThostFtdcMarginPriceTypeType: PreSettlementPrice='1', SettlementPrice='2', AveragePrice='3', OpenPrice='4'
type MarginPriceType =
    | PreSettlementPrice
    | SettlementPrice
    | AveragePrice
    | OpenPrice
    static member FromChar(c: char) =
        match c with
        | '1' -> PreSettlementPrice
        | '2' -> SettlementPrice
        | '3' -> AveragePrice
        | '4' -> OpenPrice
        | c -> invalidArg (nameof c) $"Invalid MarginPriceType: %c{c}"
    static member ToChar(d: MarginPriceType) =
        match d with
        | PreSettlementPrice -> '1'
        | SettlementPrice -> '2'
        | AveragePrice -> '3'
        | OpenPrice -> '4'

/// TThostFtdcHandlePositionAlgoIDType: Base='1', DCE='2', CZCE='3'
type HandlePositionAlgoId =
    | Base
    | DCE
    | CZCE
    static member FromChar(c: char) =
        match c with
        | '1' -> Base
        | '2' -> DCE
        | '3' -> CZCE
        | c -> invalidArg (nameof c) $"Invalid HandlePositionAlgoId: %c{c}"
    static member ToChar(d: HandlePositionAlgoId) =
        match d with
        | Base -> '1'
        | DCE -> '2'
        | CZCE -> '3'

/// TThostFtdcFindMarginRateAlgoIDType: Base='1', DCE='2', CZCE='3'
type FindMarginRateAlgoId =
    | Base
    | DCE
    | CZCE
    static member FromChar(c: char) =
        match c with
        | '1' -> Base
        | '2' -> DCE
        | '3' -> CZCE
        | c -> invalidArg (nameof c) $"Invalid FindMarginRateAlgoId: %c{c}"
    static member ToChar(d: FindMarginRateAlgoId) =
        match d with
        | Base -> '1'
        | DCE -> '2'
        | CZCE -> '3'

/// TThostFtdcHandleTradingAccountAlgoIDType: Base='1', DCE='2', CZCE='3'
type HandleTradingAccountAlgoId =
    | Base
    | DCE
    | CZCE
    static member FromChar(c: char) =
        match c with
        | '1' -> Base
        | '2' -> DCE
        | '3' -> CZCE
        | c -> invalidArg (nameof c) $"Invalid HandleTradingAccountAlgoId: %c{c}"
    static member ToChar(d: HandleTradingAccountAlgoId) =
        match d with
        | Base -> '1'
        | DCE -> '2'
        | CZCE -> '3'

// ---- Yes/No and indicator types ----

/// TThostFtdcYesNoIndicatorType: Yes='0', No='1'
type YesNoIndicator =
    | Yes
    | No
    static member FromChar(c: char) =
        match c with
        | '0' -> Yes
        | '1' -> No
        | c -> invalidArg (nameof c) $"Invalid YesNoIndicator: %c{c}"
    static member ToChar(d: YesNoIndicator) =
        match d with
        | Yes -> '0'
        | No -> '1'

/// TThostFtdcForQuoteStatusType: Submitted='a', Accepted='b', Rejected='c'
type ForQuoteStatus =
    | Submitted
    | Accepted
    | Rejected
    static member FromChar(c: char) =
        match c with
        | 'a' -> Submitted
        | 'b' -> Accepted
        | 'c' -> Rejected
        | c -> invalidArg (nameof c) $"Invalid ForQuoteStatus: %c{c}"
    static member ToChar(d: ForQuoteStatus) =
        match d with
        | Submitted -> 'a'
        | Accepted -> 'b'
        | Rejected -> 'c'

/// TThostFtdcActionTypeType: Exec='1', Abandon='2'
type ActionType =
    | Exec
    | Abandon
    static member FromChar(c: char) =
        match c with
        | '1' -> Exec
        | '2' -> Abandon
        | c -> invalidArg (nameof c) $"Invalid ActionType: %c{c}"
    static member ToChar(d: ActionType) =
        match d with
        | Exec -> '1'
        | Abandon -> '2'

/// TThostFtdcApplySrcType: Trade='0', Member='1'
type ApplySrc =
    | Trade
    | Member
    static member FromChar(c: char) =
        match c with
        | '0' -> Trade
        | '1' -> Member
        | c -> invalidArg (nameof c) $"Invalid ApplySrc: %c{c}"
    static member ToChar(d: ApplySrc) =
        match d with
        | Trade -> '0'
        | Member -> '1'

/// TThostFtdcApplyStatusType: Unknown='a', Canceled='0', Suspended='1', Accepted='3'
type ApplyStatus =
    | Unknown
    | Canceled
    | Suspended
    | Accepted
    static member FromChar(c: char) =
        match c with
        | 'a' -> Unknown
        | '0' -> Canceled
        | '1' -> Suspended
        | '3' -> Accepted
        | c -> invalidArg (nameof c) $"Invalid ApplyStatus: %c{c}"
    static member ToChar(d: ApplyStatus) =
        match d with
        | Unknown -> 'a'
        | Canceled -> '0'
        | Suspended -> '1'
        | Accepted -> '3'

// ---- Control levels ----

/// TThostFtdcOpenLimitControlLevelType: None='0', Product='1', Inst='2'
type OpenLimitControlLevel =
    | NoLimit
    | Product
    | Inst
    static member FromChar(c: char) =
        match c with
        | '0' -> NoLimit
        | '1' -> Product
        | '2' -> Inst
        | c -> invalidArg (nameof c) $"Invalid OpenLimitControlLevel: %c{c}"
    static member ToChar(d: OpenLimitControlLevel) =
        match d with
        | NoLimit -> '0'
        | Product -> '1'
        | Inst -> '2'

/// TThostFtdcOrderFreqControlLevelType: None='0', Product='1', Inst='2'
type OrderFreqControlLevel =
    | NoControl
    | Product
    | Inst
    static member FromChar(c: char) =
        match c with
        | '0' -> NoControl
        | '1' -> Product
        | '2' -> Inst
        | c -> invalidArg (nameof c) $"Invalid OrderFreqControlLevel: %c{c}"
    static member ToChar(d: OrderFreqControlLevel) =
        match d with
        | NoControl -> '0'
        | Product -> '1'
        | Inst -> '2'

/// TThostFtdcTimeRangeType: Usual='1', FNSP='2', BNSP='3', Spot='4'
type TimeRange =
    | Usual
    | FNSP
    | BNSP
    | Spot
    static member FromChar(c: char) =
        match c with
        | '1' -> Usual
        | '2' -> FNSP
        | '3' -> BNSP
        | '4' -> Spot
        | c -> invalidArg (nameof c) $"Invalid TimeRange: %c{c}"
    static member ToChar(d: TimeRange) =
        match d with
        | Usual -> '1'
        | FNSP -> '2'
        | BNSP -> '3'
        | Spot -> '4'

/// TThostFtdcMortgageFundUseRangeType: None='0', Margin='1', All='2', CNY3='3'
type MortgageFundUseRange =
    | NoUse
    | Margin
    | All
    | CNY3
    static member FromChar(c: char) =
        match c with
        | '0' -> NoUse
        | '1' -> Margin
        | '2' -> All
        | '3' -> CNY3
        | c -> invalidArg (nameof c) $"Invalid MortgageFundUseRange: %c{c}"
    static member ToChar(d: MortgageFundUseRange) =
        match d with
        | NoUse -> '0'
        | Margin -> '1'
        | All -> '2'
        | CNY3 -> '3'
