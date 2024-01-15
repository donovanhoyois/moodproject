using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MoodProject.Core.Models;

namespace MoodProject.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]/")]
[EnableCors()]
//[Authorize]
public class SymptomsTypesController
{
    private readonly MoodProjectContext DbContext;

    public SymptomsTypesController(MoodProjectContext dbContext)
    {
        DbContext = dbContext;
    }
    
    [HttpGet, ActionName("GetAll")]
    public IEnumerable<SymptomType> GetSymptomsTypes()
    {
        return DbContext.SymptomTypes.ToList();
    }

    [HttpGet, ActionName("Add")]
    public IResult AddSymptomType(string name)
    {
        var symptomType = new SymptomType(0, name);
        DbContext.SymptomTypes.Add(symptomType);
        DbContext.SaveChanges();
        return Results.Ok();
    }
}