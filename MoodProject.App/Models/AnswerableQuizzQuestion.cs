using MoodProject.Core;

namespace MoodProject.App.Models;

public class AnswerableQuizzQuestion : QuizzQuestion
{
    public float Result { get; set; } = 0f;

    public AnswerableQuizzQuestion(QuizzQuestion parentQuestion)
    {
        Question = parentQuestion.Question;
        Symptom = parentQuestion.Symptom;
    }
}