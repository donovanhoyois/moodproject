namespace MoodProject.Api.Interfaces;

public interface INotificationClient
{
    Task ReceiveMedicationNotification(string message);
}