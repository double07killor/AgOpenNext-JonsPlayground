---
title: 44 - Service APIs
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
notes: Defines external service APIs for automation and third-party integrations.
---

# 44 - Service APIs
*(Status: Drafting - External Interfaces)*

**Section ID:** 44  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2026-01-23  
**Related Sections:** 41 - Message Bus; 42 - UI Bridge; 94 - Remote Clients  
**Upstream Dependencies:** 41 - Message Bus; 42 - UI Bridge  
**Downstream Impacts:** 93 - Command Line & Automation; 94 - Remote Clients

---

## 44.1 Purpose & Scope

Define external APIs (gRPC/WebSocket/REST) that allow automation, telemetry export, and remote tooling while preserving Core safety and contract integrity.

---

## 44.2 Context

- External APIs expose a subset of UI Bridge and message bus contracts.
- Authentication and rate limiting are required for any external access.
- Out of scope: full UI rendering.

---

## 44.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference / Source |
|--------------|-----------------|------------------------|---------------------------|--------------------|
| Remote tooling | Ad-hoc TCP. | Unversioned and insecure. | Versioned APIs with auth. | Community scripts |
| Telemetry export | CSV logs. | Non-real time. | Streaming APIs. | Operator feedback |
| Automation | No formal API. | Hard to integrate. | CLI and API parity. | Feature requests |

---

## 44.4 Definitions

| Term | Definition |
|------|-------------|
| Service API | External endpoint exposed by Core for tools and integrations. |
| Client | External tool, automation script, or remote dashboard. |

---

## 44.5 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-44-000 | MUST | Security | Service APIs MUST require authentication and authorization scopes. | C-42.4 | Unauthorized calls rejected. |
| R-44-001 | MUST | Compatibility | APIs MUST be versioned with explicit deprecation timelines. | C-31.5 | Version headers verified in tests. |
| R-44-002 | SHOULD | Performance | Telemetry streaming SHOULD support >= 10 Hz updates per topic. | C-41.0 | Streaming bench meets rate. |
| R-44-003 | MUST | Reliability | API rate limiting MUST protect Core safety loops. | C-22.3 | Load tests cap external load. |
| R-44-004 | SHOULD | Auditability | All mutating API calls SHOULD be logged with operator identity. | C-65.0 | Audit logs present in job logs. |

---

## 44.6 Acceptance Criteria & Verification

- Auth tests verify access control and scopes.
- Load tests validate rate limiting and performance.
- Versioning tests validate deprecation warnings.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial Service API requirements. | Systems Engineering & Documentation Lead | |
