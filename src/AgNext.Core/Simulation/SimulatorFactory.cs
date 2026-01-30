namespace AgNext.Core.Simulation;

using AgNext.Core.Geometry;
using AgNext.Core.Kinematics;

public static class SimulatorFactory
{
    public static Simulator BuildDefault()
    {
        var boundary = new FieldBoundary(new List<Vec2>
        {
            new(0, 0),
            new(200, 0),
            new(200, 200),
            new(0, 200)
        });

        var vehicleConfig = new VehicleConfig(
            WheelbaseMeters: 2.8,
            MaxSteerAngleRad: 0.6,
            MaxSpeedMps: 8.0,
            MaxSteerRateRadPerSec: 1.5);

        var axleConfig = LoadAxleConfig();
        var terrain = new FlatTerrainModel();
        var config = new SimulatorConfig(
            vehicleConfig,
            axleConfig,
            terrain,
            boundary,
            SwathHeadingRad: 0,
            SwathSpacingMeters: axleConfig.Rows.RowCount * axleConfig.Rows.RowSpacingMeters,
            StepSeconds: 0.02,
            GnssRateHz: 10,
            ImuRateHz: 50,
            WheelSpeedRateHz: 20,
            SteerAngleRateHz: 20);

        var initialState = new VehicleState
        {
            Position = new Vec2(40, 40),
            HeadingRad = 0,
            SpeedMps = 0,
            SteerAngleRad = 0
        };

        return new Simulator(config, initialState);
    }

    private static AgNext.Core.Kinematics.AxleCentric.AxleCentricConfig LoadAxleConfig()
    {
        var relative = Path.Combine("configs", "equipment", "default-axle-6row.json");
        var baseDir = AppContext.BaseDirectory;
        var candidate = Path.Combine(baseDir, relative);
        if (File.Exists(candidate))
        {
            return AgNext.Core.Kinematics.AxleCentric.AxleCentricLoader.Load(candidate);
        }

        return AgNext.Core.Kinematics.AxleCentric.AxleCentricLoader.Load(relative);
    }
}
