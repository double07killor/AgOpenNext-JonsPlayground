---
title: 22-ADR-001 - Core-hosted Deterministic Runtime
version: 0.2.0
status: Proposed
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
notes: Records the decision to host Section 21 domains inside a single deterministic runtime scheduler that satisfies Section 22 requirements.
---

# 22-ADR-001 - Core-hosted Deterministic Runtime

*(Status: Proposed)*

**Authors:** Jon Fortney  
**Reviewers:** Systems Engineering Team  
**Created:** 2025-11-12  
**Last Updated:** 2025-11-17  
**Supersedes:**  
**Superseded by:**  
**Related SRS:** `22_Process_Model_Deployment.md`  
**Related Options:** `21_System_Decomposition_Boundaries.md`

---

## 1) Context

Section 22 now expresses the process-model requirements (R-22-001 through R-22-010) that every host mapping must satisfy. This ADR complements that by committing to a single Core runtime scheduler that honors those requirements: hosting the safety-critical domains listed in Section 21 (R-21-001 through R-21-012), sharing the authoritative SimClock (R-22-002), and exposing versioned bridges so UI and simulation hosts can attach (R-22-005). It also captures the tradeoffs considered when deciding how to fulfill the requirements identified in Section 21 and Section 22.

---

## 2) Decision

Continue to execute the Core domain catalog inside a single deterministic process whose scheduling, watchdogs, and contract exposure satisfy the requirements R-22-001 through R-22-010. 

### Decision Summary

* **Scope:** All safety-critical domains defined in Section 21 run inside the Core scheduler to meet R-22-001; non-critical services respect the same contracts via R-22-003 and R-22-005.  
* **Boundary:** Runtime hosts may not split domains across processes without producing a new ADR referencing these requirements.  
* **Implementation Level:** Architecture and deployment policy with runtime configuration; Section 22 requirements drive sequencers, watchdogs, and SimClock handling.

---

## 3) Consequences

**Positive Impacts:**

* Preserves deterministic timing and replay fidelity for R-22-001/R-22-002.  
* Keeps contract enforcement centralized so optional hosts observe the same snapshots (R-22-005).  
* Simplifies fault detection because heartbeats and watchdogs live in the same address space (R-22-004).

**Negative / Mitigated Impacts:**

* Limits per-domain fault containment; mitigation: layered watchdogs, restart policies, and contract gating per R-22-004.  
* Increases scheduling pressure when logging/UI compete with Autosteer; mitigation: throttle/offload them per R-22-003 and rely on worker threads.  
* Requires rigorous contract metadata (R-21-016) to prevent optional services from bypassing Core.

**Follow-up Actions:**

* Update runtime launchers with the watchdog groups specified in Section 22.  
* Publish extension/loading guidance referencing this ADR and Section 21/22 requirements.

---

## 4) Rationale

Model D (Core-hosted dynamic domainss best satisfies the determinism and contract expectations captured in the requirements. Models A/B lack runtime modularity, and Model C (multi-process) introduces IPC latency and SimClock duplication that would violate R-22-001/R-22-002. The single-process approach keeps every execution step aligned to one SimClock while still permitting load-time extensions under contract enforcement (R-21-016/R-22-005).

---

## 5) Alternatives Considered

| Option | Summary | Reason Not Selected |
|--------|---------|---------------------|
| Model A – Monolithic Core | All logic compiled into a single executable with no modular loading. | Lacks decoupling and makes instrumentation/extensibility difficult, preventing compliance with R-21-016. |
| Model B – Modular Monolith | Structure domains but keep them statically linked. | Still ties release cycle to code changes; does not provide the runtime scheduler flexibility R-22-003/22-005 require. |
| Model C – Multi-Process Runtime | Split Core/AgIO/UI/services into separate OS processes. | IPC latency risks breaking R-22-001/R-22-002; orchestrating restarts adds complexity. |
| Model D – Core-hosted Dynamic Domains | Single deterministic scheduler with loadable domains that respect contracts. | **Chosen option.** |

---

## 6) Implementation & Governance

* **Governance ownership:** Systems Engineering & Core Runtime WG owns scheduler policy and approves changes to host boundaries.  
* **Update cadence:** Quarterly review alongside Section 21/22 and whenever new extensions are proposed.  
* **Documentation:** Link Process Model requirements (R-22-001 through R-22-010), Section 21 catalog (R-21-001 through R-21-016), and this ADR; updates must mention this decision before merging.

---

## 7) Risks & Mitigations

| ID | Risk | Impact | Mitigation / Monitoring |
| -- | ---- | ------ | ----------------------- |
| R1 | Domain misbehavior terminates the runtime. | High | Contract validation, health watchdogs, and optional sandboxing for non-critical services. |
| R2 | Non-critical domains steal execution budget from Autosteer. | Medium | Throttle/offload per R-22-003; monitor latencies per Section 22 acceptance criteria. |
| R3 | Platform-specific packaging splits domains back into processes. | Medium | Policy review enforces ADR referencing before introducing new processes; acceptance tests check contract alignment. |

---

## 8) Legacy Implementation Notes

* Legacy AgOpenGPS mixed UI, logging, and control loops with ad-hoc shared state; this ADR references the new catalogs to avoid that anti-pattern.  
* Previously, AgIO lived in companion processes with unversioned formats; the new catalog pushes those responsibilities into Core under normalized message contracts.

---

## 9) Governance Updates

* **Review frequency:** Quarterly (aligned with Section 21/22 reviews).  
* **Decision owner:** Systems Engineering & Documentation Lead.  
* **Compliance metrics:** Traceability matrices link Section 21/22 requirements to this ADR before merging any host changes.

---

## 10) References

* **SRS Sections:** `22_Process_Model_Deployment.md` (R-22-001 through R-22-010), `21_System_Decomposition_Boundaries.md` (R-21-001 through R-21-016).  
* **Option Documents:** Comparative models from Section 21.10.  
* **Prior ADRs:** None.  
* **External References:** Replay determinism goals from Section 23-ADR-002.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.2.0 | 2025-11-17 | Updated ADR to reference the revised Section 22 requirements catalog. | Jon Fortney |  |
