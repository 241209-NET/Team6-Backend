#nullable disable
using Microsoft.EntityFrameworkCore;
using SocialMedia.API.Data;
using SocialMedia.API.Model;

namespace SocialMedia.API.Repo;

public class TweetRepo : ITweetRepo
{
    private readonly SocialMediaContext _SocialMediaContext;

    public TweetRepo(SocialMediaContext SocialMediaContext) =>
        _SocialMediaContext = SocialMediaContext;

    public Tweet CreateTweet(Tweet newTweet)
    {
        _SocialMediaContext.Tweets.Add(newTweet);
        _SocialMediaContext.SaveChanges();
        return newTweet;
    }

    public Tweet GetTweetById(int id)
    {
        return _SocialMediaContext.Tweets.Find(id);
    }

    public IEnumerable<Tweet> GetAllTweets()
    {
        return _SocialMediaContext
            .Tweets.Include(t => t.User)
            .OrderByDescending(t => t.CreatedAt)
            .ToList();
    }

    public IEnumerable<Tweet> GetTweetsByUserId(int userId)
    {
        return _SocialMediaContext.Tweets.Where(t => t.UserId == userId).ToList();
    }

    public Tweet UpdateTweet(int id, string newBody)
    {
        var tweet = _SocialMediaContext
            .Tweets.Include(t => t.User) // including user for update as well
            .FirstOrDefault(t => t.Id == id);
        tweet.Body = newBody;
        _SocialMediaContext.SaveChanges();
        return tweet;
    }

    public bool LikeTweet(int id)
    {
        var tweet = _SocialMediaContext.Tweets.Find(id);
        tweet.Likes++;
        _SocialMediaContext.SaveChanges();
        return true;
    }

    public bool UnlikeTweet(int id)
    {
        var tweet = _SocialMediaContext.Tweets.Find(id);
        tweet.Likes--;
        _SocialMediaContext.SaveChanges();
        return true;
    }

    public bool DeleteTweet(int id)
    {
        var tweet = _SocialMediaContext.Tweets.Find(id);
        _SocialMediaContext.Tweets.Remove(tweet);
        _SocialMediaContext.SaveChanges();
        return true;
    }

    public IEnumerable<Tweet> GetRepliesForTweet(int tweetId)
    {
        return _SocialMediaContext.Tweets.Where(t => t.ParentId == tweetId).ToList();
    }
}
