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

        var customQuestions = await AppApi.GetCustomQuestions();

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
                questions.Add(GenerateQuestion(symptom, FactorType.Presence, customQuestions));
                questions.Add(GenerateQuestion(symptom, FactorType.Harmfulness, customQuestions));
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

    private QuizzQuestion GenerateQuestion(Symptom symptom, FactorType factorType, IEnumerable<CustomQuizzQuestion> customQuizzQuestions)
    {
        var word = factorType == FactorType.Presence ? "présent" : "nuisible";
        var newQuestion = new QuizzQuestion()
        {
            Question = 
                customQuizzQuestions.FirstOrDefault(
                    q => q.SymptomType.Id.Equals(symptom.Id) && q.FactorType.Equals(factorType))
                ?? new CustomQuizzQuestion()
                {
                    Question = $"Le symptôme suivant a-t-il été {word} aujourd'hui ?",
                    Type = QuestionType.QCM,
                    FactorType = factorType
                },
            Symptom = symptom
        };
        newQuestion.Question.AnswerPossibilities = new List<QuizzAnswer>()
        {
            {new QuizzAnswer() {Text = $"Pas du tout", Weight = 0.10f}},
            {new QuizzAnswer() {Text = $"Très peu", Weight = 0.05f}},
            {new QuizzAnswer() {Text = $"Peu", Weight = 0.00f}},
            {new QuizzAnswer() {Text = $"Légèrement", Weight = -0.05f}},
            {new QuizzAnswer() {Text = $"Fortement", Weight = -0.10f}},
        };
        return newQuestion;
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