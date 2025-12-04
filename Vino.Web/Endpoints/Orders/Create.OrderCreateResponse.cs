using BetterThanYou.Core.DTOs.Order;
using BetterThanYou.SharedKernel.Enums;

namespace BetterThanYou.Web.Endpoints.Orders;

public class OrderCreateResponse
{
    public Guid Id { get; set; }
    public string NumeroOrder { get; set; } = string.Empty;
    public Guid ClienteId { get; set; }
    public string ClienteNome { get; set; } = string.Empty;
    public DateTime DataPedido { get; set; }
    public OrderStatus Status { get; set; }
    public decimal ValorTotal { get; set; }
    public string? Observacoes { get; set; }
    public string? EnderecoEntrega { get; set; }
    public DateTime? DataEntregaPrevista { get; set; }
    public List<OrderItemDto> Itens { get; set; } = new List<OrderItemDto>();
    public string CriadoPor { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
}