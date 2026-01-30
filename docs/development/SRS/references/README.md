# SRS Reference Library

Canonical specifications, capability catalogs, and interoperability guides that inform Nexus requirements live here. Each entry supports the corresponding SRS section and ADR set, giving engineers an authoritative contract to reference during design and review.

## Core runtime
- [Core data flow architecture](core/data-flow.md) - End-to-end pose, control, and analytics flows with links back to transport ADRs.
- [Capability registry](core/capability-registry.md) - Canonical capability identifiers and attributes used during negotiation.
- [Core to UI Bridge protocol](core/core-ui-bridge.md) - Handshake, state, and command exchange between Core and UI clients.
- [Core message bus patterns](core/message-bus-patterns.md) - Scoped subscriptions, queued handlers, and failure isolation.
- [Kinematics sensor fusion](core/kinematics-sensor-fusion.md) - State model, inputs, lever arms, and fusion rules.
- [Kinematics calibration](core/kinematics-calibration.md) - Calibration workflows and refinement rules.
- [Kinematics simulation](core/kinematics-simulation.md) - Simulation inputs, outputs, and determinism rules.
- [Kinematics math model](core/kinematics-math.md) - Equations, lever arms, and vehicle model.
- [Kinematics test plan](core/kinematics-test-plan.md) - Scenario list and validation metrics.
- [Kinematics EKF spec](core/kinematics-ekf-spec.md) - Error-state EKF structure and update order.
- [Kinematics implement geometry](core/kinematics-implement-geometry.md) - Implement pose and section geometry derivation.
- [Kinematics vehicle model](core/kinematics-vehicle-model.md) - Multi-body kinematic model and geometry inputs.
- [Implement reference points](core/kinematics-implement-types.md) - Defaults for implement classes and hitch types.
- [Section and row geometry](core/kinematics-section-row-units.md) - Row unit and shutoff geometry outputs.
- [Simulator scenarios](core/simulator-scenarios.md) - Scenario format and standard library.
- [Simulation loop model](core/simulator-loop.md) - Deterministic scheduling rules.

## Guidance stack
- [Execution & autosteer contracts](guidance/execution-and-autosteer-contracts.md) - 25 Hz orchestrator loop semantics and `SteerTargets` schema.
- [Path lookahead and preview](guidance/path-lookahead.md) - Planned path preview and section timing outputs.
- [Lookahead algorithm](guidance/lookahead-algorithm.md) - Deterministic section timing computation.
- [MPC horizon and constraints](guidance/mpc-horizon-constraints.md) - MPC tuning surfaces and limits.
- [Tool steering planning](guidance/tool-steer-planning.md) - Tractor/tool path coordination and offsets.
- [Field coverage planning](guidance/field-coverage-planning.md) - Swaths, headlands, and pass ordering.
- [Contour and slope planning](guidance/contour-slope-planning.md) - Slope-aware swaths and point-row minimization.

## Mapping platform
- [Mapping architecture](mapping/mapping-architecture.md) - Division of responsibility between deterministic Core services and the mapping plugin.
- [AgOpenGPS v6 mapping brief](aog-v6-mapping-brief.md) - Historical context and compatibility guardrails for the v6 stack.

## Hardware & transport references
- [AgIO PGN baseline](AgIO_PGN_Baseline.md) - Canonical message catalog for AgIO interoperability.
- [AgOpenGPS hardware platforms](AgOpenGPS_Hardware_Platforms.md) - Supported hardware SKUs and constraints.
- [ISOBUS section control](ISOBUS_Section_Control.md) - Task controller interoperability notes and compliance guardrails.
- [AIO board communications](hardware/aio-communications.md) - Serial, UDP, CAN input/output patterns and failover.
- [AIO transport contract](hardware/aio-transport-contract.md) - Frame structure and validation rules.
- [AIO message catalog](hardware/aio-message-catalog.md) - Canonical payload schemas and legacy PGN mappings.

## External projects
- [AgOpenGPS (official)](https://github.com/AgOpenGPS-Official/AgOpenGPS) - Legacy baseline for feature parity and migration references.
