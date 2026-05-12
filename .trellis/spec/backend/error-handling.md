# Error Handling

> How errors are handled in this project.

---

## Overview

Error handling follows F# conventions: domain errors use `Result<'T, 'E>`, exceptional runtime failures use exceptions. The core error type is `RspInfo` (native CTP response info); most async operations return `Async<Result<'T, RspInfo>>`.

---

## Error Types

### `RspInfo` — native CTP error

```fsharp
// Defined in Bridge layer. Contains ErrorId, ErrorMessage, RawErrorMessage.
// ErrorId = 0 means success.
```

### `ConnectError` — connection-level failures

```fsharp
type ConnectError =
    | Timeout of TimeSpan
    | Cancelled
    | NativeOperationFailed of string
```

### Internal helpers in `Common.fs`

```fsharp
// Convert RspInfo to Result:
let resultFromRspInfo (rspInfo: RspInfo option) =
    match rspInfo with
    | Some info when info.ErrorId <> 0 -> Error info
    | _ -> Ok()

// Create error from native return code:
let apiReturnError code =
    { ErrorId = code
      ErrorMessage = $"CTP native call returned {code}"
      RawErrorMessage = Array.empty }
```

---

## Error Handling Patterns

### Async request pattern (used in all `*Async()` methods)

1. Generate a unique `requestId` via `Interlocked.Increment`.
2. Call `pending.Begin()` to register a `TaskCompletionSource<Result<'T, RspInfo>>`.
3. Call the native API. If the return code ≠ 0, immediately complete with `Error`.
4. Otherwise, the `MailboxProcessor` agent completes the TCS when the callback fires.
5. Return `completion.Task |> ClientHelpers.awaitTask`.

### Agent callback result resolution

In the MailboxProcessor loop, every `Rsp*` message with `isLast = true` resolves the pending result:

```fsharp
| RspUserLogin(login, rspInfo, _, isLast) when isLast ->
    let result =
        match ClientHelpers.resultFromRspInfo rspInfo, login with
        | Error info, _ -> Error info
        | Ok(), Some value -> Ok value
        | Ok(), None -> Error(ClientHelpers.apiReturnError -2)
    loginPending.TrySetResult result
```

Fallback error code `-2` is used when the native callback reports success but provides no response data.

### Connection error handling

`ConnectionCoordinator` wraps `startConnection` in try/with — if native init throws, it resets phase to `NotStarted` and completes the pending connection with `ConnectError.NativeOperationFailed`.

### RspError propagation

On `RspError` with `isLast = true`, all pending operations (login, logout, queries) are failed with the error info, covering cases where the front rejects a request asynchronously.

---

## API Error Responses

All public async methods return `Async<Result<'T, RspInfo>>`. Callers match on `Ok`/`Error` to handle success/failure. There is no HTTP/REST layer — this is a library consumed in-process.

---

## Common Mistakes

- Forgetting to check `isLast` before resolving a pending result — CTP may invoke callbacks multiple times per request.
- Assuming `OnRspUserLogout` will fire — the current SDK does not reliably invoke it, so `LogoutAsync` completes after a successful native request rather than waiting for the callback.
