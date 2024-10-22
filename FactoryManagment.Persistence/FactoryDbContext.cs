using FactoryManagment.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FactoryManagment.Persistence;

public class FactoryDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public FactoryDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public DbSet<WorkerEntity> Workers { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("FactoryDb"));
    }
}