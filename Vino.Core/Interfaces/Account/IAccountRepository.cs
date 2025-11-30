namespace BetterThanYou.Core.Interfaces.Services.Account;

public interface IAccountRepository
{
    Task<Entities.Account> CreateAsync(Entities.Account account);
    Task<Entities.Account> GetByEmailAsync(string email);
}