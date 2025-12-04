using BetterThanYou.Core.Interfaces.Order;

namespace BetterThanYou.Web.Endpoints.Orders;

public class OrderUpdateRequest
{
    public required Guid Id { get; set; }
    public required Guid ClienteId { get; set; }
    public required List<OrderItemCreateDto> Itens { get; set; }
    public string? Observacoes { get; set; }
    public string? EnderecoEntrega { get; set; }
    public DateTime? DataEntregaPrevista { get; set; }
}