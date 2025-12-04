using BetterThanYou.Core.Entities;
using BetterThanYou.Core.Interfaces.Client;
using BetterThanYou.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BetterThanYou.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _db;

    public ClientRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Client> CreateAsync(Client client)
    {
        await _db.Clients.AddAsync(client);
        await _db.SaveChangesAsync();
        return client;
    }

    public async Task<List<Client>> GetAllAsync()
    {
        return await _db.Clients
            .Where(c => c.Ativo)
            .OrderByDescending(c => c.DataCriacao)
            .ToListAsync();
    }

    public async Task<Client?> GetByIdAsync(Guid id)
    {
        return await _db.Clients.FirstOrDefaultAsync(c => c.Id == id && c.Ativo);
    }

    public async Task<Client> UpdateAsync(Client client)
    {
        _db.Clients.Update(client);
        await _db.SaveChangesAsync();
        return client;
    }

    public async Task DeleteAsync(Client client)
    {
        client.Ativo = false;
        _db.Clients.Update(client);
        await _db.SaveChangesAsync();
    }
    
    public async Task<bool> ExistsByNomeFantasiaAsync(string nomeFantasia)
    {
        return await _db.Clients.AnyAsync(c => 
            c.NomeFantasia.ToLower() == nomeFantasia.ToLower() && c.Ativo);
    }

    public async Task<bool> ExistsByCpfCnpjAsync(string cpfCnpj)
    {
        return await _db.Clients.AnyAsync(c => 
            c.CpfCnpj == cpfCnpj && c.Ativo);
    }

    public async Task<bool> ExistsByNomeFantasiaAsync(string nomeFantasia, Guid excludeId)
    {
        return await _db.Clients.AnyAsync(c => 
            c.NomeFantasia.ToLower() == nomeFantasia.ToLower() && 
            c.Id != excludeId && 
            c.Ativo);
    }

    public async Task<bool> ExistsByCpfCnpjAsync(string cpfCnpj, Guid excludeId)
    {
        return await _db.Clients.AnyAsync(c => 
            c.CpfCnpj == cpfCnpj && 
            c.Id != excludeId && 
            c.Ativo);
    }
}