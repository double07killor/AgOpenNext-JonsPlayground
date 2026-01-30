---
title: 63 - Rate Control
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
notes: Defines rate control targets, compensation, and calibration.
---

# 63 - Rate Control
*(Status: Drafting - Application Control)*

**Section ID:** 63  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2026-01-23  
**Related Sections:** 61 - Kinematics; 34 - Equipment Configurations; 65 - Job Lifecycle  
**Upstream Dependencies:** 61 - Kinematics; 34 - Equipment Configurations  
**Downstream Impacts:** 52 - AgIO Core; 71 - Mapping Core

---

## 63.1 Purpose & Scope

Define how application rate targets are computed, commanded, and verified for variable rate and fixed rate operations.

---

## 63.2 Context

- Rate Control depends on pose, speed, and equipment calibration.
- It must compensate for transport delay and flow response.
- Out of scope: sensor wiring (see Section 52).

---

## 63.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference / Source |
|--------------|-----------------|------------------------|---------------------------|--------------------|
| Rate accuracy | Manual tuning. | Slow response to changes. | Automatic calibration and delay compensation. | AOG rate |
| Logging | Partial. | No traceability to jobs. | Job-linked audit trail. | Operator feedback |

---

## 63.4 Definitions

| Term | Definition |
|------|-------------|
| Target Rate | Desired application rate (e.g., kg/ha). |
| Flow Delay | Time between command and actual flow change. |
| Calibration | Mapping between command units and actual rate. |

---

## 63.5 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-63-000 | MUST | Capability | The system MUST compute target rate from prescription, speed, and implement geometry. | C-21.6 | Tests match expected target output. |
| R-63-001 | MUST | Performance | Rate response MUST settle within 1.5 s after a step change. | C-22.3 | Bench tests meet target. |
| R-63-002 | SHOULD | Accuracy | Actual rate SHOULD remain within 3% of target after settling. | C-21.6 | Field data meets target. |
| R-63-003 | MUST | Safety | Rate commands MUST be clamped to equipment min/max limits. | C-34.0 | Limit tests enforce bounds. |
| R-63-004 | SHOULD | Extensibility | The system SHOULD support per-product calibration profiles. | C-34.0 | Profiles saved and applied. |

---

## 63.6 Acceptance Criteria & Verification

- Step response and accuracy tests pass for calibrated equipment.
- Safety limit tests validate clamping.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial Rate Control requirements. | Systems Engineering & Documentation Lead | |
