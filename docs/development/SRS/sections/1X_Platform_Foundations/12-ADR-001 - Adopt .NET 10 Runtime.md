---
title: ADR 12-001 - Adopt .NET 10 Runtime Across AgOpenNext
version: 0.1.0
status: Draft
authors:
  - Jon Fortney
owner: Architecture Working Group
reviewers:
  - Architecture & Release Working Groups
approvers:
  - Project Coordinator
created: 2025-10-20
last_reviewed: 2027-03-14
review_cycle: Annual
notes: Runtime selection decision.
---

# ADR 12-001 - Adopt .NET 10 Runtime Across AgOpenNext
*(Status: Draft - 2027-03-14)*

## Context

Mixed runtimes in legacy AgOpenGPS fractured build pipelines and Linux support. Sections 11 and 14 now require reproducible, cross-platform artifacts, so a single managed runtime is essential.

## Decision

Adopt the charter’s version policy (§5.1) by using the latest stable combination of **.NET** and **Avalonia** declared compatible in CI (currently .NET 10). All solutions target the corresponding `net` framework (with platform-specific tags as needed). NativeAOT experiments may proceed only if they remain compatible with the shared runtime and pass the shared smoke suites.

## Consequences

- Removes runtime fragmentation and aligns tooling across Windows and Linux.
- Simplifies Avalonia, AgIO, and plugin development under one SDK.
- Future runtime upgrades require a new ADR.

## Follow-Up

- Audit all projects for net10.0 targets.
- Update dependency policies and analyzers.
- Maintain downgrade bundles until .NET 10 is verified on all tiers.

## Change Log

| Version | Date | Changes | Author | PR / Issue |
|---------|------|---------|--------|------------|
| 0.1.0 | 2025-11-02 | Confirmed .NET 10 baseline. | Nexus Team (Fortney) |  |
| 0.1.0 | 2025-10-20 | Initial runtime policy. | Nexus Team (Fortney) |  |
