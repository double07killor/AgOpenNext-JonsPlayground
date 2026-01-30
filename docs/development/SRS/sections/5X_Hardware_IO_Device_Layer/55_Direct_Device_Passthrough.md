---
title: 55 - Direct Device Passthrough
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
notes: Defines direct device passthrough interfaces for sensors.
---

# 55 - Direct Device Passthrough
*(Status: Drafting - Direct Links)*

**Section ID:** 55  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2026-01-23  
**Related Sections:** 52 - AgIO Core; 61 - Kinematics  
**Upstream Dependencies:** 52 - AgIO Core  
**Downstream Impacts:** 61 - Kinematics; 63 - Rate Control

---

## 55.1 Purpose & Scope

Define when sensors or devices may connect directly to Core, bypassing AgIO, and how those connections are validated.

---

## 55.2 Context

- Direct passthrough is used for high-rate sensors such as GNSS/IMU.
- Direct links must still conform to contract validation.
- Out of scope: vendor-specific device SDKs.

---

## 55.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference / Source |
|--------------|-----------------|------------------------|---------------------------|--------------------|
| Direct IO | Ad-hoc. | Non-deterministic. | Contract-based direct links. | AOG IO |

---

## 55.4 Definitions

| Term | Definition |
|------|-------------|
| Passthrough | Direct sensor link to Core without AgIO translation. |
| Validation Gate | Contract validation for direct links. |

---

## 55.5 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-55-000 | MUST | Safety | Direct device links MUST use the same contract validation as AgIO. | C-31.5 | Validation tests pass. |
| R-55-001 | SHOULD | Performance | Direct links SHOULD be used only when required for latency <= 20 ms. | C-24.0 | Latency tests validate usage. |
| R-55-002 | MUST | Observability | Direct links MUST expose health status and drop counts. | C-64.0 | Monitoring tests pass. |
| R-55-003 | SHOULD | Capability | Direct links SHOULD support multi-sensor lever arm metadata for fusion. | C-34.0 | Lever arm metadata tests pass. |

---

## 55.6 Acceptance Criteria & Verification

- Validation tests verify contract adherence.
- Monitoring tests validate health metrics.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial Direct Device Passthrough requirements. | Systems Engineering & Documentation Lead | |
