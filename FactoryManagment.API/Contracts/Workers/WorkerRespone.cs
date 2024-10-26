namespace FactoryManagment.API.Contracts.Workers;

public record WorkerResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string DateOfBirth,
    string Job);