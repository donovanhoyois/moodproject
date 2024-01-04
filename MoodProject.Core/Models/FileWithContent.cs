namespace MoodProject.Core.Models;

public class FileWithContent
{
    public string ParentName { get; set; }
    public string Name { get; set; }
    public string Base64Content { get; set; }

    public FileWithContent(string name, string base64Content)
    {
        Name = name;
        Base64Content = base64Content;
    }
}