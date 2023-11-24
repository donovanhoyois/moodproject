using System.ComponentModel.DataAnnotations.Schema;

namespace MoodProject.Core.Models;

public class ChatRoom
{
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	public string Name { get; set; }
	public Symptom Symptom { get; set; }
	public IEnumerable<ChatRoomPost> Posts { get; set; }
}