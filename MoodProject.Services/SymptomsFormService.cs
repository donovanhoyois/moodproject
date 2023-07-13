using MoodProject.Core;
using MoodProject.Core.Ports.In;
using MoodProject.Core.Ports.Out;

namespace MoodProject.Services;

public class SymptomsFormService : ISymptomsFormService
{
    private readonly IAppApi _appApi;
    public SymptomsFormService(IAppApi appApi)
    {
        _appApi = appApi;
    }

    public Task<IEnumerable<Symptom>> RetrieveHistory(User user)
    {
        return _appApi.LoadSymptomsHistory(user);
    }

    public void SaveHistory()
    {
        throw new NotImplementedException();
    }

    public bool NeedsToBeGenerated()
    {
        throw new NotImplementedException();
    }
}