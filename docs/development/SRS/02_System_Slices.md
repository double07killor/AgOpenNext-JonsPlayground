---
title: AgOpenNext System Slices Map
version: 0.1.0
status: Draft
authors:
  - Jon Fortney
owner: Systems Engineering & Documentation Lead
reviewers:
  - Systems Engineering Team
approvers:
  - Project Coordinator
created: 2025-11-08
last_reviewed: 2025-11-08
review_cycle: Quarterly
notes: Index of SRS sections and their dependencies; metadata per governance policy.
---

# System Slices Map

*(Status: Drafting)*

**Authors:** Jon Fortney
**Last Updated:** 2025-10-24

---


This index lists every active section in the SRS with quick links. Each section stays decision-neutral until an ADR is written.

| ID | Section | Scope | Dependencies / sequencing hints |
|----|---------|-------|----------------------------------|
| **1X – Platform Foundations** |  |  |  |
| 11 | [OS Support](sections/1X_Platform_Foundations/11_OS_Support.md) | Supported OSes, deployment models, baseline hardware assumptions. | **Upstream:** —  **Downstream:** 12, 21, 52 |
| 12 | [Language & Runtime](sections/1X_Platform_Foundations/12_Development_Language_Runtime.md) | Managed runtime policy, language versions, dependency governance. | **Upstream:** 11  **Downstream:** 14, 21, 53, 94 |
| 13 | [UI Framework & UX](sections/1X_Platform_Foundations/13_UI_Framework_UX.md) | Cross-platform UI stack, MVVM patterns, theming conventions. | **Upstream:** 11–12  **Downstream:** 71 (shell), 72 (maps) |
| 14 | [Build & Tooling](sections/1X_Platform_Foundations/14_Build_Tooling.md) | Toolchains, reproducible builds, signing, developer onboarding. | **Upstream:** 12  **Downstream:** 94 (release), 96 (CI gates) |
| **2X – System Architecture** |  |  |  |
| 21 | [Decomposition](sections/2X_System_Architecture/21_System_Decomposition_Boundaries.md) | Who owns what: Core, AgIO, UI, plugins. | **Upstream:** 11–13  **Downstream:** 22, 41, 42 |
| 22 | [Process Model](sections/2X_System_Architecture/22_Process_Model_Deployment.md) | In-proc, split-core, remote, containers. | **Upstream:** 21  **Downstream:** 41, 42 |
| 23 | [Units & Coordinates](sections/2X_System_Architecture/23_Units_Conventions_Coordinate_Systems.md) | SI/imperial, angles, time, CRS. | **Upstream:** 21–22  **Downstream:** 23, 31, 61, 71 |
| 24 | [Timing](sections/2X_System_Architecture/24_Threading_Scheduling_Timing.md) | Clocks, latency budgets, scheduling. | **Upstream:** 21–22  **Downstream:** 61, 62, 42 |
| **3X – Data Storage** |  |  |  |
| 31 | [Domain Data Model](sections/3X_Data_Storage/31_Domain_Data_Model.md) | Farm → season → job → session hierarchies, field data, provenance. | **Upstream:** 21, 26  **Downstream:** 32, 35 |
| 32 | [Persistence & Formats](sections/3X_Data_Storage/32_Persistence_Formats.md) | File and database formats, serialization, versioning, exports. | **Upstream:** 31  **Downstream:** 33, 34, 35 |
| 33 | [Mapping Storage](sections/3X_Data_Storage/33_Mapping_Storage.md) | How coverage and layers are stored and streamed live (tiles, chunks, CRS alignment). | **Upstream:** 26, 31–32  **Downstream:** 62, 65, 71 |
| 34 | [Equipment Configurations](sections/3X_Data_Storage/34_Equipment_Configurations.md) | How tractors, implements, sections, and IO mappings are saved and versioned. | **Upstream:** 21, 24, 32  **Downstream:** 44, 61–64 |
| 35 | [Backup & Retention](sections/3X_Data_Storage/35_Backup_Retention_Archival.md) | Backup policy, retention windows, archive/export formats. | **Upstream:** 32  **Downstream:** 94 |
| 36 | [User Preferences](sections/3X_Data_Storage/36_User_Preferences.md) | UI settings, layouts, profiles, personalization. | **Upstream:** 13, 32  **Downstream:** 71–73 |
| **4X – Interprocess Communications** |  |  |  |
| 41 | [Message Bus](sections/4X_Interprocess_Communications/41_Message_Bus.md) | Core event and command system for modules and services. | **Upstream:** 21–23  **Downstream:** 42, 43 |
| 42 | [UI Bridge](sections/4X_Interprocess_Communications/42_UI_Bridge.md) | Core ↔ UI messaging and update rules. | **Upstream:** 41  **Downstream:** 71 |
| 43 | [Plugin Bridge](sections/4X_Interprocess_Communications/43_Plugin_Bridge.md) | Extension layer that lets plugins connect to the message bus. | **Upstream:** 41  **Downstream:** 51 |
| 44 | [Service APIs](sections/4X_Interprocess_Communications/44_Service_APIs.md) | External endpoints (gRPC/WebSocket/REST) for remote tools or automation. | **Upstream:** 41  **Downstream:** 75 |
| **5X – Hardware I/O & Device Layer** |  |  |  |
| 51 | [Hardware PGNs](sections/5X_Hardware_IO_Device_Layer/51_Hardware_PGNs.md) | Defines all on-wire message types between devices (AgIO, ECUs, sensors). | **Upstream:** 41  **Downstream:** 52, 54 |
| 52 | [AgIO Core](sections/5X_Hardware_IO_Device_Layer/52_AgIO_Core.md) | Handles drivers, discovery, health, and message routing for attached hardware. | **Upstream:** 41, 51  **Downstream:** 61–64 |
| 53 | [CM5 Controller](sections/5X_Hardware_IO_Device_Layer/53_CM5_Controller.md) | Describes the integrated controller: pin maps, watchdogs, and co-located Core/AgIO behavior. | **Upstream:** 51–52  **Downstream:** 22, 55 |
| 54 | [Firmware & Updates](sections/5X_Hardware_IO_Device_Layer/54_Firmware_Updates.md) | Bootloaders, flashing, DFU/OTA, and version tracking. | **Upstream:** 53  **Downstream:** 94 |
| 55 | [Direct Device Pass-Through](sections/5X_Hardware_IO_Device_Layer/55_Direct_Device_Passthrough.md) | Serial or network links used directly by the core (e.g., GNSS or sensors bypassing AgIO). | **Upstream:** 41  **Downstream:** 61, 63 |
| **6X – Core Domain Services** |  |  |  |
| 61 | [Kinematics](sections/6X_Core_Domain_Services/61_Kinematics.md) | Sensor fusion for position, attitude, and motion. | **Upstream:** 51–52, 26  **Downstream:** 62, 63 |
| 62 | [Section Control](sections/6X_Core_Domain_Services/62_Section_Control.md) | Section logic, latency compensation, and geo triggers. | **Upstream:** 61, 65  **Downstream:** 52 |
| 63 | [Rate Control](sections/6X_Core_Domain_Services/63_Rate_Control.md) | Variable rate computation and control loop tuning. | **Upstream:** 61, 65  **Downstream:** 52 |
| 64 | [Monitoring](sections/6X_Core_Domain_Services/64_Monitoring.md) | Equipment health, alarms, and diagnostics. | **Upstream:** 52, 63  **Downstream:** 71, 96 |
| 65 | [Job Lifecycle](sections/6X_Core_Domain_Services/65_Job_Lifecycle.md) | Session and task state handling, progress, and logs. | **Upstream:** 31, 61–64  **Downstream:** 71, 94 |
| **7X – Mapping & Geospatial** |  |  |  |
| 71 | [Mapping Core](sections/7X_Mapping_Geospatial/71_Mapping_Core.md) | Core map engine: coverage, layers, CRS alignment, and updates. | **Upstream:** 26, 31, 33  **Downstream:** 72 |
| 72 | [Layer Management](sections/7X_Mapping_Geospatial/72_Layer_Management.md) | In-memory layer control, editing, and feature operations. | **Upstream:** 71  **Downstream:** 73 |
| 73 | [Rendering](sections/7X_Mapping_Geospatial/73_Rendering.md) | Tile generation, performance budgets, and drawing rules. | **Upstream:** 71–72  **Downstream:** 74 |
| 74 | [Mapping Plugins](sections/7X_Mapping_Geospatial/74_Mapping_Plugins.md) | Plugin interfaces for custom map sources and geospatial tools. | **Upstream:** 71–73  **Downstream:** 94 |
| **8X – Guidance** |  |  |  |
| 81 | [Guidance Core](sections/8X_Guidance/81_Guidance_Core.md) | Guidance orchestration and control logic. | **Upstream:** 61, 71  **Downstream:** 82 |
| 82 | [Path Planning](sections/8X_Guidance/82_Path_Planning.md) | AB lines, contours, headlands, and boundary management. | **Upstream:** 71, 81  **Downstream:** 83 |
| 83 | [Autosteer Models](sections/8X_Guidance/83_Autosteer_Models.md) | Steering algorithms and tuning parameters. | **Upstream:** 61, 82  **Downstream:** 84 |
| 84 | [Guidance Simulation](sections/8X_Guidance/84_Guidance_Simulation.md) | Offline or replay simulation for testing guidance logic. | **Upstream:** 81–83  **Downstream:** 92 |
| **9X – Frontends & Operations** |  |  |  | 
| 91 | [UI Shell](sections/9X_Frontends_Ops/91_UI_Shell.md) | Main application shell for desktop/tablet, layout, and docking. | **Upstream:** 11–13, 27, 71  **Downstream:** 92 |
| 92 | [Gauges & Panels](sections/9X_Frontends_Ops/92_Gauges_Panels.md) | Machine and implement panels, units, and operator UI patterns. | **Upstream:** 71, 64  **Downstream:** 93 |
| 93 | [Command Line & Automation](sections/9X_Frontends_Ops/93_Command_Line_Automation.md) | CLI tools, scripting, headless control, developer utilities. | **Upstream:** 41, 43  **Downstream:** — |
| 94 | [Remote Clients](sections/9X_Frontends_Ops/94_Remote_Clients.md) | Thin clients, mobile views, and remote dashboards. | **Upstream:** 41, 42, 71  **Downstream:** — |
| 95 | [Simulation & Replay](sections/9X_Frontends_Ops/95_Simulation_Replay.md) | Record/replay, synthetic sensors, deterministic time control. | **Upstream:** 33, 61–63  **Downstream:** — |



---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2025-11-12 | Reorganized and Simplified slices. | Jon Fortney |  |
| 0.1.0 | 2025-11-09 | Document metadata policy applied and appendix added. | Jon Fortney |  |
| 0.1.0 | 2025-10-24 | Initial catalog published. | Nexus Team (Codex) |  |
