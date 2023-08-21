namespace MoodProject.Core.Models;

public class ChatRoom
{
	public int Id { get; set; }
	public string Name { get; set; }
	public Symptom Symptom { get; set; }
	public IEnumerable<ChatRoomPost> Posts { get; set; }
}