using MoodProject.Core.Ports.In;
using MoodProject.Core.Ports.Out;

namespace MoodProject.Services;

public class ApiAuthService : IApiAuthService
{
    private readonly IAppApi AppApi;

    public ApiAuthService(IAppApi appApi)
    {
        AppApi = appApi;
    }
    
    public async Task<string> GetToken(string authProviderUserId)
    {
        return await AppApi.GetToken(authProviderUserId);
    }
}