namespace Ctp.Bridge.Net

open System
open System.Runtime.InteropServices
open Microsoft.Win32.SafeHandles

type DepthMarketData =
    { TradingDay: string
      ExchangeId: string
      LastPrice: decimal
      PreSettlementPrice: decimal
      PreClosePrice: decimal
      PreOpenInterest: float
      OpenPrice: decimal
      HighestPrice: decimal
      LowestPrice: decimal
      Volume: int
      Turnover: decimal
      OpenInterest: float
      ClosePrice: decimal
      SettlementPrice: decimal
      UpperLimitPrice: decimal
      LowerLimitPrice: decimal
      PreDelta: float
      CurrDelta: float
      UpdateTime: string
      UpdateMillisec: int
      BidPrice1: decimal
      BidVolume1: int
      AskPrice1: decimal
      AskVolume1: int
      BidPrice2: decimal
      BidVolume2: int
      AskPrice2: decimal
      AskVolume2: int
      BidPrice3: decimal
      BidVolume3: int
      AskPrice3: decimal
      AskVolume3: int
      BidPrice4: decimal
      BidVolume4: int
      AskPrice4: decimal
      AskVolume4: int
      BidPrice5: decimal
      BidVolume5: int
      AskPrice5: decimal
      AskVolume5: int
      AveragePrice: decimal
      ActionDay: string
      InstrumentId: string
      ExchangeInstId: string
      BandingUpperPrice: decimal
      BandingLowerPrice: decimal }

type MdCallbacks =
    { FrontConnected: (unit -> unit) option
      FrontDisconnected: (int -> unit) option
      HeartBeatWarning: (int -> unit) option
      RspUserLogin: (UserLoginResponse option -> RspInfo option -> int -> bool -> unit) option
      RspUserLogout: (UserLogoutResponse option -> RspInfo option -> int -> bool -> unit) option
      RspError: (RspInfo option -> int -> bool -> unit) option
      RspSubMarketData: (SpecificInstrument option -> RspInfo option -> int -> bool -> unit) option
      RspUnsubMarketData: (SpecificInstrument option -> RspInfo option -> int -> bool -> unit) option
      RtnDepthMarketData: (DepthMarketData -> unit) option }

    static member Empty =
        { FrontConnected = None
          FrontDisconnected = None
          HeartBeatWarning = None
          RspUserLogin = None
          RspUserLogout = None
          RspError = None
          RspSubMarketData = None
          RspUnsubMarketData = None
          RtnDepthMarketData = None }

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeDepthMarketData =
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

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private MdFrontConnectedDelegate = delegate of nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private MdFrontDisconnectedDelegate = delegate of int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private MdHeartBeatWarningDelegate = delegate of int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private MdRspUserLoginDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private MdRspUserLogoutDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private MdRspErrorDelegate = delegate of nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private MdRspSpecificInstrumentDelegate = delegate of nativeint * nativeint * int * int * nativeint -> unit

[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type private MdRtnDepthMarketDataDelegate = delegate of nativeint * nativeint -> unit

[<Struct; StructLayout(LayoutKind.Sequential)>]
type private NativeMdSpi =
    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable OnFrontConnected: MdFrontConnectedDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable OnFrontDisconnected: MdFrontDisconnectedDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable OnHeartBeatWarning: MdHeartBeatWarningDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable OnRspUserLogin: MdRspUserLoginDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable OnRspUserLogout: MdRspUserLogoutDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable OnRspError: MdRspErrorDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable OnRspSubMarketData: MdRspSpecificInstrumentDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable OnRspUnsubMarketData: MdRspSpecificInstrumentDelegate

    [<MarshalAs(UnmanagedType.FunctionPtr)>]
    [<DefaultValue>]
    val mutable OnRtnDepthMarketData: MdRtnDepthMarketDataDelegate

module private MdNativeInterop =
    [<Literal>]
    let Library = "ctpmd_bridge"

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_md_get_api_version")>]
    extern nativeint getApiVersion()

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_md_create")>]
    extern nativeint create(nativeint flowPath, int usingUdp, int multicast, int productionMode)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_md_destroy")>]
    extern void destroy(nativeint handle)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_md_set_spi")>]
    extern int setSpi(nativeint handle, NativeMdSpi& spi, nativeint userData)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_md_register_front")>]
    extern int registerFront(nativeint handle, nativeint frontAddress)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_md_init")>]
    extern void init(nativeint handle)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_md_join")>]
    extern int join(nativeint handle)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_md_req_user_login")>]
    extern int reqUserLogin(nativeint handle, NativeReqUserLogin& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_md_req_user_logout")>]
    extern int reqUserLogout(nativeint handle, NativeUserLogout& request, int requestId)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_md_subscribe_market_data")>]
    extern int subscribeMarketData(nativeint handle, nativeint[] instruments, int count)

    [<DllImport(Library, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ctp_md_unsubscribe_market_data")>]
    extern int unsubscribeMarketData(nativeint handle, nativeint[] instruments, int count)

type private MdApiSafeHandle private () =
    inherit SafeHandleZeroOrMinusOneIsInvalid(true)

    static member FromNative(handle: nativeint) =
        let safeHandle = new MdApiSafeHandle()
        safeHandle.SetHandle(handle)
        safeHandle

    override this.ReleaseHandle() =
        BridgeResolver.ensureRegistered ()
        MdNativeInterop.destroy this.handle
        true

module private MdBridgeMapping =
    let private toDecimal value = NumericHelpers.priceOrInvalid value

    let depthMarketData encoding (value: NativeDepthMarketData) =
        { TradingDay = EncodingHelpers.decodeFixed encoding value.TradingDay
          ExchangeId = EncodingHelpers.decodeFixed encoding value.ExchangeId
          LastPrice = toDecimal value.LastPrice
          PreSettlementPrice = toDecimal value.PreSettlementPrice
          PreClosePrice = toDecimal value.PreClosePrice
          PreOpenInterest = value.PreOpenInterest
          OpenPrice = toDecimal value.OpenPrice
          HighestPrice = toDecimal value.HighestPrice
          LowestPrice = toDecimal value.LowestPrice
          Volume = value.Volume
          Turnover = toDecimal value.Turnover
          OpenInterest = value.OpenInterest
          ClosePrice = toDecimal value.ClosePrice
          SettlementPrice = toDecimal value.SettlementPrice
          UpperLimitPrice = toDecimal value.UpperLimitPrice
          LowerLimitPrice = toDecimal value.LowerLimitPrice
          PreDelta = value.PreDelta
          CurrDelta = value.CurrDelta
          UpdateTime = EncodingHelpers.decodeFixed encoding value.UpdateTime
          UpdateMillisec = value.UpdateMillisec
          BidPrice1 = toDecimal value.BidPrice1
          BidVolume1 = value.BidVolume1
          AskPrice1 = toDecimal value.AskPrice1
          AskVolume1 = value.AskVolume1
          BidPrice2 = toDecimal value.BidPrice2
          BidVolume2 = value.BidVolume2
          AskPrice2 = toDecimal value.AskPrice2
          AskVolume2 = value.AskVolume2
          BidPrice3 = toDecimal value.BidPrice3
          BidVolume3 = value.BidVolume3
          AskPrice3 = toDecimal value.AskPrice3
          AskVolume3 = value.AskVolume3
          BidPrice4 = toDecimal value.BidPrice4
          BidVolume4 = value.BidVolume4
          AskPrice4 = toDecimal value.AskPrice4
          AskVolume4 = value.AskVolume4
          BidPrice5 = toDecimal value.BidPrice5
          BidVolume5 = value.BidVolume5
          AskPrice5 = toDecimal value.AskPrice5
          AskVolume5 = value.AskVolume5
          AveragePrice = toDecimal value.AveragePrice
          ActionDay = EncodingHelpers.decodeFixed encoding value.ActionDay
          InstrumentId = EncodingHelpers.decodeFixed encoding value.InstrumentId
          ExchangeInstId = EncodingHelpers.decodeFixed encoding value.ExchangeInstId
          BandingUpperPrice = toDecimal value.BandingUpperPrice
          BandingLowerPrice = toDecimal value.BandingLowerPrice }

module private MdBridgeHelpers =
    let toMdHandle (ptr: nativeint) =
        if ptr = 0n then
            invalidOp "Failed to create md bridge handle."

        MdApiSafeHandle.FromNative ptr

type private MdSpiRegistration(callbacks: MdCallbacks, encodings: EncodingPair) =
    let onFrontConnected =
        MdFrontConnectedDelegate(fun _ -> callbacks.FrontConnected |> Option.iter (fun handler -> handler ()))

    let onFrontDisconnected =
        MdFrontDisconnectedDelegate(fun reason _ ->
            callbacks.FrontDisconnected |> Option.iter (fun handler -> handler reason))

    let onHeartBeatWarning =
        MdHeartBeatWarningDelegate(fun lapse _ ->
            callbacks.HeartBeatWarning |> Option.iter (fun handler -> handler lapse))

    let onRspUserLogin =
        MdRspUserLoginDelegate(fun loginPtr rspInfoPtr requestId isLast _ ->
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
        MdRspUserLogoutDelegate(fun logoutPtr rspInfoPtr requestId isLast _ ->
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
        MdRspErrorDelegate(fun rspInfoPtr requestId isLast _ ->
            callbacks.RspError
            |> Option.iter (fun handler ->
                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler rspInfo requestId (isLast <> 0)))

    let onRspSubMarketData =
        MdRspSpecificInstrumentDelegate(fun instrumentPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspSubMarketData
            |> Option.iter (fun handler ->
                let instrument =
                    instrumentPtr
                    |> EncodingHelpers.ptrToOption<NativeSpecificInstrument>
                    |> Option.map (BridgeMapping.specificInstrument encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler instrument rspInfo requestId (isLast <> 0)))

    let onRspUnsubMarketData =
        MdRspSpecificInstrumentDelegate(fun instrumentPtr rspInfoPtr requestId isLast _ ->
            callbacks.RspUnsubMarketData
            |> Option.iter (fun handler ->
                let instrument =
                    instrumentPtr
                    |> EncodingHelpers.ptrToOption<NativeSpecificInstrument>
                    |> Option.map (BridgeMapping.specificInstrument encodings.InboundEncoding)

                let rspInfo =
                    rspInfoPtr
                    |> EncodingHelpers.ptrToOption<NativeRspInfo>
                    |> Option.map (BridgeMapping.rspInfo encodings.InboundEncoding)

                handler instrument rspInfo requestId (isLast <> 0)))

    let onRtnDepthMarketData =
        MdRtnDepthMarketDataDelegate(fun marketDataPtr _ ->
            callbacks.RtnDepthMarketData
            |> Option.iter (fun handler ->
                match EncodingHelpers.ptrToOption<NativeDepthMarketData> marketDataPtr with
                | Some marketData -> handler (MdBridgeMapping.depthMarketData encodings.InboundEncoding marketData)
                | None -> ()))

    let mutable native = NativeMdSpi()

    do
        native.OnFrontConnected <- onFrontConnected
        native.OnFrontDisconnected <- onFrontDisconnected
        native.OnHeartBeatWarning <- onHeartBeatWarning
        native.OnRspUserLogin <- onRspUserLogin
        native.OnRspUserLogout <- onRspUserLogout
        native.OnRspError <- onRspError
        native.OnRspSubMarketData <- onRspSubMarketData
        native.OnRspUnsubMarketData <- onRspUnsubMarketData
        native.OnRtnDepthMarketData <- onRtnDepthMarketData

    member _.Native = native

type MdApi(flowPath: string option, useUdp: bool, useMulticast: bool, productionMode: bool, ?encodings: EncodingPair) =
    let encodings = defaultArg encodings EncodingPair.Default

    let handle =
        BridgeResolver.ensureRegistered ()
        let usingUdpFlag = if useUdp then 1 else 0
        let multicastFlag = if useMulticast then 1 else 0
        let productionFlag = if productionMode then 1 else 0

        BridgeHelpers.withEncodedCString encodings.OutboundEncoding flowPath (fun ptr ->
            MdNativeInterop.create (ptr, usingUdpFlag, multicastFlag, productionFlag))
        |> MdBridgeHelpers.toMdHandle

    let mutable callbackRegistrationLifetime: MdSpiRegistration option = None

    member private _.Handle =
        if handle.IsInvalid then
            invalidOp "The md API handle is invalid."

        handle.DangerousGetHandle()

    static member GetApiVersion() =
        BridgeResolver.ensureRegistered ()
        MdNativeInterop.getApiVersion () |> BridgeHelpers.ptrToAnsiString

    member this.SetCallbacks(callbacks: MdCallbacks) =
        let registration = MdSpiRegistration(callbacks, encodings)
        let mutable native = registration.Native
        let result = MdNativeInterop.setSpi (this.Handle, &native, 0n)
        BridgeHelpers.throwOnNonZero result "ctp_md_set_spi"
        callbackRegistrationLifetime <- Some registration

    member this.RegisterFront(frontAddress: string) =
        BridgeHelpers.withEncodedCString encodings.OutboundEncoding (Some frontAddress) (fun ptr ->
            MdNativeInterop.registerFront (this.Handle, ptr))
        |> BridgeHelpers.throwOnNonZero
        <| "ctp_md_register_front"

    member this.Init() = MdNativeInterop.init (this.Handle)

    member this.Join() = MdNativeInterop.join (this.Handle)

    member this.ReqUserLogin(request: RequestUserLogin, requestId: int) =
        let mutable native = BridgeBuilders.reqUserLogin encodings.OutboundEncoding request
        MdNativeInterop.reqUserLogin (this.Handle, &native, requestId)

    member this.ReqUserLogout(request: RequestUserLogout, requestId: int) =
        let mutable native = BridgeBuilders.reqUserLogout encodings.OutboundEncoding request
        MdNativeInterop.reqUserLogout (this.Handle, &native, requestId)

    member this.SubscribeMarketData(instrumentIds: string seq) =
        if Seq.isEmpty instrumentIds then
            invalidArg (nameof instrumentIds) "At least one instrument id is required."

        BridgeHelpers.withEncodedStringArray encodings.OutboundEncoding instrumentIds (fun pointers ->
            MdNativeInterop.subscribeMarketData (this.Handle, Array.ofSeq pointers, Seq.length pointers))

    member this.UnsubscribeMarketData(instrumentIds: string seq) =
        if Seq.isEmpty instrumentIds then
            invalidArg (nameof instrumentIds) "At least one instrument id is required."

        BridgeHelpers.withEncodedStringArray encodings.OutboundEncoding instrumentIds (fun pointers ->
            MdNativeInterop.unsubscribeMarketData (this.Handle, Array.ofSeq pointers, Seq.length pointers))

    interface IDisposable with
        member _.Dispose() = handle.Dispose()
