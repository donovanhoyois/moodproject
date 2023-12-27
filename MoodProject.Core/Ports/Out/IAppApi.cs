using MoodProject.Core.Enums;
using MoodProject.Core.Models;
using MoodProject.Core.Models.Notifications;

namespace MoodProject.Core.Ports.Out;

public interface IAppApi
{
    public Task<string> GetToken(string userId);
    public Task<IEnumerable<SymptomType>> GetSymptomsTypes();
    public Task<IEnumerable<Symptom>> GetSymptomsByUserId(string userId);
    public Task<bool> SaveSymptoms(IEnumerable<Symptom> symptoms);
    public Task<IEnumerable<Symptom>> GetSymptomsWithHistoryByUserId(string userId);
    public Task<bool> SaveSymptomsHistory(IEnumerable<FactorValue> values);
    public Task<IEnumerable<CustomQuizzQuestion>> GetCustomQuestions();
    public Task<IEnumerable<ChatRoom>> GetRooms(string userId);
    public Task<ChatRoom> GetRoom(int id);
    public Task<IEnumerable<ChatRoomPost>> GetPostsByUserId(string userId);
    public Task<IEnumerable<ChatRoomPost>> GetPostsByModerationStatus(ModerationStatus moderationStatus);
    public Task<ChatRoomPost> GetPost(int id);
    public Task<bool> CreatePost(ChatRoomPost post);
    public Task<bool> UpdatePost(ChatRoomPost post);
    public Task<IEnumerable<ChatRoomComment>> GetCommentsByUserId(string userId);
    public Task<IEnumerable<ChatRoomComment>> GetCommentsByModerationStatus(ModerationStatus moderationStatus);
    public Task<bool> CreateComment(ChatRoomComment comment);
    public Task<bool> UpdateComment(ChatRoomComment comment);
    public Task<bool> GetGdprConsent(string authProviderId);
    public Task<bool> AcceptGdpr(string authProviderId);
    public Task<IEnumerable<Medication>> GetMedicationsByUserId(string userId);
    public Task<bool> UpdateMedications(IEnumerable<Medication> medications);
    public Task<bool> DeleteMedications(IEnumerable<Medication> medications);
    public Task<bool> RegisterNewNotificationSubscription(NotificationSubscription notificationSubscription);
    public Task<bool> UploadFile(Stream stream);
}