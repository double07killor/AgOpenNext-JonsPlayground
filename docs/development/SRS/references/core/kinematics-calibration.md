# Kinematics Calibration

This reference defines calibration workflows for kinematics sensors and lever arms.

## Calibration Inputs

- GNSS antenna offsets (x, y, z).
- IMU mounting orientation (roll, pitch, yaw).
- Wheelbase and steering geometry.
- Wheel speed scale factor.

## Calibration Workflow

1. Operator enters baseline measurements from equipment setup.
2. Core runs a calibration routine while driving a known path.
3. Residual errors are computed against GNSS/IMU alignment.
4. Refined offsets are proposed and stored in the equipment profile.

## Refinement Rules

- Refinement only occurs when GNSS fix quality meets minimum thresholds.
- Updates are clamped to a maximum delta per session.
- All changes are logged with operator confirmation.

## Validation

- A calibration run is valid if residual error drops by at least 50%.
- Calibration must not reduce stability during dropout simulations.

## Open Issues

- Define exact calibration maneuver patterns.
- Define acceptable thresholds per vehicle type.
