using System.Net.Http.Json;
using MoodProject.Core;
using MoodProject.Core.Ports.Out;

namespace MoodProject.Services;

public class AppApi : IAppApi
{
    private HttpClient _apiClient { get; init; }
    public AppApi(HttpClient apiClient)
    {
        _apiClient = apiClient;
    }
    public async Task<IEnumerable<Symptom>> LoadSymptomsHistory(User user)
    {
        return await _apiClient.GetFromJsonAsync<IEnumerable<Symptom>>($"GetSymptomsHistory?userkey={user.Mail}");
    }

    public Task SaveSymptomsHistory()
    {
        throw new NotImplementedException();
    }
}