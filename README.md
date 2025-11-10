# AgOpenNext

> The next-generation, cross-platform rewrite of AgOpenGPS — built for long-term stability, maintainability, and full hardware compatibility.  
> **Status:** Foundation phase • Pre-alpha • Not expected to reach general availability until **2027**

AgOpenNext is a community-led rebuild of AgOpenGPS designed to run on Windows, Linux, and beyond — while staying **compatible with nearly all existing AgOpenGPS hardware** (AIOs, Teensy modules, UDP/serial boards, CAN devices, etc.).  

The goal isn’t to reinvent guidance — it’s to rebuild the foundation so that the same hardware you already own keeps working for another decade, on a cleaner, modern, and open architecture.

[![License: GPLv3](https://img.shields.io/badge/License-GPLv3-blue.svg)](#license)
[![Platforms](https://img.shields.io/badge/Platforms-Windows%20%7C%20Linux-informational)](#goals--roadmap)
[![UI](https://img.shields.io/badge/UI-Avalonia-informational)](#scope--what-this-project-is)
[![.NET](https://img.shields.io/badge/.NET-Latest%20stable%20combo%20with%20Avalonia-informational)](#version-policy)

---

## What is this?
**AgOpenNext** is a ground-up rebuild of AgOpenGPS focused on long-term stability, maintainability, and true cross-platform behavior.  
Goal: **rewrite the foundation, preserve today’s workflows, unlock the next decade.**

- Modern runtime (.NET + Avalonia)  
- Clear architectural seams

If you want features-first, this isn’t that. If you want a stable base the community can grow on, you’re in the right place.

---

## Status (read this)
- **Pre-alpha.** Architecture, docs, and scaffolding come first.  
- **Field use:** Not supported yet.  
- **Contributions:** Yes—docs, scaffolding, test harnesses, and ADRs especially.

> The meme version: “It’ll be done when it’s done.” The serious version: we’re building it right.

---

## Scope — what this project *is*
- Cross-platform Core + UI replacing WinForms with **Avalonia**  
- Guidance, autosteer, mapping, field/boundary management **at parity with v6**  
- AgIO hardware abstraction (Serial/UDP/CAN) with clear contracts  
- Deterministic simulation, logging, and replay  
- Migration tools for v6 → Next

### Non-goals (for v1.0)
ML/AI, cloud sync, VR/AR, full CAN feature set, new guidance types beyond v6 parity, mobile clients as first-class—**all deferred**. Architecture leaves doors open; we’re not walking through them yet.

---

## Version Policy
AgOpenNext tracks the **latest stable combination of .NET and Avalonia** verified by CI.  
If Avalonia lags behind a new .NET release, we pin to the last compatible version until parity returns.

We’re committed to **keeping pace with modern tooling** — no more getting stuck on outdated frameworks or broken dependencies.  
Each upgrade is validated in CI and documented through ADRs to ensure stability and forward momentum.

---

## Repo Layout (subject to change)

- **[docs/](./docs)** – Charter, governance, SRS, and contributor-facing docs  
  - [README.md](./docs/README.md) – Documentation landing page  
  - [ROADMAP.md](./docs/ROADMAP.md) – High-level sequencing toward GA  
  - [PROJECT_CHARTER.md](./docs/PROJECT_CHARTER.md) – Authoritative scope, goals, and guardrails  
  - [DOC_TEMPLATE.md](./docs/DOC_TEMPLATE.md) – Reusable structure for new docs  
  - [DEVELOPER_GUIDE.md](./docs/DEVELOPER_GUIDE.md) – Contributor expectations  
  - [GLOSSARY.md](./docs/GLOSSARY.md) – Canonical terminology  
  - **[team/](./docs/team)** — Team bios and responsibilities  
    - [README.md](./docs/team/README.md)  
    - [TEMPLATE.md](./docs/team/TEMPLATE.md)  
  - **[governance/](./docs/governance)** — Governance and decision policy  
    - [GOVERNANCE.md](./docs/governance/GOVERNANCE.md)  
    - [DECISIONS.md](./docs/governance/DECISIONS.md)  
  - **[development/](./docs/development)** — Engineering standards and workflow  
    - [CODE_STYLE.md](./docs/development/CODE_STYLE.md)  
    - **[SRS/](./docs/development/SRS)** — System Requirements & ADR trail  
    - (future files such as CONTRIBUTING.md, REVIEW_GUIDE.md, TESTING.md, BUILDING.md, RELEASES.md)  
  - *(coming soon: process, operator, and user guides defined here)*  
- **[src/](./src)** — Planned home for Core, AgIO, UI, and plugin code (directory to land)  
- **[tests/](./tests)** — Placeholder for unit, integration, and replay harnesses  
- **[tools/](./tools)** — Scripts, packaging helpers, and CI utilities (future)  
- **[LICENSE](./LICENSE)** — GPLv3 licensing terms  
- **[README.md](./README.md)** — Top-level overview  

> Paths may differ initially — adjust as folders land.



---

## Current Focus
This repository is in the foundation/docs phase.  
No runnable code yet — we’re establishing architecture, documentation, and contributor standards first.

Key active areas:
- [Project Charter](./docs/PROJECT_CHARTER.md)
- [System Requirements & ADR trail](./docs/development/SRS/)
- [Contributor documentation](./docs/README.md)

---

## Roadmap

### Foundation (P0)
- v6 functional parity where it matters in the field.
- Windows + Linux with identical behavior.
- Avalonia UI target: ≥30 FPS on reference hardware.
- CI with unit, integration, and replay coverage.
- Clean separation of business logic / HAL / presentation.

### Pre-GA (P1)
- Packaging matrix (Windows + Linux).
- Migration tooling and operator docs.
- Community field validation across geographies.

### Post-v1
- Plugin system, deeper CAN integration, advanced guidance algorithms, remote/companion UIs.
- All future scope via ADRs, not vibes.

---

## Contributing
Pull requests are welcome for:
- Documentation (SRS, ADRs, contributor guides)
- Build/CI setup
- Architecture groundwork

Before contributing, read:
- [docs/PROJECT_CHARTER.md](./docs/PROJECT_CHARTER.md) — guardrails and goals  
- [docs/README.md](./docs/README.md) — documentation map and contribution guidance

---

## Community
- **GitHub** — Issues, Discussions, PRs (source of truth)  
- **Telegram (dev channel)** — realtime coordination  
- **docs.agopengps.com / Discourse** — public updates when available  

---

## License
GPLv3 — see [LICENSE](./LICENSE).  
Third-party components remain under their respective licenses.

---

## Quick Links
- [Project Charter](./docs/PROJECT_CHARTER.md)
- [Documentation Index](./docs/README.md)
- [Roadmap](./docs/ROADMAP.md)
- [SRS Overview](./docs/development/SRS/00_ReadMe.md)
- [Meet the Team](./docs/team/README.md)





