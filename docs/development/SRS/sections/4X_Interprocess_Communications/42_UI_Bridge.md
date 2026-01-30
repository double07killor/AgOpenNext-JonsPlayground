---
title: 42 - UI Bridge
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
notes: Defines the Core to UI bridge contract for state, commands, and safety gating.
---

# 42 - UI Bridge
*(Status: Drafting - Core to UI Contract)*

**Section ID:** 42  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2026-01-23  
**Related Sections:** 41 - Message Bus; 91 - UI Shell; 94 - Remote Clients  
**Upstream Dependencies:** 41 - Message Bus; 22 - Process Model  
**Downstream Impacts:** 91 - UI Shell; 92 - Gauges & Panels; 94 - Remote Clients

---

## 42.1 Purpose & Scope

Define the contract boundary between Core and UI so all UIs (desktop, remote, mobile) consume consistent snapshots, issue validated commands, and never bypass safety logic.

---

## 42.2 Context

- UI Bridge is the only ingress path for operator commands.
- UI Bridge uses schema contracts and capability metadata from the registry.
- Out of scope: UI rendering details (Section 91/92).

---

## 42.3 Interface Model

The UI Bridge is a strict contract boundary with the following flows:

1. **Handshake:** UI negotiates API version, contract versions, and capability catalog.
2. **Session:** Core issues a session id, token scope, and heartbeat interval.
3. **State channels:** UI subscribes to topics (snapshots + deltas) by capability id.
4. **Command channel:** UI sends commands with operator context and receives ack/nack.
5. **Safety gating:** Core validates every command against mode, health, and safety state.

--- 

## 42.4 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference / Source |
|--------------|-----------------|------------------------|---------------------------|--------------------|
| UI data access | Direct shared memory. | Safety and version drift. | Strict contract bridge. | AOG UI loops |
| Commands | Direct method calls. | No audit trail. | Command envelope + gating. | Legacy regressions |
| Remote UI | Ad-hoc TCP. | No auth or latency handling. | Versioned bridge with auth. | Community forks |

---

## 42.5 Definitions

| Term | Definition |
|------|-------------|
| UI Bridge | The transport and schema boundary between Core and all UIs. |
| Command Gate | The policy engine that validates operator commands. |
| Capability Card | Metadata describing a feature (name, version, fields, UI hints). |

---

## 42.6 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-42-000 | MUST | Safety | The UI Bridge MUST enforce command gating (auth, mode, safety state) before any command enters Core. | C-22.4 | Fault injection rejects invalid commands. |
| R-42-001 | MUST | Capability | The UI Bridge MUST expose state snapshots using schema-governed contracts. | C-31.5 | Snapshot schema validation passes. |
| R-42-002 | MUST | Reliability | The UI Bridge MUST provide command ack/nack with reason codes. | C-41.2 | 100% of commands return ack or nack. |
| R-42-003 | SHOULD | Performance | UI snapshot latency SHOULD be <= 200 ms p95 on local host. | C-22.3 | Benchmark meets target. |
| R-42-004 | MUST | Security | Remote UI connections MUST require authenticated sessions and signed tokens. | C-22.4 | Unauthorized sessions fail handshake. |
| R-42-005 | SHOULD | Resilience | The UI Bridge SHOULD support offline/restore semantics for remote clients. | C-94.0 | Reconnect restores latest snapshot within 2 s. |

---

## 42.7 Acceptance Criteria & Verification

- Command gating tests validate auth, mode, and safety state rules.
- Snapshot latency benchmarks meet p95 targets.
- Remote handshake tests validate token enforcement.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial UI Bridge requirements. | Systems Engineering & Documentation Lead | |
