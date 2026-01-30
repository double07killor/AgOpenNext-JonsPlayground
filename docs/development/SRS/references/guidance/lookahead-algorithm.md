# Lookahead Algorithm and Section Timing

This reference defines the lookahead computation used for section control timing.

## Inputs

- PlannedPathPreview with tractor and implement poses.
- Row and section geometry (centers and shutoff points).
- Boundary and coverage polygons.
- Speed profile and latency offsets.

## Algorithm Outline

1) Sample future poses at fixed resolution.
2) For each row/section shutoff point, compute intersection with boundary.
3) Convert intersection distance to time based on speed profile.
4) Apply lead/lag offsets per row and section.
5) Emit trigger events with SimClock timestamps.

## Output

- SectionLookaheadPlan: per-row and per-section on/off events.
- Each event includes:
  - target_id
  - on_or_off
  - trigger_time
  - expected_position

## Determinism

- Uses SimClock timestamps and deterministic sampling.
- If preview quality is degraded, suppress triggers and flag status.

## Open Issues

- Define default sampling resolution for high-speed operations.
- Define how to handle speed changes between preview updates.
