---
title: 41 - Message Bus
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
notes: Defines the Core event/command bus for deterministic domain exchanges.
---

# 41 - Message Bus
*(Status: Drafting - Core Contract Backbone)*

**Section ID:** 41  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2026-01-23  
**Related Sections:** 21 - System Decomposition & Boundaries; 22 - Process Model & Deployment; 31 - Domain Data Model  
**Upstream Dependencies:** 21 - System Decomposition; 22 - Process Model; 31 - Domain Data Model  
**Downstream Impacts:** 42 - UI Bridge; 43 - Plugin Bridge; 44 - Service APIs

---

## 41.1 Purpose & Scope

Define the Core message bus that carries domain events, telemetry, and commands across Core modules, plugins, and bridges while preserving determinism and contract validation.

---

## 41.2 Context

- The bus is the only transport inside Core for domain-to-domain interactions.
- All payloads are schema-governed per Section 31.
- Out of scope: external transports (see Section 44).

---

## 41.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference / Source |
|--------------|-----------------|------------------------|---------------------------|--------------------|
| Event routing | Ad-hoc callbacks and shared state. | Ordering and version drift. | Centralized bus with schema validation. | AgOpenGPS legacy loops |
| Commands | Direct method calls. | No audit trail. | Command envelopes with ack/failure paths. | UI bridge gaps |
| Backpressure | None. | UI or logging could block control loops. | Per-topic queues with throttling. | Field bug reports |

---

## 41.4 Definitions

| Term | Definition |
|------|-------------|
| Topic | A named channel that carries one contract type. |
| Envelope | A message wrapper with contract id, version, timestamp, and sequence. |
| Command | A request with explicit acknowledgement or rejection. |
| Snapshot | A full state dump emitted on demand or at intervals. |

---

## 41.5 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-41-000 | MUST | Capability | The bus MUST support pub/sub topics with schema validation on publish and subscribe. | C-31.5 | Invalid schemas are rejected in discovery tests. |
| R-41-001 | MUST | Reliability | The bus MUST preserve per-topic ordering using sequence numbers and SimClock timestamps. | C-22.2, C-31.5 | Reorder rate = 0 in replay verification. |
| R-41-002 | MUST | Safety | Command topics MUST require explicit ack/nack within a bounded timeout. | C-22.4 | p99 command ack <= 250 ms in lab tests. |
| R-41-003 | MUST | Performance | The bus MUST support backpressure to prevent UI/logging from blocking safety loops. | C-22.3 | Dropped or throttled non-critical topics under load. |
| R-41-004 | SHOULD | Extensibility | The bus SHOULD allow snapshot replay for late-joining consumers. | C-31.6 | Snapshot delivery verified in integration tests. |
| R-41-005 | MUST | Observability | The bus MUST expose metrics for publish rate, queue depth, drops, and latency. | C-22.4 | Metrics endpoint returns required fields. |

---

## 41.6 Acceptance Criteria & Verification

- Bus conformance tests validate schema checks, ordering, and command ack behavior.
- Load tests confirm backpressure protects safety-critical topics.
- Metrics are scraped and validated in CI.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial message bus requirements. | Systems Engineering & Documentation Lead | |
