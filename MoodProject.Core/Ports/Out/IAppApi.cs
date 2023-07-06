namespace MoodProject.Core.Ports.Out;

public interface IAppApi
{
    public SymptomHistory<Symptom> LoadSymptomsHistory();
    public void SaveSymptomsHistory();
}