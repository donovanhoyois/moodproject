namespace MoodProject.Core;

public class User
{
    public int Id { get; set; }
    public string AuthProviderUserId { get; set; }

    public User()
    {
        
    }
    public User(int id, string authProviderUserId)
    {
        Id = id;
        AuthProviderUserId = authProviderUserId;
    }
}