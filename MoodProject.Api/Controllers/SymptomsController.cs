using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MoodProject.Core;

namespace MoodProject.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]/")]
[EnableCors]
public class SymptomsController
{
    [HttpGet]
    public IEnumerable<Symptom> GetSymptoms(string userId)
    {
        using (var context = new MoodProjectContext())
        {
            return context.Symptoms.Where(symptom => symptom.UserId.Equals(userId)).ToList();
        }
    }
}