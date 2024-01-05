using System.ComponentModel.DataAnnotations.Schema;

namespace MoodProject.Core.Models;

public class ResourceFile
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int ResourceId { get; set; }
    public string Name { get; set; }
    public Uri Uri { get; set; }

    public ResourceFile()
    {
        
    }

    public ResourceFile(int resourceId, string name, Uri uri)
    {
        ResourceId = resourceId;
        Name = name;
        Uri = uri;
    }
}