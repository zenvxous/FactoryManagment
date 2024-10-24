using FactoryManagment.Domain.Enums;

namespace FactoryManagment.Persistence.Entities;

public class WorkerEntity
{
    public Guid Id { get;  set; }
    
    public string FirstName { get;  set; } = string.Empty;
    
    public string LastName { get;  set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    
    public string PhoneNumber { get; set; } = string.Empty;
    
    public DateTimeOffset DateOfBirth { get;  set; }
    
    public Jobs Job { get; set; }
}