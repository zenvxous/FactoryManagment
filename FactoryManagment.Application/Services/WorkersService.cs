using System.IdentityModel.Tokens.Jwt;
using FactoryManagment.Domain.Enums;
using FactoryManagment.Domain.Interfaces;
using FactoryManagment.Domain.Interfaces.Repositories;
using FactoryManagment.Domain.Interfaces.Services;
using FactoryManagment.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace FactoryManagment.Application.Services;

public class WorkersService : IWorkersService
{
    private readonly IWorkersRepository _workersRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly ILogsRepository _logsRepository;
    private readonly IWorkerRequestIdentifier _workerRequestIdentifier;
    private readonly IMemoryCache _memoryCache;

    public WorkersService(
        IWorkersRepository workersRepository,
        IUsersRepository usersRepository,
        ILogsRepository logsRepository,
        IWorkerRequestIdentifier workerRequestIdentifier,
        IMemoryCache memoryCache)
    {
        _workersRepository = workersRepository;
        _usersRepository = usersRepository;
        _logsRepository = logsRepository;
        _workerRequestIdentifier = workerRequestIdentifier;
        _memoryCache = memoryCache;
    }

    private async Task<User> GetCurrentUser(HttpContext context)
    {
        var token = context.Request.Cookies.FirstOrDefault(x => x.Key == "Token").Value;
        
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        
        var strId = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId")!.Value;
        
        return (await _usersRepository.GetByIdAsync(Guid.Parse(strId)))!;
    }

    public async Task<List<Worker>> GetAllWorkersAsync(HttpContext context)
    {
        var user = await GetCurrentUser(context);
        await _logsRepository.CreateAsync(user, Actions.Check);

        if (_memoryCache.TryGetValue("AllWorkers", out List<Worker>? workers))
            return workers!;
        
        workers = await _workersRepository.GetAllAsync();

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));
            
        _memoryCache.Set("AllWorkers", workers, cacheEntryOptions);

        return workers;
    }

    public async Task<List<Worker>> GetAllByJobWorkersAsync(HttpContext context, Jobs job)
    {
        var user = await GetCurrentUser(context);
        await _logsRepository.CreateAsync(user, Actions.Check);
        
        var cacheKey = $"WorkersByJob_{job}";
        if (_memoryCache.TryGetValue(cacheKey, out List<Worker>? workers))
            return workers!;
        
        workers = await _workersRepository.GetAllByJobAsync(job);
        
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));
        
        _memoryCache.Set(cacheKey, workers, cacheEntryOptions);

        return workers;
    }

    public async Task<Worker?> GetWorkerByIdentifierAsync(HttpContext context, string workerIdentifier)
    {
        var user = await GetCurrentUser(context);
        await _logsRepository.CreateAsync(user, Actions.Check);

        var cacheKey = $"Worker_{workerIdentifier}";

        if (_memoryCache.TryGetValue(cacheKey, out Worker? worker)) 
            return worker;
        
        switch (_workerRequestIdentifier.Identify(workerIdentifier))
        {
            case WorkerRequestTypes.Email:
                worker = await _workersRepository.GetByEmailAsync(workerIdentifier);
                break;
            case WorkerRequestTypes.Phone:
                worker = await _workersRepository.GetByPhoneNumberAsync(workerIdentifier);
                break;
            default:
                worker = null;
                break;
        }
            
        if (worker != null)
            _memoryCache.Set(cacheKey, worker, TimeSpan.FromMinutes(5));

        return worker;
    }

    public async Task<string> CreateWorkerAsync(HttpContext context, Worker worker)
    {
        var user = await GetCurrentUser(context);
        await _logsRepository.CreateAsync(user, Actions.Create);

        if (await _workersRepository.GetByEmailAsync(worker.Email) != null)
            return "Worker with this email already exists!";
        
        if (await _workersRepository.GetByPhoneNumberAsync(worker.PhoneNumber) != null)
            return "Worker with this phone number already exists!";
        
        await _workersRepository.CreateAsync(worker);
        
        _memoryCache.Remove("AllWorkers");
        _memoryCache.Remove($"WorkersByJob_{worker.Job}");
        
        return string.Empty;
    }

    public async Task<string> UpdateWorkerAsync(HttpContext context, Guid id, string email, string phoneNumber, Jobs job)
    {
        var worker = await _workersRepository.GetByEmailAsync(email);
        if (worker != null && worker.Id != id)
            return "Worker with this email already exists!";
        worker = await _workersRepository.GetByPhoneNumberAsync(phoneNumber);
        if (worker != null && worker.Id != id)
            return "Worker with this phone number already exists!";
        
        var user = await GetCurrentUser(context);
        
        await _logsRepository.CreateAsync(user, Actions.Update);
        
        await _workersRepository.UpdateAsync(id, email, phoneNumber, job);
        
        _memoryCache.Remove("AllWorkers");
        _memoryCache.Remove($"WorkersByJob_{job}");
        
        return string.Empty;
    }

    public async Task DeleteWorkerAsync(HttpContext context, Guid id)
    {
        var user = await GetCurrentUser(context);
        await _logsRepository.CreateAsync(user, Actions.Delete);
        
        await _workersRepository.DeleteAsync(id);
        
        _memoryCache.Remove("AllWorkers");
    }
}