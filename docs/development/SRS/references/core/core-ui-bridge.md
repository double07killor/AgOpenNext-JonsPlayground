# Core to UI Bridge Protocol

This reference describes the Core to UI Bridge protocol used by desktop, remote, and mobile clients.

## Goals

- Deterministic, versioned data exchange.
- Clear command acknowledgement with safety gating.
- Late-join snapshot support and delta updates.

## Protocol Overview

### Handshake

`mermaid
sequenceDiagram
  participant UI as UI Client
  participant Core as Core
  UI->>Core: Hello(version, client_id, capabilities)
  Core->>UI: Welcome(session_id, token_scopes, heartbeat_ms, contract_catalog)
  UI->>Core: Subscribe(topics, delta_mode)
  Core-->>UI: Snapshot(topic, payload)
`

### State Stream

`mermaid
sequenceDiagram
  participant Core as Core
  participant UI as UI Client
  Core-->>UI: Snapshot(topic, payload, seq, simclock)
  Core-->>UI: Delta(topic, payload, seq, simclock)
  UI->>Core: Ack(seq)
`

### Command Flow

`mermaid
sequenceDiagram
  participant UI as UI Client
  participant Core as Core
  UI->>Core: Command(cmd_id, payload, operator_context)
  Core->>Core: Safety Gate Validation
  Core-->>UI: Ack(cmd_id, status, reason_code)
`

### Reconnect

`mermaid
sequenceDiagram
  participant UI as UI Client
  participant Core as Core
  UI->>Core: Resume(session_id, last_seq)
  Core-->>UI: Snapshot(topic, payload, seq)
  Core-->>UI: Delta(topic, payload, seq)
`

## Message Types

- Hello
- Welcome
- Subscribe
- Snapshot
- Delta
- Command
- Ack
- Heartbeat

## Versioning Rules

- Protocol version is negotiated during Hello/Welcome.
- Contract versions are enforced by the contract catalog.
- Unknown fields are ignored if optional and schema-compatible.

## Safety Gate Rules

- Commands are rejected if the system is not in a safe state.
- Commands include operator context, mode, and job state.
- All command results include reason codes.

## Open Issues

- Define exact transport (gRPC vs WebSocket) in ADR-42-001.
- Define delta encoding strategy (full field vs patch).
