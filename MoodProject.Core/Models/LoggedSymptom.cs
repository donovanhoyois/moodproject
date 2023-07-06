namespace MoodProject.Core;

public class LoggedSymptom : Symptom
{
    public DateTime TimeStamp { get; init; }

    public LoggedSymptom(string name, DateTime timeStamp, Factor presenceFactor, Factor harmfulnessFactor) : base(name, presenceFactor, harmfulnessFactor)
    {
        TimeStamp = timeStamp;
    }
}