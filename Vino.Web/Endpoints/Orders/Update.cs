using Ardalis.ApiEndpoints;
using BetterThanYou.Core.Interfaces.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Orders;

[Authorize]
public class Update : EndpointBaseAsync
    .WithRequest<OrderUpdateRequest>
    .WithActionResult<OrderUpdateResponse>
{
    private readonly IOrderService _orderService;

    public Update(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    [HttpPut("api/v1/orders")]
    [SwaggerOperation(
        Summary = "Update an order",
        Description = "Updates an existing order (only if status is Pendente)",
        OperationId = "Orders.Update",
        Tags = new[] { "Orders" })]
    public override async Task<ActionResult<OrderUpdateResponse>> HandleAsync(
        OrderUpdateRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                     ?? throw new UnauthorizedAccessException("Usuário não identificado");

        var orderDto = await _orderService.UpdateAsync(
            request.Id,
            request.ClienteId,
            request.Itens,
            request.Observacoes,
            request.EnderecoEntrega,
            request.DataEntregaPrevista,
            userId);

        var response = new OrderUpdateResponse
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