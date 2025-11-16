---
title: Units & Coordinate Systems
version: 0.1.0
status: Draft
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
notes: Governs measurement units, angle/time conventions, and coordinate reference transformations for AgOpenNext.
---

# 23 - Units & Coordinate Systems
*(Status: Drafting - Consistent Measures)*

**Section ID:** 23  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2025-11-16  
**Related Sections:** 21 - System Decomposition & Boundaries; 22 - Process Model & Deployment; 31 - Domain Data Model  
**Upstream Dependencies:** 21 - System Decomposition; 22 - Process Model  
**Downstream Impacts:** 31 - Domain Data Model; 61 - Kinematics; 71 - Mapping Core

---

## 23.1 Purpose & Scope

Defines the canonical measurement units, angle/time conventions, coordinate reference systems, and transformation behaviors that AgOpenNext exposes to operators, Plugins, and Extensions. The goal is to keep data exchanges (pose, coverage, guidance, logging) consistent across SI and imperial reporting, preserve precision for safety domains, and ensure coordinate conversions remain deterministic when running in different deployment profiles.

---

## 23.2 Context

- Depends on the Core domain catalog (§21.3) so each Plugin knows which units/frames it must honor.  
- Ties into the Process Model (§22) because timing (UTC, GPS, SimClock) affects how we timestamp measurements.  
- Links to the Domain Data Model (§31) because unit metadata flows through the same contracts used by replay, logging, and UI bridges.

---

## 23.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Limitation | Modernization Opportunity | Reference |
|--------------|----------------|------------|---------------------------|-----------|
| Units | Mixed SI/imperial use inside AgOpenGPS with little metadata. | Unit mismatches between domains and replay/diagnostics. | Enforce declared units and conversion utilities for every contract. | §21.4 Data Contracts |
| Angles & Time | Degrees/minutes mixed; timestamps lacked SimClock alignment. | Guidance loops can drift when combining wall clock and GNSS time. | Standardize on decimal degrees/radians per domain and tie to UTC/GPS with fallbacks. | §21.11 Determinism Constraints |
| Coordinates | Field data assumed local datum; mapping and logging stored differently. | Inconsistent pass geometry when moving between projections. | Require every layer to specify CRS (WGS84, UTM, local) and provide deterministic reprojection routines. | §7X Mapping Geospatial |

> **Informative:** Shows why a dedicated units/coordinate policy is necessary for consistent behavior across Plugins and Extensions.

---

## 23.4 Definitions

| Term | Definition |
|------|-------------|
| **Canonical Unit Set** | A documented set of unit declarations (meters, hectares, degrees, seconds, L/ha) attached to every contract payload. |
| **Coordinate Reference System (CRS)** | The geodetic reference system (e.g., WGS84, NAD83, custom local datum) that defines how lat/long/easting/northing are interpreted. |
| **Conversion Policy** | Deterministic algorithms (including signed/unsigned handling) that translate between unit systems or CRSs. |
| **SimClock Timestamp** | The authoritative time source for deterministic loops; expressed in seconds since epoch with nanosecond sub-precision. |

---

## 23.5 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-23-000 | MUST | Consistency | Every contract that exposes scalar values (distance, speed, angle, rate) MUST include unit metadata and follow the Canonical Unit Set so Plugins and Extensions can convert without ambiguity. | C-21.4, C-31.5 | Contract metadata tests verify unit declarations match expected enumerations. |
| R-23-001 | MUST | Accuracy | Coordinate data (pose, coverage vertices, pass lines) MUST declare CRS and, when reprojected, preserve accuracy within 1 cm  for transformations executed by Core. | C-21.11, C-71.0 | Regression reprojection suite compares outputs to trusted geodesy library. |
| R-23-002 | SHOULD | UX | Operator-facing views (VT dialogs, UI Bridge) SHOULD allow users to toggle SI/imperial units with consistent rounding and display formatting. | C-42.1, C-21.5 | UI localization tests change modes and verify contract values stay unchanged. |
| R-23-003 | SHOULD | Timing | All Plugin datastreams SHOULD reference the SimClock timestamp, deriving from UTC/GPS time to avoid mixing wall-clock sources. | C-22.6, C-21.11 | Replay/time-shift tests confirm logs replay identically regardless of host timezone. |

### 23.5.1 Requirement Sources & Rationale

| Req ID | Source | Rationale |
|-------|--------|-----------|
| R-23-000 | §21.4 Contracts | Units are part of contract metadata to avoid interpretation drift. |
| R-23-001 | §21.11 Determinism | Accurate reprojection required for safety-critical guidance/section control. |
| R-23-002 | §42 UI Bridge | Operator preferences must match displayed values with contract truth. |
| R-23-003 | §22 Process Model | SimClock anchors deterministic timing for replay and SimClock-based logging.

---

## 23.6 Acceptance Criteria & Verification

| Criterion | Acceptance Measure | Verification Approach |
|-----------|--------------------|-----------------------|
| Unit Metadata Completeness | Every contract payload includes unit tags before serialization. | Contract schema tests verify metadata presence. |
| Coordinate Accuracy | Reprojected coordinates stay within 1 cm (horizontal) tolerances. | Geodesic regression harness compares against baseline transformation matrix. |
| Operator Unit Switch | UI toggles do not alter internal values. | UI automation exercises toggle and inspects contract payload. |
| Temporal Consistency | SimClock-aligned records replay identically across timezones. | Replay log comparisons with simulated timezone shifts.

---

## 23.7 Conversion Patterns (Informative)

- **Layer Normalization:** Mapping layers store geometry accompanied by CRS and unit metadata; reprojectors run deterministically with cached transformation matrices.  
- **Unit Registry:** Core maintains a registry of supported units and conversion multipliers (e.g., m?ft, L/ha?gallons/acre), and rejects unsupported combinations at load time.  
- **SimClock Anchoring:** Pose, guidance, and logging metadata always include the SimClock timestamp even when sourced from hardware clocks, ensuring deterministic ordering.  
- **UI Localization:** UI Bridge exposes unit toggle events along with canonical values so remote dashboards can present consistent measurements.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2025-11-16 | Initial units & coordinates section. | Jon Fortney | |
