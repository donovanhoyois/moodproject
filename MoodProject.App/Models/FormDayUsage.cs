namespace MoodProject.App.Models;

public class FormDayUsage
{
    public int Id { get; set; }
    public DateTime Time { get; set; }

    public FormDayUsage(int id, DateTime dateTime)
    {
        Id = id;
        Time = dateTime;
    }
}