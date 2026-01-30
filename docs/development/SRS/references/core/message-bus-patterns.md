# Core Message Bus Patterns

This reference captures message bus patterns observed in AgModulesDemo and used for planning.

## Key Patterns

- **Type-safe messages**: struct messages, strongly typed handlers.
- **Scoped subscriptions**: module lifecycle cleanup via scope ids.
- **Queued handlers**: time-critical modules can buffer and process in order.
- **Last-message cache**: allow late-join snapshots from recent messages.
- **Failure isolation**: handler failures are tracked and can be removed.

## Recommended Rules

1. All messages carry timestamp metadata and sequence numbers.
2. Critical domains use queued subscriptions for deterministic processing.
3. Each module has a subscription scope for clean unload.
4. The bus exposes metrics for latency and backlog.

## Observed Tests

- Latency tests for high-rate GPS messages.
- Crash resilience tests (one handler fails, others continue).
- Load tests for burst handling.

## References

- AgModulesDemo `AgOpenGPS.Core/MessageBus.cs`
