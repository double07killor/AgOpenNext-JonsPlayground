---
title: 71 - Mapping Core
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
notes: Defines core mapping, coverage, and layer update rules.
---

# 71 - Mapping Core
*(Status: Drafting - Coverage and Layers)*

**Section ID:** 71  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2026-01-23  
**Related Sections:** 33 - Mapping Storage; 61 - Kinematics; 62 - Section Control  
**Upstream Dependencies:** 33 - Mapping Storage; 61 - Kinematics  
**Downstream Impacts:** 72 - Layer Management; 73 - Rendering

---

## 71.1 Purpose & Scope

Define how live coverage, layers, and mapping state are computed and published for UI and logging.

---

## 71.2 Context

- Mapping uses pose and section control events.
- Mapping output must align with CRS and unit conventions.
- Out of scope: rendering (Section 73).

---

## 71.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference / Source |
|--------------|-----------------|------------------------|---------------------------|--------------------|
| Coverage | Mixed formats. | Hard to replay. | Contract-based coverage layers. | AOG mapping |
| Layer updates | UI-driven. | No central source. | Core authoritative map layers. | UI issues |

---

## 71.4 Definitions

| Term | Definition |
|------|-------------|
| Coverage Layer | Spatial record of applied work or sensor state. |
| Layer Snapshot | Immutable view of a layer at a point in time. |

---

## 71.5 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-71-000 | MUST | Capability | Mapping MUST publish coverage updates at >= 10 Hz. | C-31.5 | Update rate verified in tests. |
| R-71-001 | MUST | Accuracy | Coverage geometry MUST align to CRS metadata with <= 2 cm drift in replay. | C-23.1 | Replay drift tests pass. |
| R-71-002 | SHOULD | Performance | Layer snapshots SHOULD be available within 200 ms of request. | C-41.0 | Snapshot benchmark meets target. |
| R-71-003 | MUST | Reliability | Mapping MUST be thread-safe and deterministic across replay. | C-22.2 | Determinism tests pass. |

---

## 71.6 Acceptance Criteria & Verification

- Coverage update tests validate rate and accuracy.
- Replay tests confirm deterministic layer outputs.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial Mapping Core requirements. | Systems Engineering & Documentation Lead | |
