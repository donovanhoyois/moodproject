namespace MoodProject.Core.Ports.In;

public interface IAuthService
{
    public Task<string> GetUserId(string mail);
}