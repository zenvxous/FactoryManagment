using FactoryManagment.Domain.Enums;

namespace FactoryManagment.Domain.Models;

public class Log
{
    public Log(Guid id, string username, DateTime dateTime, Actions action)
    {
        Id = id;
        Time = dateTime;
        Username = username;
        Action = action;
    }
    
    public Guid Id { get; private set; }
    
    public DateTime Time { get; private set; }
    
    public string Username { get; private set; } 
    
    public Actions Action { get; private set; }
}