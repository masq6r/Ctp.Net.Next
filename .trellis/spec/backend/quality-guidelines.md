# Quality Guidelines

> Code quality standards for backend development.

---

## Overview

This is a library project (CTP SDK wrapper). No repo-specific linter or formatter is configured.

---

## Build & Test Commands

From `CLAUDE.md`:

```bash
# Main build (use -m:1 to avoid MSBuild multi-node failures)
dotnet build Ctp.Net/Ctp.Net.fsproj -m:1

# Build and run unit tests
dotnet build Tests/Ctp.Net.Tests/Ctp.Net.Tests.fsproj -m:1
dotnet run --project Tests/Ctp.Net.Tests/Ctp.Net.Tests.fsproj --no-restore

# Run smoke tests (requires smoke.local.json)
dotnet run --project Tests/Ctp.Net.SmokeTests/Ctp.Net.SmokeTests.fsproj
```

---

## Forbidden Patterns

- Do not introduce a linter/formatter not already in the repo.
- Do not use VSTest compatibility mode — test projects use pure Microsoft.Testing.Platform.
- Do not assume a solution file exists — prefer project-file commands.

---

## Required Patterns

- Keep the market-data and trader stacks intentionally parallel across all layers (NativeBridge → Bridge → public API → tests).
- When adding or changing a CTP field or API, consult the official CTP API reference under `NativeBridge/ctp-sdk/<version_date>/reference` first.

---

## Testing Requirements

- xUnit v3 with Microsoft.Testing.Platform (MTP) is the testing framework.
- `Tests/Ctp.Net.Tests/` for fast behavioral unit tests.
- `Tests/Ctp.Net.SmokeTests/` for live integration tests (requires `smoke.local.json` with real credentials).

---

## Code Review Checklist

- [ ] Encoding policy correct? (outbound GBK, inbound GB18030)
- [ ] `decimal` used for price/currency values (not native floating-point)?
- [ ] Native init sequencing correct? (e.g., `TraderClient.Connect()` subscribes topics before `Init()`)
- [ ] Coordinated edits across all layers? (header → C++ impl → F# structs → public API → tests)
