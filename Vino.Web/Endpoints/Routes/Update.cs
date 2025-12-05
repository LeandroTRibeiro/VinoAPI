using Ardalis.ApiEndpoints;
using BetterThanYou.Core.DTOs.Route;
using BetterThanYou.Core.Interfaces.Route;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Routes;

[Authorize]
public class Update : EndpointBaseAsync
    .WithRequest<RouteUpdateRequest>
    .WithActionResult<RouteUpdateResponse>
{
    private readonly IRouteService _routeService;

    public Update(IRouteService routeService)
    {
        _routeService = routeService;
    }
    
    [HttpPut("api/v1/routes")]
    [SwaggerOperation(
        Summary = "Update a route",
        Description = "Updates an existing route",
        OperationId = "Routes.Update",
        Tags = new[] { "Routes" })]
    public override async Task<ActionResult<RouteUpdateResponse>> HandleAsync(
        [FromBody] RouteUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value 
                         ?? throw new UnauthorizedAccessException("Usuário não identificado");

            var dto = new RouteUpdateDto
            {
                Nome = request.Nome,
                Descricao = request.Descricao,
                DataRota = request.DataRota,
                Status = request.Status,
                EnderecoPartida = request.EnderecoPartida,
                CidadePartida = request.CidadePartida,
                EstadoPartida = request.EstadoPartida,
                CepPartida = request.CepPartida,
                ClientesIds = request.ClientesIds
            };

            var routeDto = await _routeService.UpdateAsync(request.Id, dto, userId);

            var response = new RouteUpdateResponse
            {
                Id = routeDto.Id,
                Nome = routeDto.Nome,
                Descricao = routeDto.Descricao,
                DataRota = routeDto.DataRota,
                Status = routeDto.Status,
                Paradas = routeDto.Paradas,
                ModificadoPor = routeDto.ModificadoPor,
                DataModificacao = routeDto.DataModificacao
            };

            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}