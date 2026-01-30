---
title: 82 - Path Planning
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
notes: Defines path planning, AB lines, headlands, and boundaries.
---

# 82 - Path Planning
*(Status: Drafting - Path Generation)*

**Section ID:** 82  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2026-01-23  
**Related Sections:** 71 - Mapping Core; 81 - Guidance Core  
**Upstream Dependencies:** 71 - Mapping Core; 61 - Kinematics  
**Downstream Impacts:** 81 - Guidance Core; 62 - Section Control

---

## 82.1 Purpose & Scope

Define how AB lines, curves, headlands, and boundaries are created, edited, and validated.

---

## 82.2 Context

- Path planning provides canonical paths for guidance and section control.
- Boundaries must be respected for safety and coverage accuracy.
- Out of scope: rendering (Section 73).

---

## 82.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference / Source |
|--------------|-----------------|------------------------|---------------------------|--------------------|
| AB lines | Basic line storage. | Limited metadata. | Versioned paths with metadata. | AOG lines |
| Headlands | Manual polygons. | Inconsistent geometry. | Automatic headland generation. | Operator feedback |

---

## 82.4 Definitions

| Term | Definition |
|------|-------------|
| AB Line | Straight guidance line defined by two points. |
| Headland | Boundary buffer for turn management. |
| Boundary | Field or no-go polygon. |

---

## 82.5 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-82-000 | MUST | Capability | The system MUST support AB lines, curves, and contour guidance paths. | C-21.2 | Path creation tests pass. |
| R-82-001 | MUST | Safety | Boundaries and exclusion zones MUST be enforced for path generation. | C-21.2 | Boundary tests reject illegal paths. |
| R-82-002 | SHOULD | UX | The system SHOULD auto-generate headlands from field boundaries and implement width. | C-21.4 | Headland generator tests pass. |
| R-82-003 | SHOULD | Accuracy | Generated paths SHOULD be repeatable within 2 cm when reloaded. | C-31.5 | Persistence round-trip test. |
| R-82-004 | MUST | Capability | Path planning MUST provide a lookahead preview for guidance and section timing. | C-81.1 | Preview generation tests pass at target horizon. |
| R-82-005 | MUST | Capability | Path planning MUST support coverage planning (swaths, headlands, pass ordering) from field boundaries. | C-71.0 | Coverage plan tests match expected swath count. |
| R-82-006 | SHOULD | Capability | Coverage planning SHOULD support tool-on-path and tractor-on-path modes. | C-34.0 | Mode tests produce distinct path sets. |
| R-82-007 | SHOULD | Capability | Coverage planning SHOULD support contour-aligned swaths using slope data when available. | C-71.0 | Contour plan reduces slope misalignment score. |
| R-82-008 | SHOULD | Optimization | Coverage planning SHOULD minimize point rows and short passes. | C-71.0 | Point-row penalty score meets target thresholds. |

---

## 82.6 Acceptance Criteria & Verification

- Path generation tests validate path types and boundary enforcement.
- Round-trip persistence tests confirm repeatability.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial Path Planning requirements. | Systems Engineering & Documentation Lead | |
