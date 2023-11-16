using Blazored.LocalStorage;
using MoodProject.App.Configuration;

namespace MoodProject.App.Services;

public class CacheService
{
    private const string INSTALLED_VERSION_KEY = "installedVersion";
    
    private string LastVersionAvailable { get; init; } = string.Empty;
    private ILocalStorageService LocalStorage { get; init; }

    public CacheService(CacheConfiguration cacheConfiguration, ILocalStorageService localStorage)
    {
        LastVersionAvailable = cacheConfiguration.Version;
        LocalStorage = localStorage;
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
}