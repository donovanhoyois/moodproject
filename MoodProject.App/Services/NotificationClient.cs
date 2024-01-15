using Microsoft.AspNetCore.SignalR.Client;
using MoodProject.App.Configuration;
using MoodProject.Core.Models.Notifications;
using MoodProject.Core.Ports.In;

namespace MoodProject.App.Services;

public class NotificationClient : IAsyncDisposable
{
    private readonly NotificationsConfiguration NotificationsConfiguration;
    private readonly INotificationService NotificationService;
    private readonly CacheService CacheService;
    private readonly JsService JsService;

    private HubConnection? HubConnection;

    public NotificationClient(NotificationsConfiguration notificationsConfiguration, INotificationService notificationService, CacheService cacheService, JsService jsService)
    {
        NotificationsConfiguration = notificationsConfiguration;
        NotificationService = notificationService;
        CacheService = cacheService;
        JsService = jsService;
    }

    public async Task RequestAccess(string userId)
    {
        var subscription = await JsService.Execute<NotificationSubscription>("blazorPushNotifications.requestSubscription");
        if (subscription != null)
        {
            subscription.UserId = userId;
            await NotificationService.RegisterNewSubscription(subscription);   
        }
    }
    
    public async Task Connect(string userId)
    {
        var token = await CacheService.GetApiToken(userId, true);
        
        HubConnection = new HubConnectionBuilder()
            .WithUrl(
                NotificationsConfiguration.HubUrl,
                options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult<string?>(token);
                })
            .Build();

        // TODO: Use SignalR hub to receive notifications
        
        await HubConnection.StartAsync();
    }
    
    public async ValueTask DisposeAsync()
    {
        if (HubConnection is not null)
        {
            await HubConnection.DisposeAsync();
        }
        GC.SuppressFinalize(this);
    }
}