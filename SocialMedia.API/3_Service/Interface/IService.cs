namespace SocialMedia.API.Service;

using System.Collections.Generic;
using SocialMedia.API.DTO;
using SocialMedia.API.Model;

public interface IUserService
{
    Task<User> CreateUser(UserInDTO newUser);
    Task<IEnumerable<User>> GetAllUsers();
    Task<User>? GetUserById(int id);
    Task<User>? GetUserByUsername(string username);
    Task<User>? DeleteUserById(int id);
}

public interface ITweetService
{
    Task<Tweet> CreateTweet(TweetInDTO newTweet);
    Task<Tweet>? GetTweetById(int id);
    Task<IEnumerable<Tweet>> GetAllTweets();
    Task<IEnumerable<Tweet>> GetTweetsByUserId(int userId);
    Task<Tweet>? UpdateTweet(int id, string newBody);
    Task<bool> LikeTweet(int id);
    Task<bool> UnlikeTweet(int id);
    Task<bool> DeleteTweet(int id);
    Task<IEnumerable<Tweet>> GetRepliesForTweet(int tweetId);
}
