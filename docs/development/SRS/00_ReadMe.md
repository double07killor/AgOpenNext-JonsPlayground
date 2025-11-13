---
title: AgOpenNext SRS Overview
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
notes: SRS index and orientation; updated per governance metadata policy.
---

# AgOpenNext SRS

*(Status: Drafting)*

**Authors:** Jon Fortney  
**Last Updated:** 2025-11-8

---


Welcome to the Software Requirements Specification (SRS) workspace for the next generation of AgOpenGPS.

## What lives here

- **Software Requirements Specification (SRS)** – The living catalogue of _what_ the AgOpenNext platform must do. It captures scope, constraints, and success criteria before any code is written.
- **Architecture Decision Records (ADRs)** – Lightweight memos that explain _why_ we chose a solution once the community agrees on a direction. Every ADR links back to the requirements and options that shaped the choice.
- **Options, decision matrices, and references** – Supporting material that keeps trade-off discussions grounded in data and traceable from requirement ➜ option ➜ decision.

Think of the SRS as the map and the ADRs as the signposts we install along the route. Reading them together helps newcomers understand the current plan and gives maintainers the context needed to revisit earlier choices.

### Quick start for new readers

1. Skim the [Project charter & vision guardrails](../../PROJECT_CHARTER.md) to understand the product direction and deliberate omissions.
2. Jump to the [System slices](02_System_Slices.md) index and find the area that matches your question (e.g., UI, hardware IO, guidance).
3. Open the corresponding section under [`sections/`](sections/) to see requirements (`R-` IDs), open questions (`Q-` IDs), and option stubs (`O-` IDs).
4. Follow links into ADRs when you need the final decision, implementation guardrails, or rollout notes.

Returning contributors can skip straight to sections flagged "Under review" or "Ready for ADR" to catch the latest work-in-progress discussions.



## How this SRS is organized
- **Project charter & vision guardrails** capture what the next release aspires to solve and what is intentionally out-of-scope.
- **System slices** map every focus area (OS, UI, comms, storage, etc.) to an individual
  section document under [`sections/`](sections/).
- **Sections** collect requirements and enumerate options. They are intentionally decision-neutral—decisions live in Architecture Decision Records (ADRs).
- **Options** can be expanded in dedicated files using the
  [`docs/templates/OPTION.md`](../templates/OPTION.md) template when deeper analysis is
  needed.
- **References** house canonical specs (e.g., PGN catalogs) that new options must remain compatible with unless an ADR says otherwise.
- **ADRs** document finalized decisions. Each ADR references the section(s) and options involved so we preserve traceability.

## Document index

### Core overview
- [Project charter & vision guardrails](01_Project_Charter.md)
- [System slices](02_System_Slices.md)
- [Working notes](NOTES.md)

### Section catalog
- [11 — OS Support](sections/1X_Platform_Foundations/11_OS_Support.md)
- [12 — Language & Runtime](sections/1X_Platform_Foundations/12_Development_Language_Runtime.md)
- [13 — UI Framework & UX](sections/1X_Platform_Foundations/13_UI_Framework_UX.md)
- [14 — Build & Tooling](sections/1X_Platform_Foundations/14_Build_Tooling.md)
- [21 — Decomposition](sections/2X_System_Architecture/21_System_Decomposition_Boundaries.md)

### Active ADRs
- [11-ADR-001 — Target OS Prioritization](sections/1X_Platform_Foundations/11-ADR-001 - Target OS Prioritization.md)
- [12-ADR-001 — Adopt .NET 10 Runtime](sections/1X_Platform_Foundations/12-ADR-001 - Adopt .NET 10 Runtime.md)
- [13-ADR-001 — Adopt Avalonia 12 for the Nexus Desktop UI Shell](sections/1X_Platform_Foundations/13-ADR-001 - Adopt Avalonia 12 for the Nexus Desktop UI Shell.md)
- [14-ADR-001 — Standardize Build Environment & Tooling](sections/1X_Platform_Foundations/14-ADR-001 - Standardize Build Environment & Tooling.md)
- [11-ADR-002 — Android Full Stack + Companion Candidate](sections/1X_Platform_Foundations/11-ADR-002 - Android Support Candidate.md)
- [11-ADR-003 — iOS Companion Candidate](sections/1X_Platform_Foundations/11-ADR-003 - iOS Companion Candidate.md)

### Reference library
- [Reference library overview](references/README.md)
- [Core reference catalog](references/core/README.md)
- [Guidance reference catalog](references/guidance/README.md)
- [Mapping reference catalog](references/mapping/README.md)

### Appendices
- [DFU catalog schema](appendices/DFU_Catalog_Schema.md)
- [Gauge ID registry](appendices/GaugeId_Registry.md)
- **Schema definitions**
  - [Plugin catalog schema](appendices/schemas/plugin_catalog.schema.json)
  - [Plugin manifest schema](appendices/schemas/plugin_manifest.schema.json)
- **Sample manifests & datasets**
  - [Equipment manifest sample – articulated tractor](appendices/samples/examples/articulated-tractor.v1.json)
  - [Scenario library index](appendices/samples/scenarios/library.json)
  - [Scenario performance matrix](appendices/samples/scenarios/performance-matrix.json)
  - [Legacy auto-run manifest](appendices/samples/scenarios/legacy-auto-run/legacy-auto-run.json)
  - [Legacy auto-run soak report sample](appendices/samples/scenarios/legacy-auto-run/verification/soak-report.sample.json)
  - Plugin manifest samples
    - [Autosteer](appendices/samples/plugins/autosteer/1.0.0.json)
    - [Autosteer Lite](appendices/samples/plugins/autosteer-lite/1.2.0.json)
    - [Device manager](appendices/samples/plugins/device-manager/1.0.0.json)
    - [File IO](appendices/samples/plugins/file-io/1.0.0.json)
    - [GNSS + IMU fusion](appendices/samples/plugins/gnss-imu-fusion/1.0.0.json)
    - [ISOBUS bridge](appendices/samples/plugins/isobus-bridge/1.0.0.json)
    - [Job tasks](appendices/samples/plugins/job-tasks/1.0.0.json)
    - [NTRIP client](appendices/samples/plugins/ntrip-client/1.0.0.json)
    - [Planter monitor](appendices/samples/plugins/planter-monitor/1.0.0.json)
    - [Replay](appendices/samples/plugins/replay/1.0.0.json)
    - [Sections](appendices/samples/plugins/sections/1.1.0.json)
    - [Telemetry logging](appendices/samples/plugins/telemetry-logging/1.0.0.json)

### References
- [AgIO PGN baseline](references/AgIO_PGN_Baseline.md)
- [AgOpenGPS hardware platforms](references/AgOpenGPS_Hardware_Platforms.md)
- [ISOBUS section control](references/ISOBUS_Section_Control.md)

### ADR index
- [12-ADR-001 — .NET 10 runtime](sections/1X_Platform_Foundations/12-ADR-001 - Adopt .NET 10 Runtime.md)


## Workflow expectations
1. Start discussion in the matching GitHub Discussion for the section.
2. Open PRs to add or refine requirements (R-IDs) and options (O-IDs).
3. Maintainers review for clarity and formatting; contributors stay neutral until an ADR is published.
4. Cluster options into **decision families** (exclusive vs. composable) inside each section before deep evaluation. Document any sequencing (e.g., "pick OS target before UI skin").
5. When a family needs structured comparison, spin up a decision-matrix doc, capture scoring data, and link it from the section. This keeps the section readable while preserving analysis artifacts.
6. Once the community agrees, capture the outcome in an ADR that links back to the relevant section table.

## Status transitions
### Section status flow
- **Collecting proposals**: The default state. Entry criteria: a problem statement exists and at least one requirement is documented. Exit criteria: requirements cover baseline success metrics (e.g., hardware, latency, safety) and open questions are narrowed to decision-ready prompts.
- **Under review**: Maintainers believe the requirement set is complete enough to evaluate options. Exit criteria: decision matrix (if needed) linked, traceability matrix row completed, and blocking dependencies (see system slice index) addressed.
- **Ready for ADR**: Consensus has formed around a preferred option family and draft acceptance criteria exist. Exit criteria: ADR author identified and rollout/validation requirements captured.
- **Decided**: ADR merged. Ongoing tweaks require explicit ADR updates or follow-on requirements.

### Option status flow
- **Draft**: Option is being fleshed out; dependencies and readiness gates may be incomplete.
- **Under comparison**: Option participates in a decision matrix or structured evaluation; entry requires dependency prerequisites to be listed.
- **Candidate decision**: Option is the favored approach pending ADR write-up and validation/rollout checklists.
- **Retired**: Option remains in history but is no longer recommended; note the superseding ADR.

## Conventions
- **IDs**: `R-` for requirements, `O-` for options, `Q-` for open questions, and `ADR-` for decisions.
- **Status labels** in headings track whether a section is collecting proposals, under review, or decided.
- **Borrowables** highlight concrete code or assets from AgOpenGPS/AgIO or other projects that we can reuse.
- **Linting ideas**: unique IDs, table formatting, and link validation can be automated in CI.

## Traceability matrix
_This matrix links every requirement (R-) to the options, references, and eventual ADR homes that will satisfy it. “TBD” ADRs signal where future decisions will land once validation gates are met._



---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2025-11-09 | Added governance metadata requirements and appended this change log table. | Jon Fortney |  |
| 0.1.0 | 2025-11-08 | Converted to the AgOpenNext SRS workspace and described status/roles. | Jon Fortney |  |
| 0.1.0 | 2025-10-24 | Initial SRS overview seeded. | Nexus Team (Fortney) |  |
