using System.ComponentModel.DataAnnotations.Schema;
using MoodProject.Core.Enums;

namespace MoodProject.Core.Models;

public class ChatRoomComment
{
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	public int PostId { get; set; }
	public string AuthorId { get; set; }
	public string Content { get; set; }
	public ModerationStatus ModerationStatus { get; set; }
	public DateTime PublishedDate { get; set; }
}