using FactoryManagment.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FactoryManagment.Persistence;

public class LogsDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public LogsDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<LogEntity> Logs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("LogsDb"));
    }
}