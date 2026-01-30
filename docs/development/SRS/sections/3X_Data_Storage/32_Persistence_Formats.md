---
title: 32 - Persistence & Formats
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
notes: Defines persistence formats, versioning, and migrations.
---

# 32 - Persistence & Formats
*(Status: Drafting - Storage Foundation)*

**Section ID:** 32  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2026-01-23  
**Related Sections:** 31 - Domain Data Model; 35 - Backup & Retention  
**Upstream Dependencies:** 31 - Domain Data Model  
**Downstream Impacts:** 33 - Mapping Storage; 34 - Equipment Configurations; 35 - Backup & Retention

---

## 32.1 Purpose & Scope

Define how operational data is stored, versioned, and migrated across releases while preserving replay and auditability.

---

## 32.2 Context

- Storage must support field data, equipment configs, and replay journals.
- Data formats must be versioned and validated.
- Out of scope: cloud sync (future section).

---

## 32.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference / Source |
|--------------|-----------------|------------------------|---------------------------|--------------------|
| Formats | Mixed CSV and binary. | Hard to migrate. | Versioned schemas with migrations. | AOG data |
| Replay | Loose files. | Incomplete metadata. | Contract-aligned journals. | QA gaps |

---

## 32.4 Definitions

| Term | Definition |
|------|-------------|
| Persistence Store | Local storage for configs and journals. |
| Migration | Versioned upgrade path for stored data. |
| Journal | Append-only log of contract messages. |

---

## 32.5 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-32-000 | MUST | Capability | The system MUST store domain data and journals using versioned schemas. | C-31.5 | Schema validation tests pass. |
| R-32-001 | MUST | Reliability | Migrations MUST be reversible or provide backup before upgrade. | C-35.0 | Migration tests verify rollback. |
| R-32-002 | SHOULD | Performance | Journal writes SHOULD sustain >= 10k messages/sec on reference hardware. | C-31.5 | Write throughput benchmark passes. |
| R-32-003 | SHOULD | Compatibility | Export formats SHOULD include ISOXML, SHP, and CSV where applicable. | C-65.0 | Export tests validate outputs. |
| R-32-004 | MUST | Safety | Data corruption MUST be detected via checksums or hashes. | C-31.5 | Corruption tests detect errors. |

---

## 32.6 Acceptance Criteria & Verification

- Schema and migration tests validate persistence integrity.
- Export tests validate format compliance.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial Persistence & Formats requirements. | Systems Engineering & Documentation Lead | |
