namespace AgNext.Hardware.Aio;

public sealed class InMemoryAioTransport : IAioTransport
{
    public event Action<AioFrame>? FrameReceived;

    public void Send(AioFrame frame)
    {
        FrameReceived?.Invoke(frame);
    }
}
