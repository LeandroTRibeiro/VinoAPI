using Ardalis.ApiEndpoints;
using BetterThanYou.Core.Interfaces.Route;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Routes;

[Authorize]
public class List : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<RouteListResponse>
{
    private readonly IRouteService _routeService;

    public List(IRouteService routeService)
    {
        _routeService = routeService;
    }
    
    [HttpGet("api/v1/routes")]
    [SwaggerOperation(
        Summary = "List all routes",
        Description = "Returns all active routes",
        OperationId = "Routes.List",
        Tags = new[] { "Routes" })]
    public override async Task<ActionResult<RouteListResponse>> HandleAsync(
        CancellationToken cancellationToken = default)
    {
        var routes = await _routeService.GetAllAsync();

        var response = new RouteListResponse
        {
            Routes = routes
        };

        return Ok(response);
    }
}