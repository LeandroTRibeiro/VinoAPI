using BetterThanYou.SharedKernel.Enums;

namespace BetterThanYou.Core.Entities;

public class Route
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public DateTime DataRota { get; set; }
    public RouteStatus Status { get; set; }
    
    // Ponto de partida (opcional - pode ser o endereço da empresa)
    public string? EnderecoPartida { get; set; }
    public string? CidadePartida { get; set; }
    public string? EstadoPartida { get; set; }
    public string? CepPartida { get; set; }
    public double? LatitudePartida { get; set; }
    public double? LongitudePartida { get; set; }
    
    // Informações calculadas após otimização
    public double? DistanciaTotal { get; set; } // em km
    public int? TempoEstimado { get; set; } // em minutos
    public bool Otimizada { get; set; } = false;
    public DateTime? DataOtimizacao { get; set; }
    
    public string CriadoPor { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public string? ModificadoPor { get; set; }
    public DateTime? DataModificacao { get; set; }
    public bool Ativo { get; set; } = true;
    
    // Navigation properties
    public List<RouteStop> Paradas { get; set; } = new List<RouteStop>();
}