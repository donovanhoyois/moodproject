namespace MoodProject.Api.Interfaces;

public interface INotificationClient
{
    Task ReceiveNotification(string message);
}