using BetterThanYou.Core.Entities;
using BetterThanYou.SharedKernel.Enums;

namespace BetterThanYou.Core.DTOs.Route;

public class RouteDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public DateTime DataRota { get; set; }
    public RouteStatus Status { get; set; }
    
    public string? EnderecoPartida { get; set; }
    public string? CidadePartida { get; set; }
    public string? EstadoPartida { get; set; }
    public string? CepPartida { get; set; }
    public double? LatitudePartida { get; set; }
    public double? LongitudePartida { get; set; }
    
    public double? DistanciaTotal { get; set; }
    public int? TempoEstimado { get; set; }
    public bool Otimizada { get; set; }
    public DateTime? DataOtimizacao { get; set; }
    
    public string CriadoPor { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public string? ModificadoPor { get; set; }
    public DateTime? DataModificacao { get; set; }
    public bool Ativo { get; set; }
    
    public List<RouteStopDto> Paradas { get; set; } = new List<RouteStopDto>();
}

public class RouteStopDto
{
    public Guid Id { get; set; }
    public Guid RouteId { get; set; }
    public Guid ClienteId { get; set; }
    public string ClienteNome { get; set; } = string.Empty;
    public string? ClienteEndereco { get; set; }
    public string? ClienteCidade { get; set; }
    public string? ClienteEstado { get; set; }
    
    public int OrdemOriginal { get; set; }
    public int OrdemOtimizada { get; set; }
    
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    
    public double? DistanciaParadaAnterior { get; set; }
    public int? TempoParadaAnterior { get; set; }
    
    public DateTime? HorarioChegadaPrevisto { get; set; }
    public DateTime? HorarioChegadaReal { get; set; }
    public DateTime? HorarioSaidaReal { get; set; }
    public string? Observacoes { get; set; }
    public bool Visitado { get; set; }
}

public class RouteCreateDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public DateTime DataRota { get; set; }
    
    public string? EnderecoPartida { get; set; }
    public string? CidadePartida { get; set; }
    public string? EstadoPartida { get; set; }
    public string? CepPartida { get; set; }
    
    public List<Guid> ClientesIds { get; set; } = new List<Guid>();
}

public class RouteUpdateDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public DateTime DataRota { get; set; }
    public RouteStatus Status { get; set; }
    
    public string? EnderecoPartida { get; set; }
    public string? CidadePartida { get; set; }
    public string? EstadoPartida { get; set; }
    public string? CepPartida { get; set; }
    
    public List<Guid> ClientesIds { get; set; } = new List<Guid>();
}

public class RouteOptimizationResultDto
{
    public Guid RouteId { get; set; }
    public double DistanciaTotal { get; set; }
    public int TempoEstimado { get; set; }
    public List<RouteStopDto> ParadasOtimizadas { get; set; } = new List<RouteStopDto>();
    public string Mensagem { get; set; } = string.Empty;
}