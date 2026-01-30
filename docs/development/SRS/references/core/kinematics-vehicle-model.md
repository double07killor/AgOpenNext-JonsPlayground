# Kinematics Vehicle Model

This reference defines the non-bicycle kinematic model used for vehicle pose and section positioning.

## Model Summary

- Multi-body planar kinematic model with explicit front and rear axle centers.
- Track width is modeled to preserve lateral offsets.
- Steering is applied at the front axle with Ackermann approximations.
- Optional slip angle and wheel speed scaling can be fused when available.

## Inputs

- Wheelbase (m)
- Front track width (m)
- Rear track width (m)
- Steering angle at front axle (deg)
- Vehicle speed (m/s)
- IMU yaw rate (deg/s)
- Optional slip angle estimate (deg)

## State Variables

- Position of rear axle center (x, y)
- Heading (yaw)
- Velocity at rear axle center (v)

## Kinematic Update

- Use rear axle center as the vehicle reference point.
- Heading update uses yaw rate or steering geometry:
  - yaw_dot = v / wheelbase * tan(steer_angle) + slip_correction
- Position update:
  - x_dot = v * cos(yaw)
  - y_dot = v * sin(yaw)

## Axle and Wheel Geometry

- Front axle center position is derived from rear axle center + wheelbase in heading direction.
- Track width is used to compute left/right wheel positions for accurate section geometry.
- Ackermann approximation converts steering input to inner/outer wheel angles when needed.

## Output Frames

- Vehicle pose is published at rear axle center.
- Additional frames can be derived for front axle, hitch, and GNSS sensors.

## Accuracy Notes

- This model avoids the bicycle simplification by retaining track width.
- Use IMU yaw rate to reduce reliance on steering-only yaw estimates.
- If slip is detected, apply slip correction and increase covariance.

## Open Issues

- Define standard slip estimation method when wheel speed sensors exist.
- Define optional roll and pitch coupling for slope compensation.
