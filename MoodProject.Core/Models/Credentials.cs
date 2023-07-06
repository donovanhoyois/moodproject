namespace MoodProject.Core;

public class Credentials
{
    public string Mail { get; init; }
    public string Password { get; init; }

    public Credentials(string mail, string password)
    {
        Mail = mail;
        Password = password;
    }
}