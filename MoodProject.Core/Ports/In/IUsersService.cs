using System.Net;
using MoodProject.Core.Models;

namespace MoodProject.Core.Ports.In;

public interface IUsersService
{
    public Task<OperationResult<bool>> GetGdprConsent(string authProviderId);
    public Task<OperationResult<bool>> AcceptGdpr(string authProviderId);
    public Task<OperationResult<string>> GetNickname(string authProviderId);
    public Task<OperationResult<bool>> GetHasChosenNickname(string authProviderId);
    public Task<OperationResult<HttpStatusCode>> UpdateNickname(string authProviderId, string newNickname);
    public Task<OperationResult<Dictionary<string, string>>> GetUsernamesMapping(IEnumerable<string> userIds);
}