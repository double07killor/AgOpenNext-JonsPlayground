---
title: 73 - Rendering
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
notes: Defines rendering performance and visual update rules.
---

# 73 - Rendering
*(Status: Drafting - Visual Performance)*

**Section ID:** 73  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2026-01-23  
**Related Sections:** 13 - UI Framework & UX; 71 - Mapping Core  
**Upstream Dependencies:** 71 - Mapping Core; 13 - UI Framework & UX  
**Downstream Impacts:** 91 - UI Shell; 92 - Gauges & Panels

---

## 73.1 Purpose & Scope

Define rendering performance targets, refresh cadence, and visual update rules for maps and dashboards.

---

## 73.2 Context

- Rendering is fed by mapping layers and UI contracts.
- Rendering must honor performance targets in the charter.
- Out of scope: detailed UI styling.

---

## 73.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference / Source |
|--------------|-----------------|------------------------|---------------------------|--------------------|
| Rendering | CPU-heavy. | Low FPS on low-end hardware. | GPU-accelerated rendering. | AOG UI |
| Refresh | Mixed cadence. | Visual jitter. | Fixed frame pacing. | Operator feedback |

---

## 73.4 Definitions

| Term | Definition |
|------|-------------|
| Frame Pacing | Consistent frame delivery without stutter. |
| Tile | Renderable map chunk. |

---

## 73.5 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-73-000 | MUST | Performance | Rendering MUST sustain >= 30 FPS on reference hardware. | C-13.5 | Benchmark meets target. |
| R-73-001 | SHOULD | Performance | Map layer updates SHOULD be applied within 200 ms of receipt. | C-71.0 | Update latency benchmarks pass. |
| R-73-002 | MUST | Usability | Rendering MUST avoid frame drops during critical guidance operations. | C-13.5 | Stress tests meet target. |

---

## 73.6 Acceptance Criteria & Verification

- FPS benchmarks meet target on reference hardware.
- Update latency tests meet targets.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial Rendering requirements. | Systems Engineering & Documentation Lead | |
