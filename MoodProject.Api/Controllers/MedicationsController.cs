using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoodProject.Core.Models;
using MoodProject.Core.Models.Notifications;

namespace MoodProject.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]/")]
[EnableCors]
public class MedicationsController
{
    private readonly ILogger<MedicationsController> Logger;
    private readonly MoodProjectContext DbContext;
    
    public MedicationsController(ILogger<MedicationsController> logger, MoodProjectContext dbContext)
    {
        Logger = logger;
        DbContext = dbContext;
    }
    
    [HttpGet, ActionName("GetMedicationsByUserId")]
    public List<Medication> GetMedicationsByUserId(string userId)
    {
        return DbContext.Medications
            .Where(m => m.UserId.Equals(userId))
            .Include(m => m.DayUsages)
            .ToList();
    }
    
    [HttpPatch, ActionName("UpdateMedications")]
    public bool UpdateMedications(List<Medication> medications)
    {
        medications.ForEach(med =>
        {

            var retrievedMed =
                DbContext.Medications
                    .Where(dbMed => dbMed.Id.Equals(med.Id))
                    .Include(m => m.DayUsages)
                    .FirstOrDefault();
            if (retrievedMed != null)
            {
                
                // Deleting old DayUsages
                foreach (var existingDayUsage in retrievedMed.DayUsages.ToList())
                {
                    if (med.DayUsages.All(d => d.Id != existingDayUsage.Id))
                    {
                        DbContext.MedicationDayUsages.Remove(existingDayUsage);
                    }
                }
                
                foreach (var dayUsage in med.DayUsages)
                {
                    var existingDayUsage = retrievedMed.DayUsages.FirstOrDefault(d => d.Id == dayUsage.Id);
                    if (existingDayUsage != null)
                    {
                        // Updating dayUsage
                        DbContext.Entry(existingDayUsage).CurrentValues.SetValues(dayUsage);
                    }
                    else
                    {
                        // Adding new dayUsage
                        retrievedMed.DayUsages.Add(dayUsage);
                    }
                }
                DbContext.Entry(retrievedMed).CurrentValues.SetValues(med);
            }
            else
            {
                DbContext.Medications.Add(med);
            }
        });
        return DbContext.SaveChanges() > 0;
    }

    [HttpPost, ActionName("DeleteMedications")]
    public bool DeleteMedications(List<Medication> medications)
    {
        medications.ForEach(med =>
        {
            DbContext.Medications.Remove(med);
        });
        return DbContext.SaveChanges() > 0;
    }
    
}