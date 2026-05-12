# Add logging to Ctp.Net

## Goal

Add structured logging to the Ctp.Net library so library internals (connection lifecycle, native call results, errors) produce log output that consuming applications can route to their chosen logging backend.

## What I already know

- `Ctp.Net` is an F# library targeting .NET 10, with only `FSharpPlus` as a dependency.
- No logging exists today — errors are surfaced via `Result` types and .NET events.
- Two public client classes: `MdClient` and `TraderClient`, each backed by a `MailboxProcessor` agent.
- `Common.fs` contains shared primitives: `ConnectionCoordinator`, `SinglePendingResult`, `PendingQueryDict`.
- `Bridge/Common.fs` handles native library resolution (already produces error messages).

## Requirements

- [x] `MdClient` and `TraderClient` accept optional `ILoggerFactory` parameter.
- [x] Internal components create typed loggers via `factory.CreateLogger<T>()`.
- [x] Log levels: Info (connect/disconnect/lifecycle), Warning (recoverable), Error (failures), Debug (native call traces).
- [x] Structured logging with message templates and named parameters.
- [x] Opt-in: no logger → silent as before, no allocation (NullLoggerFactory.Instance).
- [x] No breaking change to existing constructor signatures.

## Acceptance Criteria

- [x] `MdClient(?loggerFactory: ILoggerFactory)` and `TraderClient(?loggerFactory: ILoggerFactory)` emit log events.
- [x] Existing tests pass without changes (30/30, plus 5 new logging tests).
- [x] Log events: connect start/success/failure, disconnect, login/logout result, native API errors, query lifecycle.

## Decision (ADR-lite)

**Context**: Library targets both DI and non-DI consumers (console apps). Need a logging abstraction that works well for both.

**Decision**: Accept `ILoggerFactory` as optional constructor parameter. Internal components create typed `ILogger<T>` via the factory. Uses `Microsoft.Extensions.Logging.Abstractions` (no hard provider dependency).

**Consequences**:
- Adds one NuGet dependency (`Microsoft.Extensions.Logging.Abstractions`).
- Consumers using DI pass `ILoggerFactory` from their container; console apps use `LoggerFactory.Create(...)`.
- Internal category names follow F# class names (`Ctp.Net.MdClient`, `Ctp.Net.ConnectionCoordinator`, etc.).

## Definition of Done

- [x] Tests updated (5 new logging tests via FakeLogger).
- [x] Build passes (`dotnet build Ctp.Net/Ctp.Net.fsproj -m:1`).
- [x] No breaking API changes.

## Out of Scope (explicit)

- C++ `NativeBridge` logging.
- Configuring a specific logging provider (Serilog, NLog, etc.).
- Logging every individual native callback — start with lifecycle + errors.

## Technical Notes

- `Ctp.Net/Ctp.Net.fsproj` — current deps: `FSharpPlus` only.
- `Ctp.Net/Common.fs` — `ConnectionCoordinator`, `SinglePendingResult`, `PendingQueryDict`, `SinglePendingRequest`.
- `Ctp.Net/Md.fs` — `MdClient` class with `MailboxProcessor` agent.
- `Ctp.Net/Trader.fs` — `TraderClient` class with `MailboxProcessor` agent.
- `Ctp.Net/Bridge/Common.fs` — native library resolution and error messages.
