using Ardalis.ApiEndpoints;
using BetterThanYou.Core.Interfaces.Route;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Routes;

[Authorize]
public class ListByDateRange : EndpointBaseAsync
    .WithRequest<DateRangeRequest>
    .WithActionResult<RouteListResponse>
{
    private readonly IRouteService _routeService;

    public ListByDateRange(IRouteService routeService)
    {
        _routeService = routeService;
    }
    
    [HttpGet("api/v1/routes/by-date")]
    [SwaggerOperation(
        Summary = "List routes by date range",
        Description = "Returns routes within a specific date range",
        OperationId = "Routes.ListByDateRange",
        Tags = new[] { "Routes" })]
    public override async Task<ActionResult<RouteListResponse>> HandleAsync(
        [FromQuery] DateRangeRequest request,
        CancellationToken cancellationToken = default)
    {
        var routes = await _routeService.GetByDateRangeAsync(request.StartDate, request.EndDate);

        var response = new RouteListResponse
        {
            Routes = routes
        };

        return Ok(response);
    }
}

public class DateRangeRequest
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}