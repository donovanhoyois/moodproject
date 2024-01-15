using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using MoodProject.Core.Configuration;
using MoodProject.Core.Enums;
using MoodProject.Core.Models;
using MoodProject.Core.Models.Notifications;
using MoodProject.Core.Ports.Out;

namespace MoodProject.Services;

public class AppApi : IAppApi
{
    private readonly HttpClient ApiClient;
    public AppApi(ApiConfiguration config, HttpClient apiClient)
    {
        ApiClient = apiClient;
        ApiClient.BaseAddress = new Uri(config.BaseUrl);
    }

    public async Task<string> GetToken(string userId)
    {
        var newToken = await ApiClient.GetFromJsonAsync<Token>($"Authentication/GetToken?userId={userId}");
        ApiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newToken.Value);
        return newToken.Value;
    }

    public async Task<IEnumerable<SymptomType>> GetSymptomsTypes()
    {
        return await ApiClient.GetFromJsonAsync<IEnumerable<SymptomType>>("SymptomsTypes/GetAll");
    }

    public async Task<IEnumerable<Symptom>> GetSymptomsByUserId(string userId)
    {
        return await ApiClient.GetFromJsonAsync<IEnumerable<Symptom>>($"Symptoms/GetSymptomsByUserId?userId={userId}");
    }

    public async Task<bool> SaveSymptoms(IEnumerable<Symptom> symptoms)
    {
        var response = await ApiClient.PostAsJsonAsync($"Symptoms/Update", symptoms);
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<Symptom>> GetSymptomsWithHistoryByUserId(string userId)
    {
        return await ApiClient.GetFromJsonAsync<IEnumerable<Symptom>>($"Symptoms/GetSymptomsWithHistoryByUserId?userId={userId}");
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

    public async Task<IEnumerable<ChatRoom>> GetRooms(string userId)
    {
        return await ApiClient.GetFromJsonAsync<IEnumerable<ChatRoom>>($"ChatRooms/getRoomsByUserId?userId={userId}");
    }

    public async Task<ChatRoom> GetRoom(int id)
    {
        return await ApiClient.GetFromJsonAsync<ChatRoom>($"ChatRooms/GetRoom?id={id}");
    }

    public async Task<ChatRoomPost> GetPost(int id)
    {
        return await ApiClient.GetFromJsonAsync<ChatRoomPost>($"ChatRooms/GetPost?id={id}");
    }

    public async Task<IEnumerable<ChatRoomPost>> GetPostsByUserId(string userId)
    {
        return await ApiClient.GetFromJsonAsync<IEnumerable<ChatRoomPost>>($"ChatRooms/GetPostsByUserId?userId={userId}");
    }
    
    public async Task<IEnumerable<ChatRoomPost>> GetPostsByModerationStatus(ModerationStatus moderationStatus)
    {
        return await ApiClient.GetFromJsonAsync<IEnumerable<ChatRoomPost>>($"ChatRooms/GetPostsByModerationStatus?moderationStatus={moderationStatus}");
    }

    public async Task<bool> CreatePost(ChatRoomPost post)
    {
        var response = await ApiClient.PostAsJsonAsync($"ChatRooms/CreatePost", post);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdatePost(ChatRoomPost post)
    {
        var response = await ApiClient.PatchAsJsonAsync($"ChatRooms/UpdatePost", post);
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<ChatRoomComment>> GetCommentsByUserId(string userId)
    {
        return await ApiClient.GetFromJsonAsync<IEnumerable<ChatRoomComment>>($"ChatRooms/GetCommentsByUserId?userId={userId}");
    }
    
    public async Task<IEnumerable<ChatRoomComment>> GetCommentsByModerationStatus(ModerationStatus moderationStatus)
    {
        return await ApiClient.GetFromJsonAsync<IEnumerable<ChatRoomComment>>($"ChatRooms/GetCommentsByModerationStatus?moderationStatus={moderationStatus}");
    }

    public async Task<bool> CreateComment(ChatRoomComment comment)
    {
        var response = await ApiClient.PostAsJsonAsync($"ChatRooms/CreateComment", comment);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateComment(ChatRoomComment comment)
    {
        var response = await ApiClient.PatchAsJsonAsync($"ChatRooms/UpdateComment", comment);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> GetGdprConsent(string authProviderId)
    {
        return await ApiClient.GetFromJsonAsync<bool>($"Users/GetGdprConsent?authProviderId={authProviderId}");
    }

    public async Task<bool> AcceptGdpr(string authProviderId)
    {
        var response = await ApiClient.PostAsJsonAsync($"Users/AcceptGdpr?authProviderId={authProviderId}", "");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> GetHasChosenNickanme(string authProviderId)
    {
        return await ApiClient.GetFromJsonAsync<bool>($"Users/GetHasChosenNickname?authProviderId={authProviderId}");
    }

    public async Task<string> GetUsername(string authProviderId)
    {
        return await ApiClient.GetStringAsync($"Users/GetNickname?authProviderId={authProviderId}");
    }

    public async Task<HttpStatusCode> UpdateNickname(string authProviderId, string newNickname)
    {
        var response = await ApiClient.PostAsJsonAsync($"Users/UpdateNickname?authProviderId={authProviderId}&nickname={newNickname}", "");
        return response.StatusCode;
    }

    public async Task<Dictionary<string, string>> GetUsernamesMapping(IEnumerable<string> userIds)
    {
        var response = await ApiClient.PostAsJsonAsync($"Users/GetUsernamesMapping", userIds);
        return await response.Content.ReadFromJsonAsync<Dictionary<string, string>>() ?? new Dictionary<string, string>();
    }

    public async Task<IEnumerable<Medication>> GetMedicationsByUserId(string userId)
    {
        return await ApiClient.GetFromJsonAsync<IEnumerable<Medication>>($"Medications/GetMedicationsByUserId?userId={userId}");
    }

    public async Task<bool> UpdateMedications(IEnumerable<Medication> medications)
    {
        var response = await ApiClient.PatchAsJsonAsync($"Medications/UpdateMedications", medications);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteMedications(IEnumerable<Medication> medications)
    {
        var response = await ApiClient.PostAsJsonAsync("Medications/DeleteMedications", medications);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RegisterNewNotificationSubscription(NotificationSubscription notificationSubscription)
    {
        var response = await ApiClient.PutAsJsonAsync("NotificationSubscriptions/RegisterNewNotificationSubscription", notificationSubscription);
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<Resource>> GetRessources()
    {
        return await ApiClient.GetFromJsonAsync<IEnumerable<Resource>>("Resources/GetAll");
    }

    public async Task<Resource?> GetRessource(int id)
    {
        return await ApiClient.GetFromJsonAsync<Resource?>($"Resources/GetById?id={id}");
    }

    public async Task<Resource> CreateRessource(Resource resource)
    {
        var response = await ApiClient.PutAsJsonAsync("Resources/Create", resource);
        return await response.Content.ReadFromJsonAsync<Resource>();
    }

    public async Task<bool> DeleteResource(int id)
    {
        return await ApiClient.DeleteFromJsonAsync<bool>($"Resources/Delete?id={id}");
    }

    public async Task<string> UploadFile(FileWithContent fileWithContent)
    {
        var response = await ApiClient.PutAsJsonAsync("Files/Upload", fileWithContent);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<FileWithContent?> DownloadFile(int id)
    {
        return await ApiClient.GetFromJsonAsync<FileWithContent?>($"Files/Download?id={id}");
    }

    public async Task<bool> DeleteFile(int id)
    {
        return await ApiClient.DeleteFromJsonAsync<bool>($"Files/Delete?id={id}");
    }
}