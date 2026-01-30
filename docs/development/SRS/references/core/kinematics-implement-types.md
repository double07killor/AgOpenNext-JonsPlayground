# Implement Reference Points and Classes

This reference defines default reference points and geometry assumptions for implement classes.

## Implement Classes

### Planter

- Reference point: toolbar center at hitch line.
- Section centers: row units spaced by row spacing, centered on toolbar.
- Forward offset: row unit seed drop point.

### Sprayer

- Reference point: boom center at hitch line.
- Section centers: nozzle groups evenly spaced on boom.
- Forward offset: nozzle centerline (usually near boom center).

### Mower

- Reference point: cutter deck center.
- Section centers: deck sections as configured (left, center, right).
- Forward offset: blade centerline.

### Toolbar / Generic

- Reference point: toolbar center at hitch line.
- Section centers: evenly spaced across toolbar width.
- Forward offset: operator-configured.

## Hitch Types

- Drawbar: hitch point at rear axle offset (x, y, z).
- Three-point: hitch point at lower link midpoint.
- Trailed: hitch point at tongue pivot.

## Defaults and Overrides

- Each implement class defines defaults for reference and section centers.
- Operators can override all offsets in equipment configuration.
- Override values are stored per equipment profile.

## Open Issues

- Define standard row unit offset library for common planters.
- Add validation rules for asymmetric toolbars.
