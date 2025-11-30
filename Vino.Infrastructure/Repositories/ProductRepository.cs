using BetterThanYou.Core.Entities;
using BetterThanYou.Core.Interfaces.Product;
using BetterThanYou.Infrastructure.Data;

namespace BetterThanYou.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;

    public ProductRepository(AppDbContext db)
    {
        _db = db;
    }
    
    public async Task<Product> CreateAsync(Product product)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return product;
    }
}