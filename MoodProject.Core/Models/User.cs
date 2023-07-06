namespace MoodProject.Core;

public class User
{
    public string Firstname { get; set; }
    public string Mail { get; set; }
    public List<Symptom> Symptoms { get; set; }
}