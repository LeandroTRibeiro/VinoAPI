using System.Security.Authentication;
using BetterThanYou.Core.Entities;
using BetterThanYou.Core.Interfaces.Services.Account;
using BetterThanYou.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BetterThanYou.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _db;

    public AccountRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Account> CreateAsync(Account account)
    {
        var exists = await _db.Accounts.AnyAsync(a => a.Email == account.Email);
        
        if (exists)
            throw new InvalidOperationException("Email already in use");

        _db.Accounts.Add(account);
        await _db.SaveChangesAsync();
        return account;
    }

    public async Task<Account> GetByEmailAsync(string email)
    {
        var account = await _db.Accounts.SingleOrDefaultAsync(a => a.Email == email);
        
        if (account is null)
            throw new InvalidCredentialException("Email invalid");
        
        return account;
    }
}