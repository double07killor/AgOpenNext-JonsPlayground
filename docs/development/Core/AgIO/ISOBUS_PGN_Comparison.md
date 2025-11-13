---
title: "ISOBUS PGN Comparison"
version: 0.1.0
status: Draft
authors:
  - Jon Fortney
owner: Systems Engineering Working Group
reviewers: []
approvers: []
created: 2025-11-10
review_cycle: Quarterly
notes: Context document framing the potential rewrite of PGNs with ISOBUS-consistent numbering and semantics.
---

# ISOBUS PGN Comparison

*(Status: Drafting – Context Only)*

## 1. Document Control

- **Purpose:** Define the comparison and alignment strategy between existing AgOpenGPS PGNs and their ISOBUS-compliant counterparts. This document serves as a reference point for protocol design decisions and interoperability planning within AgOpenNext.  
- **Audience:** Architecture and systems engineering teams, firmware developers, and contributors responsible for communication standards and hardware integration.  



## 2. Scope & Context

This document exists to explore how AgOpenNext could evolve its PGN framework toward an **Ethernet high-speed ISOBUS structure**—one that reuses ISO 11783 semantics while taking advantage of Ethernet bandwidth, discovery, and message density features for modern AgIO, Field Controller, and Implement ECU platforms.

### 2.1 Background

AgOpenGPS originally defined a compact PGN set for steering, rate, and section control optimized for low-bandwidth CAN/serial links.  
The rewrite breaks with the old CAN mindset and aligns every PGN to ISO 11783 numbering, command/element pairs, and DDI units while targeting Ethernet transports.  
This alignment improves:

- **Compatibility:** Ethernet-native ISOBUS tooling and gateways will understand AgOpenNext frames with minimal translation.  
- **Scalability:** High-speed links can carry the 64-section condensed work states, fusion telemetry, and monitoring words comfortably.  
- **Maintainability:** A single semantic definition simplifies firmware, logging, diagnostics, and future-proof hardware integrations.

### 2.2 Out of Scope

- Full ISO 11783 certification (VT/TC conformance or object pools).  
- Virtual Terminal/UI definitions.  
- Proprietary diagnostics, bootloaders, or physical-layer specifications.

The focus is **semantic and structural alignment with Ethernet ISOBUS**, not certification or connector standards.

### 2.3 Assumptions

1. AgOpenNext remains PGN-based but shifts the default transport to Ethernet high-speed ISOBUS, keeping CAN/serial only for legacy fallbacks.  
2. Implement control initially centralizes steering/section commands into multi-row ECUs, but every message should also support the discovery features enabled by Ethernet.  
3. Bandwidth assumptions now rely on Ethernet throughput, though payloads remain compatible with CAN-framed bridges when needed.  
4. Functional roles follow the Task Controller—Implement hierarchy so ISOBUS semantics stay consistent.

### 2.4 Objectives

- Map existing AgIO PGNs to the Ethernet ISOBUS PGN numbers (290/161/367/289/141 and others) to anchor the semantics of steering, sections, fusion, and monitoring.  
- Identify redundant legacy structures that collapse once Ethernet bandwidth is available (e.g., merging multiple steer PGNs into a single multi-actuator message).  
- Provide the foundation for an eventual PGN-rewrite ADR that codifies the Ethernet-ready numbering, payload layout, and transitional guidance for CAN/serial readers.

---
## 3. Example System Context — 6-Section AIO with Steering (Current Architecture)

This example models the current AgIO-style **All-In-One (AIO)** controller used in most existing AgOpenGPS setups.  
The AIO combines steering control, IMU and dual-antenna GPS sensing, and six section outputs, communicating with the display over **UDP**.  
PGN semantics follow the ISOBUS pattern for consistency on the Ethernet/UDP link, helping modern PCs and microcontrollers integrate smoothly.

### 3.1 Network Overview

| Role | Function | Notes |
|------|-----------|-------|
| **Cab Display / PC** | User Interface + Task Controller | Runs guidance, mapping, and section control logic; exchanges PGN-framed UDP packets with the AIO. |
| **AIO Controller** | Combined Steering + Implement ECU | Provides steering valve control, IMU, dual GPS, and six section outputs. |
| **Tractor Powertrain (optional)** | Speed source | If available, speed and heading can be injected from a separate ECU or GPS stream. |

All communication occurs over **UDP sockets** using PGN-structured payloads.  
This mirrors ISOBUS message content while taking advantage of the higher throughput that Ethernet/UDP provides for modern PCs and microcontrollers.

---

### 3.2 Initialization (Command-by-Command, UDP / ISOBUS-Aligned)

#### Discovery & Capability

| PGN / DDI | Name | Dir | When / Rate | What it does | Reliability & Failsafe |
|---|---|---|---|---|---|
| **60928** | Address Claim / Hello | Display → AIO (multicast) then unicast | Startup; retry 2–3× @ 500 ms if no reply | Announces Display; asks “who’s here?” | Fire-and-forget; if no AIO response after retries → show “AIO not found”. |
| **60928** | Hello Ack (with NAME) | AIO → Display | On Hello | Confirms presence; provides logical identity | Must ACK (app-level). If Display doesn’t ACK in 150 ms, resend ×3. |
| **59904** | Request | Display → AIO | Immediately after Hello | Requests specific objects (caps, fw info, etc.) | Reliable; if no response ≤ 150 ms → retry ×3, then fault “capabilities unavailable”. |
| **65242** | Capabilities Summary | AIO → Display | On Request | “I support: steering, IMU, dual-GPS, 6 sections; no VR.” | Reliable; if Display doesn’t ACK, resend ×2; on repeated failure AIO stays idle. |
| **61192** | DDI Support Map | AIO → Display | After caps | Lists DDIs supported: 160/161, 400/401, 523/524/525, 529 | Reliable; missing ACK → resend ×2; on failure, suppress streaming. |

#### Binding, Time, Subscriptions, Safety

| PGN / DDI | Name | Dir | When / Rate | What it does | Reliability & Failsafe |
|---|---|---|---|---|---|
| **61193** | Bind Roles | Display → AIO | Post-caps | Declares Display as master for steering/sections/time | Reliable; retry ×3 @ 150 ms. If bind fails → AIO stays SAFE (neutral + sections OFF). |
| **65254** | Time Sync (UTC tick) | Display → AIO | 1 Hz | Aligns timestamps | Best-effort; if missing → AIO still runs but timestamps may drift. |
| **61194** | Telemetry Subscription | Display → AIO | Post-bind | “Send: 523/524/525 @10 Hz, 529 @5 Hz, 401 @20 Hz” | Reliable; if no ACK → retry once; if still no ACK → Display polls 2 Hz until link OK. |
| **65280** | Heartbeat | AIO → Display | 5 Hz | Liveness + health (uptime, err flags, volt/temp) | Best-effort. If 500 ms gap → Display = degraded: stop steering; sections OFF; alert. |
| **61195** | Safety / Mode | Display → AIO | Arm/disarm or fault clear | Set mode = RUN/PAUSE/ESTOP; section interlocks | Reliable; must ACK. If NACK or timeout → retry ×3; if still fail → force PAUSE and notify. |

#### Configuration (once per session or on change)

| PGN / DDI | Name | Dir | When / Rate | What it does | Reliability & Failsafe |
|---|---|---|---|---|---|
| **61196** | Steering Config | Display → AIO | Init / change | Wheelbase, max angle, PID, etc. | Reliable; field-level ACK. If NACK → stop and surface error; don’t enter RUN. |
| **61197** | Section Map | Display → AIO | Init / change | 6 sections: width, offset, latency | Reliable; must ACK. On failure → keep previous map; sections default OFF. |
| **61198** | Config Commit (ver) | Display → AIO | After config | Persist config, echo version | Reliable. If version mismatch after retry ×3 → re-send full config. |

#### Commands (Display → AIO)

| PGN / DDI | Name | Dir | When / Rate | What it does | Reliability & Failsafe |
|---|---|---|---|---|---|
| **61184 / DDI 400** | Steering Command (Angle Setpoint) | Display → AIO | ≈ 20 Hz while RUN | Target steer angle (or equivalent cmd) | Reliable (quick ACK). If no fresh cmd ≥ 150 ms → AIO ramps to 0° (neutral). |
| **61185 / DDI 160** | Section Control (Desired) | Display → AIO | On change + 2 Hz keepalive | Bitmask for 6 sections ON/OFF | Reliable (ACK). If no cmd > 2 s → AIO forces ALL OFF (unless HW override). |

#### Feedback / Telemetry (AIO → Display)

| PGN / DDI | Name | Dir | When / Rate | What it does | Reliability & Failsafe |
|---|---|---|---|---|---|
| **61184 / DDI 401** | Steering Actual Angle | AIO → Display | 20 Hz | Feedback for closed loop & logging | Best-effort; drops OK — next sample wins. |
| **61184 / DDI 523** | Heading Angle | AIO → Display | 10 Hz | IMU fused heading | Best-effort. |
| **61184 / DDI 524** | Pitch/Roll | AIO → Display | 10 Hz | IMU attitude | Best-effort. |
| **61184 / DDI 525** | Yaw Rate | AIO → Display | 10 Hz | Turn rate for guidance | Best-effort. |
| **61184 / DDI 529** | Position (dual-antenna) | AIO → Display | 5 Hz | Lat/Lon (+ derived heading) | Best-effort. |
| **61185 / DDI 161** | Section Actual State | AIO → Display | 2 Hz (+ on change) | Confirms physical outputs | Best-effort stream + ACK to command; on mismatch AIO reasserts outputs and flags error. |

#### Acknowledgment / Errors

| PGN / DDI | Name | Dir | When / Rate | What it does | Reliability & Failsafe |
|---|---|---|---|---|---|
| **61190** | ACK / NACK | Both ways | After any reliable cmd | Positive ACK or error code for last message | If NACK = BUSY → backoff 200–400 ms then retry ≤ 5×. If NACK = BAD_FIELD → stop retries & surface error. |

---

#### Minimal fault / timeout rules (summary)

- **Heartbeat gap ≥ 500 ms:** Display degrades → stop steering cmds; assume sections OFF; alert operator.  
- **Steering cmd gap ≥ 150 ms:** AIO holds then returns to neutral.  
- **Section cmd gap ≥ 2 s:** AIO forces all sections OFF (unless HW override).  
- **Config failures:** do not enter RUN; stay SAFE until config Apply + Commit success.  
- **Lost telemetry:** ignore; next packet supersedes.

---

## 4. Example System Context — 6R/30" Planter with Ground-Drive Seed, 6-Section Planting Shutoff, 6-Section Spray Rate/Section Control, Row Monitoring, and Optional Marker Control

A practical near-term build: ground-drive seed, **AIO handles six planting sections (ON/OFF)**, a **spray rate controller** manages one liquid product with **six spray sections**, and a **16-row monitor** reports population.  
All modules communicate over Ethernet/UDP using PGN-framed payloads (ISOBUS semantics) to match the high-speed links we are targeting.

> **Context note:**  
> This example is based on a **6R/30" International corn planter** from my own operation.  
> The goal is to modernize it by adding spraying so that pass can be handled during planting.  
> Fertilizer application is custom-applied, so in-furrow isn’t a focus here.  
> The planter remains ground-drive for seed rate, which works well, but adding electronic section control, spraying, and monitoring would make it a far more capable small-acre planter.
>
> **Hardware note:**  
> This configuration can be implemented using the current **AIO V5.0 board** (with integrated **IMU** and support for **one or two GPS antennas**), an existing **rate-controller design**, and a new **16-row monitoring board** that utilizes the **16 physical hardware counters on an ESP32** for direct seed-sensor inputs.
>
> **Optional marker control:**  
> Hydraulic or electric row markers can also be managed through this system by assigning two spare section outputs on the Spray Rate Controller or an auxiliary section controller.  
> Marker position and timing can then be logged or automated in sync with the planter lift state.

---

### 4.1 Physical & Functional Layout

| Module | Function | Notes |
|---|---|---|
| **Planter AIO** | **6 planting sections, onboard IMU + GPS (1 or 2 antennas)** | Provides clutch control, motion sensing, and positional feedback. Ethernet link to Display. |
| **Spray Rate Controller** | Liquid product control + **6 spray sections** (+ optional marker IO)** | Controls herbicide or liquid fertilizer; can re-use spare channels for marker actuation. |
| **Row Monitoring Board (16-Row)** | Seed population / singulation monitoring | Six active channels; ten spare for expansion. |
| **Display / PC (Task Controller)** | Mapping, prescriptions, and logging | Acts as the Task Controller; runs guidance and section logic. |
| **Tractor AIO** | Steering, IMU, GPS, and speed broadcast | Defined in § 3; provides position and motion data to the network. |

> Fertilizer can be added as another **product channel** using the same pattern as spraying (own `DDI 14/15` for rate and `160/161` for sectioning).  
> Planting, spraying, and fertilizer sections may **not align**; the Task Controller maintains **independent section maps** for each product.

---

### 4.2 Initialization & Discovery

1. **Discovery (`PGN 60928`)** — Display broadcasts hello; AIO, Spray RC, and Monitor respond with identity & function.  
2. **Capabilities (`PGN 65242 / 61192`)**  
   - **AIO:** `DDI 160/161` (Section Control), `523–525` (IMU angles), `529` (GPS position), `section_count = 6`.  
   - **Spray RC:** `DDI 14/15` (Setpoint / Actual Rate), `160/161` (6 spray sections + optional markers).  
   - **Monitor:** `DDI 15` (Actual Application Rate – Population).  
3. **Binding (`PGN 61193`)** — Display claims master role for planting & spray section control and for rate commands.  
   All ECUs remain SAFE until a valid `Mode = RUN` command is received.

---

### 4.3 Runtime Communication

#### A. Broadcast Inputs (Tractor AIO → All)

| PGN | DDI | Description | Rate (Hz) |
|---|---|---|---|
| **65256** | — | Ground Speed | 10 |
| **65269** | — | Tractor GPS Position | 5 |
| **65267** | — | Heading / Yaw Rate | 10 |

#### B. Planting Sections (Task Controller ↔ AIO)

| PGN / DDI | Dir | Purpose | Rate (Hz) | Reliability / Failsafe |
|---|---|---|---|---|
| **61185 / 160** | TC → AIO | Desired planting section mask (6 bits) | On change + 2 Hz | Reliable; if gap > 2 s → AIO forces all planting sections OFF (unless manual override). |
| **61185 / 161** | AIO → TC | Actual planting section state | 2 + on change | Best-effort; AIO ACKs commands. |
| **61184 / 523–525** | AIO → TC | IMU angles (heading, pitch, roll) | 10 | Best-effort telemetry. |
| **61184 / 529** | AIO → TC | GPS position (single or dual) | 5 | Used for implement position offsets and logging. |

#### C. Spray Rate & Sections (Task Controller ↔ Spray Rate Controller)

| PGN / DDI | Dir | Purpose | Rate (Hz) | Reliability / Failsafe |
|---|---|---|---|---|
| **61184 / 14** | TC → RC | Spray setpoint application rate | 2 – 5 | Reliable; if gap > 1 s → hold last, then ramp to 0 rate. |
| **61184 / 15** | RC → TC | Spray actual application rate | 2 | Best-effort; next sample supersedes. |
| **61185 / 160** | TC → RC | Desired spray section mask (6 bits + optional marker bits) | On change + 2 Hz | Reliable; if gap > 2 s → RC forces all OFF. |
| **61185 / 161** | RC → TC | Actual spray section state | 2 + on change | Best-effort; RC ACKs commands. |

#### D. Row Monitoring (Monitor → Task Controller)

| PGN / DDI | Dir | Purpose | Rate (Hz) | Notes |
|---|---|---|---|---|
| **61184 / 15** | Monitor → TC | Actual population per row | 2 – 5 | Population / singulation feedback only; ground-drive seed rate unchanged. |
| **65280** | Monitor → TC | Heartbeat / Health | 2 | Gap > 1 s → flag stale data; no impact on control. |

---

### 4.4 Combined Operation

1. **Display / TC** ingests speed & GPS, maintaining three independent section maps: *Planting [6]*, *Spray [6]*, and *Markers [2]*.  
2. TC computes spray rate based on speed and map layer → sends `DDI 14` (rate setpoint) + `DDI 160` (section mask) to the Spray Rate Controller.  
3. TC sends `DDI 160` (planting mask) to the AIO for clutch control.  
4. **Spray RC** handles closed-loop flow control, reports `DDI 15` (actual rate) and `DDI 161` (section state).  
5. **AIO** streams its own IMU + GPS data to TC for implement position and logging.  
6. **Row Monitor** streams population for six rows.  
7. **Markers** (optional): TC toggles left/right outputs using spare section bits on the Spray RC (`DDI 160`), or routes commands to a dedicated aux controller.  
8. TC logs as-applied layers for planting, spraying, and marker events independently.

---

### 4.5 Optional Extensions

- Add **fertilizer** as a separate product channel (`DDI 14/15 + 160/161`).  
- Expand **planting sections** as hardware allows (12 or 16).  
- Add **flow and pressure sensors** to AIO for diagnostic feedback or backup rate reporting.  
- Enable **dual-GPS mode** on AIO for accurate implement heading and cross-track offset.  
- Integrate **marker position sensor** for auto-logging and auto-lift when planter raised.

---

### 4.6 Fault & Timeout Behavior

| Condition | Action |
|---|---|
| Planting section cmd gap > 2 s | AIO forces all planting OFF; operator alert. |
| Spray section cmd gap > 2 s | RC forces all spray OFF. |
| Spray rate setpoint gap > 1 s | Hold last 0.5 s, then decay to 0 rate. |
| Monitor heartbeat gap > 1 s | Data flagged stale; control continues. |
| AIO or RC heartbeat gap > 500 ms | TC marks device offline; sections OFF for that device. |
| Marker command timeout | Freeze current marker state until next valid cmd. |

---

### 4.7 Summary Table

| Function | Module | DDIs / PGNs | Rate (Hz) | Reliability | Comment |
|---|---|---|---|---|---|
| Planting sections (6) | AIO | `61185 / 160, 161` | 2 + changes | Reliable cmds; best-effort feedback | Binary clutch control + IMU/GPS telemetry. |
| Spray setpoint | Spray RC | `61184 / 14` | 2 – 5 | Reliable | Closed-loop rate control. |
| Spray actual | Spray RC | `61184 / 15` | 2 | Best-effort | Flow / pressure derived. |
| Spray sections (6) + markers (2) | Spray RC | `61185 / 160, 161` | 2 + changes | Reliable | Independent spray zones + marker control bits. |
| Row population | Monitor | `61184 / 15` | 2 – 5 | Best-effort | Seed monitoring only. |
| Speed / GPS / Yaw | Tractor AIO | `65256, 65269, 65267` | 5 – 10 | Broadcast | Shared inputs for all modules. |

---

This configuration demonstrates how the **AIO V5.0** (with IMU + GPS), an existing **rate-controller**, and a new **16-row monitor** can modernize a classic 6R/30" planter.  
It enables independent 6-section control for planting and spraying, optional marker automation, and per-row monitoring — all while remaining fully ISOBUS-aligned at the PGN/DDI level and using UDP transport for modern connectivity.

- Downforce may also scale with roll angle or turn radius to maintain consistent gauge-wheel load.  
- If turn data absent > 1 s → freeze rates (flat prescription).

---

### 5.5 Fault & Timeout Behavior

| Condition | Action |
|---|---|
| Missing rate cmd > 0.5 s (row) | Row ECU holds last rate then stops motor. |
| Missing downforce cmd > 0.5 s | Hold last force then relax to neutral. |
| Missing section cmd > 2 s | All sections OFF for that product. |
| Missing fert rate cmd > 1 s | Pump idle / bypass mode. |
| Heartbeat gap > 500 ms | TC marks device offline; freezes related commands. |
| Config mismatch or bind failure | SAFE mode – no drive or actuation until resolved. |

---

### 5.6 Example Communication Rates Summary

| Function | Module | DDIs / PGNs | Rate (Hz) | Reliability | Comment |
|---|---|---|---|---|---|
| Seed rate setpoint (32×) | Planter AIO ↔ Rows | `61184 / 14` | 5–10 | Reliable | Per-row variable rate. |
| Seed rate actual (32×) | Rows ↔ Planter AIO ↔ TC | `61184 / 15` | 5 | Best-effort | For as-applied logging. |
| Fert rate setpoint | Fert RC | `61184 / 14` | 2–5 | Reliable | Global product rate. |
| Fert rate actual | Fert RC | `61184 / 15` | 2 | Best-effort | Flow sensor feedback. |
| Downforce setpoint (32×) | Planter AIO ↔ Rows | `61184 / 2014` | 5–10 | Reliable | Per-row force target. |
| Downforce actual (32×) | Rows ↔ Planter AIO ↔ TC | `61184 / 2015` | 5 | Best-effort | Gauge-wheel load feedback. |
| Section enable masks | TC ↔ Planter AIO / Fert RC | `61185 / 160, 161` | 2 + changes | Reliable | Seed and fert ON/OFF states. |
| Planter IMU / GPS | Planter AIO | `61184 / 523–525, 529` | 5–10 | Best-effort | Used for compensation. |
| Heartbeat / Health | All ECUs | `65280` | 5 | Best-effort | Link supervision. |

---

### 5.7 System Behavior Summary

- **Seed Drive:** 32 independent electric motors with per-row variable rate and automatic section shutoff.  
- **Fertilizer:** Dedicated rate controller (8 sections typical) with flow/pressure feedback.  
- **Downforce:** Active per-row hydraulic or electro-mechanical system maintaining target load.  
- **Planter AIO:** Implements turn compensation, aggregates row telemetry, and feeds back planter position and attitude.  
- **Task Controller:** Manages prescriptions and as-applied logging for all three products (seed, fertilizer, downforce).

---

This Section 5 represents the **ultimate AgOpenNext implement architecture** — a scalable, multi-product, fully distributed system using ISOBUS-aligned PGNs and DDIs over UDP.  
It demonstrates how the same communication model powering a 6-row DIY planter can scale seamlessly to a **commercial-grade 32-row machine** with per-row variable-rate seeding, fertilizer application, and closed-loop downforce control, all synchronized under a single unified Task Controller.


---

# 6. A Study of Existing Accepted Solutions

---

### TL;DR

**Classic ISOBUS = CAN @ 250 kb/s.**  
It’s proven, widely adopted, but badly bandwidth-limited.  
**High-Speed ISOBUS (HSI)** is the direct successor: same ISOBUS semantics, modern high-speed transport (Ethernet/UDP). It’s being standardized right now and already appearing in OEM products (e.g. John Deere “Implement Ethernet”).

---

## 6.1 Legacy CAN ISOBUS (ISO 11783) — Limited to 250 kb/s

Classic ISOBUS is built on the **SAE J1939** communications stack, operating at **250 kb/s** on a two-wire CAN bus.  
That physical layer is the fundamental throughput limit for all legacy systems.

| Part | Year (latest base) | Title / Function |
|:----:|:------------------:|:----------------|
| **-1** | 2017 | General standard & system overview |
| **-2** | 2019 | Physical layer – defines 250 kb/s twisted-pair CAN |
| **-3** | 2015 | Data link layer (CAN 2.0B / J1939 framing) |
| **-4** | 2016 | Network layer |
| **-5** | 2018 | Network management & address claiming |
| **-6** | 2018 | Virtual Terminal (VT) – graphical HMI object pools |
| **-7** | 2009 | Implement messages & process data definitions |
| **-9** | 2014 | Tractor ECU (TECU) & power management |
| **-10** | 2015 | Task Controller & data logging |
| **-11** | 2005 | Data Dictionary (DDI) – standardized process variables |
| **-12 – 14** | 2010 – 2016 | Diagnostics, File Server, Sequence Control |

> **Summary:**  
> CAN ISOBUS is robust and universal, but 250 kb/s is a hard ceiling.  
> It cannot handle modern per-row rate control, high-rate telemetry, or HD sensors without segmentation or timing delays.

---

## 6.2 The Future — High-Speed ISOBUS (HSI) 2010 → 2025

**High-Speed ISOBUS (HSI)** is the AEF’s answer to the CAN bottleneck.  
It preserves ISOBUS semantics (PGNs, DDIs, Task Controller behavior) while moving transport to high-speed links such as Ethernet and CAN-FD.

### Timeline & Milestones

| Year | Event / Milestone |
|:----:|:------------------|
| **2011 – 2014** | AEF forms **Project Team 10 (PT10)** to research high-speed transport for ISOBUS. |
| **2017** | HSI formally presented at the **Club of Bologna** as a key future technology. |
| **2019 – 2020** | PT10 completes concept validation and prepares guideline drafts. |
| **2021** | AEF announces planned public HSI guidelines. |
| **2022** | AEF **PlugFest demonstration**: HSI prototype achieving **≈ 4,000 ×** CAN throughput (Ethernet-based). |
| **2023** | Multi-sector working groups define connector, protocol, and power requirements. |
| **2024 – 2025** | HSI published on the **ISO track as ISO 23870**, describing the high-speed interconnect for agricultural and heavy equipment. |
| **2024 – 2025 (OEM adoption)** | **John Deere** introduces “**Implement Ethernet**” on 9-Series tractors (1 Gb/s) and **requires it on MY 2025 electric-drive planters**. Other OEMs begin integrating Single-Pair Ethernet backbones. |

> **Interpretation:**  
> HSI is now in the late-standardization phase.  
> Hardware (connectors, cables) and prototype stacks exist.  
> OEMs are already designing around it.

---

## 6.3 The Upcoming HSI Hardware Connector — What’s Inside (Practical Breakdown)

**Purpose:** The new HSI connector aims to merge **Ethernet, ISOBUS, and power** into one unified, sealed hitch connector for tractors and implements. It’s the ISO world’s official way forward — designed for OEMs building million-dollar implements, not necessarily for DIY or retrofit setups.

### 6.3.1 What the official HSI connector carries
- **High-Speed Data:**  
  - Single-Pair Ethernet (SPE) — 100BASE-T1 or 1000BASE-T1  
  - Differential signaling over one twisted pair with heavy EMC shielding  
  - Typically used for high-rate traffic such as section control, downforce, or cameras

- **Legacy ISOBUS Pass-Through (CAN):**  
  - Classic ISO 11783 CAN lines (250 kb/s) for backward compatibility  
  - Lets one connector serve both modern and legacy implements

- **Power Distribution:**  
  - 12 V and/or 24 V for implement ECUs, sensors, and actuators  
  - Common ground return and shield bonding

- **Physical & Environmental Design:**  
  - Sealed (IP67+), vibration-rated, and keyed  
  - Expensive Rosenberger H-MTD or equivalent connector system  
  - Typically several hundred dollars per harness end today

### 6.3.2 Typical topology

[Tractor HSI Port]
├─ SPE (Ethernet) ─► [Implement HSI Gateway / Switch] ─► [ECUs / Row Modules]
├─ CAN (ISOBUS) ─► [Legacy CAN Backbone]
└─ 12/24 V Power ─► [Implement Power Bus]

The HSI Gateway bridges between **ISOBUS PGNs/DDIs over UDP/IP** and CAN, maintaining full ISO semantics while providing the bandwidth needed for modern systems.

---

## 6.4 A Practical, Low-Cost Approach — HSI Compatibility Without the Price Tag

Rather than chasing the full HSI connector stack, AgOpenNext-class hardware can achieve **100 % functional compatibility** using **commodity Ethernet, CAN, and power connectors**, while still being **electrically ready to link with true HSI hardware later**.

### 6.4.1 How our wiring stays simple (and affordable)
- **Ethernet Data (Main Bus):**  
  - Use standard **RJ45** (bench) or **M12 X-coded** (field) for 100/1000 Base-T.  
  - Run AgOpenNext communications as **ISOBUS-aligned PGN/DDI messages over UDP/IP**.  
  - Fully interoperable with PCs, tablets, ESP32s, Pis, or CM5 modules — no adapters required.

- **ISOBUS (Legacy CAN):**  
  - Keep the familiar **9-pin ISOBUS connector** for compatibility with existing CAN equipment.  
  - Optionally mirror specific PGNs between CAN and UDP using a small **bridge ECU**.

- **Power:**  
  - Maintain standard **Deutsch / CPC / AMP** connectors sized for load.  
  - Ground and shield bonding handled exactly as in current AIO and rate-controller setups.

This layout mirrors the functional groups of HSI without the cost or connector dependency.

---

### 6.4.2 HSI compatibility through flexible PHY layers
Our architecture treats the network **logically as HSI already** — only the *physical* layer differs.

- Default PHY: **Standard 100/1000 Base-T** (two-pair Ethernet) — no special hardware.
- Optional upgrade: **Single-Pair Ethernet (100/1000 T1)** transceiver module if you ever want direct HSI-compliant wiring.  
  - Drop-in mezzanine or media-converter board (e.g., TI DP83TC812, Marvell 88Q2112).  
  - No firmware or protocol changes needed — same UDP/IP packets, same PGNs.

So, you can plug into any Windows or Linux device today, and later interface directly with an OEM HSI harness simply by swapping PHY hardware.

---

### 6.4.3 Compatibility overview

| Link Type | Physical Layer | Speed | Transport | Compatibility |
|------------|----------------|--------|------------|----------------|
| **Legacy ISOBUS** | CAN (2-wire) | 250 kb/s | CAN frames (ISO 11783) | Backward-compatible ECUs |
| **AgOpenNext (Standard)** | Ethernet Base-T (RJ45/M12 X) | 100–1000 Mb/s | UDP/IP (PGN/DDI) | Direct with PCs, ESP32, Pi, CM5 |
| **Optional HSI** | Ethernet Base-T1 (Single Pair) | 100–1000 Mb/s | UDP/IP (PGN/DDI) | Electrical match to OEM HSI ports |

---

### 6.4.4 Why this path makes sense
- **Cheap, available parts:** RJ45/M12 cables and switches cost a fraction of H-MTD harnesses.  
- **Same protocol stack:** We still use official ISOBUS PGNs and DDIs over UDP/IP — already HSI-compliant.  
- **Zero software churn:** Switching to Single-Pair Ethernet later is a hardware change only.  
- **Future-proof:** Compatible with legacy ISOBUS, upcoming HSI networks, and all current AgOpenNext modules.

---

### 6.4.5 Example topology (today and future)
**Current (Base-T):**

[Tablet RJ45]
│
[Ethernet Switch or AIO RJ45/M12]
├─ [Rate Controller]
├─ [Planter Monitor]
└─ [Section Control / AIO]
[ISOBUS 9-pin] ──► [Legacy CAN]
[Power 12/24 V] ──► [Existing harness]


**Future (if needed):**

[Tractor HSI Port (SPE)]
├─ (Media Converter) ─► [RJ45/M12 Network]
├─ CAN pass-through ─► [Legacy Implement ECUs]
└─ Power ─► [AIO / Controller Power Bus]


---

### 6.4.6 Summary

> AgOpenNext will likely **remain on standard Ethernet (RJ45/M12)** for cost, simplicity, and availability.  
> But every message, PGN, and behavior will already align with **ISOBUS/HSI semantics**, so we can plug into future HSI-based tractors or implements with **only a physical-layer swap**, not a software rewrite.

In short: **we’ll stay cheap and practical — but we’ll speak fluent HSI when the time comes.**


## 7. High-Speed ISOBUS (HSI) Integration Strategy for AgOpenNext

### 7.1 Overview

High-Speed ISOBUS (HSI) is the next-generation evolution of ISO 11783 — moving from 250 kb/s CAN to **1 Gbit/s Ethernet over Single-Pair (1000BASE-T1)**.  
It keeps the same logical model (PGNs, DDIs, task control, diagnostics) but trades CAN frames for **UDP/IP messages** and **SOME/IP service discovery**.  
This provides the bandwidth and latency needed for modern systems like **row-by-row rate control, downforce, and camera integration**.

AgOpenGPS already operates over UDP with PGNs. That means its design is **natively compatible with HSI**, even without the expensive new connector hardware.  
However, since HSI specifications are still under restricted circulation (expected to open publicly before 2027), AgOpenNext would continue to use its **legacy PGN structure** alongside an **HSI-aligned protocol** when documentation allows— both active on the same network.

---

### 7.2 Philosophy: Dual Protocol, Shared Network

AgOpenNext will **always support both** legacy PGN, and HSI message formats.  
Each device firmware will explicitly identify which protocol it uses — but they’ll share the same UDP/IP transport, cabling, and switches.

| Firmware Type | Description | Typical Use Case |
|----------------|-------------|------------------|
| **Legacy PGN Firmware** | Current AOG UDP PGNs (simple, proven, minimal setup). | Existing AIOs, steer modules, machine controllers. |
| **HSI Firmware** | ISOBUS-aligned PGNs/DDIs with HSI discovery semantics. | Future AIOs, section/row rate ECUs, downforce modules. |

Both can operate **simultaneously on the same network** — one 100 Mb/s or gigabit Ethernet backbone — without interference.  
Discovery messages include a protocol identifier, so AgOpenNext can automatically route and interpret each stream correctly.

> There is no migration or sunset plan — **legacy PGN stays forever**, and HSI becomes an optional enhancement layer.

**Note:**  
The purpose of having this discussion now is to ensure that, if we ultimately adopt HSI as the *primary* protocol moving forward, the internal core communication structures are already aligned with ISOBUS-style PGNs and DDIs. Designing around HSI from the start allows straightforward conversion to legacy PGNs when needed — whereas attempting to retrofit HSI semantics onto a legacy-centric architecture later could be more complex and error-prone.
---

### 7.3 Typical Network Layout

**AgOpenNext Network Example**

- **Host:** Display / PC / AgOpenNext controller (Task Controller)
- **Link:** Ethernet / UDP network (10/100/1000 Mb/s via RJ45 or M12 connectors)
- **Legacy Devices:** AIO v5.0 (steer + IMU), rate controller, section controller, planter monitor
- **HSI Devices:** AIO v6.0 HSI, downforce ECU, high-speed sensors, camera/vision nodes
- **Network behavior:** Legacy PGNs and HSI PGNs share the same switches and transport while exposing their respective discovery/PGN formats

Each module connects through a **standard unmanaged Ethernet switch**.
Gigabit switches are recommended (and already common in AOG builds), though **100 Mb/s switches work perfectly** for current systems.
The switch automatically negotiates link speed per port, so a single 100 Mb/s device does *not* slow down the entire network.

### 7.4 Transport and Discovery

| Layer | Standard / Mechanism | AgOpenNext Implementation |
|--------|----------------------|-----------------------------|
| **Physical** | 100/1000 BASE-T (RJ45/M12) or 1000 BASE-T1 (SPE) | Standard wiring today; optional HSI PHY module later. |
| **Transport** | UDP/IP | Shared for both Legacy and HSI firmware. |
| **Discovery / Registration** | ISO 17215 / SOME-IP (HSI) | Simplified broadcast model now, full ISO alignment when public. |
| **Data Model** | ISOBUS PGNs & DDIs | Legacy PGNs retained; HSI adds ISO-mapped identifiers. |
| **Timing** | < 10 ms response (HSI target) | AgOpenNext already achieves sub-5 ms update rates. |

---

### 7.5 Coexistence and Interoperability

- Both protocol types coexist on the **same Ethernet subnet**.
- A master node (e.g., AgIO or display host) manages discovery, configuration, and routing between them.
- Shared power, shielding, and CAN integration remain identical to current AOG harness layouts.
- Legacy CAN ISOBUS can be bridged in via an Ethernet gateway for mixed systems.

**Example Mixed Setup:**

- Tablet running AgOpenNext → 1 Gb/s link → Switch → AIO v5.0 @ 100 Mb/s (legacy PGN)
- Tablet running AgOpenNext → 1 Gb/s link → Switch → HSI Downforce ECU @ 1 Gb/s (HSI PGNs)
- Tablet running AgOpenNext → CAN Bridge → Legacy ISOBUS Network

All links operate independently at their negotiated speeds—no bottlenecks, no firmware confusion.

### 7.6 Development and Release Roadmap

| Year | Milestone | Impact |
|------|------------|--------|
| **2025** | Continue using existing AOG PGNs (UDP). | Proven and stable baseline. |
| **2026** | Prototype HSI firmware layer; begin discovery integration. | Dual-protocol validation on shared network. |
| **2027 (GA)** | AgOpenNext v1.0/AgOpenGPS V7.0 release. | Fully supports both legacy PGN and HSI firmware simultaneously. |
| **Post-2027** | Optional hardware upgrade to 1000BASE-T1 (SPE). | Physical compatibility with OEM HSI implements. |

---

### 7.7 Summary

AgOpenNext’s communication stack is already 90 % aligned with HSI by design.  
We’ll continue to run **simple, proven UDP PGNs** for legacy devices, while **adding HSI-aligned messaging** for new hardware as public documentation allows.  

> **Legacy PGN will always be supported.**  
> **HSI-capable firmware is optional.**  
> Both will run side-by-side on the same Ethernet bus — ensuring low-cost compatibility today and seamless integration with future high-speed ISOBUS systems when they arrive.




## 8. Appendix

### Changelog

| Version | Date | Summary | Author | PR / Issue |
| --- | --- | --- | --- | --- |
| 0.1.0 | 2025-11-10 | Created first draft. | Jon Fortney @double07killor |  |
