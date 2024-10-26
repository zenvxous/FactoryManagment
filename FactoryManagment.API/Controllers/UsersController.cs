using FactoryManagment.API.Contracts.Users;
using FactoryManagment.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FactoryManagment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUsersService _usersService;

    public UsersController(IHttpContextAccessor httpContextAccessor, IUsersService usersService)
    {
        _httpContextAccessor = httpContextAccessor;
        _usersService = usersService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var error = await _usersService.RegisterUser(request.Username, request.Password, request.Email);

        if(error != string.Empty)
            return BadRequest(error);
        
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        string token;
        
        if(request.Login.Contains('@'))
            token = await _usersService.LoginUserByEmail(request.Login, request.Password, _httpContextAccessor.HttpContext!);
        else
            token = await _usersService.LoginUserByUsername(request.Login, request.Password, _httpContextAccessor.HttpContext!);
            
        if(token == string.Empty)
            return BadRequest("Invalid login or password");
        
        return Ok();
    }
}