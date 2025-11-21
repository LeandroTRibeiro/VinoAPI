using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BetterThanYou.Core.DTOs.Account;
using BetterThanYou.Core.Interfaces.Services.Account;
using BetterThanYou.SharedKernel.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BetterThanYou.Core.Services.Account;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IConfiguration _configuration;

    public AccountService(
        IAccountRepository accountRepository,
        IConfiguration configuration
    )
    {
        _accountRepository = accountRepository;
        _configuration = configuration;
    }
    public async Task<AccountDto> CreateAsync(string email, string password, string name)
    {
        if (!EmailValidation.IsValid(email))
            throw new ArgumentException("Email is invalid", nameof(email));

        var (isValid, errorMessage) = PasswordValidation.ValidateWithMessage(password);
        
        if (!isValid)
            throw new ArgumentException(errorMessage, nameof(password));
        
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is invalid", nameof(name));

        var account = new Entities.Account
        {
            Id = Guid.NewGuid(),
            Email = email.Trim(),
            Name = name.Trim(),
            PasswordHash = HashPassword(password),
            CreatedAt = DateTime.UtcNow,
        };
        
        var saved = await _accountRepository.CreateAsync(account);

        return new AccountDto()
        {
            Id = saved.Id,
            Email = saved.Email,
            Name = saved.Name,
        };
    }

    public async Task<LoginDto> LoginAsync(string email, string password)
    {
        if (!EmailValidation.IsValid(email))
            throw new ArgumentException("Email is invalid", nameof(email));

        var (isValid, errorMessage) = PasswordValidation.ValidateWithMessage(password);
        
        if (!isValid)
            throw new ArgumentException(errorMessage, nameof(password));
        
        var account = await _accountRepository.GetByEmailAsync(email);
        
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, account.PasswordHash);
        
        if(!isPasswordValid)
            throw new UnauthorizedAccessException("Invalid credentials");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
            new Claim(ClaimTypes.Email, account.Email),
            new Claim(ClaimTypes.Name, account.Name),
        };
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(
                double.Parse(_configuration["Jwt:ExpireMinutes"] ?? "60")
            ),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return new LoginDto
        {
            Id = account.Id,
            Email = account.Email,
            Name = account.Name,
            Token = tokenString,
        };
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
    }
}