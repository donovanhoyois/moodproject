namespace MoodProject.Api.Interfaces;

public interface IAuthService
{
    public Task<string> GetUserId(string mail);
}