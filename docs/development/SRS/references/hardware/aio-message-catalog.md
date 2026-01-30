# AIO Message Catalog (Draft)

This reference defines the canonical AIO message catalog used by Core and AgIO bridges.
It maps legacy AgOpenGPS PGNs to normalized payload schemas.

## Goals

- Provide a stable catalog for Core <-> AIO interoperability.
- Normalize payloads into unit-aware schemas used by Core and plugins.
- Preserve compatibility with legacy AOG PGNs where possible.

## Source of Truth

- Legacy wire format and PGN IDs come from `docs/development/SRS/references/AgIO_PGN_Baseline.md`.
- This catalog defines canonical field names and units that Core uses internally.

## Canonical Payload Schemas

### SteerCommand

| Field | Type | Units | Notes |
| --- | --- | --- | --- |
| target_steer_angle | float | deg | Signed; positive = left (vehicle frame). |
| speed | float | m/s | Vehicle speed at command time. |
| engage_state | enum | - | disengaged, engaged, standby. |
| xte | float | m | Cross-track error at command time. |
| section_mask_1_16 | uint16 | bitmask | Optional section control 1-16. |

### SteerFeedback

| Field | Type | Units | Notes |
| --- | --- | --- | --- |
| actual_steer_angle | float | deg | Signed; from steering sensor. |
| imu_heading | float | deg | Heading from IMU module (if present). |
| imu_roll | float | deg | Roll from IMU module (if present). |
| switch_state | uint8 | bitmask | Driver input state. |
| pwm_display | uint8 | - | Debug or status display value. |

### ImuSample

| Field | Type | Units | Notes |
| --- | --- | --- |
| heading | float | deg | Absolute heading from IMU. |
| roll | float | deg | Roll. |
| yaw_rate | float | deg/s | Yaw rate around Z. |

### GnssMain

| Field | Type | Units | Notes |
| --- | --- | --- | --- |
| latitude | float | deg | WGS84 latitude. |
| longitude | float | deg | WGS84 longitude. |
| heading_true | float | deg | True heading from GNSS. |
| heading_dual | float | deg | Dual-antenna heading if available. |
| speed | float | m/s | Ground speed. |
| roll | float | deg | Roll estimate from GNSS/INS. |
| altitude | float | m | Altitude MSL. |
| satellites | uint8 | count | Tracked satellites. |
| fix_quality | enum | - | none, float, rtk, dgps, standalone. |
| hdop | float | - | Horizontal dilution. |
| age | float | s | Age of correction. |
| imu_heading | float | deg | IMU heading if bridged. |
| imu_roll | float | deg | IMU roll if bridged. |
| imu_pitch | float | deg | IMU pitch if bridged. |
| imu_yaw_rate | float | deg/s | IMU yaw rate if bridged. |

### MachineData

| Field | Type | Units | Notes |
| --- | --- | --- | --- |
| uturn | bool | - | U-turn state. |
| speed | float | m/s | Speed reported by machine module. |
| hyd_lift | bool | - | Hydraulic lift status. |
| tram | bool | - | Tramline status. |
| geo_stop | bool | - | Geofence stop. |
| section_mask_1_16 | uint16 | bitmask | Section control 1-16. |

## AOG PGN Mappings (Legacy Compatibility)

The mappings below define which legacy PGNs correspond to canonical payloads.
Scaling factors follow the AOG baseline and must be confirmed per firmware.

| Canonical Payload | PGN (hex) | PGN (dec) | Direction | Default Rate | Notes |
| --- | --- | --- | --- | --- | --- |
| SteerCommand | FE | 254 | Core -> AIO | 25 Hz | AOG Steer Data. |
| SteerFeedback | FD | 253 | AIO -> Core | 25 Hz | AOG From Autosteer. |
| ImuSample | D3 | 211 | AIO -> Core | 50 Hz | AOG From IMU. |
| GnssMain | D6 | 214 | AIO -> Core | 10 Hz | AOG Main Antenna. |
| MachineData | EF | 239 | AIO -> Core | 10 Hz | AOG Machine Data. |

### Scaling and Packing Rules (To Confirm)

- Angles are encoded as int16 with scale 0.01 deg.
- Speed uses int16 with scale 0.1 m/s.
- Latitude/Longitude are int64 with scale 1e-7 deg.
- Roll, pitch, yaw rate use int16 with scale 0.01 deg or 0.01 deg/s.

## Validation Rules

- Reject frames that fail checksum or length validation.
- Clamp out-of-range values and set health flags.
- Report missing required fields as sensor dropouts.

## Open Issues

- Confirm exact scaling for speed and angles per firmware variant.
- Add tool GPS and section dimension payload mappings.
- Define CAN framing for AIO where UDP/serial are not used.
