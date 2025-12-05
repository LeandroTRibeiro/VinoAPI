using BetterThanYou.Core.Entities;
using BetterThanYou.Core.Interfaces.Order;
using BetterThanYou.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BetterThanYou.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _db;

    public OrderRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Order> CreateAsync(Order order)
    {
        await _db.Orders.AddAsync(order);
        await _db.SaveChangesAsync();
        return order;
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _db.Orders
            .Include(o => o.Cliente)
            .Include(o => o.Itens)
                .ThenInclude(i => i.Produto)
            .Where(o => o.Ativo)
            .OrderByDescending(o => o.DataCriacao)
            .ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _db.Orders
            .Include(o => o.Cliente)
            .Include(o => o.Itens)
                .ThenInclude(i => i.Produto)
            .FirstOrDefaultAsync(o => o.Id == id && o.Ativo);
    }

public async Task<Order> UpdateAsync(Order order)
{
    
    var trackedItems = _db.ChangeTracker.Entries<OrderItem>()
        .Where(e => e.Entity.OrderId == order.Id)
        .ToList();
    
    foreach (var entry in trackedItems)
    {
        entry.State = EntityState.Detached;
    }
    
    var deletedCount = await _db.OrderItems
        .Where(i => i.OrderId == order.Id)
        .ExecuteDeleteAsync();
    
    var orderFromDb = await _db.Orders
        .FirstOrDefaultAsync(o => o.Id == order.Id);
    
    if (orderFromDb == null)
    {
        throw new KeyNotFoundException("Pedido não encontrado");
    }
    
    orderFromDb.ClienteId = order.ClienteId;
    orderFromDb.Observacoes = order.Observacoes;
    orderFromDb.EnderecoEntrega = order.EnderecoEntrega;
    orderFromDb.DataEntregaPrevista = order.DataEntregaPrevista;
    orderFromDb.ModificadoPor = order.ModificadoPor;
    orderFromDb.DataModificacao = order.DataModificacao;
    orderFromDb.ValorTotal = order.ValorTotal;
    orderFromDb.Status = order.Status;
    orderFromDb.DataEntregaRealizada = order.DataEntregaRealizada;
    
    foreach (var item in order.Itens)
    {
        item.OrderId = orderFromDb.Id;
        await _db.OrderItems.AddAsync(item);
    }
    
    var entries = _db.ChangeTracker.Entries().ToList();
    
    foreach (var entry in entries)
    {
    }
    
    try
    {
        var changes = await _db.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        throw;
    }
    
    await _db.Entry(orderFromDb).Collection(o => o.Itens).LoadAsync();
    
    
    return orderFromDb;
}

    public async Task DeleteAsync(Order order)
    {
        order.Ativo = false;
        _db.Orders.Update(order);
        await _db.SaveChangesAsync();
    }

    public async Task<string> GenerateNextOrderNumberAsync()
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"PED-{year}-";
        
        var lastOrder = await _db.Orders
            .Where(o => o.NumeroOrder.StartsWith(prefix))
            .OrderByDescending(o => o.NumeroOrder)
            .FirstOrDefaultAsync();
        
        int nextNumber = 1;
        
        if (lastOrder != null)
        {
            var lastNumberStr = lastOrder.NumeroOrder.Replace(prefix, "");
            if (int.TryParse(lastNumberStr, out int lastNumber))
            {
                nextNumber = lastNumber + 1;
            }
        }
        
        return $"{prefix}{nextNumber:D4}";
    }
    
    public async Task DeleteOrderItemsAsync(Guid orderId)
    {
        var items = await _db.OrderItems.Where(i => i.OrderId == orderId).ToListAsync();
        _db.OrderItems.RemoveRange(items);
    }
    
public async Task<Order> UpdateWithItemsAsync(Order order, List<OrderItem> novosItens)
{
    
    var orderFromDb = await _db.Orders
        .Include(o => o.Cliente)
        .Include(o => o.Itens)
            .ThenInclude(i => i.Produto)
        .FirstOrDefaultAsync(o => o.Id == order.Id);
    
    
    if (orderFromDb == null)
        throw new KeyNotFoundException("Pedido não encontrado");
    
    
    orderFromDb.ClienteId = order.ClienteId;
    orderFromDb.Observacoes = order.Observacoes;
    orderFromDb.EnderecoEntrega = order.EnderecoEntrega;
    orderFromDb.DataEntregaPrevista = order.DataEntregaPrevista;
    orderFromDb.ModificadoPor = order.ModificadoPor;
    orderFromDb.DataModificacao = order.DataModificacao;
    orderFromDb.ValorTotal = order.ValorTotal;
    orderFromDb.Status = order.Status;
    orderFromDb.DataEntregaRealizada = order.DataEntregaRealizada;
    
    
    var itensAntigos = await _db.OrderItems.Where(i => i.OrderId == orderFromDb.Id).ToListAsync();
    _db.OrderItems.RemoveRange(itensAntigos);
    
    
    orderFromDb.Itens.Clear();
    
    
    foreach (var item in novosItens)
    {
        item.OrderId = orderFromDb.Id;
   
        await _db.OrderItems.AddAsync(item);
    }
    
    
    await _db.SaveChangesAsync();
    
    
    await _db.Entry(orderFromDb).Collection(o => o.Itens).LoadAsync();
    
    return orderFromDb;
}
public async Task<Order> UpdateStatusAsync(Order order)
{
    var orderFromDb = await _db.Orders
        .Include(o => o.Cliente)
        .FirstOrDefaultAsync(o => o.Id == order.Id);
    
    if (orderFromDb == null)
        throw new KeyNotFoundException("Pedido não encontrado");
    
    orderFromDb.Status = order.Status;
    orderFromDb.DataEntregaRealizada = order.DataEntregaRealizada;
    orderFromDb.ModificadoPor = order.ModificadoPor;
    orderFromDb.DataModificacao = order.DataModificacao;
    
    await _db.SaveChangesAsync();
    
    await _db.Entry(orderFromDb).Collection(o => o.Itens).Query()
        .Include(i => i.Produto)
        .LoadAsync();
    
    return orderFromDb;
}
}