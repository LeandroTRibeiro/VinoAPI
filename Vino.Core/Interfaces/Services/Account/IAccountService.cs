using BetterThanYou.Core.DTOs.Account;

namespace BetterThanYou.Core.Interfaces.Services.Account;

public interface IAccountService
{
    Task<AccountDto> CreateAsync(string email, string password, string name);
    Task<LoginDto> LoginAsync(string email, string password);
}