using System.Net.Http.Json;
using MoodProject.Core;
using MoodProject.Core.Models;
using MoodProject.Core.Ports.Out;

namespace MoodProject.Services;

public class AppApi : IAppApi
{
    private HttpClient ApiClient;
    public AppApi(HttpClient apiClient)
    {
        ApiClient = apiClient;
    }

    public async Task<IEnumerable<SymptomType>> GetSymptomsTypes()
    {
        return await ApiClient.GetFromJsonAsync<IEnumerable<SymptomType>>("SymptomsTypes/GetAll");
    }

    public async Task<IEnumerable<Symptom>> GetSymptoms(string userId)
    {
        return await ApiClient.GetFromJsonAsync<IEnumerable<Symptom>>($"Symptoms/Get?userId={userId}");
    }

    public async Task<bool> SaveSymptoms(IEnumerable<Symptom> symptoms)
    {
        var response = await ApiClient.PostAsJsonAsync($"Symptoms/Update", symptoms);
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<Symptom>> GetSymptomsWithHistory(string userId)
    {
        return await ApiClient.GetFromJsonAsync<IEnumerable<Symptom>>($"Symptoms/GetHistory?userId={userId}");
    }

    public async Task<bool> SaveSymptomsHistory(IEnumerable<FactorValue> values)
    {
        var response = await ApiClient.PostAsJsonAsync($"Symptoms/UpdateHistory", values);
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<CustomQuizzQuestion>> GetCustomQuestions()
    {
        return await ApiClient.GetFromJsonAsync<IEnumerable<CustomQuizzQuestion>>("CustomQuizzQuestions/GetAll");
    }

    public async Task<IEnumerable<ChatRoom>> GetChatRooms(string userId)
    {
        return await ApiClient.GetFromJsonAsync<IEnumerable<ChatRoom>>($"ChatRooms/GetRoomsAccessibleByUser?userId={userId}");
    }

    public async Task<ChatRoom> GetChatRoom(int id)
    {
        return await ApiClient.GetFromJsonAsync<ChatRoom>($"ChatRooms/GetRoom?id={id}");
    }

    public async Task<IEnumerable<ChatRoomPost>> GetUnpublishedPosts()
    {
        return await ApiClient.GetFromJsonAsync<IEnumerable<ChatRoomPost>>($"ChatRooms/GetPendingPosts");
    }

    public async Task<ChatRoomPost> GetChatRoomPost(int id)
    {
        return await ApiClient.GetFromJsonAsync<ChatRoomPost>($"ChatRooms/GetPost?id={id}");
    }

    public async Task<IEnumerable<ChatRoomPost>> GetChatRoomPostsOfUser(string userId)
    {
        return await ApiClient.GetFromJsonAsync<IEnumerable<ChatRoomPost>>($"ChatRooms/GetPostsOfUser?userId={userId}");
    }

    public async Task<bool> CreateChatRoomPost(ChatRoomPost post)
    {
        var response = await ApiClient.PostAsJsonAsync($"ChatRooms/CreatePost", post);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateChatRoomPost(ChatRoomPost post)
    {
        var response = await ApiClient.PatchAsJsonAsync($"ChatRooms/UpdatePost", post);
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<ChatRoomComment>> GetUnpublishedComments()
    {
        return await ApiClient.GetFromJsonAsync<IEnumerable<ChatRoomComment>>($"ChatRooms/GetPendingComments");
    }

    public async Task<IEnumerable<ChatRoomComment>> GetChatRoomCommentsOfUser(string userId)
    {
        return await ApiClient.GetFromJsonAsync<IEnumerable<ChatRoomComment>>($"ChatRooms/GetCommentsOfUser?userId={userId}");
    }

    public async Task<bool> CreateChatRoomComment(ChatRoomComment comment)
    {
        var response = await ApiClient.PostAsJsonAsync($"ChatRooms/CreateComment", comment);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateChatRoomComment(ChatRoomComment comment)
    {
        var response = await ApiClient.PatchAsJsonAsync($"ChatRooms/UpdateComment", comment);
        return response.IsSuccessStatusCode;
    }
}