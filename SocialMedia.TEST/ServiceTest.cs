using Microsoft.AspNetCore.SignalR;
using Moq;
using SocialMedia.API.DTO;
using SocialMedia.API.Hubs;
using SocialMedia.API.Model;
using SocialMedia.API.Repo;
using SocialMedia.API.Service;

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
        _tweetService = new TweetService(
            _mockTweetRepo.Object,
            _mockUserRepo.Object,
            new NoOpHubContext<SocialMediaHub>()
        );
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
        var result = await Assert.ThrowsAsync<ArgumentException>(async () => await _tweetService.CreateTweet(newTweetDto));

        // Assert
        
        Assert.Equal("Tweet body cannot be null or empty.", result.Message);
        
    }

    [Fact]
    public async Task CreateTweetNoParentIdTest()
    {
        
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
    public async Task GetUserById_NoUserFoundTest()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testUser",
            Password = "password123",
        };

        _mockUserRepo.Setup(repo => repo.GetUserById(1)).ReturnsAsync(user);

        var result = await Assert.ThrowsAsync<ArgumentException>(() => _userService.GetUserById(2));

        // Assert
        
        Assert.Equal("User with ID 2 not found.", result.Message);
        
    }

    [Fact]
    public async Task GetUserByUsername_ReturnUserTest()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testUser",
            Password = "password123",
        };

        _mockUserRepo.Setup(repo => repo.GetUserByUsername("testUser")).ReturnsAsync(user);

        // Act
        var result = await _userService.GetUserByUsername("testUser");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Username, result.Username);
        _mockUserRepo.Verify(repo => repo.GetUserByUsername("testUser"), Times.Once);
    }

    [Fact]
    public async Task GetUserByUsername_NoUserFoundTest()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testUser",
            Password = "password123",
        };

        _mockUserRepo.Setup(repo => repo.GetUserByUsername("testUser")).ReturnsAsync(user);

        var result = await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.GetUserByUsername("someGuy"));

        // Assert
        
        Assert.Equal("User with username 'someGuy' not found.", result.Message);        
    }

    [Fact]
    public async Task GetUserByUsername_NullUsernameTest()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testUser",
            Password = "password123",
        };

        _mockUserRepo.Setup(repo => repo.GetUserByUsername("testUser")).ReturnsAsync(user);

        var result = await Assert.ThrowsAsync<ArgumentException>(() => _userService.GetUserByUsername(null));

        // Assert
        
        Assert.Equal("Username cannot be null or empty. (Parameter 'username')", result.Message);        
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

    /*[Fact]
    public async Task DeleteTweetByIdTest()
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
        //_mockTweetRepo.Setup(repo => repo.DeleteTweet(1).ReturnsAsync(true));

        // Act
        var result = await _tweetService.DeleteTweet(1);

        // Assert
        Assert.True(result);
        //Assert.Equal(user.Id, result.Id);
        _mockTweetRepo.Verify(repo => repo.DeleteTweet(1), Times.Once);
    }*/



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
    public async Task GetTweetById_TweetDoesNotExistTest()
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
        var result = await Assert.ThrowsAsync<ArgumentException>(() => _tweetService.GetTweetById(2));
        

        // Assert
        
        Assert.Equal("Tweet with ID 2 does not exist.", result.Message);
        
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
    public async Task GetAllTweets_NoTweetsTest()
    {
        // Arrange
        var tweets = new List<Tweet>
        {};

        _mockTweetRepo.Setup(repo => repo.GetAllTweets()).ReturnsAsync(tweets);

        // Act
        var result = await Assert.ThrowsAsync<ArgumentException>(() =>_tweetService.GetAllTweets());

        // Assert
        
        Assert.Equal("No tweets found.", result.Message);
        
    }

    [Fact]
    public async Task GetAllTweetsByUserIdTest()
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
                UserId = 1,
            },
            new Tweet
            {
                Id = 3,
                Body = "Tweet 2",
                Likes = 0,
                UserId = 2,
            },
        };

        _mockTweetRepo.Setup(repo => repo.GetTweetsByUserId(1)).ReturnsAsync(tweets);

        // Act

        var result = await _tweetService.GetTweetsByUserId(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(tweets.Count, result.Count());
        _mockTweetRepo.Verify(repo => repo.GetTweetsByUserId(1), Times.Once);
    }

    [Fact]
    public async Task GetAllTweetsByUserId_IdGreaterThanOneTest()
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
                UserId = 1,
            },
            
        };

        _mockTweetRepo.Setup(repo => repo.GetTweetsByUserId(1)).ReturnsAsync(tweets);

        // Act
        //var result = await Assert.ThrowsAsync<ArgumentException>(() =>_tweetService.GetAllTweets());

        var result = await Assert.ThrowsAsync<ArgumentException>(() => _tweetService.GetTweetsByUserId(-1));

        // Assert
        
        Assert.Equal("User ID must be greater than 0.", result.Message);
        
    }

    [Fact]
    public async Task GetAllTweetsByUserId_UserIdNotFoundTest()
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
                UserId = 1,
            },
            
        };

        _mockTweetRepo.Setup(repo => repo.GetTweetsByUserId(1)).ReturnsAsync(tweets);

        // Act
        //var result = await Assert.ThrowsAsync<ArgumentException>(() =>_tweetService.GetAllTweets());

        var result = await Assert.ThrowsAsync<ArgumentException>(() => _tweetService.GetTweetsByUserId(2));

        // Assert
        
        Assert.Equal("No tweets found for user with ID 2.", result.Message);
        
    }
    [Fact]
    public async Task GetAllTweetRepliesTest()
    {
        // Arrange
        
        var parentTweet = new Tweet 
            {
                Id = 1,
                Body = "Tweet 1",
                Likes = 0,
                UserId = 1,
            };
        
        var tweets = new List<Tweet>
        {
            
            new Tweet
            {
                Id = 2,
                ParentId = 1,
                Body = "Reply 1",
                Likes = 0,
                UserId = 2,
            },
            new Tweet
            {
                Id = 3,
                ParentId = 1,
                Body = "Reply 2",
                Likes = 0,
                UserId = 3,
            },
        };

        _mockTweetRepo.Setup(repo => repo.GetTweetById(1)).ReturnsAsync(parentTweet);
        _mockTweetRepo.Setup(repo => repo.GetRepliesForTweet(1)).ReturnsAsync(tweets);

        // Act

        var result = await _tweetService.GetRepliesForTweet(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(tweets.Count, result.Count());
        _mockTweetRepo.Verify(repo => repo.GetRepliesForTweet(1), Times.Once);
    }

    [Fact]
    public async Task UpdateTweet_UpdatedTweetNullTest()
    {
        // Arrange
        var tweet = new Tweet
        {
            Id = 1,
            Body = "Test tweet",
            Likes = 5,
            UserId = 1,
        };

        var user = new User
        {
            Id = 1,
            Username = "testUser",
            Password = "password123",
        };

         _mockTweetRepo.Setup(repo => repo.GetTweetById(1)).ReturnsAsync(tweet);
         _mockUserRepo.Setup(repo => repo.GetUserById(1)).ReturnsAsync(user);

         // Act
        var result = await Assert.ThrowsAsync<ArgumentException>(() => _tweetService.UpdateTweet(1, "New Message"));

        // Assert
        Assert.Equal("There was a problem with updating the Tweet with ID 1.", result.Message);        

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
    public async Task LikeTweet_IdDoesNotExistTest()
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
        var result = await Assert.ThrowsAsync<ArgumentException>(() =>_tweetService.LikeTweet(2));

        // Assert
        //Assert.True(result);
        //_mockTweetRepo.Verify(repo => repo.GetTweetById(1), Times.Once);
        //_mockTweetRepo.Verify(repo => repo.LikeTweet(1), Times.Once);
        Assert.Equal("Tweet with ID 2 does not exist.", result.Message);
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

    [Fact]
    public async Task UnlikeTweet_IdDoesNotExistTest()
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
        var result = await Assert.ThrowsAsync<ArgumentException>(() =>_tweetService.UnlikeTweet(2));

        // Assert        

        Assert.Equal("Tweet with ID 2 does not exist.", result.Message);
    }

    [Fact]
    public async Task UnlikeTweet_LessThan0LikesTest()
    {
        // Arrange
        var tweet = new Tweet
        {
            Id = 1,
            Body = "Test tweet",
            Likes = 0,
            UserId = 1,
        };

        _mockTweetRepo.Setup(repo => repo.GetTweetById(1)).ReturnsAsync(tweet);
        _mockTweetRepo.Setup(repo => repo.UnlikeTweet(1)).ReturnsAsync(true);

        // Act
        var result = await Assert.ThrowsAsync<InvalidOperationException>(() =>_tweetService.UnlikeTweet(1));

        // Assert        

        Assert.Equal("Tweet with ID 1 cannot have less than 0 likes.", result.Message);
    }
}
