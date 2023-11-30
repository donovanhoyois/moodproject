namespace MoodProject.Core.Models;

public class User
{
    public int Id { get; set; }
    public string AuthProviderUserId { get; set; } = string.Empty;
    public bool HasAcceptedGdpr { get; set; }
}