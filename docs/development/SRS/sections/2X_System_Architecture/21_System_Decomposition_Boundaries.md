---
title: 21 - Decomposition
version: 0.1.0
status: Draft
authors:
  - Jon Fortney
owner: Systems Engineering & Documentation Lead
reviewers:
  - Systems Engineering Team
approvers:
  - Project Coordinator
created: 2025-10-21
last_reviewed: 2025-11-09
review_cycle: Quarterly
notes: Architecture decomposition overview; metadata updated per governance policy.
---

# 21 - Decomposition
*(Status: Drafting)*

**Section ID:** 21 | **Version:** 0.1.0  
**Editors:** Jon Fortney  
**Last Updated:** 2025-11-12  
**Related Sections:** 11 - Operating System Support, 12 - Language & Runtime, 13 - UI Framework & UX  
**Upstream Dependencies:** 1X - Platform Foundations, 4X - Interprocess Communications  
**Downstream Impacts:** 3X - Data Storage, 5X - Hardware I/O Device Layer

## 21.1 Purpose & Scope

Capture the logical domains (kinematics, guidance, autosteer, mapping, UI bridge, AgIO, simulation) that AgOpenNext must deliver and describe how their responsibilities stay decoupled despite packaging variations.

## 21.2 Context

- Remote UI/headless operation must respect the charter’s headless strategy (see §7, Headless / Remote UI Strategy) so the UI bridge stays the only exposed consumer surface.

## 21.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Limitation | Opportunity | Reference |
|--------------|-----------------|------------|--------------|-----------|
| Modularization | Tight coupling between WinForms and Core. | Poor isolation. | Clearly separated domains and bridges. | Legacy architecture note |
| Interfaces | Shared memory + direct hardware access. | Hard to scale remote UIs. | Message bus and gRPC bridges. | Design memo |
| Extensions | Custom loops per feature. | Low reuse. | Domain catalog guiding plugins. | Domain catalog |

## 21.4 Definitions

| Term | Definition |
|------|-------------|
| Domain | Logical capability such as Guidance or Mapping. |
| Bridge | Versioned interface exposing domain data/commands. |
| Capability | Packaged service or plugin fulfilling a domain.

## 21.5 Requirements

| ID | Priority | Summary | Verification |
|----|----------|---------|--------------|
| R-DEC-001 | MUST | Publish the domain catalog with owners/responsibilities. | Domain entries referenced by workflows. |
| R-DEC-002 | SHOULD | Stabilize messaging contracts for each domain. | Contract tests exercise each interface with versioning checks. |
| R-DEC-003 | SHOULD | Version domain-level data contracts for traceability. | Registry records changes and affected consumers.

## 21.6 Acceptance & Verification

- Domain catalog entries list owners and responsibilities.
- Integration tests cover message contracts and detect version mismatches.
- Traceability tracker cites impacted components when contracts change.

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2025-11-12 | Template-form rewrite. | Jon Fortney |  |
| 0.1.0 | 2025-10-21 | Original decomposition catalog. | Jon Fortney |  |
