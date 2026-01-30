---
title: 61-ADR-001 - Vehicle Kinematic Model Selection
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

# 61-ADR-001 - Vehicle Kinematic Model Selection

## Status

Accepted.

## Context

The kinematics stack must provide accurate, deterministic pose and section positions for guidance, section control, and mapping. A bicycle model is simple but collapses track width and axle geometry, which introduces errors for wide implements, tight turns, and when estimating per-row positions.

## Decision

Use a multi-body kinematic model with explicit front and rear axle reference points, steering geometry, and track width. The model must support:

- Separate front and rear axle centers with wheelbase and track width.
- Steering angle applied at front axle with Ackermann geometry approximations.
- Optional slip angle and wheel speed scaling inputs when available.
- Deterministic propagation at SimClock tick rate.

A bicycle model is not used as the primary model for guidance or section position outputs.

## Rationale

- Preserves geometry needed for per-row and implement pose accuracy.
- Supports tighter turns and articulated implements without collapsing the vehicle into a single track.
- Aligns with sensor placements (IMU, GNSS) and lever arm modeling.

## Consequences

- Slightly more compute than a bicycle model, but still compatible with target hardware.
- Requires additional configuration parameters (track width, axle offsets).
- Simplified variants may still be used for low-end fallback, but only with explicit degraded accuracy flags.

## Related References

- `docs/development/SRS/references/core/kinematics-math.md`
- `docs/development/SRS/references/core/kinematics-implement-geometry.md`
- `docs/development/SRS/references/core/kinematics-vehicle-model.md`
