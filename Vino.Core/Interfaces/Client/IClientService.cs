using BetterThanYou.Core.DTOs.Account;

namespace BetterThanYou.Core.Interfaces.Client;

public interface IClientService
{
    Task<ClientDto> CreateAsync(string nomeFantasia, string? razaoSocial, string? email, 
        string? cpfCnpj, string? endereco, string? cidade, string? estado, string? cep, 
        List<Entities.ContactPhone> telefones, string? fotoUrl, string criadoPor);
    
    Task<List<ClientDto>> GetAllAsync();
    
    Task<ClientDto> GetByIdAsync(Guid id);
    
    Task<ClientDto> UpdateAsync(Guid id, string nomeFantasia, string? razaoSocial, string? email, 
        string? cpfCnpj, string? endereco, string? cidade, string? estado, string? cep, 
        List<Entities.ContactPhone> telefones, string? fotoUrl, string modificadoPor);
    
    Task DeleteAsync(Guid id);
}