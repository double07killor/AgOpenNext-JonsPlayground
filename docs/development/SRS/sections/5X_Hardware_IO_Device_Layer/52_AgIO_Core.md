---
title: 52 - AgIO Core
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
notes: Defines hardware IO routing, discovery, and health handling.
---

# 52 - AgIO Core
*(Status: Drafting - Hardware IO Hub)*

**Section ID:** 52  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2026-01-23  
**Related Sections:** 51 - Hardware PGNs; 61 - Kinematics; 64 - Monitoring  
**Upstream Dependencies:** 51 - Hardware PGNs  
**Downstream Impacts:** 61 - Kinematics; 62 - Section Control; 63 - Rate Control

---

## 52.1 Purpose & Scope

Define how AgIO discovers hardware, routes PGNs, and normalizes device data for Core.

---

## 52.2 Context

- AgIO is the gateway between hardware and Core contracts.
- It must enforce PGN validation and device health checks.
- Out of scope: firmware updates (Section 54).

---

## 52.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference / Source |
|--------------|-----------------|------------------------|---------------------------|--------------------|
| Device discovery | Manual. | Slow setup. | Automatic discovery and registry. | AgIO legacy |
| Health checks | Minimal. | Silent failures. | Health contracts and watchdogs. | Field issues |

---

## 52.4 Definitions

| Term | Definition |
|------|-------------|
| Device Registry | List of connected devices and capabilities. |
| PGN Router | Component that validates and routes PGNs. |

---

## 52.5 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-52-000 | MUST | Capability | AgIO MUST discover devices and publish a device registry. | C-51.0 | Discovery tests pass. |
| R-52-001 | MUST | Safety | AgIO MUST validate incoming PGNs and reject invalid payloads. | C-51.0 | Validation tests pass. |
| R-52-002 | SHOULD | Reliability | Device health heartbeats SHOULD be published at >= 1 Hz. | C-64.0 | Heartbeat tests pass. |
| R-52-003 | MUST | Performance | AgIO routing latency MUST be <= 50 ms p95. | C-61.0 | Routing benchmarks pass. |
| R-52-004 | MUST | Capability | AgIO MUST support GNSS source aggregation with configurable priority and failover. | C-51.0 | Failover tests switch sources within 2 s. |
| R-52-005 | SHOULD | Reliability | AgIO SHOULD auto-scan serial devices for valid NMEA streams. | C-51.0 | Auto-scan tests detect GGA/RMC/VTG. |

---

## 52.6 Acceptance Criteria & Verification

- Device discovery tests validate registry contents.
- PGN validation tests confirm rejection of invalid messages.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial AgIO Core requirements. | Systems Engineering & Documentation Lead | |
