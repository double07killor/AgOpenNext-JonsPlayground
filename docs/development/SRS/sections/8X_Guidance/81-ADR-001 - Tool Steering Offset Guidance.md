---
title: 81-ADR-001 - Tool Steering Offset Guidance
version: 0.1.0
status: Accepted
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
---

# 81-ADR-001 - Tool Steering Offset Guidance

## Status

Accepted.

## Context

Some operations require the tractor to deviate from the implement path (e.g., haybine offset to keep tractor out of crop) or to align tractor and implement to the same path for compaction control. Guidance intent must support both.

## Decision

Guidance will support two target path modes:

1) Tractor-on-path: vehicle reference follows the guidance path.
2) Tool-on-path: implement reference follows the guidance path; tractor path is offset to maintain tool alignment.

The selected mode is stored in the guidance plan and applied to intent generation and lookahead planning.

## Rationale

- Supports crop protection and compaction strategies.
- Enables tool steering when available without forcing tractor onto the tool path.
- Aligns with operator expectations for different implements.

## Consequences

- Requires implement geometry and articulation to be configured.
- Requires lookahead preview to include both tractor and tool paths.
- Requires UI to surface which path is authoritative.

## Related References

- `docs/development/SRS/references/guidance/path-lookahead.md`
- `docs/development/SRS/references/guidance/tool-steer-planning.md`
