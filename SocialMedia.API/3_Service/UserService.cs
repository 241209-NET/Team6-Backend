using SocialMedia.API.DTO;
using SocialMedia.API.Model;
using SocialMedia.API.Repo;
using SocialMedia.API.Util;

namespace SocialMedia.API.Service;

public class UserService : IUserService
{
    private readonly IUserRepo _userRepo;

    public UserService(IUserRepo userRepo) => _userRepo = userRepo;

    public async Task<User> CreateUser(UserInDTO newUser)
    {
        User fromDTO = Utilities.UserDTOToObject(newUser);

        return await _userRepo.CreateUser(fromDTO);
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _userRepo.GetAllUsers();
    }

    public async Task<User>? GetUserById(int id)
    {
        var foundUser =
            await _userRepo.GetUserById(id)!
            ?? throw new ArgumentException($"User with ID {id} not found.");
        return foundUser;
    }

    public async Task<User>? GetUserByUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be null or empty.", nameof(username));
        }
        var foundUser =
            await _userRepo.GetUserByUsername(username)!
            ?? throw new InvalidOperationException($"User with username '{username}' not found.");
        return foundUser;
    }

    public async Task<User>? DeleteUserById(int id)
    {
        var deletedUser = await _userRepo.DeleteUserById(id)!;
        return deletedUser;
    }
}
