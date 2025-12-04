using BetterThanYou.SharedKernel.Enums;

namespace BetterThanYou.Web.Endpoints.Orders;

public class UpdateStatusResponse
{
    public Guid Id { get; set; }
    public string NumeroOrder { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public DateTime? DataEntregaRealizada { get; set; }
}