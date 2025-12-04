using BetterThanYou.SharedKernel.Enums;

namespace BetterThanYou.Core.Entities;

public class Order
{
    public Guid Id { get; set; }
    public string NumeroOrder { get; set; } = string.Empty;
    public Guid ClienteId { get; set; }
    public DateTime DataPedido { get; set; }
    public OrderStatus Status { get; set; }
    public decimal ValorTotal { get; set; }
    public string? Observacoes { get; set; }
    public string? EnderecoEntrega { get; set; }
    public DateTime? DataEntregaPrevista { get; set; }
    public DateTime? DataEntregaRealizada { get; set; }
    
    public string CriadoPor { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public string? ModificadoPor { get; set; }
    public DateTime? DataModificacao { get; set; }
    public bool Ativo { get; set; } = true;
    
    // Navigation properties
    public Client Cliente { get; set; } = null!;
    public List<OrderItem> Itens { get; set; } = new List<OrderItem>();
}