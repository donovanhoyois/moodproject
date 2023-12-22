using MoodProject.Core.Models;

namespace MoodProject.Core.Ports.In;

public interface IQuizzGenerator
{
    public Task<OperationResult<IEnumerable<QuizzQuestion>>> Generate(string userId);
}