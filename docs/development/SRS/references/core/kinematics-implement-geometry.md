# Kinematics Implement and Section Geometry

This reference defines how implement and section geometry is derived from the vehicle pose.
It complements the fusion model by specifying spatial outputs required by section control and mapping.

## Coordinate Frames

- Vehicle frame: origin at tractor reference point (default rear axle center).
- X axis forward, Y axis left, Z axis up (right-handed).
- Implement frame: origin at hitch point or implement reference defined in equipment config.

## Inputs

- Vehicle pose (position, velocity, attitude) from kinematics.
- Hitch type and offsets (drawbar, 3-point, tool pivot).
- Implement geometry (width, section widths, section offsets).
- Optional implement articulation sensor (angle or yaw).

## Tractor to Implement Transform

- Base transform: vehicle pose + lever arm from vehicle reference to hitch point.
- If implement is rigid (no articulation), use fixed yaw relative to vehicle frame.
- If implement is articulated, apply measured articulation angle to implement frame.

## Section Geometry Model

- Each section has a lateral offset (section_center_y) and width (section_width).
- Section center in implement frame:
  - x = section_forward_offset
  - y = section_center_y
  - z = section_height_offset
- Section polygon (for coverage) is derived by projecting left/right edges:
  - y_left = section_center_y + section_width / 2
  - y_right = section_center_y - section_width / 2
  - x uses section_forward_offset; z optional for slope handling.

## Output Contracts

- ImplementPose: pose of implement reference point in world frame.
- SectionPose[]: pose for each section center in world frame.
- SectionPolygon[] (optional): world-frame polygon for coverage.

## Accuracy and Timing

- Implement and section outputs must be aligned to SimClock.
- If articulation sensor drops out, hold last known angle and flag degraded quality.
- Position errors from lever arm uncertainty should be tracked in covariance outputs.

## Calibration Notes

- Hitch-to-implement offsets are calibrated during equipment setup.
- Optional refinement: infer lateral offset from repeated coverage overlap patterns.

## Open Issues

- Decide default reference point for different implement classes (planter, sprayer, mower).
- Define slope correction for section geometry on uneven terrain.
