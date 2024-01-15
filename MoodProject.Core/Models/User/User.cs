using System.Text.Json.Serialization;

namespace MoodProject.Core.Models.User;

public class User : BaseUser
{
    [JsonPropertyName("identities")]
    public List<UserIdentity> Identities { get; set; } = new();
}