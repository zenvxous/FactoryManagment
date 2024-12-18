using System.Text.RegularExpressions;

namespace FactoryManagment.Domain.Validators;

public static class UsersValidator
{
    private const string EMAIL_PATTERN = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    private const int MAX_USERNAME_LENGTH = 50;

    public static string Validate(string username, string email)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email))
            return "Username and/or email address are required";
        
        if (username.Length > MAX_USERNAME_LENGTH)
            return "Username is too long";
        
        if (!IsEmailValid(email))
            return "Invalid email address";
        
        return string.Empty;
    }
    
    private static bool IsEmailValid(string email)
    {
        return Regex.IsMatch(email, EMAIL_PATTERN);
    }
}