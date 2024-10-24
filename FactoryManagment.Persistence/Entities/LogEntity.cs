using FactoryManagment.Domain.Enums;

namespace FactoryManagment.Persistence.Entities;

public class LogEntity
{
    public Guid Id { get; set; }
    public DateTime Time { get; set; } = DateTime.Now;
    
    public string Username { get; set; } = string.Empty;
    
    public Actions Action { get; set; }
}