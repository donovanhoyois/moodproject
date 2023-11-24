using MoodProject.Core.Models;

namespace MoodProject.App.Models;

public class NewChatRoomCommentForm : ChatRoomComment
{
    public bool MandatoryBoxIsChecked { get; set; }
}