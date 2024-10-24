using FactoryManagment.Domain.Enums;

namespace FactoryManagment.Domain.Interfaces;

public interface IWorkerRequestIdentifier
{
    WorkerRequestTypes Identify(string workerRequestFilter);
}