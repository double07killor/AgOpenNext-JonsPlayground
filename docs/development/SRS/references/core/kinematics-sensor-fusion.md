# Kinematics Sensor Fusion

This reference describes the sensor fusion model used by Core kinematics.

## Goals

- Fuse GNSS, IMU, wheel speed, and steering angle into a single pose estimate.
- Preserve deterministic behavior with SimClock alignment.
- Support multiple GNSS receivers with configurable lever arms.

## State Model (High Level)

State vector (example):

- Position (x, y, z)
- Velocity (vx, vy, vz)
- Attitude (roll, pitch, yaw)
- Gyro bias (bgx, bgy, bgz)
- Accel bias (bax, bay, baz)

## Measurement Inputs

- GNSS position and velocity (primary absolute reference).
- IMU angular rates and accelerations (short-term dynamics).
- Wheel speed and steering angle (vehicle model constraints).
- Optional dual-antenna heading (yaw stabilization).
- GNSS source selection and failover handled by AgIO source aggregator.

## Lever Arm Handling

Each sensor is modeled with a lever arm relative to the vehicle reference point:

- GNSS antenna position offset (x, y, z).
- IMU mounting offset and orientation.
- Additional GNSS modules can be configured individually.

Lever arms are applied to transform sensor measurements into the vehicle frame.

## Input Acquisition

- GNSS streams are normalized from NMEA or proprietary frames into contract messages.
- IMU streams may arrive over serial, UDP, or AIO board frames.
- Each input path must report fix quality, timestamps, and health flags.

## Fusion Algorithm

Preferred approach: error-state EKF with:

- Prediction step using IMU integration.
- Update step using GNSS measurements and vehicle model constraints.
- Bias estimation for gyro and accel.

Fallback approach: complementary filter with reduced dynamics for low-end hardware.

## Health and Quality

Quality outputs should include:

- Fix type (RTK, float, DGPS, standalone).
- Variance/uncertainty estimates.
- Sensor health flags and drop counts.
- Implement and section pose quality flags derived from kinematics covariance.

## Determinism

- All updates are ordered by SimClock sequence number.
- Outputs include SimClock timestamps and sequence ids.

## Open Issues

- Define exact vehicle model (bicycle model vs full kinematic model).
- Define dual-GNSS blending strategy when both antennas present.
