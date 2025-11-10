---
title: ADR 12-001 — Adopt .NET 10 Runtime Across AgOpenNext
version: 0.1.0
status: Draft
authors:
  - Jon Fortney
owner: Architecture Working Group
reviewers:
  - Architecture Working Group
  - Release & Tooling Working Group
approvers:
  - Project Coordinator
created: 2025-10-20
last_reviewed: 2027-03-14
review_cycle: Annual
notes: Runtime selection decision; metadata per governance policy.
---

# 12-ADR-001 — Adopt .NET 10 Runtime Across AgOpenNext
*(Status: Draft — 2027-03-14)*

**Section ID:** 12 | **Version:** 0.1.0  
**Related SRS:** `12_Development_Language_Runtime.md`  
**Related Options:** `12-O1_Runtime_Policy.md`  
**Upstream Dependencies:** 11 — Operating System Support  
**Downstream Impacts:** 13 — UI Framework & UX, 14 — Build Environment & Tooling

## 1) Context

Section 12 defines the runtime and language policies governing all AgOpenNext components.  
Legacy AgOpenGPS releases mixed **.NET Framework**, **.NET 6**, and native utilities,
resulting in divergent build pipelines, inconsistent Linux support, and plugin
incompatibilities.

Sections 11 and 14 introduce new requirements for reproducible cross-platform builds
and deterministic tooling. To satisfy these, all managed components — including Core,
UI, AgIO, and CLI — must share a single supported runtime.

The .NET 10 release provides the required cross-platform JIT, SDK, and CI tooling for
both Windows and Linux targets. It also enables Avalonia and NativeAOT development
without fragmenting the stack.

---

## 2) Decision

Adopt **.NET 10** as the managed runtime for all AgOpenNext managed components until a
future migration ADR supersedes this one.

- All projects **must target** `net10.0` (or `net10.0-windows`, `net10.0-linux`, etc. as
  needed for platform-specific assets).
- CI and local environments **must use** the .NET 10 SDK pinned via `global.json`.
- NativeAOT or trimming experiments **may** proceed if outputs remain compatible with
  .NET 10 tooling and pass §11 smoke tests.
- Runtime upgrades follow Microsoft’s stable release cadence; evaluate previews but do not adopt
  until a successor ADR is approved.

---

## 3) Consequences

### Positive Impacts
- Unifies runtime and eliminates build drift between Windows and Linux.  
- Enables Avalonia, ASP.NET Core, and shared analyzers under one toolchain.
- Provides plugin developers a stable baseline with long-term vendor support.

### Negative / Mitigated Impacts
- Requires migration of legacy .NET Framework projects. *Mitigation:* adapter shims and
  migration guides.  
- Slightly higher hardware/runtime requirements. *Mitigation:* document minimum specs in §11.  
- Future runtime transitions require planning. *Mitigation:* annual review and evergreen upgrade backlog.

### Follow-Up Actions
- Audit all solutions for `TargetFramework` entries =`net10.0`.
- Update dependency allowlists and analyzers for .NET 10 compatibility.
- Maintain temporary downgrade bundles until .NET 10 is verified on both OSes.

---

## 4) Rationale

.NET 10 delivers the cross-platform stability, runtime features, and toolchain
maturity needed for AgOpenNext. Remaining on .NET Framework or adopting unstable runtimes
would violate §11 and §12 requirements for OS parity, determinism, and maintainability.

---

## 5) Alternatives Considered

| Option | Summary | Outcome |
|---------|----------|----------|
| Windows-only .NET Framework | Continue shipping legacy runtime for Windows builds only. | **Rejected** — fails cross-platform and modern tooling requirements. |
| Adopt .NET 9 or preview builds | Use short-term or unstable runtimes. | **Rejected** — unsupported runtimes and upgrade churn increase maintenance cost. |
| Move to native C++/Qt stack | Rewrite outside the .NET ecosystem. | **Rejected** — prohibitively expensive and breaks existing code and plugins. |

---

## 6) Governance

- **Ownership:** Platform Foundations Working Group.  
- **Review Cadence:** Annual or when Microsoft announces a new runtime release.  
- **Artifacts:** `global.json`, runtime compliance dashboard, analyzer configurations.  
- **Exit Criteria:** Superseded by a future ADR that adopts the next supported runtime.

---

## 7) Risks & Mitigations

| ID | Risk | Mitigation |
|----|------|-------------|
| **ADR12-R1** | Legacy dependencies incompatible with .NET 10. | Maintain compatibility backlog and provide shim packages. |
| **ADR12-R2** | Runtime upgrades introduce regressions. | Maintain regression suites and stage upgrades in preview branches. |
| **ADR12-R3** | Contributors lack .NET 10 tooling. | Provide bootstrap scripts and enforce toolchain setup via §14 build policy. |

---

## Appendix: Change Log

| Version | Date | Changes | Author | PR / Issue |
|---------|------|---------|--------|------------|
| 0.1.0 | 2025-11-02 | Elevated runtime baseline to .NET 10 and refreshed governance. | Nexus Team (Fortney) |  |
| 0.1.0 | 2025-10-25 | Clarified NativeAOT scope and dependency governance linkage. | Nexus Team (Fortney) |  |
| 0.1.0 | 2025-10-20 | Initial adoption of the .NET 8 runtime. | Nexus Team (Fortney) |  |



