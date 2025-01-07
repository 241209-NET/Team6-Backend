using Microsoft.AspNetCore.SignalR;
using SocialMedia.API.DTO;
using SocialMedia.API.Hubs;
using SocialMedia.API.Model;
using SocialMedia.API.Repo;
using SocialMedia.API.Util;

namespace SocialMedia.API.Service;

public class TweetService : ITweetService
{
    private readonly ITweetRepo _tweetRepo;
    private readonly IUserRepo _userRepo;
    private readonly IHubContext<SocialMediaHub> _hubContext;

    public TweetService(
        ITweetRepo tweetRepo,
        IUserRepo userRepo,
        IHubContext<SocialMediaHub> hubContext
    )
    {
        _tweetRepo = tweetRepo;
        _userRepo = userRepo;
        _hubContext = hubContext;
    }

    public Tweet CreateTweet(TweetInDTO tweetDTO)
    {
        if (tweetDTO == null)
        {
            throw new ArgumentNullException(nameof(tweetDTO), "Tweet DTO cannot be null.");
        }

        var newTweet = Utilities.TweetDTOToObject(tweetDTO);

        if (string.IsNullOrWhiteSpace(newTweet.Body))
        {
            throw new ArgumentException("Tweet body cannot be null or empty.");
        }

        if (newTweet.ParentId.HasValue)
        {
            var parentTweet = _tweetRepo.GetTweetById(newTweet.ParentId.Value);
            if (parentTweet == null)
            {
                throw new ArgumentException(
                    $"Parent tweet with ID {newTweet.ParentId.Value} does not exist."
                );
            }
        }

        var user = _userRepo.GetUserById(newTweet.UserId);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {newTweet.UserId} does not exist.");
        }

        newTweet.User = user;
        newTweet.CreatedAt = DateTime.UtcNow;
        newTweet.Likes = 0;

        var createdTweet = _tweetRepo.CreateTweet(newTweet);

        // Notify clients in real time
        _hubContext.Clients.All.SendAsync("ReceiveTweet", createdTweet);

        return createdTweet;
    }

    public Tweet GetTweetById(int id)
    {
        var tweet = _tweetRepo.GetTweetById(id);
        if (tweet == null)
        {
            throw new ArgumentException($"Tweet with ID {id} does not exist.");
        }

        return tweet;
    }

    public IEnumerable<Tweet> GetAllTweets()
    {
        var tweets = _tweetRepo.GetAllTweets();
        if (!tweets.Any())
        {
            throw new ArgumentException("No tweets found.");
        }

        return tweets;
    }

    public IEnumerable<Tweet> GetTweetsByUserId(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("User ID must be greater than 0.");
        }

        var tweets = _tweetRepo.GetTweetsByUserId(userId);
        if (!tweets.Any())
        {
            throw new ArgumentException($"No tweets found for user with ID {userId}.");
        }

        return tweets;
    }

    public Tweet UpdateTweet(int tweetId, string newBody)
    {
        if (string.IsNullOrWhiteSpace(newBody))
        {
            throw new ArgumentException("Tweet body cannot be null or empty.");
        }

        var tweet = _tweetRepo.GetTweetById(tweetId);
        if (tweet == null)
        {
            throw new ArgumentException($"Tweet with ID {tweetId} does not exist.");
        }

        var updatedTweet = _tweetRepo.UpdateTweet(tweetId, newBody);
        if (updatedTweet == null)
        {
            throw new ArgumentException(
                $"There was a problem with updating the Tweet with ID {tweetId}."
            );
        }

        // Notify clients in real time
        _hubContext.Clients.All.SendAsync("UpdateTweet", updatedTweet);

        return updatedTweet;
    }

    public bool LikeTweet(int tweetId)
    {
        var tweet = _tweetRepo.GetTweetById(tweetId);
        if (tweet == null)
        {
            throw new ArgumentException($"Tweet with ID {tweetId} does not exist.");
        }

        var result = _tweetRepo.LikeTweet(tweetId);

        // Notify clients in real time
        _hubContext.Clients.All.SendAsync("LikeTweet", tweetId);

        return result;
    }

    public bool UnlikeTweet(int tweetId)
    {
        var tweet = _tweetRepo.GetTweetById(tweetId);
        if (tweet == null)
        {
            throw new ArgumentException($"Tweet with ID {tweetId} does not exist.");
        }

        if (tweet.Likes <= 0)
        {
            throw new InvalidOperationException(
                $"Tweet with ID {tweetId} cannot have less than 0 likes."
            );
        }

        var result = _tweetRepo.UnlikeTweet(tweetId);

        // Notify clients in real time
        _hubContext.Clients.All.SendAsync("UnlikeTweet", tweetId);

        return result;
    }

    public bool DeleteTweet(int tweetId)
    {
        var tweet = _tweetRepo.GetTweetById(tweetId);
        if (tweet == null)
        {
            throw new ArgumentException($"Tweet with ID {tweetId} does not exist.");
        }

        var result = _tweetRepo.DeleteTweet(tweetId);

        // Notify clients in real time
        _hubContext.Clients.All.SendAsync("DeleteTweet", tweetId);

        return result;
    }

    public IEnumerable<Tweet> GetRepliesForTweet(int tweetId)
    {
        var tweet = _tweetRepo.GetTweetById(tweetId);
        if (tweet == null)
        {
            throw new ArgumentException($"Tweet with ID {tweetId} does not exist.");
        }

        var replies = _tweetRepo.GetRepliesForTweet(tweetId);
        if (!replies.Any())
        {
            throw new ArgumentException($"No replies found for tweet with ID {tweetId}.");
        }

        return replies;
    }
}
