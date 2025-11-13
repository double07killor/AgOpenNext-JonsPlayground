---
title: 13 - UI Framework & UX
version: 0.1.0
status: Draft
authors:
  - AgOpenNext Team (Codex)
owner: Systems Engineering & Documentation Lead
reviewers:
  - Systems Engineering Team
approvers:
  - Project Coordinator
created: 2025-10-20
last_reviewed: 2025-11-09
review_cycle: Quarterly
notes: UI/UX modernization goals; governance metadata applied.
---

# 13 - UI Framework & UX
*(Status: Drafting)*

**Section ID:** 13 | **Version:** 0.1.0  
**Editors:** AgOpenNext Team (Codex)  
**Last Updated:** 2025-11-12  
**Related Sections:** 11 - Operating System Support, 12 - Language & Runtime, 9X - Frontends & Ops  
**Upstream Dependencies:** 1X - Platform Foundations, 4X - Interprocess Communications  
**Downstream Impacts:** 9X - Frontends & Ops, Training & UX Guidelines

## 13.1 Purpose & Scope

Define the presentation stack and operator experience that delivers a modern, cross-platform desktop UI plus companion/remote workflows for operators and QA teams.

## 13.2 Context

- The charter mandates ≥30 FPS on reference hardware and a full-featured modernization that keeps parity with critical workflows (see §3, G3).  

## 13.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Limitation | Opportunity | Reference |
|--------------|-----------------|------------|--------------|-----------|
| UI Stack | WinForms desktop only. | Windows-only. | Modern cross-platform shell with metadata-driven controls. | UI modernization plan |
| Dashboards | Hard-coded panels. | Slow to adapt. | Metadata widgets. | Dashboard backlog |
| Remote Clients | Ad-hoc transports. | Fragmented UX. | UI Bridge + remote run modes. | gRPC spec |

## 13.4 Definitions

| Term | Definition |
|------|-------------|
| Run Mode | Operating mode such as LocalInProc or CompanionRemote. |
| Metadata Widget | Dashboard component derived from schema. |
| UI Bridge | Interface exposing Core data/commands to frontends.

## 13.5 Requirements

| ID | Priority | Summary | Verification |
|----|----------|---------|--------------|
| R-UI-001 | MUST | Ship a modern desktop UI that meets the charter’s performance target (≥30 FPS on reference hardware) and supports core operator workflows. | Performance tests on reference hardware and critical feature checklist pass. |
| R-UI-002 | SHOULD | Surface metadata-driven dashboards and widgets so new layers appear without code changes. | Metadata widget demos connect to live data flows. |
| R-UI-003 | MUST | Support remote/companion frontends via the UI Bridge transport so headless cores remain operable. | Remote smoke tests over gRPC/WebSocket succeed. |
| R-UI-004 | SHOULD | Address accessibility, multi-monitor, and touch ergonomics within the UI guidelines. | Accessibility/layout checklist referenced by training materials. |

## 13.6 Acceptance & Verification

- Desktop/performance benchmarks demonstrate ≥30 FPS and coverage of critical operator workflows.
- Remote frontends validate connectivity via the UI Bridge transport.
- Accessibility and layout documentation stay current.

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2025-11-12 | Template-compliant rewrite. | AgOpenNext Team (Codex) |  |
| 0.1.0 | 2025-10-20 | Original UI modernization doc. | AgOpenNext Team (Codex) |  |
