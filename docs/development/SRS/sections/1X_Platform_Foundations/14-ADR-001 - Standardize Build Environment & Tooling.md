---
title: ADR 14-001 - Standardize Build Environment & Tooling
version: 0.1.0
status: Draft
authors:
  - Nexus Team (Codex)
owner: Release & Tooling Working Group
reviewers:
  - Platform Foundations Working Group
approvers:
  - Project Coordinator
created: 2025-10-25
last_reviewed: 2025-10-25
review_cycle: Annual
notes: Build & CI governance.
---

# ADR 14-001 - Standardize Build Environment & Tooling
*(Status: Draft - 2025-10-25)*

## Context

Sections 11 and 12 mandate reproducible, cross-platform builds; the charter’s release criteria (see §5, D5) require smoke suites on Windows/Linux and documented parity before promoting new tiers. Legacy tooling relied on manual setup and inconsistent signing, so shared scripts, containers, and secrets are necessary.

## Decision

Standardize around pinned toolchains, vault-based signing, cross-platform bootstrap scripts, and dual Windows/Linux CI workflows. Container images used by developers and CI must match.

## Consequences

- Reproducible, signed artifacts improve trust.
- Bootstrap scripts reduce onboarding friction.
- Dual-lane CI extends runtime but ensures parity before merges.

## Follow-Up

- Validate bootstrap scripts on clean Windows and Linux machines.
- Record container image tags and rebuild cadence.
- Document vault policies and SBOM publication steps.

## Change Log

| Version | Date | Changes | Author | PR / Issue |
|---------|------|---------|--------|------------|
| 0.1.0 | 2025-11-09 | Focused ADR on shared toolchain tasks. | Nexus Team (Codex) |  |
| 0.1.0 | 2025-10-25 | Initial standardization draft. | Nexus Team (Codex) |  |
