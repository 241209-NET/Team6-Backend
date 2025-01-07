using SocialMedia.API.DTO;
using SocialMedia.API.Model;

namespace SocialMedia.API.Util;

public static class Utilities
{
    public static User UserDTOToObject(UserInDTO userDTO)
    {
        return new User { Username = userDTO.Username, Password = userDTO.Password };
    }

    public static Tweet TweetDTOToObject(TweetInDTO tweetDTO)
    {
        return new Tweet
        {
            Body = tweetDTO.Body,
            UserId = tweetDTO.UserId,
            ParentId = tweetDTO.ParentId,
        };
    }
}
