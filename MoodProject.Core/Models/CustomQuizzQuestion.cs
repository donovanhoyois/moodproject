using System.ComponentModel.DataAnnotations.Schema;
using MoodProject.Core.Enums;

namespace MoodProject.Core.Models;

public class CustomQuizzQuestion
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public QuestionType Type { get; set; }
    public SymptomType SymptomType { get; set; }
    public FactorType FactorType { get; set; }
    public string Text { get; set; }
    public IEnumerable<QuizzAnswer> AnswerPossibilities { get; set; }

    public CustomQuizzQuestion()
    {
        
    }
}