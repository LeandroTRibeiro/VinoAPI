using Ardalis.ApiEndpoints;
using BetterThanYou.Core.Interfaces.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BetterThanYou.Web.Endpoints.Orders;

[Authorize]
public class List : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<OrderListResponse>
{
    private readonly IOrderService _orderService;

    public List(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    [HttpGet("api/v1/orders")]
    [SwaggerOperation(
        Summary = "List all orders",
        Description = "Returns all active orders",
        OperationId = "Orders.List",
        Tags = new[] { "Orders" })]
    public override async Task<ActionResult<OrderListResponse>> HandleAsync(
        CancellationToken cancellationToken = default)
    {
        var orders = await _orderService.GetAllAsync();

        var response = new OrderListResponse
        {
            Orders = orders
        };

        return Ok(response);
    }
}