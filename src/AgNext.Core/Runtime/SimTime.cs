namespace AgNext.Core.Runtime;

public readonly struct SimTime
{
    public double TimeSeconds { get; }
    public long Sequence { get; }

    public SimTime(double timeSeconds, long sequence)
    {
        TimeSeconds = timeSeconds;
        Sequence = sequence;
    }
}
