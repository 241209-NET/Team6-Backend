namespace SocialMedia.API.Model;

public class Tweet
{
    public int Id { get; set; }
    public required string Body { get; set; }
    public int Likes { get; set; } = 0;
    public int? ParentId { get; set; }
    public DateTime CreatedAt { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }
    public ICollection<Tweet>? Replies { get; set; }
}
