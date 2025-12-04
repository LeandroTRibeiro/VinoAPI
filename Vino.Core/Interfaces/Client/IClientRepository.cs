namespace BetterThanYou.Core.Interfaces.Client;

public interface IClientRepository
{
    Task<Entities.Client> CreateAsync(Entities.Client client);
    Task<List<Entities.Client>> GetAllAsync();
    Task<Entities.Client?> GetByIdAsync(Guid id);
    Task<Entities.Client> UpdateAsync(Entities.Client client);
    Task DeleteAsync(Entities.Client client);
    Task<bool> ExistsByNomeFantasiaAsync(string nomeFantasia);
    Task<bool> ExistsByCpfCnpjAsync(string cpfCnpj);
    Task<bool> ExistsByNomeFantasiaAsync(string nomeFantasia, Guid excludeId);
    Task<bool> ExistsByCpfCnpjAsync(string cpfCnpj, Guid excludeId);
}