---
title: 23-ADR-001 - Coordinate Reference & Spatial Frames
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
notes: Documents policy for canonical coordinate frames and spatial transformations.
---

# 23-ADR-001 - Coordinate Reference & Spatial Frames

*(Status: Proposed)*

**Authors:** Jon Fortney  
**Reviewers:** Systems Engineering Team  
**Created:** 2025-11-16  
**Last Updated:** 2025-11-16  
**Supersedes:**  
**Superseded by:**  
**Related SRS:** `23_Units_Conventions_Coordinate_Systems.md`  
**Related Options:** 

---

## 1) Context

AgOpenGPS used inconsistent local datums and mixed projections, which made coverage, mapping, and guidance data hard to reconcile across datasets. The Units & Coordinate Systems SRS (§23) requires canonical CRS metadata for every contract; this ADR now enforces that requirement as architecture policy so the Core always performs deterministic spatial frame transformations before exposing data to Plugins, Extensions, or UI bridges.

---

## 2) Decision

Declare WGS84 latitude/longitude as the canonical global frame, allow farm-level custom datums registered in a CRS registry, and require that every spatial payload (pose, pass lines, coverage vertices) carries CRS identifiers plus transformation metadata. Core transformation services compute and cache reprojection matrices deterministically, and raise validation warnings when transformed coordinates drift beyond the 1 cm tolerance specified in §23.5.

Coordinate metadata includes the EPSG code, axis ordering, and unit tags (meters, degrees, feet, etc.), matching the definition of the Canonical Unit Set in §23.4 so each Plugin or Extension interprets geometry magnitude and direction consistently.

### Decision Summary

* **Scope:** Pose, mapping, guidance, section control, logging, replay, and UI Bridge contracts.  
* **Boundary:** CRS transforms run inside Core; UI Bridge and Extensions consume canonical geometries with metadata.  
* **Implementation Level:** Architecture policy; new CRS entries require ADR updates.

---

## 3) Consequences

**Positive Impacts:**

* Guarantees every Plugin/Extension shares the same coordinate semantics and metadata, eliminating mismatched coverage or logging layers.  
* Simplifies traceability since CRS IDs accompany every geometry payload.  
* Supports Extensions by centralizing reprojections in Core instead of scattering logic.

**Negative / Mitigated Impacts:**

* Registering custom datums requires validation; mitigation: reprojection regression tests for each new entry.  
* Transformation math must remain precise; mitigation: rely on trusted geodetic libraries and cache matrices.  
* Metadata propagation increases contract size; mitigation: limit metadata to concise identifiers (EPSG codes or registry IDs).

**Follow-up Actions:**

* Build the CRS registry and deterministic transformation service.  
* Publish reprojection test harness tied to tolerance budgets.  
* Document canonical frames and acceptable error budgets.

---

## 4) Rationale

Spatial consistency is critical for guidance, section control, and mapping. This ADR enforces the SRS requirement for canonical coordinate behavior by keeping transformations inside Core, ensuring every domain sees the same frames regardless of the original sensor or datastore.

---

## 5) Alternatives Considered

| Option | Summary | Reason Not Selected |
|--------|---------|---------------------|
| Each domain handles its own CRS | Domains interpret their own frames. | Leads to inconsistent coverage and logging; not deterministic. |
| Force everything into WGS84 | Convert all data to WGS84 unconditionally. | Discards precision for local datums and restricts farm-specific setups. |
| Registry-driven canonical frame (chosen) | Register datums, transform centrally, publish metadata. | Provides both determinism and flexibility.

---

## 6) Implementation & Governance

* **Governance ownership:** Systems Engineering & Mapping WG maintains the CRS registry.  
* **Update cadence:** Reviewed quarterly; new CRS registrations require ADR approval.  
* **Documentation:** CRS registry docs and SRS entries must cite this ADR.

---

## 7) Risks & Mitigations

| ID | Risk | Impact | Mitigation / Monitoring |
|----|------|--------|-------------------------|
| R-CRS-1 | Erroneous CRS registration corrupts data. | High | Vet new entries via reprojection regression tests. |
| R-CRS-2 | Transformation rounding breaks coverage. | Medium | Use high-precision geodetic library and cache matrices. |
| R-CRS-3 | Extensions ignore CRS metadata. | Medium | Discovery tests reject payloads lacking identifiers.

---

## 8) Legacy Implementation Notes

* AgOpenGPS V6 stored coverage in ad-hoc projections, causing drift between logging and mapping.  
* Legacy transforms were scattered; this ADR centralizes them in Core.

---

## 9) Governance Updates

* **Review frequency:** Quarterly with release candidates.  
* **Decision owner:** Systems Engineering & Documentation Lead.  
* **Compliance metrics:** CRS registry audit logs and reprojection test results traced to this ADR.

---

## 10) References

* **SRS Sections:** `23_Units_Conventions_Coordinate_Systems.md`.  
* **Option Documents:** `23-O1`.  
* **External References:** EPSG/PROJ geodesy guides.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2025-11-16 | Added coordinate CRS ADR. | Systems Engineering & Documentation Lead | |
