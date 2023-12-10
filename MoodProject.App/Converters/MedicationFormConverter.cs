using MoodProject.App.Models;
using MoodProject.Core.Models;

namespace MoodProject.App.Converters;

public static class MedicationFormConverter
{
    public static Medication ConvertFormToModel(MedicationForm medicationForm, string userId)
    {
        var medication = (Medication) medicationForm;
        medication.UserId = userId;
        medication.DayUsages = new List<MedicationDayUsage>();
        foreach (var formDayUsage in medicationForm.FormDayUsages)
        {
            var medicationDayUsage = new MedicationDayUsage(formDayUsage.Time);
            medicationDayUsage.Id = formDayUsage.Id;
            medicationDayUsage.MedicationId = medicationForm.Id;
            medication.DayUsages.Add(medicationDayUsage);
        }

        return medication;
    }

    public static MedicationForm ConvertModelToForm(Medication medication)
    {
        var medicationForm = new MedicationForm()
        {
            Id = medication.Id,
            UserId = medication.UserId,
            Name = medication.Name,
            Usage = medication.Usage,
            DayUsages = medication.DayUsages,
            MonthUsage = medication.MonthUsage,
            IsDisabled = medication.IsDisabled,
        };
        medicationForm.FormDayUsages = new List<FormDayUsage>();
        foreach (var dayUsage in medication.DayUsages)
        {
            medicationForm.FormDayUsages.Add(new FormDayUsage(dayUsage.Id, new DateTime(dayUsage.TimeOfTheDay.Ticks)));
        }
        medicationForm.UsagePerDay = medicationForm.FormDayUsages.Count;
        
        return medicationForm;
    }
}