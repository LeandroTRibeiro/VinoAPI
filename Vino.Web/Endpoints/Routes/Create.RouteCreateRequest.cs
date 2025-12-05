namespace BetterThanYou.Web.Endpoints.Routes;

public class RouteCreateRequest
{
    public required string Nome { get; set; }
    public string? Descricao { get; set; }
    public DateTime DataRota { get; set; }
    
    public string? EnderecoPartida { get; set; }
    public string? CidadePartida { get; set; }
    public string? EstadoPartida { get; set; }
    public string? CepPartida { get; set; }
    
    public List<Guid> ClientesIds { get; set; } = new List<Guid>();
}