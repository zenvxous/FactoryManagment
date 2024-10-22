using FactoryManagment.Domain.Enums;
using FactoryManagment.Domain.Interfaces.Repositories;
using FactoryManagment.Domain.Models;
using FactoryManagment.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace FactoryManagment.Persistence.Repositories;

public class WorkersRepository : IWorkersRepository
{
    private readonly FactoryDbContext _dbContext;

    public WorkersRepository(FactoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private List<Worker> CreateWorkers(List<WorkerEntity> workerEntities)
    {
        var workers = workerEntities
            .Select(w => Worker.Create(
                w.Id,
                w.FirstName,
                w.LastName,
                w.Email,
                w.PhoneNumber,
                w.DateOfBirth,
                w.Job).Worker)
            .ToList();
        
        return workers;
    }

    public async Task<List<Worker>> GetAllAsync()
    {
        var workerEntities = await _dbContext.Workers
            .AsNoTracking()
            .ToListAsync();
        
        return CreateWorkers(workerEntities);
    }

    public async Task<Worker?> GetByEmailAsync(string email)
    {
        var workerEntity = await _dbContext.Workers
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Email == email);
        
        return workerEntity == null ? null : Worker.Create(
            workerEntity.Id,
            workerEntity.FirstName,
            workerEntity.LastName,
            workerEntity.Email,
            workerEntity.PhoneNumber,
            workerEntity.DateOfBirth,
            workerEntity.Job).Worker;
    }

    public async Task<Worker?> GetByPhoneNumberAsync(string phoneNumber)
    {
        var workerEntity = await _dbContext.Workers
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.PhoneNumber == phoneNumber);
        
        return workerEntity == null ? null : Worker.Create(
            workerEntity.Id,
            workerEntity.FirstName,
            workerEntity.LastName,
            workerEntity.Email,
            workerEntity.PhoneNumber,
            workerEntity.DateOfBirth,
            workerEntity.Job).Worker;
    }

    public async Task<List<Worker>> GetAllByJobAsync(Jobs job)
    {
        var workerEntities = await _dbContext.Workers
            .AsNoTracking()
            .Where(w => w.Job == job)
            .ToListAsync();
        
        return CreateWorkers(workerEntities);
    }

    public async Task CreateAsync(Worker worker)
    {
        var workerEntity = new WorkerEntity
        {
            Id = worker.Id,
            FirstName = worker.FirstName,
            LastName = worker.LastName,
            Email = worker.Email,
            PhoneNumber = worker.PhoneNumber,
            DateOfBirth = worker.DateOfBirth,
            Job = worker.Job
        };
        
        await _dbContext.Workers.AddAsync(workerEntity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, string email, string phoneNumber, Jobs job)
    {
        await _dbContext.Workers
            .Where(w => w.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(w => w.Email, email)
                .SetProperty(w => w.PhoneNumber, phoneNumber)
                .SetProperty(w => w.Job, job));
    }

    public async Task DeleteAsync(Guid id)
    {
        await _dbContext.Workers
            .Where(w => w.Id == id)
            .ExecuteDeleteAsync();
    }
    
    
}