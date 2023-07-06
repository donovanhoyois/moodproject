namespace MoodProject.Core;

public class Symptom
{
    public string Name { get; set; }
    public Factor PresenceFactor { get; set; }
    public Factor HarmfulnessFactor { get; set; }

    public Symptom(string name, Factor presenceFactor, Factor harmfulnessFactor)
    {
        Name = name;
        PresenceFactor = presenceFactor;
        HarmfulnessFactor = harmfulnessFactor;
    }
}