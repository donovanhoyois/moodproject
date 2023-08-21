using MoodProject.Core.Enums;
using MoodProject.Core.Models;
using MoodProject.Core.Ports.In;
using MoodProject.Core.Ports.Out;

namespace MoodProject.Services;

public class ChatRoomsService : IChatRoomsService
{
	private readonly IAppApi AppApi;
	
	public ChatRoomsService(IAppApi appApi)
	{
		AppApi = appApi;
	}
	
	public async Task<OperationResult<IEnumerable<ChatRoom>>> GetRooms(string userId)
	{
		var apiResponse = await AppApi.GetChatRooms(userId);
		return apiResponse.Any()
			? new OperationResult<IEnumerable<ChatRoom>>(OperationResultType.Ok)
			{
				Content = apiResponse
			}
			: new OperationResult<IEnumerable<ChatRoom>>(OperationResultType.Error)
			{
				Message = "Aucun espace de discussion n'est disponible pour vous."
			};
	}

	public async Task<OperationResult<ChatRoom>> GetRoomById(int id)
	{
		var apiResponse = await AppApi.GetChatRoom(id);
		if (apiResponse == null)
		{
			return new OperationResult<ChatRoom>(OperationResultType.Error)
			{
				Message = "Cet espace de discussion est introuvable."
			};
		}

		if (apiResponse.Posts.Any())
		{
			return new OperationResult<ChatRoom>(OperationResultType.Ok)
			{
				Content = apiResponse
			};
		}

		return new OperationResult<ChatRoom>(OperationResultType.Error)
		{
			Message = "Aucune publication n'est disponible."
		};

	}


	public async Task<OperationResult<ChatRoomPost>> GetPostById(int id)
	{
		var apiResponse = await AppApi.GetChatPost(id);
		if (apiResponse == null)
		{
			return new OperationResult<ChatRoomPost>(OperationResultType.Error)
			{
				Message = "Ce post est introuvable."
			};
		}

		return new OperationResult<ChatRoomPost>(OperationResultType.Ok)
		{
			Content = apiResponse
		};

	}
}