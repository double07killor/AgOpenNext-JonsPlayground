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

Define the presentation stack and operator experience, balancing the legacy WinForms UI with Avalonia modernization and remote/companion frontends.

## 13.2 Context

- The charter mandates ≥30 FPS on reference hardware and complete WinForms parity before retiring it (see §3, G3).  

## 13.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Limitation | Opportunity | Reference |
|--------------|-----------------|------------|--------------|-----------|
| UI Stack | WinForms desktop only. | Windows-only. | Avalonia cross-platform shell. | UI modernization plan |
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
| R-UI-001 | MUST | Keep WinForms shipping until Avalonia covers production features. | WinForms regression suite passes on Windows. |
| R-UI-002 | SHOULD | Deliver Avalonia 12 shell that renders the same view models on Windows and Linux. | Avalonia parity checklist passes. |
| R-UI-003 | SHOULD | Implement metadata-driven dashboards for new layers. | Metadata widget demos connect to live data. |
| R-UI-004 | SHOULD | Enable remote frontends via the UI Bridge transport. | Remote smoke tests over gRPC/WebSocket succeed. |
| R-UI-005 | SHOULD | Document accessibility/multi-monitor behavior. | Accessibility checklist referenced by training.

## 13.6 Acceptance & Verification

- Avalonia parity checklist covers run modes and dashboards.
- Remote frontends demonstrate gRPC/WebSocket connectivity.
- Accessibility and layout documentation stay current.

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2025-11-12 | Template-compliant rewrite. | AgOpenNext Team (Codex) |  |
| 0.1.0 | 2025-10-20 | Original UI modernization doc. | AgOpenNext Team (Codex) |  |
