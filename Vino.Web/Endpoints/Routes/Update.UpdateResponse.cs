using BetterThanYou.Core.DTOs.Route;
using BetterThanYou.SharedKernel.Enums;

namespace BetterThanYou.Web.Endpoints.Routes;

public class RouteUpdateResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public DateTime DataRota { get; set; }
    public RouteStatus Status { get; set; }
    public List<RouteStopDto> Paradas { get; set; } = new List<RouteStopDto>();
    public string? ModificadoPor { get; set; }
    public DateTime? DataModificacao { get; set; }
}