using System.IdentityModel.Tokens.Jwt;
using FactoryManagment.Domain.Enums;
using FactoryManagment.Domain.Interfaces;
using FactoryManagment.Domain.Interfaces.Repositories;
using FactoryManagment.Domain.Interfaces.Services;
using FactoryManagment.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace FactoryManagment.Application.Services;

public class WorkersService : IWorkersService
{
    private readonly IWorkersRepository _workersRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly ILogsRepository _logsRepository;
    private readonly IWorkerRequestIdentifier _workerRequestIdentifier;

    public WorkersService(
        IWorkersRepository workersRepository,
        IUsersRepository usersRepository,
        ILogsRepository logsRepository,
        IWorkerRequestIdentifier workerRequestIdentifier)
    {
        _workersRepository = workersRepository;
        _usersRepository = usersRepository;
        _logsRepository = logsRepository;
        _workerRequestIdentifier = workerRequestIdentifier;
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

        return await _workersRepository.GetAllAsync();
    }

    public async Task<List<Worker>> GetAllByJobWorkersAsync(HttpContext context, Jobs job)
    {
        var user = await GetCurrentUser(context);
        await _logsRepository.CreateAsync(user, Actions.Check);
        
        return await _workersRepository.GetAllByJobAsync(job);
    }

    public async Task<Worker?> GetWorkerByIdentifierAsync(HttpContext context, string workerIdentifier)
    {
        var user = await GetCurrentUser(context);
        await _logsRepository.CreateAsync(user, Actions.Check);

        Worker? worker;
        switch (_workerRequestIdentifier.Identify(workerIdentifier))
        {
            case(WorkerRequestTypes.Email):
                worker = await _workersRepository.GetByEmailAsync(workerIdentifier);
                break;
            case(WorkerRequestTypes.Phone):
                worker = await _workersRepository.GetByPhoneNumberAsync(workerIdentifier);
                break;
            default:
                worker = null;
                break;
        }

        return worker;
    }

    public async Task<string> CreateWorkerAsync(HttpContext context, Worker worker)
    {
        var user = await GetCurrentUser(context);
        await _logsRepository.CreateAsync(user, Actions.Create);

        if (await _workersRepository.GetByEmailAsync(worker.Email) != null)
            return "Worker with this email already exists";
        
        if (await _workersRepository.GetByPhoneNumberAsync(worker.PhoneNumber) != null)
            return "Worker with this phone number already exists";
        
        await _workersRepository.CreateAsync(worker);
        
        return string.Empty;
    }

    public async Task UpdateWorkerAsync(HttpContext context, Guid id, string email, string phoneNumber, Jobs job)
    {
        var user = await GetCurrentUser(context);
        await _logsRepository.CreateAsync(user, Actions.Update);
        
        await _workersRepository.UpdateAsync(id, email, phoneNumber, job);
    }

    public async Task DeleteWorkerAsync(HttpContext context, Guid id)
    {
        var user = await GetCurrentUser(context);
        await _logsRepository.CreateAsync(user, Actions.Delete);
        
        await _workersRepository.DeleteAsync(id);
    }
    
}