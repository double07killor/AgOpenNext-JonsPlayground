---
title: 74 - Mapping Plugins
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
notes: Defines plugin interfaces for map sources and tools.
---

# 74 - Mapping Plugins
*(Status: Drafting - Map Extension Interfaces)*

**Section ID:** 74  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2026-01-23  
**Related Sections:** 43 - Plugin Bridge; 72 - Layer Management  
**Upstream Dependencies:** 43 - Plugin Bridge; 72 - Layer Management  
**Downstream Impacts:** 91 - UI Shell

---

## 74.1 Purpose & Scope

Define how mapping plugins add new map sources, analysis layers, and tooling without modifying Core.

---

## 74.2 Context

- Map plugins must use the same CRS and unit conventions.
- Plugins can add layers but cannot bypass Core validation.
- Out of scope: core coverage logic (Section 71).

---

## 74.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference / Source |
|--------------|-----------------|------------------------|---------------------------|--------------------|
| Map plugins | Limited, custom. | Hard to extend. | Plugin registry with capability cards. | Community requests |

---

## 74.4 Definitions

| Term | Definition |
|------|-------------|
| Map Source | External or local provider of map tiles or features. |
| Tool Plugin | UI tool that operates on map layers. |

---

## 74.5 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-74-000 | MUST | Capability | Mapping plugins MUST declare layer types and required contracts in the manifest. | C-43.0 | Manifest validation passes. |
| R-74-001 | MUST | Compatibility | Mapping plugins MUST adhere to CRS and unit metadata rules. | C-23.1 | Metadata validation tests pass. |
| R-74-002 | SHOULD | Extensibility | Plugins SHOULD support dynamic enable/disable without restart. | C-43.2 | Hot reload tests pass. |

---

## 74.6 Acceptance Criteria & Verification

- Plugin layer validation tests pass.
- CRS compliance tests pass.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial Mapping Plugins requirements. | Systems Engineering & Documentation Lead | |
