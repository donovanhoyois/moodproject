using System.ComponentModel.DataAnnotations.Schema;
using MoodProject.Core.Enums;

namespace MoodProject.Core.Models;

public class Medication
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Name { get; set; }
    public MedicationUsage Usage { get; set; }
    public Stack<MedicationDayUsage> DayUsages { get; set; } = new();
    public MedicationMonthUsage MonthUsage { get; set; } = MedicationMonthUsage.NONE;
    public bool IsDisabled { get; set; }

    public Medication()
    {
        
    }
}