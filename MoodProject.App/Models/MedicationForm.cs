using System.ComponentModel.DataAnnotations;
using MoodProject.Core.Enums;
using MoodProject.Core.Models;

namespace MoodProject.App.Models;

public class MedicationForm : Medication
{
    [Range(1, 10)]
    public int UsagePerDay { get; set; } = 1;

    public static Dictionary<MedicationUsage, string> UsageDictionnary { get; set; } = new()
    {
        { MedicationUsage.PER_DAY, "Quotidien" },
        { MedicationUsage.PER_MONTH, "Non Quotidien" }
    };

    public static Dictionary<MedicationMonthUsage, string> MonthUsageDictionnary { get; set; } = new()
    {
        { MedicationMonthUsage.EVERY_WEEK, "Chaque semaine" },
        { MedicationMonthUsage.EVERY_TWO_WEEK, "Toutes les 2 semaines" },
        { MedicationMonthUsage.ONE_TIME_A_MONTH, "Une fois par mois" },
        { MedicationMonthUsage.TWO_TIMES_A_MONTH, "Deux fois par mois" }
    };

    public List<FormDayUsage> FormDayUsages { get; set; } = new();

    public MedicationForm()
    {
        UpdateListLength();
    }

    public void UpdateListLength()
    {
        ValidateUsagePerDay();

        while (FormDayUsages.Count < UsagePerDay)
        {
            FormDayUsages.Add(new FormDayUsage(0, new DateTime()));
        }

        while (FormDayUsages.Count > UsagePerDay)
        {
            FormDayUsages.RemoveAt(FormDayUsages.Count-1);
        }
    }

    private void ValidateUsagePerDay()
    {
        if (UsagePerDay > 10) { UsagePerDay = 10; }
        if (UsagePerDay < 1) { UsagePerDay = 1; }
    }
}
