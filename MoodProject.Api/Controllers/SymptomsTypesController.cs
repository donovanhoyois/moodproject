using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MoodProject.Core;
using MoodProject.Core.Models;

namespace MoodProject.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]/")]
[EnableCors()]
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
    public int AddSymptomType(string name)
    {
        DbContext.SymptomTypes.Add(new SymptomType(0, name));
        return DbContext.SaveChanges();
    }
}