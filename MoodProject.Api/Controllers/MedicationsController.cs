using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MoodProject.Core.Models;

namespace MoodProject.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]/")]
[EnableCors]
public class MedicationsController
{
    private readonly MoodProjectContext DbContext;
    
    public MedicationsController(MoodProjectContext dbContext)
    {
        DbContext = dbContext;
    }
    
    [HttpGet, ActionName("GetMedicationsByUserId")]
    public List<Medication> GetMedicationsByUserId(string userId)
    {
        return DbContext.Medications
            .Where(m => m.UserId.Equals(userId))
            .ToList();
    }

    [HttpPatch, ActionName("UpdateMedications")]
    public bool UpdateMedications(List<Medication> medications)
    {
        medications.ForEach(med =>
        {
            var retrievedMed = DbContext.Medications.FirstOrDefault(dbMed => dbMed.Id.Equals(med.Id));
            if (retrievedMed != null)
            {
                DbContext.Entry(retrievedMed).CurrentValues.SetValues(med);
            }
            else
            {
                DbContext.Medications.Add(med);
            }
            
        });
        return DbContext.SaveChanges() > 0;
    }
}