# AIO Transport Contract

This reference defines a generic transport contract for AIO-class boards.

## Transport Types

- Serial (USB or UART)
- UDP (local network)
- CAN (if supported by firmware)

## Frame Structure (Generic)

| Field | Size | Notes |
|-------|------|-------|
| Header | 1-2 bytes | Sync marker |
| Message ID | 1 byte | PGN or command id |
| Payload | N bytes | Schema-defined fields |
| Checksum | 1 byte | Simple sum or CRC |
| Footer | 1 byte | End marker |

## Required Message Classes

- GNSS position + fix quality
- IMU attitude + rates
- Wheel speed
- Steer command
- Section control
- Rate command

## Validation Rules

- Invalid checksum => reject and log.
- Missing required fields => reject.
- Out-of-range values => clamp or reject.

## Open Issues

- Define the canonical checksum algorithm.
- Define payload schemas and scaling factors.
