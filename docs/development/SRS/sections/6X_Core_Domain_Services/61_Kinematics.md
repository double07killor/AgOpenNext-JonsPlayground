---
title: 61 - Kinematics
version: 0.1.0
status: Draft
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
notes: Defines kinematics fusion, pose output, and accuracy targets.
---

# 61 - Kinematics
*(Status: Drafting - Pose Authority)*

**Section ID:** 61  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2026-01-23  
**Related Sections:** 21 - System Decomposition & Boundaries; 23 - Units & Coordinate Systems; 31 - Domain Data Model  
**Upstream Dependencies:** 51 - Hardware PGNs; 52 - AgIO Core; 23 - Units & Coordinates  
**Downstream Impacts:** 62 - Section Control; 63 - Rate Control; 81 - Guidance Core; 71 - Mapping Core

---

## 61.1 Purpose & Scope

Define the authoritative pose, velocity, and attitude outputs for AgOpenNext, including fusion inputs, timing, and accuracy targets.

---

## 61.2 Context

- Kinematics drives guidance, autosteer, mapping, and rate/section control.
- Output must be deterministic and SimClock aligned.
- Fusion uses GNSS, IMU, wheel speed, and steering angle with configurable lever arms.
- Out of scope: UI visualization (see Section 91/92).

---

## 61.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference / Source |
|--------------|-----------------|------------------------|---------------------------|--------------------|
| Pose fusion | GNSS-only in some modes. | Drift and jitter under low quality. | Multi-sensor fusion with IMU and wheel speed. | AOG kinematics |
| Timing | Wall clock stamps. | Replay drift. | SimClock aligned timestamps. | Replay issues |
| Accuracy | Variable by setup. | No explicit targets. | Explicit accuracy tiers by GNSS mode. | Field feedback |

---

## 61.4 Definitions

| Term | Definition |
|------|-------------|
| Pose | Position, velocity, and attitude at a specific SimClock tick. |
| RTK | Real-time kinematic correction for GNSS. |
| Fusion | Estimation combining GNSS, IMU, wheel speed, and steering angle. |

---

## 61.5 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-61-000 | MUST | Capability | The system MUST publish pose, velocity, and attitude at a fixed rate aligned to SimClock. | C-22.2 | Pose stream at >= 50 Hz with deterministic timestamps. |
| R-61-001 | MUST | Accuracy | With RTK GNSS, the system MUST achieve <= 2 cm horizontal error p95 in steady-state. | C-23.1 | Field calibration test meets target. |
| R-61-002 | SHOULD | Accuracy | With non-RTK GNSS, the system SHOULD achieve <= 30 cm horizontal error p95. | C-23.1 | Bench and field tests. |
| R-61-003 | MUST | Reliability | The system MUST tolerate temporary sensor dropouts (GNSS or IMU) without producing invalid poses. | C-22.4 | Dropout tests maintain pose validity for 5 s. |
| R-61-004 | MUST | Performance | Pose latency from sensor capture to publish MUST be <= 50 ms p95. | C-22.3 | Timing benchmarks meet target. |
| R-61-005 | SHOULD | Observability | The system SHOULD publish pose quality metrics (fix type, variance, health). | C-64.0 | Quality metrics appear in monitoring. |
| R-61-006 | MUST | Capability | The fusion model MUST accept lever arm offsets for each sensor (GNSS, IMU) and apply them in pose estimation. | C-34.0 | Lever arm tests match expected pose shifts. |
| R-61-007 | MUST | Reliability | The system MUST estimate and track IMU bias (gyro and accel) during operation. | C-61.0 | Bias stability tests meet drift limits. |
| R-61-008 | SHOULD | Calibration | The system SHOULD support calibration refinement when operator-entered offsets are inaccurate. | C-34.0 | Calibration refinement reduces residual error by >= 50%. |
| R-61-009 | MUST | Capability | The system MUST publish implement pose derived from vehicle pose and equipment geometry. | C-34.0 | Implement pose matches geometry within 2 cm. |
| R-61-010 | MUST | Capability | The system MUST publish per-section center positions for section control and rate control. | C-62.0 | Section centers align with implement geometry in simulations. |
| R-61-011 | SHOULD | Observability | The system SHOULD publish per-section pose quality or covariance when available. | C-64.0 | Quality fields present in section outputs. |
| R-61-012 | MUST | Capability | The system MUST publish per-row unit center positions for row-based control and mapping. | C-62.0 | Row centers align with implement geometry in simulations. |
| R-61-013 | SHOULD | Capability | The system SHOULD publish per-row shutoff positions to support lead and lag compensation. | C-62.0 | Shutoff points align with configured offsets in simulations. |
| R-61-014 | MUST | Accuracy | Section and row center positions MUST be within 2 cm p95 of ground truth under RTK conditions. | C-23.1 | Field validation meets target error. |

---

## 61.6 Acceptance Criteria & Verification

- Fusion regression tests validate pose accuracy and latency.
- Dropout simulations validate continuity and health flags.
- Field validation on reference hardware meets targets.
- Lever arm tests verify correct application of sensor offsets.
- Bias estimation tests validate stable drift behavior.
- Implement and row unit geometry tests validate per-row and per-section outputs.

## 61.7 References

- `docs/development/SRS/references/core/kinematics-sensor-fusion.md`
- `docs/development/SRS/references/core/kinematics-calibration.md`
- `docs/development/SRS/references/core/kinematics-simulation.md`
- `docs/development/SRS/references/core/kinematics-math.md`
- `docs/development/SRS/references/core/kinematics-test-plan.md`
- `docs/development/SRS/references/core/kinematics-ekf-spec.md`
- `docs/development/SRS/references/core/simulator-scenarios.md`
- `docs/development/SRS/references/core/kinematics-implement-geometry.md`
- `docs/development/SRS/references/core/kinematics-vehicle-model.md`
- `docs/development/SRS/references/core/kinematics-implement-types.md`
- `docs/development/SRS/references/core/kinematics-section-row-units.md`

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial kinematics requirements. | Systems Engineering & Documentation Lead | |
