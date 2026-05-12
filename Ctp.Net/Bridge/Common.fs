namespace Ctp.Net.Bridge

open System
open System.IO
open System.Text
open System.Reflection
open System.Runtime.InteropServices

module internal BridgeResolver =
    let private syncRoot = obj ()
    let mutable private registered = false

    let private libraryFileName (libraryName: string) =
        if RuntimeInformation.IsOSPlatform(OSPlatform.Windows) then
            $"{libraryName}.dll"
        elif RuntimeInformation.IsOSPlatform(OSPlatform.OSX) then
            $"lib{libraryName}.dylib"
        else
            $"lib{libraryName}.so"

    let private isBridgeLibrary (libraryName: string) =
        libraryName = "ctpmd_bridge" || libraryName = "ctptrader_bridge"

    let private candidateDirectories () =
        seq {
            let fromEnv = Environment.GetEnvironmentVariable("CTP_BRIDGE_DIR")

            if not (String.IsNullOrWhiteSpace fromEnv) then
                yield fromEnv

            yield AppContext.BaseDirectory
            yield Path.Combine(AppContext.BaseDirectory, "native")
        }
        |> Seq.distinct

    let private createMissingLibraryMessage (libraryName: string) =
        let fileName = libraryFileName libraryName
        let configuredBridgeDir = Environment.GetEnvironmentVariable("CTP_BRIDGE_DIR")

        let searchedPaths =
            candidateDirectories ()
            |> Seq.map (fun directory -> Path.Combine(directory, fileName))
            |> String.concat Environment.NewLine

        let bridgeDirHint =
            if String.IsNullOrWhiteSpace configuredBridgeDir then
                "CTP_BRIDGE_DIR is not set."
            else
                $"CTP_BRIDGE_DIR is set to '{configuredBridgeDir}'."

        String.concat
            Environment.NewLine
            [ $"Unable to load native bridge library '{fileName}'."
              bridgeDirHint
              "Native bridge lookup order:"
              "  1. CTP_BRIDGE_DIR"
              "  2. AppContext.BaseDirectory"
              "  3. AppContext.BaseDirectory/native"
              "Set CTP_BRIDGE_DIR to the directory containing the Ctp.Bridge.C build output, for example:"
              "  export CTP_BRIDGE_DIR=/path/to/Ctp.Net/Ctp.Bridge.C/build"
              "Checked paths:"
              searchedPaths ]

    let ensureRegistered () =
        lock syncRoot (fun () ->
            if not registered then
                NativeLibrary.SetDllImportResolver(
                    Assembly.GetExecutingAssembly(),
                    DllImportResolver(fun libraryName _ _ ->
                        let fileName = libraryFileName libraryName

                        candidateDirectories ()
                        |> Seq.tryPick (fun directory ->
                            let path = Path.Combine(directory, fileName)

                            if File.Exists path then
                                let mutable handle = 0n

                                if NativeLibrary.TryLoad(path, &handle) then
                                    Some handle
                                else
                                    None
                            else
                                None)
                        |> function
                            | Some handle -> handle
                            | None when isBridgeLibrary libraryName ->
                                raise (DllNotFoundException(createMissingLibraryMessage libraryName))
                            | None -> 0n)
                )

                registered <- true)

module internal EncodingHelpers =
    do Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)

    let outboundDefault = lazy (Encoding.GetEncoding("GBK"))
    let inboundDefault = lazy (Encoding.GetEncoding("GB18030"))

    let normalize (value: string option) =
        match value with
        | Some text when String.IsNullOrWhiteSpace text -> None
        | _ -> value

    let encodeFixed (encoding: Encoding) size (value: string option) =
        let buffer = Array.zeroCreate<byte> size

        match normalize value with
        | None -> buffer
        | Some text ->
            let encoded = encoding.GetBytes text

            if encoded.Length >= size then
                invalidArg (nameof value) $"Encoded byte length {encoded.Length} exceeds fixed field size {size - 1}."

            Array.Copy(encoded, buffer, encoded.Length)
            buffer

    let decodeFixed (encoding: Encoding) (buffer: byte array) =
        if isNull buffer then
            ""
        else
            let length =
                match Array.tryFindIndex ((=) 0uy) buffer with
                | Some index -> index
                | None -> buffer.Length

            encoding.GetString(buffer, 0, length)

    let charToByte (value: char option) =
        match value with
        | Some c -> byte c
        | None -> 0uy

    let byteToChar (value: byte) = if value = 0uy then None else Some(char value)

    let ptrToOption<'T when 'T: struct> (ptr: nativeint) =
        if ptr = 0n then
            None
        else
            Some(Marshal.PtrToStructure<'T>(ptr))

type EncodingPair =
    { OutboundEncoding: Encoding
      InboundEncoding: Encoding }

    static member Default =
        BridgeResolver.ensureRegistered ()

        { OutboundEncoding = EncodingHelpers.outboundDefault.Value
          InboundEncoding = EncodingHelpers.inboundDefault.Value }

module internal NumericHelpers =
    let invalidPrice = -1m

    let tryDecimal (value: float) =
        if Double.IsNaN value || Double.IsInfinity value then
            None
        elif value > float Decimal.MaxValue || value < float Decimal.MinValue then
            None
        else
            Some(decimal value)

    let decimalOr defaultValue value = tryDecimal value |> Option.defaultValue defaultValue

    let priceOrInvalid value = decimalOr invalidPrice value

type RspInfo = { ErrorId: int; ErrorMessage: string; RawErrorMessage: byte array }

type SpecificInstrument = { InstrumentId: string }

type UserLoginResponse =
    { TradingDay: string
      LoginTime: string
      BrokerId: string
      UserId: string
      SystemName: string
      FrontId: int
      SessionId: int
      MaxOrderRef: string
      ShfeTime: string
      DceTime: string
      CzceTime: string
      FfexTime: string
      IneTime: string
      SysVersion: string
      GfexTime: string
      LoginDrIdentityId: int
      UserDrIdentityId: int
      LastLoginTime: string
      ReserveInfo: string }

type UserLogoutResponse = { BrokerId: string; UserId: string }

type RequestUserLogin =
    { TradingDay: string option
      BrokerId: string
      UserId: string
      Password: string
      UserProductInfo: string
      InterfaceProductInfo: string option
      ProtocolInfo: string option
      MacAddress: string option
      OneTimePassword: string option
      LoginRemark: string option
      ClientIpPort: int option
      ClientIPAddress: string option
      SmsCode: string option }

    static member Create(brokerId: string, userId: string, password: string, ?userProductInfo: string) =
        { TradingDay = None
          BrokerId = brokerId
          UserId = userId
          Password = password
          UserProductInfo = defaultArg userProductInfo ""
          InterfaceProductInfo = None
          ProtocolInfo = None
          MacAddress = None
          OneTimePassword = None
          LoginRemark = None
          ClientIpPort = None
          ClientIPAddress = None
          SmsCode = None }

type RequestUserLogout =
    { BrokerId: string
      UserId: string }

    static member Create(brokerId: string, userId: string) = { BrokerId = brokerId; UserId = userId }

[<Struct; StructLayout(LayoutKind.Sequential)>]
type internal NativeRspInfo =
    [<DefaultValue>]
    val mutable ErrorId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable ErrorMsg: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type internal NativeSpecificInstrument =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 81)>]
    [<DefaultValue>]
    val mutable InstrumentId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type internal NativeReqUserLogin =
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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)>]
    [<DefaultValue>]
    val mutable LoginRemark: byte array

    [<DefaultValue>]
    val mutable ClientIpPort: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)>]
    [<DefaultValue>]
    val mutable ClientIpAddress: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable SmsCode: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type internal NativeUserLogout =
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)>]
    [<DefaultValue>]
    val mutable BrokerId: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)>]
    [<DefaultValue>]
    val mutable UserId: byte array

[<Struct; StructLayout(LayoutKind.Sequential)>]
type internal NativeRspUserLogin =
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

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 41)>]
    [<DefaultValue>]
    val mutable SysVersion: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)>]
    [<DefaultValue>]
    val mutable GfexTime: byte array

    [<DefaultValue>]
    val mutable LoginDrIdentityId: int

    [<DefaultValue>]
    val mutable UserDrIdentityId: int

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)>]
    [<DefaultValue>]
    val mutable LastLoginTime: byte array

    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 65)>]
    [<DefaultValue>]
    val mutable ReserveInfo: byte array

type internal EncodedCString private (pointer: nativeint) =
    member _.Pointer = pointer

    static member Create(encoding: Encoding, value: string option) =
        let bytes =
            match value |> EncodingHelpers.normalize with
            | None -> [| 0uy |]
            | Some text ->
                let encoded = encoding.GetBytes text
                Array.append encoded [| 0uy |]

        let pointer = Marshal.AllocHGlobal bytes.Length
        Marshal.Copy(bytes, 0, pointer, bytes.Length)
        new EncodedCString(pointer)

    interface IDisposable with
        member this.Dispose() =
            if this.Pointer <> 0n then
                Marshal.FreeHGlobal this.Pointer

module internal BridgeMapping =
    let rspInfo encoding (value: NativeRspInfo) =
        let raw =
            if isNull value.ErrorMsg then
                Array.empty
            else
                Array.copy value.ErrorMsg

        { ErrorId = value.ErrorId
          ErrorMessage = EncodingHelpers.decodeFixed encoding raw
          RawErrorMessage = raw }

    let specificInstrument encoding (value: NativeSpecificInstrument) =
        { InstrumentId = EncodingHelpers.decodeFixed encoding value.InstrumentId }

    let userLogin encoding (value: NativeRspUserLogin) =
        { TradingDay = EncodingHelpers.decodeFixed encoding value.TradingDay
          LoginTime = EncodingHelpers.decodeFixed encoding value.LoginTime
          BrokerId = EncodingHelpers.decodeFixed encoding value.BrokerId
          UserId = EncodingHelpers.decodeFixed encoding value.UserId
          SystemName = EncodingHelpers.decodeFixed encoding value.SystemName
          FrontId = value.FrontId
          SessionId = value.SessionId
          MaxOrderRef = EncodingHelpers.decodeFixed encoding value.MaxOrderRef
          ShfeTime = EncodingHelpers.decodeFixed encoding value.ShfeTime
          DceTime = EncodingHelpers.decodeFixed encoding value.DceTime
          CzceTime = EncodingHelpers.decodeFixed encoding value.CzceTime
          FfexTime = EncodingHelpers.decodeFixed encoding value.FfexTime
          IneTime = EncodingHelpers.decodeFixed encoding value.IneTime
          SysVersion = EncodingHelpers.decodeFixed encoding value.SysVersion
          GfexTime = EncodingHelpers.decodeFixed encoding value.GfexTime
          LoginDrIdentityId = value.LoginDrIdentityId
          UserDrIdentityId = value.UserDrIdentityId
          LastLoginTime = EncodingHelpers.decodeFixed encoding value.LastLoginTime
          ReserveInfo = EncodingHelpers.decodeFixed encoding value.ReserveInfo }

    let userLogout encoding (value: NativeUserLogout) : UserLogoutResponse =
        { BrokerId = EncodingHelpers.decodeFixed encoding value.BrokerId
          UserId = EncodingHelpers.decodeFixed encoding value.UserId }

module internal BridgeBuilders =
    let reqUserLogin encoding (request: RequestUserLogin) =
        let mutable native = NativeReqUserLogin()
        native.TradingDay <- EncodingHelpers.encodeFixed encoding 9 request.TradingDay
        native.BrokerId <- EncodingHelpers.encodeFixed encoding 11 (Some request.BrokerId)
        native.UserId <- EncodingHelpers.encodeFixed encoding 16 (Some request.UserId)
        native.Password <- EncodingHelpers.encodeFixed encoding 41 (Some request.Password)
        native.UserProductInfo <- EncodingHelpers.encodeFixed encoding 11 (Some request.UserProductInfo)
        native.InterfaceProductInfo <- EncodingHelpers.encodeFixed encoding 11 request.InterfaceProductInfo
        native.ProtocolInfo <- EncodingHelpers.encodeFixed encoding 11 request.ProtocolInfo
        native.MacAddress <- EncodingHelpers.encodeFixed encoding 21 request.MacAddress
        native.OneTimePassword <- EncodingHelpers.encodeFixed encoding 41 request.OneTimePassword
        native.LoginRemark <- EncodingHelpers.encodeFixed encoding 36 request.LoginRemark
        native.ClientIpPort <- defaultArg request.ClientIpPort 0
        native.ClientIpAddress <- EncodingHelpers.encodeFixed encoding 33 request.ClientIPAddress
        native.SmsCode <- EncodingHelpers.encodeFixed encoding 41 request.SmsCode
        native

    let reqUserLogout encoding (request: RequestUserLogout) =
        let mutable native = NativeUserLogout()
        native.BrokerId <- EncodingHelpers.encodeFixed encoding 11 (Some request.BrokerId)
        native.UserId <- EncodingHelpers.encodeFixed encoding 16 (Some request.UserId)
        native

module internal BridgeHelpers =
    let ptrToAnsiString (ptr: nativeint) = if ptr = 0n then "" else Marshal.PtrToStringAnsi(ptr)

    let throwOnNonZero code operation =
        if code <> 0 then
            invalidOp $"{operation} failed with native return code {code}."

    let withEncodedCString (encoding: Encoding) (value: string option) (action: nativeint -> 'T) =
        use encoded = EncodedCString.Create(encoding, value)
        action encoded.Pointer

    let withEncodedStringArray (encoding: Encoding) (values: string seq) (action: nativeint seq -> 'T) =
        let encoded = values |> Seq.map (fun value -> EncodedCString.Create(encoding, Some value))

        try
            let pointers = encoded |> Seq.map (fun value -> value.Pointer)
            action pointers
        finally
            encoded |> Seq.iter (fun value -> (value :> IDisposable).Dispose())
