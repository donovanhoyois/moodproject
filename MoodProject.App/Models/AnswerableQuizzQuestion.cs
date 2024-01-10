using MoodProject.Core.Models;

namespace MoodProject.App.Models;

public class AnswerableQuizzQuestion : QuizzQuestion
{
    public float Result { get; set; } = 0f;

    public AnswerableQuizzQuestion(QuizzQuestion parentQuestion)
    {
        CustomQuestion = parentQuestion.CustomQuestion;
        Symptom = parentQuestion.Symptom;
    }
}