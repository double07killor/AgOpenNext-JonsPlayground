---
title: 31-ADR-001 — Schema-Governed Data Contracts
version: 0.1.0
status: Proposed
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
notes: Documents the decision to enforce typed, versioned contracts for all domain exchanges and telemetry journals.
---

# 31-ADR-001 — Schema-Governed Data Contracts

*(Status: Proposed)*

**Authors:** Jon Fortney  
**Reviewers:** Systems Engineering Team  
**Created:** 2025-11-13  
**Last Updated:** 2025-11-13  
**Supersedes:**  
**Superseded by:**  
**Related SRS:** `31_Domain_Data_Model.md`  
**Related Options:** `31-O?-Contract Styles`

---

## 1) Context

Legacy AgOpenNext relied on shared memory, ad-hoc playlists, and undocumented log formats for pose, coverage, and health data (§21.2). Without explicit contracts, determinism, replay fidelity, and cross-platform consistency broke down quickly (§21.4, §21.9). The new AgOpenNext baseline treats every exchange as a versioned struct, independent of transport, with discovery tests, validation rules, and replay/evolution guardrails (§21.4.2–§21.4.5). This ADR captures the commitment to that discipline.

---

## 2) Decision

Enforce schema-governed contracts for every domain message and telemetry journal, with a central registry of versions and validation rules, so that both live operation and replay use the same verified payloads.

### Decision Summary

* **Scope:** Pose telemetry, spatial layers, guidance intents, actuator setpoints, health, diagnostics, and logging/replay streams.  
* **Boundary:** Does not prescribe transport (in-memory, IPC, or remote API) as long as payloads match the schema registry.  
* **Implementation Level:** Architectural policy linking runtime contract discovery tests, logging pipelines, and documentation for future SDK contributors.

---

## 3) Consequences

**Positive Impacts:**

* Guarantees deterministic replay because every record can be replayed against the same schema (§21.9).  
* Simplifies extension: new modules can adopt newer versions of a contract while older modules ignore unknown optional fields (§21.4.4).  
* Enables multiple frontends and automation clients to rely on stable payloads without reading internal memory (§21.5, §31). 

**Negative / Mitigated Impacts:**

* Requires upfront investment in schema tooling (generation, validation). Mitigation: integrate generation into build/CI and reuse existing contract catalogs.  
* Schema evolution must be managed (versions, backward compatible changes). Mitigation: add governance around additive-only changes and discovery tests (per §31.5.1).  
* Logging infrastructure must enforce ordering to preserve determinism; mitigate with sequence numbers/timestamps and journaling helpers (§21.9, §31.7).

**Follow-up Actions:**

* Build a contract registry service that exposes schema definitions and validation helpers for C#, AgIO, UI, and simulation components.  
* Automate discovery tests during startup and test pipelines to catch missing/unknown contracts early.  
* Document how schema versions are bumped and how replay consumers handle older records.

---

## 4) Rationale

Unstructured messaging was the legacy anti-pattern (§21.2). A schema-first approach directly addresses the determinism (§21.11), contract governance (§21.4), and replay (§21.9) requirements captured in the Domain Data Model SRS. While alternatives like flexible JSON or message passing lacked strong typing and verification, schema contracts permit compile-time validation, forward compatibility, and telemetry gating so the same data surfaces serve both live control and replay/diagnostics.

---

## 5) Alternatives Considered

| Option | Summary | Reason Not Selected |
|--------|---------|---------------------|
| Simple Shared Memory | Domains access shared structs with no version checks. | Breaks determinism and blocks replay verification; no schema evolution support. |
| JSON-over-IPC | Send JSON payloads across any transport. | Too verbose for hard real-time loops and lacks deterministic binary representation. |
| Contract Registry (Chosen) | Typed, versioned schema catalog with validation built into every module. | Provides determinism, validation, and forward compatibility. |

---

## 6) Implementation & Governance

* **Governance ownership:** Systems Engineering Data WG maintains the schema registry and reviews contract changes.  
* **Update cadence:** Contract changes require quarterly review; major version bumps trigger a traceability update in the Domain Data Model SRS and this ADR.  
* **Documentation:** Schema registry docs, replay guidelines (§21.9), and the Process Model SRS must be kept in sync with any contract change.

---

## 7) Risks & Mitigations

| ID | Risk | Impact | Mitigation / Monitoring |
|----|------|--------|-------------------------|
| R1 | Schema drift between publishers and consumers. | High | Discovery tests and startup validation to detect mismatches. |
| R2 | Replay logs become incompatible after schema changes. | Medium | Version metadata in logs and backward compatibility rules requiring additive changes. |
| R3 | Contract enforcement slows down bootstrapping. | Low | Cache validated schemas; perform discovery asynchronously after health gating critical loops. |

---

## 8) Legacy Implementation Notes

* **AgOpenNext:** Used custom binary blobs and replay stubs, which diverged from production behavior and made QA unreliable.
* **AgIO PGNs:** Legacy PGN parsing was duplicated across components; the new registry centralizes schema definitions to avoid drift.

---

## 9) Governance Updates

* **Review frequency:** Quarterly with every release candidate.  
* **Decision owner:** Systems Engineering & Documentation Lead with the Data & Contracts WG.  
* **Compliance metrics:** Track discovery test pass rate, replay fidelity metrics, and contract registry audit logs.

---

## 10) References

* **SRS Sections:** `31_Domain_Data_Model.md` — Requirements and metrics for contracts/journals.  
* **Option Documents:** `31-O?-Contract Styles` (for future formalization).  
* **Prior ADRs:** None.  
* **External References:** §21.4 Contract Principles, §21.9 Replay Requirements.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2025-11-13 | Added schema-governed data contract ADR. | Systems Engineering & Documentation Lead | |
