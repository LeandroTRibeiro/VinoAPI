using Ardalis.ApiEndpoints;
using BetterThanYou.Core.Interfaces.Route;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Routes;

[Authorize]
public class UpdateStopStatus : EndpointBaseAsync
    .WithRequest<UpdateStopStatusRequest>
    .WithActionResult<UpdateStopStatusResponse>
{
    private readonly IRouteService _routeService;

    public UpdateStopStatus(IRouteService routeService)
    {
        _routeService = routeService;
    }
    
    [HttpPatch("api/v1/routes/{routeId}/stops/{stopId}")]
    [SwaggerOperation(
        Summary = "Update stop status",
        Description = "Updates the status of a specific stop (mark as visited, register times, add notes)",
        OperationId = "Routes.UpdateStopStatus",
        Tags = new[] { "Routes" })]
    public override async Task<ActionResult<UpdateStopStatusResponse>> HandleAsync(
        [FromRoute] UpdateStopStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var routeDto = await _routeService.UpdateStopStatusAsync(
                request.RouteId,
                request.StopId,
                request.HorarioChegada,
                request.HorarioSaida,
                request.Observacoes
            );

            var response = new UpdateStopStatusResponse
            {
                Route = routeDto
            };

            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}