namespace BetterThanYou.Core.DTOs.Account;

public class AccountDto
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Email { get; init; }
    public string? Name { get; init; }
}