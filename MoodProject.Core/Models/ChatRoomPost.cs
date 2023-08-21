using MoodProject.Core.Enums;

namespace MoodProject.Core.Models;

public class ChatRoomPost
{
	public int Id { get; set; }
	public string AuthorId { get; set; }
	public string Title { get; set; }
	public string Post { get; set; }
	public ModerationStatus ModerationStatus { get; set; }
	public DateTime PublishedDate { get; set; }
	public IEnumerable<ChatRoomComment> Comments { get; set; }
}