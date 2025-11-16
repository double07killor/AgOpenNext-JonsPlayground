---
title: 23-ADR-002 - Temporal Architecture & SimClock Data
version: 0.1.0
status: Proposed
authors:
  - Jon Fortney
owner: Systems Engineering & Documentation Lead
reviewers:
  - Systems Engineering Team
approvers:
  - Project Coordinator
created: 2025-11-16
last_reviewed: 2025-11-16
review_cycle: Quarterly
notes: Defines how AgOpenNext timestamps, replays, and aligns data with UTC/GPS/SimClock.
---

# 23-ADR-002 — Temporal Architecture & SimClock Data

*(Status: Proposed)*

**Authors:** Jon Fortney  
**Reviewers:** Systems Engineering Team  
**Created:** 2025-11-16  
**Last Updated:** 2025-11-16  
**Supersedes:**  
**Superseded by:**  
**Related SRS:** `23_Units_Conventions_Coordinate_Systems.md`  
**Related Options:** `23-O2` (timestamp sourcing)

---

## 1) Context

The legacy AgOpenGPS logs mixed wall-clock timestamps with GNSS time, complicating deterministic replay and cross-region data fusion. §23 of the SRS demands SimClock timestamps for every contract and ties the timing model to UTC/GPS. This ADR formalizes how AgOpenNext captures, publishes, and replays temporal data so all Plugins and Extensions stay synchronized with ISO/ISOBUS expectations.

---

## 2) Decision

Adopt the SimClock as the authoritative timebase for control and logging loops, derive UTC/GPS offsets at boot, and include UTC/GPS/SIM metadata in every timestamped payload exposed on the message bus. Every timestamp uses ISO 8601 formatting with UTC timezone (for example, `2025-11-16T12:34:56.123456Z`) plus the SimClock tick count (seconds since epoch with nanosecond sub-precision) so both humans and machines can correlate logs. Hardware clocks feed SimClock through deterministic publishers; any deviation triggers contract validation errors. Replays use recorded SimClock values to drive deterministic playback.

### Decision Summary

* **Scope:** Pose, guidance, section control, logging, replay, UI Bridge contracts.  
* **Boundary:** SimClock is managed inside Core; Extensions read the metadata but do not adjust it.  
* **Implementation Level:** Policy enforced via Process Model (§22) and Simulation requirements (§21.9).

---

## 3) Consequences

**Positive Impacts:**

* Deterministic replay becomes achievable because every record carries SimClock ticks and standard UTC formatting.  
* Cross-platform data fusion works: UTC/GPS metadata disambiguates regional clock offsets.  
* Safety loops rely on a single heartbeat, preventing mixed wall-time anomalies.

**Negative / Mitigated Impacts:**

* SimClock drift must be corrected; mitigation: rely on GNSS PPS where available and fallback to monotonic timers.  
* Extensions cannot override SimClock; mitigation: provide read-only metadata plus rate adjustments documented in the SRS.  
* Logging UTC/GPS may confuse consumers; mitigation: include human-readable explanations in metadata.

**Follow-up Actions:**

* Document SimClock offset calculations and UTC/GPS metadata payloads.  
* Add SimClock replay harnesses to verify determinism.  
* Audit SimClock providers for compliance.

---

## 4) Rationale

Temporal determinism is foundational to safety-critical control loops and replay fidelity. This ADR ensures SimClock metadata flows through the same contracts described in §23, giving every Plugin the same timing reference while aligning with ISO/ISOBUS timing practices.

---

## 5) Alternatives Considered

| Option | Summary | Reason Not Selected |
|--------|---------|---------------------|
| Use wall clock solely | Timestamp with host time. | Results vary across timezones and replay fails. |
| GNSS-only time | Use GPS time only. | Not available indoors; need fallback. |
| SimClock canonical (chosen) | Derive from GNSS/wall and propagate metadata. | Ensures determinism with fallbacks.

---

## 6) Implementation & Governance

* **Governance ownership:** Systems Engineering & Simulation WG maintains SimClock policies.  
* **Update cadence:** Reviewed with release candidates and resets.  
* **Documentation:** Process Model and Simulation docs reference this ADR for timing budgets.

---

## 7) Risks & Mitigations

| ID | Risk | Impact | Mitigation / Monitoring |
|----|------|--------|-------------------------|
| R-TIM-1 | SimClock drift from GNSS. | High | PPS correction, monotonic fallback. |
| R-TIM-2 | Replay logs misaligned with UTC. | Medium | Include UTC/GPS metadata and validation. |
| R-TIM-3 | Extension attempts to set SimClock. | Low | Discovery tests reject writers.

---

## 8) Legacy Notes

* Legacy logs mixed wall-clock and GNSS time, which broke deterministic replay.

---

## 9) Governance Updates

* **Review frequency:** Quarterly.  
* **Decision owner:** Systems Engineering & Simulation WG.  
* **Compliance metrics:** SimClock replay verification and metadata audit logs.

---

## 10) References

* **SRS Sections:** `23_Units_Conventions_Coordinate_Systems.md`.  
* **Option Documents:** `23-O2`.  
* **External References:** GPS, ISO 8601 timing standards.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2025-11-16 | Added temporal ADR. | Systems Engineering & Documentation Lead | |
