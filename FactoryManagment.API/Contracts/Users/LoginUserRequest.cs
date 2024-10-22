using System.ComponentModel.DataAnnotations;

namespace FactoryManagment.API.Contracts.Users;

public record LoginUserRequest(
    [Required] string Login,
    [Required] string Password);
