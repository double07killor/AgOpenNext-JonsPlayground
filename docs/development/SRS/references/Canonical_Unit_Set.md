---
title: Canonical Unit Set & Registry
version: 0.2.0
status: Draft
authors:
  - Jon Fortney
owner: Systems Engineering & Documentation Lead
reviewers:
  - Systems Engineering Team
approvers:
  - Project Coordinator
created: 2025-11-16
last_reviewed: 2025-11-16
review_cycle: Quarterly
notes: Shared registry documenting canonical base units, composite units, and ag-specific derived units with metadata requirements.
---

# Canonical Unit Set & Registry

This registry is the single source for canonical unit codes, metadata fields, and reference examples used by Plugins, Extensions, and UI bridges.

## Base Units

| Measurement | Canonical Unit | Unit Code | Example | Explanation |
|-------------|----------------|-----------|---------|-------------|
| Distance | Meter (`m`) | `UOM_LEN_M` | `12.34 m` | Linear geometry for pose, coverage, implements. |
| Time | Second (`s`) | `UOM_TIME_S` | `1234.512 s` | SimClock tick count expressed in elapsed seconds. |
| Timestamp | ISO 8601 UTC string | `UOM_TIME_ISO` | `2025-11-13T22:34:10.512Z` | Human-readable timestamp tied to the same SimClock tick. |
| Mass | Kilogram (`kg`) | `UOM_MASS_KG` | `5000 kg` | Mass totals (yield, seed, fertilizer). |
| Angle | Degree (`deg`) | `UOM_ANGLE_DEG` | `180` | Headings, convergence. |
| Volume | Liter (`L`) | `UOM_VOL_L` | `30 L` | Tank/fluid volumes. |
| Area | Hectare (`ha`) | `UOM_AREA_HA` | `1.25 ha` | Coverage, job sizing. |
| Percent | Percent (`%`) | `UOM_PERCENT` | `85%` | Thresholds/crop loss. |
| Pressure | Pascal (`Pa`) | `UOM_PRESSURE_PA` | `200000 Pa` | Hydraulic/air. |
| Temperature | Celsius (`C`) | `UOM_TEMP_C` | `20 C` | Ambient/fluid. |

## Composite Units

| Measurement | Canonical Unit | Unit Code | Example | Explanation |
|-------------|----------------|-----------|---------|-------------|
| Speed | Meter per second (`m/s`) | `UOM_SPEED_MS` | `4.5 m/s` | Guidance/section velocities. |
| Acceleration | Meter per second squared (`m/s^2`) | `UOM_ACCEL_MS2` | `1.2 m/s^2` | Guidance/traction tuning. |
| Angular Speed | Degree per second (`deg/s`) | `UOM_ANG_SPEED_DS` | `5 deg/s` | Steering/yaw dynamics. |
| Volume Flow | Liter per hectare (`L/ha`) | `UOM_FLOW_LPHA` | `35.7 L/ha` | Application rate. |
| Volume per Time | Liter per second (`L/s`) | `UOM_VOL_TIME_LS` | `0.8 L/s` | Pump/flow. |
| Count per Time | Count per second (`#/s`) | `UOM_COUNT_TIME_CS` | `120 /s` | Pulses/per-second metrics. |
| Count per Area | Count per hectare (`#/ha`) | `UOM_COUNT_AREA_CHA` | `180 #/ha` | Density of treated rows/passes per hectare. |
| Volume per Mass | Liter per kilogram (`L/kg`) | `UOM_VOL_MASS_LKG` | `0.5 L/kg` | Solutions. |
| Mass per Volume | Kilogram per liter (`kg/L`) | `UOM_MASS_VOLUME_KGL` | `1.2 kg/L` | Density. |
| Mass per Count | Kilogram per thousand (`kg/1000`) | `UOM_MASS_COUNT` | `25 kg/1000` | Seed weight. |
| Volume per Area | Liter per hectare (`L/ha`) | `UOM_VOL_AREA_YIELD` | `4200 L/ha` | Liquid yield. |
| Mass per Area | Kilogram per hectare (`kg/ha`) | `UOM_MASS_AREA_YIELD` | `8500 kg/ha` | Grain yield. |
| Parts per million | `ppm` | `UOM_PPM` | `150 ppm` | Moisture/contamination. |
| Rotational Speed | Rotations per minute (`rpm`) | `UOM_ROT_RPM` | `1200 rpm` | Spinning shafts. |
| Volume per Volume | Liter per liter (`L/L`) | `UOM_VOL_VOL` | `0.05 L/L` | Concentrations. |
| Mass per Mass | Kilogram per kilogram (`kg/kg`) | `UOM_MASS_MASS` | `0.02 kg/kg` | Ratios. |

## Two Bit State Encoding

| Name | Value | Binary | Description |
|------|-------|--------|-------------|
| Disabled/Off/Below | `0` | `00` | Capability is disabled, off, or reporting below threshold. |
| Enabled/On/Above/Loading | `1` | `01` | Capability is enabled, on, or loading. |
| Error | `2` | `10` | Capability reports an error condition. |
| Undefined/Not installed | `3` | `11` | Capability is undefined or not installed. |

## Agricultural Derived Units Imperial

| Measurement | Derived Unit | Unit Code | Canonical Pairing |
|-------------|--------------|-----------|------------------|
| Volume | Gallons | `UOM_VOL_GAL` | `Liter` canonical |
| Volume | Bushels | `UOM_VOL_BU` | `Liter` |
| Area | Acres | `UOM_AREA_AC` | `Hectare` canonical |
| Flow rate | Gallons per acre | `UOM_FLOW_GPACA` | `L/ha` canonical (1 gal/acre ~ 9.354 L/ha) |
| Volume Per Area | Bushels per acre | `UOM_VOL_BUACA` | `Liter` |


Derived units are stored as metadata pairs `(value, unit_code)` alongside canonical values so UI/Extensions can display operator-friendly metrics without altering determinism.

## Metadata Encoding

Every contract payload includes:
- `unit_code`: enumerated string (e.g., `UOM_LEN_M`).
- `unit_scale`: conversion factor to the canonical unit (1.0 when canonical).
- `crs_epsg`: EPSG code when spatial.
- `timestamp_iso8601`: ISO 8601 UTC string plus SimClock tick count.

Extensions introducing new units must register them here and provide conversion tables.
