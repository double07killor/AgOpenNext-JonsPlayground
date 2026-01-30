# Kinematics Math Model

This reference defines the math model for kinematics fusion and vehicle motion.

## Frames

- Vehicle frame: X forward, Y right, Z down.
- World frame: ENU or local tangent plane with CRS metadata.

## State Vector (example)

x = [px, py, pz, vx, vy, vz, roll, pitch, yaw, bgx, bgy, bgz, bax, bay, baz]^T

## Prediction Model

IMU integration (discrete, dt):

- omega = gyro - b_g
- accel = accel_meas - b_a
- update attitude with omega
- update velocity with rotated accel + gravity
- update position with velocity

## Measurement Models

GNSS position:

z_gnss = H_gnss * x + v
H_gnss selects position (with lever arm applied)

GNSS velocity:

z_vel = H_vel * x + v
H_vel selects velocity

Wheel speed:

z_ws = v_forward + v

Steering angle:

z_delta = delta_meas + v

## Lever Arm Application

Given antenna offset r = [rx, ry, rz]:

position_antenna = position_vehicle + R(roll,pitch,yaw) * r

For multiple GNSS antennas, each uses its own r_i.

## Vehicle Model

Bicycle model (low speed):

yaw_rate = v / L * tan(delta)

L = wheelbase

## Filter Structure

Use an error-state EKF:

1) Predict state with IMU.
2) Update with GNSS, wheel speed, steering angle.
3) Estimate biases (bg, ba).

Fallback:

Complementary filter with GNSS heading and IMU yaw rate.

## Outputs

- Pose (position, velocity, attitude)
- Covariance estimates for quality metrics

## Open Issues

- Choose specific noise models and tuning defaults.
