using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MoodProject.Core;

namespace MoodProject.Api.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors]
public class SymptomsHistoryController
{
    [HttpGet(Name = "GetSymptomsHistory")]
    public List<Symptom> GetAll()
    {
        var testValues = new List<FactorValue>()
        {
            new FactorValue(DateTime.Now - TimeSpan.FromDays(3), 0.1f),
            new FactorValue(DateTime.Now - TimeSpan.FromDays(2), 0.15f),
            new FactorValue(DateTime.Now - TimeSpan.FromDays(1), 0.25f)
        };
        return new List<Symptom>()
        {
            new Symptom("test", new Factor(testValues, testValues), new Factor(testValues, testValues))
        };
    }
}