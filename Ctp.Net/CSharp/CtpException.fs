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

type CtpEventArgs<'T1, 'T2>(item1: 'T1, item2: 'T2) =
    inherit EventArgs()

    member _.Item1 = item1
    member _.Item2 = item2

module internal CSharpHelpers =
    open Microsoft.FSharp.Control

    let nullToOption (v: 'T when 'T : null) =
        if obj.ReferenceEquals(v, null) then None else Some v

    let nullableToOption (v: Nullable<'T>) =
        if v.HasValue then Some v.Value else None

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
        let! requestId = Async.StartAsTask(comp, cancellationToken = ct)
        return requestId
    }
