namespace MoodProject.App.Models;

public class DayUsage
{
    public DateTime Time { get; set; }

    public DayUsage(DateTime dateTime)
    {
        this.Time = dateTime;
    }
}