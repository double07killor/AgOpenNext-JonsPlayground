---
title: 91 - UI Shell
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
notes: Defines the primary UI shell for desktop and tablet deployments.
---

# 91 - UI Shell
*(Status: Drafting - Modular UI Shell)*

**Section ID:** 91  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2026-01-23  
**Related Sections:** 42 - UI Bridge; 73 - Rendering; 92 - Gauges & Panels  
**Upstream Dependencies:** 42 - UI Bridge; 73 - Rendering  
**Downstream Impacts:** 92 - Gauges & Panels; 94 - Remote Clients

---

## 91.1 Purpose & Scope

Define the modular UI shell that hosts panels, dashboards, and workflows while remaining fully driven by Core contracts.

---

## 91.2 Context

- UI shell must not contain safety-critical logic.
- UI shell must support plugin-driven panels.
- Out of scope: core control logic.

---

## 91.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference / Source |
|--------------|-----------------|------------------------|---------------------------|--------------------|
| UI layout | Fixed layouts. | Low modularity. | Dockable and composable panels. | AOG UI |
| Plugins | Limited. | Hard to extend. | Panel plugins via capability metadata. | Community requests |

---

## 91.4 Definitions

| Term | Definition |
|------|-------------|
| Panel | A UI component presenting data or controls for a capability. |
| Workspace | A saved arrangement of panels and layouts. |

---

## 91.5 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-91-000 | MUST | Capability | The UI shell MUST render panels based on capability metadata and contracts. | C-42.0 | Panel discovery tests pass. |
| R-91-001 | MUST | Usability | The UI shell MUST support dockable and resizable layouts with saved workspaces. | C-13.5 | Workspace save/load tests pass. |
| R-91-002 | SHOULD | Performance | UI shell SHOULD maintain >= 30 FPS under standard workloads. | C-73.0 | Performance tests pass. |
| R-91-003 | SHOULD | Extensibility | UI shell SHOULD allow plugin panels without a full app rebuild. | C-43.0 | Plugin panel load tests pass. |

---

## 91.6 Acceptance Criteria & Verification

- Panel discovery tests validate capability-driven UI.
- Workspace tests validate layout save/restore.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial UI Shell requirements. | Systems Engineering & Documentation Lead | |
