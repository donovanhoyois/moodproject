using MoodProject.App.Models;
using MoodProject.Core.Enums;
using MoodProject.Core.Models;

namespace MoodProject.App.Converters;

public static class MedicationFormConverter
{
    public static Medication ConvertFormToModel(MedicationForm medicationForm, string userId)
    {
        var medication = (Medication) medicationForm;
        medication.UserId = userId;
        medication.DayUsages = new List<MedicationDayUsage>();
        switch (medicationForm.Usage)
        {
            case MedicationUsage.PER_DAY:
                medication.AreNotificationsEnabled = medicationForm.AreNotificationsEnabled;
                foreach (var formDayUsage in medicationForm.FormDayUsages)
                {
                    var medicationDayUsage = new MedicationDayUsage(formDayUsage.Time)
                    {
                        Id = formDayUsage.Id,
                        MedicationId = medicationForm.Id
                    };
                    medication.DayUsages.Add(medicationDayUsage);
                }
                break;
            
            case MedicationUsage.PER_MONTH:
                medication.AreNotificationsEnabled = false;
                medication.MonthUsage = medicationForm.MonthUsage;
                break;
        }
        

        return medication;
    }

    public static MedicationForm ConvertModelToForm(Medication medication)
    {
        var medicationForm = new MedicationForm
        {
            Id = medication.Id,
            UserId = medication.UserId,
            Name = medication.Name,
            Usage = medication.Usage,
            DayUsages = medication.DayUsages,
            MonthUsage = medication.MonthUsage,
            IsDisabled = medication.IsDisabled,
            FormDayUsages = new List<FormDayUsage>(),
            AreNotificationsEnabled = medication.AreNotificationsEnabled
        };
        foreach (var dayUsage in medication.DayUsages)
        {
            medicationForm.FormDayUsages.Add(new FormDayUsage(dayUsage.Id, new DateTime(dayUsage.TimeOfTheDay.Ticks)));
        }
        medicationForm.UsagePerDay = medicationForm.FormDayUsages.Count;
        
        return medicationForm;
    }
}