using MoodProject.Core.Models.Notifications;

namespace MoodProject.Core.Ports.In;

public interface INotificationService
{
    public Task RegisterNewSubscription(NotificationSubscription notificationSubscription);
}