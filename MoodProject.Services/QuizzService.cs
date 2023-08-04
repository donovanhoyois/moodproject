using MoodProject.Core;
using MoodProject.Core.Ports.In;
using MoodProject.Core.Ports.Out;

namespace MoodProject.Services;

public class QuizzService : IQuizzService
{
    private readonly IAppApi AppApi;
    private const int MAX_DAYS_SINCE_LAST_QUIZZ = 7;
    private const int MIN_DAYS_SINCE_LAST_QUIZZ = 1;
    private const float MAX_POSITIVE_VALUE = 1f;
    private const float MAX_NEGATIVE_VALUE = -1f;
    private const int VALUES_DERIVATION_LENGTH = 3;
    

    private Dictionary<FactorType, float> FactorWeights = new Dictionary<FactorType, float>()
    {
        {FactorType.Presence, 1},
        {FactorType.Harmfulness, 3}
    };

    public QuizzService(IAppApi appApi)
    {
        AppApi = appApi;
    }

    public async Task<IEnumerable<QuizzQuestion>?> Generate(string userId)
    {
        var symptoms = await AppApi.GetSymptomsWithHistory(userId);
        var questions = new List<QuizzQuestion>();

        foreach (var symptom in symptoms)
        {
            
            // Min & Max days since last quizz
            var lastValue = symptom.ValuesHistory.FirstOrDefault();
            var daysSinceLastValue = DateTime.Now - lastValue.Timestamp;
            switch (daysSinceLastValue.TotalDays)
            {
                case < MIN_DAYS_SINCE_LAST_QUIZZ:
                    return null;
            }
            
            var daysRequiredBeforeNextQuizz = GetRequiredDaysBeforeNextQuizz(symptom);
            if (daysSinceLastValue.TotalDays >= daysRequiredBeforeNextQuizz)
            {
                questions.Add(GenerateQuestion(symptom, FactorType.Presence));
                questions.Add(GenerateQuestion(symptom, FactorType.Harmfulness));
            }
        }

        return questions;
    }

    public async Task<bool> Submit(IEnumerable<Symptom> symptoms,IEnumerable<FactorValue> values)
    {
        values = DerivateNewValues(symptoms, values);
        return await AppApi.SaveSymptomsHistory(values);
    }

    private float GetAverageValues(Symptom symptom, int max, FactorType? type = null)
    {
        if (type != null)
        {
            return symptom.ValuesHistory
                .Where(v => v.Type.Equals(type))
                .Take(max)
                .Average(v => v.Value);
        }
        else
        {
            var values = symptom.ValuesHistory.Take(max);
            var sum = 0f;
            foreach (var weight in FactorWeights)
            {
                var factorTypeValues = values.Where(v => v.Type.Equals(weight.Key));
                if (factorTypeValues.Any())
                {
                    sum += factorTypeValues.Sum(v => v.Value) * weight.Value;
                }
            }

            var average = sum / values.Count();
            return average;
        }
    }

    private double GetRequiredDaysBeforeNextQuizz(Symptom symptom)
    {
        var average = GetAverageValues(symptom, 5);
        var daysRequired = Math.Round(4 * Math.Sqrt(1 + average), MidpointRounding.ToZero);
        return daysRequired;
    }

    private QuizzQuestion GenerateQuestion(Symptom symptom, FactorType factorType)
    {
        // TODO: Get questions from db
        var word = factorType == FactorType.Presence ? "présent" : "nuisible";
        return new QuizzQuestion()
        {
            Question = $"Le symptôme suivant est-il {word} aujourd'hui ?",
            Type = QuestionType.QCM,
            Symptom = symptom,
            FactorType = factorType,
            AnswerPossibilities = new List<QuizzAnswer>()
            {
                { new QuizzAnswer(){ Text = $"Très peu {word}", Weight = 0.1f } },
                { new QuizzAnswer(){ Text = $"Peu {word}", Weight = 0.05f } },
                { new QuizzAnswer(){ Text = $"Légèrement {word}", Weight = -0.05f } },
                { new QuizzAnswer(){ Text = $"Assez {word}", Weight = -0.1f } },
                { new QuizzAnswer(){ Text = $"Très {word}", Weight = -0.15f } },
            }
        };
    }

    private IEnumerable<FactorValue> DerivateNewValues(IEnumerable<Symptom> symptoms, IEnumerable<FactorValue> newValues)
    {
        foreach (var value in newValues)
        {
            var average = GetAverageValues(symptoms.FirstOrDefault(s => s.Id.Equals(value.SymptomId)), VALUES_DERIVATION_LENGTH, value.Type);
            value.Value += average;
        }
        return newValues;
    }
}