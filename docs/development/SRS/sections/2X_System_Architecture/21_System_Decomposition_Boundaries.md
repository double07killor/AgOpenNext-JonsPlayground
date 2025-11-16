---
title: 21 - System Decomposition & Boundaries
version: 0.3.0
status: Draft
authors:
  - Jon Fortney
owner: Systems Engineering & Documentation Lead
reviewers:
  - Systems Engineering Team
approvers:
  - Project Coordinator
created: 2025-10-21
last_reviewed: 2025-11-17
review_cycle: Quarterly
notes: Catalog of Core domains expressed as requirements so the runtime/process mapping in Section 22 has clear inputs.
---

# 21 - System Decomposition & Boundaries
*(Status: Drafting – Owned Capability Catalog)*

**Section ID:** 21 | **Version:** 0.2.0  
**Editors:** Jon Fortney  
**Last Updated:** 2025-11-17  
**Related Sections:** 11 - Operating System Support, 12 - Language & Runtime, 13 - UI Framework & UX, 22 - Process Model & Deployment  
**Upstream Dependencies:** 1X - Platform Foundations, 4X - Interprocess Communications  
**Downstream Impacts:** 3X - Data Storage, 5X - Hardware I/O Device Layer, 41 - Message Bus, 52 - AgIO Core

## 21.1 Purpose & Scope

This section defines the capabilities that must exist inside Core as requirements. Each requirement enumerates the responsibilities, inputs, and outputs for a domain so that Section 22 can reason about where each capability executes. Domains can be implemented as mandatory plugins or optional extensions, but they must appear in this catalog before any deployment documentation references them.

## 21.2 Domain Requirements

| Req ID | Domain | Requirement Statement | Verification |
|--------|--------|-----------------------|--------------|
| R-21-001 | **Kinematics** | The system SHALL fuse GNSS, IMU, wheel angle/speed, and slip measurements to produce and publish the authoritative pose, velocity, and timebase every SimClock tick. | Contract tests replay pose identical to run-time logs; Pose/SimClock pair is consumed by all downstream tests. |
| R-21-002 | **Guidance** | The system SHALL generate steering intent from AB lines, headlands, and boundary-aware paths while honoring look-ahead, convergence, and prior coverage metadata published by Field Management. | Guidance regression suite validates headland-aware paths and signals precedence over stale intent. |
| R-21-003 | **Autosteer** | The system SHALL translate guidance targets into actuator commands, enforce engagement/state logic, arbitrate multiple control sources, and emit child health data on the safety-critical loop. | Autosteer latency tests keep loop below 10 ms median and watchdog receives heartbeat every cycle. |
| R-21-004 | **Field Management** | The system SHALL store field, boundary, headland, and guidance line configurations, compute derived polygons (e.g., headland geometry), and publish them for Guidance, Section Control, and Mapping before the next command cycle. | Field data ingestion suite covers imported fields/headlands and verifies consumers ingest updates within one tick. |
| R-21-005 | **Mapping** | The system SHALL maintain live coverage maps, application overlays, and pass geometry that are coherent, thread-safe, and snapshotable for replay or visualization.| Mapping contract tests ensure commanded coverage tracks Section Control events and snapshots match replay data. |
| R-21-006 | **Section Control** | The system SHALL operate boom sections and row units using coverage-based shutoff, automatic re-enable logic, and boundary/safety zone enforcement after consulting guidance intent and field data. | Section Control integration tests confirm coverage gaps trigger shutoff/re-enable without violating boundaries. |
| R-21-007 | **Monitoring** | The system SHALL aggregate command/work state, yield, planter, and flow/blockage sensors from other domains and publish alarm/diagnostic payloads. | Monitoring dashboards reproduce combined state; alarm tests trigger for fault scenarios. |
| R-21-008 | **System Health & Telemetry** | The system SHALL track device status, heartbeats, watchdogs, and faults, exposing human/machine-readable telemetry for diagnostics and recovery. | Heartbeat/health tests detect missing heartbeats and trigger documented recovery. |
| R-21-009 | **Data Logging / Replay** | The system SHALL record deterministic timelines (pose, commands, coverage, operator actions) for replay, QA, and compliance with the exact contract streams emitted during live runs. | Replay compares logged timeline to live run and verifies deterministic sequence. |
| R-21-010 | **Simulation** | The system SHALL provide contract-aligned sensor and actuator substitutes driven by a deterministic SimClock for offline testing and troubleshooting. | Simulation traces align with Core timelines when supplied the same input scenario. |
| R-21-011 | **AgIO / Hardware I/O** | The system SHALL bridge serial, CAN, UDP, and vendor-specific buses into normalized internal messages while keeping parity between legacy and next-gen link layers. | Hardware-in-the-loop tests confirm normalized messages match expected schema for downstream consumers. |
| R-21-012 | **UI Bridge** | The system SHALL surface Core state and accept ingress commands through versioned APIs so UI surfaces consume the same data without duplicating safety-critical logic. | UI contract tests verify state snapshots and command round-trips without modifying internal state directly. |

## 21.3 Domain Interaction Requirements

- **Headland Geometry:** R-21-013 – The system SHALL have Field Management compute headland geometry and publish it before Guidance or Section Control consume it so boundary-aware steering respects updated polygons within one SimClock tick.
- **Coverage Synchronization:** R-21-014 – The system SHALL route commanded coverage events (and other work-state inputs) from Section Control, and any additional monitoring sensors, through Monitoring before Mapping consumes them so Monitoring can assign thresholds, normalize signals, and produce layered data for Mapping without requiring GPS/coverage hardware on the same host.
- **Coverage Planning Ownership:** R-21-015 – Field Management SHALL be responsible for computing coverage plans/headland polygons (for now) and publishing them to Guidance/Section Control before each command cycle, with the option to extract this capability into a dedicated coverage/path-planning domain later if requirements demand further separation of responsibilities.
- **Health Aggregation:** R-21-016 – The system SHALL have Monitoring and System Health & Telemetry aggregate domain telemetry and republish health/diagnostic contracts to keep observability centralized.
- **Contract Registry:** R-21-017 – The system SHALL require every domain to publish contract metadata (version, inputs, outputs) so Section 22 can bind it to a runtime host without ambiguity.

## 21.4 Acceptance & Verification

- Domain requirements are traceable to deployment docs in Section 22 and the ADR that defines the canonical domain set.
- Integration tests exercise each requirement (pose fidelity, guidance/headland, autosteer timing, field data visibility, monitoring alarms) and fail if contracts break.
- Traceability matrix references each requirement ID so changes to domain responsibilities trigger architectural reviews before release.

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.2.0 | 2025-11-16 | Complete rewrite to properly split section group 2X | Jon Fortney |  |
