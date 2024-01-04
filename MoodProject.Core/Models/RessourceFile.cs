using System.ComponentModel.DataAnnotations.Schema;

namespace MoodProject.Core.Models;

public class RessourceFile
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int RessourceId { get; set; }
    public string Name { get; set; }
    public Uri Uri { get; set; }

    public RessourceFile()
    {
        
    }

    public RessourceFile(int ressourceId, string name, Uri uri)
    {
        RessourceId = ressourceId;
        Name = name;
        Uri = uri;
    }
}