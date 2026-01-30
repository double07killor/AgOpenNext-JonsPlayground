# Section and Row Unit Geometry

This reference defines per-section and per-row geometry outputs for kinematics and section control.

## Row Unit Model

- Each row unit has its own center point and shutoff point.
- Row unit center is used for coverage and as-applied mapping.
- Shutoff point is used for section control timing (lead/lag).

## Row Unit Inputs

- Row spacing (m)
- Row count
- Toolbar center offset (m)
- Forward offset from hitch (m)
- Shutoff offset relative to row center (m)

## Row Unit Positions

Given toolbar center at (x0, y0):

- row_index i in [0..row_count-1]
- y_i = y0 + (i - (row_count-1)/2) * row_spacing
- x_i = x0 + row_forward_offset

Shutoff point:

- x_shutoff = x_i + shutoff_offset_x
- y_shutoff = y_i + shutoff_offset_y

## Section Aggregation

- Sections are logical groupings of row units.
- Section center is the average of row unit centers in the group.
- Section width is the span from leftmost to rightmost row unit.

## Output Contracts

- RowPose[]: per-row center positions in world frame.
- RowShutoffPose[]: per-row shutoff positions in world frame.
- SectionPose[]: per-section center positions in world frame.
- SectionPolygon[]: optional polygon for coverage.

## Timing

- All row and section outputs must be aligned to SimClock.
- Lead/lag adjustments use row shutoff positions and vehicle speed.

## Open Issues

- Define row unit naming conventions for variable-rate plugins.
- Add row-level quality flags in low-confidence modes.
