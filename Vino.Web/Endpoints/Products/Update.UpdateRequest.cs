using Microsoft.AspNetCore.Http;

namespace BetterThanYou.Web.Endpoints.Products;

public class ProductUpdateRequest
{
    public Guid Id { get; set; } 
    public required string Nome { get; set; }
    public required string Descricao { get; set; }
    public required int QuantidadeEstoque { get; set; }
    public required decimal PrecoCusto { get; set; }
    public required decimal PrecoVenda { get; set; }
    public IFormFile? Foto { get; set; }
}