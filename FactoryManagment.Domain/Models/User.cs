using FactoryManagment.Domain.Validators;

namespace FactoryManagment.Domain.Models;

public class User
{
    private User(Guid id, string username, string hashedPassword, string email)
    {
        Id = id;
        Username = username;
        HashedPassword = hashedPassword;
        Email = email;
    }
    public Guid Id { get; private set; }
    
    public string Username { get; private set; }
    
    public string HashedPassword { get; private set; }
    
    public string Email { get; private set; }

    public static (string Error, User User) Create(Guid id, string username, string hashedPassword, string email)
    {
        var error = UsersValidator.Validate(username, hashedPassword);
        
        var user = new User(id, username, hashedPassword, email);
        
        return (error, user);
    }
}