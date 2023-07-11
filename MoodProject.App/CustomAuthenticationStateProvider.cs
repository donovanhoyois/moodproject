/*
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace MoodProject.App;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "test"),
        }, "Custom Authentication");
        
        

        var user = new ClaimsPrincipal(identity);

        return Task.FromResult(new AuthenticationState(user));
    }
}
*/