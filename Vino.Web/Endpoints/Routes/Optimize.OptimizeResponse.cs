using BetterThanYou.Core.DTOs.Route;

namespace BetterThanYou.Web.Endpoints.Routes;

public class RouteOptimizeResponse
{
    public Guid RouteId { get; set; }
    public double DistanciaTotal { get; set; }
    public int TempoEstimado { get; set; }
    public List<RouteStopDto> ParadasOtimizadas { get; set; } = new List<RouteStopDto>();
    public string Mensagem { get; set; } = string.Empty;
}