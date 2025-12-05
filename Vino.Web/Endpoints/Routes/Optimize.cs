using Ardalis.ApiEndpoints;
using BetterThanYou.Core.Interfaces.Route;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Routes;

[Authorize]
public class Optimize : EndpointBaseAsync
    .WithRequest<Guid>
    .WithActionResult<RouteOptimizeResponse>
{
    private readonly IRouteService _routeService;

    public Optimize(IRouteService routeService)
    {
        _routeService = routeService;
    }
    
    [HttpPost("api/v1/routes/{id}/optimize")]
    [SwaggerOperation(
        Summary = "Optimize route",
        Description = "Optimizes the route using Nearest Neighbor algorithm to minimize distance and time",
        OperationId = "Routes.Optimize",
        Tags = new[] { "Routes" })]
    public override async Task<ActionResult<RouteOptimizeResponse>> HandleAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _routeService.OptimizeRouteAsync(id);

            var response = new RouteOptimizeResponse
            {
                RouteId = result.RouteId,
                DistanciaTotal = result.DistanciaTotal,
                TempoEstimado = result.TempoEstimado,
                ParadasOtimizadas = result.ParadasOtimizadas,
                Mensagem = result.Mensagem
            };

            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}