# Contour and Slope-Aware Planning

This reference defines slope-aware and contour-following path planning goals and strategies.

## Goals

- Prefer paths aligned to slope contours to reduce erosion and improve stability.
- Minimize point rows and sharp turns.
- Allow equipment width adjustments to improve contour adherence.

## Inputs

- Field boundary polygon and exclusion zones.
- Terrain model (DEM) or slope grid with elevation.
- Implement width range and overlap settings.
- Headland width constraints and minimum turn radius.

## Outputs

- ContourAlignedPlan: swaths aligned to contour lines with penalties for deviation.
- PointRowPlan: minimized turning rows with quantified penalty scores.
- PlanScore: composite score for plan comparison and selection.

## Scoring Model (Draft)

- Contour alignment score: penalize angle deviation from local contour tangent.
- Turn penalty: penalize high curvature and short headlands.
- Point row penalty: penalize single-row or partial-width passes.
- Coverage penalty: penalize overlap and gaps.

## Optimization Strategy (Draft)

- Generate candidate swath orientations by sampling contour tangent directions.
- For each orientation, compute swaths and headlands.
- Evaluate plan score and retain top candidates.
- Optional: run heuristic search (e.g., simulated annealing) to refine pass ordering.

## Determinism

- Candidate generation uses fixed seeds.
- Same inputs produce the same plan ordering and scores.

## Open Issues

- Define slope grid resolution standards for planning.
- Decide minimum acceptable contour alignment threshold per operation.
- Decide how to blend contour preference with straight AB-line requests.
