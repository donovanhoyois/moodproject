using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using MoodProject.Api.Configuration;
using MoodProject.Api.Schemes;
using MoodProject.Api.Services.Builder;
using MoodProject.Core.Models.User;

namespace MoodProject.Api.Services;

public class AuthorizationService
{
    private ILogger<AuthorizationService> Logger { get; init; }
    private HttpClient HttpClient { get; init; }
    private Uri TokenUrl { get; init; }
    private Uri BaseUrl { get; init; }
    private AuthorityConfiguration Configuration { get; init; }
    private Auth0Token? Auth0Token { get; set; }
    private DateTime? LastTokenRequest { get; set; }

    public AuthorizationService(ILogger<AuthorizationService> logger, HttpClient httpClient, AuthorityConfiguration authorityConfiguration)
    {
        Logger = logger;
        HttpClient = httpClient;
        TokenUrl = new Uri(authorityConfiguration.TokenUrl);
        BaseUrl = new Uri(authorityConfiguration.BaseUrl);
        Configuration = authorityConfiguration;
    }
    private async Task RefreshToken()
    {
        if (Auth0Token == null || LastTokenRequest == null || LastTokenRequest.Value.Add(TimeSpan.FromSeconds(Auth0Token.ExpiresIn)) < DateTime.Now)
        {
            var tokenRequest = new Auth0TokenRequest()
            {
                ClientId = Configuration.ClientId,
                ClientSecret = Configuration.ClientSecret,
                Audience = Configuration.Audience,
                GrantType = Configuration.GrantType
            };
            var response = await HttpClient.PostAsJsonAsync(TokenUrl, tokenRequest);
            Auth0Token = JsonSerializer.Deserialize<Auth0Token>(await response.Content.ReadAsStringAsync());
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Auth0Token.Token);
            LastTokenRequest = DateTime.Now;
        }
    }

    public async Task<bool> GetGdprStatus(string userId)
    {
        await RefreshToken();
        
        var user = await HttpClient.GetFromJsonAsync<User>($"{BaseUrl}/users/{userId}?fields=user_metadata");
        return user?.Metadata?.HasAcceptedGdpr ?? false;
    }

    public async Task UpdateGdprStatus(string userId, bool newStatus)
    {
        await RefreshToken();
        
        var metadata = new UserMetadataBuilder()
            .WithHasAcceptedGdpr(newStatus)
            .Build();

        var user = new UserBuilder()
            .WithMetadata(metadata);
        
        await HttpClient.PatchAsJsonAsync($"{BaseUrl}/users/{userId}", user);
    }

    public async Task<string> GetNickname(string userId)
    {
        await RefreshToken();
        
        var user = await HttpClient.GetFromJsonAsync<User>($"{BaseUrl}/users/{userId}?fields=nickname");
        return user.Nickname;
    }

    public async Task<bool> GetHasChosenNickname(string userId)
    {
        await RefreshToken();
        
        var userMetadata = await HttpClient.GetFromJsonAsync<User>($"{BaseUrl}/users/{userId}?fields=user_metadata");
        return userMetadata?.Metadata?.HasChosenNickname ?? false;
    }

    public async Task<HttpStatusCode> UpdateNickname(string userId, string nickname)
    {
        await RefreshToken();

        if (nickname != "x")
        {
            var nicknameAlreadyExists = await NicknameAlreadyExists(nickname);
            if (nicknameAlreadyExists)
            {
                return HttpStatusCode.Forbidden;
            }
        }

        var metadata = new UserMetadataBuilder()
            .WithHasChosenNickname(true)
            .Build();

        var user = new UserBuilder()
            .WithNickname(nickname)
            .WithMetadata(metadata)
            .Build();

        var r = await HttpClient.PatchAsJsonAsync($"{BaseUrl}/users/{userId}", user);
        return HttpStatusCode.OK;
    }
    
    public async Task<Dictionary<string, string>> GetUsernames(List<string> userIds)
    {
        await RefreshToken();

        Dictionary<string, string> maps = new();
        var luceneQuery = new LuceneQueryBuilder()
            .WithFieldName("user_id")
            .WithFieldValues(userIds)
            .WithLogicalOperator("OR")
            .Build();
        try
        {
            var usernamesList = await HttpClient.GetFromJsonAsync<List<User>>($"{BaseUrl}/users?fields=nickname,identities&q={luceneQuery}") ?? new List<User>();
            foreach (var username in usernamesList)
            {
                foreach (var identity in username.Identities)
                {
                    maps.Add($"{identity.Provider}|{identity.UserId}", username.Nickname);
                }
            }

            if (usernamesList.IsNullOrEmpty())
            {
                Logger.LogError("Error while retrieving mapping between user ids & nickname from Auth0: List returned is empty.");
            }

            if (usernamesList.Count != userIds.Count)
            {
                Logger.LogWarning("Not all usernames have been retrieved ({}/{}).", usernamesList.Count, userIds.Count);
            }
        }
        catch (Exception e)
        {
            Logger.LogError("Error while retrieving mapping between user ids & nickname from Auth0: {}", e);
        }
        
        return maps;
    }

    private async Task<bool> NicknameAlreadyExists(string nickname)
    {
        var luceneQuery = new LuceneQueryBuilder()
            .WithFieldName("nickname")
            .WithFieldValues(new List<string>() { nickname })
            .Build();
        
        var response = await HttpClient.GetFromJsonAsync<List<User>>($"{BaseUrl}/users?fields=nickname&q={luceneQuery}");
        return !response.IsNullOrEmpty();
    }
}