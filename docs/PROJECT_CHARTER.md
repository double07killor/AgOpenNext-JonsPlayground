---
title: AgOpenNext Project Charter
version: 0.6.2
status: Draft for Community Review
authors:
  - Jon Fortney
  - Markus Nuuja
owner: Systems Engineering & Documentation Lead
reviewers:
  - TBD
approvers:
  - TBD
created: 2025-10-20
last_reviewed: 2025-11-09
review_cycle: Ad-hoc when scope or assumptions shift materially
notes: Markdown copy is canonical; add the Google Docs link when a mirrored version exists.
---

# AgOpenNext Project Charter

Metadata for this charter lives in the YAML front-matter block above; keep those values in sync with the ownership,
review, and lifecycle expectations outlined in the governance policy.

The companion [roadmap](./ROADMAP.md) documents how the charter's goals translate into high-level phases and sequencing
toward GA so decision-makers can track progress without duplicating governance commitments.

## 1. Executive Summary, Mission & Vision

AgOpenNext is a ground-up rebuild of AgOpenGPS, engineered for long-term stability, maintainability, and true
cross-platform operation.  
Its purpose isn’t to add features, but to rebuild the foundation—keeping today’s workflows stable while enabling a
decade of open, sustainable innovation.

**Mission:**  
Build a modern, modular guidance platform that preserves v6 reliability, eliminates technical debt, and enables open,
frictionless contributions.

**Vision:**  
A community-driven precision agriculture platform that:

- Runs consistently across all platforms.
- Preserves v6 performance through automated and real-world validation.
- Encourages experimentation through clear architecture and documented interfaces.
- Grows an open ecosystem where innovation compounds instead of fragmenting.
- Supports current hardware while remaining adaptable for what comes next.

Core principle: **Rewrite the foundation. Preserve the functionality. Unlock the future.**

## 2. Guiding Principles & Guardrails

- **Foundation first:** Modernize runtime, architecture, and tooling before expanding feature scope.
- **Parity with intent:** Document all deviations from v6 behavior in ADRs, including validation and mitigation plans.
- **Modular and extensible:** Maintain clear architectural seams that allow extending the functionality flexibly.
- **Inclusive contribution model:** Keep workflows open and reproducible across Windows and Linux, avoiding proprietary
  dependencies.
- **Deterministic validation:** Use automated, replayable tests to detect regressions before they reach the field.
- **Future-proof design:** Architecture anticipates future extensions without forcing premature implementation.

## 3. Goals & Success Criteria
| Goal                            | Success Criteria                                                                                                                                                                                      | Priority    |
|---------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------|
| **G1 — Functional Parity**      | Pass all critical v6 field operation test suites; preserve essential guidance accuracy, GNSS processing, autosteer behavior. Any intentional retirements documented in ADRs with operator approval.   | P0-Critical |
| **G2 — Cross-Platform Support** | Unified codebase for Windows, Linux, and ARM64 builds with identical behavior. Non-gating stretch targets: Android (P2) and iOS Companion (P3). All builds must install cleanly and pass smoke tests. | P0-Critical |
| **G3 — Modern UI**              | Avalonia UI achieves ≥30 FPS on reference hardware; functionally complete WinForms replacement.                                                                                                       | P0-Critical |
| **G4 — Test Infrastructure**    | Unit tests ≥80 % coverage; integration tests 100 % on critical paths; all pass in CI before merge.                                                                                                    | P0-Critical |
| **G5 — Architectural Clarity**  | Clean separation: business logic, hardware abstraction, presentation; documented APIs.                                                                                                                | P0-Critical |
| **G6 — Developer Experience**   | New contributor setup <1 hour on Windows or Linux; contribution checklists align with CI expectations.                                                                                                | P1-High     |
| **G7 — Community Validation**   | Field testing by ≥20 operators across ≥3 continents prior to general availability.                                                                                                                    | P1-High     |
---

## 4. Foundation Phase Boundaries (Non-Goals/Out of Scope for v1.0)

The foundation phase prioritizes modernization and stability.  
These areas are not part of the official GA scope but remain supported by architecture for future implementation.

- **New implement types / guidance algorithms:** Deferred (e.g., Stanley, MPC).
- **Machine learning / AI:** Out of scope for v1.0.
- **Cloud or data sync:** No built-in support; extension points only.
- **Mobile clients:** Android P2; iOS companion P3.
- **VR/AR or fleet tools:** Deferred until post-validation.
- **CAN bus full implementation:** Architecture supports it; full feature set deferred post-GA.
- **Direct USB serial connections for control modules:** May be deprecated in favor of UDP-based communication. Serial
  support for GNSS receivers remains in scope.
- **Data migration:** Limited support for v5 and earlier via published tooling only.

**Rationale:** Focused scope ensures a stable, modern core while keeping paths open for future expansion.

## 5. Scope

### 5.1 Version Policy

AgOpenNext targets the most recent stable combination of **.NET** and **Avalonia** verified to work together.

- Develop on the latest **.NET** version fully supported by the current **Avalonia** release.
- Upgrade only after CI passes and compatibility with existing modules is confirmed.
- If Avalonia does not support the current .NET version, use the most recent compatible .NET version until support is
  available.

### 5.2 In Scope

**Platform Foundations:**

- Unified .NET runtime (latest LTS or stable) and Avalonia UI
- CI/CD pipelines for Windows and Linux: installers, packages, containers, and systemd units (headless supported by
  design).
- Modern graphics rendering via Avalonia (OpenGL/Vulkan).

**Functionality (v6 Parity):**
- AgIO Hardware Abstraction:
    - Serial and UDP IO for hardware communication
    - GNSS integration (NMEA parsing, NTRIP client)
    - PGN-based communication with hardware modules
- Field management, boundary management, and data recording
- Autosteer guidance (AB lines, curves, contours, pivot, U turns)
- Implement control (sections, rate, tramlines) and vehicle calibration

**Architecture & Communication:**
- Clear separation of concerns across business logic, hardware, and UI layers.
- Well-defined interfaces supporting parallel development, comprehensive testing, and maintainability.
- Loosely-coupled components enabling simulation, validation, and future extensibility.

**User Interface:**

- Full Avalonia-based replacement for WinForms including configuration dialogs and calibration wizards.
- Real-time field display (≥30 FPS target) with settings persistence and accessibility compliance.

## 6. Stakeholders & Governance

AgOpenNext is a volunteer-driven project built on shared ownership, transparency, and objective decision-making.  
Roles describe areas of responsibility, not rank, and will continue to evolve as the project matures.

Contributor roles and current maintainers are listed in the [Meet the Team](./team/README.md) directory.  
Full governance details, decision processes, and approval rules are defined in:

- [Governance Framework](./governance/GOVERNANCE.md)
- [Decision Levels and Approval Rules](./governance/DECISIONS.md)

---

### Governance Principles

- **Transparency:** All design and architectural decisions are documented through ADRs, RFCs, or SRS updates.
- **Objectivity:** Decisions rely on evidence, testing, and documented rationale — not emotion or popularity.
- **Accountability:** Each change references an ADR, Issue, or PR discussion for traceability.
- **Efficiency:** Governance exists to keep development organized, not to slow it down. Paperwork ends where progress
  begins.
- **Communication:** GitHub and Telegram serve as primary collaboration channels; synchronous meetings are rare and
  optional.
- **Adaptability:** Roles and processes can evolve through the amendment process as the project grows.

---

This structure keeps governance lightweight but rigorous — emphasizing documentation, accountability, and engineering
objectivity over hierarchy.  
For the full governance model and amendment procedures, refer to the linked documents above.

## 7. Key Deliverables

Deliverables follow a foundation-first progression: contracts → logic → infrastructure → presentation → delivery →
quality → documentation.

| ID     | Deliverable                      | Description                                                                                             | Acceptance Criteria                                                     |
|--------|----------------------------------|---------------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------|
| **D1** | Interface Specifications              | Documented interface contracts and APIs for all major components (business logic, hardware, UI bridge). | All interfaces documented; ADR approval; enables parallel development.  |
| **D2** | Guidance & Field Management      | Cross-platform business logic (.NET) for guidance algorithms, field management, and implement control.  | Unit tests ≥80%; integration with D1 interfaces validated.              |
| **D3** | AgIO Hardware Abstraction Layer  | Hardware interface layer for Windows, Linux, and ARM64 handling serial, UDP, and sensors.               | All v6 hardware operational; cross-platform service validated.          |
| **D4** | Avalonia UI                      | Cross-platform desktop interface replacing WinForms with full configuration and field display.          | Functional v6 parity; ≥30 FPS on reference hardware.                    |
| **D5** | Packaging & Distribution         | Platform-specific packages: Windows installers (P0), Linux packages (P1), Android APK (P2), iOS (P3).   | Clean install and smoke tests pass on all P0/P1 platforms.              |
| **D6** | Test Suites                      | Automated unit, integration, and field scenario replay tests with CI/CD integration.                    | 100% of critical v6 field scenarios pass; ≥80% code coverage.           |
| **D7** | Documentation                    | Architecture docs, API references, operator guides, and contributor onboarding materials.               | Published and community reviewed; supports <1hr contributor setup.      |
| **D8** | Migration Toolkit           | Automated tools for v6 → Next data migration with validation and rollback support.                      | User settings, fields, boundaries, and vehicle configs migrate cleanly. |

---

## 8. Risks & Mitigations

Risks sorted by priority (Impact × Likelihood):
| ID  | Risk                                            | Likelihood | Impact   | Mitigation / Contingency                                                                                        |
|-----|-------------------------------------------------|------------|----------|-----------------------------------------------------------------------------------------------------------------|
| R1  | Volunteer time fluctuates.                      | High       | Critical | Keep scope small; rotate ownership; acknowledge contributions.                                                  |
| R2  | Key contributor departure or burnout.           | Medium     | Critical | Document tribal knowledge; cross-train on critical areas; distribute architectural knowledge; "bus factor" > 2. |
| R3  | Project timeline extends causing momentum loss. | High       | High     | Set realistic milestones; celebrate incremental progress; maintain visible roadmap; accept delays openly.       |
| R4  | Feature creep diverts focus.                    | High       | High     | Track extras as plugin or post-GA ideas; enforce ADR boundaries.                                                |
| R5  | AI-assisted code reduces maintainability.       | Medium     | High     | Mandatory code review; require documentation; enforce architectural patterns; pair AI with expertise.           |
| R6  | Community fragmentation or competing fork.      | Medium     | High     | Transparent ADR process; consensus-seeking governance; address conflicts early; acknowledge valid concerns.     |
| R7  | Breaking changes alienate v6 user base.         | Medium     | High     | Extensive field testing (G7); migration guides; maintain compatibility layer where feasible; phased rollout.    |
| R8  | GNSS/device behavior differs by OS.             | Medium     | High     | Standardize on open formats; expand hardware abstraction; document quirks; cross-platform testing.              |
| R9  | Dependency vulnerabilities or abandonment.      | Medium     | Medium   | Monitor dependency health; maintain abstraction layers; evaluate alternatives; contribute upstream.             |
| R10 | Test coverage doesn't catch field edge cases.   | Medium     | Medium   | Diverse field testing (G7); replay captured data; involve operators in test design; maintain beta program.      |
---

## 9. Migration & Transition

- Automated tools migrate v6 data (fields, vehicles, settings).
- Real datasets validated pre-GA with rollback support.
- Clear guides and community help for operators.
- **No forced migration:** v6 remains supported.

---

## 10. Success Measures

- **Functional parity:** 100 % of critical v6 features working in real field use.
- **Cross-platform reliability:** Windows, Linux, and ARM64 builds stable and validated.
- **Field validation:** ≥20 operators across ≥3 continents.
- **Build quality:** ≥95 % CI pass rate; <5 critical bugs at RC.
- **Adoption:** Operators migrate from v6 voluntarily due to stability and usability gains.
- **Governance maturity:** All key decisions traceable through ADRs or documented rationale.

---

## 11. Post-Foundation Outlook

Post-v1.0: plugin system, CAN bus integration, advanced guidance algorithms, cloud sync, and broader ecosystem
support.  
All future features remain ADR-driven and community-reviewed — **foundation first** always.

---

## 12. Charter Governance & Revision

### Approval

This charter represents a shared understanding of AgOpenNext’s scope and principles — it is **not** a legal contract.  
Approval requires open community review with no major objections on GitHub or Telegram.  
Core contributors acknowledge that work should remain within the charter’s defined scope and values.  
The Project Coordinator declares the charter “accepted” based on observed community consensus, consistent with
the [Governance Framework](./governance/GOVERNANCE.md).

### Governance & Values

Contributors are expected to uphold:

- **Foundation first:** Modernization and stability before expansion.
- **Transparency:** All major decisions are documented and reviewable.
- **Objectivity:** Engineering choices grounded in data and testing, not popularity.
- **Quality:** Field-tested, operator-validated releases.
- **Sustainability:** Designs and processes built for long-term maintainability.
- **Efficiency:** Governance and documentation serve progress, not bureaucracy.

### Revision Process

Revisions follow the [Governance Amendment Process](./governance/GOVERNANCE.md#7-amendment-process).

- **Minor updates** (clarifications, typo fixes, or formatting): may be committed directly with a short changelog note
  and version increment.
- **Major updates** (scope, governance, or directional changes): proposed via GitHub Discussion or PR, open for a
  one-week comment period before approval by the Project Coordinator and at least one Systems Engineer or Maintainer who
  is not the author.
- Version numbers increment with each accepted major change.
- All previous versions remain archived in-repo for transparency and traceability.

---

Further execution details—timelines, QA, and resource planning—are maintained in separate project documents within
the [development](./development) folder.

## 13. Appendices

### A. Glossary

For definitions of technical terms (ADR, SRS, HAL, etc.), see the maintained [Glossary](./GLOSSARY.md) document in the
repository.

### B. References

- **AgOpenGPS Repository:** [github.com/AgOpenGPS-Official/AgOpenGPS](https://github.com/AgOpenGPS-Official/AgOpenGPS)
- **Documentation:** [docs.agopengps.com](https://docs.agopengps.com)
- **Avalonia UI:** [avaloniaui.net](https://avaloniaui.net)
- **.NET Documentation:** [learn.microsoft.com/dotnet](https://learn.microsoft.com/dotnet)
- **Community Forum:** [discourse.agopengps.com](https://discourse.agopengps.com)

### C. Change Log

| Version | Date       | Changes                                                                                                                                                                                                              | Author               | PR / Issue |
|---------|------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------------------|------------|
| 0.6.2   | 2025-11-09 | Restructured deliverables (D1=Interface Specs, foundation-first order), enhanced risk assessment (10 prioritized risks including AI maintainability, contributor burnout, timeline delays), clarified LTS policy, added ARM64 to P0 cross-platform support, removed redundant "core" terminology, improved architectural scope clarity, fixed terminology consistency. | Markus               |            |
| 0.6.1   | 2025-11-09 | Fix formatting in PROJECT_CHARTER.md  | Markus               |            |
| 0.6.0   | 2025-11-08 | Back to Markdown for Github, even more compact at 246 lines, cut out redundant fat                                                                                                                                   | Jon Fortney          |            |
| 0.5.0   | 2025-11-06 | Compact Edition: Condensed from 673 to 370 lines (~45% reduction) for easier reading and sharing; preserved all essential sections and critical information; streamlined tables and explanations; removed redundancy | Markus               |            |
| 0.4.0   | 2025-10-24 | Major rewrite for clarity and realism: simplified governance, reframed risks, modernized mission and vision to reflect community-led development.                                                                    | Nexus Team (Fortney) |            |
| 0.3.2   | 2025-10-23 | Streamlined charter to emphasize mission, guardrails, goals, and scope; removed process-specific execution details.                                                                                                  | Nexus Team (Codex)   |            |
| 0.3.1   | 2025-10-22 | Consolidated charter with vision guardrails and baseline assumptions.                                                                                                                                                | Nexus Team (Codex)   |            |
| 0.3.0   | 2025-10-22 | Expanded goals, scope, and governance based on Next charter lessons learned.                                                                                                                                         | Nexus Team (Fortney) |            |
| 0.2.0   | 2025-10-21 | Community review update incorporating steering feedback.                                                                                                                                                             | Next Team (Markus)   |            |
| 0.1.0   | 2025-10-20 | Initial draft aligning with SRS foundations.                                                                                                                                                                         | Nexus Team (Codex)   |            |

### E. Ongoing Discussions

These topics remain under active discussion within the community and may inform future charter updates or ADRs.  
They represent naming, platform, and structural decisions that are not yet finalized but influence long-term direction.

| Topic                             | Summary                                                                                                                                                                                                                                               | Current Status                                                                                             |
|-----------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------------|
| **Project Codename**              | Whether to retain **AgOpenNext** or adopt an alternate such as *AgOpenGPS Next*, *AgNext*, or *AgOpenGPS Nexus*. Final name will serve as the bridge between the v6 lineage and future v7 release.                                                    | AgOpenNext favored; open for feedback.                                                                     |
| **First GA Release Name**         | Determining what to call the first general-availability release: continue using the codename (*AgOpenNext v1.0*) or formally resume the legacy naming as **AgOpenGPS v7.0** to maintain continuity with previous versions.                            | Community leaning toward **AgOpenGPS v7.0** to signal a direct, modern continuation of the AgOpenGPS line. |
| **Domain Terminology**            | What to call individual core domains—built-in and community. “Modules” currently refers to hardware PCBs; “plugins” suits community extensions; “blocks” is also used informally. Architectural boundaries are defined, but naming remains undecided. | Decision pending; may standardize through early ADRs.                                                      |
| **Runtime Version Policy**        | Defining the target policy for **.NET** and **Avalonia** versions (latest stable vs LTS). Current practice is “latest stable combination verified by CI.”                                                                                             | Ongoing; documented in §5.1.                                                                               |
| **Headless / Remote UI Strategy** | How to handle headless operation and remote UIs. Android is popular as a first-class runtime; Android/iOS companions may leverage a UI-bridge interface for lightweight remote control.                                                               | Architectural support in place; implementation priority TBD.                                               |
| **AgIO Naming / Scope**           | Decide whether the hardware abstraction layer should retain the historical name **AgIO**, be renamed to a more generic “Hardware Abstraction,” or formally adopt **AgIO Hardware Abstraction**. Impacts docs, code namespace, and messaging to new contributors. | Leaning toward keeping **AgIO** for continuity; scope clarification via early ADR. |
| **Serial vs UDP for Control Modules** | Whether to continue supporting **direct USB serial** for control modules or move exclusively to **UDP-based** comms. Serial is convenient for bench testing and legacy installs; UDP simplifies architecture and multi-module setups. GNSS serial remains in scope. | Tentative compromise: keep serial but mark as **legacy / not recommended** for field use. |

*These items are tracked for transparency and may be formalized in future charter revisions or ADRs as consensus
emerges.*

*End of document.*
