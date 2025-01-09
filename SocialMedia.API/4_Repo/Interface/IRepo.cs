using SocialMedia.API.Model;

namespace SocialMedia.API.Repo;

public interface IUserRepo
{
    Task<User> CreateUser(User newUser);
    Task<IEnumerable<User>> GetAllUsers();
    Task<User>? GetUserById(int id);
    Task<User>? GetUserByUsername(string username);
    Task<User>? DeleteUserById(int id);
}

public interface ITweetRepo
{
    Task<Tweet> CreateTweet(Tweet newTweet);
    Task<Tweet>? GetTweetById(int id);
    Task<IEnumerable<Tweet>> GetAllTweets();
    Task<IEnumerable<Tweet>> GetTweetsByUserId(int userId);
    Task<Tweet>? UpdateTweet(int id, string newBody);
    Task<bool> LikeTweet(int id);
    Task<bool> UnlikeTweet(int id);
    Task<bool> DeleteTweet(int id);
    Task<IEnumerable<Tweet>> GetRepliesForTweet(int tweetId);
}
