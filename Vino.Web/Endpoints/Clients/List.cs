using Ardalis.ApiEndpoints;
using BetterThanYou.Core.Interfaces.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Clients;

[Authorize]
public class List : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<ClientListResponse>
{
    private readonly IClientService _clientService;

    public List(IClientService clientService)
    {
        _clientService = clientService;
    }
    
    [HttpGet("api/v1/clients")]
    [SwaggerOperation(
        Summary = "List all clients",
        Description = "Returns all active clients",
        OperationId = "Clients.List",
        Tags = new[] { "Clients" })]
    public override async Task<ActionResult<ClientListResponse>> HandleAsync(
        CancellationToken cancellationToken = default)
    {
        var clients = await _clientService.GetAllAsync();

        var response = new ClientListResponse
        {
            Clients = clients
        };

        return Ok(response);
    }
}