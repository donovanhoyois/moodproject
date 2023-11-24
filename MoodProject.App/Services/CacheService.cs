using Blazored.LocalStorage;
using MoodProject.App.Configuration;

namespace MoodProject.App.Services;

public class CacheService
{
    private const string INSTALLED_VERSION_KEY = "installedVersion";
    
    private string LastVersionAvailable { get; }
    private ILocalStorageService LocalStorage { get; }

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
        
        if (installedVersion == null || string.IsNullOrEmpty(installedVersion))
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