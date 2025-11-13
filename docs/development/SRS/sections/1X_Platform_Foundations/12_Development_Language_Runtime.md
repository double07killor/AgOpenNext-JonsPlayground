---
title: 12 - Language & Runtime
version: 0.1.0
status: Draft
authors:
  - Jon Fortney
owner: Systems Engineering & Documentation Lead
reviewers:
  - Systems Engineering Team
approvers:
  - Project Coordinator
created: 2025-10-21
last_reviewed: 2025-11-09
review_cycle: Quarterly
notes: Runtime and dependency governance; updated per metadata standard.
---

# 12 - Language & Runtime
*(Status: Drafting - Decision Agnostic)*

**Section ID:** 12 | **Version:** 0.1.0  
**Editors:** Jon Fortney  
**Last Updated:** 2025-11-12  
**Related Sections:** 11 - Operating System Support, 14 - Build Tooling  
**Upstream Dependencies:** 1X - Platform Foundations, 4X - Interprocess Communications  
**Downstream Impacts:** 6X - Core Domain Services, 9X - Frontends & Ops

## 12.1 Purpose & Scope

Define the runtime, language, and dependency policies that keep every managed project (Core, UI, AgIO, CLI, plugins) on the same toolchain so Windows and Linux behavior remains identical.

## 12.2 Context

- All managed assemblies target the same managed runtime via `global.json` for reproducibility, honoring the charter version policy that keeps the latest stable .NET/Avalonia combination aligned after CI validation.  
- Shared contract packages and view models keep Core, UI, and AgIO compatible.  
- Build automation (Section 14) enforces tooling consistency reviewed here.  
- Native rewrites outside the managed ecosystem are out of scope until explicitly revisited.

## 12.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Limitation | Opportunity | Reference |
|--------------|-----------------|------------|--------------|-----------|
| Runtime | Mixed .NET Framework and .NET 6. | Divergent tooling, Linux gaps. | Single managed runtime strategy. | Runtime audit |
| Dependency | Ad-hoc packages per project. | Version drift. | Curated allowlist with CI checks. | Dependency board |
| Contracts | Manual duplication. | Plugin breakage. | Shared contract packages. | Plugin registry |

## 12.4 Definitions

| Term | Definition |
|------|-------------|
| Managed Runtime | The common runtime all managed code targets. |
| Contract Package | Versioned interface library consumed by Core, UI, plugins. |
| Allowlist | Approved dependency list reviewed via CI.

## 12.5 Requirements

| ID | Priority | Summary | Verification |
|----|----------|---------|--------------|
| R-STACK-001 | MUST | Keep every managed project on the same runtime so Windows and Linux behavior remains identical. | CI enforces a single framework version via `global.json`. |
| R-STACK-002 | MUST | Share contracts/view models so Core, UI, and AgIO reuse the same code. | Contract packages build and publish shared NuGet artifacts. |
| R-STACK-003 | MUST | Pin SDKs and dependency versions for deterministic builds. | Hash comparisons match across local, CI, and release artifacts. |
| R-STACK-004 | SHOULD | Keep OS-specific logic behind dependency-injected interfaces. | Adapter tests cover each configurable implementation. |
| R-STACK-005 | SHOULD | Publish a reviewed dependency allowlist each release. | Allowlist documentation cites approvals and review history.

## 12.6 Acceptance & Verification

- CI rejects projects that deviate from the defined framework version.  
- Contract package builds produce stable artifacts consumed downstream.  
- Dependency allowlist entries appear in release notes with review history.

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2025-11-12 | Template-aligned rewrite. | Jon Fortney |  |
| 0.1.0 | 2025-10-21 | Initial governance draft. | Nexus Team (Fortney) |  |
