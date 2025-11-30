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

    public async Task<List<ProdutctDto>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();
    
        return products.Select(p => new ProdutctDto
        {
            Id = p.Id,
            Nome = p.Nome,
            Descricao = p.Descricao,
            QuantidadeEstoque = p.QuantidadeEstoque,
            PrecoCusto = p.PrecoCusto,
            PrecoVenda = p.PrecoVenda,
            FotoUrl = p.FotoUrl,
            CriadoPor = p.CriadoPor,
            DataCriacao = p.DataCriacao,
            ModificadoPor = p.ModificadoPor,
            DataModificacao = p.DataModificacao,
            Ativo = p.Ativo
        }).ToList();    
    }

    public async Task<ProdutctDto> GetByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
            return null;

        return new ProdutctDto
        {
            Id = product.Id,
            Nome = product.Nome,
            Descricao = product.Descricao,
            QuantidadeEstoque = product.QuantidadeEstoque,
            PrecoCusto = product.PrecoCusto,
            PrecoVenda = product.PrecoVenda,
            FotoUrl = product.FotoUrl,
            CriadoPor = product.CriadoPor,
            DataCriacao = product.DataCriacao,
            ModificadoPor = product.ModificadoPor,
            DataModificacao = product.DataModificacao,
            Ativo = product.Ativo
        };    
    }
    
    public async Task<ProdutctDto> UpdateAsync(Guid id, string nome, string descricao, 
        int quantidadeEstoque, decimal precoCusto, decimal precoVenda, string? fotoUrl, string modificadoPor)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório", nameof(nome));

        if (quantidadeEstoque < 0)
            throw new ArgumentException("Quantidade em estoque não pode ser negativa", nameof(quantidadeEstoque));

        if (precoCusto < 0)
            throw new ArgumentException("Preço de custo não pode ser negativo", nameof(precoCusto));

        if (precoVenda < 0)
            throw new ArgumentException("Preço de venda não pode ser negativo", nameof(precoVenda));

        var product = await _productRepository.GetByIdAsync(id);
    
        if (product == null)
            throw new KeyNotFoundException("Produto não encontrado");

        product.Nome = nome;
        product.Descricao = descricao;
        product.QuantidadeEstoque = quantidadeEstoque;
        product.PrecoCusto = precoCusto;
        product.PrecoVenda = precoVenda;
    
        if (!string.IsNullOrEmpty(fotoUrl))
            product.FotoUrl = fotoUrl;
    
        product.ModificadoPor = modificadoPor;
        product.DataModificacao = DateTime.UtcNow;

        var updated = await _productRepository.UpdateAsync(product);

        return new ProdutctDto
        {
            Id = updated.Id,
            Nome = updated.Nome,
            Descricao = updated.Descricao,
            QuantidadeEstoque = updated.QuantidadeEstoque,
            PrecoCusto = updated.PrecoCusto,
            PrecoVenda = updated.PrecoVenda,
            FotoUrl = updated.FotoUrl,
            CriadoPor = updated.CriadoPor,
            DataCriacao = updated.DataCriacao,
            ModificadoPor = updated.ModificadoPor,
            DataModificacao = updated.DataModificacao,
            Ativo = updated.Ativo
        };
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
    
        if (product == null)
            throw new KeyNotFoundException("Produto não encontrado");

        await _productRepository.DeleteAsync(product);
    }
}