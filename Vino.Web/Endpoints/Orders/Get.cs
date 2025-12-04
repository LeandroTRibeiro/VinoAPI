using Ardalis.ApiEndpoints;
using BetterThanYou.Core.Interfaces.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Orders;

[Authorize]
public class Get : EndpointBaseAsync
    .WithRequest<Guid>
    .WithActionResult<OrderGetResponse>
{
    private readonly IOrderService _orderService;

    public Get(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    [HttpGet("api/v1/orders/{id}")]
    [SwaggerOperation(
        Summary = "Get order by ID",
        Description = "Returns a single order by its ID",
        OperationId = "Orders.Get",
        Tags = new[] { "Orders" })]
    public override async Task<ActionResult<OrderGetResponse>> HandleAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var order = await _orderService.GetByIdAsync(id);
        
        return Ok(order);
    }
}