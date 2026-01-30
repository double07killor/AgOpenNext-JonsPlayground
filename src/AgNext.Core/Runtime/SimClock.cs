namespace AgNext.Core.Runtime;

public sealed class SimClock
{
    private readonly double _stepSeconds;
    private double _timeSeconds;
    private long _sequence;

    public SimClock(double stepSeconds)
    {
        _stepSeconds = stepSeconds;
    }

    public double StepSeconds => _stepSeconds;
    public SimTime Current => new(_timeSeconds, _sequence);

    public SimTime Tick()
    {
        _timeSeconds += _stepSeconds;
        _sequence++;
        return Current;
    }
}
