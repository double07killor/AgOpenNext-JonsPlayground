---
title: ADR 11-001 - Target OS Prioritization
version: 0.1.0
status: Draft
authors:
  - Jon Fortney
owner: Systems Engineering
reviewers:
  - Systems Engineering & Maintainers
approvers:
  - Project Coordinator
created: 2025-10-20
last_reviewed: 2025-11-09
review_cycle: Annual
notes: Establishes the Windows/Linux support baseline.
---

# ADR 11-001 - Target OS Prioritization
*(Status: Draft - 2025-10-25)*

## Context

AgOpenNext must deliver parity across Core and UI while avoiding scope creep from experimental builds. Legacy releases were Windows-only, and community branches introduced Linux variants without consistent verification.

## Decision

Support Windows 10/11 (x64) and Ubuntu LTS (x64/ARM64) as the only Primary platforms for full-stack deployments, aligning with the charter’s G2 cross-platform goal. Experimental/preview platforms (Android and iOS) remain out of scope until the corresponding charter stretch-target policy and follow-on approvals materialize.

## Consequences

- Focused engineering and QA effort on dual-platform parity.
- Release notes publish the supported matrix and parity checklist.
- Other OS experiments require separate approvals and carry no release guarantees.

## Follow-Up

- Maintain a living Support Matrix listing the current tiers.
- Run Windows and Linux smoke suites on every merge candidate.
- Record promotion criteria for new OS proposals.

## Change Log

| Version | Date | Changes | Author | PR / Issue |
|---------|------|---------|--------|------------|
| 0.1.0 | 2025-11-09 | Simplified context and follow-up tasks. | Jon Fortney |  |
| 0.1.0 | 2025-10-20 | Initial Windows/Linux support baseline. | Nexus Team (Fortney) |  |
