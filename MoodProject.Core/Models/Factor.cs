namespace MoodProject.Core;

public class Factor
{
    public IEnumerable<FactorValue> ShortTermValue { get; set; }
    public IEnumerable<FactorValue> LongTermValue { get; set; }

    public Factor(IEnumerable<FactorValue> shortTermValue, IEnumerable<FactorValue> longTermValue)
    {
        ShortTermValue = shortTermValue;
        LongTermValue = longTermValue;
    }
}