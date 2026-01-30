# Kinematics Simulation

This reference describes how the simulator drives kinematics and sensor fusion.

## Goals

- Provide deterministic replay for kinematics and guidance.
- Allow hardware-in-the-loop substitution for real sensors.

## Simulation Inputs

- Vehicle dynamics model (position, velocity, steering).
- Sensor noise profiles for GNSS and IMU.
- Configured lever arms and mounting offsets.

## Output Streams

- Simulated GNSS position/velocity.
- Simulated IMU angular rates and accelerations.
- Simulated wheel speed and steering angle.

## Determinism Rules

- All simulated outputs use SimClock for timestamps.
- Random noise uses fixed seeds per scenario.

## Validation

- Replay must match simulated truth within defined tolerances.
- Sensor dropout scenarios must trigger expected fallbacks.
- Scenario definitions live in `docs/development/SRS/references/core/simulator-scenarios.md`.

## Open Issues

- Define default noise models and distributions.
- Define standard scenario library for kinematics validation.
