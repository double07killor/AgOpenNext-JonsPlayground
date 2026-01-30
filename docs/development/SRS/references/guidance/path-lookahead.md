# Path Lookahead and Preview Planning

This reference defines how guidance produces a future path preview for autosteer and section timing.

## Goals

- Provide a deterministic planned path preview for the next X meters or X seconds.
- Use the preview to compute section and row unit lookahead triggers.
- Provide UI visualization of planned path and tool tracking.

## Inputs

- Current vehicle pose and kinematics state.
- Active guidance path (AB line, curve, contour, headland).
- Implement geometry and articulation model.
- Configured prediction horizon (meters and/or seconds).

## Outputs

- PlannedPathPreview: ordered list of future poses (vehicle + implement) with timestamps.
- SectionLookaheadPlan: per-section and per-row lead/lag trigger events.
- AutosteerTargetSequence: steering targets across the prediction horizon.

## Planned Path Preview

- Preview is generated at a fixed resolution (e.g., 0.1 s or 0.5 m).
- Each preview point includes:
  - SimClock timestamp
  - Vehicle pose (x, y, yaw)
  - Implement pose
  - Section center positions
  - Curvature and target speed

## Section Lookahead

- For each row/section, compute when it will cross the boundary or target path edge.
- Convert boundary crossings into lead/lag timing for section on/off commands.
- Lead/lag uses vehicle speed and configurable offsets per row unit.

## UI Visualization

- UI consumes PlannedPathPreview and renders:
  - Vehicle path preview
  - Implement/tool path preview
  - Section centers and forecasted on/off positions

## Determinism Rules

- Preview generation uses SimClock timestamps.
- If inputs are missing, preview is withheld and quality flag is set.

## Open Issues

- Define default preview horizon for low-speed vs high-speed modes.
- Define preview density for UI without overloading IPC.
