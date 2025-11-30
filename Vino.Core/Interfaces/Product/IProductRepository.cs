namespace BetterThanYou.Core.Interfaces.Product;

public interface IProductRepository
{
    Task<Entities.Product> CreateAsync(Entities.Product product);
    Task<List<Entities.Product>> GetAllAsync();
    Task<Entities.Product> GetByIdAsync(Guid id);
}