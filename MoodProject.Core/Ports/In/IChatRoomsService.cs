using MoodProject.Core.Enums;
using MoodProject.Core.Models;

namespace MoodProject.Core.Ports.In;

public interface IChatRoomsService
{
	public Task<OperationResult<IEnumerable<ChatRoom>>> GetRooms(string userId);
	public Task<OperationResult<ChatRoom>> GetRoomById(int id);
	public Task<OperationResult<ChatRoomPost>> GetPostById(int id);
	public Task<OperationResult<IEnumerable<ChatRoomPost>>> GetPosts(ModerationStatus moderationStatus);
	public Task<OperationResult<IEnumerable<ChatRoomPost>>> GetPostsOfUser(string userId);
	public Task<OperationResult<ChatRoomPost>> CreatePost(ChatRoomPost post);
	public Task<OperationResult<ChatRoomPost>> UpdatePost(ChatRoomPost post);
	public Task<OperationResult<IEnumerable<ChatRoomComment>>> GetComments(ModerationStatus moderationStatus);
	public Task<OperationResult<IEnumerable<ChatRoomComment>>> GetCommentsOfUser(string userId);
	public Task<OperationResult<ChatRoomComment>> CreateComment(ChatRoomComment comment);
	public Task<OperationResult<ChatRoomComment>> UpdateComment(ChatRoomComment comment);
}