using MoodProject.Api.Configuration;
using MoodProject.Core.Models;
using MoodProject.Core.Models.Notifications;

namespace MoodProject.Api.Services;

public class NotificationService
{
    private readonly DatabaseConfiguration DatabaseConfiguration;
    
    public NotificationService(DatabaseConfiguration databaseConfiguration)
    {
        DatabaseConfiguration = databaseConfiguration;
    }
    
    /// <summary>
    /// This method retrieve the notification in the database to sent them to a notification hub.
    /// This is intended to be called one time every hour. 
    /// </summary>
    /// <remarks></remarks>
    /// <returns>A <see cref="Queue{T}"/> of <see cref="MedicationNotification"/> containing the notifications to sent in the next hour.</returns>
    public Queue<MedicationNotification> GetMedicationsNotificationsNextHour()
    {
        using var dbContext = new MoodProjectContext(DatabaseConfiguration);
        
        var serverUtcOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours;

        var minTimeSpan = new TimeSpan(DateTime.Now.Ticks);
        var maxTimeSpan = new TimeSpan(DateTime.Now.Ticks) + TimeSpan.FromHours(1);

        // Retrieving day usages from database for the next hour
        var dayUsages = RetrieveDayUsagesBetween(dbContext, minTimeSpan, maxTimeSpan, serverUtcOffset);

        // Apply timestamp to retrieved day usages to send it back to notifier service
        var medicationNotifications = CreateNotificationWithTimestamp(dbContext, dayUsages, minTimeSpan, serverUtcOffset);

        return new Queue<MedicationNotification>(medicationNotifications.FindAll(notification => notification.NotificationSubscription != null).OrderBy(medNotification => medNotification.Time));
    }

    private List<MedicationDayUsage> RetrieveDayUsagesBetween(MoodProjectContext dbContext, TimeSpan minTimeSpan, TimeSpan maxTimeSpan, int serverUtcOffset)
    {
        var dayUsages = new List<MedicationDayUsage>();
        foreach (var d in dbContext.Medications.Where(m => m.AreNotificationsEnabled).SelectMany(m => m.DayUsages).ToList())
        {
            var dayTimespan = new TimeSpan(minTimeSpan.Days, d.TimeOfTheDay.Hour + (d.UtcOffset - serverUtcOffset), d.TimeOfTheDay.Minute, d.TimeOfTheDay.Minute);
            var nextDayTimespan = new TimeSpan(minTimeSpan.Days + 1, d.TimeOfTheDay.Hour + (d.UtcOffset - serverUtcOffset), d.TimeOfTheDay.Minute, d.TimeOfTheDay.Minute);
            if (dayTimespan >= minTimeSpan && dayTimespan <= maxTimeSpan || nextDayTimespan >= minTimeSpan && nextDayTimespan <= maxTimeSpan)
            {
                dayUsages.Add(d);
            }
        }

        return dayUsages;
    }
    
    private List<MedicationNotification> CreateNotificationWithTimestamp(MoodProjectContext dbContext, List<MedicationDayUsage> dayUsages, TimeSpan minTimeSpan, int serverUtcOffset)
    {
        var medicationNotifications = new List<MedicationNotification>();
        foreach (var dayUsage in dayUsages)
        {
            var todayTimespan = new TimeSpan(minTimeSpan.Days, dayUsage.TimeOfTheDay.Hour + (serverUtcOffset - dayUsage.UtcOffset), dayUsage.TimeOfTheDay.Minute, dayUsage.TimeOfTheDay.Second);
            var nextNotificationTimespan = todayTimespan < new TimeSpan(DateTime.Now.Ticks)
                ? new TimeSpan(minTimeSpan.Days + 1, dayUsage.TimeOfTheDay.Hour + (serverUtcOffset - dayUsage.UtcOffset), dayUsage.TimeOfTheDay.Minute,
                    dayUsage.TimeOfTheDay.Second)
                : todayTimespan;
            var correspondingMedication = dbContext.Medications.First(med => med.Id.Equals(dayUsage.MedicationId));
            var userNotificationSubscription = dbContext.NotificationSubscriptions.FirstOrDefault(subscription => subscription.UserId.Equals(correspondingMedication.UserId));
            medicationNotifications.Add(
                new MedicationNotification(
                    "Rappel de médicament",
                    $"Il est l'heure de prendre votre {correspondingMedication.Name}",
                    userNotificationSubscription,
                    correspondingMedication.UserId,
                    correspondingMedication.Name,
                    nextNotificationTimespan
                )
            );
        }

        return medicationNotifications;
    }
}