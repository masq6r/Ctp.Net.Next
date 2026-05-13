# Quality Guidelines

> Code quality standards for backend development.

---

## Overview

<!--
Document your project's quality standards here.

Questions to answer:
- What patterns are forbidden?
- What linting rules do you enforce?
- What are your testing requirements?
- What code review standards apply?
-->

(To be filled by the team)

---

## Forbidden Patterns

<!-- Patterns that should never be used and why -->

(To be filled by the team)

---

## Required Patterns

<!-- Patterns that must always be used -->

(To be filled by the team)

---

## Testing Requirements

<!-- What level of testing is expected -->

(To be filled by the team)

---

## Scenario: CTP Trader query signatures must preserve SDK-required fields

### 1. Scope / Trigger
- Trigger: adding or changing a `TraderClient` query wrapper for a CTP `ReqQry*` API.
- Trigger: changing request records in `Ctp.Net/Bridge/TraderBridge.fs` that are marshalled into CTP query structs.
- Trigger: cross-layer query work spanning public API -> managed interop -> native bridge.

### 2. Signatures
- Public wrapper signatures in `Ctp.Net/Trader.fs` must require every SDK-required request field except values that are always sourced from `CtpOptions`.
- Bridge request records in `Ctp.Net/Bridge/TraderBridge.fs` must model SDK-required fields as non-optional F# fields.
- Current examples:
  - `QueryInstrumentMarginRateAsync(hedgeFlag: char, instrumentId: string, ?exchangeId: string, ?investUnitId: string)`
  - `QueryExchangeMarginRateAsync(hedgeFlag: char, instrumentId: string, ?exchangeId: string)`
  - `QueryInstrumentCommissionRateAsync(instrumentId: string, ?exchangeId: string, ?investUnitId: string)`

### 3. Contracts
- `ReqQryInstrumentMarginRate`
  - Required from SDK contract: `BrokerID`, `InvestorID`, `InstrumentID`, `HedgeFlag`
  - Optional from SDK contract: `ExchangeID`, `InvestUnitID`
  - Managed wrapper contract: `BrokerID` <- `options.BrokerId`, `InvestorID` <- `options.UserId`, caller must provide `instrumentId` and `hedgeFlag`
- `ReqQryExchangeMarginRate`
  - Required from SDK contract: `BrokerID`, `InstrumentID`, `HedgeFlag`
  - Optional from SDK contract: `ExchangeID`
  - Managed wrapper contract: `BrokerID` <- `options.BrokerId`, caller must provide `instrumentId` and `hedgeFlag`
- `ReqQryInstrumentCommissionRate`
  - Required from SDK contract: `BrokerID`, `InvestorID`, `InstrumentID`
  - Optional from SDK contract: `ExchangeID`, `InvestUnitID`
  - Managed wrapper contract: `BrokerID` <- `options.BrokerId`, `InvestorID` <- `options.UserId`, caller must provide `instrumentId`
- Marshalling rule: required fields must be encoded with `Some request.<Field>` when building native structs; optional fields may use option values directly.

### 4. Validation & Error Matrix
- SDK marks field as required -> public wrapper must not expose it as optional unless the library always derives it internally from `CtpOptions`
- Public wrapper makes an SDK-required field optional -> bug; fix the wrapper signature and the bridge request record together
- Bridge request record keeps an SDK-required field as `option` -> bug; fix the record and builder together
- Native builder encodes a required field without `Some ...` -> bug; the managed contract is not enforcing the SDK requirement at the marshalling boundary

### 5. Good/Base/Bad Cases
- Good: wrapper requires `instrumentId` / `hedgeFlag` where the SDK requires them, while still auto-filling `BrokerId` / `InvestorId` from client options
- Base: SDK-optional fields such as `ExchangeID` or `InvestUnitID` remain optional in the wrapper and request record
- Bad: mirroring every SDK struct field as optional just because the native struct is zero-initializable

### 6. Tests Required
- Build both `Tests/Ctp.Net.Tests/Ctp.Net.Tests.fsproj` and `Tests/Ctp.Net.SmokeTests/Ctp.Net.SmokeTests.fsproj`
- Run the main unit suite after signature changes
- Update any smoke/integration call sites so they provide newly required arguments
- Assertion points:
  - public query methods still compile at call sites with the required arguments
  - bridge request builders marshal required fields as non-empty encoded values
  - no reflection-only API-shape test should be added just to assert method signatures

### 7. Wrong vs Correct
#### Wrong
```fsharp
member this.QueryExchangeMarginRateAsync(?hedgeFlag: char, ?exchangeId: string, ?instrumentId: string) =
    let request =
        { BrokerId = options.BrokerId
          HedgeFlag = hedgeFlag
          ExchangeId = exchangeId
          InstrumentId = instrumentId }
```

#### Correct
```fsharp
member this.QueryExchangeMarginRateAsync(hedgeFlag: char, instrumentId: string, ?exchangeId: string) =
    let request: QueryExchangeMarginRateRequest =
        { BrokerId = options.BrokerId
          HedgeFlag = hedgeFlag
          ExchangeId = exchangeId
          InstrumentId = instrumentId }
```

---

## Code Review Checklist

<!-- What reviewers should check -->

(To be filled by the team)
