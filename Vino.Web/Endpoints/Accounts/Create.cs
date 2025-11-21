using Ardalis.ApiEndpoints;
using BetterThanYou.Core.Interfaces.Services.Account;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Accounts;

public class Create : EndpointBaseAsync
    .WithRequest<CreateRequest>
    .WithActionResult<CreateResponse>
{
    private readonly IAccountService _accountService;

    public Create(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    [HttpPost("api/v1/accounts")]
    [SwaggerOperation(
        Summary = "Create a new account",
        Description = "Registers a new account (minimal stub)",
        OperationId = "Accounts.Create",
        Tags = new[] { "Accounts" })]
    public override async Task<ActionResult<CreateResponse>> HandleAsync([FromBody]CreateRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var accountDto = await _accountService.CreateAsync(request.Email, request.Password, request.Name);

        var response = new CreateResponse
        {
            Id = accountDto.Id,
            Email = accountDto.Email,
            Name = accountDto.Name,
        };
        
        return Created($"api/v1/accounts/{response.Id}", response);
    }
}