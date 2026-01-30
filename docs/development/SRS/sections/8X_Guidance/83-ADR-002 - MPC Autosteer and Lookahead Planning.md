---
title: 83-ADR-002 - MPC Autosteer and Lookahead Planning
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

# 83-ADR-002 - MPC Autosteer and Lookahead Planning

## Status

Accepted.

## Context

Autosteer must drive both tractor and implement positioning with high accuracy. Classic PID-only loops struggle to handle future path curvature, tool trailing, and lookahead-based section timing. A model predictive controller (MPC) can account for future path constraints, actuator limits, and implement geometry.

## Decision

Use MPC as the primary high-precision autosteer controller. The MPC will consume:

- Vehicle pose and kinematic model state.
- Planned path segments with curvature and constraints.
- Implement geometry and articulation model.
- Actuator limits and steering dynamics.

The controller must generate a steering command sequence over a prediction horizon and publish the planned path preview for UI visualization and section timing.

## Rationale

- MPC can plan ahead on curved paths and headlands.
- It can incorporate implement trailing and articulation constraints.
- Preview output enables operator trust and improves section timing.

## Consequences

- Requires additional compute and tuning compared to PID.
- Needs an accurate vehicle/implement model and constraints.
- A simplified fallback controller remains required for low-end hardware.

## Related References

- `docs/development/SRS/sections/8X_Guidance/83_Autosteer_Models.md`
- `docs/development/SRS/sections/8X_Guidance/82_Path_Planning.md`
- `docs/development/SRS/references/guidance/path-lookahead.md`
