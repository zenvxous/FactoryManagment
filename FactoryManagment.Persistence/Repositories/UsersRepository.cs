using FactoryManagment.Domain.Interfaces.Repositories;
using FactoryManagment.Domain.Models;
using FactoryManagment.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace FactoryManagment.Persistence.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly FactoryDbContext _dbContext;

    public UsersRepository(FactoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var userEntity = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

        return userEntity == null ? null : User.Create(
            userEntity.Id,
            userEntity.Username,
            userEntity.HashedPassword,
            userEntity.Email).User;
    }
    
    public async Task CreateAsync(User user)
    {
        var userEntity = new UserEntity
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            HashedPassword = user.HashedPassword
        };
        
        await _dbContext.Users.AddAsync(userEntity);
        await _dbContext.SaveChangesAsync();
    }
}