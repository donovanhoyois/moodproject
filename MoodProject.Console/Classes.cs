namespace MoodProject.Console;

public class Question
{
    public string prop1 { get; set; }
    public string prop2 { get; set; }
}

public class AnswerableQuestion : Question
{
    public string prop3 { get; set; }
}