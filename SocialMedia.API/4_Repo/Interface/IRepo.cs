using SocialMedia.API.Model;

namespace SocialMedia.API.Repo;

public interface IUserRepo
{
    User CreateUser(User newUser);
    IEnumerable<User> GetAllUsers();
    User? GetUserById(int id);
    User? GetUserByUsername(string username);
    User? DeleteUserById(int id);
}

public interface ITweetRepo
{
    Tweet CreateTweet(Tweet newTweet);
    Tweet? GetTweetById(int id);
    IEnumerable<Tweet> GetAllTweets();
    IEnumerable<Tweet> GetTweetsByUserId(int userId);
    Tweet? UpdateTweet(int id, string newBody);
    bool LikeTweet(int id);
    bool UnlikeTweet(int id);
    bool DeleteTweet(int id);
    IEnumerable<Tweet> GetRepliesForTweet(int tweetId);
}
