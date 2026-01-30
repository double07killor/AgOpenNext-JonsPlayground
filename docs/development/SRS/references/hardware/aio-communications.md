# AIO Board Communications

This reference captures hardware IO patterns for AIO-class boards (serial, CAN, UDP).

## Input Sources

- NMEA sentences (GGA, RMC, VTG) for GNSS position, speed, and heading.
- Proprietary NMEA extensions (e.g., PAOGI) for richer telemetry.
- IMU packets over serial or UDP for roll, pitch, yaw rate.

## Output Commands

- Steer command frames with angle, engage state, and checksum.
- Section control frames with bitmask payloads.
- Rate control frames with target and calibration metadata.

## Source Selection and Failover

- GNSS source aggregator should select the best available source.
- Serial auto-scan should detect valid NMEA streams (GGA/RMC/VTG).
- gpsd or network sources should be optional and configurable.

## Safety and Health

- Watchdog and heartbeat logs should record AIO link state.
- Invalid frames should be rejected and logged.

## References

- AgOpenGPS `AgIO` serial + NMEA parsing
- AgOpenGPS-Nexus `Aog.Agio` GNSS source aggregation and serial scanning
