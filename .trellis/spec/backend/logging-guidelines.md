# Logging Guidelines

> Logging conventions for the managed backend layer in this project.

---

## Overview

This repository records operational logs in the managed `Ctp.Net` layer via `Microsoft.Extensions.Logging`.

Current code follows these conventions:

- `MdClient`, `TraderClient`, `ConnectionCoordinator`, and `PendingQueryDict` accept an optional logger or logger factory and default to `NullLogger` / `NullLoggerFactory` when the caller does not configure logging.
- Logging is emitted at request boundaries, connection lifecycle boundaries, and native callback error boundaries.
- Logs carry small structured facts such as `RequestId`, `QueryName`, `ReturnCode`, `ErrorId`, and counts instead of full request dumps.
- The managed wrapper is the main correlation layer. `NativeBridge` is not the primary place for request/result tracing.

Real examples live in:

- `Ctp.Net/Md.fs`
- `Ctp.Net/Trader.fs`
- `Ctp.Net/Common.fs`

---

## Log Levels

### Debug

Use `LogDebug` for request start, successful internal correlation, and non-user-facing state transitions.

Current examples:

- `"Sending login request"` in `Ctp.Net/Md.fs`
- `"Sending {QueryName} request"` in `Ctp.Net/Trader.fs`
- `"{QueryName} query completed for request {RequestId} with {ItemCount} items"` in `Ctp.Net/Common.fs`
- `"Already connected, returning immediately"` in `Ctp.Net/Common.fs`
- `"Order insert accepted, requestId={RequestId}"` in `Ctp.Net/Trader.fs`

### Information

Use `LogInformation` for notable lifecycle events and successful user-visible milestones.

Current examples:

- `"Initiating front connection"` in `Ctp.Net/Common.fs`
- `"Front disconnected"` in `Ctp.Net/Common.fs`
- `"Md login succeeded"` and `"Md logout succeeded"` in `Ctp.Net/Md.fs`

### Warning

Use `LogWarning` for recoverable abnormal behavior where the library can still translate the condition into a controlled failure or retryable state.

Current examples:

- `"Connection attempt timed out after {TimeoutSeconds}s"` in `Ctp.Net/Common.fs`
- `"Connection attempt was cancelled"` in `Ctp.Net/Common.fs`
- `"RspError isLast=true, failing all pending operations"` in `Ctp.Net/Md.fs`
- `"RspError isLast=true for request {RequestId}"` in `Ctp.Net/Trader.fs`

### Error

Use `LogError` when a native call fails immediately, when a callback carries `RspInfo.ErrorId <> 0`, or when connection startup throws.

Current examples:

- `"Login request failed with native return code {ReturnCode}"`
- `"CTP error: [{ErrorId}] {ErrorMessage}"`
- `"{QueryName} query failed for request {RequestId}: [{ErrorId}] {ErrorMessage}"`
- `logger.LogError(ex, "Native connection initiation failed: {Message}", ex.Message)` in `Ctp.Net/Common.fs`

---

## Structured Logging

Follow the message-template style already used in `Ctp.Net`.

### Required pattern

- Use structured placeholders, not interpolated log strings.
- Include only the fields needed to correlate the event.
- Prefer request correlation fields over dumping full objects.

Common fields already used in the codebase:

- `{RequestId}`
- `{QueryName}`
- `{ReturnCode}`
- `{ErrorId}`
- `{ErrorMessage}`
- `{ItemCount}`
- `{TimeoutSeconds}`
- `{InstrumentCount}`
- `{InstrumentId}`

### Good examples

```fsharp
logger.LogDebug("Sending {QueryName} request", queryName)

logger.LogError(
    "{QueryName} query failed for request {RequestId}: [{ErrorId}] {ErrorMessage}",
    queryName,
    requestId,
    info.ErrorId,
    info.ErrorMessage
)

logger.LogWarning("Connection attempt timed out after {TimeoutSeconds}s", value.TotalSeconds)
```

### Avoid

```fsharp
logger.LogError($"Login failed for {options.UserId}: {info.ErrorMessage}")
logger.LogDebug($"Request payload = %A{request}")
```

Those patterns lose structured fields and risk leaking credentials or oversized payload data.

---

## What to Log

Log the boundaries where the wrapper translates native behavior into managed behavior.

### 1. Request submission

Log when the managed wrapper submits an operation to the native API.

Examples:

- login / logout request send in `Ctp.Net/Md.fs`
- authenticate / query / order request send in `Ctp.Net/Trader.fs`

### 2. Immediate native return-code failure

If a native `Req*` call returns a non-zero code, log it before completing the pending result with a failure.

Examples:

- `Login request failed with native return code {ReturnCode}`
- `Order cancel failed with native return code {ReturnCode}`

### 3. Callback-delivered error information

When the CTP callback supplies `RspInfo`, log the error id and decoded message.

Examples:

- generic callback error in `Ctp.Net/Md.fs` and `Ctp.Net/Trader.fs`
- query finalization error in `Ctp.Net/Common.fs`

### 4. Lifecycle transitions

Log connect/disconnect and similar state changes that matter when diagnosing connection sequencing.

Examples:

- `Initiating front connection`
- `Front connected`
- `Front disconnected`

### 5. Completion summaries

For multi-item queries, log the completion summary with request correlation and item count.

Example:

- `"{QueryName} query completed for request {RequestId} with {ItemCount} items"` in `Ctp.Net/Common.fs`

---

## What NOT to Log

Do not add logs that expose secrets or mirror whole native payloads.

### Never log

- `CtpOptions.Password`
- `CtpOptions.AuthCode`
- full request records built from login, authenticate, and order APIs
- raw native byte buffers such as `RspInfo.RawErrorMessage`
- smoke-test credential files or their contents

### Prefer logging these instead

- request type
- request id
- instrument count or instrument id when needed
- native return code
- `RspInfo.ErrorId` and decoded `ErrorMessage`
- item count for query responses

---

## Design Decision: Correlate logs at the managed boundary

**Context**: The SDK is callback-driven and a single feature can cross request submission, callback dispatch, pending completion, and public event publication.

**Decision**: Keep correlation logs in the managed `Ctp.Net` layer, where `RequestId`, query names, pending completions, and result translation are visible together.

**Why**:

- this layer knows both native callback shape and managed async/event semantics
- it keeps logging consistent across `MdClient` and `TraderClient`
- it avoids leaking raw native structs just to aid debugging

**Examples**:

- `PendingQueryDict.Register` logs completion/failure with `QueryName` + `RequestId` in `Ctp.Net/Common.fs`
- `TraderClient` logs `RspError isLast=true for request {RequestId}` before failing the pending request in `Ctp.Net/Trader.fs`

---

## Common Mistakes

### Common Mistake: Logging the whole request object

**Symptom**: Logs become noisy and may expose credentials or broker/user identifiers.

**Cause**: Treating logs as object dumps instead of correlation events.

**Fix**: Log the operation name plus stable identifiers such as `RequestId`, `InstrumentId`, or `InstrumentCount`.

### Common Mistake: Logging success and failure at the same level

**Symptom**: Operational dashboards cannot distinguish healthy flow from failure flow.

**Cause**: Using `Information` or `Debug` for error-bearing `RspInfo` cases.

**Fix**: Keep `RspInfo.ErrorId <> 0` and non-zero native return codes at `Error`; use `Warning` only for controlled but abnormal states such as timeout, cancellation, or forced pending failure.
