using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MoodProject.Core.Models.Notifications;

namespace MoodProject.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]/")]
[EnableCors]
public class NotificationSubscriptionsController
{
    private readonly ILogger<MedicationsController> Logger;
    private readonly MoodProjectContext DbContext;
    
    public NotificationSubscriptionsController(ILogger<MedicationsController> logger, MoodProjectContext dbContext)
    {
        Logger = logger;
        DbContext = dbContext;
    }
    
    [HttpPut, ActionName("RegisterNewNotificationSubscription")]
    public bool Register(NotificationSubscription notificationSubscription)
    {
        var foundSubscription = DbContext.NotificationSubscriptions.FirstOrDefault(sub => sub.UserId.Equals(notificationSubscription.UserId));
        if (foundSubscription != null)
        {
            DbContext.Remove(foundSubscription);
        }

        DbContext.NotificationSubscriptions.Add(notificationSubscription);
        return DbContext.SaveChanges() > 0;
    }
}