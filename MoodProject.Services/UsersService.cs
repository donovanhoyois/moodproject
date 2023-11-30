using MoodProject.Core.Ports.In;
using MoodProject.Core.Ports.Out;

namespace MoodProject.Services;

public class UsersService : IUsersService
{
    private readonly IAppApi AppApi;

    public UsersService(IAppApi appApi)
    {
        AppApi = appApi;
    }
    
    public async Task<bool> GetGdprConsent(string authProviderId)
    {
        return await AppApi.GetGdprConsent(authProviderId);
    }

    public async Task<bool> AcceptGdpr(string authProviderId)
    {
        return await AppApi.AcceptGdpr(authProviderId);
    }
}