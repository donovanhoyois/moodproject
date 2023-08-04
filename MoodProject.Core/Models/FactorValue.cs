namespace MoodProject.Core;

public class FactorValue
{
    public int Id { get; set; }
    public int SymptomId { get; set; }
    public FactorType Type { get; set; }
    public DateTime Timestamp { get; set; }
    public float Value { get; set; }

    public FactorValue()
    {
        
    }

    public FactorValue(DateTime timestamp, float value)
    {
        Timestamp = timestamp;
        Value = value;
    }
}