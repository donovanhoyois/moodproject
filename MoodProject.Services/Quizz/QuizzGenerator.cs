using MoodProject.Core.Enums;
using MoodProject.Core.Models;
using MoodProject.Core.Ports.In;
using MoodProject.Core.Ports.Out;
using MoodProject.Services.Configuration;

namespace MoodProject.Services;

public class QuizzGenerator : IQuizzGenerator
{
    private IAppApi AppApi;
    private QuizzConfiguration QuizzConfiguration;
    private IQuizzService QuizzService;
    
    public QuizzGenerator(IAppApi appApi, QuizzConfiguration quizzConfiguration, IQuizzService quizzService)
    {
        AppApi = appApi;
        QuizzConfiguration = quizzConfiguration;
        QuizzService = quizzService;
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
            if (QuizzConfiguration.IgnoreMinDaysToGenerateQuizz)
            {
                // FOR DEBUG
                questions.Add(GenerateQuestion(symptom, FactorType.Presence, customQuestions));
                questions.Add(GenerateQuestion(symptom, FactorType.Harmfulness, customQuestions));
            }
            else
            {
                // Min & Max days since last quizz
                var lastValue = symptom.ValuesHistory.FirstOrDefault();
                var daysSinceLastValue = DateTime.Now - lastValue?.Timestamp ?? TimeSpan.MaxValue;
                if  (daysSinceLastValue.TotalDays < QuizzConfiguration.MaxDaysBeforeNewQuizz)
                {
                    return new OperationResult<IEnumerable<QuizzQuestion>>(null, OperationResultType.Error,
                        "Vous avez déjà répondu à votre questionnaire aujourd'hui.");
                }
            
                var daysRequiredBeforeNextQuizz = GetRequiredDaysBeforeNextQuizz(symptom);
                if (daysSinceLastValue.TotalDays >= daysRequiredBeforeNextQuizz || daysSinceLastValue.TotalDays >= QuizzConfiguration.MaxDaysBeforeNewQuizz)
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
    
    private QuizzQuestion GenerateQuestion(Symptom symptom, FactorType factorType, IEnumerable<CustomQuizzQuestion> customQuizzQuestions, QuestionType? questionType = null)
    {
        if (questionType == null)
        {
            var r = new Random().Next(Enum.GetNames(typeof(QuestionType)).Length+1);
            questionType = (QuestionType)r;
        }
        
        var newQuestion = new QuizzQuestion
        {
            CustomQuestion = 
                customQuizzQuestions.FirstOrDefault(
                    q => q.SymptomType.Id.Equals(symptom.Id) && q.FactorType.Equals(factorType))
                ?? new CustomQuizzQuestion
                {
                    Text = GetDefaultQuestionText(questionType.Value, factorType, symptom.Type.Name),
                    Type = questionType.Value,
                    FactorType = factorType,
                    AnswerPossibilities = GetDefaultAnswers(questionType.Value)
                },
            Symptom = symptom
        };

        return newQuestion;
    }
    
    private string GetDefaultQuestionText(QuestionType questionType, FactorType factorType, string symptomeTypeName)
    {
        var word = factorType == FactorType.Presence ? "présent" : "nuisible";
        
        switch (questionType)
        {
            case QuestionType.LikertScale:
                return $"Sur une échelle de 1 à 5, à quel point le symptôme suivant a-t-il été {word} aujourd'hui ? {symptomeTypeName}";
            case QuestionType.Emojis:
                return factorType == FactorType.Presence
                    ? $"Comment vous sentez-vous par rapport à vos {symptomeTypeName.ToLower()} aujourd'hui ? Cela a-t-il perturbé votre journée ?"
                    : $"Comment vous sentez-vous par rapport à vos {symptomeTypeName.ToLower()} aujourd'hui ? Cela a-t-il nui au bon déroulement de votre journée ?";
            default:
                return $"Le symptôme suivant a-t-il été {word} aujourd'hui ? {symptomeTypeName}";
        }
    }
    
    private IEnumerable<QuizzAnswer> GetDefaultAnswers(QuestionType questionType)
    {
        switch (questionType)
        {
            case QuestionType.LikertScale:
                return new List<QuizzAnswer>
                {
                    new() { Text = "1", Weight = 6f },
                    new() { Text = "2", Weight = 4f },
                    new() { Text = "3", Weight = 0f },
                    new() { Text = "4", Weight = -4f },
                    new() { Text = "5", Weight = -6f },
                };
            case QuestionType.Emojis:
                return new List<QuizzAnswer>
                {
                    new() { Text = "Très bien", Weight = 6f },
                    new() { Text = "Bien", Weight = 4f },
                    new() { Text = "Normalement", Weight = 0f },
                    new() { Text = "Mal", Weight = -4f },
                    new() { Text = "Très mal", Weight = -6f },
                };
            default:
                return new List<QuizzAnswer>
                {
                    new() { Text = "Pas du tout", Weight = 6f },
                    new() { Text = "Très peu", Weight = 4f },
                    new() { Text = "Peu", Weight = 0f },
                    new() { Text = "Légèrement", Weight = -4f },
                    new() { Text = "Fortement", Weight = -6f },
                };
        }
    }
    
    private double GetRequiredDaysBeforeNextQuizz(Symptom symptom)
    {
        var average = QuizzService.GetAverageValues(symptom, 3);
        var daysRequired = Math.Round(4 * Math.Sqrt(1 + average), MidpointRounding.ToZero);
        return double.IsNaN(daysRequired) ? 0 : daysRequired;
    }
}