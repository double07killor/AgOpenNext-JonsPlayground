# AgOpenNext Tasks (Large, Independent)

These tasks are designed to be worked on individually. Each task is scoped to a
separate “realm” to minimize conflicts and cross-coupling. Please follow the
instructions in each task and keep changes localized to the files/paths listed.

---

## AN-001 UI Module Palette + Layout Editing Mode
**Goal:** Add a module palette to let users add/remove UI modules at runtime, with a dedicated “layout edit mode.”

**Scope (UI only):**
- `src/AgNext.UI/MainWindow.axaml`
- `src/AgNext.UI/MainWindow.axaml.cs`
- `src/AgNext.UI/ViewModels/*`

**Requirements:**
- Add a right-side slide-in “Module Palette” panel that lists available modules with search/filter.
- Add a “Layout Edit Mode” toggle (top bar) that:
  - Shows resize handles for overlay modules.
  - Enables drag-and-drop reordering for right-rail modules.
  - Shows a trash/remove zone for quick removal.
- Modules added via palette should be tracked by a new registry (see existing `IUiModuleRegistry`).
- Persist palette state and module visibility in the same layout store (`UiLayoutStore`), or a sibling JSON file.
- Ensure no hard dependency on demo plugins; use metadata from registry.

**Acceptance:**
- User can add/remove modules without restart.
- Right-rail reorder persists across restarts.
- Overlay modules can be resized and persist size.

---

## AN-002 Plugin Contracts for UI Modules + Metadata
**Goal:** Formalize a plugin contract for UI module metadata and configuration.

**Scope (Core + UI plugin contract only):**
- `src/AgNext.Core` (new simple DTOs/interfaces)
- `src/AgNext.UI/ViewModels/Modules`
- `src/AgNext.UI.Plugins.Demo` (update to comply)

**Requirements:**
- Define a `UiModuleDescriptor` with fields: `Id`, `Title`, `Category`, `Region`, `IsMovable`, `DefaultSize`, `DefaultPosition`.
- Update registry so module providers can register descriptors first, then supply view models on demand.
- Add a simple “module settings” model with key/value pairs (string/object) and allow registry to pass defaults.
- Update demo plugin to register descriptors and create modules from descriptors.

**Acceptance:**
- UI can list modules without instantiating them.
- Module descriptors show in the palette (AN-001 will consume them).

---

## AN-003 Core-to-UI Telemetry Hub (Typed Snapshot)
**Goal:** Create a typed telemetry hub in Core for UI consumption, replacing ad-hoc message pulls.

**Scope (Core only):**
- `src/AgNext.Core/Messages`
- `src/AgNext.Core/Runtime`
- `src/AgNext.Core/Plugins` (if needed)

**Requirements:**
- Introduce a `UiTelemetrySnapshot` message aggregating speed, heading, autosteer state, section state, rate state, and planner state.
- Publish on a fixed cadence (e.g., every sim tick) via message bus.
- Ensure snapshot is stable and immutable (record-like).
- Add unit tests verifying snapshot contents are updated when relevant messages are published.

**Acceptance:**
- UI can bind to a single snapshot event for most read-only display data.

---

## AN-004 Map Rendering Layers + Overlay API
**Goal:** Build a layer-based rendering pipeline for the map view.

**Scope (UI only):**
- `src/AgNext.UI/Controls/MapView.cs`
- New folder `src/AgNext.UI/Rendering/*`

**Requirements:**
- Introduce a `IMapLayer` interface: `UpdateCpu`, `UploadGpu`, `Draw`.
- Implement at least 3 layers: boundary, swaths, vehicle path.
- Ensure layers can be toggled on/off by UI modules.
- Respect ES3 constraints; no geometry shaders.

**Acceptance:**
- Map still draws as before, but via layers.
- Easy to add new layers without touching MapView code.

---

## AN-005 Hardware Adapter: UDP Transport Implementation
**Goal:** Add a real UDP transport for the AIO bridge, separate from the in-memory simulator.

**Scope (Hardware only):**
- `src/AgNext.Hardware/Aio/*`

**Requirements:**
- Implement `UdpAioTransport` with connect, send, receive, and graceful shutdown.
- Add a config object for host/port/timeout.
- Provide a simple loopback test (unit test or integration) that validates send/receive.

**Acceptance:**
- AIO bridge can switch between in-memory and UDP with minimal code change.

---

## AN-006 Deterministic Simulation Replay
**Goal:** Add a deterministic replay feature for simulation runs.

**Scope (Core only):**
- `src/AgNext.Core/Simulation/*`
- `src/AgNext.Core/Logging/*`

**Requirements:**
- Record all inputs (manual control, swath changes, boundary edits) into a log with timestamps.
- Provide a replay mode that replays inputs and asserts state hashes at checkpoints.
- Add a CLI or simple entry point to run a replay file (can be internal, not public yet).

**Acceptance:**
- A recorded run can be replayed and match state hashes.

---

## AN-007 UI Settings + Persisted Workspace
**Goal:** Provide a user settings system and workspace file for UI layout, map settings, and preferences.

**Scope (UI only):**
- `src/AgNext.UI/ViewModels`
- `src/AgNext.UI` (new `Settings` folder)

**Requirements:**
- Create a `UserSettings` model with theme, units, and layout file path.
- Store settings under `%AppData%/AgOpenNext/settings.json`.
- Allow a “workspace” file to be loaded/saved that bundles layout + map layer toggles.
- Provide a simple Settings UI pane to edit the above (no need for polished styling).

**Acceptance:**
- Settings persist across restarts.
- Workspace save/load works.

---

## AN-008 Section Control UI + Diagnostics Panel
**Goal:** Build a diagnostics panel to show section control and rate state details.

**Scope (UI only):**
- `src/AgNext.UI/ViewModels`
- `src/AgNext.UI/MainWindow.axaml`

**Requirements:**
- Add a new right-rail module that displays:
  - Section on/off array with timestamps.
  - Rate targets and last command.
  - Planter population values.
- Add a simple “Export diagnostics” button to dump the current snapshot to JSON file.

**Acceptance:**
- Panel updates live and export writes a JSON file.

---

## AN-009 Config-Driven Equipment Profiles
**Goal:** Load equipment profiles from `configs/equipment/*.json` and apply to simulation.

**Scope (Core only):**
- `src/AgNext.Core/Simulation`
- `configs/equipment`

**Requirements:**
- Define a JSON schema for equipment profiles (vehicle + implement + sections).
- Add a loader that reads the config and applies to `SimulatorConfig`.
- Add a unit test that loads `configs/equipment/default-axle-6row.json`.

**Acceptance:**
- Simulator uses config data instead of hard-coded defaults.

---

## AN-010 Plugin Host Diagnostics + UI Listing
**Goal:** Add a plugin host diagnostics panel listing loaded plugins and modules.

**Scope (UI + plugin loader only):**
- `src/AgNext.UI/ViewModels`
- `src/AgNext.UI/MainWindow.axaml`
- `src/AgNext.UI/ViewModels/Modules`

**Requirements:**
- Track loaded plugin assemblies and providers (name, version, module count).
- Add a right-rail module that lists them.
- Show load errors with a brief message (do not throw).

**Acceptance:**
- User can see which plugins are loaded and which modules came from each.

