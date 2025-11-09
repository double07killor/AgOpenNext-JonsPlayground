---
title: Code Style (C#/.NET 10 + Avalonia)
version: 0.1.0
status: Draft
authors:
  - Jon Fortney
owner: Development Standards Working Group
reviewers:
  - Systems Engineering
approvers:
  - Project Coordinator
created: 2025-11-09
last_reviewed: 2025-11-09
review_cycle: Annual
license: GPLv3
---

# Code Style

Goals: consistency, readability, easy reviews, and zero bikeshedding.  
CI enforces these rules automatically; local `dotnet format` should fix most issues.

## 1. Language and Project Defaults
- Target: .NET 10, latest C# language version.
- `nullable` enabled in all projects.
- File-scoped namespaces.
- `implicit usings` enabled.
- Treat warnings as errors in CI.

## 2. Naming
- `PascalCase` for public types and members.
- `camelCase` for locals and parameters.
- `_camelCase` for private fields; no prefixes like `m_` or `s_`.
- Async methods end with `Async`.
- Interfaces start with `I`.

## 3. Layout and Formatting
- Indent with 4 spaces.
- One type per file.
- Braces on new lines for all blocks.
- Keep files under roughly 500 lines when possible; split partial classes if needed.

## 4. Nullability and Parameters
- Prefer non-nullable types; use nullable only when truly optional.
- Validate public inputs and throw `ArgumentNullException` or `ArgumentOutOfRangeException` as needed.
- Include `CancellationToken` in any potentially long-running async method.

## 5. Exceptions and Logging
- Never swallow exceptions; always add context.
- Use structured logging; avoid string concatenation for log messages.
- Throw exceptions only for exceptional conditions, not control flow.

## 6. Async Guidelines
- Avoid `async void` except in event handlers.
- Use `ConfigureAwait(false)` in libraries where appropriate.
- Avoid fire-and-forget; if required, document intent and error handling.

## 7. Dependency Injection, Options, and Globals
- No singletons via static classes.
- Use dependency injection.
- Configuration handled through `IOptions<T>` or equivalent.
- Avoid ambient or global state; pass dependencies explicitly.

## 8. Public API
- XML documentation required for all public APIs.
- Breaking public changes require an ADR under `/docs/development/SRS/ADR/`.

## 9. Testing
- Add or update unit tests for all new code where feasible.
- Maintain deterministic tests; avoid time-based sleeps.
- Public interfaces and protocols require schema or snapshot tests.

## 10. Avalonia UI
- Follow MVVM pattern; Views contain no business logic.
- No blocking calls on the UI thread.
- Ensure DPI and scaling support (100%, 150%, 200%).
- Use styles, templates, and bindings over code-behind layout logic.
- Ensure accessibility: keyboard focus, contrast, and readable sizes.

## 11. Commits and Pull Requests
- Use conventional commit prefixes (`feat:`, `fix:`, `refactor:`, `docs:`, etc.).
- Keep PRs small and focused.
- Include screenshots or short clips for UI changes.
- Always run locally before PR:
  - `dotnet restore`
  - `dotnet build`
  - `dotnet test`
  - `dotnet format`

## 12. File Headers
- No boilerplate headers in source files.
- License handled at repository level; use top-level `LICENSE` file.

## 13. Tooling
- Run `dotnet format` before pushing.
- Roslyn analyzers enabled.
- StyleCop optional; enable only rules that improve clarity without slowing review.

## 14. Comments and Authorship

- Comment intent, not mechanics. Explain *why* something is done, not *what* the code already shows.
- Keep comments current; outdated comments are worse than none.
- Use XML or triple-slash `///` comments for all public APIs and interfaces.
- Avoid cluttering code with author tags or initials.
- Major rewrites or specialized algorithms may include an inline note such as:
  `// Implemented by <name> <date> — rationale: <short reason>`
- Long-term authorship and revision history live in Git commits and the repository log, not the source file.
- Code comments should be clear and useful. Humor and TODOs are fine in moderation – just make sure they don’t obscure intent or mislead someone reading the code later.

## Appendix: Change Log

| Version | Date | Changes | Author | PR / Issue |
|---------|------|---------|--------|------------|
| 0.1.0 | 2025-11-09 | Added governance metadata/change-log appendix requirement and recorded it here. | Jon Fortney |  |
