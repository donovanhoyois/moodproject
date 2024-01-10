using System.Text.Json.Serialization;

namespace MoodProject.Core.Models.User;

public class BaseUser
{
    [JsonPropertyName("nickname")]
    public string Nickname { get; set; } = string.Empty;
    [JsonPropertyName("user_metadata")]
    public UserMetadata? Metadata { get; set; }
}