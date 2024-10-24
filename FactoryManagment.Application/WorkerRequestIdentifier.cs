using System.Text.RegularExpressions;
using FactoryManagment.Domain.Enums;
using FactoryManagment.Domain.Interfaces;

namespace FactoryManagment.Application;

public class WorkerRequestIdentifier : IWorkerRequestIdentifier
{
    private const string EMAIL_PATTERN = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    private const string PHONE_PATTERN = @"^\+375\d{9}$";

    public  WorkerRequestTypes Identify(string workerRequestFilter)
    {
        if (IsEmailValid(workerRequestFilter))
            return WorkerRequestTypes.Email;
        
        if(IsPhoneValid(workerRequestFilter))
            return WorkerRequestTypes.Phone;
        
        return WorkerRequestTypes.Unknown;
    }
    
    private bool IsEmailValid(string email)
    {
        return Regex.IsMatch(email, EMAIL_PATTERN);
    }
    
    private bool IsPhoneValid(string phoneNumber)
    {
        return Regex.IsMatch(phoneNumber, PHONE_PATTERN);
    }
}