using FactoryManagment.Domain.Models;

namespace FactoryManagment.Domain.Interfaces.Auth;

public interface IJwtProvider
{
    string GenerateToken(User user);
}