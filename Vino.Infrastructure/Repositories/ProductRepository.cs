using BetterThanYou.Core.Entities;
using BetterThanYou.Core.Interfaces.Product;
using BetterThanYou.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<Product>> GetAllAsync()
    {
        return await _db.Products
            .Where(p => p.Ativo)
            .OrderByDescending(p => p.DataCriacao)
            .ToListAsync();
        
    }
}