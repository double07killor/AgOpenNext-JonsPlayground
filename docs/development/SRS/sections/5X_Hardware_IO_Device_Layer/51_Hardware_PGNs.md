---
title: 51 - Hardware PGNs
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
notes: Defines on-wire messages and hardware payload contracts.
---

# 51 - Hardware PGNs
*(Status: Drafting - On-Wire Contracts)*

**Section ID:** 51  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2026-01-23  
**Related Sections:** 52 - AgIO Core; 31 - Domain Data Model  
**Upstream Dependencies:** 31 - Domain Data Model  
**Downstream Impacts:** 52 - AgIO Core; 53 - CM5 Controller

---

## 51.1 Purpose & Scope

Define the on-wire message set between devices, sensors, and controllers, including IDs, payloads, and expected rates.

---

## 51.2 Context

- PGNs define communication between AgIO and hardware devices.
- Payloads must map to schema contracts in Core.
- Out of scope: physical connectors.

---

## 51.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference / Source |
|--------------|-----------------|------------------------|---------------------------|--------------------|
| PGN catalog | Mixed definitions. | Drift across devices. | Centralized PGN registry. | AgIO baseline |

---

## 51.4 Definitions

| Term | Definition |
|------|-------------|
| PGN | Parameter Group Number defining a message type. |
| Payload | Structured data carried by a PGN. |

---

## 51.5 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-51-000 | MUST | Capability | The PGN catalog MUST define id, payload schema, and expected rate for each message. | C-31.5 | Catalog validation tests pass. |
| R-51-001 | MUST | Reliability | PGNs MUST include version metadata to prevent drift. | C-31.5 | Version checks pass. |
| R-51-002 | SHOULD | Performance | Critical PGNs SHOULD be transmitted at >= 10 Hz. | C-61.0 | Rate tests pass. |
| R-51-003 | MUST | Safety | Invalid PGNs MUST be rejected or flagged by AgIO. | C-52.0 | Invalid PGN tests pass. |
| R-51-004 | MUST | Compatibility | The catalog MUST include NMEA and proprietary sentences used for GNSS and IMU telemetry. | C-31.5 | Coverage tests validate message list. |

---

## 51.6 Acceptance Criteria & Verification

- PGN catalog tests validate payload schemas and rates.
- Invalid PGN tests validate rejection behavior.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial Hardware PGN requirements. | Systems Engineering & Documentation Lead | |
