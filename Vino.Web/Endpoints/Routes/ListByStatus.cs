using Ardalis.ApiEndpoints;
using BetterThanYou.Core.Interfaces.Route;
using BetterThanYou.SharedKernel.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Routes;

[Authorize]
public class ListByStatus : EndpointBaseAsync
    .WithRequest<RouteStatus>
    .WithActionResult<RouteListResponse>
{
    private readonly IRouteService _routeService;

    public ListByStatus(IRouteService routeService)
    {
        _routeService = routeService;
    }
    
    [HttpGet("api/v1/routes/by-status/{status}")]
    [SwaggerOperation(
        Summary = "List routes by status",
        Description = "Returns routes with a specific status (0=Planejada, 1=EmAndamento, 2=Concluida, 3=Cancelada)",
        OperationId = "Routes.ListByStatus",
        Tags = new[] { "Routes" })]
    public override async Task<ActionResult<RouteListResponse>> HandleAsync(
        [FromRoute] RouteStatus status,
        CancellationToken cancellationToken = default)
    {
        var routes = await _routeService.GetByStatusAsync(status);

        var response = new RouteListResponse
        {
            Routes = routes
        };

        return Ok(response);
    }
}