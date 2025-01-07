namespace SocialMedia.API.Model;

using System.Text.Json.Serialization;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}
