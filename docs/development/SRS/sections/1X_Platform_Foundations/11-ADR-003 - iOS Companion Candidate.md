---
title: ADR 11-003 — iOS Companion Candidate
version: 0.1.0
status: Draft
authors:
  - Jon Fortney
owner: Systems Engineering
reviewers:
  - Systems Engineering & Maintainers
approvers:
  - Project Coordinator
created: 2025-11-12
last_reviewed: 2025-11-12
review_cycle: Annual
notes: Companion-only iOS concept tied to charter stretch target (G2/P3).
---

# ADR 11-003 — iOS Companion Candidate
*(Status: Draft — 2025-11-12)*

## Context

The charter marks iOS as a P3 stretch target (G2). Operators appreciate companion-only experiences (headless Core + remote UI), and iOS offers a widely available tablet surface for such workflows without full-stack hardware access.

## Decision (Draft)

Document a companion-only iOS pathway that uses the UI Bridge to connect to headless Core instances running on desktop or embedded Linux hosts. This ADR remains a proposal; no approvals, builds, or field development happen until Windows/Linux parity, Android trial readiness, and charter-driven criteria are met.

## Consequences

- Companion iOS clients share the same UI Bridge APIs as other remote surfaces, simplifying transport development.
- Touch/UX considerations remain anchored to the Avalonia UI strategy to reuse view models and metadata widgets.
- Until governance elevates this draft, iOS stays outside the official release scope.

## Follow-Up

- capture acceptance criteria in the remote UI plan (latency, authentication, layout).  
- Validate gRPC/WebSocket transports between iOS companion shells and headless Core deployments.  
- Revisit for an approval cycle once the charter’s stretch-target criteria are satisfied.

## Change Log

| Version | Date | Changes | Author | PR / Issue |
|---------|------|---------|--------|------------|
| 0.1.0 | 2025-11-12 | Added companion-only iOS stretch target draft. | Jon Fortney |  |
