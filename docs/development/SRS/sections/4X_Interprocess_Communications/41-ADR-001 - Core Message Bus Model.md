---
title: 41-ADR-001 - Core Message Bus Model
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
notes: Records the decision to use a single in-proc message bus for Core contracts.
---

# 41-ADR-001 - Core Message Bus Model

*(Status: Proposed)*

## 1) Context

Section 41 requires deterministic ordering, schema validation, and backpressure. Legacy patterns used direct calls and shared state, which broke ordering and replay.

---

## 2) Decision

Adopt a single in-process message bus inside Core with schema-validated topics, ordered envelopes, and bounded queues.

### Decision Summary

- **Scope:** Core domains and plugins.
- **Boundary:** External APIs use bridges, not direct bus access.
- **Implementation Level:** Architectural policy with runtime enforcement.

---

## 3) Consequences

**Positive:** Deterministic ordering, easy replay, unified metrics.

**Negative:** Requires careful queue sizing and backpressure policies.

**Follow-up Actions:** Define queue sizes and topic priorities in Section 24.

---

## 4) Alternatives Considered

| Option | Summary | Reason Not Selected |
|--------|---------|---------------------|
| Direct method calls | Direct invocation between modules. | Breaks ordering and replay. |
| External broker | Separate process message broker. | Adds latency and operational complexity. |

---

## 5) References

- SRS: 41_Message_Bus.md

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial message bus decision. | Systems Engineering & Documentation Lead | |
