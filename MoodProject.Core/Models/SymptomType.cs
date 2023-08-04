namespace MoodProject.Core;

public class SymptomType
{
    public int Id { get; set; }
    public string Name { get; set; }

    public SymptomType()
    {
        
    }
    public SymptomType(int id, string name)
    {
        Id = id;
        Name = name;
    }
}