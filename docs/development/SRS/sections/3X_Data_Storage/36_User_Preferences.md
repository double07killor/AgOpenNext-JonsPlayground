---
title: 36 - User Preferences
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
notes: Defines operator profiles, UI settings, and personalization storage.
---

# 36 - User Preferences
*(Status: Drafting - Operator Profiles)*

**Section ID:** 36  
**Version:** 0.1.0  
**Editors:** @owner  
**Last Updated:** 2026-01-23  
**Related Sections:** 91 - UI Shell; 92 - Gauges & Panels  
**Upstream Dependencies:** 32 - Persistence & Formats  
**Downstream Impacts:** 91 - UI Shell; 92 - Gauges & Panels

---

## 36.1 Purpose & Scope

Define how operator profiles, UI preferences, and display settings are stored and applied.

---

## 36.2 Context

- Preferences must not affect core safety logic.
- Profiles may be shared across devices.
- Out of scope: authentication policy (future section).

---

## 36.3 Legacy Comparison

| Area / Theme | Legacy Behavior | Identified Limitation | Modernization Opportunity | Reference / Source |
|--------------|-----------------|------------------------|---------------------------|--------------------|
| Preferences | Single config file. | No per-operator settings. | Profile-based preferences. | Operator feedback |

---

## 36.4 Definitions

| Term | Definition |
|------|-------------|
| Operator Profile | Saved preference set for a specific operator. |
| Workspace | Saved UI layout and panel preferences. |

---

## 36.5 Requirements

| ID | Priority | Category | Summary | Source / C-IDs | Key Metrics / Verification |
|----|----------|----------|---------|----------------|-----------------------------|
| R-36-000 | MUST | Capability | The system MUST support multiple operator profiles with distinct settings. | C-91.0 | Profile switching tests pass. |
| R-36-001 | SHOULD | Usability | Preferences SHOULD include unit display, layout, and alert settings. | C-92.0 | Preference tests validate fields. |
| R-36-002 | SHOULD | Portability | Profiles SHOULD be exportable for backup or transfer. | C-35.0 | Export tests pass. |

---

## 36.6 Acceptance Criteria & Verification

- Profile tests validate save/load and switching.
- Preference schema tests validate required fields.

---

## Appendix: Change Log

| Version | Date | Changes | Owner/Author | PR / Issue |
|---------|------|---------|--------------|------------|
| 0.1.0 | 2026-01-23 | Initial User Preferences requirements. | Systems Engineering & Documentation Lead | |
