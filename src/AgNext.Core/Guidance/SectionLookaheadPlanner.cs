namespace AgNext.Core.Guidance;

using AgNext.Core.Geometry;
using AgNext.Core.Kinematics;
using AgNext.Core.Simulation;

public sealed class SectionLookaheadPlanner
{
    private readonly double _leadDistanceMeters;

    public SectionLookaheadPlanner(double leadDistanceMeters)
    {
        _leadDistanceMeters = leadDistanceMeters;
    }

    public bool[] ComputeSectionStates(Pose2D implementPose, IReadOnlyList<Vec2> rowShutoffPoints, FieldBoundary boundary)
    {
        var forward = implementPose.Forward;
        var states = new bool[rowShutoffPoints.Count];
        for (var i = 0; i < rowShutoffPoints.Count; i++)
        {
            var lookaheadPoint = rowShutoffPoints[i] + forward * _leadDistanceMeters;
            states[i] = boundary.Contains(lookaheadPoint);
        }
        return states;
    }
}
