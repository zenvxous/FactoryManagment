using System.Text.RegularExpressions;

namespace FactoryManagment.Domain.Validators;

public static class UpdateWorkerRequestValidator
{
    private const string EMAIL_PATTERN = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    private const string PHONE_PATTERN = @"^\+375\d{9}$";
    
    public static string Validate(string  email, string phoneNumber)
    {
        if (!IsEmailValid(email))
            return "Invalid email address!";
        
        if (!IsPhoneValid(phoneNumber))
            return "Invalid phone number!";
        
        return string.Empty;
    }
    
    private static bool IsPhoneValid(string phoneNumber)
    {
        return Regex.IsMatch(phoneNumber, PHONE_PATTERN);
    }
    
    private static bool IsEmailValid(string email)
    {
        return Regex.IsMatch(email, EMAIL_PATTERN);
    }
}