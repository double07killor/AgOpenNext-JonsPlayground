---
title: 82-ADR-001 - Field Coverage Planning Engine
version: 0.1.1
status: Accepted
authors:
  - Jon Fortney
owner: Systems Engineering & Documentation Lead
reviewers:
  - Systems Engineering Team
approvers:
  - Project Coordinator
created: 2026-01-23
last_reviewed: 2026-01-23
review_cycle: Quarterly
---

# 82-ADR-001 - Field Coverage Planning Engine

## Status

Accepted.

## Context

Field coverage planning must generate efficient pass ordering, headlands, and coverage routes that respect boundaries, obstacles, and implement width. The Fields2Cover project provides a mature open source baseline for coverage path planning, but AgOpenNext will implement its own planner to integrate slope-aware contour alignment and tool steering constraints.

## Decision

Use Fields2Cover as a reference and benchmark suite, not as the primary coverage engine. AgOpenNext will implement its own coverage planning pipeline that supports contour alignment, point-row minimization, and tool-on-path constraints.

## Rationale

- Maintains flexibility for slope-aware and tool steering planning.
- Allows a deterministic, tightly integrated planning pipeline.
- Still benefits from Fields2Cover as a validation reference.

## Consequences

- Requires implementing coverage planning internally.
- Requires validation against Fields2Cover outputs for baseline parity.
- Coverage planning becomes a distinct planning stage before guidance intent.

## Related References

- `docs/development/SRS/references/guidance/field-coverage-planning.md`
- `docs/development/SRS/references/guidance/contour-slope-planning.md`
- `external/references/Fields2Cover`
