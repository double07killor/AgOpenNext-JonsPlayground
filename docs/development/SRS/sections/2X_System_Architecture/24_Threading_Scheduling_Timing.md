---
title: 24 - Threading, Scheduling, and Timing
version: 0.1.0
status: Draft
authors:
  - Jon Fortney
owner: Systems Engineering & Documentation Lead
reviewers:
  - Systems Engineering Team
approvers:
  - Project Coordinator
created: 2026-01-23
last_reviewed: 2026-01-23
review_cycle: Quarterly
notes: Defines deterministic scheduling, SimClock cadence, and timing budgets.
---

# 24 - Threading, Scheduling, and Timing
*(Status: Drafting - Deterministic Runtime Timing)*

**Section ID:** 24  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2026-01-23  
**Related Sections:** 22 - Process Model & Deployment; 23 - Units & Coordinate Systems  
**Upstream Dependencies:** 22 - Process Model; 23 - Units & Coordinates  
**Downstream Impacts:** 61 - Kinematics; 81 - Guidance Core; 83 - Autosteer Models

---

## 24.1 Purpose & Scope

Define the deterministic scheduler, SimClock cadence, thread priorities, and latency budgets that all safety-critical domains must follow.

---

## 24.2 Context

- Core runs in a single host with deterministic ordering.
- SimClock is the authoritative timebase across domains.
- Out of scope: hardware timer implementations.

---

## 24.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference / Source |
|--------------|-----------------|------------------------|---------------------------|--------------------|
| Scheduling | Mixed timers and threads. | Drift and jitter. | Single deterministic scheduler. | AOG runtime |
| Timing budgets | Implicit. | No latency guarantees. | Explicit budgets with verification. | QA gaps |

---

## 24.4 Definitions

| Term | Definition |
|------|-------------|
| SimClock Tick | One deterministic scheduler cycle. |
| Jitter | Variation in tick interval. |
| Budget | Max allowed latency per domain. |

---

## 24.5 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-24-000 | MUST | Performance | The Core scheduler MUST run at a fixed tick rate of 50 Hz. | C-22.2 | Tick rate verified in profiling. |
| R-24-001 | MUST | Performance | Scheduler jitter MUST be <= 2 ms p95 on reference hardware. | C-22.2 | Jitter benchmark meets target. |
| R-24-002 | MUST | Safety | Safety-critical domains MUST execute within assigned time budgets each tick. | C-22.1 | Budget watchdogs enforce limits. |
| R-24-003 | SHOULD | Reliability | Non-critical tasks SHOULD be deferred or throttled under load. | C-22.3 | Load tests show graceful degradation. |
| R-24-004 | MUST | Observability | Timing metrics MUST be logged for replay and diagnostics. | C-31.5 | Logs include tick timing data. |

---

## 24.6 Acceptance Criteria & Verification

- Profiling tests confirm tick rate and jitter targets.
- Load tests validate budget enforcement and throttling.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial timing requirements. | Systems Engineering & Documentation Lead | |
