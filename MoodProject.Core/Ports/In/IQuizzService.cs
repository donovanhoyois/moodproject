using MoodProject.Core.Enums;
using MoodProject.Core.Models;

namespace MoodProject.Core.Ports.In;

public interface IQuizzService
{
    public Task<OperationResult<IEnumerable<QuizzQuestion>>> Generate(string userId);
    public Task<bool> Submit(IEnumerable<Symptom> symptoms, IEnumerable<FactorValue> values);
    public decimal GetAverageValues(Symptom symptom, int max, int offset = 0, FactorType? type = null);
    public decimal? GetHealthAverage(IEnumerable<Symptom> symptoms, int offset = 0);
    public IEnumerable<decimal?> GetHealthAverageHistory(IEnumerable<Symptom> symptoms, int depth);
    public OperationResult<decimal?> GetHealthAverageAsPercentage(IEnumerable<Symptom> symptoms, int offset = 0);
}