namespace MoodProject.Api.Configuration;

public class AuthorityConfiguration
{
    public string ClientId { get; init; }
    public string ClientSecret { get; init; }
    public string Audience { get; init; }
    public string GrantType { get; init; }
    public string TokenUrl { get; init; }
    public string BaseUrl { get; init; }
}