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
    public List<MedicationSchedule> MedicationSchedules { get; set; } = new();
    public bool IsDisabled { get; set; }
}