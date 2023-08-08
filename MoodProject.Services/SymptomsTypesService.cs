using System.Collections;
using System.Net.Http.Json;
using MoodProject.Core;
using MoodProject.Core.Models;
using MoodProject.Core.Ports.In;
using MoodProject.Core.Ports.Out;

namespace MoodProject.Services;

public class SymptomsTypesService : ISymptomsTypesService
{
    private IAppApi ApiClient;
    public SymptomsTypesService(IAppApi apiClient)
    {
        this.ApiClient = apiClient;
    }
    public async Task<IEnumerable<SymptomType>> GetAll()
    {
        return await ApiClient.GetSymptomsTypes();
    }
}