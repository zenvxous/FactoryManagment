using FactoryManagment.Domain.Enums;
using FactoryManagment.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace FactoryManagment.Domain.Interfaces.Services;

public interface IWorkersService
{
    Task<List<Worker>> GetAllWorkersAsync(HttpContext context);
    Task<List<Worker>> GetAllByJobWorkersAsync(HttpContext context, Jobs job);
    Task<Worker?> GetWorkerByIdentifierAsync(HttpContext context, string workerIdentifier);
    Task<string> CreateWorkerAsync(HttpContext context, Worker worker);
    Task<string> UpdateWorkerAsync(HttpContext context, Guid id, string email, string phoneNumber, Jobs job);
    Task DeleteWorkerAsync(HttpContext context, Guid id);
}