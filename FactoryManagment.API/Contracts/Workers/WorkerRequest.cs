using System.ComponentModel.DataAnnotations;

namespace FactoryManagment.API.Contracts.Workers;

public record WorkerRequest(
    [Required] string FirstName, // Example
    [Required] string LastName, // Example
    [Required] string Email, // example@example.com
    [Required] string PhoneNumber, // +375123456789
    [Required] string DateOfBirth, // 2222-10-10 YYYY-MM-DD
    [Required] string Job);