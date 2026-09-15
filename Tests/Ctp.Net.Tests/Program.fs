namespace Ctp.Net.Next.Tests

open Xunit
open System
open Ctp.Net.Next
open System.Text
open Ctp.Net.Next.Bridge
open Ctp.Net.Next.CSharp
open System.Threading
open System.Threading.Tasks
open System.Collections.Generic
open Microsoft.Extensions.Logging

module Helper =

    let waitFor predicate (timeoutMs: int) =
        Assert.True(SpinWait.SpinUntil(Func<bool>(predicate), timeoutMs))

    let assertOk result =
        match result with
        | Ok() -> ()
        | Error error -> failwith $"Expected Ok() but got {error}."

    let assertConnectError expected result =
        match expected, result with
        | ConnectError.Timeout expectedTimeout, Error(ConnectError.Timeout actualTimeout) ->
            Assert.Equal(expectedTimeout, actualTimeout)
        | ConnectError.Cancelled, Error ConnectError.Cancelled -> ()
        | ConnectError.NativeOperationFailed expectedMessage, Error(ConnectError.NativeOperationFailed actualMessage) ->
            Assert.Equal(expectedMessage, actualMessage)
        | _, Ok() -> failwith $"Expected Error({expected}) but got Ok()."
        | _, Error actual -> failwith $"Expected Error({expected}) but got Error({actual})."

type EncodingTests() =

    [<Fact>]
    member _.``outbound encoding defaults to GBK``() =
        let defaults = CtpEncodingOptions.Default

        Assert.Equal(Encoding.GetEncoding("GBK").WebName, defaults.OutboundEncoding.WebName)

    [<Fact>]
    member _.``inbound encoding defaults to GB18030``() =
        let defaults = CtpEncodingOptions.Default

        Assert.Equal(Encoding.GetEncoding("GB18030").WebName, defaults.InboundEncoding.WebName)

    [<Fact>]
    member _.``user login request keeps broker and user ids``() =
        let login = UserLoginRequest.Create("9999", "demo", "secret")
        Assert.Equal("9999", login.BrokerId)
        Assert.Equal("demo", login.UserId)

    [<Fact>]
    member _.``user logout request keeps broker and user ids``() =
        let logout = UserLogoutRequest.Create("9999", "demo")
        Assert.Equal("9999", logout.BrokerId)
        Assert.Equal("demo", logout.UserId)

type TemporalHelperTests() =

    [<Theory>]
    [<InlineData(null)>]
    [<InlineData("")>]
    [<InlineData("   ")>]
    [<InlineData("--:--:--")>]
    [<InlineData(" --:--:-- ")>]
    member _.``missing times use existing sentinels``(value: string) =
        Assert.Equal(TimeOnly.MinValue, TemporalHelpers.parseTime value)
        Assert.True((TemporalHelpers.parseTimeOption value).IsNone)
        Assert.Equal(TimeOnly.MinValue, TemporalHelpers.parseTimeWithMillis value 500)

    [<Theory>]
    [<InlineData("00:00:00", 0, 0, 0)>]
    [<InlineData(" 21:20:30 ", 21, 20, 30)>]
    member _.``valid times preserve midnight and milliseconds``(value: string, hour: int, minute: int, second: int) =
        let expected = TimeOnly(hour, minute, second)
        Assert.Equal(expected, TemporalHelpers.parseTime value)
        Assert.Equal(Some expected, TemporalHelpers.parseTimeOption value)
        Assert.Equal(expected.Add(TimeSpan.FromMilliseconds 5.), TemporalHelpers.parseTimeWithMillis value 5)

    [<Theory>]
    [<InlineData("not-a-time")>]
    [<InlineData("25:00:00")>]
    member _.``unknown malformed times remain errors``(value: string) =
        Assert.Throws<FormatException>(fun () -> TemporalHelpers.parseTime value |> ignore) |> ignore
        Assert.Throws<FormatException>(fun () -> TemporalHelpers.parseTimeOption value |> ignore) |> ignore
        Assert.Throws<FormatException>(fun () -> TemporalHelpers.parseTimeWithMillis value 0 |> ignore) |> ignore

    [<Fact>]
    member _.``login response accepts unavailable exchange times``() =
        let mutable native = Unchecked.defaultof<NativeRspUserLogin>
        native.TradingDay <- Encoding.ASCII.GetBytes("20260910")
        native.LoginTime <- Encoding.ASCII.GetBytes("21:20:30")
        native.ShfeTime <- Encoding.ASCII.GetBytes("--:--:--")
        native.DceTime <- Encoding.ASCII.GetBytes("--:--:--")
        native.CzceTime <- Encoding.ASCII.GetBytes("--:--:--")
        native.FfexTime <- Encoding.ASCII.GetBytes("--:--:--")
        native.IneTime <- Encoding.ASCII.GetBytes("--:--:--")
        native.GfexTime <- Encoding.ASCII.GetBytes("--:--:--")
        let actual = BridgeMapping.userLogin Encoding.ASCII native
        Assert.Equal(DateOnly(2026, 9, 10), actual.TradingDay)
        Assert.Equal(TimeOnly(21, 20, 30), actual.LoginTime)
        for time in [ actual.ShfeTime; actual.DceTime; actual.CzceTime; actual.FfexTime; actual.IneTime; actual.GfexTime ] do
            Assert.Equal(TimeOnly.MinValue, time)

    [<Theory>]
    [<InlineData("")>]
    [<InlineData("   ")>]
    [<InlineData("0")>]
    [<InlineData("       0")>]
    [<InlineData("00000000")>]
    member _.``missing date values use non optional sentinel``(value: string) =
        Assert.Equal(DateOnly.MinValue, TemporalHelpers.parseDate value)

    [<Theory>]
    [<InlineData("")>]
    [<InlineData("   ")>]
    [<InlineData("0")>]
    [<InlineData("       0")>]
    [<InlineData("00000000")>]
    member _.``missing date values use optional sentinel``(value: string) =
        Assert.True((TemporalHelpers.parseDateOption value).IsNone)

    [<Theory>]
    [<InlineData("20260910")>]
    [<InlineData(" 20260910 ")>]
    member _.``valid dates allow surrounding whitespace``(value: string) =
        Assert.Equal(DateOnly(2026, 9, 10), TemporalHelpers.parseDate value)

    [<Fact>]
    member _.``malformed nonzero date remains an error``() =
        Assert.Throws<FormatException>(fun () -> TemporalHelpers.parseDate "not-a-date" |> ignore)
        |> ignore

type TraderBridgeGeneratedTests() =

    let assembly = typeof<InstrumentResponse>.Assembly

    let flags =
        System.Reflection.BindingFlags.Instance
        ||| System.Reflection.BindingFlags.Public
        ||| System.Reflection.BindingFlags.NonPublic

    let staticFlags =
        System.Reflection.BindingFlags.Static
        ||| System.Reflection.BindingFlags.Public
        ||| System.Reflection.BindingFlags.NonPublic

    let getType name =
        match assembly.GetType(name, false) with
        | null -> failwith $"Expected type '{name}' to exist."
        | t -> t

    let zeroInitializeByteArrays (instance: obj) =
        let nativeType = instance.GetType()

        for field in nativeType.GetFields(flags) do
            if field.FieldType = typeof<byte array> then
                let size =
                    field.GetCustomAttributes(typeof<System.Runtime.InteropServices.MarshalAsAttribute>, false)
                    |> Array.tryHead
                    |> Option.map (fun attr -> (attr :?> System.Runtime.InteropServices.MarshalAsAttribute).SizeConst)
                    |> Option.defaultValue 0

                field.SetValue(instance, Array.zeroCreate<byte> size)

    let setFixedStringField (instance: obj) fieldName (value: string) =
        let nativeType = instance.GetType()
        let field = nativeType.GetField(fieldName, flags)

        let size =
            field.GetCustomAttributes(typeof<System.Runtime.InteropServices.MarshalAsAttribute>, false)
            |> Array.tryHead
            |> Option.map (fun attr -> (attr :?> System.Runtime.InteropServices.MarshalAsAttribute).SizeConst)
            |> Option.defaultValue 0

        let buffer = Array.zeroCreate<byte> size
        let encoded = Encoding.UTF8.GetBytes(value)
        encoded.CopyTo(buffer, 0)
        field.SetValue(instance, buffer)

    let setNativeField (instance: obj) fieldName value =
        instance.GetType().GetField(fieldName, flags).SetValue(instance, value)

    let getFixedStringField (instance: obj) fieldName =
        let nativeType = instance.GetType()
        let field = nativeType.GetField(fieldName, flags)
        let bytes = field.GetValue(instance) :?> byte array
        let zeroIndex = bytes |> Array.tryFindIndex ((=) 0uy) |> Option.defaultValue bytes.Length
        Encoding.UTF8.GetString(bytes, 0, zeroIndex)

    let generatedType = getType "Ctp.Net.Next.Bridge.TraderBridgeGenerated"

    let mapNativeAs recordType nativeTypeName (native: obj) =
        let nativeType = getType nativeTypeName
        let mapNative = generatedType.GetMethod("mapNative", staticFlags)

        mapNative.MakeGenericMethod(recordType, nativeType).Invoke(null, [| Encoding.UTF8; native |])

    let mapNativeRecord recordType nativeTypeName fields: obj =
        let qualifiedNativeTypeName = $"Ctp.Net.Next.Bridge.{nativeTypeName}"
        let nativeType = getType qualifiedNativeTypeName
        let native = Activator.CreateInstance(nativeType)
        zeroInitializeByteArrays native

        for fieldName, value in fields do
            setNativeField native fieldName value

        mapNativeAs recordType qualifiedNativeTypeName native

    let buildNativeAs recordType nativeTypeName (record: obj) =
        let nativeType = getType nativeTypeName
        let buildNative = generatedType.GetMethod("buildNative", staticFlags)
        buildNative.MakeGenericMethod(recordType, nativeType).Invoke(null, [| Encoding.UTF8; record |])

    let mapInstrument fields =
        let nativeType = getType "Ctp.Net.Next.Bridge.NativeInstrument"
        let native = Activator.CreateInstance(nativeType)
        zeroInitializeByteArrays native

        for name, value in fields do
            let field = nativeType.GetField(name, flags)
            field.SetValue(native, byte value)

        mapNativeAs typeof<InstrumentResponse> "Ctp.Net.Next.Bridge.NativeInstrument" native :?> InstrumentResponse

    [<Fact>]
    member _.``instrument mapping treats zero date sentinels as missing``() =
        let nativeType = getType "Ctp.Net.Next.Bridge.NativeInstrument"
        let native = Activator.CreateInstance(nativeType)
        zeroInitializeByteArrays native

        for fieldName in [ "CreateDate"; "OpenDate"; "ExpireDate"; "StartDelivDate"; "EndDelivDate" ] do
            setFixedStringField native fieldName "       0"

        let instrument =
            mapNativeAs typeof<InstrumentResponse> "Ctp.Net.Next.Bridge.NativeInstrument" native
            :?> InstrumentResponse

        Assert.Equal(DateOnly.MinValue, instrument.CreateDate)
        Assert.Equal(DateOnly.MinValue, instrument.OpenDate)
        Assert.Equal(DateOnly.MinValue, instrument.ExpireDate)
        Assert.Equal(DateOnly.MinValue, instrument.StartDelivDate)
        Assert.Equal(DateOnly.MinValue, instrument.EndDelivDate)

    [<Fact>]
    member _.``instrument mapping supports optional union fields``() =
        let instrument =
            mapInstrument
                [ "ProductClass", '1'
                  "InstLifePhase", '1'
                  "PositionType", '1'
                  "PositionDateType", '1'
                  "MaxMarginSideAlgorithm", '1'
                  "OptionsType", '1'
                  "CombinationType", '3' ]

        Assert.Equal(Some ProductClass.Futures, instrument.ProductClass)
        Assert.Equal(Some InstLifePhase.Started, instrument.InstLifePhase)
        Assert.Equal(Some PositionType.Net, instrument.PositionType)
        Assert.Equal(Some PositionDateType.UseHistory, instrument.PositionDateType)
        Assert.Equal(Some MaxMarginSideAlgorithm.Yes, instrument.MaxMarginSideAlgorithm)
        Assert.Equal(Some OptionsType.CallOptions, instrument.OptionsType)
        Assert.Equal(Some CombinationType.STD, instrument.CombinationType)

    [<Fact>]
    member _.``instrument mapping treats invalid optional union values as none``() =
        let instrument = mapInstrument [ "CombinationType", '0' ]

        Assert.True(instrument.CombinationType.IsNone)

    [<Fact>]
    member _.``generated mapping parses dateonly timeonly and millisec fields``() =
        let nativeType = getType "Ctp.Net.Next.Bridge.NativeTraderDepthMarketData"
        let native = Activator.CreateInstance(nativeType)
        zeroInitializeByteArrays native
        setFixedStringField native "TradingDay" "20260519"
        setFixedStringField native "UpdateTime" "13:14:15"
        setFixedStringField native "ActionDay" "20260520"
        nativeType.GetField("UpdateMillisec", flags).SetValue(native, 789)

        let depth =
            mapNativeAs typeof<DepthMarketData> "Ctp.Net.Next.Bridge.NativeTraderDepthMarketData" native :?> DepthMarketData

        Assert.Equal(DateOnly(2026, 5, 19), depth.TradingDay)
        Assert.Equal(TimeOnly(13, 14, 15, 789), depth.UpdateTime)
        Assert.Equal(DateOnly(2026, 5, 20), depth.ActionDay)

    [<Fact>]
    member _.``generated builder encodes dateonly fields``() =
        let request: QrySettlementInfoRequest =
            { BrokerId = "9999"
              InvestorId = "demo"
              TradingDay = DateOnly(2026, 5, 19)
              AccountId = None
              CurrencyId = None }

        let native =
            buildNativeAs typeof<QrySettlementInfoRequest> "Ctp.Net.Next.Bridge.NativeQrySettlementInfo" (box request)

        Assert.Equal("20260519", getFixedStringField native "TradingDay")

    [<Fact>]
    member _.``generated builder encodes timeonly fields``() =
        let request: UserSystemInfoRequest =
            { BrokerId = "9999"
              UserId = "demo"
              ClientSystemInfoLen = 0
              ClientSystemInfo = ""
              Reserve1 = ""
              ClientIpPort = 0
              ClientLoginTime = TimeOnly(1, 2, 3, 456)
              ClientAppId = ""
              ClientPublicIp = ""
              ClientLoginRemark = ""
              Mac = "" }

        let native =
            buildNativeAs typeof<UserSystemInfoRequest> "Ctp.Net.Next.Bridge.NativeUserSystemInfo" (box request)

        Assert.Equal("01:02:03", getFixedStringField native "ClientLoginTime")

    [<Fact>]
    member _.``instrument status callback payload maps typed status and time``() =
        let nativeType = getType "Ctp.Net.Next.Bridge.NativeInstrumentStatus"
        let native = Activator.CreateInstance(nativeType)
        zeroInitializeByteArrays native
        setFixedStringField native "ExchangeId" "SHFE"
        setFixedStringField native "InstrumentId" "ag2612"
        setFixedStringField native "EnterTime" "09:00:00"
        nativeType.GetField("InstrumentStatus", flags).SetValue(native, byte '2')
        nativeType.GetField("EnterReason", flags).SetValue(native, byte '1')
        nativeType.GetField("TradingSegmentSN", flags).SetValue(native, 3)

        let status =
            mapNativeAs typeof<InstrumentStatusResponse> "Ctp.Net.Next.Bridge.NativeInstrumentStatus" native
            :?> InstrumentStatusResponse

        Assert.Equal("SHFE", status.ExchangeId)
        Assert.Equal("ag2612", status.InstrumentId)
        Assert.Equal(Some InstrumentStatus.Continuous, status.InstrumentStatus)
        Assert.Equal(Some InstStatusEnterReason.Automatic, status.EnterReason)
        Assert.Equal(TimeOnly(9, 0), status.EnterTime)
        Assert.Equal(3, status.TradingSegmentSN)

    [<Fact>]
    member _.``all new trader callback payloads support generated mapping``() =
        let cases =
            [ typeof<InstrumentStatusResponse>, "NativeInstrumentStatus"
              typeof<BulletinResponse>, "NativeBulletin"
              typeof<TradingNoticeInfoResponse>, "NativeTradingNoticeInfo"
              typeof<ErrorConditionalOrderResponse>, "NativeErrorConditionalOrder"
              typeof<CfmmcTradingAccountTokenResponse>, "NativeCFMMCTradingAccountToken"
              typeof<RepealRequest>, "NativeReqRepeal"
              typeof<RepealResponse>, "NativeRspRepeal"
              typeof<OpenAccountResponse>, "NativeOpenAccount"
              typeof<CancelAccountResponse>, "NativeCancelAccount"
              typeof<ChangeAccountResponse>, "NativeChangeAccount" ]

        for recordType, nativeTypeName in cases do
            let nativeType = getType $"Ctp.Net.Next.Bridge.{nativeTypeName}"
            let native = Activator.CreateInstance(nativeType)
            zeroInitializeByteArrays native

            for field in Microsoft.FSharp.Reflection.FSharpType.GetRecordFields(recordType) do
                if field.PropertyType = typeof<DateOnly> then
                    setFixedStringField native field.Name "20260908"
                elif field.PropertyType = typeof<TimeOnly> then
                    setFixedStringField native field.Name "09:08:07"

            let mapped = mapNativeAs recordType $"Ctp.Net.Next.Bridge.{nativeTypeName}" native
            Assert.NotNull(mapped)

    [<Fact>]
    member _.``trader callback surface includes every previously missing callback``() =
        let expected =
            set
                [ "RtnInstrumentStatus"
                  "RtnBulletin"
                  "RtnTradingNotice"
                  "RtnErrorConditionalOrder"
                  "RtnCfmmcTradingAccountToken"
                  "RtnFromBankToFutureByBank"
                  "RtnFromFutureToBankByBank"
                  "RtnRepealFromBankToFutureByBank"
                  "RtnRepealFromFutureToBankByBank"
                  "RtnRepealFromBankToFutureByFutureManual"
                  "RtnRepealFromFutureToBankByFutureManual"
                  "RtnRepealFromBankToFutureByFuture"
                  "RtnRepealFromFutureToBankByFuture"
                  "RtnOpenAccountByBank"
                  "RtnCancelAccountByBank"
                  "RtnChangeAccountByBank"
                  "ErrRtnRepealBankToFutureByFutureManual"
                  "ErrRtnRepealFutureToBankByFutureManual" ]

        let fields =
            Microsoft.FSharp.Reflection.FSharpType.GetRecordFields typeof<TraderCallbacks>
            |> Array.map _.Name
            |> Set.ofArray

        Assert.True(Set.isSubset expected fields)

    [<Fact>]
    member _.``new trader callback enums round trip official values``() =
        Assert.Equal('7', InstrumentStatus.ToChar(InstrumentStatus.FromChar '7'))
        Assert.Equal('3', InstStatusEnterReason.ToChar(InstStatusEnterReason.FromChar '3'))
        Assert.Equal('2', Gender.ToChar(Gender.FromChar '2'))
        Assert.Equal('1', MoneyAccountStatus.ToChar(MoneyAccountStatus.FromChar '1'))
        Assert.Equal('2', CashExchangeCode.ToChar(CashExchangeCode.FromChar '2'))
        Assert.Equal('2', BankRepealFlag.ToChar(BankRepealFlag.FromChar '2'))
        Assert.Equal('2', BrokerRepealFlag.ToChar(BrokerRepealFlag.FromChar '2'))

    [<Fact>]
    member _.``resume type values and defaults match the SDK``() =
        Assert.Equal(0, int ResumeType.Restart)
        Assert.Equal(1, int ResumeType.Resume)
        Assert.Equal(2, int ResumeType.Quick)
        Assert.Equal(3, int ResumeType.None)
        Assert.Equal(4, int ResumeType.ResumeFromSeqNo)
        Assert.Equal(ResumeType.Restart, ResumeTypeValidation.privateDefault)
        Assert.Equal(ResumeType.Restart, ResumeTypeValidation.publicDefault)

    [<Fact>]
    member _.``resume type validation enforces private and public SDK sets``() =
        [ ResumeType.Restart; ResumeType.Resume; ResumeType.Quick; ResumeType.ResumeFromSeqNo ]
        |> List.iter ResumeTypeValidation.validatePrivate

        [ ResumeType.Restart; ResumeType.Resume; ResumeType.Quick; ResumeType.None ]
        |> List.iter ResumeTypeValidation.validatePublic

        Assert.Throws<ArgumentException>(fun () -> ResumeTypeValidation.validatePrivate ResumeType.None)
        |> ignore

        Assert.Throws<ArgumentException>(fun () -> ResumeTypeValidation.validatePublic ResumeType.ResumeFromSeqNo)
        |> ignore

        Assert.Throws<ArgumentException>(fun () -> ResumeTypeValidation.validatePrivate (enum<ResumeType> 99))
        |> ignore

        Assert.Throws<ArgumentException>(fun () -> ResumeTypeValidation.validatePublic (enum<ResumeType> -1))
        |> ignore

    [<Fact>]
    member _.``new character enums round trip every official value``() =
        [ '1'; '2'; '3' ]
        |> List.iter (fun value -> Assert.Equal(value, ParkedOrderStatus.ToChar(ParkedOrderStatus.FromChar value)))

        [ '0'; '1' ]
        |> List.iter (fun value -> Assert.Equal(value, OrderSource.ToChar(OrderSource.FromChar value)))

        [ '0'; '1'; '2'; '3'; '4'; '5'; '6'; '7' ]
        |> List.iter (fun value -> Assert.Equal(value, OrderType.ToChar(OrderType.FromChar value)))

    [<Fact>]
    member _.``new optional character enums map invalid values to none``() =
        let parked =
            mapNativeRecord typeof<ParkedOrder> "NativeParkedOrder" [ "Status", box (byte 'x') ]
            :?> ParkedOrder

        let order =
            mapNativeRecord
                typeof<ErrorConditionalOrderResponse>
                "NativeErrorConditionalOrder"
                [ "OrderSource", box (byte 'x'); "OrderType", box (byte 'x') ]
            :?> ErrorConditionalOrderResponse

        Assert.True(parked.Status.IsNone)
        Assert.True(order.OrderSource.IsNone)
        Assert.True(order.OrderType.IsNone)

    [<Fact>]
    member _.``generated mapping applies existing character enums to remaining fields``() =
        let brokerParams =
            mapNativeRecord
                typeof<BrokerTradingParamsResponse>
                "NativeBrokerTradingParams"
                [ "AvailIncludeCloseProfit", box (byte '2') ]
            :?> BrokerTradingParamsResponse

        let combAction =
            mapNativeRecord
                typeof<CombActionResponse>
                "NativeCombAction"
                [ "ActionStatus", box (byte 'b'); "HedgeFlag", box (byte '3') ]
            :?> CombActionResponse

        let quote =
            mapNativeRecord
                typeof<QuoteResponse>
                "NativeQuote"
                [ "AskOffsetFlag", box (byte '3')
                  "BidOffsetFlag", box (byte '4')
                  "AskHedgeFlag", box (byte '1')
                  "BidHedgeFlag", box (byte '2')
                  "QuoteStatus", box (byte '5') ]
            :?> QuoteResponse

        let investor =
            mapNativeRecord
                typeof<InvestorResponse>
                "NativeInvestor"
                [ "IdentifiedCardType", box (byte '1') ]
            :?> InvestorResponse

        let transfer =
            mapNativeRecord
                typeof<TransferRequest>
                "NativeReqTransfer"
                [ "VerifyCertNoFlag", box (byte '0')
                  "BankAccType", box (byte '2')
                  "BankSecuAccType", box (byte '3')
                  "BankPwdFlag", box (byte '1')
                  "SecuPwdFlag", box (byte '2') ]
            :?> TransferRequest

        Assert.Equal(Some IncludeCloseProfit.NotInclude, brokerParams.AvailIncludeCloseProfit)
        Assert.Equal(Some OrderActionStatus.Accepted, combAction.ActionStatus)
        Assert.Equal(Some HedgeFlag.Hedge, combAction.HedgeFlag)
        Assert.Equal(Some OffsetFlag.CloseToday, quote.AskOffsetFlag)
        Assert.Equal(Some OffsetFlag.CloseYesterday, quote.BidOffsetFlag)
        Assert.Equal(Some HedgeFlag.Speculation, quote.AskHedgeFlag)
        Assert.Equal(Some HedgeFlag.Arbitrage, quote.BidHedgeFlag)
        Assert.Equal(Some OrderStatus.Canceled, quote.QuoteStatus)
        Assert.Equal(Some IdCardType.IDCard, investor.IdentifiedCardType)
        Assert.Equal(Some YesNoIndicator.Yes, transfer.VerifyCertNoFlag)
        Assert.Equal(Some BankAccType.SavingCard, transfer.BankAccType)
        Assert.Equal(Some BankAccType.CreditCard, transfer.BankSecuAccType)
        Assert.Equal(Some PwdFlag.BlankCheck, transfer.BankPwdFlag)
        Assert.Equal(Some PwdFlag.EncryptCheck, transfer.SecuPwdFlag)

    [<Fact>]
    member _.``generated mapping decodes C bool and enum bool semantics``() =
        let mapInvestor isActive isOrderFreq isOpenVolLimit =
            mapNativeRecord
                typeof<InvestorResponse>
                "NativeInvestor"
                [ "IsActive", box isActive
                  "IsOrderFreq", box isOrderFreq
                  "IsOpenVolLimit", box isOpenVolLimit ]
            :?> InvestorResponse

        let missing = mapInvestor 0 0uy (byte 'x')
        let falseValue = mapInvestor 1 (byte '0') (byte '0')
        let trueValue = mapInvestor -7 (byte '1') (byte '1')

        Assert.False(missing.IsActive)
        Assert.True(missing.IsOrderFreq.IsNone)
        Assert.True(missing.IsOpenVolLimit.IsNone)
        Assert.True(falseValue.IsActive)
        Assert.Equal(Some false, falseValue.IsOrderFreq)
        Assert.Equal(Some false, falseValue.IsOpenVolLimit)
        Assert.True(trueValue.IsActive)
        Assert.Equal(Some true, trueValue.IsOrderFreq)
        Assert.Equal(Some true, trueValue.IsOpenVolLimit)

    [<Fact>]
    member _.``generated builder encodes bool values as native zero and one``() =
        let request isOffset: InputOffsetSettingRequest =
            { BrokerId = "9999"
              InvestorId = "demo"
              InstrumentId = "ag2612"
              UnderlyingInstrId = ""
              ProductId = "ag"
              OffsetType = OffsetType.OptOffset
              Volume = 1
              IsOffset = isOffset
              RequestId = 0
              UserId = "demo"
              ExchangeId = "SHFE"
              IpAddress = None
              MacAddress = None }

        let getEncoded value =
            let native =
                buildNativeAs
                    typeof<InputOffsetSettingRequest>
                    "Ctp.Net.Next.Bridge.NativeInputOffsetSetting"
                    (box (request value))

            native.GetType().GetField("IsOffset", flags).GetValue(native) :?> int

        Assert.Equal(0, getEncoded false)
        Assert.Equal(1, getEncoded true)

    [<Fact>]
    member _.``public API exposes strong types for changed fields and resume arguments``() =
        let expectedFields =
            [ typeof<BrokerTradingParamsResponse>, "AvailIncludeCloseProfit", typeof<IncludeCloseProfit option>
              typeof<CombActionResponse>, "ActionStatus", typeof<OrderActionStatus option>
              typeof<InputQuoteRequest>, "AskOffsetFlag", typeof<OffsetFlag>
              typeof<InputQuoteRequest>, "AskHedgeFlag", typeof<HedgeFlag option>
              typeof<InvestorResponse>, "IdentifiedCardType", typeof<IdCardType option>
              typeof<InvestorResponse>, "IsActive", typeof<bool>
              typeof<InvestorResponse>, "IsOrderFreq", typeof<bool option>
              typeof<TransferRequest>, "VerifyCertNoFlag", typeof<YesNoIndicator option>
              typeof<TransferRequest>, "BankSecuAccType", typeof<BankAccType option>
              typeof<TransferRequest>, "BankPwdFlag", typeof<PwdFlag option>
              typeof<ParkedOrder>, "Status", typeof<ParkedOrderStatus option>
              typeof<ErrorConditionalOrderResponse>, "OrderSource", typeof<OrderSource option>
              typeof<ErrorConditionalOrderResponse>, "OrderType", typeof<OrderType option>
              typeof<ErrorConditionalOrderResponse>, "IsSwapOrder", typeof<bool> ]

        for recordType, fieldName, expectedType in expectedFields do
            let field =
                Microsoft.FSharp.Reflection.FSharpType.GetRecordFields(recordType)
                |> Array.find (fun field -> field.Name = fieldName)

            Assert.Equal(expectedType, field.PropertyType)

        let traderApiType = getType "Ctp.Net.Next.Bridge.TraderApi"
        let privateTopic = traderApiType.GetMethod("SubscribePrivateTopic", flags)
        let publicTopic = traderApiType.GetMethod("SubscribePublicTopic", flags)
        Assert.Equal(typeof<ResumeType>, privateTopic.GetParameters().[0].ParameterType)
        Assert.Equal(typeof<ResumeType>, publicTopic.GetParameters().[0].ParameterType)

        let csharpConstructor =
            typeof<Ctp.Net.Next.CSharp.TraderClient>.GetConstructors()
            |> Array.find (fun ctor -> ctor.GetParameters().Length = 7)

        Assert.Equal(typeof<Nullable<ResumeType>>, csharpConstructor.GetParameters().[2].ParameterType)
        Assert.Equal(typeof<Nullable<ResumeType>>, csharpConstructor.GetParameters().[4].ParameterType)


type OptionHelperTests() =

    [<Fact>]
    member _.``options create login request with broker user and password``() =
        let options = CtpOptions.Create("tcp://front", "9999", "demo", "secret")
        let login = OptionHelpers.createUserLoginRequest options

        Assert.Equal("9999", login.BrokerId)
        Assert.Equal("demo", login.UserId)
        Assert.Equal("secret", login.Password)
        Assert.Equal("", login.UserProductInfo)

    [<Fact>]
    member _.``options login request keeps user product info``() =
        let options = CtpOptions.Create("tcp://front", "9999", "demo", "secret", userProductInfo = "strategy")
        let login = OptionHelpers.createUserLoginRequest options

        Assert.Equal("strategy", login.UserProductInfo)

    [<Fact>]
    member _.``options create logout request with broker and user ids``() =
        let options = CtpOptions.Create("tcp://front", "9999", "demo", "secret")
        let logout = OptionHelpers.createUserLogoutRequest options

        Assert.Equal("9999", logout.BrokerId)
        Assert.Equal("demo", logout.UserId)

    [<Fact>]
    member _.``options create authenticate request with user product app id and auth code``() =
        let options =
            CtpOptions.Create(
                "tcp://front",
                "9999",
                "demo",
                "secret",
                userProductInfo = "strategy",
                appId = "app",
                authCode = "auth"
            )

        let request = OptionHelpers.createAuthenticateRequest options

        Assert.Equal("9999", request.BrokerId)
        Assert.Equal("demo", request.UserId)
        Assert.Equal("strategy", request.UserProductInfo.Value)
        Assert.Equal("app", request.AppId)
        Assert.Equal("auth", request.AuthCode)

    [<Fact>]
    member _.``options create authenticate request with empty defaults``() =
        let options = CtpOptions.Create("tcp://front", "9999", "demo", "secret")
        let request = OptionHelpers.createAuthenticateRequest options

        Assert.True(request.UserProductInfo.IsNone)
        Assert.Equal("", request.AppId)
        Assert.Equal("", request.AuthCode)

    [<Fact>]
    member _.``options create settlement confirm request with broker and investor ids``() =
        let options = CtpOptions.Create("tcp://front", "9999", "demo", "secret")
        let request = OptionHelpers.createSettlementInfoConfirmRequest options

        Assert.Equal("9999", request.BrokerId)
        Assert.Equal("demo", request.InvestorId)
        Assert.Equal(DateOnly.MinValue, request.ConfirmDate)
        Assert.Equal(TimeOnly.MinValue, request.ConfirmTime)
        Assert.Equal(0, request.SettlementId)
        Assert.Equal("", request.AccountId)
        Assert.Equal("", request.CurrencyId)


type NumericHelperTests() =

    [<Fact>]
    member _.``float in decimal range converts successfully``() =
        let actual = NumericHelpers.tryDecimal 123.45

        Assert.Equal(Some 123.45m, actual)

    [<Fact>]
    member _.``float above decimal max returns none``() =
        let actual = NumericHelpers.tryDecimal Double.MaxValue

        Assert.Equal(None, actual)

    [<Fact>]
    member _.``nan returns none``() =
        let actual = NumericHelpers.tryDecimal Double.NaN

        Assert.Equal(None, actual)

    [<Fact>]
    member _.``infinity returns none``() =
        let actual = NumericHelpers.tryDecimal Double.PositiveInfinity

        Assert.Equal(None, actual)

    [<Fact>]
    member _.``price above decimal max returns invalid sentinel``() =
        let actual = NumericHelpers.priceOrInvalid Double.MaxValue

        Assert.Equal(-1m, actual)

    [<Fact>]
    member _.``nan price returns invalid sentinel``() =
        let actual = NumericHelpers.priceOrInvalid Double.NaN

        Assert.Equal(-1m, actual)

    [<Theory>]
    [<InlineData(Double.PositiveInfinity)>]
    [<InlineData(Double.NegativeInfinity)>]
    member _.``infinite price returns invalid sentinel``(value: float) =
        let actual = NumericHelpers.priceOrInvalid value

        Assert.Equal(-1m, actual)


type SinglePendingResultTests() =

    [<Fact>]
    member _.``pending result completes when set``() =
        let pending = SinglePendingResult<int>()
        let completion = pending.Begin()

        pending.TrySetResult 42

        Assert.True(completion.Task.Wait(1000))
        Assert.Equal(42, completion.Task.Result)

    [<Fact>]
    member _.``only one pending operation is allowed``() =
        let pending = SinglePendingResult<int>()
        pending.Begin() |> ignore

        Assert.Throws<InvalidOperationException>(fun () -> pending.Begin() |> ignore)
        |> ignore

    [<Fact>]
    member _.``taken pending is cleared``() =
        let pending = SinglePendingResult<int>()
        let completion = pending.Begin()

        let taken = pending.TryTake()

        Assert.True(taken.IsSome)
        Assert.Same(completion, taken.Value)
        Assert.True((pending.TryTake()).IsNone)

    [<Fact>]
    member _.``completed pending allows next operation``() =
        let pending = SinglePendingResult<int>()
        let first = pending.Begin()

        pending.TrySetResult 1

        Assert.True(first.Task.Wait(1000))
        Assert.Equal(1, first.Task.Result)

        let second = pending.Begin()

        pending.TrySetResult 2

        Assert.True(second.Task.Wait(1000))
        Assert.Equal(2, second.Task.Result)


type SinglePendingRequestTests() =

    [<Fact>]
    member _.``pending request completes with mapped request``() =
        let pending = SinglePendingRequest<string list, Result<string list, RspInfo>>()
        let completion = pending.Begin([ "au2506"; "ag2506" ])

        pending.TrySetResultFromRequest Ok

        Assert.True(completion.Task.Wait(1000))

        match completion.Task.Result with
        | Ok value -> Assert.Equal<string list>([ "au2506"; "ag2506" ], value)
        | Error error -> failwith $"Expected Ok but got Error({error})."

    [<Fact>]
    member _.``pending request can complete with explicit error``() =
        let pending = SinglePendingRequest<string list, Result<string list, RspInfo>>()
        let completion = pending.Begin([ "au2506" ])
        let error = ClientHelpers.apiReturnError 7

        pending.TrySetResult(Error error)

        Assert.True(completion.Task.Wait(1000))

        match completion.Task.Result with
        | Error actual -> Assert.Equal(error.ErrorId, actual.ErrorId)
        | Ok value -> failwith $"Expected error but got Ok({value})."

    [<Fact>]
    member _.``only one pending request is allowed``() =
        let pending = SinglePendingRequest<string list, Result<string list, RspInfo>>()
        pending.Begin([ "au2506" ]) |> ignore

        Assert.Throws<InvalidOperationException>(fun () -> pending.Begin([ "ag2506" ]) |> ignore)
        |> ignore


type PendingQueryDictTests() =

    [<Fact>]
    member _.``stream until last accumulates all responses``() =
        let pending = PendingQueryDict()
        let completion = ClientHelpers.createCompletionSource<Result<int list, RspInfo>> ()

        pending.Register(1, "QueryNumbers", completion)
        pending.TryHandleResponse(1, Some(box 1), None, false, PendingResponseCompletionPolicy.StreamUntilLast)

        Assert.False(completion.Task.IsCompleted)

        pending.TryHandleResponse(1, Some(box 2), None, true, PendingResponseCompletionPolicy.StreamUntilLast)

        Assert.True(completion.Task.Wait(1000))

        match completion.Task.Result with
        | Ok values -> Assert.Equal<int list>([ 1; 2 ], values)
        | Error error -> failwith $"Expected Ok but got Error({error})."

    [<Fact>]
    member _.``final only ignores non final responses``() =
        let pending = PendingQueryDict()
        let completion = ClientHelpers.createCompletionSource<Result<int list, RspInfo>> ()

        pending.Register(1, "Login", completion)
        pending.TryHandleResponse(1, Some(box 1), None, false, PendingResponseCompletionPolicy.FinalOnly)

        Assert.False(completion.Task.IsCompleted)

        pending.TryHandleResponse(1, Some(box 2), None, true, PendingResponseCompletionPolicy.FinalOnly)

        Assert.True(completion.Task.Wait(1000))

        match completion.Task.Result with
        | Ok values -> Assert.Equal<int list>([ 2 ], values)
        | Error error -> failwith $"Expected Ok but got Error({error})."

    [<Fact>]
    member _.``final only returns error from final response``() =
        let pending = PendingQueryDict()
        let completion = ClientHelpers.createCompletionSource<Result<int list, RspInfo>> ()
        let error = ClientHelpers.apiReturnError 7

        pending.Register(1, "Authenticate", completion)
        pending.TryHandleResponse(1, Some(box 1), Some error, true, PendingResponseCompletionPolicy.FinalOnly)

        Assert.True(completion.Task.Wait(1000))

        match completion.Task.Result with
        | Error actual -> Assert.Equal(error.ErrorId, actual.ErrorId)
        | Ok value -> failwith $"Expected Error but got Ok({value})."

    [<Fact>]
    member _.``rsp error failure clears pending request``() =
        let pending = PendingQueryDict()
        let completion = ClientHelpers.createCompletionSource<Result<int list, RspInfo>> ()
        let error = ClientHelpers.apiReturnError 9

        pending.Register(1, "QueryNumbers", completion)
        pending.TryHandleResponse(1, Some(box 1), None, false, PendingResponseCompletionPolicy.StreamUntilLast)
        pending.TryFail(1, error)
        pending.TryHandleResponse(1, Some(box 2), None, true, PendingResponseCompletionPolicy.StreamUntilLast)

        Assert.True(completion.Task.Wait(1000))

        match completion.Task.Result with
        | Error actual -> Assert.Equal(error.ErrorId, actual.ErrorId)
        | Ok value -> failwith $"Expected Error but got Ok({value})."


type FlowControlTests() =

    [<Fact>]
    member _.``await task honors async cancellation``() =
        let completion = TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously)
        use cts = new CancellationTokenSource()
        let task = Async.StartAsTask(ClientHelpers.awaitTask completion.Task, cancellationToken = cts.Token)

        cts.CancelAfter 50

        Assert.ThrowsAny<OperationCanceledException>(fun () -> task.GetAwaiter().GetResult() |> ignore)
        |> ignore

    [<Fact>]
    member _.``await task with explicit cancellation honors provided token``() =
        let completion = TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously)
        use cts = new CancellationTokenSource()
        let task = Async.StartAsTask(ClientHelpers.awaitTaskWithCancellation cts.Token completion.Task)

        cts.CancelAfter 50

        Assert.ThrowsAny<OperationCanceledException>(fun () -> task.GetAwaiter().GetResult() |> ignore)
        |> ignore

    [<Fact>]
    member _.``query completion timeout returns synthetic timeout and clears pending request``() =
        let options =
            { CtpFlowControlOptions.Default with
                QueryCompletionTimeout = TimeSpan.FromMilliseconds 50.0 }

        let flow = FlowController options
        let pending = PendingQueryDict()
        let completion = ClientHelpers.createCompletionSource<Result<int list, RspInfo>> ()

        pending.Register(42, "QueryNumbers", completion)

        let result =
            flow.AwaitQueryCompletionAsync("QueryNumbers", 42, pending, completion.Task).GetAwaiter().GetResult()

        match result with
        | Error error -> Assert.Equal(-10001, error.ErrorId)
        | Ok value -> failwith $"Expected timeout error but got Ok({value})."

        pending.TryHandleResponse(42, Some(box 1), None, true, PendingResponseCompletionPolicy.StreamUntilLast)
        Assert.False(completion.Task.IsCompleted)

    [<Fact>]
    member _.``subscription batching uses configured batch size``() =
        let options = { CtpFlowControlOptions.Default with SubscriptionBatchSize = 3 }

        let flow = FlowController options

        let batches =
            flow.BatchSubscriptions([ "au2506"; "ag2506"; "cu2506"; "zn2506"; "al2506"; "ni2506"; "sn2506" ])

        Assert.Equal<string list list>(
            [ [ "au2506"; "ag2506"; "cu2506" ]
              [ "zn2506"; "al2506"; "ni2506" ]
              [ "sn2506" ] ],
            batches
        )

    [<Fact>]
    member _.``retryable error predicates match flow control policy``() =
        let flow = FlowController CtpFlowControlOptions.Default
        let retryableQuery = ClientHelpers.apiReturnError 90
        let nonRetryableQuery = ClientHelpers.apiReturnError 7

        Assert.True(flow.ShouldRetryNativeReturnCode(0, -2))
        Assert.True(flow.ShouldRetryNativeReturnCode(0, -3))
        Assert.False(flow.ShouldRetryNativeReturnCode(0, -1))
        Assert.True(flow.ShouldRetryQueryError(0, retryableQuery))
        Assert.False(flow.ShouldRetryQueryError(0, nonRetryableQuery))

    [<Fact>]
    member _.``retryable predicates stop after configured retry budget``() =
        let flow =
            FlowController(
                { CtpFlowControlOptions.Default with
                    MaxNativeReturnCodeRetries = 1
                    MaxQueryRspErrorRetries = 1 }
            )

        let retryableQuery = ClientHelpers.apiReturnError 90

        Assert.True(flow.ShouldRetryNativeReturnCode(0, -2))
        Assert.False(flow.ShouldRetryNativeReturnCode(1, -2))
        Assert.True(flow.ShouldRetryQueryError(0, retryableQuery))
        Assert.False(flow.ShouldRetryQueryError(1, retryableQuery))

    [<Fact>]
    member _.``dispatch gate throttles consecutive sends``() =
        let flow = FlowController({ CtpFlowControlOptions.Default with MaxDispatchesPerSecond = 20 })

        let stopwatch = Diagnostics.Stopwatch.StartNew()
        flow.AwaitDispatchAsync().GetAwaiter().GetResult()
        let afterFirst = stopwatch.Elapsed
        flow.AwaitDispatchAsync().GetAwaiter().GetResult()
        let afterSecond = stopwatch.Elapsed

        Assert.True(afterSecond - afterFirst >= TimeSpan.FromMilliseconds 30.0)

    [<Fact>]
    member _.``dispatch wait honors cancellation while throttled``() =
        let flow = FlowController({ CtpFlowControlOptions.Default with MaxDispatchesPerSecond = 1 })

        flow.AwaitDispatchAsync().GetAwaiter().GetResult()

        use cts = new CancellationTokenSource()
        cts.CancelAfter 50

        let blocked = flow.AwaitDispatchAsync(cancellationToken = cts.Token)

        Assert.ThrowsAny<OperationCanceledException>(fun () -> blocked.GetAwaiter().GetResult() |> ignore)
        |> ignore

    [<Fact>]
    member _.``query execution gate serializes callers``() =
        let flow = FlowController CtpFlowControlOptions.Default
        let firstLease = flow.AcquireQueryExecutionAsync().GetAwaiter().GetResult()
        let secondLeaseTask = flow.AcquireQueryExecutionAsync()

        Assert.False(secondLeaseTask.Wait(50))

        firstLease.Dispose()

        Assert.True(secondLeaseTask.Wait(1000))
        use secondLease = secondLeaseTask.Result
        Assert.NotNull(secondLease)

    [<Fact>]
    member _.``query execution wait honors cancellation``() =
        let flow = FlowController CtpFlowControlOptions.Default
        use firstLease = flow.AcquireQueryExecutionAsync().GetAwaiter().GetResult()
        use cts = new CancellationTokenSource()
        cts.CancelAfter 50

        let blocked = flow.AcquireQueryExecutionAsync(cancellationToken = cts.Token)

        Assert.ThrowsAny<OperationCanceledException>(fun () -> blocked.GetAwaiter().GetResult() |> ignore)
        |> ignore

    [<Fact>]
    member _.``query completion returns successful result before timeout``() =
        let flow =
            FlowController(
                { CtpFlowControlOptions.Default with
                    QueryCompletionTimeout = TimeSpan.FromSeconds 1.0 }
            )

        let pending = PendingQueryDict()
        let completion = ClientHelpers.createCompletionSource<Result<int list, RspInfo>> ()
        completion.TrySetResult(Ok [ 1; 2 ]) |> ignore

        let result =
            flow.AwaitQueryCompletionAsync("QueryNumbers", 42, pending, completion.Task).GetAwaiter().GetResult()

        match result with
        | Ok values -> Assert.Equal<int list>([ 1; 2 ], values)
        | Error error -> failwith $"Expected Ok but got Error({error})."

    [<Fact>]
    member _.``subscription batching falls back to size one when configured batch size is invalid``() =
        let flow = FlowController({ CtpFlowControlOptions.Default with SubscriptionBatchSize = 0 })

        let batches = flow.BatchSubscriptions([ "au2506"; "ag2506"; "cu2506" ])

        Assert.Equal<string list list>([ [ "au2506" ]; [ "ag2506" ]; [ "cu2506" ] ], batches)


type ClientCancellationApiTests() =

    [<Fact>]
    member _.``md login uses ambient cancellation token``() =
        let compileOnly: Ctp.Net.Next.MdClient -> Async<Result<UserLoginResponse, RspInfo>> =
            fun client -> client.LoginAsync()

        Assert.NotNull(box compileOnly)

    [<Fact>]
    member _.``trader query uses ambient cancellation token``() =
        let compileOnly: Ctp.Net.Next.TraderClient -> Async<Result<TradingAccountResponse list, RspInfo>> =
            fun client -> client.QueryTradingAccountAsync("CNY")

        Assert.NotNull(box compileOnly)


type ConnectionCoordinatorTests() =

    [<Fact>]
    member _.``connect waits for first front connected``() =
        let starts = ref 0
        let coordinator = ConnectionCoordinator(fun () -> Interlocked.Increment(starts) |> ignore)
        let task = Async.StartAsTask(coordinator.Connect())

        Helper.waitFor (fun () -> !starts = 1) 1000
        Assert.False(task.IsCompleted)

        coordinator.HandleFrontConnected()
        task.GetAwaiter().GetResult() |> Helper.assertOk

    [<Fact>]
    member _.``front disconnected before first connect does not fail``() =
        let starts = ref 0
        let coordinator = ConnectionCoordinator(fun () -> Interlocked.Increment(starts) |> ignore)
        let task = Async.StartAsTask(coordinator.Connect())

        Helper.waitFor (fun () -> !starts = 1) 1000
        coordinator.HandleFrontDisconnected()
        Assert.False(task.IsCompleted)

        coordinator.HandleFrontConnected()
        task.GetAwaiter().GetResult() |> Helper.assertOk

    [<Fact>]
    member _.``timeout does not restart connection flow``() =
        let starts = ref 0
        let coordinator = ConnectionCoordinator(fun () -> Interlocked.Increment(starts) |> ignore)
        let timeout = TimeSpan.FromMilliseconds 50.0

        coordinator.Connect(timeout = timeout)
        |> Async.RunSynchronously
        |> Helper.assertConnectError (ConnectError.Timeout timeout)

        Assert.Equal(1, !starts)

        let secondTask = Async.StartAsTask(coordinator.Connect())
        Thread.Sleep 20
        Assert.Equal(1, !starts)

        coordinator.HandleFrontConnected()
        secondTask.GetAwaiter().GetResult() |> Helper.assertOk

    [<Fact>]
    member _.``cancellation returns cancelled``() =
        let starts = ref 0
        let coordinator = ConnectionCoordinator(fun () -> Interlocked.Increment(starts) |> ignore)
        use cts = new CancellationTokenSource()

        cts.CancelAfter 50

        let result = coordinator.ConnectTask(cancellationToken = cts.Token).GetAwaiter().GetResult()

        Assert.Equal(1, !starts)
        result |> Helper.assertConnectError ConnectError.Cancelled

    [<Fact>]
    member _.``connect wrapper uses explicit cancellation token``() =
        let starts = ref 0
        let coordinator = ConnectionCoordinator(fun () -> Interlocked.Increment(starts) |> ignore)
        use cts = new CancellationTokenSource()

        cts.CancelAfter 50

        Async.RunSynchronously(coordinator.Connect(), cancellationToken = cts.Token)
        |> Helper.assertConnectError ConnectError.Cancelled

        Assert.Equal(1, !starts)

    [<Fact>]
    member _.``connect wrapper falls back to ambient cancellation token``() =
        let starts = ref 0
        let coordinator = ConnectionCoordinator(fun () -> Interlocked.Increment(starts) |> ignore)
        use cts = new CancellationTokenSource()

        cts.CancelAfter 50

        let result =
            Async.StartAsTask(coordinator.Connect(), cancellationToken = cts.Token).GetAwaiter().GetResult()

        Assert.Equal(1, !starts)
        result |> Helper.assertConnectError ConnectError.Cancelled

    [<Fact>]
    member _.``concurrent connect shares one start``() =
        let starts = ref 0
        let coordinator = ConnectionCoordinator(fun () -> Interlocked.Increment(starts) |> ignore)
        let firstTask = Async.StartAsTask(coordinator.Connect())
        let secondTask = Async.StartAsTask(coordinator.Connect())

        Helper.waitFor (fun () -> !starts = 1) 1000
        coordinator.HandleFrontConnected()

        firstTask.GetAwaiter().GetResult() |> Helper.assertOk
        secondTask.GetAwaiter().GetResult() |> Helper.assertOk
        Assert.Equal(1, !starts)

    [<Fact>]
    member _.``reconnect does not reinit``() =
        let starts = ref 0
        let coordinator = ConnectionCoordinator(fun () -> Interlocked.Increment(starts) |> ignore)
        let firstTask = Async.StartAsTask(coordinator.Connect())

        Helper.waitFor (fun () -> !starts = 1) 1000
        coordinator.HandleFrontConnected()
        firstTask.GetAwaiter().GetResult() |> Helper.assertOk

        coordinator.Connect() |> Async.RunSynchronously |> Helper.assertOk
        Assert.Equal(1, !starts)

        coordinator.HandleFrontDisconnected()

        let secondTask = Async.StartAsTask(coordinator.Connect())
        Thread.Sleep 20
        Assert.Equal(1, !starts)

        coordinator.HandleFrontConnected()
        secondTask.GetAwaiter().GetResult() |> Helper.assertOk

    [<Fact>]
    member _.``connection count starts at zero``() =
        let coordinator = ConnectionCoordinator(fun () -> ())
        Assert.Equal(0, coordinator.ConnectionCount)

    [<Fact>]
    member _.``connection count increments on first front connected``() =
        let coordinator = ConnectionCoordinator(fun () -> ())
        let task = Async.StartAsTask(coordinator.Connect())
        coordinator.HandleFrontConnected()
        task.GetAwaiter().GetResult() |> Helper.assertOk
        Assert.Equal(1, coordinator.ConnectionCount)

    [<Fact>]
    member _.``connection count increments on reconnection``() =
        let coordinator = ConnectionCoordinator(fun () -> ())

        let firstTask = Async.StartAsTask(coordinator.Connect())
        coordinator.HandleFrontConnected()
        firstTask.GetAwaiter().GetResult() |> Helper.assertOk
        Assert.Equal(1, coordinator.ConnectionCount)

        coordinator.HandleFrontDisconnected()

        let secondTask = Async.StartAsTask(coordinator.Connect())
        coordinator.HandleFrontConnected()
        secondTask.GetAwaiter().GetResult() |> Helper.assertOk
        Assert.Equal(2, coordinator.ConnectionCount)

    [<Fact>]
    member _.``connection count does not increment on duplicate front connected``() =
        let coordinator = ConnectionCoordinator(fun () -> ())
        let task = Async.StartAsTask(coordinator.Connect())
        coordinator.HandleFrontConnected()
        task.GetAwaiter().GetResult() |> Helper.assertOk
        Assert.Equal(1, coordinator.ConnectionCount)

        coordinator.HandleFrontConnected()
        Assert.Equal(1, coordinator.ConnectionCount)

    [<Fact>]
    member _.``native start failure is mapped``() =
        let coordinator = ConnectionCoordinator(fun () -> invalidOp "boom")

        coordinator.Connect()
        |> Async.RunSynchronously
        |> Helper.assertConnectError (ConnectError.NativeOperationFailed "boom")


module FakeLogger =

    type LogEntry = { Level: LogLevel; Message: string }

    type FakeLogger() =
        let entries = ResizeArray<LogEntry>()

        member _.Entries = entries :> IReadOnlyList<LogEntry>

        interface ILogger with
            member _.BeginScope _ = null
            member _.IsEnabled _ = true

            member _.Log(logLevel, _eventId, state, exn, formatter: Func<_, _, _>) =
                let message = formatter.Invoke(state, exn) |> string
                entries.Add({ Level = logLevel; Message = message })

    type FakeLoggerProvider() =
        let logger = FakeLogger()

        member _.Logger = logger

        interface ILoggerProvider with
            member _.CreateLogger _ = logger :> ILogger
            member _.Dispose() = ()


type LoggingTests() =

    let factoryWithProvider (provider: FakeLogger.FakeLoggerProvider) =
        { new ILoggerFactory with
            member _.CreateLogger(category) =
                provider :> ILoggerProvider |> fun p -> p.CreateLogger(category)

            member _.AddProvider _ = ()
            member _.Dispose() = () }

    [<Fact>]
    member _.``native failure logs error``() =
        use provider = new FakeLogger.FakeLoggerProvider()
        use factory = factoryWithProvider provider
        let logger = factory.CreateLogger("Ctp.Net.Next.ConnectionCoordinator")
        let coordinator = ConnectionCoordinator((fun () -> invalidOp "boom"), logger = logger)

        coordinator.Connect()
        |> Async.RunSynchronously
        |> Helper.assertConnectError (ConnectError.NativeOperationFailed "boom")

        let errors =
            provider.Logger.Entries
            |> Seq.filter (fun e -> e.Level = LogLevel.Error)
            |> List.ofSeq

        Assert.Single(errors) |> ignore
        Assert.Contains("Native connection initiation failed", errors[0].Message)
        Assert.Contains("boom", errors[0].Message)

    [<Fact>]
    member _.``connect timeout logs warning``() =
        use provider = new FakeLogger.FakeLoggerProvider()
        use factory = factoryWithProvider provider
        let logger = factory.CreateLogger("Ctp.Net.Next.ConnectionCoordinator")
        let coordinator = ConnectionCoordinator((fun () -> ()), logger = logger)
        let timeout = TimeSpan.FromMilliseconds 50.0

        coordinator.Connect(timeout = timeout)
        |> Async.RunSynchronously
        |> Helper.assertConnectError (ConnectError.Timeout timeout)

        let warnings =
            provider.Logger.Entries
            |> Seq.filter (fun e -> e.Level = LogLevel.Warning)
            |> List.ofSeq

        Assert.Single(warnings) |> ignore
        Assert.Contains("timed out", warnings[0].Message)

    [<Fact>]
    member _.``front connected logs debug``() =
        use provider = new FakeLogger.FakeLoggerProvider()
        use factory = factoryWithProvider provider
        let logger = factory.CreateLogger("Ctp.Net.Next.ConnectionCoordinator")
        let coordinator = ConnectionCoordinator((fun () -> ()), logger = logger)

        let task = Async.StartAsTask(coordinator.Connect())
        Helper.waitFor (fun () -> task.IsCompleted = false) 1000
        coordinator.HandleFrontConnected()
        task.GetAwaiter().GetResult() |> Helper.assertOk

        let debugs =
            provider.Logger.Entries
            |> Seq.filter (fun e -> e.Level = LogLevel.Debug)
            |> List.ofSeq

        Assert.Contains(debugs, fun e -> e.Message.Contains("Front connected"))

    [<Fact>]
    member _.``front disconnected logs info``() =
        use provider = new FakeLogger.FakeLoggerProvider()
        use factory = factoryWithProvider provider
        let logger = factory.CreateLogger("Ctp.Net.Next.ConnectionCoordinator")
        let coordinator = ConnectionCoordinator((fun () -> ()), logger = logger)

        let task = Async.StartAsTask(coordinator.Connect())
        coordinator.HandleFrontConnected()
        task.GetAwaiter().GetResult() |> Helper.assertOk
        coordinator.HandleFrontDisconnected()

        let infos =
            provider.Logger.Entries
            |> Seq.filter (fun e -> e.Level = LogLevel.Information)
            |> List.ofSeq

        Assert.Contains(infos, fun e -> e.Message.Contains("Front disconnected"))

    [<Fact>]
    member _.``no logger means no logging``() =
        let coordinator = ConnectionCoordinator(fun () -> ())
        let task = Async.StartAsTask(coordinator.Connect())

        coordinator.HandleFrontConnected()
        task.GetAwaiter().GetResult() |> Helper.assertOk
// No exception thrown = pass


type CommandDispatchTests() =

    let testFlow delay retries =
        FlowController(
            { CtpFlowControlOptions.Default with
                MaxDispatchesPerSecond = 0
                MaxNativeReturnCodeRetries = retries
                NativeReturnCodeRetryDelay = delay }
        )

    [<Fact>]
    member _.``accepted native command returns the accepted request id``() =
        let requestIds = ResizeArray<int>()
        let flow = testFlow TimeSpan.Zero 0

        let result =
            CommandDispatch.runAsync
                "TestCommand"
                (fun () -> 42)
                flow
                (Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance :> ILogger)
                (fun requestId ->
                    requestIds.Add requestId
                    0)
            |> Async.RunSynchronously

        Assert.Equal(Ok 42, result)
        Assert.Equal<int list>([ 42 ], List.ofSeq requestIds)

    [<Fact>]
    member _.``nonretryable native command returns the final error``() =
        let flow = testFlow TimeSpan.Zero 3

        let result =
            CommandDispatch.runAsync
                "TestCommand"
                (fun () -> 7)
                flow
                (Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance :> ILogger)
                (fun _ -> -9)
            |> Async.RunSynchronously

        match result with
        | Error error ->
            Assert.Equal(-9, error.ErrorId)
            Assert.Contains("-9", error.ErrorMessage)
        | Ok requestId -> failwith $"Expected native error, got accepted request id {requestId}."

    [<Fact>]
    member _.``retryable native returns use a new request id and return the accepted id``() =
        let requestIds = ResizeArray<int>()
        let returnCodes = [| -2; -3; 0 |]
        let attempt = ref 0
        let flow = testFlow TimeSpan.Zero 3

        let result =
            CommandDispatch.runAsync
                "TestCommand"
                (let next = ref 100
                 fun () ->
                     let value = !next
                     next := value + 1
                     value)
                flow
                (Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance :> ILogger)
                (fun requestId ->
                    requestIds.Add requestId
                    let current = returnCodes[!attempt]
                    attempt := !attempt + 1
                    current)
            |> Async.RunSynchronously

        Assert.Equal(Ok 102, result)
        Assert.Equal<int list>([ 100; 101; 102 ], List.ofSeq requestIds)

    [<Fact>]
    member _.``cancellation interrupts native retry delay``() =
        let flow = testFlow (TimeSpan.FromSeconds 1.) 3
        use cts = new CancellationTokenSource()
        cts.CancelAfter 50

        let work =
            CommandDispatch.runAsync
                "TestCommand"
                (fun () -> 1)
                flow
                (Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance :> ILogger)
                (fun _ -> -2)

        Assert.Throws<TaskCanceledException>(fun () ->
            Async.StartAsTask(work, cancellationToken = cts.Token).GetAwaiter().GetResult() |> ignore)
        |> ignore

    [<Fact>]
    member _.``CSharp command wrapper maps native dispatch errors``() =
        let computation = async { return raise (NativeRequestException("TestCommand", -9)) }

        let nativeException =
            Assert.Throws<CtpNativeException>(fun () ->
                CSharpHelpers.startCommandAsync CancellationToken.None computation
                |> fun task -> task.GetAwaiter().GetResult()
                |> ignore)

        Assert.Equal(-9, nativeException.ReturnCode)
        Assert.Contains("TestCommand", nativeException.Message)


type CallbackSemanticTests() =

    [<Theory>]
    [<InlineData("LoginWithCaptcha", "OnRspUserLogin")>]
    [<InlineData("ParkedOrderRequest", "OnRspParkedOrderInsert")>]
    [<InlineData("QryCfmmcTradingAccountKeyRequest", "OnRspQryCFMMCTradingAccountKey")>]
    [<InlineData("QryInvestorProdSpbmDetailRequest", "OnRspQryInvestorProdSPBMDetail")>]
    [<InlineData("QrySecAgentAcIdMapRequest", "OnRspQrySecAgentACIDMap")>]
    [<InlineData("QuerySecAgentTradingAccountRequest", "OnRspQrySecAgentTradingAccount")>]
    [<InlineData("QueryCfmmcTradingAccountToken", "OnRspQueryCFMMCTradingAccountToken")>]
    [<InlineData("QueryCfmmcTradingAccountTokenRequest", "OnRspQueryCFMMCTradingAccountToken")>]
    member _.``correlated operation names preserve official callback spelling``(operationName: string, expected: string) =
        Assert.Equal(expected, TraderCallbackNames.forOperation operationName)

    [<Theory>]
    [<InlineData(TraderOperationNames.OrderAction, "OnRspOrderAction")>]
    [<InlineData(TraderOperationNames.BatchOrderAction, "OnRspBatchOrderAction")>]
    [<InlineData(TraderOperationNames.CancelOffsetSetting, "OnRspCancelOffsetSetting")>]
    [<InlineData(TraderOperationNames.ExecOrderAction, "OnRspExecOrderAction")>]
    [<InlineData(TraderOperationNames.HedgeCfm, "OnRspHedgeCfm")>]
    [<InlineData(TraderOperationNames.HedgeCfmAction, "OnRspHedgeCfmAction")>]
    [<InlineData(TraderOperationNames.OffsetSetting, "OnRspOffsetSetting")>]
    [<InlineData(TraderOperationNames.OptionSelfCloseAction, "OnRspOptionSelfCloseAction")>]
    [<InlineData(TraderOperationNames.QuoteAction, "OnRspQuoteAction")>]
    [<InlineData(TraderOperationNames.SpdApply, "OnRspSpdApply")>]
    [<InlineData(TraderOperationNames.SpdApplyAction, "OnRspSpdApplyAction")>]
    member _.``command operation identities map to matching callbacks``(operationName: string, expected: string) =
        Assert.Equal(expected, TraderCallbackNames.forOperation operationName)

    [<Fact>]
    member _.``detailed async error keeps callback payload and optional rsp info``() =
        let rspInfo =
            { ErrorId = 17
              ErrorMessage = "order rejected"
              RawErrorMessage = [| 111uy |] }

        let payload = box "order"
        let error: TraderAsyncError =
            { CallbackName = "OnErrRtnOrderInsert"
              Payload = payload
              RspInfo = Some rspInfo }

        Assert.Equal("OnErrRtnOrderInsert", error.CallbackName)
        Assert.Same(payload, error.Payload)
        Assert.Equal(Some rspInfo, error.RspInfo)

        let legacyPayload = box (error.Payload, error.RspInfo)
        let legacyItem, legacyRsp = unbox<obj * RspInfo option> legacyPayload
        Assert.Same(payload, legacyItem)
        Assert.Equal(Some rspInfo, legacyRsp)

    [<Fact>]
    member _.``command response retains callback identity payload and completion flag``() =
        let payload = box "input order"

        let response: TraderCommandResponse =
            { CallbackName = "OnRspOrderInsert"
              OperationName = "OrderInsert"
              RequestId = 23
              IsLast = true
              RspInfo = None
              Payload = Some payload }

        Assert.Equal("OnRspOrderInsert", response.CallbackName)
        Assert.Equal("OrderInsert", response.OperationName)
        Assert.Equal(23, response.RequestId)
        Assert.True(response.IsLast)
        Assert.Same(payload, response.Payload.Value)


type CSharpProjectionTests() =

    let options = CtpOptions.Create("tcp://front", "9999", "demo", "secret")

    [<Fact>]
    member _.``CSharp MD auto resubscribe defaults to true and accepts explicit false``() =
        let constructors = typeof<Ctp.Net.Next.CSharp.MdClient>.GetConstructors()

        let autoResubscribeParameters =
            constructors
            |> Seq.collect (fun constructorInfo -> constructorInfo.GetParameters())
            |> Seq.filter (fun parameter -> parameter.Name = "autoResubscribe")
            |> Seq.toList

        Assert.NotEmpty(autoResubscribeParameters)

        for parameter in autoResubscribeParameters do
            Assert.True(parameter.IsOptional)
            Assert.True(unbox<bool> parameter.DefaultValue)

        let configuration = MdClientOptions.FromFront(options)
        configuration.AutoResubscribe <- false
        Assert.False(configuration.AutoResubscribe)

    [<Fact>]
    member _.``CSharp client options compose NameServer FENS and advanced settings``() =
        let fens =
            { BrokerId = "9999"
              UserId = "demo"
              LoginMode = None }

        let md = MdClientOptions.FromNameServer(options, "tcp://name-server", fens)
        md.Encodings <- CtpEncodingOptions.Default
        md.FlowControl <- CtpFlowControlOptions.Default
        md.UseUdp <- true
        md.UseMulticast <- true
        md.AutoResubscribe <- false

        match md.Endpoint with
        | CtpEndpoint.NameServer(address, Some actualFens) ->
            Assert.Equal("tcp://name-server", address)
            Assert.Equal(fens, actualFens)
        | endpoint -> failwith $"Expected NameServer endpoint, got {endpoint}."

        Assert.Equal(CtpEncodingOptions.Default.OutboundEncoding.WebName, md.Encodings.OutboundEncoding.WebName)
        Assert.Equal(CtpFlowControlOptions.Default, md.FlowControl)
        Assert.True(md.UseUdp)
        Assert.True(md.UseMulticast)
        Assert.False(md.AutoResubscribe)

        let trader = TraderClientOptions.FromNameServer(options, "tcp://name-server", fens)
        trader.Encodings <- CtpEncodingOptions.Default
        trader.FlowControl <- CtpFlowControlOptions.Default
        trader.PrivateTopicResumeType <- Nullable ResumeType.Quick
        trader.PrivateTopicSequenceNo <- Nullable 42

        match trader.Endpoint with
        | CtpEndpoint.NameServer(address, Some actualFens) ->
            Assert.Equal("tcp://name-server", address)
            Assert.Equal(fens, actualFens)
        | endpoint -> failwith $"Expected NameServer endpoint, got {endpoint}."

        Assert.Equal(ResumeType.Quick, trader.PrivateTopicResumeType.Value)
        Assert.Equal(42, trader.PrivateTopicSequenceNo.Value)

    [<Fact>]
    member _.``every Trader Rtn callback has matching strongly typed FSharp and CSharp events``() =
        let rtnCallbacks =
            Microsoft.FSharp.Reflection.FSharpType.GetRecordFields(typeof<TraderCallbacks>)
            |> Array.filter (fun field -> field.Name.StartsWith("Rtn", StringComparison.Ordinal))

        Assert.Equal(30, rtnCallbacks.Length)

        for callback in rtnCallbacks do
            let eventName = $"{callback.Name.Substring(3)}Received"

            let callbackFunctionType = callback.PropertyType.GetGenericArguments().[0]
            let expectedPayloadType = callbackFunctionType.GetGenericArguments().[0]

            let fsharpEvent = typeof<Ctp.Net.Next.TraderClient>.GetProperty(eventName)
            Assert.NotNull(fsharpEvent)

            let fsharpPayloadType = fsharpEvent.PropertyType.GetGenericArguments() |> Array.last
            Assert.Equal(expectedPayloadType, fsharpPayloadType)

            let csharpEvent = typeof<Ctp.Net.Next.CSharp.TraderClient>.GetEvent(eventName)
            Assert.NotNull(csharpEvent)

            let csharpPayloadType = csharpEvent.EventHandlerType.GetGenericArguments() |> Array.last
            Assert.Equal(expectedPayloadType, csharpPayloadType)

    [<Fact>]
    member _.``every Trader ErrRtn callback has matching strongly typed FSharp and CSharp events``() =
        let errorCallbacks =
            Microsoft.FSharp.Reflection.FSharpType.GetRecordFields(typeof<TraderCallbacks>)
            |> Array.filter (fun field -> field.Name.StartsWith("ErrRtn", StringComparison.Ordinal))

        Assert.Equal(22, errorCallbacks.Length)

        for callback in errorCallbacks do
            let eventName = $"{callback.Name.Substring(6)}ErrorReceived"
            let callbackFunctionType = callback.PropertyType.GetGenericArguments().[0]
            let payloadOptionType = callbackFunctionType.GetGenericArguments().[0]
            let expectedPayloadType = payloadOptionType.GetGenericArguments().[0]

            let fsharpEvent = typeof<Ctp.Net.Next.TraderClient>.GetProperty(eventName)
            Assert.NotNull(fsharpEvent)

            let fsharpDataType = fsharpEvent.PropertyType.GetGenericArguments() |> Array.last
            Assert.Equal(typedefof<TraderAsyncErrorData<_>>, fsharpDataType.GetGenericTypeDefinition())
            Assert.Equal(expectedPayloadType, fsharpDataType.GetGenericArguments().[0])

            let csharpEvent = typeof<Ctp.Net.Next.CSharp.TraderClient>.GetEvent(eventName)
            Assert.NotNull(csharpEvent)

            let csharpDataType = csharpEvent.EventHandlerType.GetGenericArguments() |> Array.last
            Assert.Equal(typedefof<TraderAsyncErrorEventArgs<_>>, csharpDataType.GetGenericTypeDefinition())
            Assert.Equal(expectedPayloadType, csharpDataType.GetGenericArguments().[0])

    [<Fact>]
    member _.``every Trader command response callback has matching strongly typed FSharp and CSharp events``() =
        let callbackNames =
            [| "RspOrderInsert"
               "RspOrderAction"
               "RspBatchOrderAction"
               "RspCancelOffsetSetting"
               "RspCombActionInsert"
               "RspExecOrderAction"
               "RspExecOrderInsert"
               "RspForQuoteInsert"
               "RspFromBankToFutureByFuture"
               "RspFromFutureToBankByFuture"
               "RspHedgeCfm"
               "RspHedgeCfmAction"
               "RspOffsetSetting"
               "RspOptionSelfCloseAction"
               "RspOptionSelfCloseInsert"
               "RspQuoteAction"
               "RspQuoteInsert"
               "RspSpdApply"
               "RspSpdApplyAction" |]

        let callbacks =
            Microsoft.FSharp.Reflection.FSharpType.GetRecordFields(typeof<TraderCallbacks>)
            |> Array.filter (fun field -> Array.contains field.Name callbackNames)

        Assert.Equal(callbackNames.Length, callbacks.Length)

        for callback in callbacks do
            let eventName = $"{callback.Name.Substring(3)}ResponseReceived"
            let callbackFunctionType = callback.PropertyType.GetGenericArguments().[0]
            let payloadOptionType = callbackFunctionType.GetGenericArguments().[0]
            let expectedPayloadType = payloadOptionType.GetGenericArguments().[0]

            let fsharpEvent = typeof<Ctp.Net.Next.TraderClient>.GetProperty(eventName)
            Assert.NotNull(fsharpEvent)

            let fsharpDataType = fsharpEvent.PropertyType.GetGenericArguments() |> Array.last
            Assert.Equal(typedefof<TraderCommandResponseData<_>>, fsharpDataType.GetGenericTypeDefinition())
            Assert.Equal(expectedPayloadType, fsharpDataType.GetGenericArguments().[0])

            let csharpEvent = typeof<Ctp.Net.Next.CSharp.TraderClient>.GetEvent(eventName)
            Assert.NotNull(csharpEvent)

            let csharpDataType = csharpEvent.EventHandlerType.GetGenericArguments() |> Array.last
            Assert.Equal(typedefof<TraderCommandResponseEventArgs<_>>, csharpDataType.GetGenericTypeDefinition())
            Assert.Equal(expectedPayloadType, csharpDataType.GetGenericArguments().[0])

    [<Fact>]
    member _.``legacy erased Trader events are obsolete and typed CSharp args do not expose FSharpOption``() =
        let hasObsoleteAttribute (memberInfo: System.Reflection.MemberInfo) =
            memberInfo.GetCustomAttributes(typeof<ObsoleteAttribute>, true).Length > 0

        let legacyEventNames =
            [| "NotificationReceived"
               "AsyncErrorReceived"
               "AsyncErrorDetailedReceived"
               "CommandResponseReceived" |]

        for eventName in legacyEventNames do
            let fsharpProperty = typeof<Ctp.Net.Next.TraderClient>.GetProperty(eventName)
            Assert.True(hasObsoleteAttribute fsharpProperty, $"F# {eventName} should be obsolete.")

            let csharpEvent = typeof<Ctp.Net.Next.CSharp.TraderClient>.GetEvent(eventName)
            Assert.True(hasObsoleteAttribute csharpEvent, $"C# {eventName} should be obsolete.")

        let publicPropertyTypes (genericType: Type) =
            genericType.GetProperties()
            |> Array.map _.PropertyType

        for propertyType in publicPropertyTypes (typedefof<TraderAsyncErrorEventArgs<_>>.MakeGenericType(typeof<InputOrderResponse>)) do
            Assert.DoesNotContain("FSharpOption", propertyType.ToString())

        for propertyType in publicPropertyTypes (typedefof<TraderCommandResponseEventArgs<_>>.MakeGenericType(typeof<InputOrderResponse>)) do
            Assert.DoesNotContain("FSharpOption", propertyType.ToString())

    [<Fact>]
    member _.``CSharp Trader projection covers non Try FSharp members without FSharpOption``() =
        let flags = System.Reflection.BindingFlags.Instance ||| System.Reflection.BindingFlags.Public

        let fsharpMethods =
            typeof<Ctp.Net.Next.TraderClient>.GetMethods(flags)
            |> Seq.filter (fun methodInfo -> not (methodInfo.Name.StartsWith("get_")))
            |> Seq.map (fun methodInfo -> methodInfo.Name)
            |> Set.ofSeq

        let csharpMethods =
            typeof<Ctp.Net.Next.CSharp.TraderClient>.GetMethods(flags)
            |> Seq.filter (fun methodInfo -> not (methodInfo.Name.StartsWith("get_")))
            |> Seq.map (fun methodInfo -> methodInfo.Name)
            |> Set.ofSeq

        let expectedMethods =
            fsharpMethods
            |> Set.filter (fun name -> name <> "Connect" && not (name.StartsWith("Try")))

        let missing = Set.difference expectedMethods csharpMethods

        Assert.Empty(missing)

        let optionParameters =
            typeof<Ctp.Net.Next.CSharp.TraderClient>.GetMethods(flags)
            |> Seq.collect (fun methodInfo -> methodInfo.GetParameters())
            |> Seq.filter (fun parameter -> parameter.ParameterType.ToString().Contains("FSharpOption"))
            |> Seq.toList

        Assert.Empty(optionParameters)


type AbiLayoutTests() =

    let assembly = typeof<InstrumentResponse>.Assembly

    let nativeType name =
        match assembly.GetType(name, false) with
        | null -> failwith $"Expected native interop type '{name}'."
        | value -> value

    let assertLayout name expectedSize fields =
        let typeInfo = nativeType $"Ctp.Net.Next.Bridge.{name}"
        Assert.Equal(expectedSize, System.Runtime.InteropServices.Marshal.SizeOf(typeInfo))

        for fieldName, expectedOffset in fields do
            Assert.Equal(
                expectedOffset,
                System.Runtime.InteropServices.Marshal.OffsetOf(typeInfo, fieldName).ToInt32()
            )

    [<Fact>]
    member _.``new MD C ABI structs keep the checked-in interop layout``() =
        assertLayout
            "NativeMulticastInstrument"
            144
            [ "TopicId", 0
              "InstrumentNo", 4
              "CodePrice", 8
              "VolumeMultiple", 16
              "PriceTick", 24
              "InstrumentId", 32
              "Reserve1", 113 ]

        assertLayout "NativeQryMulticastInstrument" 116 [ "TopicId", 0; "InstrumentId", 4; "Reserve1", 85 ]
        assertLayout "NativeMdFensUserInfo" 28 [ "BrokerId", 0; "UserId", 11; "LoginMode", 27 ]

        assertLayout
            "NativeMdForQuoteRsp"
            169
            [ "TradingDay", 0
              "Reserve1", 9
              "ForQuoteSysId", 40
              "ForQuoteTime", 61
              "ActionDay", 70
              "ExchangeId", 79
              "InstrumentId", 88 ]

    [<Fact>]
    member _.``MD callback table has one native function pointer per callback``() =
        assertLayout "NativeMdSpi" 104 [ "OnRtnForQuoteRsp", 96 ]
