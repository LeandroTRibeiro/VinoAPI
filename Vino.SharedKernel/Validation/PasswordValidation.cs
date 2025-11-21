using System.Text.RegularExpressions;

namespace BetterThanYou.SharedKernel.Validation;

public class PasswordValidation
{
    private const int MinLength = 8;

    public static bool IsValid(string? password)
    {
        if (string.IsNullOrWhiteSpace(password)) return false;
        
        if (password.Length < MinLength) return false;

        if (!Regex.IsMatch(password, @"[A-Z]")) return false;
        
        if (!Regex.IsMatch(password, @"[a-z]")) return false;
        
        if (!Regex.IsMatch(password, @"[0-9]")) return false;
        
        if (!Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]"))
            return false;

        return true;
    }

    public static (bool isValid, string errorMessage) ValidateWithMessage(string? password)
    {
        if (string.IsNullOrWhiteSpace(password)) 
            return (false, "Password is required.");
        
        if (password.Length < MinLength) 
            return (false, $"Password must be at least {MinLength} charactes long.");

        if (!Regex.IsMatch(password, @"[A-Z]"))
            return (false, "Password must contain at least one upper case letter.");
        
        if(!Regex.IsMatch(password, @"[a-z]"))
            return (false, "Password must contain at least one lower case letter.");

        if (!Regex.IsMatch(password, @"[0-9]"))
            return (false, "Password must contain at least one number");
        
        if(!Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]"))
            return (false, "Password must contain at least one special character.");

        return (true, string.Empty);

    }
}