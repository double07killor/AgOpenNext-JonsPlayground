---
title: ADR 11-002 — Android Full Stack + Companion Candidate
version: 0.1.0
status: Draft
authors:
  - Jon Fortney
owner: Systems Engineering
reviewers:
  - Systems Engineering & Maintainers
approvers:
  - Project Coordinator
created: 2025-11-12
last_reviewed: 2025-11-12
review_cycle: Annual
notes: Draft exploring Android at the charter's stretch priority (G2/P2).
---

# ADR 11-002 — Android Full Stack + Companion Candidate
*(Status: Draft — 2025-11-12)*

## Context

The project charter flags Android as a P2 stretch target under the cross-platform goal (G2). Many contributors expect Android to host both full-stack (Core + UI) deployments and companion scenarios that mirror desktop workflows while remaining lightweight.

## Decision (Draft)

Propose Android as the next platform once windows/Linux parity is confirmed: full-stack Core + UI bundles (APK + native libraries) plus a companion-only mode leveraging the same UI Bridge as remote clients. This document records intentions only; actual expansions require a separate approval and will not ship until after the Windows/Linux baseline is stable.

## Consequences

- Investigation and prototypes focus on verifying Android GPU/input performance before declaring it supported.
- Companion workflow plugs into the UI Bridge, reducing duplicated transport plumbing.
- Until governance approves, Android remains a preview target with no release guarantees.

## Follow-Up

- Document the Android smoke suite requirements and parity checklist for future governance review.
- Run prototype builds on representative Android hardware to gather stability data.
- If adoption paths align with the charter’s readiness criteria (functionality, QA coverage, operator validation), promote this ADR to a full approval cycle.

## Change Log

| Version | Date | Changes | Author | PR / Issue |
|---------|------|---------|--------|------------|
| 0.1.0 | 2025-11-12 | Created stretch-target candidate for Android. | Jon Fortney |  |
