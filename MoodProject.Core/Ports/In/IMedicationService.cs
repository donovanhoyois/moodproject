using MoodProject.Core.Models;

namespace MoodProject.Core.Ports.In;

public interface IMedicationService
{
    public Task<OperationResult<IEnumerable<Medication>>> GetMedicationsOfUser(string userId);
    public Task<OperationResult<IEnumerable<Medication>>> UpdateMedications(IEnumerable<Medication> medications);
    public Task<OperationResult<Medication>> UpdateMedication(Medication medication);
}