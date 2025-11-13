---
title: "PGN Messaging Strategy Review"
version: 0.1.0
status: Draft
authors:
  - Systems Engineering Working Group
owner: Systems Engineering Working Group
reviewers:
  - Communications Team
approvers:
  - Project Coordinator
created: 2025-11-10
review_cycle: Quarterly
notes: Aligns the planned PGN rewrite with ISOBUS semantics and records the current catalog to keep migration options visible.
---

# PGN Messaging Strategy Review
*(Status: Drafting - Decision Agnostic)*

**Section ID:** 4X-53 | **Version:** 0.1.0  
**Section Number:** 4X-53  
**Related Sections:** 4X-ADR-006, 4X-ADR-016, 4X-47 - Live Telemetry Mesh  
**Upstream Dependencies:** 2X - System Architecture, 6X - Core Domain Services  
**Downstream Impacts:** 5X - Solution Delivery, 9X - Frontends & Ops

## 4X.1 Purpose & Scope

This section defines the PGNs that must exist in the AgOpenNext mesh going forward and compares the legacy AgIO catalog to the ISOBUS-aligned rewrite the founder and reformers are championing.  It captures the baseline framing, the target semantics, and the requirements that guarantee steering, section control, fusion, and monitoring continue to interoperate across CAN, UDP, and serial transports.

## 4X.2 Context

- Depends on AgIO Link discovery flows (`C8`-`CB`, `DD`, `DE`) remaining available during migration so old hardware can still join the mesh.  
- Interfaces to the guidance, section-control, and fusion domains (`6X`) must continue to observe PGN semantics even if the transport changes.  
- Out of scope: nanopb schema selection or tooling; this section focuses on the semantic catalog and ISOBUS mapping.

### 4X.2.1 Current PGN Baseline

AgIO today frames every message as `0x80 0x81 Src PGN Len Data CRC`, with the same payload shared across UDP and serial (serial adds COBS framing).  The catalog listed below shows the core steer, machine, IMU, GPS, and tool PGNs that the fleet already understands.

| Module | PGN Name | PGN (dec) | Summary of payload intent |
|--------|----------|-----------|---------------------------|
| Steer | `Steer Data` | 0xFE (254) | Speed demand, steer angle, lateral error, section bitfields |
| Steer | `Steer Settings` | 0xFC (252) | PID gains, PWM/H bridge limits, encoder offsets |
| Steer | `From AutoSteer` | 0xFD (253) | Actual steering feedback and IMU heading/roll |
| Machine | `Machine Data` | 0xEF (239) | Drive speed, tram/geo-stop, hydraulics, section masks |
| Machine | `Machine Config` | 0xEE (238) | Hydraulics timers, enable bits, user flags |
| Machine | `Pin Config` | 0xEC (236) | 24 GPIO/section assignments |
| Machine | `Section Dimensions` | 0xEB (235) | Min/max distances for 16 sections |
| Machine | `64 sections` | 0xE5 (229) | 64-bit on/off mask, speed feedback |
| IMU | `From IMU` | 0xD3 (211) | Heading, roll, gyro |
| IMU | `IMU Disconnect` | 0xD4 (212) | Disconnect indicator |
| GPS | `Main Antenna` | 0xD6 (214) | Position/velocity/heading/HDOP plus IMU derivatives |
| GPS | `Tool Antenna` | 0xD7 (215) | Tool-specific positioning |
| Combined | `ToAutosteer` | 0xF9 (249) | Wheel angle sensor |
| Tool Steer | `Tool Steering` | 0xE9 (233) | Tool XTE, status, speed |
| Tool Steer | `Tool Settings` | 0xE7 (231) | Motor inversion/driver selectors |
| Tool Steer | `From Tool Steer` | 0xE6 (230) | Feedback, PWM, switches |
| Tool Steer | `Switch Control` | 0xEA (234) | Section groups, master switches |

### 4X.2.2 ISOBUS-aligned messaging path

The rewrite aligns with ISO 11783 DDI semantics: PGN 290 (and 291+) for desired section states, PGN 161/162+ for actual section feedback, PGN 367 for overrides, PGN 289 for master setpoints, and PGN 141 for master feedback.  Every payload follows ISOBUS command/element/DDI triples so scalings and units remain consistent across vendors.  CAN remains the primary transport for this stack, but the AgIO header (plus UDP/serial framing) bridges legacy modules until every node migrates.

## 4X.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference |
|--------------|-----------------|-----------------------|---------------------------|-----------|
| Section control | AgIO encoded sections as fixed masks (`E5`, `EF`) | Fragmented semantics, no DDI alignment, 16-bit and 64-bit inconsistently handled | Consolidate into ISOBUS PGNs (290/161/367/289/141) with optional 1/2/8/16-bit monitoring | `ISOBUS_Section_Control.md` |
| Steering | Steer command, settings, and feedback split across PGNs with `***` fields | No clear identifier for multiple active steering devices | Merge steering into a single command/feedback pair with per-device identifier bits | `AgIO_PGN_Baseline.md` |
| Fusion & GPS | Multiple GPS/IMU PGNs plus tool-specific messages | Duplication and unclear lever arm mapping | Define one GPS PGN, one IMU telemetry PGN, plus fusion config/pose PGNs | Section 7 below |
| Monitoring | No structured per-section monitoring, relies on heuristics | Hard to know if sections follow commands | Add PGN for monitoring states with selectable resolution (1/2/8/16 bit) | Operator WG notes |

## 4X.4 Definitions

| Term | Definition |
|------|-----------|
| **PGN** | Parameter Group Number from the J1939 specification, reused here for AgOpenNext framing. |
| **Condensed work state** | ISOBUS encoding that packs a section's desired state into two bits. |
| **DDI (Data Dictionary Identifier)** | ISO 11783 field that encodes command, element, and DDI number. |
| **AgIO header** | The `0x80 0x81 Src PGN Len ... CRC` framing retained on UDP/serial. |
| **Fusion pose** | Status broadcast from a fusion engine containing a frame ID and pose. |

## 4X.5 Requirements

| ID | Priority | Category | Summary | Source | Verification |
|----|----------|----------|---------|--------|--------------|
| **R-PGN-001** | MUST | Steer control | Support a single steer command PGN that covers tractors and tools by encoding 2–4 identifier bits, carries speed, steer angle, XTE, and per-section desired states (64 sections, condensed work state). | Section 7 catalog | Packet decoding tests that route identifier bits to the correct actuator |
| **R-PGN-002** | MUST | Steer feedback | Emit a steer feedback PGN that provides actual steer angle, status, and optional wheel sensor delta fields for any active steering node. | Section 7 catalog | Regression tests consuming CAN/UDP/serial streams |
| **R-PGN-003** | MUST | Section control | Provide one section control PGN that encodes condensed work states (PGN 290 semantics), overrides (367), and master state (289/141) with optional 1/2/8/16-bit monitoring. | `ISOBUS_Section_Control.md` | Simulator verifies per-section bits match commanded states |
| **R-PGN-004** | SHOULD | Fusion | Define fusion configuration and pose PGNs (lever arms, frame IDs, DR params) to bootstrap hardware DR fusion and confirm outputs. | Fusion engineering | Fusion hardware verifies lever arm application |
| **R-PGN-005** | SHOULD | Monitoring | Broadcast a monitoring PGN (per-section health, fault flags) so dashboards can compare commanded vs actual states. | Operator feedback | Dashboard metrics highlight deviating sections |
| **R-PGN-006** | MUST | GPS/IMU | Keep singular GPS and IMU telemetry/state PGNs; no other PGN should duplicate position/attitude data. | Legacy baseline | Sensor fusion pipeline validates a single-source pose stream |

### 4X.5.1 Requirement Sources & Rationale

| Req ID | Source | Rationale |
|--------|--------|-----------|
| R-PGN-001 | Steering Working Group | Enables multiple actuators per PGN and avoids legacy fragmentation. |
| R-PGN-002 | Field feedback logs | Keeps one feedback stream for diagnostics. |
| R-PGN-003 | `ISOBUS_Section_Control.md` | Aligns with ISO 11783 condensed work state semantics. |
| R-PGN-004 | Fusion engineering design | Needed to boot fusion engines with accurate lever arms. |
| R-PGN-005 | Operator dashboards | Monitoring ensures commanded vs actual states are auditable. |
| R-PGN-006 | Core navigation team | Prevents duplicated GPS/IMU streams that confuse fusion filters. |

## 4X.6 Acceptance Criteria & Verification

- All new PGNs (command, feedback, section, fusion, monitoring) decode via CAN, UDP, and serial without losing any ISOBUS fields.  
- Section control PGN maintains a 64-section condensed work state and exposes overrides/master per ISO 11783 DDI semantics.  
- Fusion configuration PGN is accepted by hardware DR engines and fusion pose PGN aligns with the configured frame ID.  
- Monitoring PGN resolution (1/2/8/16-bit) is selectable and feeds the operator dashboard comparison metrics.

## 4X.7 Required PGN catalog

This rewrite targets the PGNs listed below; equivalents or deprecated entries from the legacy catalog will be retired or folded into these semantics.

| PGN | Role | Core data need | Notes |
|-----|------|----------------|-------|
| 0xFE (254) | Steer command | Speed demand, steer angle, cross-track error, section work states | Identity bits differentiate up to four actuators; sections use condensed work state bits for 64 sections. |
| 0xFD (253) | Steer feedback | Actual steer angle, status, wheel delta | Generic for every steering entity. |
| 0xFC (252) | Steer tuning | PID/QC gains, PWM limits, encoder offset | Replaces the separate settings PGN. |
| 0xEF (239) | Machine command | Drive speed, tram/geo-stop, section demands | Ties to PGN 290 semantics. |
| 0xEE (238) | Machine config | Hydraulics timers, enable flags, user bits | Captures machine capability metadata. |
| 0xEC (236) | Pin assignments | Section/implement/power gate mappings | Resolves downstream implementation indexes. |
| 0xEB (235) | Section geometry | Min/max extents and grouping | Supports per-section layout for PGN 290/291+. |
| 0xE5 (229) | Section control | 64-bit condensed work state, overrides, master state | Bundles PGNs 290/161/367/289/141 with optional bit-depth for monitoring. |
| 0xD6 (214) | GPS | Aggregated position/heading/velocity/HDOP | Single GPS stream for all receivers. |
| 0xD3 (211) | IMU telemetry | Heading, roll, pitch rate | Core inertial data. |
| 0xD4 (212) | IMU state | Disconnect/liveness flag | Health indicator only. |
| 0xF9 (249) | Fusion config | Lever arms, frame IDs, DR tuning | Drives AgIO fusion setup. |
| 0xF8 (248) | Fusion pose | Pose + frame ID feedback | Verifies DR output. |
| 0xF7 (247) | Rate control output | Section-wise rate requests | Aligns with ISOBUS rate control semantics. |
| 0xF6 (246) | Monitoring | Section health (1/2/8/16-bit), faults | Feeds dashboards per requirements. |

Hello/subnet/diagnostic PGNs (`C8`-`CB`, `DD`, `DE`) remain in place for discovery and messaging.
