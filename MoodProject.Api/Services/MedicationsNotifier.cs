﻿using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using MoodProject.Api.Configuration;
using MoodProject.Api.Interfaces;
using MoodProject.Core.Models.Notifications;
using WebPush;
using TimeSpan = System.TimeSpan;

namespace MoodProject.Api.Services;

/// <summary>
/// This BackgroundService retrieve notifications from a service and then send the notifications to the correct clients at the right time. 
/// </summary>
public sealed class MedicationsNotifier : BackgroundService
{
    private static readonly TimeSpan DefaultServiceRequestPeriod = TimeSpan.FromHours(1);
    private static readonly TimeSpan DefaultSendNotificationPeriod = TimeSpan.FromSeconds(1);
    private readonly ILogger<MedicationsNotifier> Logger;
    private readonly IHubContext<NotificationsHub, INotificationClient> Context;
    private readonly IServiceProvider ServiceProvider;

    private Queue<MedicationNotification> NotificationsQueue = new();
    private PeriodicTimer ServiceRequestTimer = new(DefaultServiceRequestPeriod);
    private PeriodicTimer SendNotificationTimer = new(DefaultSendNotificationPeriod);
    
    private readonly VapidDetails VapidKeys;

    private bool FirstServiceRequest = true;

    
    public MedicationsNotifier(
        ILogger<MedicationsNotifier> logger,
        IHubContext<NotificationsHub, INotificationClient> context,
        IServiceProvider serviceProvider,
        NotificationConfiguration notificationConfiguration)
    {
        Logger = logger;
        Context = context;
        ServiceProvider = serviceProvider;
        VapidKeys = new VapidDetails(notificationConfiguration.Subject, notificationConfiguration.PublicKey, notificationConfiguration.PrivateKey);
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
            Logger.LogInformation($"{nameof(MedicationsNotifier)} is waiting for next service call.");
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
                Logger.LogInformation($"{nameof(MedicationsNotifier)} has a new Timer: {newTimer.Hours}h{newTimer.Minutes}m{newTimer.Seconds}s, next notification is scheduled @ {(DateTime.Now + newTimer).TimeOfDay.ToString(@"hh\:mm\:ss")}");
            }
            catch (Exception e)
            {
                Logger.LogError($"{(NotificationsQueue.Peek().Time - new TimeSpan(DateTime.Now.Ticks)).TotalSeconds} is an invalid period.");
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
    /// <remarks>As NotificationService is a scoped service and MedicationNotifier is a singleton, the scoped service need to be retrieved when used</remarks>
    private void RefreshNotificationQueue()
    {
        FirstServiceRequest = false;
        Logger.LogInformation($"{nameof(MedicationsNotifier)} is retrieving medications from service.");
        using (var scope = ServiceProvider.CreateScope())
        {
            var notificationService = scope.ServiceProvider.GetRequiredService<NotificationService>();
            NotificationsQueue = notificationService.GetMedicationsNotificationsNextHour();
        }
        Logger.LogInformation($"{nameof(MedicationsNotifier)} retrieved {NotificationsQueue.Count} medications from service.");
        SendNotificationTimer = new PeriodicTimer(DefaultSendNotificationPeriod);
    }

    /// <summary>
    /// This method send the notifications to the correct client, based on the UserId stored in the <see cref="MedicationNotification"/>.
    /// </summary>
    private async Task SendNotifications()
    {
        var sentNotifications = new List<Task>();
        while (NotificationsQueue.Any() && NotificationsQueue.Peek().Time < new TimeSpan(DateTime.Now.Ticks) + TimeSpan.FromSeconds(1))
        {
            var notificationToSend = NotificationsQueue.Dequeue();
            var pushSubscription = new PushSubscription(notificationToSend.NotificationSubscription.Url, notificationToSend.NotificationSubscription.P256dh, notificationToSend.NotificationSubscription.Auth);
            var webPushClient = new WebPushClient();
            try
            {
                var payload = JsonSerializer.Serialize(new
                {
                    notificationToSend,
                    url = "medications",
                });
                Logger.LogInformation($"{nameof(MedicationsNotifier)} is sending a notification to {notificationToSend.UserId} with subscription {notificationToSend.NotificationSubscription.Id}");
                await webPushClient.SendNotificationAsync(pushSubscription, payload, VapidKeys);
            }
            catch (Exception e)
            {
                Logger.LogWarning($"Error while trying to send notification to {notificationToSend.UserId}: {e}");
            }
 
            sentNotifications.Add(
                Context.Clients
                    .User(notificationToSend.UserId)
                    .ReceiveMedicationNotification(notificationToSend));
                    
            await Task.WhenAll(sentNotifications);
        }
        Logger.LogInformation($"{nameof(MedicationsNotifier)} has sent {sentNotifications.Count} notifications @ {DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss")}");
    }
}