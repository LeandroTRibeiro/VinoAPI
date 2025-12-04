using Ardalis.ApiEndpoints;
using BetterThanYou.Core.Interfaces.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Orders;

[Authorize]
public class Delete : EndpointBaseAsync
    .WithRequest<Guid>
    .WithActionResult
{
    private readonly IOrderService _orderService;

    public Delete(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    [HttpDelete("api/v1/orders/{id}")]
    [SwaggerOperation(
        Summary = "Delete (deactivate) an order",
        Description = "Deactivates an order (soft delete)",
        OperationId = "Orders.Delete",
        Tags = new[] { "Orders" })]
    public override async Task<ActionResult> HandleAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        await _orderService.DeleteAsync(id);
        return NoContent();
    }
}