using System.ComponentModel.DataAnnotations.Schema;

namespace MoodProject.Core.Models;


public class QuizzQuestion
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public CustomQuizzQuestion CustomQuestion { get; set; }
    public Symptom Symptom { get; set; }
}

