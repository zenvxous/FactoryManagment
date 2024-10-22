using FactoryManagment.Domain.Models;

namespace FactoryManagment.Domain.Interfaces.Repositories;

public interface IUsersRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task CreateAsync(User user);
}