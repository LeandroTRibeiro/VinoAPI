namespace BetterThanYou.Web.Endpoints.Products;

public class ProductCreateResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public int QuantidadeEstoque { get; set; }
    public decimal PrecoCusto { get; set; }
    public decimal PrecoVenda { get; set; }
    public string? FotoUrl { get; set; }
    public string CriadoPor { get; set; }
    public DateTime DataCriacao { get; set; }
}