using API.DTOs;
using API.Entities;
using API.Repositories;
using API.Services;
using Moq;

namespace Tests.Unit;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepository;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepository = new Mock<IUserRepository>();
        _userService = new UserService(_userRepository.Object);
    }

    [Fact]
    public async Task CreateUser_ReturnsUser_WhenGivenValidInput()
    {
        var newUser = new CreateUserDto
        {
            FirstName = "Mohamed",
            LastName = "Houidi",
            Mail = "houidi@test.com",
        };
        // Set up the repository to return a user when CreateUser is called
        _userRepository.Setup(repo => repo.CreateUser(It.IsAny<AppUser>()))
            .ReturnsAsync(new AppUser { FirstName = newUser.FirstName, LastName = newUser.LastName, Mail = newUser.Mail }); // fill in data
        
        var result = await _userService.CreateUser(newUser);
        Assert.NotNull(result);
    }
}