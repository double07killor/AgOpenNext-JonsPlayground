---
title: Decision Levels and Approval Rules
version: 0.1.0
status: Draft
authors:
  - Jon Fortney
owner: Governance Working Group
reviewers:
  - Project Coordinator
approvers:
  - Project Coordinator
created: 2025-11-08
last_reviewed: 2025-11-08
review_cycle: Annual
license: GPLv3
---

# Decision Levels and Approval Rules

This document defines the classification, evidence, and approval requirements for changes within AgOpenNext.  
All decisions should be objective, traceable, and grounded in data or design evidence.

## 1. Decision Levels

| Level | Scope | Examples | Required Evidence | Approval |
|-------|--------|-----------|------------------|-----------|
| **L0 — Trivial** | Typos, formatting, or comment updates | Fixing grammar, adjusting whitespace | None | Self-merge after CI passes |
| **L1 — Minor / Internal** | Tooling, build scripts, or non-functional refactors | Updating CI pipelines, renaming internal variables | Successful build/test | One Reviewer or Maintainer |
| **L2 — Domain Feature / Refactor** | Changes confined to a single code area that do not affect public interfaces | UI layout changes, algorithm tuning, minor logic refactor | Tests updated and passing; rationale in PR | Responsible Project Lead + one additional Reviewer |
| **L3 — Cross-Domain or API Change** | Features or refactors that span multiple modules or modify public interfaces | Adding new AgIO message type, modifying Core-UI API, altering configuration schema | Design notes or evaluation; linked SRS and draft ADR | At least two Maintainers or Systems Engineers |
| **L4 — Architectural / Governance / License** | Any change that alters architecture, protocols, or project governance | New module interface, ADR framework changes, governance edits | RFC or ADR with rationale and acceptance criteria | Project Coordinator + at least one Systems Engineer or Maintainer (not the author) |

All L3 and L4 changes require traceability to an ADR or RFC recorded in `/docs/development/SRS/ADR/`.

## 2. Evidence Expectations

Each decision level above L1 must provide one or more of the following forms of evidence:

- Reference to requirements (`R-` IDs) and related ADR(s)
- Benchmarks or test results proving no regression
- Design comparison or decision matrix
- Implementation notes within the pull request
- Updated documentation or changelog entries

Incomplete or missing evidence is grounds for rejection until corrected.

## 3. Review Rules

- Every pull request must be reviewed by at least one person who did **not** author the change.  
- For L3 and L4 changes, the approving reviewers must include a Systems Engineer or Maintainer.  
- The Project Coordinator holds final arbitration when reviewers disagree or when scope crosses multiple domains.  
- Reviews should focus on correctness, traceability, and measurable impact — not personal style preferences.

## 4. Traceability

- All decisions, regardless of level, must be linked to at least one Issue, ADR, or SRS section.  
- Commits should reference the relevant identifiers (e.g., `ADR-0012`, `R-102`, or issue number).  
- The Systems Engineer maintains the master index of ADRs and their current status.

## 5. Escalation and Resolution

1. Unresolved review conflicts escalate to the Project Coordinator for mediation.  
2. If consensus cannot be reached, the Coordinator issues a final ruling documented in the ADR or PR discussion.  
3. Any objection must include a technical rationale and, if possible, an alternative recommendation.  

## 6. Amendments

Changes to this document follow the [Governance Amendment Process](./GOVERNANCE.md#7-amendment-process).

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2025-11-09 | Added metadata/change-log requirement and documented the new policy inline. | Jon Fortney |  |
