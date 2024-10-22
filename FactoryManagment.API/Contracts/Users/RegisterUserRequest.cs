using System.ComponentModel.DataAnnotations;

namespace FactoryManagment.API.Contracts.Users;

public record RegisterUserRequest(
    [Required] string Username,
    [Required] string Password,
    [Required] string Email);
