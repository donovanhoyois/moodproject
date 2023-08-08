using MoodProject.Core.Enums;
using MoodProject.Core.Models;

namespace MoodProject.Core.Ports.In;

public interface IQuizzService
{
    public Task<OperationResult<IEnumerable<QuizzQuestion>>> Generate(string userId);
    public Task<bool> Submit(IEnumerable<Symptom> symptoms, IEnumerable<FactorValue> values);
    public float GetAverageValues(Symptom symptom, int max, FactorType? type = null);
    public float GetAverageValues(Symptom symptom, DateTime from, FactorType? type = null);
    public float GetHealthAverage(IEnumerable<Symptom> symptoms, int days);
    public IEnumerable<float> GetHealthAverageHistory();
    public OperationResult<float> GetHealthAverageAsPercentage(IEnumerable<Symptom> symptoms, int days);
}