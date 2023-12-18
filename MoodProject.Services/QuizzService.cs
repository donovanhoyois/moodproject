using System.Net.NetworkInformation;
using Microsoft.Extensions.Logging;
using MoodProject.Core.Enums;
using MoodProject.Core.Models;
using MoodProject.Core.Ports.In;
using MoodProject.Core.Ports.Out;
using MoodProject.Services.Configuration;

namespace MoodProject.Services;

public class QuizzService : IQuizzService
{
    private readonly IAppApi AppApi;
    private readonly QuizzConfiguration QuizzConfiguration;
    private const int MAX_DAYS_SINCE_LAST_QUIZZ = 7;
    private const int MIN_DAYS_SINCE_LAST_QUIZZ = 1;
    private const float MIN_FACTOR_VALUE = 0f;
    private const float MAX_FACTOR_VALUE = 100f;
    private const int VALUES_DERIVATION_LENGTH = 4;
    private const int HEALTH_AVERAGE_VALUES_COUNT = 1;
    private const float TENDENCY_WEIGHT = 0.1f;
    

    private Dictionary<FactorType, float> FactorWeights = new Dictionary<FactorType, float>()
    {
        {FactorType.Presence, 1},
        {FactorType.Harmfulness, 3}
    };

    public QuizzService(IAppApi appApi, QuizzConfiguration quizzConfiguration)
    {
        AppApi = appApi;
        QuizzConfiguration = quizzConfiguration;
    }

    public async Task<OperationResult<IEnumerable<QuizzQuestion>>> Generate(string userId)
    {
        var symptoms = await AppApi.GetSymptomsWithHistoryByUserId(userId);
        var questions = new List<QuizzQuestion>();

        if (!symptoms.Any())
        {
            return new OperationResult<IEnumerable<QuizzQuestion>>(questions, OperationResultType.Error, "Vous n'avez encore enregistré aucun symptôme sur votre profil.");
        }
        
        var customQuestions = (await AppApi.GetCustomQuestions()).ToList();

        foreach (var symptom in symptoms)
        {
            Console.WriteLine("Config: "+QuizzConfiguration.IgnoreMinDaysToGenerateQuizz);
            if (QuizzConfiguration.IgnoreMinDaysToGenerateQuizz)
            {
                questions.Add(GenerateQuestion(symptom, FactorType.Presence, customQuestions));
                questions.Add(GenerateQuestion(symptom, FactorType.Harmfulness, customQuestions));
            }
            else
            {
                // Min & Max days since last quizz
                var lastValue = symptom.ValuesHistory.FirstOrDefault();
                var daysSinceLastValue = DateTime.Now - lastValue?.Timestamp ?? TimeSpan.MaxValue;
                switch (daysSinceLastValue.TotalDays)
                {
                    case < MIN_DAYS_SINCE_LAST_QUIZZ:
                        return new OperationResult<IEnumerable<QuizzQuestion>>(null, OperationResultType.Error,
                            "Vous avez déjà répondu à votre questionnaire aujourd'hui.");
                }
            
                var daysRequiredBeforeNextQuizz = GetRequiredDaysBeforeNextQuizz(symptom);
                if (daysSinceLastValue.TotalDays >= daysRequiredBeforeNextQuizz)
                {
                    questions.Add(GenerateQuestion(symptom, FactorType.Presence, customQuestions));
                    questions.Add(GenerateQuestion(symptom, FactorType.Harmfulness, customQuestions));
                }
            }
        }

        return questions.Any()
            ? new OperationResult<IEnumerable<QuizzQuestion>>(questions, OperationResultType.Ok)
            : new OperationResult<IEnumerable<QuizzQuestion>>(null, OperationResultType.Error,
                "Aucun questionnaire n'est disponible pour vous aujourd'hui !");
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
    

    public float GetHealthAverage(IEnumerable<Symptom> symptoms, int offset = 0)
    {
        var averages = new List<float>();
        foreach (var symptom in symptoms)
        {
            averages.Add(GetAverageValues(symptom, HEALTH_AVERAGE_VALUES_COUNT, offset: offset));
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

    private double GetRequiredDaysBeforeNextQuizz(Symptom symptom)
    {
        var average = GetAverageValues(symptom);
        var daysRequired = Math.Round(4 * Math.Sqrt(1 + average), MidpointRounding.ToZero);
        return double.IsNaN(daysRequired) ? 0 : daysRequired;
    }

    private QuizzQuestion GenerateQuestion(Symptom symptom, FactorType factorType, IEnumerable<CustomQuizzQuestion> customQuizzQuestions, QuestionType? questionType = null)
    {
        if (questionType == null)
        {
            var r = new Random().Next(Enum.GetNames(typeof(QuestionType)).Length+1);
            questionType = (QuestionType)r;
        }
        //TODO: implement other questions types

        Console.WriteLine(questionType);
        var word = factorType == FactorType.Presence ? "présent" : "nuisible";
        var newQuestion = new QuizzQuestion()
        {
            CustomQuestion = 
                customQuizzQuestions.FirstOrDefault(
                    q => q.SymptomType.Id.Equals(symptom.Id) && q.FactorType.Equals(factorType))
                ?? new CustomQuizzQuestion()
                {
                    Text = $"Le symptôme suivant a-t-il été {word} aujourd'hui ?",
                    Type = QuestionType.QCM,
                    FactorType = factorType
                },
            Symptom = symptom
        };
        newQuestion.CustomQuestion.AnswerPossibilities = new List<QuizzAnswer>()
        {
            {new QuizzAnswer() {Text = $"Pas du tout", Weight = 6f}},
            {new QuizzAnswer() {Text = $"Très peu", Weight = 4f}},
            {new QuizzAnswer() {Text = $"Peu", Weight = 0f}},
            {new QuizzAnswer() {Text = $"Légèrement", Weight = -4f}},
            {new QuizzAnswer() {Text = $"Fortement", Weight = -6f}},
        };
        return newQuestion;
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