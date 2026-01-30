---
title: 43 - Plugin Bridge
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
notes: Defines the Core to plugin interface for capabilities and lifecycle management.
---

# 43 - Plugin Bridge
*(Status: Drafting - Extensibility Contract)*

**Section ID:** 43  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2026-01-23  
**Related Sections:** 41 - Message Bus; 31 - Domain Data Model; 65 - Job Lifecycle  
**Upstream Dependencies:** 41 - Message Bus; 31 - Domain Data Model  
**Downstream Impacts:** 61 - Kinematics; 62 - Section Control; 63 - Rate Control; 74 - Mapping Plugins

---

## 43.1 Purpose & Scope

Define how plugins load, declare capabilities, exchange contracts, and remain compatible across releases without destabilizing Core.

---

## 43.2 Context

- Plugins are the main path for expansion beyond the baseline feature set.
- Every plugin must declare compatibility against contract versions.
- Out of scope: UI plugins (covered in Section 74/92).

---

## 43.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference / Source |
|--------------|-----------------|------------------------|---------------------------|--------------------|
| Plugin API | Ad-hoc DLLs. | Version drift and crashes. | Declarative manifests with validation. | Community forks |
| Capability discovery | Manual config. | Fragile setup. | Capability registry and auto-discovery. | Plugin wishlist |
| Isolation | None. | Single plugin crash downed UI. | Controlled lifecycle + gating. | Legacy issues |

---

## 43.4 Definitions

| Term | Definition |
|------|-------------|
| Plugin Manifest | Declarative file listing capabilities, contracts, and dependencies. |
| Capability | Named feature surface exposed to Core and UI (e.g., planter-monitor). |
| Compatibility Range | Allowed contract versions for a plugin to load. |

---

## 43.5 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-43-000 | MUST | Capability | Plugins MUST declare a manifest with capability ids, contract ids, and version ranges. | C-31.5 | Manifest validation passes at load. |
| R-43-001 | MUST | Safety | Core MUST refuse to load plugins with incompatible contract versions. | C-31.5 | Incompatible plugin load fails with reason code. |
| R-43-002 | MUST | Reliability | Plugin lifecycle MUST support load, start, stop, and unload without crashing Core. | C-22.4 | Lifecycle tests pass 100 times. |
| R-43-003 | SHOULD | Observability | Plugin health and metrics SHOULD be exposed to Monitoring. | C-64.0 | Health metrics appear in dashboard. |
| R-43-004 | MUST | Security | Plugins MUST run with declared permissions and cannot access Core internals outside the bridge. | C-22.4 | Permission tests enforce isolation rules. |

---

## 43.6 Acceptance Criteria & Verification

- Plugin compatibility tests enforce contract ranges.
- Lifecycle tests validate clean load/unload.
- Permission tests verify isolation.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial Plugin Bridge requirements. | Systems Engineering & Documentation Lead | |
