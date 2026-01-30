namespace AgNext.Core.Guidance;

using AgNext.Core.Kinematics;

public interface IAutoSteerController
{
    double ComputeSteerTarget(Pose2D pose, GuidancePath path);
}
