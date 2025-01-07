using SocialMedia.API.DTO;
using SocialMedia.API.Model;
using SocialMedia.API.Repo;
using SocialMedia.API.Util;

namespace SocialMedia.API.Service;

public class UserService : IUserService
{
    private readonly IUserRepo _userRepo;

    public UserService(IUserRepo userRepo) => _userRepo = userRepo;

    public User CreateUser(UserInDTO newUser)
    {
        User fromDTO = Utilities.UserDTOToObject(newUser);

        return _userRepo.CreateUser(fromDTO);
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _userRepo.GetAllUsers();
    }

    public User? GetUserById(int id)
    {
        var foundUser =
            _userRepo.GetUserById(id)
            ?? throw new ArgumentException($"User with ID {id} not found.");
        return foundUser;
    }

    public User? GetUserByUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be null or empty.", nameof(username));
        }
        var foundUser =
            _userRepo.GetUserByUsername(username)
            ?? throw new InvalidOperationException($"User with username '{username}' not found.");
        return foundUser;
    }

    public User? DeleteUserById(int id)
    {
        var deletedUser = _userRepo.DeleteUserById(id);
        return deletedUser;
    }
}
