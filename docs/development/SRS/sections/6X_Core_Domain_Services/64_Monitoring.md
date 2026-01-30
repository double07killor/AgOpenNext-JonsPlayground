---
title: 64 - Monitoring
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
notes: Defines health, alarms, and diagnostics aggregation.
---

# 64 - Monitoring
*(Status: Drafting - Health and Alarms)*

**Section ID:** 64  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2026-01-23  
**Related Sections:** 21 - System Decomposition & Boundaries; 42 - UI Bridge; 65 - Job Lifecycle  
**Upstream Dependencies:** 41 - Message Bus; 31 - Domain Data Model  
**Downstream Impacts:** 91 - UI Shell; 92 - Gauges & Panels

---

## 64.1 Purpose & Scope

Define how system health, alarms, and diagnostics are aggregated and delivered to operators and logs.

---

## 64.2 Context

- Monitoring consumes health telemetry from all domains and plugins.
- Alarm signals must be visible in UI and logged for replay.
- Out of scope: hardware sensor wiring (see Section 52).

---

## 64.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference / Source |
|--------------|-----------------|------------------------|---------------------------|--------------------|
| Alarm handling | Mixed UI alerts. | No central severity model. | Unified severity and ack model. | Field feedback |
| Health data | Sparse. | No heartbeat tracking. | Standard heartbeat contracts. | Legacy logs |

---

## 64.4 Definitions

| Term | Definition |
|------|-------------|
| Alarm | A condition requiring operator attention. |
| Health Contract | Schema describing subsystem status and heartbeat. |
| Acknowledgement | Operator confirmation of an alarm. |

---

## 64.5 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-64-000 | MUST | Capability | Monitoring MUST aggregate health contracts from all Core domains and plugins. | C-21.16 | Health list matches registered domains. |
| R-64-001 | MUST | Safety | Alarms MUST be classified by severity with explicit operator actions. | C-21.8 | Alarm catalog covers all safety states. |
| R-64-002 | MUST | Reliability | Heartbeat loss MUST trigger alarms within 1 s. | C-22.4 | Fault injection meets timing. |
| R-64-003 | SHOULD | Auditability | Alarm acknowledgements SHOULD be logged to job history. | C-65.0 | Acks appear in logs. |
| R-64-004 | SHOULD | Usability | Monitoring SHOULD provide filterable alarms by domain and severity. | C-91.0 | UI tests validate filters. |

---

## 64.6 Acceptance Criteria & Verification

- Heartbeat loss tests trigger alarms within target.
- Alarm catalog reviews confirm coverage for safety conditions.
- UI alarm filter tests pass.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial Monitoring requirements. | Systems Engineering & Documentation Lead | |
