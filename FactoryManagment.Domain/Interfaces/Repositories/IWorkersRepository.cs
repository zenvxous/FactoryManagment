using FactoryManagment.Domain.Enums;
using FactoryManagment.Domain.Models;

namespace FactoryManagment.Domain.Interfaces.Repositories;

public interface IWorkersRepository
{
    Task<List<Worker>> GetAllAsync();
    Task<Worker?> GetByEmailAsync(string email);
    Task<Worker?> GetByPhoneNumberAsync(string phoneNumber);
    Task<List<Worker>> GetAllByJobAsync(Jobs job);
    Task CreateAsync(Worker worker);
    Task UpdateAsync(Guid id, string email, string phoneNumber, Jobs job);
    Task DeleteAsync(Guid id);
}