# Simulation Loop Model

This reference defines the simulator loop used for deterministic replay.

## Execution Modes

- Real-time: ticks at SimClock cadence.
- Simulated-time: ticks advance instantly based on scheduled events.

## Core Loop

1) Advance SimClock.
2) Emit simulated sensor frames.
3) Run kinematics + guidance + autosteer.
4) Record outputs and metrics.

## Determinism Rules

- All randomness uses fixed seeds per scenario.
- Inputs are replayed in recorded order.
- Outputs are verified against expected ranges.

## Scheduling

- Use a single scheduler for all simulated tasks.
- Allow queued message processing to avoid reentrancy.

## Open Issues

- Define maximum simulation time scale for CI.
