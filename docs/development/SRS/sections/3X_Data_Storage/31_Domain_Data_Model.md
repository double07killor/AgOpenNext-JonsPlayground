---
title: Domain Data Model
version: 0.1.0
status: Draft
authors:
  - Jon Fortney
owner: Systems Engineering & Documentation Lead
reviewers:
  - Systems Engineering Team
approvers:
  - Project Coordinator
created: 2025-11-13
last_reviewed: 2025-11-13
review_cycle: Quarterly
notes: Defines the data contracts, logging, and persistence expectations for AgOpenNext domain state.
---

# 31 - Domain Data Model
*(Status: Drafting - Schema Discipline)*

**Section ID:** 31  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2025-11-13  
**Related Sections:** 21 - System Decomposition & Boundaries; 32 - Persistence & Formats; 33 - Mapping Storage  
**Upstream Dependencies:** 21 - System Decomposition; 22 - Process Model; 24 - Timing  
**Downstream Impacts:** 32 - Persistence & Formats; 33 - Mapping Storage; 61 - Kinematics; 71 - Mapping Core

---

## 31.1 Purpose & Scope

Sets the authoritative schema for the AgOpenNext runtime-pose, temporal state, coverage layers, guidance intents, health, and command data-so every domain can exchange typed, versioned, deterministic messages regardless of deployment or host environment.  
This section also defines the expectations for logging/replay streams and simulation substitution so recorded data reproduces live behavior.

---

## 31.2 Context

- Relies on the contract catalog produced by §21.4 to keep exchanges schema-bound, versioned, and transport-neutral.  
- Interacts with Mapping (coverage layers), Guidance (targets), AgIO (hardware telemetry), and the UI bridge through defined data surfaces.  
- Out of scope: physical storage formats (handled in §32) and UI rendering assumptions (§71-§73).

---

## 31.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference / Source |
|--------------|----------------|------------------------|---------------------------|--------------------|
| Data Contracts | Legacy AgOpenNext relied on informal shared memory structures and undocumented message loops. | No guarantee of determinism or forward compatibility; fields could be mutated by any component. | Introduce struct-based contracts, version identifiers, and discovery tests. | §21.4 Data & Interface Contracts |
| Logging & Replay | Replay relied on custom stubs and informal logging; simulation drift was common. | Recorded runs often diverged from live behavior and lacked schema checks. | Normalize logging into contract sequences and deterministically ordered streams tied to the SimClock. | §21.9 Simulation & Replay Requirements |
| Simulation | Hardware substitution used ad-hoc providers with bespoke interfaces. | Simulation diverged from production contracts, making regression validation unreliable. | Map simulated hardware to the same contract surfaces as live data for parity. | §21.3 & §21.9 |

> **Informative:** Legacy behavior overview only; modern requirements enforce schema discipline.

---

## 31.4 Definitions

| Term | Definition |
|------|-------------|
| **Contract** | A typed, versioned message schema describing the payload and cadence shared between domains (pose, spatial layer, guidance intent, health, etc.). |
| **Discovery Tests** | Automated verification that each module publishes the expected contract set and gracefully ignores unknown versions. |
| **Deterministic Journal** | Ordered log of domain messages, commands, and hardware telemetry that can be re-played verbatim. |
| **Simulation Provider** | A pluggable component that feeds simulated telemetry or consumes commands while adhering to the same contracts as real hardware. |
| **Schema Evolution** | The controlled process for extending a contract (additive fields, version bumps) without breaking older modules. |

---

## 31.5 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-31-000 | MUST | Capability | AgOpenNext MUST expose a catalog of versioned contracts for pose, spatial layers, guidance intents, setpoints, health, and diagnostics that every domain can discover and validate. | C-21.4, C-21.7 | Discovery tests enumerate contracts at startup and fail fast on schema mismatches. |
| R-31-001 | MUST | Performance | Logged and replayed sequences MUST preserve ordering, timestamps, and discrete command state so deterministic replay matches live runs within tolerated error margins. | C-21.9, C-21.11 | Replay verification matches pose within 1 cm and coverage within 1% as defined in §21.9. |
| R-31-002 | SHOULD | Extensibility | Contracts SHOULD allow optional fields so extensions can add metadata (e.g., telemetry tags) without breaking older consumers. | C-21.4.6 Compatibility & Validation | Schema evolution rules enforce additive changes only; unknown fields are ignored at load. |
| R-31-003 | MUST | Safety | Instrumentation data (health, watchdog status) MUST be recorded and propagated alongside control contracts to support diagnostics and restart handling. | C-21.3.8, C-21.12 Verification Objectives | Logging pipeline records health updates with each loop, and alerting monitors pipeline integrity. |

### 31.5.1 Requirement Sources & Rationale

| Req ID | Source (issue/discussion/standard) | Rationale (one line) |
|-------|-------------------------------------|----------------------|
| R-31-000 | §21.4 Contract Principles | Contracts are the only portable means to exchange state safely. |
| R-31-001 | §21.9 Simulation & Replay | Deterministic replay depends on contract-aligned journals. |
| R-31-002 | §21.4.4 Compatibility & Validation | Additive schema evolution avoids forced rolling updates. |
| R-31-003 | §21.12 Verification Objectives | Health telemetry must travel with contract payloads for compliance. |

---

## 31.6 Acceptance Criteria & Verification

| Criterion | Acceptance Measure | Verification Approach |
|-----------|--------------------|-----------------------|
| Contract Discoverability | Runtime enumerates all required contracts and rejects incompatible modules. | Startup discovery test ensures publishers/consumers match expected schema list. |
| Replay Equivalence | Replay runs produce identical sequence outputs to live runs (pose within 1 cm, coverage 1%). | Regression suite compares replay to recorded runs using identical seeds. |
| Schema Evolution Safety | New contract versions do not break consumers that only understand older releases. | Compatibility tests load previous contract versions and ensure unknown fields are ignored. |
| Health Traceability | Health and diagnostic contracts appear alongside command contracts to support root-cause. | Logging pipeline cross-checks health messages with control loops during fault injection. |

---

## 31.7 Data Integrity Patterns (Informative)

- **Structured Journals:** All contract updates are stamped with sequence numbers and SimClock timestamps so consumers can detect dropped or reordered messages.  
- **Transport Abstraction:** Whether running in-process, across IPC, or in remote UIs, the same contract payloads apply; transports may be gRPC, shared memory, or files.  
- **Schema Registry:** A lightweight registry stores contract definitions, supported versions, and validation rules so modules can enforce compatibility before processing.  
- **Logging Streams:** Reusable log channels capture every contract message once and hand off to both replay storage and diagnostics consumers.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2025-11-13 | Initial Domain Data Model SRS referencing contract and replay expectations from §21. | Systems Engineering & Documentation Lead | |
