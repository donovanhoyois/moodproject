using MoodProject.Core;
using MoodProject.Core.Ports.In;
using MoodProject.Core.Ports.Out;

namespace MoodProject.Services;

public class SymptomsFormService : ISymptomsFormService
{
    private IAppApi _appApi { get; init; }
    public SymptomsFormService(IAppApi appApi)
    {
        _appApi = appApi;
    }

    public SymptomHistory<Symptom> RetrieveHistory()
    {
        throw new NotImplementedException();
    }

    public void SaveHistory()
    {
        throw new NotImplementedException();
    }

    public bool NeedsToBeGenerated()
    {
        throw new NotImplementedException();
    }

    public SymptomsForm Generate()
    {
        throw new NotImplementedException();
    }
}