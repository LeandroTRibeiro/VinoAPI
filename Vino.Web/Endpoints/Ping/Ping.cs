using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Ping;

public class Ping : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<string>
{
    [HttpGet("api/ping")]
    [SwaggerOperation(
        Summary = "Ping Pong",
        Description = "Retorna uma string simples para teste.",
        OperationId = "Ping",
        Tags = new[] { "Ping" })]
    public override async Task<ActionResult<string>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return await Task.FromResult("Pong");
    }
}