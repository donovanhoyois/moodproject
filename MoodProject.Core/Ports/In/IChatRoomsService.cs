using MoodProject.Core.Models;

namespace MoodProject.Core.Ports.In;

public interface IChatRoomsService
{
	public Task<OperationResult<IEnumerable<ChatRoom>>> GetRooms(string userId);
	public Task<OperationResult<ChatRoom>> GetRoomById(int id);
	public Task<OperationResult<ChatRoomPost>> GetPostById(int id);
}