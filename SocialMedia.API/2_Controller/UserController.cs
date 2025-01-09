using Microsoft.AspNetCore.Mvc;

namespace SocialMedia.API.Controller;

using SocialMedia.API.DTO;
using SocialMedia.API.Service;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // POST: api/User
    [HttpPost]
    public async Task<IActionResult> CreateUser(UserInDTO newUser)
    {
        if (newUser == null)
        {
            return BadRequest("User data cannot be null.");
        }

        try
        {
            var createdUser = await _userService.CreateUser(newUser);
            return Ok(createdUser);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // GET: api/User
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // GET: api/User/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var user = await _userService.GetUserById(id)!;
            return Ok(user);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // GET: api/User/username/{username}
    [HttpGet("username/{username}")]
    public async Task<IActionResult> GetUserByUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return BadRequest("Username cannot be null or empty.");
        }

        try
        {
            var user = await _userService.GetUserByUsername(username)!;
            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/User/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserById(int id)
    {
        try
        {
            var deletedUser = await _userService.DeleteUserById(id)!;
            return Ok(deletedUser);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // POST: api/User/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserInDTO userDto)
    {
        if (string.IsNullOrWhiteSpace(userDto.Username))
            return BadRequest("Username cannot be empty.");

        // 1) Look up user by username
        var user = await _userService.GetUserByUsername(userDto.Username)!;
        if (user == null)
            return NotFound("User not found.");

        // 2) verify password
        if (user.Password != userDto.Password)
        {
            return Unauthorized("Invalid credentials.");
        }

        return Ok(user);
    }
}
