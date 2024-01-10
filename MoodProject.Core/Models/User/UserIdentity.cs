using System.Text.Json.Serialization;

namespace MoodProject.Core.Models.User;

public class UserIdentity
{
    [JsonPropertyName("connection")]
    public string Connection { get; set; }
    [JsonPropertyName("provider")]
    public string Provider { get; set; }
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }
    [JsonPropertyName("isSocial")]
    public bool IsSocial { get; set; }
}