using Ardalis.ApiEndpoints;
using BetterThanYou.Core.Interfaces.Services.Account;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Accounts;

public class Login : EndpointBaseAsync
    .WithRequest<AccountLoginRequest>
    .WithResult<AccountLoginResponse>
{
    private readonly IAccountService _accountService;

    public Login(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    [HttpPost("api/v1/login")]
    [SwaggerOperation(
        Summary = "Authenticate user",
        Description = "Authenticates a user with email and password, returning an access token if the credentials are valid.",
        OperationId = "Login",
        Tags = new[] { "Authentication" }
    )]
    public async override Task<AccountLoginResponse> HandleAsync(AccountLoginRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var result = await _accountService.LoginAsync(request.Email, request.Password);

        return new AccountLoginResponse
        {
            Id = result.Id,
            Email = result.Email,
            Name = result.Name,
            Token = result.Token
        };
    }
}