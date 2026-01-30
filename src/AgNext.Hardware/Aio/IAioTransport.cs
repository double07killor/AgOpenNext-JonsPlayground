namespace AgNext.Hardware.Aio;

public sealed record AioFrame(byte Source, byte Pgn, byte[] Payload);

public interface IAioTransport
{
    event Action<AioFrame> FrameReceived;
    void Send(AioFrame frame);
}
