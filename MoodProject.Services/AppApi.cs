using System.Net.Http.Json;
using MoodProject.Core;
using MoodProject.Core.Ports.Out;

namespace MoodProject.Services;

public class AppApi : IAppApi
{
    private HttpClient ApiClient;
    public AppApi(HttpClient apiClient)
    {
        ApiClient = apiClient;
    }

    public async Task<IEnumerable<SymptomType>> GetSymptomsTypes()
    {
        return await ApiClient.GetFromJsonAsync<IEnumerable<SymptomType>>("SymptomsTypes/GetAll");
    }

    public async Task<IEnumerable<Symptom>> GetSymptoms(string userId)
    {
        return await ApiClient.GetFromJsonAsync<IEnumerable<Symptom>>($"Symptoms/Get?userId={userId}");
    }

    public async Task<bool> SaveSymptoms(IEnumerable<Symptom> symptoms)
    {
        var response = await ApiClient.PostAsJsonAsync($"Symptoms/Update", symptoms);
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<Symptom>> GetSymptomsWithHistory(string userId)
    {
        return await ApiClient.GetFromJsonAsync<IEnumerable<Symptom>>($"Symptoms/GetHistory?userId={userId}");
    }

    public async Task<bool> SaveSymptomsHistory(IEnumerable<FactorValue> values)
    {
        var response = await ApiClient.PostAsJsonAsync($"Symptoms/UpdateHistory", values);
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<CustomQuizzQuestion>> GetCustomQuestions()
    {
        return await ApiClient.GetFromJsonAsync<IEnumerable<CustomQuizzQuestion>>("CustomQuizzQuestions/GetAll");
    }
}