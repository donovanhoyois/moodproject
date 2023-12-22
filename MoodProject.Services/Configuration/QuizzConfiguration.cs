namespace MoodProject.Services.Configuration;

public class QuizzConfiguration
{
    public bool IgnoreMinDaysToGenerateQuizz { get; set; }
    public int MinDaysBeforeNewQuizz { get; set; }
    public int MaxDaysBeforeNewQuizz { get; set; }
}