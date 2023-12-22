using MoodProject.Core.Enums;
using MoodProject.Core.Models;
using MoodProject.Core.Ports.In;

namespace MoodProject.Services;

public class HealthService : IHealthService
{
    private readonly IQuizzService QuizzService;
    private const int HEALTH_AVERAGE_VALUES_COUNT = 1;
    
    public HealthService(IQuizzService quizzService)
    {
        QuizzService = quizzService;
    }
    public float GetHealthAverage(IEnumerable<Symptom> symptoms, int offset = 0)
    {
        var averages = new List<float>();
        foreach (var symptom in symptoms)
        {
            averages.Add(QuizzService.GetAverageValues(symptom, HEALTH_AVERAGE_VALUES_COUNT, offset: offset));
        }

        
        return averages.Count > 0 ? averages.Average() : Single.NaN;
    }

    public OperationResult<float> GetHealthAverageAsPercentage(IEnumerable<Symptom> symptoms, int offset = 0)
    {
        var average = GetHealthAverage(symptoms, offset);
        if (float.IsNaN(average))
        {
            var op = new OperationResult<float>(
                float.NaN,
                OperationResultType.Error,
                "Pas assez de données disponibles pour établir une moyenne santé.");
            return op;
        }

        return new OperationResult<float>(average, OperationResultType.Ok);
    }

    public IEnumerable<float> GetHealthAverageHistory(IEnumerable<Symptom> symptoms, int depth = 10)
    {
        var valuesHistory = new List<float>();
        var maxHistoryDepth = symptoms.Select(s => s.ValuesHistory.Count()).Max();
        for (var i = 0; i < maxHistoryDepth && i < depth; i++)
        {
            var v = GetHealthAverageAsPercentage(symptoms, offset: i);
            if (v.Status.Equals(OperationResultType.Ok))
            {
                valuesHistory.Add(v.Content);
            }
            else
            {
                break;
            }
        }

        return valuesHistory;
    }
}