# Logging Guidelines

> How logging is done in this project.

---

## Overview

Uses `Microsoft.Extensions.Logging.Abstractions` — the standard .NET logging abstraction. Consumers provide an `ILoggerFactory` via optional constructor parameter. If no factory is provided, `NullLoggerFactory.Instance` is used (silent, zero allocation).

---

## Log Levels

| Level | When |
|-------|------|
| `Debug` | Native call traces, subscribe/unsubscribe, query dispatch, request accepted |
| `Information` | Lifecycle: connect success, disconnect, login/logout/auth success |
| `Warning` | Connection timeout, cancellation, RspError propagating to all pending ops |
| `Error` | Native call failures, CTP error responses (login reject, order reject, etc.) |

---

## Structured Logging

Log messages use message templates with named parameters (not string interpolation):

```fsharp
logger.LogError("CTP error: [{ErrorId}] {ErrorMessage}", info.ErrorId, info.ErrorMessage)
logger.LogInformation("Md login succeeded")
logger.LogWarning("Connection attempt timed out after {TimeoutSeconds}s", value.TotalSeconds)
```

---

## What to Log

- Connection lifecycle: init start, front connected, front disconnected
- Authentication flow: authenticate, settlement confirm, login, logout — success and failure
- Native call errors: when `api.Req*` returns non-zero
- CTP error callbacks (RspError) with ErrorId and ErrorMessage
- Order insert/cancel results
- Query request lifecycle

---

## What NOT to Log

- Sensitive credentials (password, auth code) — never include in log messages
- Every individual market data tick (RtnDepthMarketData) — too high volume
