using API.DTOs;
using API.Entities;
using API.Repositories;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly UserService userService;

    public UsersController(IUserRepository userRepository, UserService userService)
    {
        this._userRepository = userRepository;
        this.userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers([FromQuery] UserFilters userFilters)
    {
        return Ok(await _userRepository.GetUsers(userFilters));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        return await _userRepository.GetUser(id);
    }
    
    [HttpPost("Create")]
    public async Task<ActionResult<AppUser>> CreateUser(CreateUserDto newUser)
    {
        if (newUser is null)
            return BadRequest("No data provided");
        var user = await userService.CreateUser(newUser: newUser);
        if (user == null)
        {
            return BadRequest("Invalid Data");
        }
        return CreatedAtAction(nameof(CreateUser), new { Id = user.Id }, user);
    }
}
