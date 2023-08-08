using MoodProject.Core.Models;

namespace MoodProject.Core.Ports.Out;

public interface IAppApi
{
    public Task<IEnumerable<SymptomType>> GetSymptomsTypes();
    public Task<IEnumerable<Symptom>> GetSymptoms(string userId);
    public Task<bool> SaveSymptoms(IEnumerable<Symptom> symptoms);
    public Task<IEnumerable<Symptom>> GetSymptomsWithHistory(string userId);
    public Task<bool> SaveSymptomsHistory(IEnumerable<FactorValue> values);
    public Task<IEnumerable<CustomQuizzQuestion>> GetCustomQuestions();
}