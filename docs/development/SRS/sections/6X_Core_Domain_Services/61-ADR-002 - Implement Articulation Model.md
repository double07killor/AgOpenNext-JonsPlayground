---
title: 61-ADR-002 - Implement Articulation Model
version: 0.1.0
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

# 61-ADR-002 - Implement Articulation Model

## Status

Accepted.

## Context

Implements can be rigid, semi-mounted, or fully articulated with pivoting joints. Section positioning accuracy depends on how the implement yaw and offsets are modeled relative to the tractor reference point.

## Decision

Use a kinematic articulation model that supports three classes:

1) Rigid implement: fixed yaw relative to vehicle frame.
2) Pivoted implement: single articulation joint with measured or estimated articulation angle.
3) Trailed multi-body: two-stage articulation with optional second joint.

The implement pose is derived from the vehicle pose, hitch offset, and articulation angle(s). If articulation sensors are unavailable, the system estimates articulation from kinematics and wheel speed constraints, and marks output quality as degraded.

## Rationale

- Covers the most common equipment classes without introducing heavy dynamics.
- Supports accurate section positions for planters and toolbars.
- Allows incremental upgrades when articulation sensors are added.

## Consequences

- Requires explicit implement class and hitch definitions in equipment configuration.
- Adds state estimation for articulation when no sensor is present.
- Enforces quality flags when estimated articulation is used.

## Related References

- `docs/development/SRS/references/core/kinematics-implement-geometry.md`
- `docs/development/SRS/references/core/kinematics-vehicle-model.md`
