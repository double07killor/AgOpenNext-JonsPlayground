---
title: 23-ADR-003 - Generic Unit System & Measurement Conventions
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
notes: Captures the policy for canonical unit declarations and conversion practices.
---

# 23-ADR-003 — Generic Unit System & Measurement Conventions

*(Status: Proposed)*

**Authors:** Jon Fortney  
**Reviewers:** Systems Engineering Team  
**Created:** 2025-11-16  
**Last Updated:** 2025-11-16  
**Supersedes:**  
**Superseded by:**  
**Related SRS:** `23_Units_Conventions_Coordinate_Systems.md`  
**Related Options:** `23-O3` (unit metadata handling)

---

## 1) Context

AgOpenGPS previously stored scalars without explicit units, leading to ambiguous conversions between meters/feet, liters/hectares, and degrees/radians. §23 requires the Canonical Unit Set (meters, seconds, degrees, liters, hectares, etc.) in contract metadata. This ADR enforces the practice so Plugins and Extensions convert from canonical values rather than mutate them.

---

## 2) Decision

Adopt a unit profile model: each contract schema declares its canonical unit, and every bus session/topic advertises `unit_profile_id` (e.g., `CUS:v1`) plus a hash of the Canonical Unit Set metadata. Payloads that remain in canonical units skip per-value tags, while non-canonical fields carry a compact `unit_id` or message-level override so receivers know the deviation. Metadata entries follow ISO 80000 naming conventions and the registry codes, and Core exposes conversion helpers for UI/Extensions that need presentation-only units. Unit conversions stay outside Core's canonical storage/message bus.

### Decision Summary

* **Scope:** All scalar contract values (distance, speed, angles, flow rates), including agricultural units when expressed in canonical equivalents.  
* **Boundary:** Core stores canonical units; UI Bridge/Extensions translate for presentation, referencing the unit metadata tags.  
* **Implementation Level:** Policy enforced via contract schema validations and the unit registry.

---

## 3) Consequences

**Positive Impacts:**

* Removes ambiguity about metrics flowing between Plugins.  
* Enables consistent conversions across UI and replay, honoring the unit metadata.  
* Prevents drift when toggling display units (SI vs. imperial) because canonical values stay unchanged.

**Negative / Mitigated Impacts:**

* Extensions must respect canonical metadata; mitigation: provide SDK helpers.  
* Additional metadata increases contract size; mitigation: use compact enumerations (unit codes).  
* Mistaken unit declarations break conversions; mitigation: schema validation rejects mismatches.

**Follow-up Actions:**

* Publish the Canonical Unit Set and unit code metadata table in `docs/development/SRS/references/Canonical_Unit_Set.md`.  
* Build contract validation suites that check unit tags against ISO 80000.  
* Document how to use conversion helpers in Extensions.  
* Extend the registry with ag-specific derived units and conversions.

## Appendix: Canonical Unit Set (Generic Unit System)

This appendix references only the raw base units; derived units and metadata live in `docs/development/SRS/references/Canonical_Unit_Set.md`. Each contract references the same codes when declaring unit profiles.

| Measurement | Canonical Unit | Unit Code | Notes |
|-------------|----------------|-----------|-------|
| Distance | Meter (`m`) | `UOM_LEN_M` | Linear measurements (pose, coverage). |
| Time | Second with ISO 8601 UTC | `UOM_TIME_S` | SimClock timestamps. |
| Mass | Kilogram (`kg`) | `UOM_MASS_KG` | Yield/seed totals. |
| Angle | Degree (`deg`) | `UOM_ANGLE_DEG` | Headings, convergence. |
| Volume | Liter (`L`) | `UOM_VOL_L` | Tanks and fluids. |

Refer to the registry for composite/derived units, examples, and metadata encoding.

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2025-11-16 | Added generic unit ADR. | Systems Engineering & Documentation Lead | |
