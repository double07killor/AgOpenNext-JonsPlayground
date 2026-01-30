---
title: 34 - Equipment Configurations
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
notes: Defines storage and validation for equipment configurations.
---

# 34 - Equipment Configurations
*(Status: Drafting - Equipment Models)*

**Section ID:** 34  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2026-01-23  
**Related Sections:** 32 - Persistence & Formats; 52 - AgIO Core  
**Upstream Dependencies:** 32 - Persistence & Formats  
**Downstream Impacts:** 61 - Kinematics; 62 - Section Control; 63 - Rate Control

---

## 34.1 Purpose & Scope

Define how tractor, implement, and sensor configurations are stored, validated, and versioned.

---

## 34.2 Context

- Equipment configs drive geometry, calibration, and IO mapping.
- Configs must be validated before use in Core.
- Out of scope: hardware firmware updates (Section 54).

---

## 34.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference / Source |
|--------------|-----------------|------------------------|---------------------------|--------------------|
| Config storage | Flat config files. | Limited validation. | Schema-validated manifests. | AOG configs |
| Versioning | None. | Hard to migrate. | Versioned equipment manifests. | Field issues |

---

## 34.4 Definitions

| Term | Definition |
|------|-------------|
| Equipment Manifest | Versioned description of tractor, implement, and sensors. |
| Calibration | Per-device offsets and scale factors. |

---

## 34.5 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-34-000 | MUST | Capability | Equipment configurations MUST be stored as schema-validated manifests. | C-32.0 | Manifest validation tests pass. |
| R-34-001 | MUST | Safety | Core MUST reject invalid or incomplete equipment configs. | C-32.0 | Validation rejects missing fields. |
| R-34-002 | SHOULD | Usability | Configs SHOULD support multiple profiles per machine. | C-65.0 | Profile switching tests pass. |
| R-34-003 | SHOULD | Reliability | Calibration values SHOULD be versioned and traceable to jobs. | C-65.0 | Calibration audit tests pass. |

---

## 34.6 Acceptance Criteria & Verification

- Manifest validation tests confirm schema compliance.
- Profile and calibration tests validate behavior.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial Equipment Configurations requirements. | Systems Engineering & Documentation Lead | |
