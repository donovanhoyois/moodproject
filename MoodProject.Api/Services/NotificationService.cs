using MoodProject.Core.Models;
using MoodProject.Core.Models.Notifications;

namespace MoodProject.Api.Services;

public class NotificationService
{
    private readonly ILogger<NotificationService> Logger;
    private readonly MoodProjectContext DbContext;
    
    public NotificationService(ILogger<NotificationService> logger)
    {
        Logger = logger;
        DbContext = new MoodProjectContext();
    }
    
    /// <summary>
    /// This method retrieve the notification in the database to sent them to a notification hub.
    /// This is intended to be called one time every hour. 
    /// </summary>
    /// <remarks></remarks>
    /// <returns>A <see cref="Queue{T}"/> of <see cref="MedicationNotification"/> containing the notifications to sent in the next hour.</returns>
    public Queue<MedicationNotification> GetMedicationsNotificationsNextHour()
    {
        var minTimeSpan = new TimeSpan(DateTime.Now.Ticks);
        var maxTimeSpan = new TimeSpan(DateTime.Now.Ticks) + TimeSpan.FromHours(1);

        var dayUsages = new List<MedicationDayUsage>();

        foreach (var d in DbContext.MedicationDayUsages)
        {
            var dayTimespan = new TimeSpan(minTimeSpan.Days, d.TimeOfTheDay.Hour, d.TimeOfTheDay.Minute, d.TimeOfTheDay.Minute);
            var nextDayTimespan = new TimeSpan(minTimeSpan.Days + 1, d.TimeOfTheDay.Hour, d.TimeOfTheDay.Minute, d.TimeOfTheDay.Minute);
            if (dayTimespan >= minTimeSpan && dayTimespan <= maxTimeSpan || nextDayTimespan >= minTimeSpan && nextDayTimespan <= maxTimeSpan)
            {
                dayUsages.Add(d);
            }
        }

        var medicationNotifications = new List<MedicationNotification>();
        foreach (var dayUsage in dayUsages)
        {
            var todayTimespan = new TimeSpan(minTimeSpan.Days, dayUsage.TimeOfTheDay.Hour, dayUsage.TimeOfTheDay.Minute, dayUsage.TimeOfTheDay.Minute);
            var nextNotificationTimespan = todayTimespan < new TimeSpan(DateTime.Now.Ticks)
                ? new TimeSpan(minTimeSpan.Days, dayUsage.TimeOfTheDay.Hour, dayUsage.TimeOfTheDay.Minute,
                    dayUsage.TimeOfTheDay.Minute)
                : todayTimespan;
            var correspondingMedication = DbContext.Medications.First(med => med.Id.Equals(dayUsage.MedicationId));
            medicationNotifications.Add(new MedicationNotification( correspondingMedication.UserId, correspondingMedication.Name, nextNotificationTimespan));
        }

        // TODO: Remove
        var debugQueue = new Queue<MedicationNotification>();
        debugQueue.Enqueue(new MedicationNotification("testUser", "Medicament test", new TimeSpan(DateTime.Now.Ticks) + TimeSpan.FromSeconds(4)));
        debugQueue.Enqueue(new MedicationNotification("testUser2", "Medicament test 2", new TimeSpan(DateTime.Now.Ticks) + TimeSpan.FromSeconds(4)));
        debugQueue.Enqueue(new MedicationNotification("testUser", "Medicament test", new TimeSpan(DateTime.Now.Ticks) + TimeSpan.FromSeconds(8)));
        return debugQueue;
        
        return new Queue<MedicationNotification>(medicationNotifications.OrderBy(medNotification => medNotification.Time));
    }
}