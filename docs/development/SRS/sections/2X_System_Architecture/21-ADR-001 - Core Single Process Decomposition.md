---
title: 21-ADR-001 - Core Single Process Decomposition
version: 0.2.0
status: Proposed
authors:
  - Jon Fortney
owner: Systems Engineering & Documentation Lead
reviewers:
  - Systems Engineering Team
approvers:
  - Project Coordinator
created: 2025-11-15
last_reviewed: 2025-11-17
review_cycle: Quarterly
notes: Establishes the canonical in-process domain catalog that satisfies the requirement catalog in Section 21.
---

# 21-ADR-001 - Core Single Process Decomposition

*(Status: Proposed)*

**Authors:** Jon Fortney  
**Reviewers:** Systems Engineering Team  
**Created:** 2025-11-15  
**Last Updated:** 2025-11-17  
**Related SRS:** `21_System_Decomposition_Boundaries.md`, `22_Process_Model_Deployment.md`  
**Related Options:** N/A

---

## 1) Context

Section 21 now enumerates domain responsibilities as requirements (R-21-001 through R-21-016). This ADR records the architectural choice that all of those domains (kinematics through UI bridge) remain inside Core under a single cohesive decomposition so the requirements can be fulfilled deterministically and consistently. The process model in Section 22 (R-22-001 through R-22-010) will later describe how those requirements map to hosts, but this decision ensures the catalog itself stays intact and usable by downstream deployments.

The domains we expect to host inside Core are:

- **Kinematics (R-21-001)**
- **Guidance (R-21-002)**
- **Autosteer (R-21-003)**
- **Field Management (R-21-004)**
- **Mapping (R-21-005)**
- **Section Control (R-21-006)**
- **Monitoring (R-21-007)**
- **System Health & Telemetry (R-21-008)**
- **Data Logging / Replay (R-21-009)**
- **Simulation (R-21-010)**
- **AgIO / Hardware I/O (R-21-011)**
- **UI Bridge (R-21-012)**

Each domain must keep the contract metadata (R-21-016) so Section 22 can assign runtime hosts and keep bridging consistent.

---

## 2) Decision

Core will host the full domain catalog inside one deterministically scheduled process, exposing each capability through the message bus so Section 22 can orchestrate scheduling, arbitration, and bridging without reintroducing scattered timebases or side-channel access paths. Optional extensions and the simulation controller will run inside this same host so they can share the SimClock and contract metadata directly rather than living in separate address spaces.

### Decision Summary

* **Scope:** All domains listed above plus the shared SimClock/timebase and bridge layers for UI/simulation so R-21-001 through R-21-012 are satisfied in one host.  
* **Boundary:** External UIs remain outside Core but consume these contracts; any additional host must reference R-22-005 before bypassing the single-source-of-truth, but extensions and simulation controllers stay within Core to preserve the deterministic scheduler.  
* **Implementation Level:** Architectural policy controlling how plugins load and which contracts they publish; Section 22 (R-22-001, R-22-002) governs the deterministic scheduler serving this catalog.

---

## 3) Consequences

**Positive Impacts:**

* Keeps a single authoritative pose/command pipeline so Guidance, Autosteer, Mapping, and Section Control never diverge (supporting R-21-001 through R-21-006).  
* Ensures all log/replay/simulation timelines reference the same SimClock as the runtime, satisfying R-21-009, R-21-010, and R-22-002.  
* Allows Monitoring and System Health to aggregate state without cross-process delays, fulfilling R-21-007/R-21-008 while remaining observable through Section 22’s watchdog policy (R-22-004).

**Negative / Mitigated Impacts:**

* Reduces fault isolation between domains; mitigation: the Section 22 watchdog/watchdog response policies (R-22-004) guard against cascading failures.  
* Places more pressure on Core scheduling when optional services run; mitigation: throttle or offload them per R-22-003 and assign them clear contracts (R-21-016).  
* Makes headland/coverage jobs harder to split later; mitigation: Field Management owns headland geometry (R-21-004) and publishes before Guidance/Section Control consume it (R-21-013).

**Follow-up Actions:**

* Keep the Section 21 catalog updated with owners, contract versions, and validation traces for each domain.  
* Land Section 22’s runtime mapping with explicit host assignments that cite this ADR and the relevant R-22 requirements before any deployment.

---

## 4) Rationale

The single-process decomposition best supports the determinism and contract fidelity demanded by the requirements in Section 21 while keeping the implementation surface manageable. Distributed models (multi-process or legacy UI-hosted loops) introduce IPC latency and multiple timebases that would break R-21-001 through R-21-012 and complicate the contract registry (R-21-016). This ADR keeps the catalog stable so Section 22 can focus on scheduling without re-solving what belongs where.

---

## 5) Alternatives Considered

| Option | Summary | Reason Not Selected |
|--------|---------|---------------------|
| Multi-process domain hosts | Split each domain into its own process with IPC. | IPC increases latency and makes SimClock alignment harder, violating R-21-001 and R-22-002. |
| Legacy UI-hosted loops | Keep domains coupled to the remote UI process. | Leads to unsafe shared state and inconsistent contracts (R-21-016). |
| Core-hosted catalog (current) | Single deterministic process with message-bus contracts. | **Chosen for determinism and extensibility.** |

---

## 6) Implementation & Governance

* **Governance ownership:** Systems Engineering & Architecture Guild ensure the catalog aligns with Section 21 requirements and that Section 22 documents host mappings.  
* **Update cadence:** Quarterly review alongside Section 21/22 updates and whenever a new domain/extension proposal arises.  
* **Documentation:** Section 21 catalog, Section 22 deployment docs, and contract registries must cite this ADR when adding or reorganizing domains.

---

## 7) Risks & Mitigations

| ID | Risk | Impact | Mitigation / Monitoring |
| -- | ---- | ------ | ----------------------- |
| R1 | Single-process failure affects every domain. | High | Section 22 watchdogs (R-22-004) and graceful degradation messaging to the UI bridge. |
| R2 | Non-critical domains consume budget needed by Autosteer. | Medium | Throttle/offload per R-22-003 and keep contract metadata current (R-21-016). |
| R3 | Developers introduce new domains without runtime mapping. | Medium | Enforce review process requiring reference to this ADR and Section 22 before merging new capabilities. |

---

## 8) Legacy Implementation Notes

* Legacy AgOpenNext mixes UI, logging, and control loops with ad-hoc shared state; this ADR replaces that behavior with a single catalog referenced in Section 21.  
* Prior AgIO companions exposed raw telemetry without normalized contracts; the new catalog/ADR push those responsibilities into Core (R-21-011).

---

## 9) Governance Updates

* **Review frequency:** Quarterly (aligned with Section 21/22 reviews).  
* **Decision owner:** Systems Engineering & Documentation Lead.  
* **Compliance metrics:** Traceability matrix links requirements in Section 21/22 back to this ADR, ensuring every change references the catalog.  

---

## 10) References

* **SRS Sections:** `21_System_Decomposition_Boundaries.md` (R-21-001 through R-21-016), `22_Process_Model_Deployment.md` (R-22-001 through R-22-010).  
* **Option Documents:** Not applicable.  
* **Prior ADRs:** `22-ADR-001 - Core-hosted Deterministic Runtime.md`.  
* **External References:** Replay/verifiability goals from Section 23-ADR-002.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
| ------- | ---- | ------- | ------------ | ---------- |
| 0.2.0 | 2025-11-17 | Re-tethered ADR to the new Section 21 requirements catalog. | Jon Fortney |  |
