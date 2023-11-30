using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoodProject.Core;

namespace MoodProject.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]/")]
[EnableCors]
public class UsersController
{
    private readonly MoodProjectContext DbContext;
    
    public UsersController(MoodProjectContext dbContext)
    {
        DbContext = dbContext;
    }
    
    [HttpGet, ActionName("GetGDPRConsent")]
    public bool GetGdprConsent(string authProviderId)
    {
        return DbContext.Users
            .FirstOrDefault(u => u.AuthProviderUserId.Equals(authProviderId))?
            .HasAcceptedGdpr
               ?? false;
    }
}
/* 
 
                                    <!>
        KEEP CODE IN CASE THIS IS USEFUL LATER TO USE THE AUTH0 API
                                    <!>
 
 */
public static class Helper{
    
    private static async Task<string> GetToken(HttpClient httpClient)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://dev-s3pebaupby0iyub8.us.auth0.com/oauth/token");
        request.Content = new StringContent("{\"client_id\":\"HhYYUECtqYEWgaGIb7OYUn8uFPbigS5d\",\"client_secret\":\"2pJejsYtf-eXORa5lRKg8AirOJalFgHIPFEkpBKNy1vJ5BEDeE3LWQWjzi87yZdB\",\"audience\":\"https://dev-s3pebaupby0iyub8.us.auth0.com/api/v2/\",\"grant_type\":\"client_credentials\"}", Encoding.UTF8, "application/json");
        var response = await httpClient.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();
        var token = JsonSerializer.Deserialize<JsonNode>(responseString)["access_token"].ToString();
        return token;
    }
    public static async Task<string> GetUserId(string mail, HttpClient httpClient)
    {
        var token = await GetToken(httpClient);
        
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://dev-s3pebaupby0iyub8.us.auth0.com/api/v2/users-by-email?email={mail}");
        request.Headers.Add("authorization", $"Bearer {token}");
        var response = await httpClient.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();
        var userId = JsonSerializer.Deserialize<JsonNode>(responseString)[0]["identities"][0]["user_id"].ToString();
        return userId;
    }
}