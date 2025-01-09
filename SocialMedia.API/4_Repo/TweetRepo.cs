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

    public async Task<Tweet> CreateTweet(Tweet newTweet)
    {
        await _SocialMediaContext.Tweets.AddAsync(newTweet);
        await _SocialMediaContext.SaveChangesAsync();
        return newTweet;
    }

    public async Task<Tweet> GetTweetById(int id)
    {
        return await _SocialMediaContext.Tweets.FindAsync(id);
    }

    public async Task<IEnumerable<Tweet>> GetAllTweets()
    {
        return await _SocialMediaContext
            .Tweets.Include(t => t.User)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tweet>> GetTweetsByUserId(int userId)
    {
        return await _SocialMediaContext.Tweets.Where(t => t.UserId == userId).ToListAsync();
    }

    public async Task<Tweet> UpdateTweet(int id, string newBody)
    {
        var tweet = await _SocialMediaContext
            .Tweets.Include(t => t.User) // including user for update as well
            .FirstOrDefaultAsync(t => t.Id == id);
        tweet.Body = newBody;
        await _SocialMediaContext.SaveChangesAsync();
        return tweet;
    }

    public async Task<bool> LikeTweet(int id)
    {
        var tweet = await _SocialMediaContext.Tweets.FindAsync(id);
        tweet.Likes++;
        await _SocialMediaContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UnlikeTweet(int id)
    {
        var tweet = await _SocialMediaContext.Tweets.FindAsync(id);
        tweet.Likes--;
        await _SocialMediaContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteTweet(int id)
    {
        var tweet = await _SocialMediaContext.Tweets.FindAsync(id);
        _SocialMediaContext.Tweets.Remove(tweet);
        await _SocialMediaContext.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Tweet>> GetRepliesForTweet(int tweetId)
    {
        return await _SocialMediaContext.Tweets.Where(t => t.ParentId == tweetId).ToListAsync();
    }
}
