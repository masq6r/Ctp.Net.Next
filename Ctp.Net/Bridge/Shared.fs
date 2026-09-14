namespace Ctp.Net.Next.Bridge

open System

/// FENS credentials shared by the market-data and trader APIs.
type FensUserInfoRequest =
    { BrokerId: string
      UserId: string
      LoginMode: LoginMode option }

/// The asynchronous for-quote notification emitted by the market-data API.
type ForQuoteRspResponse =
    { TradingDay: DateOnly
      Reserve1: string
      ForQuoteSysId: string
      ForQuoteTime: TimeOnly
      ActionDay: DateOnly
      ExchangeId: string
      InstrumentId: string }
