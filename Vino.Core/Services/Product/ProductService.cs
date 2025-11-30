using BetterThanYou.Core.DTOs.Account;
using BetterThanYou.Core.Interfaces.Product;

namespace BetterThanYou.Core.Services.Product;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<ProdutctDto> CreateAsync(string nome, string descricao, int quantidadeEstoque, decimal precoCusto, decimal precoVenda,
        string? fotoUrl, string criadoPor)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório", nameof(nome));

        if (quantidadeEstoque < 0)
            throw new ArgumentException("Quantidade em estoque não pode ser negativa", nameof(quantidadeEstoque));

        if (precoCusto < 0)
            throw new ArgumentException("Preço de custo não pode ser negativo", nameof(precoCusto));

        if (precoVenda < 0)
            throw new ArgumentException("Preço de venda não pode ser negativo", nameof(precoVenda));
        
        var product = new Entities.Product
        {
            Id = Guid.NewGuid(),
            Nome = nome,
            Descricao = descricao,
            QuantidadeEstoque = quantidadeEstoque,
            PrecoCusto = precoCusto,
            PrecoVenda = precoVenda,
            FotoUrl = fotoUrl,
            CriadoPor = criadoPor,
            DataCriacao = DateTime.UtcNow,
            Ativo = true
        };
        
        var saved = await _productRepository.CreateAsync(product);

        return new ProdutctDto
        {
            Id = saved.Id,
            Nome = saved.Nome,
            Descricao = saved.Descricao,
            QuantidadeEstoque = saved.QuantidadeEstoque,
            PrecoCusto = saved.PrecoCusto,
            PrecoVenda = saved.PrecoVenda,
            FotoUrl = saved.FotoUrl,
            CriadoPor = saved.CriadoPor,
            DataCriacao = saved.DataCriacao,
            Ativo = saved.Ativo
        };
    }
}