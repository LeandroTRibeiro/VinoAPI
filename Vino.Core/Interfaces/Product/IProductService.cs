using BetterThanYou.Core.DTOs.Account;

namespace BetterThanYou.Core.Interfaces.Product;

public interface IProductService
{
    Task<ProdutctDto> CreateAsync(string nome, string descricao, int quantidadeEstoque, decimal precoCusto, decimal precoVenda, string? fotoUrl, string criadoPor);
    Task<List<ProdutctDto>> GetAllAsync();
    Task<ProdutctDto> GetByIdAsync(Guid id);
}