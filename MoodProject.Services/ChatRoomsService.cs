using MoodProject.Core.Enums;
using MoodProject.Core.Models;
using MoodProject.Core.Ports.In;
using MoodProject.Core.Ports.Out;

namespace MoodProject.Services;

public class ChatRoomsService : IChatRoomsService
{
	private readonly IAppApi AppApi;
	
	private const int POST_TITLE_MAX_LENGTH = 64;
	public const int POST_CONTENT_MAX_LENGTH = 2000;

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
		var apiResponse = await AppApi.GetChatRoomPost(id);
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

	public async Task<OperationResult<ChatRoomPost>> CreatePost(ChatRoomPost post)
	{
		if (post.Title.Length > POST_TITLE_MAX_LENGTH)
		{
			return new OperationResult<ChatRoomPost>(OperationResultType.Error)
			{
				Message = "Le titre est trop long."
			};
		}

		if (post.Content.Length > POST_CONTENT_MAX_LENGTH)
		{
			return new OperationResult<ChatRoomPost>(OperationResultType.Error)
			{
				Message = "Le contenu est trop long."
			};
		}

		var apiResponse = await AppApi.CreateChatRoomPost(post);
		return apiResponse
			? new OperationResult<ChatRoomPost>(OperationResultType.Ok)
			{
				Message = "Votre nouvelle publication a été envoyée. Une fois acceptée par la modération, elle sera publique."
			}
			: new OperationResult<ChatRoomPost>(OperationResultType.Error)
			{
				Message = "Une erreur est survenue, veuillez nous en excuser."
			};
	}
}