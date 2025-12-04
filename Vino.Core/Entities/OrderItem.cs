namespace BetterThanYou.Core.Entities;

public class OrderItem
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal { get; set; }
    
    // Navigation properties
    public Order Order { get; set; } = null!;
    public Product Produto { get; set; } = null!;
}