namespace MoodProject.Core.Models.Notifications;

public class NotificationSubscription
{
    public int? Id { get; set; }

    public string? UserId { get; set; }

    public string? Url { get; set; }

    public string? P256dh { get; set; }

    public string? Auth { get; set; }
}