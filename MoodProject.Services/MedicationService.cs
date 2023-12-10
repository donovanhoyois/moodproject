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
        if (medication.Name.Equals(string.Empty))
        {
            return new OperationResult<Medication>(OperationResultType.Error)
            {
                Content = medication,
                Message = "Le nom du médicament ne peut pas être vide."
            };
        }
        
        var apiResponse = await AppApi.UpdateMedications(new List<Medication>(){ medication });
        return apiResponse
            ? new OperationResult<Medication>(OperationResultType.Ok)
            {
                Content = medication,
                Message = medication.Id == 0 ? "Le médicament a bien été créé." : "Le médicament a bien été mis à jour."
            }
            : new OperationResult<Medication>(OperationResultType.Error)
            {
                Content = medication,
                Message = "Une erreur est survenue lors de la mise à jour du médicament."
            };
    }

    public async Task<OperationResult<IEnumerable<Medication>>> DeleteMedications(IEnumerable<Medication> medications)
    {
        var apiResponse = await AppApi.DeleteMedications(medications);
        return apiResponse
            ? new OperationResult<IEnumerable<Medication>>(OperationResultType.Ok)
            {
                Content = medications,
                Message = "Les médicaments ont bien été supprimés."
            }
            : new OperationResult<IEnumerable<Medication>>(OperationResultType.Error)
            {
                Content = medications,
                Message = "Une erreur est survenue lors de la suppression des médicaments."
            };
    }

    public async Task<OperationResult<Medication>> DeleteMedication(Medication medication)
    {
        var apiResponse = await AppApi.DeleteMedications(new List<Medication>(){ medication });
        return apiResponse
            ? new OperationResult<Medication>(OperationResultType.Ok)
            {
                Content = medication,
                Message = "Le médicament a bien été supprimé."
            }
            : new OperationResult<Medication>(OperationResultType.Error)
            {
                Content = medication,
                Message = "Une erreur est survenue lors de la suppression du médicament."
            };
    }
}