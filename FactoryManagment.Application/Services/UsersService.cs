using Microsoft.AspNetCore.Http;
using FactoryManagment.Domain.Interfaces.Auth;
using FactoryManagment.Domain.Interfaces.Repositories;
using FactoryManagment.Domain.Interfaces.Services;
using FactoryManagment.Domain.Models;

namespace FactoryManagment.Application.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public UsersService(IUsersRepository usersRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    {
        _usersRepository = usersRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    private bool CheckUserLogin(User? user, string password)
    {
        if (user == null)
            return false;
        
        if(!_passwordHasher.VerifyHashedPassword(user.HashedPassword, password))
            return false;
        
        return true;
    }

    public async Task<string> RegisterUser(string username, string password, string email)
    {
        var hashedPassword = _passwordHasher.HashPassword(password);
        
        var (error, user) = User.Create(Guid.NewGuid(), username, hashedPassword, email);

        if (!string.IsNullOrEmpty(error))
            return error;

        await _usersRepository.CreateAsync(user);
        
        return string.Empty;
    }

    public async Task<string> LoginUserByUsername(string username, string password, HttpContext context)
    {
        var user  = await _usersRepository.GetByUsernameAsync(username);

        if (!CheckUserLogin(user, password))
            return string.Empty;
        
        var token = _jwtProvider.GenerateToken(user!);
        
        context.Response.Cookies.Append("Token", token);
        
        return token;
    }

    public async Task<string> LoginUserByEmail(string email, string password, HttpContext context)
    {
        var user = await _usersRepository.GetByEmailAsync(email);
        
        if (!CheckUserLogin(user, password))
            return string.Empty;
        
        var token = _jwtProvider.GenerateToken(user!);
        
        context.Response.Cookies.Append("Token", token);
        
        return token;
    }
}