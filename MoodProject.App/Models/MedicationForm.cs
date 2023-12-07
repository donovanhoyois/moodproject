using MoodProject.Core.Models;

namespace MoodProject.App.Models;

public class MedicationForm : Medication
{
    public readonly CheckableDayOfTheWeek[] DaysOfTheWeek = new CheckableDayOfTheWeek[7];
    public List<int> DaysOfTheMonth = new();

    public MedicationForm()
    {
        for (var i = 0; i < DaysOfTheWeek.Length; i++)
        {
            DaysOfTheWeek[i] = new CheckableDayOfTheWeek(i + 1, false);
        }
    }
}