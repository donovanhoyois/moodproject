namespace MoodProject.Core.Ports.In;

public interface IQuizzService
{
    public Task<IEnumerable<QuizzQuestion>> Generate(string userId);
    public Task<bool> Submit(IEnumerable<Symptom> symptoms, IEnumerable<FactorValue> values);
}