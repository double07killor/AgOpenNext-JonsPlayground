---
title: 42-ADR-001 - Core UI Bridge Protocol
version: 0.1.0
status: Proposed
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
notes: Records the decision to use a versioned Core-UI bridge with snapshots, deltas, and command acks.
---

# 42-ADR-001 - Core UI Bridge Protocol

*(Status: Proposed)*

## 1) Context

Section 42 requires a strict contract boundary for UI clients. Legacy shared-memory access and ad-hoc sockets caused drift and unsafe command paths.

---

## 2) Decision

Adopt a versioned bridge protocol with handshake, state snapshots, optional deltas, and explicit command ack/nack.

### Decision Summary

- **Scope:** Desktop UI, remote clients, mobile clients.
- **Boundary:** UI never bypasses bridge or safety gate.
- **Implementation Level:** Protocol spec with schema contracts.

---

## 3) Consequences

**Positive:** Safe command gating, consistent UI across platforms.

**Negative:** Requires a protocol spec and compatibility testing.

**Follow-up Actions:** Publish protocol details in eferences/core/core-ui-bridge.md.

---

## 4) Alternatives Considered

| Option | Summary | Reason Not Selected |
|--------|---------|---------------------|
| Shared memory | Direct UI access to Core state. | Unsafe, unversioned. |
| Raw socket API | Custom messages per client. | Drift and duplication. |

---

## 5) References

- SRS: 42_UI_Bridge.md
- Reference: eferences/core/core-ui-bridge.md

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial Core-UI bridge decision. | Systems Engineering & Documentation Lead | |
