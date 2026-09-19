namespace Ctp.Net.CSharp

open System
open System.Threading
open System.Threading.Tasks
open System.Runtime.InteropServices
open Ctp.Net
open Ctp.Net.Bridge

type CtpException(errorId: int, message: string) =
    inherit Exception(message)

    member _.ErrorId = errorId

type CtpConnectionException(message: string, ?innerException: Exception) =
    inherit CtpException(-1, message)

type CtpTimeoutException(timeout: TimeSpan, operationName: string) =
    inherit CtpException(-10001, $"%s{operationName} timed out after %.1f{timeout.TotalSeconds}s")

type CtpResponseException(errorId: int, errorMessage: string) =
    inherit CtpException(errorId, errorMessage)

type CtpNativeException(returnCode: int, operationName: string) =
    inherit CtpException(returnCode, $"%s{operationName} failed with native return code %d{returnCode}")

    member _.ReturnCode = returnCode
    member _.OperationName = operationName

type CtpEventArgs<'T1, 'T2>(item1: 'T1, item2: 'T2) =
    inherit EventArgs()

    member _.Item1 = item1
    member _.Item2 = item2

[<Sealed>]
type TraderAsyncErrorEventArgs<'TPayload>
    (
        callbackName: string,
        payload: 'TPayload,
        hasPayload: bool,
        rspInfo: RspInfo,
        hasRspInfo: bool
    ) =
    inherit EventArgs()

    member _.CallbackName = callbackName
    member _.Payload = payload
    member _.HasPayload = hasPayload
    member _.RspInfo = rspInfo
    member _.HasRspInfo = hasRspInfo

[<Sealed>]
type TraderCommandResponseEventArgs<'TPayload>
    (
        callbackName: string,
        operationName: string,
        requestId: int,
        isLast: bool,
        payload: 'TPayload,
        hasPayload: bool,
        rspInfo: RspInfo,
        hasRspInfo: bool
    ) =
    inherit EventArgs()

    member _.CallbackName = callbackName
    member _.OperationName = operationName
    member _.RequestId = requestId
    member _.IsLast = isLast
    member _.Payload = payload
    member _.HasPayload = hasPayload
    member _.RspInfo = rspInfo
    member _.HasRspInfo = hasRspInfo

[<Sealed>]
type MdClientOptions private (options: CtpOptions, endpoint: CtpEndpoint) =
    member _.Options = options
    member internal _.Endpoint = endpoint
    member val Encodings = Unchecked.defaultof<CtpEncodingOptions> with get, set
    member val UseUdp = false with get, set
    member val UseMulticast = false with get, set
    member val LoggerFactory = Unchecked.defaultof<Microsoft.Extensions.Logging.ILoggerFactory> with get, set
    member val FlowControl = Unchecked.defaultof<CtpFlowControlOptions> with get, set
    member val AutoResubscribe = true with get, set

    static member FromFront(options: CtpOptions) =
        MdClientOptions(options, CtpEndpoint.Front options.FrontAddress)

    static member FromNameServer
        (options: CtpOptions, address: string, [<Optional>] fens: FensUserInfoRequest)
        =
        let fens =
            if obj.ReferenceEquals(box fens, null) then None else Some fens

        MdClientOptions(options, CtpEndpoint.NameServer(address, fens))

[<Sealed>]
type TraderClientOptions private (options: CtpOptions, endpoint: CtpEndpoint) =
    member _.Options = options
    member internal _.Endpoint = endpoint
    member val Encodings = Unchecked.defaultof<CtpEncodingOptions> with get, set
    member val PrivateTopicResumeType = Nullable<ResumeType>() with get, set
    member val PrivateTopicSequenceNo = Nullable<int>() with get, set
    member val PublicTopicResumeType = Nullable<ResumeType>() with get, set
    member val LoggerFactory = Unchecked.defaultof<Microsoft.Extensions.Logging.ILoggerFactory> with get, set
    member val FlowControl = Unchecked.defaultof<CtpFlowControlOptions> with get, set

    static member FromFront(options: CtpOptions) =
        TraderClientOptions(options, CtpEndpoint.Front options.FrontAddress)

    static member FromNameServer
        (options: CtpOptions, address: string, [<Optional>] fens: FensUserInfoRequest)
        =
        let fens =
            if obj.ReferenceEquals(box fens, null) then None else Some fens

        TraderClientOptions(options, CtpEndpoint.NameServer(address, fens))

module internal CSharpHelpers =
    open Microsoft.FSharp.Control

    let nullToOption (v: 'T when 'T : null) =
        if obj.ReferenceEquals(v, null) then None else Some v

    let valueToOption (v: 'T) =
        if obj.ReferenceEquals(box v, null) then None else Some v

    let nullableToOption (v: Nullable<'T>) =
        if v.HasValue then Some v.Value else None

    let private optionalValue value =
        match value with
        | Some item -> item, true
        | None -> Unchecked.defaultof<_>, false

    let asyncErrorEventArgs (data: TraderAsyncErrorData<'TPayload>) =
        let payload, hasPayload = optionalValue data.Payload
        let rspInfo, hasRspInfo = optionalValue data.RspInfo
        TraderAsyncErrorEventArgs(data.CallbackName, payload, hasPayload, rspInfo, hasRspInfo)

    let commandResponseEventArgs (data: TraderCommandResponseData<'TPayload>) =
        let payload, hasPayload = optionalValue data.Payload
        let rspInfo, hasRspInfo = optionalValue data.RspInfo

        TraderCommandResponseEventArgs(
            data.CallbackName,
            data.OperationName,
            data.RequestId,
            data.IsLast,
            payload,
            hasPayload,
            rspInfo,
            hasRspInfo
        )

    let startAsync (ct: CancellationToken) (comp: Async<Result<'T, RspInfo>>) = task {
        let! result = Async.StartAsTask(comp, cancellationToken = ct)

        match result with
        | Ok value -> return value
        | Error info -> return raise (CtpResponseException(info.ErrorId, info.ErrorMessage))
    }

    let startAsyncList (ct: CancellationToken) (comp: Async<Result<'T list, RspInfo>>) = task {
        let! result = Async.StartAsTask(comp, cancellationToken = ct)

        match result with
        | Ok items -> return (items :> System.Collections.Generic.IReadOnlyList<'T>)
        | Error info -> return raise (CtpResponseException(info.ErrorId, info.ErrorMessage))
    }

    let startConnectAsync (ct: CancellationToken) (comp: Async<Result<unit, ConnectError>>) = task {
        let! result = Async.StartAsTask(comp, cancellationToken = ct)

        match result with
        | Ok() -> return ()
        | Error(ConnectError.Timeout t) -> return raise (CtpTimeoutException(t, "Connect"))
        | Error ConnectError.Cancelled -> return raise (OperationCanceledException())
        | Error(ConnectError.NativeOperationFailed m) -> return raise (CtpConnectionException m)
    }

    let startCommandAsync (ct: CancellationToken) (comp: Async<int>) = task {
        try
            let! requestId = Async.StartAsTask(comp, cancellationToken = ct)
            return requestId
        with
        | :? NativeRequestException as ex ->
            return raise (CtpNativeException(ex.ReturnCode, ex.OperationName))
    }
