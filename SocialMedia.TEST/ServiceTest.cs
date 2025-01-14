using Microsoft.AspNetCore.SignalR;
using Moq;
using SocialMedia.API.DTO;
using SocialMedia.API.Hubs;
using SocialMedia.API.Model;
using SocialMedia.API.Repo;
using SocialMedia.API.Service;
using SocialMedia.API.Hubs;

namespace SocialMedia.TEST;

public class ServiceTests
{
    private readonly Mock<IUserRepo> _mockUserRepo = new();
    private readonly Mock<ITweetRepo> _mockTweetRepo = new();
    
    private readonly UserService _userService;
    private readonly TweetService _tweetService;
    
    

    public ServiceTests()
    {
        _userService = new UserService(_mockUserRepo.Object);
        _tweetService = new TweetService(_mockTweetRepo.Object, _mockUserRepo.Object, new NoOpHubContext<SocialMediaHub>());

    }
    
    [Fact]
    public async Task CreateTweetSuccess()
    {
        // Arrange
        var newUser = new User
        {
            Id = 1,
            Username = "testUser",
            Password = "password123",
        };
        _mockUserRepo.Setup(repo => repo.CreateUser(It.IsAny<User>())).ReturnsAsync(newUser);


        var newTweetDto = new TweetInDTO { UserId = 1, ParentId = null, Body = "Hello World" };
        var newTweet = new Tweet
        {
            UserId = 1, 
            ParentId = null, 
            Body = "Hello World"
            
        };
        


        _mockTweetRepo.Setup(repo => repo.CreateTweet(It.IsAny<Tweet>())).ReturnsAsync(newTweet);
        

        // Act
        var result = await _tweetService.CreateTweet(newTweetDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newTweet.Body, result.Body);
        _mockTweetRepo.Verify(repo => repo.CreateTweet(It.IsAny<Tweet>()), Times.Once);
    }

    [Fact]
    public async Task CreateTweetNullBodyTest()
    {
        // Tests for null body
        // Arrange

        var newTweetDto = new TweetInDTO { UserId = 1, ParentId = null, Body = "" };
        var newTweet = new Tweet
        {
            UserId = 1, 
            ParentId = null, 
            Body = "Hello World"
            
        };

        _mockTweetRepo.Setup(repo => repo.CreateTweet(It.IsAny<Tweet>())).ReturnsAsync(newTweet);        

        // Act
        var result = await Assert.ThrowsAsync<ArgumentException>(() => _tweetService.CreateTweet(newTweetDto));

        // Assert
        
        Assert.Equal("Tweet body cannot be null or empty.", result.Message);
        
    }

    [Fact]
    public async Task CreateTweetNoParentIdTest()
    {
        // Tests for null body
        // Arrange

        var newTweetDto = new TweetInDTO { UserId = 1, ParentId = 1, Body = "Hello World" };
        var newTweet = new Tweet
        {
            UserId = 1, 
            ParentId = 1, 
            Body = "Hello World"
            
        };

        _mockTweetRepo.Setup(repo => repo.CreateTweet(It.IsAny<Tweet>())).ReturnsAsync(newTweet);        

        // Act
        var result = await Assert.ThrowsAsync<ArgumentException>(() => _tweetService.CreateTweet(newTweetDto));

        // Assert
        
        Assert.Equal("Parent tweet with ID 1 does not exist.", result.Message);
        
    }

    [Fact]
    public async Task CreateTweetNullDTOTest()
    {
        // Tests for null body
        // Arrange

        var newTweetDto = new TweetInDTO {Body = null}; 
        var newTweet = new Tweet
        {
            UserId = 1, 
            ParentId = 1, 
            Body = "Hello World"
            
        };

        _mockTweetRepo.Setup(repo => repo.CreateTweet(It.IsAny<Tweet>())).ReturnsAsync(newTweet);        

        // Act
        var result = await Assert.ThrowsAsync<ArgumentException>(() => _tweetService.CreateTweet(null));

        // Assert
        
        Assert.Equal("Parent tweet with ID 1 does not exist.", result.Message);
        
    }

    [Fact]
    public async Task CreateUser_ShouldReturnCreatedUser()
    {
        // Arrange
        var newUserDto = new UserInDTO { Username = "testUser", Password = "password123" };
        var newUser = new User
        {
            Id = 1,
            Username = "testUser",
            Password = "password123",
        }; 
        

        _mockUserRepo.Setup(repo => repo.CreateUser(It.IsAny<User>())).ReturnsAsync(newUser);

        // Act
        var result = await _userService.CreateUser(newUserDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newUser.Username, result.Username);
        _mockUserRepo.Verify(repo => repo.CreateUser(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task GetAllUsers_ReturnAllUsersTest()
    {
        // Arrange
        var users = new List<User>
        {
            new User
            {
                Id = 1,
                Username = "user1",
                Password = "pass1",
            },
            new User
            {
                Id = 2,
                Username = "user2",
                Password = "pass2",
            },
        };

        _mockUserRepo.Setup(repo => repo.GetAllUsers()).ReturnsAsync(users);

        // Act
        var result = await _userService.GetAllUsers();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(users.Count, result.Count());
        _mockUserRepo.Verify(repo => repo.GetAllUsers(), Times.Once);
    }

    [Fact]
    public async Task GetUserById_ReturnUserTest()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testUser",
            Password = "password123",
        };

        _mockUserRepo.Setup(repo => repo.GetUserById(1)).ReturnsAsync(user);

        // Act
        var result = await _userService.GetUserById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        _mockUserRepo.Verify(repo => repo.GetUserById(1), Times.Once);
    }

    [Fact]
    public async Task DeleteUserById_ReturnDeletedUserTest()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testUser",
            Password = "password123",
        };

        _mockUserRepo.Setup(repo => repo.DeleteUserById(1)).ReturnsAsync(user);

        // Act
        var result = await _userService.DeleteUserById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        _mockUserRepo.Verify(repo => repo.DeleteUserById(1), Times.Once);
    }

    [Fact]
    public async Task GetTweetById_ReturnTweetTest()
    {
        // Arrange
        var tweet = new Tweet
        {
            Id = 1,
            Body = "Test tweet",
            Likes = 5,
            UserId = 1,
        };

        _mockTweetRepo.Setup(repo => repo.GetTweetById(1)).ReturnsAsync(tweet);

        // Act
        var result = await _tweetService.GetTweetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(tweet.Id, result.Id);
        _mockTweetRepo.Verify(repo => repo.GetTweetById(1), Times.Once);
    }

    [Fact]
    public async Task GetAllTweets_ReturnAllTweetsTest()
    {
        // Arrange
        var tweets = new List<Tweet>
        {
            new Tweet
            {
                Id = 1,
                Body = "Tweet 1",
                Likes = 0,
                UserId = 1,
            },
            new Tweet
            {
                Id = 2,
                Body = "Tweet 2",
                Likes = 0,
                UserId = 2,
            },
        };

        _mockTweetRepo.Setup(repo => repo.GetAllTweets()).ReturnsAsync(tweets);

        // Act
        var result = await _tweetService.GetAllTweets();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(tweets.Count, result.Count());
        _mockTweetRepo.Verify(repo => repo.GetAllTweets(), Times.Once);
    }

    [Fact]
    public async Task LikeTweet_IncrementLikesTest()
    {
        // Arrange
        var tweet = new Tweet
        {
            Id = 1,
            Body = "Test tweet",
            Likes = 5,
            UserId = 1,
        };

        _mockTweetRepo.Setup(repo => repo.GetTweetById(1)).ReturnsAsync(tweet);
        _mockTweetRepo.Setup(repo => repo.LikeTweet(1)).ReturnsAsync(true);

        // Act
        var result = await _tweetService.LikeTweet(1);

        // Assert
        Assert.True(result);
        _mockTweetRepo.Verify(repo => repo.GetTweetById(1), Times.Once);
        _mockTweetRepo.Verify(repo => repo.LikeTweet(1), Times.Once);
    }

    [Fact]
    public async Task UnlikeTweet_DecrementLikesTest()
    {
        // Arrange
        var tweet = new Tweet
        {
            Id = 1,
            Body = "Test tweet",
            Likes = 5,
            UserId = 1,
        };

        _mockTweetRepo.Setup(repo => repo.GetTweetById(1)).ReturnsAsync(tweet);
        _mockTweetRepo.Setup(repo => repo.UnlikeTweet(1)).ReturnsAsync(true);

        // Act
        var result = await _tweetService.UnlikeTweet(1);

        // Assert
        Assert.True(result);
        _mockTweetRepo.Verify(repo => repo.GetTweetById(1), Times.Once);
        _mockTweetRepo.Verify(repo => repo.UnlikeTweet(1), Times.Once);
    }
}
