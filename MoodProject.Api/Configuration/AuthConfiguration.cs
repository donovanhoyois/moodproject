namespace MoodProject.Api.Configuration;

public class AuthConfiguration
{
    public string Secret { get; init; }
    public string[] ValidAudiences { get; init; }
    public string ValidIssuer { get; init; }
    public int HoursBeforeTokenExpiration { get; init; }
}