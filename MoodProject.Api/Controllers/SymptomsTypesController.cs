using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MoodProject.Core;

namespace MoodProject.Api.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors("dev")]
public class SymptomsTypesController
{
    [HttpGet(Name = "GetSymptomsTypes")]
    public IEnumerable<SymptomType> GetAll()
    {
        return new List<SymptomType>()
        {
            new SymptomType(0, "Symptome 1"),
            new SymptomType(1, "Symptome 2"),
            new SymptomType(2, "Symptome 3")
        };
    }
}