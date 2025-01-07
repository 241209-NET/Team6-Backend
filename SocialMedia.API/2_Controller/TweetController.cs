using Microsoft.AspNetCore.Mvc;
using SocialMedia.API.DTO;
using SocialMedia.API.Model;
using SocialMedia.API.Service;

namespace SocialMedia.API.Controller;

[Route("api/[controller]")]
[ApiController]
public class TweetController : ControllerBase
{
    private readonly ITweetService _tweetService;

    public TweetController(ITweetService tweetService)
    {
        _tweetService = tweetService;
    }

    // GET: api/Tweet
    [HttpGet]
    public IActionResult GetAllTweets()
    {
        try
        {
            var tweets = _tweetService.GetAllTweets();
            return Ok(tweets);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // POST: api/Tweet
    [HttpPost]
    public IActionResult CreateTweet(TweetInDTO newTweet)
    {
        try
        {
            var createdTweet = _tweetService.CreateTweet(newTweet);
            return CreatedAtAction(
                nameof(GetTweetById),
                new { id = createdTweet.Id },
                createdTweet
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET: api/Tweet/{id}
    [HttpGet("{id}")]
    public IActionResult GetTweetById(int id)
    {
        try
        {
            var tweet = _tweetService.GetTweetById(id);
            return Ok(tweet);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // GET: api/Tweet/User/{userId}
    [HttpGet("User/{userId}")]
    public IActionResult GetTweetsByUserId(int userId)
    {
        try
        {
            var tweets = _tweetService.GetTweetsByUserId(userId);
            return Ok(tweets);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT: api/Tweet/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateTweet(int id, string newBody)
    {
        try
        {
            var updatedTweet = _tweetService.UpdateTweet(id, newBody);
            return Ok(updatedTweet);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // POST: api/Tweet/{id}/Like
    [HttpPost("{id}/Like")]
    public IActionResult LikeTweet(int id)
    {
        try
        {
            var result = _tweetService.LikeTweet(id);
            return Ok(new { success = result });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // POST: api/Tweet/{id}/Unlike
    [HttpPost("{id}/Unlike")]
    public IActionResult UnlikeTweet(int id)
    {
        try
        {
            var result = _tweetService.UnlikeTweet(id);
            return Ok(new { success = result });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/Tweet/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteTweet(int id)
    {
        try
        {
            var result = _tweetService.DeleteTweet(id);
            return Ok(new { success = result });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // GET: api/Tweet/{id}/Replies
    [HttpGet("{id}/Replies")]
    public IActionResult GetRepliesForTweet(int id)
    {
        try
        {
            var replies = _tweetService.GetRepliesForTweet(id);
            return Ok(replies);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
