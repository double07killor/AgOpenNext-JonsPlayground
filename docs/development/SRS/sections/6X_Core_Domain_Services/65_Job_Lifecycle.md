---
title: 65 - Job Lifecycle
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
notes: Defines job/session states, logging, and audit trails.
---

# 65 - Job Lifecycle
*(Status: Drafting - Session and Audit)*

**Section ID:** 65  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2026-01-23  
**Related Sections:** 31 - Domain Data Model; 95 - Simulation & Replay  
**Upstream Dependencies:** 31 - Domain Data Model; 41 - Message Bus  
**Downstream Impacts:** 94 - Remote Clients; 95 - Simulation & Replay

---

## 65.1 Purpose & Scope

Define job/session state, start/stop semantics, audit trails, and data retention for operational runs.

---

## 65.2 Context

- Job lifecycle controls when data logging and exports begin/end.
- Job state is consumed by UI and remote tooling.
- Out of scope: storage format details (Section 32).

---

## 65.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference / Source |
|--------------|-----------------|------------------------|---------------------------|--------------------|
| Session state | Manual notes. | No consistent history. | Structured session states. | Operator feedback |
| Audit trail | Partial logs. | Missing operator actions. | Full command and alarm history. | QA gaps |

---

## 65.4 Definitions

| Term | Definition |
|------|-------------|
| Job | A single operational task with a start and end time. |
| Session | A runtime instance of Core tied to a job. |
| Audit Trail | Immutable log of operator commands and system events. |

---

## 65.5 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-65-000 | MUST | Capability | The system MUST implement a job state machine (setup, active, paused, completed, aborted). | C-31.5 | State transitions validated in tests. |
| R-65-001 | MUST | Auditability | All operator commands and alarms MUST be recorded in the job audit trail. | C-31.5 | Audit trail completeness tests pass. |
| R-65-002 | SHOULD | Reliability | Job state SHOULD survive unexpected restarts with <= 5 s state loss. | C-32.0 | Recovery tests meet target. |
| R-65-003 | SHOULD | Usability | Operators SHOULD be able to resume or duplicate prior jobs. | C-32.0 | UI tests validate resume. |

---

## 65.6 Acceptance Criteria & Verification

- State machine tests validate allowed transitions.
- Audit trail tests confirm completeness.
- Recovery tests validate restart behavior.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial Job Lifecycle requirements. | Systems Engineering & Documentation Lead | |
