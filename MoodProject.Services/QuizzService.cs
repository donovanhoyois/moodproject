using System.Net.NetworkInformation;
using MoodProject.Core.Enums;
using MoodProject.Core.Models;
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
    private const int HEALTH_AVERAGE_VALUES_COUNT = 3;
    

    private Dictionary<FactorType, decimal> FactorWeights = new Dictionary<FactorType, decimal>()
    {
        {FactorType.Presence, 1},
        {FactorType.Harmfulness, 3}
    };

    public QuizzService(IAppApi appApi)
    {
        AppApi = appApi;
    }

    public async Task<OperationResult<IEnumerable<QuizzQuestion>>> Generate(string userId)
    {
        var symptoms = await AppApi.GetSymptomsWithHistoryByUserId(userId);
        var questions = new List<QuizzQuestion>();

        if (!symptoms.Any())
        {
            return new OperationResult<IEnumerable<QuizzQuestion>>(questions, OperationResultType.Error, "Vous n'avez encore enregistré aucun symptôme sur votre profil.");
        }
        
        var customQuestions = await AppApi.GetCustomQuestions();

        foreach (var symptom in symptoms)
        {
            /* DEBUG QUIZZ TO ASK QUESTIONS EVERY TIME */
            questions.Add(GenerateQuestion(symptom, FactorType.Presence, customQuestions));
            questions.Add(GenerateQuestion(symptom, FactorType.Harmfulness, customQuestions));
            
            /*
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
            */
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

    public decimal GetAverageValues(Symptom symptom, int max = 5, int offset = 0, FactorType? type = null)
    {
        if (type != null)
        {
            if (!symptom.ValuesHistory.Any())
            {
                return 0;
            }
            return symptom.ValuesHistory
                .Where(v => v.Type.Equals(type))
                .Skip(offset)
                .Take(max)
                .Average(v => v.Value);
        }

        var values = symptom.ValuesHistory.Skip(offset*2).Take(max).ToList();
        var sum = 0m;
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
    

    public decimal? GetHealthAverage(IEnumerable<Symptom> symptoms, int offset = 0)
    {
        var averages = new List<decimal>();
        foreach (var symptom in symptoms)
        {
            averages.Add(GetAverageValues(symptom, HEALTH_AVERAGE_VALUES_COUNT, offset: offset));
        }

        
        return averages.Count > 0 ? (decimal) averages.Average() : null;
    }

    public OperationResult<decimal?> GetHealthAverageAsPercentage(IEnumerable<Symptom> symptoms, int offset = 0)
    {
        var average = GetHealthAverage(symptoms, offset);
        if (average == null)
        {
            var op = new OperationResult<decimal?>(
                null,
                OperationResultType.Error,
                "Pas assez de données disponibles pour établir une moyenne santé.");
            return op;
        }

        Console.WriteLine($"Health average: {average}");
        return new OperationResult<decimal?>(50 + average*50, OperationResultType.Ok);
    }

    public IEnumerable<decimal?> GetHealthAverageHistory(IEnumerable<Symptom> symptoms, int depth = 10)
    {
        var valuesHistory = new List<decimal?>();
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

    private decimal GetRequiredDaysBeforeNextQuizz(Symptom symptom)
    {
        
        //TODO: adapt to decimal
        var average = GetAverageValues(symptom);
        var daysRequired = Math.Round(4 * Math.Sqrt(1 + average), MidpointRounding.ToZero);
        return daysRequired == null ? 0 : daysRequired;
    }

    private QuizzQuestion GenerateQuestion(Symptom symptom, FactorType factorType, IEnumerable<CustomQuizzQuestion> customQuizzQuestions)
    {
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
            var average = GetAverageValues(symptoms.FirstOrDefault(s => s.Id.Equals(value.SymptomId)), VALUES_DERIVATION_LENGTH, type: value.Type);
            value.Value += average;
        }
        return newValues;
    }
}