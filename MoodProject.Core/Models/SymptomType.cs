using System.ComponentModel.DataAnnotations.Schema;

namespace MoodProject.Core.Models;

public class SymptomType
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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