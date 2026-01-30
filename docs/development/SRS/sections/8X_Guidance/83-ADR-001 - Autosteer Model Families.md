---
title: 83-ADR-001 - Autosteer Model Families
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
notes: Records the decision to support multiple autosteer model families.
---

# 83-ADR-001 - Autosteer Model Families

*(Status: Proposed)*

## 1) Context

Section 83 requires high-precision autosteer across diverse equipment. A single model is unlikely to cover all setups.

---

## 2) Decision

Support at least two model families (PID and LQR) with per-machine tuning profiles.

### Decision Summary

- **Scope:** All autosteer deployments.
- **Boundary:** Model selection is a configuration choice per machine.
- **Implementation Level:** Control layer with model plugins and tuning profiles.

---

## 3) Consequences

**Positive:** Better tuning across varied machines.

**Negative:** Increased validation workload.

**Follow-up Actions:** Define tuning workflow and storage in Section 34.

---

## 4) Alternatives Considered

| Option | Summary | Reason Not Selected |
|--------|---------|---------------------|
| Single model | Only PID. | Not flexible enough for all setups. |

---

## 5) References

- SRS: 83_Autosteer_Models.md

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial autosteer model decision. | Systems Engineering & Documentation Lead | |
