namespace MoodProject.Core.Ports.In;

public interface ISymptomsFormService
{
    public SymptomHistory<Symptom> RetrieveHistory();
    public void SaveHistory();
    public bool NeedsToBeGenerated();
    public SymptomsForm Generate();
}