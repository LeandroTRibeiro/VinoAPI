using BetterThanYou.Core.DTOs.Order;

namespace BetterThanYou.Web.Endpoints.Orders;

public class OrderListResponse
{
    public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
}