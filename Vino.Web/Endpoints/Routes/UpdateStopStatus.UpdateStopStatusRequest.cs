using Microsoft.AspNetCore.Mvc;

namespace BetterThanYou.Web.Endpoints.Routes;

public class UpdateStopStatusRequest
{
    [FromRoute(Name = "routeId")]
    public Guid RouteId { get; set; }
    
    [FromRoute(Name = "stopId")]
    public Guid StopId { get; set; }
    
    [FromBody]
    public UpdateStopStatusBody Body { get; set; } = null!;
    
    public DateTime? HorarioChegada => Body?.HorarioChegada;
    public DateTime? HorarioSaida => Body?.HorarioSaida;
    public string? Observacoes => Body?.Observacoes;
}

public class UpdateStopStatusBody
{
    public DateTime? HorarioChegada { get; set; }
    public DateTime? HorarioSaida { get; set; }
    public string? Observacoes { get; set; }
}