# Kinematics EKF Specification

This reference defines the EKF structure for kinematics fusion in deterministic terms.

## State Vector

x = [px, py, pz, vx, vy, vz, roll, pitch, yaw, bgx, bgy, bgz, bax, bay, baz]^T

## Process Model

Given IMU measurements:

omega = gyro - b_g
accel = accel_meas - b_a

Prediction (discrete, dt):

1) Attitude update using omega (small angle or quaternion integration)
2) Velocity update: v = v + (R * accel + g) * dt
3) Position update: p = p + v * dt
4) Bias modeled as random walk

## Measurement Models

GNSS position:

z_p = p + R * r_gnss + v_p

GNSS velocity:

z_v = v + v_v

Wheel speed:

z_ws = v_forward + v_ws

Steering angle (optional):

z_delta = delta + v_delta

## Jacobians

Jacobian definitions are derived from the chosen attitude representation.
Use consistent ordering for state and error-state vectors.

## Noise Models

Default noise parameters (to be tuned):

- Gyro noise: 0.02 deg/sqrt(s)
- Accel noise: 0.02 m/s^2/sqrt(s)
- GNSS position noise: 0.02 m (RTK), 0.3 m (non-RTK)
- GNSS velocity noise: 0.05 m/s

## Filter Update Order

1) Predict with IMU
2) Update with GNSS position
3) Update with GNSS velocity
4) Update with wheel speed
5) Update with steering angle (if available)

## Output

Publish pose, velocity, attitude, and covariance summaries per SimClock tick.

## Open Issues

- Choose attitude integration method (quaternion vs DCM)
- Decide on error-state representation
