using System.ComponentModel.DataAnnotations.Schema;

namespace MoodProject.Core.Models;

public class MedicationSchedule
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int MedicationId { get; set; }
    public DateTime DateTime { get; set; }
}