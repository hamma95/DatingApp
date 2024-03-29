using API.DTOs;
using API.Entities;
using API.Repositories;

namespace API.Services;

public class UserService
{
    private readonly IUserRepository userRepository;

    public UserService(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }
    
    public async Task<AppUser> CreateUser(CreateUserDto newUser)
    {
        var user = new AppUser
        {
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            Mail = newUser.Mail
        };

        if (!ValidateUser(user))
        {
            return null;
        }
        
        user = userRepository.CreateUser(user).Result;
        await userRepository.SaveChanges();
        return user;
    }

    private bool ValidateUser(AppUser user)
    {
        bool isValidUser = !string.IsNullOrWhiteSpace(user.FirstName);
        isValidUser &= !string.IsNullOrWhiteSpace(user.LastName);
        isValidUser &= !string.IsNullOrWhiteSpace(user.Mail);
        return isValidUser;
    }
}