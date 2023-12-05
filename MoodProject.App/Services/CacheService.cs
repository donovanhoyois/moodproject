using Blazored.LocalStorage;
using MoodProject.App.Configuration;
using MoodProject.Core.Ports.In;

namespace MoodProject.App.Services;

public class CacheService
{
    private const string INSTALLED_VERSION_KEY = "installedVersion";
    private const string HAS_ACCEPTED_GDPR_KEY = "gdprPolicy";
    
    private string LastVersionAvailable { get; init; } = string.Empty;
    private bool HasAcceptedGdpr { get; set; } = false;
    private ILocalStorageService LocalStorage { get; init; }
    private IUsersService UsersService { get; init; }

    public CacheService(CacheConfiguration cacheConfiguration, ILocalStorageService localStorage, IUsersService usersService)
    {
        LastVersionAvailable = cacheConfiguration.Version;
        LocalStorage = localStorage;
        UsersService = usersService;
    }

    public string GetLastVersionAvailable()
    {
        return LastVersionAvailable;
    }

    public async Task<string> GetInstalledVersion()
    {
        var installedVersion = await LocalStorage.GetItemAsync<string>(INSTALLED_VERSION_KEY);
        
        if (installedVersion == null || installedVersion.Equals(string.Empty))
        {
            await LocalStorage.SetItemAsync(INSTALLED_VERSION_KEY, LastVersionAvailable);
            installedVersion = LastVersionAvailable;
        }
        
        return installedVersion;
    }
    
    public async Task Update(){
        await LocalStorage.SetItemAsync(INSTALLED_VERSION_KEY, LastVersionAvailable);
    }

    public async Task<bool> GetHasAcceptedGdpr(string authProviderId)
    {
        var hasAcceptedGdpr = await LocalStorage.GetItemAsync<bool>(HAS_ACCEPTED_GDPR_KEY);
        if (!hasAcceptedGdpr && authProviderId != string.Empty)
        {
            hasAcceptedGdpr = await UsersService.GetGdprConsent(authProviderId);
        }
        await LocalStorage.SetItemAsync(HAS_ACCEPTED_GDPR_KEY, hasAcceptedGdpr);
        HasAcceptedGdpr = hasAcceptedGdpr;
        return HasAcceptedGdpr;
    }

    public async Task UpdateHasAcceptedGdpr(bool newValue)
    {
        HasAcceptedGdpr = newValue;
        await LocalStorage.SetItemAsync(HAS_ACCEPTED_GDPR_KEY, newValue);
    }
}