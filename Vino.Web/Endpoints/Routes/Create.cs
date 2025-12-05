using Ardalis.ApiEndpoints;
using BetterThanYou.Core.DTOs.Route;
using BetterThanYou.Core.Interfaces.Route;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Routes;

[Authorize]
public class Create : EndpointBaseAsync
    .WithRequest<RouteCreateRequest>
    .WithActionResult<RouteCreateResponse>
{
    private readonly IRouteService _routeService;

    public Create(IRouteService routeService)
    {
        _routeService = routeService;
    }
    
    [HttpPost("api/v1/routes")]
    [SwaggerOperation(
        Summary = "Create a new route",
        Description = "Creates a new route with client stops",
        OperationId = "Routes.Create",
        Tags = new[] { "Routes" })]
    public override async Task<ActionResult<RouteCreateResponse>> HandleAsync(
        [FromBody] RouteCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value 
                         ?? throw new UnauthorizedAccessException("Usuário não identificado");

            var dto = new RouteCreateDto
            {
                Nome = request.Nome,
                Descricao = request.Descricao,
                DataRota = request.DataRota,
                EnderecoPartida = request.EnderecoPartida,
                CidadePartida = request.CidadePartida,
                EstadoPartida = request.EstadoPartida,
                CepPartida = request.CepPartida,
                ClientesIds = request.ClientesIds
            };

            var routeDto = await _routeService.CreateAsync(dto, userId);

            var response = new RouteCreateResponse
            {
                Id = routeDto.Id,
                Nome = routeDto.Nome,
                Descricao = routeDto.Descricao,
                DataRota = routeDto.DataRota,
                Status = routeDto.Status,
                Paradas = routeDto.Paradas,
                CriadoPor = routeDto.CriadoPor,
                DataCriacao = routeDto.DataCriacao
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