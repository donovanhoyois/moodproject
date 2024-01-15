using MoodProject.Core.Enums;
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
        return await AppApi.GetSymptomsByUserId(userId);
    }

    public async Task<OperationResult<bool>> SaveSymptoms(IEnumerable<Symptom> symptoms)
    {
        var apiResponse = await AppApi.SaveSymptoms(symptoms);
        return apiResponse
            ? new OperationResult<bool>(OperationResultType.Ok)
            {
                Message = "Vos symptômes ont bien été mis à jour."
            }
            : new OperationResult<bool>(OperationResultType.Error)
            {
                Message = "Une erreur est survenue lors de la mise à jour de vos symptômes."
            };
    }

    public async Task<IEnumerable<Symptom>> GetSymptomsWithHistory(string userId)
    {
        return await AppApi.GetSymptomsWithHistoryByUserId(userId);
    }
}