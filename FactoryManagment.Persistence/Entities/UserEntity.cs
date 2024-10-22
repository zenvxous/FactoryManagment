namespace FactoryManagment.Persistence.Entities;

public class UserEntity
{
    public Guid Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string HashedPassword { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
}