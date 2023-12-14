using Microsoft.AspNetCore.Components.Authorization;

namespace MoodProject.App.Services;

public class IdentityService
{
    private readonly AuthenticationStateProvider AuthenticationStateProvider;
    
    public IdentityService(AuthenticationStateProvider authenticationStateProvider)
    {
        AuthenticationStateProvider = authenticationStateProvider;
    }
    public string UserId { get; set; } = string.Empty;
    
    public async Task<string> GetUserId()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        if (UserId.Equals(string.Empty) && authState.User.Claims.Any())
        {
            UserId = authState.User.Claims.First(c => c.Type.Equals("sub")).Value;
            return UserId;
        }
        return UserId;
    }

    public async Task<bool> IsAuthenticated()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        return authState.User.Claims.Any();
    }
}