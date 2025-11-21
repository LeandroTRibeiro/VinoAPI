using System.Net.Mail;

namespace BetterThanYou.SharedKernel.Validation;

public class EmailValidation
{
    public static bool IsValid(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;

        try
        {
            var addr = new MailAddress(input);
            var normalized = addr.Address.Trim();

            if (normalized.Contains(' ')) return false; 
            if (normalized.EndsWith(".")) return false; 

            return true;
        }
        catch
        {
            return false;
        }
    }
}