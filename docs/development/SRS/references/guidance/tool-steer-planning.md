# Tool Steering Planning

This reference defines how guidance plans tool steering and tractor offsets.

## Modes

- Tractor-on-path: vehicle path is authoritative.
- Tool-on-path: implement path is authoritative; tractor path is offset.
- Dual-path: maintain tractor within exclusion zones while tool follows path.

## Inputs

- Implement geometry and articulation model.
- Hitch and tool steering actuator limits.
- Field boundaries and exclusion zones.
- Guidance path definition and coverage plan.

## Tractor and Tool Paths

- Tool-on-path mode computes tractor path by applying the inverse implement transform.
- Tractor-on-path mode computes tool path by forward transform of implement geometry.
- If tool steering is enabled, tractor path can remain near a preferred lane while tool tracks the target path.

## Optimization Objectives

- Minimize tool path error (primary in tool-on-path).
- Minimize tractor intrusion into exclusion zones or crop.
- Minimize steering effort and curvature changes.
- Maintain smooth transitions between path segments.

## Output Contracts

- ToolSteerTargetSequence: desired tool articulation angles.
- TractorSteerTargetSequence: desired tractor steering angles.
- PlannedPathPreview: includes both tractor and tool paths.

## Open Issues

- Define maximum allowable tractor offset for each implement class.
- Define constraints for combined tractor + tool steering.
