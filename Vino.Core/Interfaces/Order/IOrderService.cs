using BetterThanYou.Core.DTOs.Order;
using BetterThanYou.SharedKernel.Enums;

namespace BetterThanYou.Core.Interfaces.Order;

public interface IOrderService
{
    Task<OrderDto> CreateAsync(Guid clienteId, List<OrderItemCreateDto> itens, 
        string? observacoes, string? enderecoEntrega, DateTime? dataEntregaPrevista, string criadoPor);
    
    Task<List<OrderDto>> GetAllAsync();
    
    Task<OrderDto> GetByIdAsync(Guid id);
    
    Task<OrderDto> UpdateAsync(Guid id, Guid clienteId, List<OrderItemCreateDto> itens, 
        string? observacoes, string? enderecoEntrega, DateTime? dataEntregaPrevista, string modificadoPor);
    
    Task<OrderDto> UpdateStatusAsync(Guid id, OrderStatus status, string modificadoPor);
    
    Task DeleteAsync(Guid id);
}

public class OrderItemCreateDto
{
    public Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
}