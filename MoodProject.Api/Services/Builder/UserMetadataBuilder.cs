using MoodProject.Core.Models.User;

namespace MoodProject.Api.Services.Builder;

public class UserMetadataBuilder
{
    private UserMetadata Metadata;

    public UserMetadataBuilder()
    {
        Metadata = new UserMetadata();
    }

    public UserMetadataBuilder WithHasAcceptedGdpr(bool value)
    {
        Metadata.HasAcceptedGdpr = value;
        return this;
    }

    public UserMetadataBuilder WithHasChosenNickname(bool value)
    {
        Metadata.HasChosenNickname = value;
        return this;
    }

    public UserMetadata Build()
    {
        return Metadata;
    }
}