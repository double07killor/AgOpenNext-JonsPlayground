---
title: 53 - CM5 Controller
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
notes: Defines the CM5 controller integration and watchdog behavior.
---

# 53 - CM5 Controller
*(Status: Drafting - Embedded Controller)*

**Section ID:** 53  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2026-01-23  
**Related Sections:** 52 - AgIO Core; 24 - Timing  
**Upstream Dependencies:** 52 - AgIO Core  
**Downstream Impacts:** 54 - Firmware Updates; 55 - Direct Device Passthrough

---

## 53.1 Purpose & Scope

Define CM5 controller responsibilities, pin mapping, watchdogs, and co-location policies for Core and AgIO.

---

## 53.2 Context

- CM5 hosts critical IO and optionally co-hosts Core.
- Watchdog behavior must be deterministic and logged.
- Out of scope: full hardware schematics.

---

## 53.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference / Source |
|--------------|-----------------|------------------------|---------------------------|--------------------|
| Controller | CM4/legacy boards. | Mixed pin maps. | Standard CM5 pin map registry. | Hardware notes |

---

## 53.4 Definitions

| Term | Definition |
|------|-------------|
| Watchdog | Hardware or software safety timer. |
| Pin Map | Mapping between IO pins and signals. |

---

## 53.5 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-53-000 | MUST | Capability | CM5 controller MUST publish a pin map registry and version. | C-52.0 | Pin map validation tests pass. |
| R-53-001 | MUST | Safety | Watchdog timers MUST trigger safe shutdown within 250 ms of missed heartbeat. | C-24.0 | Watchdog tests meet timing. |
| R-53-002 | SHOULD | Reliability | CM5 SHOULD support redundant power monitoring inputs. | C-52.0 | Power monitoring tests pass. |

---

## 53.6 Acceptance Criteria & Verification

- Watchdog timing tests validate shutdown behavior.
- Pin map registry tests validate version and mapping.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial CM5 Controller requirements. | Systems Engineering & Documentation Lead | |
