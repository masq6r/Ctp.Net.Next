# Database Guidelines

> Not applicable for this repository.

---

## Overview

This repository does not contain a relational database, document database, ORM layer, query builder, migration system, or SQL schema management workflow.

Current implementation scope is:

- `NativeBridge/` for the native C++ bridge over the vendor CTP SDK
- `Ctp.Net/` for the managed F# interop and async/event wrapper
- `Tests/` for unit tests and smoke tests around the native/managed integration

There are no repository-local concepts such as:

- application tables or collections
- migration files
- database transactions
- repository or DAO layers
- SQL naming conventions

AI agents must not invent database tasks, persistence layers, ORM abstractions, or migration work unless the repository actually gains those components in the future.

---

## Query Patterns

Not applicable.

The closest thing to a request/response contract in this repository is CTP native API traffic, not database querying. Query-like operations such as trading-account and position requests are CTP API calls handled through the native bridge and managed pending-request coordination, not SQL or ORM queries.

Relevant code paths:

- `Ctp.Net/Trader.fs`
- `Ctp.Net/Common.fs`
- `NativeBridge/include/ctp_bridge.h`

---

## Migrations

Not applicable.

This repository has no schema migration tooling, migration directory, or rollout procedure.

---

## Naming Conventions

Not applicable.

There are no table, column, index, foreign-key, or migration naming rules to follow in the current codebase.

---

## Common Mistakes

### Common Mistake: Assuming a generic backend persistence layer exists

**Symptom**: Proposed changes mention repositories, ORM entities, migrations, or schema updates that do not map to any real code in this repository.

**Cause**: Treating this project like a typical CRUD backend instead of a native SDK wrapper.

**Fix**: Model data-flow changes through the actual layers that exist here: vendor CTP reference → `NativeBridge/include/ctp_bridge.h` → `NativeBridge/src/` → `Ctp.Net/Bridge/` → `Ctp.Net/` → tests.

### Common Mistake: Describing CTP requests as database queries

**Symptom**: Specs or code reviews talk about query optimization, persistence caching, or transaction boundaries for operations that are actually remote CTP API calls.

**Cause**: Reusing generic backend language instead of the project's real protocol boundary.

**Fix**: Describe these operations as CTP request/response flows, callback handling, and pending-result coordination.
