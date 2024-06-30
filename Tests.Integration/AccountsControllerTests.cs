using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using API;
using API.DTOs;
using API.Entities;
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
        var responseContent = JsonConvert.DeserializeObject<UserDto>(value: await response.Content.ReadAsStringAsync());
        Assert.Equal(responseContent.Mail, newUser.Mail);

        var userInDb = await userRepo.GetUsers(new UserFilters { Mail = newUser.Mail });
        Assert.Single(userInDb);

        var token = responseContent.Token;
        
        // send request with no token
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/users");
        request.Content = new StringContent(content: JsonConvert.SerializeObject(value: new UserFilters { Mail = newUser.Mail}),
            encoding: Encoding.UTF8,
            mediaType: "application/json");
        response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        
        // send request with a token
        request = new HttpRequestMessage(HttpMethod.Get, "/api/users");
        request.Headers.Add("Authorization", $"Bearer {token}");
        request.Content = new StringContent(content: JsonConvert.SerializeObject(value: new UserFilters { Mail = newUser.Mail}),
            encoding: Encoding.UTF8,
            mediaType: "application/json");
        response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var user = JsonConvert.DeserializeObject<List<AppUser>>(await response.Content?.ReadAsStringAsync());
        Assert.NotNull(user);
    }
}