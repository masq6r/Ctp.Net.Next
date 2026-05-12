# Directory Structure

> How backend code is organized in this project.

---

## Overview

This repo is a two-layer wrapper around the official CTP SDK:

1. **NativeBridge** (C++) — links the vendor SDK, exposes a stable C ABI in `include/ctp_bridge.h`.
2. **Ctp.Net** (F#) — managed wrapper. `Ctp.Net/Bridge/` owns low-level interop; top-level files wrap callback-driven native APIs into `Async<Result<...>>` and .NET events.

---

## Directory Layout

```
Ctp.Net/
├── Ctp.Net.fsproj          # Main project file
├── AssemblyInfo.fs
├── Common.fs               # Shared options, connection/request coordination primitives
├── Md.fs                   # Market-data client (MailboxProcessor agent)
├── Trader.fs               # Trader client (MailboxProcessor agent)
└── Bridge/
    ├── Common.fs           # Native library resolution, encoding, marshalling helpers
    ├── MdBridge.fs         # Market-data native interop (DllImport, structs, callbacks)
    └── TraderBridge.fs     # Trader native interop (DllImport, structs, callbacks)

NativeBridge/
├── include/
│   └── ctp_bridge.h        # Stable C ABI
├── src/
│   ├── md_bridge.cpp       # Market-data bridge implementation
│   └── trader_bridge.cpp   # Trader bridge implementation
├── ctp-sdk/<version_date>/ # Bundled CTP SDK headers and libraries
├── build.sh                # Linux build
└── build.ps1               # Windows build

Tests/
├── Ctp.Net.Tests/          # Fast behavioral unit tests (xUnit v3 MTP)
└── Ctp.Net.SmokeTests/     # Live integration tests against real CTP fronts
```

---

## Module Organization

- **`Common.fs`**: Shared domain types (`CtpOptions`, `CtpEncodingOptions`, `ConnectError`), connection coordination (`ConnectionCoordinator`, `SinglePendingResult`, `SinglePendingRequest`), and query helpers (`PendingQueryDict`). Used by both `MdClient` and `TraderClient`.
- **`Md.fs`** / **`Trader.fs`**: Public client classes. Each uses a `MailboxProcessor` agent to serialize native callbacks, complete pending async requests, and raise user-facing .NET events. These are intentionally parallel across all layers.
- **`Bridge/`**: Low-level interop layer. `DllImport` declarations, native struct layouts (`[<StructLayout>]`), callback delegate registration, `SafeHandle` lifetimes, and marshalling. Public surface is `MdApi` / `TraderApi` thin wrappers.

---

## Naming Conventions

- F# source files: PascalCase (e.g., `Common.fs`, `MdBridge.fs`).
- Namespaces: `Ctp.Net` for public API, `Ctp.Net.Bridge` for interop layer.
- Internal helpers in modules marked `internal` (e.g., `module internal ClientHelpers`).
- Agent message types: private DU suffixed with `AgentMessage` (e.g., `MdAgentMessage`).

---

## Examples

- Well-organized public API: `Ctp.Net/Trader.fs` — clear separation of agent loop, async public methods, and event exposure.
- Clean interop boundary: `Ctp.Net/Bridge/Common.fs` — native resolution, encoding defaults, fixed-width string marshalling all in one place.
