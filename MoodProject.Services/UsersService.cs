using System.Net;
using MoodProject.Core.Enums;
using MoodProject.Core.Models;
using MoodProject.Core.Ports.In;
using MoodProject.Core.Ports.Out;

namespace MoodProject.Services;

public class UsersService : IUsersService
{
    private readonly IAppApi AppApi;

    public UsersService(IAppApi appApi)
    {
        AppApi = appApi;
    }
    
    public async Task<OperationResult<bool>> GetGdprConsent(string authProviderId)
    {
        var response = await AppApi.GetGdprConsent(authProviderId);
        return new OperationResult<bool>(OperationResultType.Ok)
        {
            Content = response
        };
    }

    public async Task<OperationResult<bool>> AcceptGdpr(string authProviderId)
    {
        var response = await AppApi.AcceptGdpr(authProviderId);
        return new OperationResult<bool>(OperationResultType.Ok)
        {
            Content = response
        };
    }

    public async Task<OperationResult<string>> GetNickname(string authProviderId)
    {
        var response = await AppApi.GetUsername(authProviderId);
        return new OperationResult<string>(OperationResultType.Ok)
        {
            Content = response
        };
    }

    public async Task<OperationResult<bool>> GetHasChosenNickname(string authProviderId)
    {
        var response = await AppApi.GetHasChosenNickanme(authProviderId);
        return new OperationResult<bool>(OperationResultType.Ok)
        {
            Content = response
        };
    }

    public async Task<OperationResult<HttpStatusCode>> UpdateNickname(string authProviderId, string newNickname)
    {
        var response = await AppApi.UpdateNickname(authProviderId, newNickname);
        return response == HttpStatusCode.OK
            ? new OperationResult<HttpStatusCode>(OperationResultType.Ok)
            {
                Content = response,
                Message = "Votre surnom a bien été mis à jour."
            }
            : new OperationResult<HttpStatusCode>(OperationResultType.Error)
            {
                Content = response,
                Message = response == HttpStatusCode.Forbidden ? "Le surnom existe déjà" : "Erreur inconnue"
            };
    }

    public async Task<OperationResult<Dictionary<string, string>>> GetUsernamesMapping(IEnumerable<string> userIds)
    {
        var apiResponse = await AppApi.GetUsernamesMapping(userIds);
        return apiResponse.Any()
            ? new OperationResult<Dictionary<string, string>>(OperationResultType.Ok)
            {
                Content = apiResponse
            }
            : new OperationResult<Dictionary<string, string>>(OperationResultType.Error)
            {
                Content = apiResponse,
                Message = "Une erreur est survenue lors de la récupération des noms d'utilisateurs."
            };

    }
}