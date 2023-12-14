using MoodProject.Core.Models.Notifications;

namespace MoodProject.Api.Interfaces;

public interface INotificationClient
{
    Task ReceiveMedicationNotification(MedicationNotification notification);
}