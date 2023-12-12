namespace MoodProject.Core.Models.Notifications;

public class MedicationNotification
{
    public string UserId { get; set; }
    public string MedicationName { get; init; }
    public TimeSpan Time { get; set; }
    
    public MedicationNotification(string userId, string medicationName, TimeSpan time)
    {
        UserId = userId;
        MedicationName = medicationName;
        Time = time;
    }
}