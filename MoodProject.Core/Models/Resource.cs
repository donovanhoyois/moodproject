using System.ComponentModel.DataAnnotations.Schema;

namespace MoodProject.Core.Models;

public class Resource
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public List<ResourceFile> Files { get; set; } = new();
}