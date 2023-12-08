using MoodProject.Core.Enums;
using MoodProject.Core.Models;
using MoodProject.Core.Ports.In;
using MoodProject.Core.Ports.Out;

namespace MoodProject.Services;

public class MedicationService : IMedicationService
{
    private readonly IAppApi AppApi;

    public MedicationService(IAppApi appApi)
    {
        AppApi = appApi;
    }
    
    public async Task<OperationResult<IEnumerable<Medication>>> GetMedicationsOfUser(string userId)
    {
        var apiResponse = await AppApi.GetMedicationsByUserId(userId);
        return apiResponse != null
            ? new OperationResult<IEnumerable<Medication>>(OperationResultType.Ok)
            {
                Content = apiResponse
            }
            : new OperationResult<IEnumerable<Medication>>(OperationResultType.Error)
            {
                Message = "Une erreur est survenue lors de la récupération des médicaments."
            };
    }

    public async Task<OperationResult<IEnumerable<Medication>>> UpdateMedications(IEnumerable<Medication> medications)
    {
        var apiResponse = await AppApi.UpdateMedications(medications);
        return apiResponse
            ? new OperationResult<IEnumerable<Medication>>(OperationResultType.Ok)
            {
                Content = medications,
                Message = "Les médicaments ont bien été mis à jour."
            }
            : new OperationResult<IEnumerable<Medication>>(OperationResultType.Error)
            {
                Content = medications,
                Message = "Une erreur est survenue lors de la mise à jour des médicaments."
            };
    }

    public async Task<OperationResult<Medication>> UpdateMedication(Medication medication)
    {
        Console.WriteLine(medication.Name);
        var apiResponse = await AppApi.UpdateMedications(new List<Medication>(){ medication });
        return apiResponse
            ? new OperationResult<Medication>(OperationResultType.Ok)
            {
                Content = medication,
                Message = "Les médicaments ont bien été mis à jour."
            }
            : new OperationResult<Medication>(OperationResultType.Error)
            {
                Content = medication,
                Message = "Une erreur est survenue lors de la mise à jour des médicaments."
            };
    }
}