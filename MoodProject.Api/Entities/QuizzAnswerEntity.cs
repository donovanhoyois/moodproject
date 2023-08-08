using MoodProject.Core;
using MoodProject.Core.Models;

namespace MoodProject.Api;

public class QuizzAnswerEntity : QuizzAnswer
{
    public int QuizzQuestionId { get; set; }
}