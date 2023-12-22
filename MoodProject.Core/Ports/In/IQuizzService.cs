using MoodProject.Core.Enums;
using MoodProject.Core.Models;

namespace MoodProject.Core.Ports.In;

public interface IQuizzService
{
    public Task<bool> Submit(IEnumerable<Symptom> symptoms, IEnumerable<FactorValue> values);
    public float GetAverageValues(Symptom symptom, int max, int offset = 0, FactorType? type = null);
}