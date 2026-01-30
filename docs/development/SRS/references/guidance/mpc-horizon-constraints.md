# MPC Horizon and Constraints

This reference defines the MPC horizon, constraints, and tuning surfaces for autosteer.

## Horizon Definition

- Prediction horizon is defined by distance and time:
  - horizon_time_s (default 5 to 10 s)
  - horizon_distance_m (default 20 to 60 m)
- The controller uses the shorter of the two limits.
- Preview resolution is configurable (e.g., 0.1 s or 0.5 m).

## Constraints

- Max steering angle (deg)
- Max steering rate (deg/s)
- Max curvature (1/m)
- Max lateral acceleration (m/s^2)
- Max yaw rate (deg/s)

## Cost Function Inputs

- Cross-track error to path
- Heading error to path tangent
- Implement path error (when tool-on-path mode enabled)
- Control effort penalty
- Constraint slack penalty

## Multi-Actuator Support

- If tool steering is available, MPC allocates steering between tractor and implement:
  - Tractor steering prioritizes stability
  - Tool steering prioritizes tool-on-path error

## Tuning Surfaces

- Parameters are selectable per equipment profile.
- Default tuning profiles: low-speed, medium-speed, high-speed.

## Safety Gates

- If constraints are violated, fall back to safe steering command limits.
- If MPC solve time exceeds budget, use last valid target and flag degraded mode.

## Open Issues

- Define default horizon per vehicle class.
- Validate solve times on low-end hardware.
