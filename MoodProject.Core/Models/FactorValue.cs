using System.ComponentModel.DataAnnotations.Schema;
using MoodProject.Core.Enums;

namespace MoodProject.Core.Models;

public class FactorValue
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int SymptomId { get; set; }
    public FactorType Type { get; set; }
    public float Value { get; set; }
    public DateTime Timestamp { get; set; }

    public FactorValue()
    {
        
    }

    public FactorValue(DateTime timestamp, float value)
    {
        Timestamp = timestamp;
        Value = value;
    }
}