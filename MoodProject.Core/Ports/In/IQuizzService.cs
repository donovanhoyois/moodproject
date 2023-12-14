using MoodProject.Core.Enums;
using MoodProject.Core.Models;

namespace MoodProject.Core.Ports.In;

public interface IQuizzService
{
    public Task<OperationResult<IEnumerable<QuizzQuestion>>> Generate(string userId);
    public Task<bool> Submit(IEnumerable<Symptom> symptoms, IEnumerable<FactorValue> values);
    public float GetAverageValues(Symptom symptom, int max, int offset = 0, FactorType? type = null);
    public float GetHealthAverage(IEnumerable<Symptom> symptoms, int offset = 0);
    public IEnumerable<float> GetHealthAverageHistory(IEnumerable<Symptom> symptoms, int depth);
    public OperationResult<float> GetHealthAverageAsPercentage(IEnumerable<Symptom> symptoms, int offset = 0);
}