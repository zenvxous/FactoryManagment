using System.Text.RegularExpressions;

namespace FactoryManagment.Domain.Validators;

public static class WorkersValidator
{
    private const string EMAIL_PATTERN = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    private const string PHONE_PATTERN = @"^\+375\d{9}$";

    public static string Validate(string firstName, string lastName, string email, string phoneNumber, DateTimeOffset dateOfBirth)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(email))
            return "All fields must be filled!";

        if (!IsFirstOrLastNameValid(firstName, lastName))
            return "First and last name should start with uppercase letter";

        if (!IsEmailValid(email))
            return "Invalid email address!";
        
        if (!IsPhoneValid(phoneNumber))
            return "Invalid phone number!";

        if (!IsAgeValid(dateOfBirth))
            return "Invalid age!";

        return string.Empty;
    }

    private static bool IsFirstOrLastNameValid(string firstName, string lastName)
    {
        return char.IsUpper(firstName[0]) && char.IsUpper(lastName[0]);
    }

    private static bool IsEmailValid(string email)
    {
        return Regex.IsMatch(email, EMAIL_PATTERN);
    }
    
    private static bool IsPhoneValid(string phoneNumber)
    {
        return Regex.IsMatch(phoneNumber, PHONE_PATTERN);
    }

    private static bool IsAgeValid(DateTimeOffset dateOfBirth)
    {
        var age = DateTimeOffset.Now.Year - dateOfBirth.Year;
        if (DateTimeOffset.Now.DayOfYear < dateOfBirth.DayOfYear)
            age++;
        
        return age >= 18;
    }
}