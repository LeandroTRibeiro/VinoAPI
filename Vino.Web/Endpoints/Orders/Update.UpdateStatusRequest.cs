using BetterThanYou.SharedKernel.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BetterThanYou.Web.Endpoints.Orders;

public class UpdateStatusRequest
{
    public Guid Id { get; set; }
    public required OrderStatus Status { get; set; }
}