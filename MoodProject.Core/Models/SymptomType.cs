namespace MoodProject.Core;

public class SymptomType
{
    public int Id { get; init; }
    public string Name { get; set; }
    public SymptomType(int id, string name)
    {
        Id = id;
        Name = name;
    }
}