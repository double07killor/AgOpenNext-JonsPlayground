---
title: 83 - Autosteer Models
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
notes: Defines autosteer control models, tuning, and safety gates.
---

# 83 - Autosteer Models
*(Status: Drafting - Steering Control)*

**Section ID:** 83  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2026-01-23  
**Related Sections:** 81 - Guidance Core; 52 - AgIO Core; 34 - Equipment Configurations  
**Upstream Dependencies:** 81 - Guidance Core; 52 - AgIO Core  
**Downstream Impacts:** 52 - AgIO Core; 64 - Monitoring

---

## 83.1 Purpose & Scope

Define the steering control models, tuning parameters, and engagement logic that deliver high-precision autosteer.

---

## 83.2 Context

- Autosteer consumes guidance intent and pose.
- It must meet strict latency and safety constraints.
- Out of scope: physical actuator wiring (Section 52).

---

## 83.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference / Source |
|--------------|-----------------|------------------------|---------------------------|--------------------|
| Steering models | Limited set. | Hard to tune per machine. | Multiple models with calibration profiles. | AOG autosteer |
| Safety gates | Manual checks. | Inconsistent behavior. | Centralized gating and watchdogs. | Field issues |

---

## 83.4 Definitions

| Term | Definition |
|------|-------------|
| Autosteer Model | Control algorithm mapping intent to actuator commands. |
| Engage | Command that activates autosteer. |
| Disengage | Command that deactivates autosteer. |

---

## 83.5 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-83-000 | MUST | Capability | Autosteer MUST run a steering control loop at >= 50 Hz. | C-22.2 | Loop rate verified under load. |
| R-83-001 | MUST | Safety | Autosteer MUST disengage within 100 ms when safety conditions fail. | C-22.4 | Fault injection meets timing. |
| R-83-002 | SHOULD | Accuracy | Steering command output SHOULD maintain cross-track error <= 5 cm p95 on RTK. | C-81.1 | Field tests meet target. |
| R-83-003 | MUST | Reliability | Autosteer MUST support at least two model families (e.g., PID, LQR) with per-machine tuning. | C-34.0 | Model selection tests pass. |
| R-83-004 | SHOULD | Observability | Autosteer SHOULD publish model state and error metrics for monitoring. | C-64.0 | Metrics appear in monitoring. |
| R-83-005 | MUST | Capability | Autosteer MUST support MPC as a primary controller for predictive steering and tool tracking. | C-81.1 | MPC controller runs with bounded latency at target rate. |
| R-83-006 | SHOULD | Capability | Autosteer SHOULD publish planned steering targets across the prediction horizon. | C-81.1 | Planned target stream available to UI and section lookahead. |
| R-83-007 | SHOULD | Capability | Autosteer SHOULD support tool steering actuators when available. | C-34.0 | Tool steering commands published with tractor steering. |

---

## 83.6 Acceptance Criteria & Verification

- Loop rate and disengage timing tests pass.
- Field accuracy tests meet cross-track targets.
- Model selection tests validate tuning profiles.
- MPC preview outputs are available for UI and section lookahead.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial Autosteer requirements. | Systems Engineering & Documentation Lead | |
