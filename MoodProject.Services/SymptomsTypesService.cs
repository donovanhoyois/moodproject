using System.Collections;
using System.Net.Http.Json;
using MoodProject.Core;
using MoodProject.Core.Ports.In;

namespace MoodProject.Services;

public class SymptomsTypesService : ISymptomsTypesService
{
    private HttpClient ApiClient;
    public SymptomsTypesService(HttpClient apiClient)
    {
        this.ApiClient = apiClient;
    }
    public async Task<IEnumerable<SymptomType>> GetAll()
    {
        return await ApiClient.GetFromJsonAsync<IEnumerable<SymptomType>>("SymptomsTypes");
    }
}