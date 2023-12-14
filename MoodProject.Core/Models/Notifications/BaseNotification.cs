namespace MoodProject.Core.Models.Notifications;

public abstract class BaseNotification
{
    public string Title { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
}