using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoodProject.Core.Models;

namespace MoodProject.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]/")]
[EnableCors]
public class ChatRoomsController
{
	private MoodProjectContext DbContext;
    
	public ChatRoomsController(MoodProjectContext dbContext)
	{
		DbContext = dbContext;
	}
	
	[HttpGet, ActionName("GetAllRooms")]
	public IEnumerable<ChatRoom> GetALlRooms()
	{
		return DbContext.ChatRooms.Include(r => r.Posts);
	}

	[HttpGet, ActionName("GetRoomsAccessibleByUser")]
	public IEnumerable<ChatRoom> GetRoomsAccessibleByUser(string userId)
	{
		var userSymptomsTypesIds =
			DbContext.Symptoms
				.Where(s => s.UserId.Equals(userId) && !s.isDisabled)
				.Select(s => s.Type.Id);

		return DbContext.ChatRooms
			.Where(r => userSymptomsTypesIds.Contains(r.Id))
			.Include(r => r.Posts);
	}

	[HttpGet, ActionName("GetRoom")]
	public ChatRoom GetRoom(int id)
	{
		return DbContext.ChatRooms
			.Where(r => r.Id.Equals(id))
			.Include(r => r.Posts)
			.First();
	}

	[HttpGet, ActionName("GetPost")]
	public ChatRoomPost GetPost(int id)
	{
		return DbContext.ChatRoomPosts
			.Where(p => p.Id.Equals(id))
			.Include(p => p.Comments)
			.FirstOrDefault();
	}
	
}