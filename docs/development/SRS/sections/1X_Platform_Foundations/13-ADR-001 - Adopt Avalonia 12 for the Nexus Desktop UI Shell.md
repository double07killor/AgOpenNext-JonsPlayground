---
title: ADR 13-001 - Adopt Avalonia 12 for the AgOpenNext Desktop UI Shell
version: 0.1.0
status: Draft
authors:
  - Nexus Team (Codex)
owner: Systems Engineering
reviewers:
  - Systems Engineering Team
approvers:
  - Project Coordinator
created: 2025-10-20
last_reviewed: 2025-11-09
review_cycle: Annual
notes: UI modernization decision.
---

# ADR 13-001 - Adopt Avalonia 12 for the AgOpenNext Desktop UI Shell
*(Status: Draft - 2025-11-09)*

## Context

Section 13 requires a cross-platform desktop shell and metadata-driven dashboards. Legacy WinForms and alternative stacks fractured tooling, so a single modern framework is needed.

## Decision

Adopt Avalonia 12 as the primary desktop shell, pairing it with the charter-mandated latest stable .NET version (per §5.1). WinForms stays available until parity is reached, but all new work targets Avalonia with shared view models and metadata-driven dashboards.

## Consequences

- Provides one UI codebase for Windows and Linux with shared theming.
- Enables metadata-driven dashboards and remote run modes without duplicate logic.
- Requires migration guides and trial runs until WinForms retirement.

## Follow-Up

- Publish Avalonia templates and metadata widget documentation.
- Schedule UX smoke tests for multi-monitor/run modes.
- Track accessibility and latency metrics to inform WinForms sunset.

## Change Log

| Version | Date | Changes | Author | PR / Issue |
|---------|------|---------|--------|------------|
| 0.1.0 | 2025-11-02 | Clarified scope and tasks. | Nexus Team (Codex) |  |
| 0.1.0 | 2025-10-20 | Initial decision. | Nexus Team (Codex) |  |
