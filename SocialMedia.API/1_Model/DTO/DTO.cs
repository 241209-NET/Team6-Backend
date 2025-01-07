namespace SocialMedia.API.DTO;

public class UserInDTO
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public class TweetInDTO
{
    public required string Body { get; set; }
    public int UserId { get; set; }
    public int? ParentId { get; set; }
}
