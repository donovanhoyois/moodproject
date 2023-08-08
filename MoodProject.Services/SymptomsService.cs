using MoodProject.Core;
using MoodProject.Core.Models;
using MoodProject.Core.Ports.In;
using MoodProject.Core.Ports.Out;

namespace MoodProject.Services;

public class SymptomsService : ISymptomsService
{
    private readonly IAppApi AppApi;
    public SymptomsService(IAppApi appApi)
    {
        AppApi = appApi;
    }

    public async Task<IEnumerable<Symptom>> GetSymptoms(string userId)
    {
        return await AppApi.GetSymptoms(userId);
    }

    public async Task<bool> SaveSymptoms(IEnumerable<Symptom> symptoms)
    {
        return await AppApi.SaveSymptoms(symptoms);
    }

    public async Task<IEnumerable<Symptom>> GetSymptomsWithHistory(string userId)
    {
        return await AppApi.GetSymptomsWithHistory(userId);
    }
}