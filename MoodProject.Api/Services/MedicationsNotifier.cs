using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MoodProject.Api.Controllers;
using MoodProject.Api.Interfaces;
using MoodProject.Core.Models.Notifications;
using TimeSpan = System.TimeSpan;

namespace MoodProject.Api.Services;

/// <summary>
/// This BackgroundService retrieve notifications from a service and then send the notifications to the correct clients at the right time. 
/// </summary>
public class MedicationsNotifier : BackgroundService
{
    private static readonly TimeSpan DefaultServiceRequestPeriod = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan DefaultSendNotificationPeriod = TimeSpan.FromSeconds(1);
    private readonly ILogger<MedicationsNotifier> Logger;
    private readonly IHubContext<NotificationsHub, INotificationClient> Context;
    private readonly NotificationService NotificationService;

    private Queue<MedicationNotification> NotificationsQueue = new();
    private PeriodicTimer ServiceRequestTimer = new(DefaultServiceRequestPeriod);
    private PeriodicTimer SendNotificationTimer = new(DefaultSendNotificationPeriod);

    private bool FirstServiceRequest = true;

    public MedicationsNotifier(ILogger<MedicationsNotifier> logger, IHubContext<NotificationsHub, INotificationClient> context, NotificationService notificationService)
    {
        Logger = logger;
        Context = context;
        NotificationService = notificationService;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ServiceRequestTimer = new PeriodicTimer(DefaultServiceRequestPeriod);
        while (FirstServiceRequest || !stoppingToken.IsCancellationRequested && await ServiceRequestTimer.WaitForNextTickAsync(stoppingToken))
        {
            RefreshNotificationQueue();
            RefreshNextNotificationTimer();
            while (!stoppingToken.IsCancellationRequested && NotificationsQueue.Any() && await SendNotificationTimer.WaitForNextTickAsync(stoppingToken))
            {
                await SendNotifications();
                RefreshNextNotificationTimer();
            }
            Logger.LogInformation("{} is waiting for next service call.", nameof(MedicationsNotifier));
        }
    }

    /// <summary>
    /// This method refresh the timer to wait until the next notification need to be sent.
    /// </summary>
    private void RefreshNextNotificationTimer()
    {
        if (NotificationsQueue.Any())
        {
            try
            {
                var newTimer = NotificationsQueue.Peek().Time - new TimeSpan(DateTime.Now.Ticks);
                SendNotificationTimer = new PeriodicTimer(newTimer);
                Logger.LogInformation("{} has a new Timer: {}h{}m{}s, next notification is scheduled @ {}", nameof(MedicationsNotifier), newTimer.Hours, newTimer.Minutes, newTimer.Seconds, (DateTime.Now + newTimer).TimeOfDay.ToString(@"hh\:mm\:ss"));
            }
            catch (Exception e)
            {
                Logger.LogError("{} is an invalid period.",(NotificationsQueue.Peek().Time - new TimeSpan(DateTime.Now.Ticks)).TotalSeconds);
            }
        }
        else
        {
            SendNotificationTimer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        }
    }

    /// <summary>
    /// This method called every hour and call the service to retrieve notifications to sent.
    /// </summary>
    private void RefreshNotificationQueue()
    {
        FirstServiceRequest = false;
        Logger.LogInformation("{} is retrieving medications from service.", nameof(MedicationsNotifier));
        NotificationsQueue = NotificationService.GetMedicationsNotificationsNextHour();
        Logger.LogInformation("{} retrieved {} medications from service.", nameof(MedicationsNotifier), NotificationsQueue.Count);
        SendNotificationTimer = new PeriodicTimer(DefaultSendNotificationPeriod);
    }

    /// <summary>
    /// This method send the notifications to the correct client, based on the UserId stored in the <see cref="MedicationNotification"/>.
    /// </summary>
    private async Task SendNotifications()
    {
        var sentNotifications = new List<Task>();
        while (NotificationsQueue.Any() && NotificationsQueue.Peek().Time < new TimeSpan(DateTime.Now.Ticks))
        {
            var notificationToSend = NotificationsQueue.Dequeue();
            sentNotifications.Add(
                Context.Clients
                    .User(notificationToSend.UserId)
                    .ReceiveNotification($"Il est l'heure de prendre votre médicament: {notificationToSend.MedicationName}"));
                    
            await Task.WhenAll(sentNotifications);
        }
        Logger.LogInformation("{} has sent {} notifications @ {}", nameof(MedicationsNotifier), sentNotifications.Count, DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss"));
    }
}