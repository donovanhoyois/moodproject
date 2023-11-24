using MoodProject.Core.Enums;
using MoodProject.Core.Models;
using MoodProject.Core.Ports.In;
using MoodProject.Core.Ports.Out;

namespace MoodProject.Services;

public class ChatRoomsService : IChatRoomsService
{
	private readonly IAppApi AppApi;
	
	private const int POST_TITLE_MAX_LENGTH = 64;
	private const int POST_CONTENT_MAX_LENGTH = 2000;
	private const int COMMENT_MAX_LENGTH = 2000;

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

	public async Task<OperationResult<IEnumerable<ChatRoomPost>>> GetPosts(ModerationStatus moderationStatus)
	{
		var apiResponse = await AppApi.GetPosts(moderationStatus);
		return apiResponse != null
			? new OperationResult<IEnumerable<ChatRoomPost>>(OperationResultType.Ok)
			{
				Content = apiResponse
			}
			: new OperationResult<IEnumerable<ChatRoomPost>>(OperationResultType.Error)
			{
				Message = "Une erreur est survenue lors de la récupération des posts en attente de modération."
			};
	}

	public async Task<OperationResult<IEnumerable<ChatRoomPost>>> GetPostsOfUser(string userId)
	{
		var apiResponse = await AppApi.GetChatRoomPostsOfUser(userId);
		return apiResponse != null
			? new OperationResult<IEnumerable<ChatRoomPost>>(OperationResultType.Ok)
			{
				Content = apiResponse
			}
			: new OperationResult<IEnumerable<ChatRoomPost>>(OperationResultType.Error)
			{
				Message = "Une erreur est survenue lors de la récupération de vos publications."
			};
	}

	public async Task<OperationResult<ChatRoomPost>> CreatePost(ChatRoomPost post)
	{
		if (post.Title is null or "")
		{
			return new OperationResult<ChatRoomPost>(OperationResultType.Error)
			{
				Message = "Le titre de la publication est trop court."
			};
		}
		if (post.Content is null or "")
		{
			return new OperationResult<ChatRoomPost>(OperationResultType.Error)
			{
				Message = "Le contenu de la publication est trop court."
			};
		}
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

	public async Task<OperationResult<ChatRoomPost>> UpdatePost(ChatRoomPost post)
	{
		var apiResponse = await AppApi.UpdateChatRoomPost(post);
		return apiResponse
			? new OperationResult<ChatRoomPost>(OperationResultType.Ok)
			{
				Message = "La publication a bien été mise à jour."
			}
			: new OperationResult<ChatRoomPost>(OperationResultType.Error)
			{
				Message = "La publication n'a pas pu être mise à jour."
			};
	}

	public async Task<OperationResult<IEnumerable<ChatRoomComment>>> GetComments(ModerationStatus moderationStatus)
	{
		var apiResponse = await AppApi.GetComments(moderationStatus);
		return apiResponse != null
			? new OperationResult<IEnumerable<ChatRoomComment>>(OperationResultType.Ok)
			{
				Content = apiResponse
			}
			: new OperationResult<IEnumerable<ChatRoomComment>>(OperationResultType.Error)
			{
				Message = "Une erreur est survenue lors de la récupération des commentaires en attente de modération."
			};
	}

	public async Task<OperationResult<IEnumerable<ChatRoomComment>>> GetCommentsOfUser(string userId)
	{
		var apiResponse = await AppApi.GetChatRoomCommentsOfUser(userId);
		return apiResponse != null
			? new OperationResult<IEnumerable<ChatRoomComment>>(OperationResultType.Ok)
			{
				Content = apiResponse
			}
			: new OperationResult<IEnumerable<ChatRoomComment>>(OperationResultType.Error)
			{
				Message = "Une erreur est survenue lors de la récupération de vos commentaires."
			};
	}

	public async Task<OperationResult<ChatRoomComment>> CreateComment(ChatRoomComment comment)
	{
		if (comment.Content is null or "")
		{
			return new OperationResult<ChatRoomComment>(OperationResultType.Error)
			{
				Message = "Le commentaire est trop court."
			};
		}
		if (comment.Content.Length > COMMENT_MAX_LENGTH)
		{
			return new OperationResult<ChatRoomComment>(OperationResultType.Error)
			{
				Message = "Le commentaire est trop long."
			};
		}

		var apiResponse = await AppApi.CreateChatRoomComment(comment);
		return apiResponse
			? new OperationResult<ChatRoomComment>(OperationResultType.Ok)
			{
				Message = "Votre commentaire a été envoyé. Une fois accepté par la modération, celui-ci sera public."
			}
			: new OperationResult<ChatRoomComment>(OperationResultType.Error)
			{
				Message = "Une erreur est survenue, veuillez nous en excuser."
			};
	}

	public async Task<OperationResult<ChatRoomComment>> UpdateComment(ChatRoomComment comment)
	{
		var apiResponse = await AppApi.UpdateChatRoomComment(comment);
		return apiResponse
			? new OperationResult<ChatRoomComment>(OperationResultType.Ok)
			{
				Message = "Le commentaire a bien été mis à jour."
			}
			: new OperationResult<ChatRoomComment>(OperationResultType.Error)
			{
				Message = "Le commentaire n'a pas pu être mis à jour."
			};
	}
}