using BetterThanYou.Core.Entities;

namespace BetterThanYou.Core.Interfaces.Order;

public interface IOrderRepository
{
    Task<Entities.Order> CreateAsync(Entities.Order order);
    Task<List<Entities.Order>> GetAllAsync();
    Task<Entities.Order?> GetByIdAsync(Guid id);
    Task<Entities.Order> UpdateAsync(Entities.Order order);
    Task DeleteAsync(Entities.Order order);
    Task<string> GenerateNextOrderNumberAsync();
    Task DeleteOrderItemsAsync(Guid orderId);
    Task<Entities.Order> UpdateWithItemsAsync(Entities.Order order, List<OrderItem> novosItens);
    Task<Entities.Order> UpdateStatusAsync(Entities.Order order);
}