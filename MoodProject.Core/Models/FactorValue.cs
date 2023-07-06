namespace MoodProject.Core;

public class FactorValue
{
    public DateTime Timestamp { get; init; }
    public float Value { get; init; }

    public FactorValue(DateTime timestamp, float value)
    {
        Timestamp = timestamp;
        Value = value;
    }
}