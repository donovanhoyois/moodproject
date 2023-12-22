using MoodProject.Core.Enums;
using MoodProject.Core.Models;
using MoodProject.Core.Ports.In;
using MoodProject.Core.Ports.Out;
using MoodProject.Services.Configuration;

namespace MoodProject.Services;

public class QuizzService : IQuizzService
{
    private readonly IAppApi AppApi;
    
    private const float MIN_FACTOR_VALUE = 0f;
    private const float MAX_FACTOR_VALUE = 100f;
    private const int VALUES_DERIVATION_LENGTH = 4;
    private const float TENDENCY_WEIGHT = 0.1f;
    

    private Dictionary<FactorType, float> FactorWeights = new Dictionary<FactorType, float>
    {
        {FactorType.Presence, 1},
        {FactorType.Harmfulness, 3}
    };

    public QuizzService(IAppApi appApi)
    {
        AppApi = appApi;
    }

    public async Task<bool> Submit(IEnumerable<Symptom> symptoms,IEnumerable<FactorValue> values)
    {
        values = DerivateNewValues(symptoms, values);
        return await AppApi.SaveSymptomsHistory(values);
    }

    public float GetAverageValues(Symptom symptom, int max = 5, int offset = 0, FactorType? type = null)
    {
        if (type != null)
        {
            if (!symptom.ValuesHistory.Any())
            {
                return (MIN_FACTOR_VALUE + MAX_FACTOR_VALUE) / 2f;
            }
            return symptom.ValuesHistory
                .Where(v => v.Type.Equals(type))
                .Skip(offset)
                .Take(max)
                .Average(v => v.Value);
        }

        var values = symptom.ValuesHistory.Skip(offset*2).Take(max).ToList();
        var sum = 0f;
        foreach (var weight in FactorWeights)
        {
            var factorTypeValues =
                symptom.ValuesHistory
                    .Where(v => v.Type.Equals(weight.Key))
                    .Skip(offset)
                    .Take(max)
                    .ToList();
            if (factorTypeValues.Any())
            {
                sum += factorTypeValues.Sum(v => v.Value) * weight.Value;
            }
        }

        var average = sum / values.Count() / 4;
        return average;
    }

    private IEnumerable<FactorValue> DerivateNewValues(IEnumerable<Symptom> symptoms, IEnumerable<FactorValue> newValues)
    {
        foreach (var value in newValues)
        {
            var average = GetAverageValues(symptoms.FirstOrDefault(s => s.Id.Equals(value.SymptomId)), 1, type: value.Type);
            value.Value += average;
            var tendency = GetTendency(symptoms.FirstOrDefault(s => s.Id.Equals(value.SymptomId)), value.Type, value, VALUES_DERIVATION_LENGTH);
            value.Value += (tendency * MAX_FACTOR_VALUE / 10) * TENDENCY_WEIGHT;

            if (value.Value < MIN_FACTOR_VALUE)
            {
                value.Value = MIN_FACTOR_VALUE;
            }

            if (value.Value > MAX_FACTOR_VALUE)
            {
                value.Value = MAX_FACTOR_VALUE;
            }
        }
        return newValues;
    }

    private int GetTendency(Symptom symptom, FactorType factorType, FactorValue newValue, int max = 5)
    {
        var tendency = 0;
        var factorValues = symptom.ValuesHistory.Where(value => value.Type.Equals(factorType)).ToList();
        FactorValue? lastElement = null;
        for (var i = 0; i < max && i < factorValues.Count; i++)
        {
            if (i > 0)
            {
                if (factorValues[i].Value < factorValues[i - 1].Value)
                {
                    tendency += 1;
                }
                else if (factorValues[i].Value > factorValues[i - 1].Value)
                {
                    tendency += -1;
                }
            }
            lastElement = factorValues[i];
        }
        
        
        if (lastElement != null)
        {
            tendency += newValue.Value > lastElement.Value ? 1 : -1;
        }
        
        return tendency;
    }
}