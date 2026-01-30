# Kinematics Test Plan

This reference defines test scenarios and validation rules for kinematics.

## Test Categories

1) Static accuracy
2) Dynamic accuracy
3) Dropout resilience
4) Calibration refinement
5) Multi-antenna behavior

## Scenario Library

- Straight line at constant speed (5 m/s, 10 m/s).
- Constant radius turn (r = 50 m) at 5 m/s.
- Figure-8 path with alternating curvature.
- Stop-and-go with acceleration and deceleration.
- GNSS dropout for 5 s and 10 s.
 - See `docs/development/SRS/references/core/simulator-scenarios.md` for the full scenario catalog.

## Metrics

- Horizontal error p95 (RTK: <= 2 cm).
- Heading error p95 (<= 0.5 deg at steady state).
- Latency p95 (<= 50 ms).
- Bias drift (gyro <= 0.02 deg/s over 10 min).

## Validation Rules

- Runs are valid if input data has fix quality >= threshold.
- Calibration refinement is valid if residual error improves by >= 50%.
- Dropout recovery is valid if pose remains continuous and variance increases.

## Artifacts

- Simulated scenarios with fixed seeds.
- Field logs with ground-truth markers.

## Open Issues

- Define ground-truth acquisition method for field tests.
