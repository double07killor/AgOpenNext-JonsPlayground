---
title: 11 - Operating System Support
version: 0.1.0
status: Draft
authors:
  - Jon Fortney
owner: Systems Engineering & Documentation Lead
reviewers:
  - Systems Engineering Team
approvers:
  - Project Coordinator
created: 2025-10-24
last_reviewed: 2025-11-09
review_cycle: Quarterly
notes: OS coverage requirements; governance metadata aligned to policy.
---

# 11 - Operating System Support
*(Status: Drafting)*

**Section ID:** 11 | **Version:** 0.1.0  
**Editors:** Jon Fortney  
**Last Updated:** 2025-11-12  
**Related Sections:** 12 - Language & Runtime, 14 - Build Tooling  
**Upstream Dependencies:** 2X - System Architecture, 4X - Interprocess Communications  
**Downstream Impacts:** 5X - Hardware I/O Device Layer, 6X - Core Domain Services

## 11.1 Purpose & Scope

Define the operating systems, deployment modes, and hardware consistency goals that AgOpenNext must deliver for the Core and UI stack.

## 11.2 Context

- Windows 10/11 (x64) and Ubuntu LTS (x64/ARM64) remain the Primary platforms defined in the charter (see §3, G2).  
- Secondary stretch targets (Android full-stack + companion, iOS companion-only) exist as drafts in the ADR catalog but carry no release guarantees until a follow-up decision.  
- Hardware communication (Serial, UDP, CAN) flows through AgIO so driver behavior stays consistent across platforms.  
- Build tooling and signing reference the runtime/dependency policies described in Sections 12 and 14, aligned with the charter’s version policy (§5.1).  
- Out of scope: container-only variants or mobile-first shells until reviewed under a new section.

## 11.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Limitation | Modernization Opportunity | Reference |
|--------------|-----------------|------------|---------------------------|-----------|
| Deployment | Windows-only installer. | No Linux parity. | Dual Windows/Linux installers built by shared CI. | Release 2024 |
| Hardware I/O | Win32/CAN binaries. | OS-specific code paths. | AgIO abstraction with consistent Serial/UDP/CAN flows. | AgIO design note |
| UX | WinForms-only. | Platform locked. | Metadata-driven dashboards on new shell. | UI modernization doc |

## 11.4 Definitions

| Term | Definition |
|------|-------------|
| Platform Tier | Classification of supported OS targets (Primary, Preview). |
| Full-Stack Mode | Core and UI run on the same host with direct hardware access. |
| Companion Mode | Remote UI attaching to headless Core over transport.

## 11.5 Requirements

| ID | Priority | Summary | Verification |
|----|----------|---------|--------------|
| R-OS-000 | MUST | Deliver desktop installers for Core + UI on Windows and Ubuntu to enable Full-Stack mode. | Installer runs, UI renders, smoke tests pass per OS. |
| R-OS-001 | MUST | Maintain identical Serial/UDP/CAN behavior via AgIO across supported OS tiers. | AgIO regression suite passes per OS. |
| R-OS-002 | SHOULD | Document parity requirements for secondary tiers before claiming official support. | Parity checklist referenced in release notes. |
| R-OS-003 | SHOULD | Publish signed, reproducible artifacts per tier. | CI signature verification step succeeds. |
| R-OS-004 | SHOULD | Provide hardware and performance guidance per OS. | Guidance documents in onboarding materials.

## 11.6 Acceptance & Verification

- Windows and Linux smoke suites must succeed on every merge candidate.
- Packaging pipelines produce signed installers verified before release.
- Operator guidance documents list the supported tiers and hardware expectations.

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2025-11-12 | Template-aligned rewrite. | Jon Fortney |  |
| 0.1.0 | 2025-11-09 | Added governance metadata. | Jon Fortney |  |
| 0.1.0 | 2025-10-24 | Initial OS-only draft. | Nexus Team (Fortney) |  |
