#nullable disable
using Microsoft.EntityFrameworkCore;
using SocialMedia.API.Data;
using SocialMedia.API.Model;

namespace SocialMedia.API.Repo;

public class UserRepo : IUserRepo
{
    private readonly SocialMediaContext _SocialMediaContext;

    public UserRepo(SocialMediaContext SocialMediaContext) =>
        _SocialMediaContext = SocialMediaContext;

    public async Task<User> CreateUser(User newUser)
    {
        await _SocialMediaContext.Users.AddAsync(newUser);
        await _SocialMediaContext.SaveChangesAsync();
        return newUser;
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _SocialMediaContext.Users.ToListAsync();
    }

    public async Task<User> GetUserById(int id)
    {
        return await _SocialMediaContext.Users.FindAsync(id);
    }

    public async Task<User> GetUserByUsername(string username)
    {
        return await _SocialMediaContext.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User> DeleteUserById(int id)
    {
        var user =
            await GetUserById(id) ?? throw new ArgumentException($"User with ID {id} not found.");
        _SocialMediaContext.Users.Remove(user);
        await _SocialMediaContext.SaveChangesAsync();
        return user;
    }
}
