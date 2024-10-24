using FactoryManagment.Domain.Enums;
using FactoryManagment.Domain.Validators;

namespace FactoryManagment.Domain.Models;

public class Worker
{
    private const string EMAIL_PATTERN = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    
    private Worker(Guid id, string firstName, string lastName, string email, string phoneNumber, DateTimeOffset dateOfBirth, Jobs job)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        DateOfBirth = dateOfBirth;
        Job = job;
    }
    
    public Guid Id { get; private set; }
    
    public string FirstName { get; private set; }
    
    public string LastName { get; private set; }
    
    public string Email { get; private set; }
    
    public string PhoneNumber { get; private set; }
    
    public DateTimeOffset DateOfBirth { get; private set; }
    
    public Jobs Job { get; private set; }

    public static (string Error, Worker Worker) Create(Guid id, string firstName, string lastName, string email, string phoneNumber ,DateTimeOffset dateOfBirth, Jobs job)
    {
        var error = WorkersValidator.Validate(firstName, lastName, email, phoneNumber, dateOfBirth);
        
        var worker = new Worker(id, firstName, lastName, email, phoneNumber, dateOfBirth, job);

        return (error, worker);
    }
}