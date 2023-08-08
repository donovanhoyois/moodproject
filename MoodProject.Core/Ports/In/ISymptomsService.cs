using MoodProject.Core.Models;

namespace MoodProject.Core.Ports.In;

public interface ISymptomsService
{
    public Task<IEnumerable<Symptom>> GetSymptoms(string userId);
    public Task<bool> SaveSymptoms(IEnumerable<Symptom> symptoms);
    public Task<IEnumerable<Symptom>> GetSymptomsWithHistory(string userId);
}