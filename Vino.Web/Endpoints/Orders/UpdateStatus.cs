using Ardalis.ApiEndpoints;
using BetterThanYou.Core.Interfaces.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Orders;

[Authorize]
public class UpdateStatus : EndpointBaseAsync
    .WithRequest<UpdateStatusRequest>
    .WithActionResult<UpdateStatusResponse>
{
    private readonly IOrderService _orderService;

    public UpdateStatus(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    [HttpPatch("api/v1/orders/status")]
    [SwaggerOperation(
        Summary = "Update order status",
        Description = "Updates the status of an order",
        OperationId = "Orders.UpdateStatus",
        Tags = new[] { "Orders" })]
    public override async Task<ActionResult<UpdateStatusResponse>> HandleAsync(
        UpdateStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                     ?? throw new UnauthorizedAccessException("Usuário não identificado");

        var orderDto = await _orderService.UpdateStatusAsync(
            request.Id,
            request.Status,
            userId);

        var response = new UpdateStatusResponse
        {
            Id = orderDto.Id,
            NumeroOrder = orderDto.NumeroOrder,
            Status = orderDto.Status,
            DataEntregaRealizada = orderDto.DataEntregaRealizada
        };

        return Ok(response);
    }
}