using MoodProject.Core;

namespace MoodProject.App.Models;

public class QuizzForm
{
    public List<AnswerableQuizzQuestion> Questions { get; set; } = new();
    public QuizzForm(IEnumerable<QuizzQuestion> questions)
    {
        foreach (var q in questions)
        {
            Questions.Add(new(q));
        }
    }
}