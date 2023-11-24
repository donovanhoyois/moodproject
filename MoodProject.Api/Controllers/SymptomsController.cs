using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoodProject.Core;
using MoodProject.Core.Models;

namespace MoodProject.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]/")]
[EnableCors]
public class SymptomsController
{
    private readonly MoodProjectContext DbContext;
    
    public SymptomsController(MoodProjectContext dbContext)
    {
        DbContext = dbContext;
    }
    
    [HttpGet, ActionName("Get")]
    public IEnumerable<Symptom> GetSymptoms(string userId)
    {
        return DbContext.Symptoms.Where(symptom => symptom.UserId.Equals(userId) && !symptom.isDisabled).ToList();
    }
    
    [HttpGet, ActionName("GetSymptomsWithHistoryByUserId")]
    public IEnumerable<Symptom> GetSymptomsHistory(string userId)
    {
        return DbContext.Symptoms
            .Include(symptom => symptom.Type)
            .Include(symptom => symptom.ValuesHistory.OrderByDescending(value => value.Timestamp))
            .Where(s => s.UserId.Equals(userId) && !s.isDisabled)
            .ToList();
    }

    [HttpPost, ActionName("Update")]
    public void UpdateSymptoms(IEnumerable<Symptom> newSymptoms)
    {
        var userId = newSymptoms.FirstOrDefault()?.UserId ?? string.Empty;
        var cleanedSymptoms = new List<Symptom>();

        var existingSymptoms = DbContext.Symptoms.Where(s => s.UserId.Equals(userId));
        
        foreach (var symptomToInsert in newSymptoms)
        {
            var foundType = DbContext.SymptomTypes.First(type => type.Id.Equals(symptomToInsert.TypeId));
            if (foundType != null)
            {
                symptomToInsert.Type = foundType;
            }

            var existingSymptom = existingSymptoms.FirstOrDefault(s =>
                s.UserId.Equals(symptomToInsert.UserId) && s.TypeId.Equals(symptomToInsert.TypeId));
            if (existingSymptom == null)
            {
                cleanedSymptoms.Add(symptomToInsert);
            }
        }

        // Disable old symptoms
        foreach (var existingSymptom in existingSymptoms)
        {
            existingSymptom.isDisabled =
                newSymptoms.FirstOrDefault(s => s.TypeId.Equals(existingSymptom.TypeId)) == null;
        }
        
        DbContext.Symptoms.AddRange(cleanedSymptoms);
        DbContext.SaveChanges();
    }

    [HttpPost, ActionName("UpdateHistory")]
    public bool UpdateSymptomsHistory(IEnumerable<FactorValue> values)
    {
        DbContext.FactorValues.AddRange(values);
        var nbChanges = DbContext.SaveChanges();
        return nbChanges > 0;
    }
}