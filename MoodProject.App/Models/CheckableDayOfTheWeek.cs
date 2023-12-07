namespace MoodProject.App.Models;

public class CheckableDayOfTheWeek
{
    public int Id { get; set; }
    public bool IsChecked { get; set; } = false;

    public CheckableDayOfTheWeek(int id, bool isChecked)
    {
        this.Id = id;
        this.IsChecked = isChecked;
    }
}