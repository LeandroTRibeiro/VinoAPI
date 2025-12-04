using Ardalis.ApiEndpoints;
using BetterThanYou.Core.Interfaces.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Orders;

[Authorize]
public class Create : EndpointBaseAsync
    .WithRequest<OrderCreateRequest>
    .WithActionResult<OrderCreateResponse>
{
    private readonly IOrderService _orderService;

    public Create(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    [HttpPost("api/v1/orders")]
    [SwaggerOperation(
        Summary = "Create a new order",
        Description = "Creates a new order",
        OperationId = "Orders.Create",
        Tags = new[] { "Orders" })]
    public override async Task<ActionResult<OrderCreateResponse>> HandleAsync(
        OrderCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                     ?? throw new UnauthorizedAccessException("Usuário não identificado");

        var orderDto = await _orderService.CreateAsync(
            request.ClienteId,
            request.Itens,
            request.Observacoes,
            request.EnderecoEntrega,
            request.DataEntregaPrevista,
            userId);

        var response = new OrderCreateResponse
        {
            Id = orderDto.Id,
            NumeroOrder = orderDto.NumeroOrder,
            ClienteId = orderDto.ClienteId,
            ClienteNome = orderDto.ClienteNome,
            DataPedido = orderDto.DataPedido,
            Status = orderDto.Status,
            ValorTotal = orderDto.ValorTotal,
            Observacoes = orderDto.Observacoes,
            EnderecoEntrega = orderDto.EnderecoEntrega,
            DataEntregaPrevista = orderDto.DataEntregaPrevista,
            Itens = orderDto.Itens,
            CriadoPor = orderDto.CriadoPor,
            DataCriacao = orderDto.DataCriacao
        };

        return Ok(response);
    }
}