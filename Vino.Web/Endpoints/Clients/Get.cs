using Ardalis.ApiEndpoints;
using BetterThanYou.Core.Interfaces.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Clients;

[Authorize]
public class Get : EndpointBaseAsync
    .WithRequest<Guid>
    .WithActionResult<ClientGetResponse>
{
    private readonly IClientService _clientService;

    public Get(IClientService clientService)
    {
        _clientService = clientService;
    }
    
    [HttpGet("api/v1/clients/{id}")]
    [SwaggerOperation(
        Summary = "Get client by ID",
        Description = "Returns a single client by its ID",
        OperationId = "Clients.Get",
        Tags = new[] { "Clients" })]
    public override async Task<ActionResult<ClientGetResponse>> HandleAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var client = await _clientService.GetByIdAsync(id);
        
        return Ok(client);
    }
}