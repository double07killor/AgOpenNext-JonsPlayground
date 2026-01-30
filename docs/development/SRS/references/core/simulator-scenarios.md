# Simulator Scenario Library

This reference defines the canonical scenario format and baseline scenarios for kinematics.

## Scenario Format (YAML)

```yaml
id: sim-straight-001
name: Straight Line RTK
description: Constant speed straight line with RTK-grade GNSS.
clock:
  rate_hz: 50
  duration_s: 300
  seed: 12345
vehicle:
  wheelbase_m: 3.0
  track_m: 2.2
  cg_height_m: 1.0
  reference_point: rear_axle
implements:
  tool_offset_m: 6.0
  tool_lateral_m: 0.0
path:
  type: line
  start:
    lat_deg: 47.1234567
    lon_deg: -119.1234567
  heading_deg: 90
  length_m: 1200
controls:
  speed_profile:
    type: constant
    value_mps: 5.0
  steering_profile:
    type: zero
sensors:
  gnss:
    rate_hz: 10
    noise:
      position_sigma_m: 0.02
      velocity_sigma_mps: 0.05
    dropout:
      enabled: false
  imu:
    rate_hz: 200
    noise:
      gyro_sigma_dps: 0.02
      accel_sigma_mps2: 0.02
    bias:
      gyro_bias_dps: [0.0, 0.0, 0.0]
      accel_bias_mps2: [0.0, 0.0, 0.0]
  wheel_speed:
    rate_hz: 50
    noise:
      speed_sigma_mps: 0.02
faults: []
outputs:
  include_ground_truth: true
  include_sensor_streams: true
```

## Standard Scenario Catalog

| ID | Purpose | Key Features |
| --- | --- | --- |
| sim-straight-001 | Baseline straight-line accuracy | Constant speed, RTK noise. |
| sim-straight-002 | Non-RTK accuracy | Larger GNSS noise, reduced fix quality. |
| sim-turn-001 | Constant radius turn | 50 m radius, 5 m/s. |
| sim-figure8-001 | Alternating curvature | Two loops with opposing curvature. |
| sim-stopgo-001 | Accel/decel behavior | Full stop and restart. |
| sim-dropout-gnss-001 | GNSS dropout resilience | 5 s dropout, IMU-only predict. |
| sim-dropout-imu-001 | IMU dropout resilience | 5 s dropout, GNSS-only update. |
| sim-bias-gyro-001 | Gyro bias drift | Slow bias random walk. |
| sim-slip-001 | Wheel slip event | Wheel speed scale error during turn. |
| sim-dual-gnss-001 | Dual-antenna heading | Multi-antenna heading blend. |
| sim-leverarm-err-001 | Lever arm misconfig | 0.3 m offset error, calibration recovery. |
| sim-latency-001 | Transport latency | Variable latency and jitter on GNSS. |

## Determinism Rules

- Scenario seed controls all stochastic noise generators.
- SimClock provides monotonically increasing timestamps.
- Output ordering follows sensor rate and SimClock tick.

## Validation Hooks

- Each scenario includes expected error bounds for p95 accuracy.
- Dropout scenarios include expected health flag transitions.
- Bias scenarios include expected drift rates.

## Open Issues

- Define standard coordinate origin for replay datasets.
- Add terrain slope models for roll/pitch excitation.
