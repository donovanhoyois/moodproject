using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MoodProject.Api.Services;

namespace MoodProject.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]/")]
[EnableCors]
[Authorize]
public class UsersController
{
    private readonly MoodProjectContext DbContext;
    private readonly AuthorizationService AuthorizationService;
    
    public UsersController(MoodProjectContext dbContext, AuthorizationService authorizationService)
    {
        DbContext = dbContext;
        AuthorizationService = authorizationService;
    }
    
    [HttpGet, ActionName("GetGDPRConsent")]
    public async Task<bool> GetGdprConsent(string authProviderId)
    {
        return await AuthorizationService.GetGdprStatus(authProviderId);
    }

    [HttpPost, ActionName("AcceptGDPR")]
    public async Task AcceptGdpr(string authProviderId)
    {
        await AuthorizationService.UpdateGdprStatus(authProviderId, true);
    }
    
    [HttpGet, ActionName("GetHasChosenNickname")]
    public async Task<bool> GetHasChosenNickname(string authProviderId)
    {
        return await AuthorizationService.GetHasChosenNickname(authProviderId);
    }
    
    [HttpGet, ActionName("GetNickname")]
    public async Task<string> GetNickname(string authProviderId)
    {
        return await AuthorizationService.GetNickname(authProviderId);
    }

    [HttpPost, ActionName("UpdateNickname")]
    [ProducesResponseType(200)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> UpdateNickname(string authProviderId, string nickname)
    {
        var updateResponse = await AuthorizationService.UpdateNickname(authProviderId, nickname);
        return new StatusCodeResult((int) updateResponse);
    }

    [HttpPost, ActionName("GetUsernamesMapping")]
    public async Task<Dictionary<string, string>> GetUsernamesMapping(List<string> userIds)
    {
        return await AuthorizationService.GetUsernames(userIds);
    }
}