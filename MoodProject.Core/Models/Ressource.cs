using System.ComponentModel.DataAnnotations.Schema;

namespace MoodProject.Core.Models;

public class Ressource
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public List<RessourceFile> Files { get; set; } = new();
}