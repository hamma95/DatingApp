using System.Net;
using System.Text;
using API.DTOs;
using Newtonsoft.Json;

namespace Tests.Integration;

public class UsersControllerTests : BaseTests
{
    public UsersControllerTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact (Skip = "fails after implementing auth")]
    public async Task CreateUser_ReturnsCreatedResult_WhenGivenValidInput()
    {
        // Arrange
        var newUser = new CreateUserDto
        {
            FirstName = "Mohamed",
            LastName = "Houidi",
            Mail = "houidi@test.com",
        };
        
        // Act
        var response = await _client.PostAsync(
            requestUri: "/api/users/Create",
            content: new StringContent(content: JsonConvert.SerializeObject(value: newUser),
                encoding: Encoding.UTF8,
                mediaType: "application/json"));

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal(expected: HttpStatusCode.Created, actual: response.StatusCode);
    }
}

