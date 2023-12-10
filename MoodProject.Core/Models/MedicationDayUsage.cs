using System.ComponentModel.DataAnnotations.Schema;

namespace MoodProject.Core.Models;

public class MedicationDayUsage
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int MedicationId { get; set; }
    public TimeOnly TimeOfTheDay { get; set; }

    public MedicationDayUsage()
    {
        
    }

    public MedicationDayUsage(DateTime timeOfTheDay)
    {
        TimeOfTheDay = new TimeOnly(timeOfTheDay.Hour, timeOfTheDay.Minute, timeOfTheDay.Second);
    }
}