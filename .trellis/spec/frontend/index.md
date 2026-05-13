# Frontend Development Guidelines

> Not applicable for this repository.

---

## Overview

This repository does not contain a frontend application, browser runtime, UI component library, or TypeScript package.

Current scope is limited to:

- `NativeBridge`: native C++ bridge over the vendor CTP SDK
- `Ctp.Net`: managed F# wrapper and interop layer
- `Tests`: unit tests and smoke tests for the native/managed integration

AI agents must not invent frontend tasks, frontend architecture, or frontend conventions for normal work in this repository.

---

## Guidelines Index

| Guide | Status | Note |
|-------|--------|------|
| `directory-structure.md` | N/A | No frontend source tree exists |
| `component-guidelines.md` | N/A | No component system exists |
| `hook-guidelines.md` | N/A | No hook-based runtime exists |
| `state-management.md` | N/A | No frontend state layer exists |
| `type-safety.md` | N/A | Frontend-specific type rules do not apply |
| `quality-guidelines.md` | N/A | Frontend lint, a11y, and UI review rules do not apply |

---

## When to Revisit

Replace this section only if the repository gains a real frontend package with its own source tree, build, and test workflow.
