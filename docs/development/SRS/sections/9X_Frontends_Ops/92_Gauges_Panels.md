---
title: 92 - Gauges & Panels
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
notes: Defines operator panels, gauges, and unit handling.
---

# 92 - Gauges & Panels
*(Status: Drafting - Operator Panels)*

**Section ID:** 92  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2026-01-23  
**Related Sections:** 91 - UI Shell; 23 - Units & Coordinate Systems  
**Upstream Dependencies:** 91 - UI Shell; 42 - UI Bridge  
**Downstream Impacts:** 94 - Remote Clients

---

## 92.1 Purpose & Scope

Define how operator panels display state, controls, and unit conversions while remaining contract-driven.

---

## 92.2 Context

- Panels must reflect canonical units and convert only for display.
- Panels must surface critical alarms and actions.
- Out of scope: mapping rendering (Section 73).

---

## 92.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference / Source |
|--------------|-----------------|------------------------|---------------------------|--------------------|
| Gauges | Hardcoded. | Slow to add new sensors. | Metadata-driven gauges. | AOG UI |
| Units | Mixed units. | Operator confusion. | Central unit conversion rules. | Field feedback |

---

## 92.4 Definitions

| Term | Definition |
|------|-------------|
| Gauge | UI element displaying a numeric or categorical value. |
| Panel Template | A layout definition driven by capability metadata. |

---

## 92.5 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-92-000 | MUST | Capability | Gauges MUST be generated from contract metadata and capability cards. | C-42.0 | Metadata-driven gauge tests pass. |
| R-92-001 | MUST | UX | Display units MUST follow the canonical unit set with user-selectable conversions. | C-23.2 | Unit toggle tests pass. |
| R-92-002 | SHOULD | Usability | Critical controls SHOULD require confirmation or safety gating. | C-42.0 | UI tests validate confirmations. |

---

## 92.6 Acceptance Criteria & Verification

- Metadata-driven gauge tests pass.
- Unit conversion tests pass.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial Gauges & Panels requirements. | Systems Engineering & Documentation Lead | |
