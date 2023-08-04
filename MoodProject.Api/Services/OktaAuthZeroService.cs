using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using MoodProject.Core;
using MoodProject.Core.Ports.In;
using StringContent = System.Net.Http.StringContent;

namespace MoodProject.Services;

public class OktaAuthZeroService : IAuthService
{
    private HttpClient _httpClient;
    public OktaAuthZeroService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private async Task<string> GetToken()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://dev-s3pebaupby0iyub8.us.auth0.com/oauth/token");
        request.Content = new StringContent("{\"client_id\":\"HhYYUECtqYEWgaGIb7OYUn8uFPbigS5d\",\"client_secret\":\"2pJejsYtf-eXORa5lRKg8AirOJalFgHIPFEkpBKNy1vJ5BEDeE3LWQWjzi87yZdB\",\"audience\":\"https://dev-s3pebaupby0iyub8.us.auth0.com/api/v2/\",\"grant_type\":\"client_credentials\"}", Encoding.UTF8, "application/json");
        var response = await _httpClient.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();
        var token = JsonSerializer.Deserialize<JsonNode>(responseString)["access_token"].ToString();
        return token;
    }
    public async Task<string> GetUserId(string mail)
    {
        var token = await GetToken();
        
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://dev-s3pebaupby0iyub8.us.auth0.com/api/v2/users-by-email?email={mail}");
        request.Headers.Add("authorization", $"Bearer {token}");
        var response = await _httpClient.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();
        var userId = JsonSerializer.Deserialize<JsonNode>(responseString)[0]["identities"][0]["user_id"].ToString();
        return userId;
    }
}