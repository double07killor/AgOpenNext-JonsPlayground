# Field Coverage Planning

This reference defines coverage planning inputs, outputs, and integration with guidance.

## Goals

- Generate efficient swaths, headlands, and pass ordering.
- Support tool-on-path and tractor-on-path modes.
- Minimize overlap, avoid obstacles, and respect exclusion zones.

## Coverage Inputs

- Field boundary polygon with exclusions.
- Implement working width and overlap settings.
- Preferred headland passes and turn radius constraints.
- Guidance mode (tractor-on-path or tool-on-path).

## Coverage Outputs

- CoveragePlan: ordered list of swaths and turns.
- HeadlandPlan: boundary offset passes.
- PathSequence: stitched guidance paths for execution.

## Fields2Cover Integration

- Use Fields2Cover to compute swaths and ordering.
- Map Fields2Cover outputs to AgOpenNext path schema.
- Preserve metadata for replay and audit.

## Tool Steering Considerations

- Tool-on-path: swaths are generated for tool reference path.
- Tractor-on-path: swaths are generated for vehicle path.
- Dual-path: enforce tractor exclusion zones while tool path remains on swath.

## Open Issues

- Define default ordering strategies (e.g., AB, boustrophedon, spiral).
- Define transition behavior between headlands and interior swaths.
