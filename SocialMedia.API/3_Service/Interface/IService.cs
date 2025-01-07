namespace SocialMedia.API.Service;

using System.Collections.Generic;
using SocialMedia.API.DTO;
using SocialMedia.API.Model;

public interface IUserService
{
    User CreateUser(UserInDTO newUser);
    IEnumerable<User> GetAllUsers();
    User? GetUserById(int id);
    User? GetUserByUsername(string username);
    User? DeleteUserById(int id);
}

public interface ITweetService
{
    Tweet CreateTweet(TweetInDTO newTweet);
    Tweet? GetTweetById(int id);
    IEnumerable<Tweet> GetAllTweets();
    IEnumerable<Tweet> GetTweetsByUserId(int userId);
    Tweet? UpdateTweet(int id, string newBody);
    bool LikeTweet(int id);
    bool UnlikeTweet(int id);
    bool DeleteTweet(int id);
    IEnumerable<Tweet> GetRepliesForTweet(int tweetId);
}
