using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoodProject.Core.Enums;
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

	[HttpGet, ActionName("GetPendingPosts")]
	public IEnumerable<ChatRoomPost> GetPendingPosts()
	{
		return DbContext.ChatRoomPosts
			.Where(post => post.ModerationStatus.Equals(ModerationStatus.Pending));
	}

	[HttpGet, ActionName("GetPostsOfUser")]
	public IEnumerable<ChatRoomPost> GetPostsOfUser(string userId)
	{
		return DbContext.ChatRoomPosts.Where(p => p.AuthorId.Equals(userId));
	}

	[HttpPost, ActionName("CreatePost")]
	public bool CreatePost(ChatRoomPost post)
	{
		DbContext.ChatRoomPosts.Add(post);
		var nbChanges = DbContext.SaveChanges();
		return nbChanges > 0;
	}

	[HttpPatch, ActionName("UpdatePost")]
	public bool UpdatePost(ChatRoomPost post)
	{
		var retrievedPost = DbContext.ChatRoomPosts.FirstOrDefault(p => p.Id.Equals(post.Id));
		if (retrievedPost != null)
		{
			DbContext.Entry(retrievedPost).CurrentValues.SetValues(post);
			return DbContext.SaveChanges() > 0;
		}

		return false;
	}

	[HttpGet, ActionName("GetPendingComments")]
	public IEnumerable<ChatRoomComment> GetPendingComments()
	{
		return DbContext.ChatRoomComments
			.Where(comment => comment.ModerationStatus.Equals(ModerationStatus.Pending));
	}

	[HttpGet, ActionName("GetCommentsOfUser")]
	public IEnumerable<ChatRoomComment> GetCommentsOfUser(string userId)
	{
		return DbContext.ChatRoomComments.Where(p => p.AuthorId.Equals(userId));
	}

	[HttpPost, ActionName("CreateComment")]
	public bool CreateComment(ChatRoomComment comment)
	{
		DbContext.ChatRoomComments.Add(comment);
		var nbChanges = DbContext.SaveChanges();
		return nbChanges > 0;
	}

	[HttpPatch, ActionName("UpdateComment")]
	public bool UpdateComment(ChatRoomComment comment)
	{
		var retrievedComment = DbContext.ChatRoomComments.FirstOrDefault(c => c.Id.Equals(comment.Id));
		if (retrievedComment != null)
		{
			DbContext.Entry(retrievedComment).CurrentValues.SetValues(comment);
			return DbContext.SaveChanges() > 0;
		}

		return false;
	}
	
}