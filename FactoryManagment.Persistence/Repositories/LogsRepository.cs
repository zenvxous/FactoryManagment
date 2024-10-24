using FactoryManagment.Domain.Enums;
using FactoryManagment.Domain.Interfaces.Repositories;
using FactoryManagment.Domain.Models;
using FactoryManagment.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace FactoryManagment.Persistence.Repositories;

public class LogsRepository : ILogsRepository
{
    private readonly LogsDbContext _context;

    public LogsRepository(LogsDbContext context)
    {
        _context = context;
    }

    public async Task<List<Log>> GetAllAsync()
    {
        var logEntities = await _context.Logs
            .AsNoTracking()
            .ToListAsync();

        return logEntities.Select(l =>
            new Log(
                l.Id,
                l.Username,
                l.Time,
                l.Action))
            .ToList();
    }

    public async Task CreateAsync(User user, Actions action)
    {
        var logEntity = new LogEntity
        {
            Id = Guid.NewGuid(),
            Username = user.Username,
            Action = action,
            Time = DateTime.UtcNow
        };
        
        await _context.Logs.AddAsync(logEntity);
        await _context.SaveChangesAsync();
    }
}