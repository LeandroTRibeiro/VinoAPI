using Ardalis.ApiEndpoints;
using BetterThanYou.Core.Interfaces.Route;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Routes;

[Authorize]
public class Delete : EndpointBaseAsync
    .WithRequest<Guid>
    .WithActionResult
{
    private readonly IRouteService _routeService;

    public Delete(IRouteService routeService)
    {
        _routeService = routeService;
    }
    
    [HttpDelete("api/v1/routes/{id}")]
    [SwaggerOperation(
        Summary = "Delete (deactivate) a route",
        Description = "Deactivates a route (soft delete)",
        OperationId = "Routes.Delete",
        Tags = new[] { "Routes" })]
    public override async Task<ActionResult> HandleAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _routeService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}