namespace MoodProject.Core;

public class SymptomHistory<T>
{
    private Dictionary<DateTime, Symptom> History { get; init; }

    public SymptomHistory(Dictionary<DateTime, Symptom> history)
    {
        History = history;
    }
}