namespace MoodProject.Core.Ports.In;

public interface IApiAuthService
{
    public Task<string> GetToken(string authProviderUserId);
}