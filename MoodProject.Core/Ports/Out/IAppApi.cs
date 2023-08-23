using MoodProject.Core.Models;

namespace MoodProject.Core.Ports.Out;

public interface IAppApi
{
    public Task<IEnumerable<SymptomType>> GetSymptomsTypes();
    public Task<IEnumerable<Symptom>> GetSymptoms(string userId);
    public Task<bool> SaveSymptoms(IEnumerable<Symptom> symptoms);
    public Task<IEnumerable<Symptom>> GetSymptomsWithHistory(string userId);
    public Task<bool> SaveSymptomsHistory(IEnumerable<FactorValue> values);
    public Task<IEnumerable<CustomQuizzQuestion>> GetCustomQuestions();
    public Task<IEnumerable<ChatRoom>> GetChatRooms(string userId);
    public Task<ChatRoom> GetChatRoom(int id);
    public Task<IEnumerable<ChatRoomPost>> GetUnpublishedPosts();
    public Task<ChatRoomPost> GetChatRoomPost(int id);
    public Task<IEnumerable<ChatRoomPost>> GetChatRoomPostsOfUser(string userId);
    public Task<bool> CreateChatRoomPost(ChatRoomPost post);
    public Task<bool> UpdateChatRoomPost(ChatRoomPost post);
    public Task<IEnumerable<ChatRoomComment>> GetUnpublishedComments();
    public Task<IEnumerable<ChatRoomComment>> GetChatRoomCommentsOfUser(string userId);
    public Task<bool> CreateChatRoomComment(ChatRoomComment comment);
    public Task<bool> UpdateChatRoomComment(ChatRoomComment comment);
}