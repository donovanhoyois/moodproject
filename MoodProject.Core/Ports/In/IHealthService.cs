using MoodProject.Core.Models;

namespace MoodProject.Core.Ports.In;

public interface IHealthService
{
    public float GetHealthAverage(IEnumerable<Symptom> symptoms, int offset = 0);
    public IEnumerable<float> GetHealthAverageHistory(IEnumerable<Symptom> symptoms, int depth);
    public OperationResult<float> GetHealthAverageAsPercentage(IEnumerable<Symptom> symptoms, int offset = 0);
}