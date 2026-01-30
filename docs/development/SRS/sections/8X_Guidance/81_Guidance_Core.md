---
title: 81 - Guidance Core
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
notes: Defines guidance intent generation and constraints.
---

# 81 - Guidance Core
*(Status: Drafting - Guidance Intent)*

**Section ID:** 81  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2026-01-23  
**Related Sections:** 61 - Kinematics; 82 - Path Planning; 83 - Autosteer Models  
**Upstream Dependencies:** 61 - Kinematics; 82 - Path Planning  
**Downstream Impacts:** 83 - Autosteer Models; 62 - Section Control

---

## 81.1 Purpose & Scope

Define how guidance intent (desired heading, curvature, and path adherence) is generated from pose and guidance paths.

---

## 81.2 Context

- Guidance intent drives autosteer and section control.
- It must respect boundaries, headlands, and operator overrides.
- Out of scope: path creation (Section 82).

---

## 81.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference / Source |
|--------------|-----------------|------------------------|---------------------------|--------------------|
| Guidance | Mixed algorithms per mode. | Inconsistent behavior. | Unified intent contract. | AOG guidance |
| Overrides | Manual only. | No audit trail. | Logged operator intent changes. | Field feedback |

---

## 81.4 Definitions

| Term | Definition |
|------|-------------|
| Guidance Intent | Desired heading, curvature, and speed alignment for autosteer. |
| Cross-track error | Lateral distance from desired path. |

---

## 81.5 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-81-000 | MUST | Capability | Guidance MUST compute intent at >= 50 Hz aligned to SimClock. | C-22.2 | Intent stream rate verified. |
| R-81-001 | SHOULD | Accuracy | Cross-track error SHOULD be <= 5 cm p95 on RTK under nominal conditions. | C-61.1 | Field tests meet target. |
| R-81-002 | MUST | Safety | Guidance MUST clamp curvature and steering rates within equipment limits. | C-34.0 | Limits enforced in tests. |
| R-81-003 | MUST | Reliability | Loss of path data MUST trigger safe intent outputs within 100 ms. | C-22.4 | Fault injection meets timing. |
| R-81-004 | MUST | Capability | Guidance MUST support tractor-on-path and tool-on-path intent modes. | C-34.0 | Mode switching tests validate outputs. |

---

## 81.6 Acceptance Criteria & Verification

- Guidance latency and cross-track tests pass.
- Limit enforcement tests validate clamp behavior.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial Guidance Core requirements. | Systems Engineering & Documentation Lead | |
