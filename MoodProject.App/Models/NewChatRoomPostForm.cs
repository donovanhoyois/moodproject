using MoodProject.Core.Models;

namespace MoodProject.App.Models;

public class NewChatRoomPostForm : ChatRoomPost
{
    public bool MandatoryBoxIsChecked { get; set; }
}