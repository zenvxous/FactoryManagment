using System.ComponentModel.DataAnnotations;

namespace FactoryManagment.API.Contracts.Workers;

public record UpdateWorkerRequest(
    [Required] Guid Id,
    [Required] string Email, // example@example.com
    [Required] string PhoneNumber, // +375123456789
    [Required] string Job); 
