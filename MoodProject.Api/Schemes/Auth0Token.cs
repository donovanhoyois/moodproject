using System.Text.Json.Serialization;

namespace MoodProject.Api.Schemes;

public class Auth0Token
{
    [JsonPropertyName("access_token")]
    public string Token { get; set; }
    [JsonPropertyName("scope")]
    public string Scope { get; set; }
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
    [JsonPropertyName("token_type")]
    public string Type { get; set; }
}

