using MoodProject.Core.Enums;

namespace MoodProject.Core.Models;

public class CustomQuizzQuestion
{
    public int Id { get; set; }
    public string Text { get; set; }
    public QuestionType Type { get; set; }
    public SymptomType SymptomType { get; set; }
    public FactorType FactorType { get; set; }
    public IEnumerable<QuizzAnswer> AnswerPossibilities { get; set; }

    public CustomQuizzQuestion()
    {
        
    }
}