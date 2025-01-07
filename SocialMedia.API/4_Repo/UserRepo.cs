#nullable disable
using SocialMedia.API.Data;
using SocialMedia.API.Model;

namespace SocialMedia.API.Repo;

public class UserRepo : IUserRepo
{
    private readonly SocialMediaContext _SocialMediaContext;

    public UserRepo(SocialMediaContext SocialMediaContext) =>
        _SocialMediaContext = SocialMediaContext;

    public User CreateUser(User newUser)
    {
        _SocialMediaContext.Users.Add(newUser);
        _SocialMediaContext.SaveChanges();
        return newUser;
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _SocialMediaContext.Users.ToList();
    }

    public User GetUserById(int id)
    {
        return _SocialMediaContext.Users.Find(id);
    }

    public User GetUserByUsername(string username)
    {
        return _SocialMediaContext.Users.FirstOrDefault(u => u.Username == username);
    }

    public User DeleteUserById(int id)
    {
        var user = GetUserById(id) ?? throw new ArgumentException($"User with ID {id} not found.");
        _SocialMediaContext.Users.Remove(user);
        _SocialMediaContext.SaveChanges();
        return user;
    }
}
