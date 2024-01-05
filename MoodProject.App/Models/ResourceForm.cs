using MoodProject.Core.Enums;
using MoodProject.Core.Models;

namespace MoodProject.App.Models;

public class ResourceForm : Resource
{
    public static Dictionary<ResourceType, string> TypeDictionary { get; set; } = new()
    {
        { ResourceType.Contact, "Informations de contact" },
        { ResourceType.Meeting, "Lieux de recontre" },
        { ResourceType.Testimony, "Témoignages" },
        { ResourceType.Other, "Autres" },
    };
}