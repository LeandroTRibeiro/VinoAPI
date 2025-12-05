using Ardalis.ApiEndpoints;
using BetterThanYou.Core.Interfaces.Route;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Routes;

[Authorize]
public class Get : EndpointBaseAsync
    .WithRequest<Guid>
    .WithActionResult<RouteGetResponse>
{
    private readonly IRouteService _routeService;

    public Get(IRouteService routeService)
    {
        _routeService = routeService;
    }
    
    [HttpGet("api/v1/routes/{id}")]
    [SwaggerOperation(
        Summary = "Get route by ID",
        Description = "Returns a single route by its ID with all stops",
        OperationId = "Routes.Get",
        Tags = new[] { "Routes" })]
    public override async Task<ActionResult<RouteGetResponse>> HandleAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var route = await _routeService.GetByIdAsync(id);
            
            var response = new RouteGetResponse
            {
                Route = route
            };

            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}