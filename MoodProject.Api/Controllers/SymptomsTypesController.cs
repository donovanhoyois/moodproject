using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MoodProject.Core;

namespace MoodProject.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]/")]
[EnableCors()]
public class SymptomsTypesController
{
    [HttpGet]
    public IEnumerable<SymptomType> GetSymptomsTypes()
    {
        using (var context = new MoodProjectContext())
        {
            var types = context.SymptomTypes.ToList();
            return types;
        }
    }

    [HttpGet]
    public IResult AddSymptomType(string name)
    {
        using (var context = new MoodProjectContext())
        {
            var symptomType = new SymptomType(0, name);
            context.SymptomTypes.Add(symptomType);
            context.SaveChanges();
        }
        return Results.Ok();
    }
}