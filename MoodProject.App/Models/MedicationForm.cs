using MoodProject.Core.Enums;
using MoodProject.Core.Models;

namespace MoodProject.App.Models;

public class MedicationForm : Medication
{
    public int UsagePerDay { get; set; } = 1;

    public Dictionary<MedicationUsage, string> UsageDictionnary { get; set; } = new()
    {
        { MedicationUsage.PER_DAY, "Quotidien" },
        { MedicationUsage.PER_MONTH, "Non Quotidien" }
    };

    public Dictionary<MedicationMonthUsage, string> MonthUsageDictionnary { get; set; } = new()
    {
        { MedicationMonthUsage.NONE, "Aucun" },
        { MedicationMonthUsage.EVERY_WEEK, "Chaque semaine" },
        { MedicationMonthUsage.EVERY_TWO_WEEK, "Toutes les 2 semaines" },
        { MedicationMonthUsage.ONE_TIME_A_MONTH, "Une fois par mois" },
        { MedicationMonthUsage.TWO_TIMES_A_MONTH, "Deux fois par mois" }
    };

    public Stack<DayUsage> DayUsages { get; set; } = new();
    public MedicationMonthUsage MonthUsage { get; set; } = MedicationMonthUsage.NONE;

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
