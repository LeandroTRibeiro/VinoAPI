using Ardalis.ApiEndpoints;
using BetterThanYou.Core.Interfaces.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Clients;

[Authorize]
public class Delete : EndpointBaseAsync
    .WithRequest<Guid>
    .WithActionResult
{
    private readonly IClientService _clientService;

    public Delete(IClientService clientService)
    {
        _clientService = clientService;
    }
    
    [HttpDelete("api/v1/clients/{id}")]
    [SwaggerOperation(
        Summary = "Delete (deactivate) a client",
        Description = "Deactivates a client (soft delete)",
        OperationId = "Clients.Delete",
        Tags = new[] { "Clients" })]
    public override async Task<ActionResult> HandleAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        await _clientService.DeleteAsync(id);
        return NoContent();
    }
}