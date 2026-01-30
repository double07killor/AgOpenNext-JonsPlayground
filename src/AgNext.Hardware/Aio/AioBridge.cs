namespace AgNext.Hardware.Aio;

using AgNext.Core.Kinematics;
using AgNext.Core.Sensors;

public sealed class AioBridge
{
    private readonly IAioTransport _transport;

    public event Action<GnssSample>? GnssReceived;
    public event Action<ImuSample>? ImuReceived;

    public AioBridge(IAioTransport transport)
    {
        _transport = transport;
        _transport.FrameReceived += OnFrameReceived;
    }

    public void SendSteerCommand(double steerAngleRad, double speedMps)
    {
        var payload = new byte[8];
        var steer = (short)(steerAngleRad * 1000);
        var speed = (short)(speedMps * 100);
        payload[0] = (byte)(speed & 0xFF);
        payload[1] = (byte)((speed >> 8) & 0xFF);
        payload[2] = (byte)(steer & 0xFF);
        payload[3] = (byte)((steer >> 8) & 0xFF);
        _transport.Send(new AioFrame(0x7F, AioMessageCatalog.SteerDataPgn, payload));
    }

    private void OnFrameReceived(AioFrame frame)
    {
        if (frame.Pgn == AioMessageCatalog.ImuPgn && frame.Payload.Length >= 2)
        {
            var yawRate = BitConverter.ToInt16(frame.Payload, 0) / 1000.0;
            var roll = 0.0;
            var pitch = 0.0;
            if (frame.Payload.Length >= 6)
            {
                roll = BitConverter.ToInt16(frame.Payload, 2) / 1000.0;
                pitch = BitConverter.ToInt16(frame.Payload, 4) / 1000.0;
            }
            ImuReceived?.Invoke(new ImuSample(default, yawRate, roll, pitch));
        }

        if (frame.Pgn == AioMessageCatalog.GnssMainPgn && frame.Payload.Length >= 8)
        {
            var lat = BitConverter.ToInt32(frame.Payload, 0) / 1e7;
            var lon = BitConverter.ToInt32(frame.Payload, 4) / 1e7;
            var position = new AgNext.Core.Geometry.Vec3(lon, lat, 0);
            GnssReceived?.Invoke(new GnssSample(default, position, 0, 0, FixQuality.Standalone));
        }
    }
}
