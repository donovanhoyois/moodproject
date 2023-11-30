namespace MoodProject.Core.Ports.In;

public interface IUsersService
{
    public Task<bool> GetGdprConsent(string authProviderId);
    public Task<bool> AcceptGdpr(string authProviderId);
}