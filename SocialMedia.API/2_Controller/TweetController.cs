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
    public async Task<IActionResult> GetAllTweets()
    {
        try
        {
            var tweets = await _tweetService.GetAllTweets();
            return Ok(tweets);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // POST: api/Tweet
    [HttpPost]
    public async Task<IActionResult> CreateTweet(TweetInDTO newTweet)
    {
        try
        {
            var createdTweet = await _tweetService.CreateTweet(newTweet);
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
    public async Task<IActionResult> GetTweetById(int id)
    {
        try
        {
            var tweet = await _tweetService.GetTweetById(id)!;
            return Ok(tweet);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // GET: api/Tweet/User/{userId}
    [HttpGet("User/{userId}")]
    public async Task<IActionResult> GetTweetsByUserId(int userId)
    {
        try
        {
            var tweets = await _tweetService.GetTweetsByUserId(userId);
            return Ok(tweets);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT: api/Tweet/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTweet(int id, string newBody)
    {
        try
        {
            var updatedTweet = await _tweetService.UpdateTweet(id, newBody)!;
            return Ok(updatedTweet);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // POST: api/Tweet/{id}/Like
    [HttpPost("{id}/Like")]
    public async Task<IActionResult> LikeTweet(int id)
    {
        try
        {
            var result = await _tweetService.LikeTweet(id);
            return Ok(new { success = result });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // POST: api/Tweet/{id}/Unlike
    [HttpPost("{id}/Unlike")]
    public async Task<IActionResult> UnlikeTweet(int id)
    {
        try
        {
            var result = await _tweetService.UnlikeTweet(id);
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
    public async Task<IActionResult> DeleteTweet(int id)
    {
        try
        {
            var result = await _tweetService.DeleteTweet(id);
            return Ok(new { success = result });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // GET: api/Tweet/{id}/Replies
    [HttpGet("{id}/Replies")]
    public async Task<IActionResult> GetRepliesForTweet(int id)
    {
        try
        {
            var replies = await _tweetService.GetRepliesForTweet(id);
            return Ok(replies);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
