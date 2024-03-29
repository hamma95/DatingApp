using System.Net;
using System.Text;
using API;
using API.DTOs;
using API.Interfaces;
using API.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Tests.Integration;

public class AccountsControllerTests : BaseTests
{
    private readonly IServiceProvider _serviceProvider;

    public AccountsControllerTests(CustomWebApplicationFactory factory) : base(factory)
    {
        this._serviceProvider = factory.Services;
    }

    [Fact]
    public async void UserRegistrationTest()
    {
        // Arrange
        var newUser = new RegisterDto()
        {
            Mail = "houidi@test.com",
            Password = "SomeStrongPassword"
        };

        using var scope = _serviceProvider.CreateScope();
        var userRepo = scope.ServiceProvider.GetService<IUserRepository>();
        // Act
        
        var response = await _client.PostAsync(
            requestUri: "/api/account/Register",
            content: new StringContent(content: JsonConvert.SerializeObject(value: newUser),
                encoding: Encoding.UTF8,
                mediaType: "application/json"));

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal(expected: HttpStatusCode.OK, actual: response.StatusCode);
        var responseContent = JsonConvert.DeserializeObject<UserDto>(value: await response.Content.ReadAsStringAsync());
        Assert.Equal(responseContent.Mail, newUser.Mail);

        var userInDb = await userRepo.GetUsers(new UserFilters { Mail = newUser.Mail });
        Assert.Single(userInDb);
    }
}