using MoodProject.Core.Models;

namespace MoodProject.App.Models;

public class MedicationForm : Medication
{
    public int UsagePerDay = 1;

    public Stack<DayUsage> DayUsages = new();
    public Stack<int> DaysOfTheMonth = new();

    public MedicationForm()
    {

        UpdateListLength();
    }

    public void UpdateListLength()
    {
        while (DayUsages.Count < UsagePerDay)
        {
            DayUsages.Push(new DayUsage(new DateTime()));
        }

        while (DayUsages.Count > UsagePerDay)
        {
            DayUsages.Pop();
        }
    }
}
