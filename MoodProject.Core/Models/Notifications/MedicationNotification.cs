namespace MoodProject.Core.Models.Notifications;

public class MedicationNotification : BaseNotification
{
    public NotificationSubscription? NotificationSubscription { get; }
    public string UserId { get; set; }
    public string MedicationName { get; init; }
    public TimeSpan Time { get; set; }
    
    public MedicationNotification(string title, string content, NotificationSubscription? notificationSubscription, string userId, string medicationName, TimeSpan time)
    {
        Title = title;
        Content = content;
        NotificationSubscription = notificationSubscription;
        UserId = userId;
        MedicationName = medicationName;
        Time = time;
    }
}