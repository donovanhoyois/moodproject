namespace MoodProject.Core.Ports.Out;

public interface IAppApi
{
    public Task<IEnumerable<Symptom>> LoadSymptomsHistory(User user);
    public Task SaveSymptomsHistory();
}