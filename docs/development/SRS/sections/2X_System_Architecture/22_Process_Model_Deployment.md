---
title: 22 - Process Model & Deployment
version: 0.3.0
status: Draft
authors:
  - Jon Fortney
owner: Systems Engineering & Documentation Lead
reviewers:
  - Systems Engineering Team
approvers:
  - Project Coordinator
created: 2025-11-12
last_reviewed: 2025-11-17
review_cycle: Quarterly
notes: Defines the runtime/process mapping requirements that host Section 21 domains while maintaining determinism and contract fidelity.
---

# 22 - Process Model & Deployment
*(Status: Drafting – Runtime Host Mapping)*

**Section ID:** 22  
**Version:** 0.3.0  
**Editors:** Jon Fortney  
**Last Updated:** 2025-11-17  
**Related Sections:** 21 - System Decomposition & Boundaries, 41 - Message Bus, 52 - AgIO Core, 61 - Kinematics, 71 - Mapping Core  
**Upstream Dependencies:** 11 - Operating System Support, 12 - Language & Runtime, 13 - UI Framework & UX  
**Downstream Impacts:** 41 - Message Bus, 42 - UI Bridge, 52 - AgIO Core, 61 - Kinematics, 71 - Mapping Core, 80 - Deployment & Packaging

## 22.1 Purpose & Scope

This section states the requirements for assigning Section 21 domain responsibilities to runtime hosts (processes, threads, schedulers) and for enforcing deterministic execution, health monitoring, and bridge contracts. Every new host mapping must cite one of these requirements and demonstrate compliance before implementation.

## 22.2 Process Model Requirements

| Req ID | Focus | Requirement Statement | Verification |
|--------|-------|-----------------------|--------------|
| R-22-001 | Core Runtime Host | The system SHALL execute all safety-critical domains (kinematics, guidance, autosteer, mapping, section control, AgIO, monitoring, system health, data logging/replay) within a single Core runtime process governed by the deterministic scheduler referenced in R-22-002. | Scheduler logs and replay traces demonstrate fixed ordering and consistent SimClock ticks across deployments. |
| R-22-002 | SimClock Alignment | The system SHALL share a single authoritative SimClock across Core, Simulation, Data Logging, and any determinism-critical bridges so that all traced events reference the same timebase. | SimClock tick comparisons between hosts remain within configured tolerance; replay matches live timelines. |
| R-22-003 | Non-critical Domains | The system SHALL schedule non-critical domains (UI Bridge, optional extensions, monitoring/logging workers) on bounded threads or separate host processes that subscribe to contract-aligned snapshots without interfering with Core’s safety loops. | Fault injection tests pause non-critical hosts without breaking Core loops; contract snapshots remain intact. |
| R-22-004 | Watchdog & Health | The process model SHALL define watchdogs, heartbeats, and recovery policies per host so missing signals trigger documented recovery sequences rather than silent failures. | Watchdog failure tests result in policy-defined responses (restart, notification) within acceptance windows. |
| R-22-005 | Bridge Contracts | The system SHALL expose versioned bridge contracts for UI, simulation, and extension hosts so that each host knows exactly which domains it consumes or controls before establishing IPC. | Contract registry lists versions per host; handshake tests enforce version agreement. |
| R-22-006 | Deployment Profiles | The system SHALL document how desktop, embedded, simulation, and remote-service deployments reuse the same host mappings or contract bridges to ensure parity in behavior. | Deployment runbooks enumerate host mappings and show tests validating contract parity. |

## 22.3 Deployment Profile Requirements

- R-22-007 – Desktop/Cab Deployment SHALL co-locate Core runtime, UI bridge, while throttling optional logging/monitoring threads so debugging remains straightforward without violating Core timing budgets.
- R-22-009 – Simulation / Replay Deployment SHALL reuse Simulation host contracts from R-22-005 and share SimClock output with Core so offline testing mirrors live behavior.
- R-22-010 – Hybrid Deployments SHALL describe which hosts (Core, Simulation, UI bridge) run remotely vs. on-vehicle while preserving heartbeat/contract visibility defined in R-22-004 and R-22-005.

## 22.4 Acceptance & Verification

- The Process Model acceptance tests demonstrate that Core scheduling (R-22-001) stays deterministic under load and that SimClock alignment (R-22-002) reproduces event order across replay runs.
- Health and watchdog tests validate that each host responds to missing heartbeats according to R-22-004, and contract snapshot tests prove R-22-005 compliance.
- Deployment profiles are updated with host mapping matrices referencing this section; change control verifies new hosts cite at least one of these requirements before release.

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.3.0 | 2025-11-17 | Restated Section 22 as requirements with explicit host and deployment expectations. | Jon Fortney |  |
| 0.2.0 | 2025-11-16 | Runtime host mapping with descriptive tables/informative profiles. | Jon Fortney |  |
