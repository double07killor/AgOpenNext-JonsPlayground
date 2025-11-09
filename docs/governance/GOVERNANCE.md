---
title: AgOpenNext Governance Framework
version: 0.1.0
status: Draft
author: Jon Fortney
last_reviewed: 
---

# AgOpenNext Governance Framework

This document defines how AgOpenNext is organized, how authority is delegated, how files are controlled, and how decisions are recorded, reviewed, and amended.

Governance is designed to be objective, traceable, and evidence-based — decisions are grounded in data, design constraints, and measurable outcomes rather than popularity or personality.

## 1. Purpose & Scope

This framework establishes:
- The structure of leadership and technical roles
- The approval and change process for documents (SRS, ADRs, and Charter)
- File control and authorship requirements
- Decision-making authority and review rules

It does not govern the legal ownership of code or content. 

## 2. Organizational Structure

### 2.1 Leadership Roles

| Role | Description |
|------|--------------|
| Project Coordinator | Serves as the overall steward of the project and final authority for governance interpretation. Facilitates alignment across domains, approves major documents such as the Charter, and resolves disputes that cannot be settled within the technical team. |
| Project Lead(s) | Responsible for executing development within their section of the codebase (e.g., Core, UI, AgIO, Kinematics, Guidance etc) according to approved ADRs and requirements. Leads manage day-to-day implementation details, maintain code quality, and ensure their work conforms to system-level decisions made collectively through the ADR process. |



### 2.2 Technical Roles

| Role | Description |
|------|--------------|
| Systems Engineer(s) | Define and maintain the system architecture and requirements. Own and manage all SRS and ADR documents. Validate traceability between requirements and implementation, approve or reject SRS/ADR edits, and ensure decisions are documented and linked to commits. |
| Maintainers | Responsible for code-level stability and integration. Review and merge pull requests once requirements compliance has been verified by a Systems Engineer when applicable. Maintain build health, enforce coding standards, and ensure that merged work aligns with approved ADRs. |
| Reviewer(s) | Provide independent peer review for all pull requests. Every PR must be reviewed by at least one person who did not author the change. Reviewers check correctness, clarity, and test coverage before Maintainers merge. |


## 3. Authority & Responsibility

### 3.1 Project Coordinator
- Serves as the top-level authority for project direction and governance interpretation.
- Approves major governance or charter amendments after open community review.
- Resolves disputes that cannot be settled within the technical team.
- Coordinates between Project Leads to maintain alignment with the overall mission and roadmap.

### 3.2 Project Lead(s)
- Responsible for implementation and technical oversight within their section of the codebase (e.g., Core, UI, AgIO).
- Execute development work according to approved ADRs and requirements.
- Review and approve pull requests within their domain once they meet the required review and testing standards.
- Escalate architectural questions or cross-domain issues to the Systems Engineer.

### 3.3 Systems Engineer(s)
- Define and maintain the architecture and system requirements baseline.
- Approve or reject any edits to the SRS or ADR files.
- Ensure every requirement and ADR maintains traceability to implementation or testing.
- Sign off on architectural or protocol-level changes before merge.
- Provide final technical interpretation of ADRs when ambiguity exists.

### 3.4 Maintainers
- Manage repository health, enforce code style, and ensure CI/CD stability.
- Merge PRs only after required reviews are complete and, when applicable, Systems Engineer approval is obtained.
- Verify that merged work aligns with approved ADRs and does not break documented behavior.

### 3.5 Reviewers
- Provide independent peer review for all pull requests.
- Every PR must be reviewed by at least one person other than the author.
- Focus on code correctness, readability, and sufficient test coverage.
- Escalate architectural inconsistencies or unclear requirements to the Systems Engineer or relevant Project Lead.


## 4. Decision and Review Processes

This section defines how system requirements (SRS) and architectural decisions (ADRs) move from concept to approval, how governance documents are updated, and how the Project Charter is maintained.

### 4.1 SRS Section and Option Status

#### Section Status Flow

| Status | Description | Entry / Exit Criteria |
|---------|--------------|-----------------------|
| Collecting proposals | Default state for new topics. Requirements and problem statements are being gathered. | Entry: problem statement exists. Exit: baseline success metrics defined and open questions narrowed to decision-ready prompts. |
| Under review | Requirements believed complete enough to evaluate implementation options. | Entry: Maintainers and Systems Engineer agree content is evaluable. Exit: decision matrix (if needed) linked and dependencies addressed. |
| Ready for ADR | Consensus has formed on a preferred option family; acceptance criteria exist. | Entry: preferred option identified and validated. Exit: ADR author assigned and rollout / validation requirements documented. |
| Decided | ADR merged and traceability updated. | Exit: further changes require a new or revised ADR. |

#### Option Status Flow

| Status | Description | Entry / Exit Criteria |
|---------|--------------|-----------------------|
| Draft | Option being developed; dependencies may be incomplete. | Entry: concept or sketch exists. Exit: dependencies and readiness gates documented. |
| Under comparison | Option included in a decision matrix or structured evaluation. | Entry: dependency prerequisites listed. Exit: outcome recorded in matrix or ADR draft. |
| Candidate decision | Option selected as preferred approach pending ADR approval. | Entry: evaluation complete. Exit: ADR written and accepted. |
| Retired | Option remains for history but is no longer recommended. | Exit: superseded ADR recorded. |

#### Conventions

- IDs: `R-` for requirements, `O-` for options, `Q-` for open questions, `ADR-` for approved decisions.
- Status labels appear in section headings to track progress.
- Traceability between SRS entries, ADRs, and implementation commits is mandatory.
- Linting (unique IDs, link validation, table structure) should be automated in CI.

### 4.2 ADR Lifecycle

| Stage | Description | Required Action |
|--------|--------------|-----------------|
| Draft | Initial submission by author. | Include title, author, summary, date, and related SRS references. |
| In review | Open discussion period or assigned reviewers. | Link to relevant SRS sections or issues; gather feedback. |
| Approved | Accepted and merged. | Systems Engineer assigns ADR number, updates ADR index, and records traceability. |
| Superseded | Replaced by newer ADR. | Cross-link both ADRs; mark prior one as superseded. |
| Retired | No longer valid but kept for historical reference. | Mark as Deprecated in header and ADR index. |

All ADR modifications after approval must increment the version and include a changelog entry.

### 4.3 SRS and ADR Contribution Rules

- Only approved Systems Engineers may commit directly to `/docs/development/SRS/**` or `/docs/development/SRS/ADR/**`.
- Other contributors propose changes via pull requests.
- Each change must reference at least one related Issue or ADR.
- Systems Engineers ensure status fields and traceability matrices are updated before merge.

### 4.4 Governance Document Changes

- Governance document edits are Level 4 (see [DECISIONS.md](./DECISIONS.md)).
- Proposed through pull requests labeled governance.
- Each file must include an updated version number and changelog note.
- Approval required from the Project Coordinator **and** at least one Systems Engineer or Maintainer **who is not the author of the change**.
- The Coordinator may request additional reviewers or community feedback before merge.
- Once merged, a short summary of the change should be posted in public communication channels (e.g., Telegram, GitHub Discussions).

### 4.5 Charter Approval and Revision

**Approval process**
- Community discussion period of one week (Telegram + GitHub Discussions) with no major objections.
- Core contributors acknowledge the scope and agree to operate within it.
- The Project Coordinator declares the Charter accepted based on observed consensus.

**Revisions**
- Minor updates (typos or clarifications): commit directly with a changelog and announce.
- Major updates (scope, timeline, or governance): open Discussion, allow one-week comment period, update version number, and merge if no major objections remain.

No signatures or legal obligations are implied. The Charter represents a shared technical understanding, not a contract.


## 5. File Control & Traceability

### 5.1 Required Metadata Header

Every governance, SRS, ADR, template, or policy artifact must open with YAML front matter containing:

```
---
title: <document name>
version: <semantic version>
status: Draft|In Review|Approved|Superseded
authors:
  - <primary author>
owner: <role or team>
reviewers:
  - <reviewer name/role>
approvers:
  - <approver name/role>
created: YYYY-MM-DD
last_reviewed: YYYY-MM-DD
review_cycle: <frequency or trigger>
notes: <optional clarifications>
---
```

GitHub renders this metadata automatically, so a separate “Document Control” section is optional; if you repeat the values, mirror the YAML exactly to avoid conflicting facts.

### 5.2 Appendix: Change Log

Each controlled document must conclude with an `## Appendix: Change Log` section containing a table (Version, Date, Changes, Author, PR/Issue if known). This table provides reviewers with history without requiring git history searches.

### 5.3 Revision Control
- All updates occur through Pull Requests.
- Each merge increments the `version` field in the front matter.
- Major rewrites require the change-log table described above and a summary comment explaining the revision.
- Git history remains the source of truth – nothing is deleted.

### 5.4 Approval Records
The Systems Engineer maintains an index file (/docs/development/SRS/INDEX.md) listing every ADR and its approval signatures.

## 6. Enforcement & Compliance

### 6.1 Validation
Automated checks (CI or pre-commit hooks) should verify:
- Presence of metadata header
- Valid status value
- Linked Issue/ADR references

### 6.2 Attribution

All new or modified controlled documents must clearly identify an author.

This requirement exists both to ensure proper recognition of contributors **and** to preserve long-term traceability.  
Knowing who authored or last revised a document allows future contributors to ask questions, clarify intent, and maintain continuity as the project evolves.

Primary authors — those who have written the majority of the document — should be listed in the metadata header under the `author` field.  
Substantial revisions or additions by others (beyond minor typo or clarification fixes) must be recorded in the document’s changelog section or commit message summary.

Anonymous or unattributed commits to governance, SRS, or ADR files will be rejected.


## 7. Amendment Process

1. Create a pull request labeled **governance**.
2. Include a clear summary, rationale, and expected effect of the change.
3. The proposal must be reviewed by the **Project Coordinator** and at least one **Systems Engineer or Maintainer** who is not the author.
4. Approval follows Level 4 rules as defined in [DECISIONS.md](./DECISIONS.md).
5. Update the `version` and `last_reviewed` headers in the modified file.
6. After merge, post a short summary of the change in public communication channels (e.g., Telegram, GitHub Discussions).

## 8. References

- [docs/governance/DECISIONS.md](./DECISIONS.md)
- [docs/team/README.md](../team/README.md)
- [docs/development/SRS/](../development/SRS/)

## Appendix: Change Log

| Version | Date | Changes | Author | PR / Issue |
|---------|------|---------|--------|------------|
| 0.1.0 | 2025-11-09 | Added metadata/change-log template requirements and documented the enforcement steps. | Jon Fortney |  |

