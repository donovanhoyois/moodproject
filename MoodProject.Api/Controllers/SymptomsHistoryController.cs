using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MoodProject.Core;

namespace MoodProject.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]/")]
[EnableCors]
public class SymptomsHistoryController
{
    [HttpGet(Name = "GetSymptomsHistory")]
    public List<Symptom> GetSymptomsHistory(string userKey)
    {
        /*
        var testValues = new List<FactorValue>()
        {
            new FactorValue(DateTime.Now - TimeSpan.FromDays(3), 0.1f),
            new FactorValue(DateTime.Now - TimeSpan.FromDays(2), 0.15f),
            new FactorValue(DateTime.Now - TimeSpan.FromDays(1), 0.25f)
        };
        return new List<Symptom>()
        {
            new Symptom(new SymptomType(0, "test"), new FactorValuesHistory(testValues, testValues), new FactorValuesHistory(testValues, testValues))
        };
        */
        return null;
    }

    [HttpGet]
    public void Test()
    {
        /*
        using (var context = new MoodProjectContext())
        {
            var factorValue = new List<FactorValue>() {new FactorValue(DateTime.Now, 0f)};
            context.Symptoms.Add(new Symptom("test", new FactorValuesHistory(factorValue, factorValue), new FactorValuesHistory(factorValue, factorValue)));
        }
        */
    }
}