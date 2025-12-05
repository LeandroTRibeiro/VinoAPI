namespace BetterThanYou.Core.Entities;

public class RouteStop
{
    public Guid Id { get; set; }
    public Guid RouteId { get; set; }
    public Guid ClienteId { get; set; }
    
    // Ordem original (antes da otimização)
    public int OrdemOriginal { get; set; }
    
    // Ordem otimizada (após a otimização)
    public int OrdemOtimizada { get; set; }
    
    // Coordenadas geocodificadas
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    
    // Informações de tempo/distância para esta parada
    public double? DistanciaParadaAnterior { get; set; } // em km
    public int? TempoParadaAnterior { get; set; } // em minutos
    
    // Informações da visita
    public DateTime? HorarioChegadaPrevisto { get; set; }
    public DateTime? HorarioChegadaReal { get; set; }
    public DateTime? HorarioSaidaReal { get; set; }
    public string? Observacoes { get; set; }
    public bool Visitado { get; set; } = false;
    
    // Navigation properties
    public Route Route { get; set; } = null!;
    public Client Cliente { get; set; } = null!;
}