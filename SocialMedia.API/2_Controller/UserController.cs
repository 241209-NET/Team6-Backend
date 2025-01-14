using Microsoft.AspNetCore.Mvc;

namespace SocialMedia.API.Controller;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.API.DTO;
using SocialMedia.API.Service;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public UserController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
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
    [Authorize]
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

    // GET: api/User/current - adding for easy token use, decodes here
    [Authorize]
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentUser()
    {
        try
        {
            // Extract the user ID from the JWT claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "Invalid or missing token." });
            }

            // Parse the user ID from the claim
            var userId = int.Parse(userIdClaim.Value);

            // Retrieve the user details using the user ID
            var user = await _userService.GetUserById(userId)!;
            return Ok(user);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new { message = "An unexpected error occurred.", details = ex.Message }
            );
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

        // 1) Look up user by username //Update the GetUserByUsername in Controller with GetUserWithToken
        var user = await _userService.GetUserByUsername(userDto.Username)!;
        if (user == null)
            return NotFound("User not found.");

        // 2) verify password
        if (user.Password != userDto.Password)
        {
            return Unauthorized("Invalid credentials.");
        }

        if (user != null)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", user.Id.ToString()),
                new Claim("Username", user.Username!.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signIn
            );
            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { Token = tokenValue, User = user });
        }
        return NoContent();
    }
}
