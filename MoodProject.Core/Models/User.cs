namespace MoodProject.Core;

public class User
{
    public int Id { get; set; }
    public string Mail { get; set; }
    public List<Symptom> Symptoms { get; set; }
}