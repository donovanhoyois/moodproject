using System.ComponentModel.DataAnnotations.Schema;

namespace MoodProject.Core.Models;

public class QuizzAnswer
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Text { get; set; }
    public float Weight { get; set; }
}