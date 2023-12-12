using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MoodProject.Api.Interfaces;

namespace MoodProject.Api.Services;

[Authorize]
public class NotificationsHub : Hub<INotificationClient>
{
    private ILogger<NotificationsHub> Logger;
    public NotificationsHub(ILogger<NotificationsHub> logger)
    {
        Logger = logger;
        Logger.LogInformation("{Service} initialized.", nameof(NotificationsHub));
    }
    public override async Task OnConnectedAsync()
    {
        Logger.LogInformation("New user logged to {service}: {user}", nameof(NotificationsHub), Context.User?.Identity?.Name);
        await base.OnConnectedAsync();
    }
}