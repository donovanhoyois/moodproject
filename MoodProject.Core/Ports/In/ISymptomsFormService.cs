namespace MoodProject.Core.Ports.In;

public interface ISymptomsFormService
{
    public Task<IEnumerable<Symptom>> RetrieveHistory(User user);
    public void SaveHistory();
    public bool NeedsToBeGenerated();
}