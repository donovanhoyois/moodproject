using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoodProject.Core;

namespace MoodProject.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]/")]
[EnableCors]
public class CustomQuizzQuestionsController
{
    private MoodProjectContext DbContext;
    
    public CustomQuizzQuestionsController(MoodProjectContext dbContext)
    {
        DbContext = dbContext;
    }
    
    [HttpGet, ActionName("GetAll")]
    public IEnumerable<CustomQuizzQuestion> GetAllCustomsQuizzQuestions()
    {
        return DbContext.QuizzQuestions
            .Include(q => q.SymptomType)
            .ToList();
    }
}