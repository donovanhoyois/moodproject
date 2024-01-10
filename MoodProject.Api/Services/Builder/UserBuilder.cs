using MoodProject.Core.Models.User;

namespace MoodProject.Api.Services.Builder;

public class UserBuilder
{
    private BaseUser User { get; set; }

    public UserBuilder()
    {
        User = new BaseUser()
        {
            Metadata = new UserMetadata()
        };
    }

    public UserBuilder WithNickname(string nickname)
    {
        User.Nickname = nickname;
        return this;
    }

    public UserBuilder WithMetadata(UserMetadata metadata)
    {
        User.Metadata = metadata;
        return this;
    }

    public BaseUser Build()
    {
        return User;
    }
}