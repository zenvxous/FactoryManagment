using FactoryManagment.Domain.Models;

namespace FactoryManagment.Domain.Interfaces.Repositories;

public interface IUsersRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(Guid id);
    Task CreateAsync(User user);
}