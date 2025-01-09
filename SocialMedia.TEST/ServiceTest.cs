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
    private readonly Mock<IHubContext<SocialMediaHub>> _mockHubContext = new();
    private readonly UserService _userService;
    private readonly TweetService _tweetService;
<<<<<<< HEAD
    private readonly Mock<IHubContext<SocialMediaHub>> _mockHubContext = new();
    private readonly IHubContext<SocialMediaHub> _HubContext;
=======
    private readonly IHubContext<SocialMediaHub> _hubContext;
>>>>>>> 89ca67c0edcce20c79d7354bc4c8f3f84c516e86

    public ServiceTests()
    {
        _userService = new UserService(_mockUserRepo.Object);
<<<<<<< HEAD
        _tweetService = new TweetService(_mockTweetRepo.Object, _mockUserRepo.Object, _mockHubContext.Object);

    }
    
    [Fact]
    public void CreateTweetSuccess()
    {
        // Arrange
        var newUser = new User
        {
            Id = 1,
            Username = "testUser",
            Password = "password123",
        };
        _mockUserRepo.Setup(repo => repo.CreateUser(It.IsAny<User>())).Returns(newUser);


        var newTweetDto = new TweetInDTO { UserId = 1, ParentId = null, Body = "Hello World" };
        var newTweet = new Tweet
        {
            UserId = 1, 
            ParentId = null, 
            Body = "Hello World"
            
        };
        


        _mockTweetRepo.Setup(repo => repo.CreateTweet(It.IsAny<Tweet>())).Returns(newTweet);
        

        // Act
        var result = _tweetService.CreateTweet(newTweetDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newTweet.Body, result.Body);
        _mockTweetRepo.Verify(repo => repo.CreateTweet(It.IsAny<Tweet>()), Times.Once);
    }

    [Fact]
    public void CreateTweetNullBodyTest()
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

        _mockTweetRepo.Setup(repo => repo.CreateTweet(It.IsAny<Tweet>())).Returns(newTweet);        

        // Act
        var result = Assert.Throws<ArgumentException>(() => _tweetService.CreateTweet(newTweetDto));

        // Assert
        
        Assert.Equal("Tweet body cannot be null or empty.", result.Message);
        
    }

    [Fact]
    public void CreateTweetNoParentIdTest()
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

        _mockTweetRepo.Setup(repo => repo.CreateTweet(It.IsAny<Tweet>())).Returns(newTweet);        

        // Act
        var result = Assert.Throws<ArgumentException>(() => _tweetService.CreateTweet(newTweetDto));

        // Assert
        
        Assert.Equal("Parent tweet with ID 1 does not exist.", result.Message);
        
    }

    [Fact]
    public void CreateTweetNullDTOTest()
    {
        // Tests for null body
        // Arrange

        var newTweetDto = new TweetInDTO {}; 
        var newTweet = new Tweet
        {
            UserId = 1, 
            ParentId = 1, 
            Body = "Hello World"
            
        };

        _mockTweetRepo.Setup(repo => repo.CreateTweet(It.IsAny<Tweet>())).Returns(newTweet);        

        // Act
        var result = Assert.Throws<ArgumentException>(() => _tweetService.CreateTweet(null));

        // Assert
        
        Assert.Equal("Parent tweet with ID 1 does not exist.", result.Message);
        
=======
        _tweetService = new TweetService(
            _mockTweetRepo.Object,
            _mockUserRepo.Object,
            _mockHubContext.Object
        );
>>>>>>> 89ca67c0edcce20c79d7354bc4c8f3f84c516e86
    }

    [Fact]
    public void CreateUser_ShouldReturnCreatedUser()
    {
        // Arrange
        var newUserDto = new UserInDTO { Username = "testUser", Password = "password123" };
        var newUser = new User
        {
            Id = 1,
            Username = "testUser",
            Password = "password123",
        }; 
        

        _mockUserRepo.Setup(repo => repo.CreateUser(It.IsAny<User>())).Returns(newUser);

        // Act
        var result = _userService.CreateUser(newUserDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newUser.Username, result.Username);
        _mockUserRepo.Verify(repo => repo.CreateUser(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public void GetAllUsers_ReturnAllUsersTest()
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

        _mockUserRepo.Setup(repo => repo.GetAllUsers()).Returns(users);

        // Act
        var result = _userService.GetAllUsers();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(users.Count, result.Count());
        _mockUserRepo.Verify(repo => repo.GetAllUsers(), Times.Once);
    }

    [Fact]
    public void GetUserById_ReturnUserTest()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testUser",
            Password = "password123",
        };

        _mockUserRepo.Setup(repo => repo.GetUserById(1)).Returns(user);

        // Act
        var result = _userService.GetUserById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        _mockUserRepo.Verify(repo => repo.GetUserById(1), Times.Once);
    }

    [Fact]
    public void DeleteUserById_ReturnDeletedUserTest()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testUser",
            Password = "password123",
        };

        _mockUserRepo.Setup(repo => repo.DeleteUserById(1)).Returns(user);

        // Act
        var result = _userService.DeleteUserById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        _mockUserRepo.Verify(repo => repo.DeleteUserById(1), Times.Once);
    }

    [Fact]
    public void GetTweetById_ReturnTweetTest()
    {
        // Arrange
        var tweet = new Tweet
        {
            Id = 1,
            Body = "Test tweet",
            Likes = 5,
            UserId = 1,
        };

        _mockTweetRepo.Setup(repo => repo.GetTweetById(1)).Returns(tweet);

        // Act
        var result = _tweetService.GetTweetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(tweet.Id, result.Id);
        _mockTweetRepo.Verify(repo => repo.GetTweetById(1), Times.Once);
    }

    [Fact]
    public void GetAllTweets_ReturnAllTweetsTest()
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

        _mockTweetRepo.Setup(repo => repo.GetAllTweets()).Returns(tweets);

        // Act
        var result = _tweetService.GetAllTweets();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(tweets.Count, result.Count());
        _mockTweetRepo.Verify(repo => repo.GetAllTweets(), Times.Once);
    }

    [Fact]
    public void LikeTweet_IncrementLikesTest()
    {
        // Arrange
        var tweet = new Tweet
        {
            Id = 1,
            Body = "Test tweet",
            Likes = 5,
            UserId = 1,
        };

        _mockTweetRepo.Setup(repo => repo.GetTweetById(1)).Returns(tweet);
        _mockTweetRepo.Setup(repo => repo.LikeTweet(1)).Returns(true);

        // Act
        var result = _tweetService.LikeTweet(1);

        // Assert
        Assert.True(result);
        _mockTweetRepo.Verify(repo => repo.GetTweetById(1), Times.Once);
        _mockTweetRepo.Verify(repo => repo.LikeTweet(1), Times.Once);
    }

    [Fact]
    public void UnlikeTweet_DecrementLikesTest()
    {
        // Arrange
        var tweet = new Tweet
        {
            Id = 1,
            Body = "Test tweet",
            Likes = 5,
            UserId = 1,
        };

        _mockTweetRepo.Setup(repo => repo.GetTweetById(1)).Returns(tweet);
        _mockTweetRepo.Setup(repo => repo.UnlikeTweet(1)).Returns(true);

        // Act
        var result = _tweetService.UnlikeTweet(1);

        // Assert
        Assert.True(result);
        _mockTweetRepo.Verify(repo => repo.GetTweetById(1), Times.Once);
        _mockTweetRepo.Verify(repo => repo.UnlikeTweet(1), Times.Once);
    }
}
