using FactoryManagment.Domain.Enums;
using FactoryManagment.Domain.Models;

namespace FactoryManagment.Domain.Interfaces.Repositories;

public interface ILogsRepository
{
    Task<List<Log>> GetAllAsync();
    Task CreateAsync(User user, Actions action);
}