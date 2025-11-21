namespace BetterThanYou.Core.DTOs.Account;

public class LoginDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}