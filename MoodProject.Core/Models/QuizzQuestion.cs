namespace MoodProject.Core;


public class QuizzQuestion
{
    public string Question { get; set; }
    public QuestionType Type { get; set; }
    public Symptom Symptom { get; set; }
    public FactorType FactorType { get; set; }
    public IEnumerable<QuizzAnswer> AnswerPossibilities { get; set; }

    public QuizzQuestion()
    {
        
    }
}

