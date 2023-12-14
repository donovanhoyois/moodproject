using MoodProject.Core.Models.Notifications;
using MoodProject.Core.Ports.In;
using MoodProject.Core.Ports.Out;

namespace MoodProject.Services;

public class NotificationService : INotificationService
{
    private readonly IAppApi AppApi;
    
    public NotificationService(IAppApi appApi)
    {
        AppApi = appApi;
    }

    public async Task RegisterNewSubscription(NotificationSubscription notificationSubscription)
    {
        await AppApi.RegisterNewNotificationSubscription(notificationSubscription);
    }
}