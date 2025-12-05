using BetterThanYou.Core.DTOs.Route;
using BetterThanYou.SharedKernel.Enums;

namespace BetterThanYou.Web.Endpoints.Routes;

public class RouteCreateResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public DateTime DataRota { get; set; }
    public RouteStatus Status { get; set; }
    public List<RouteStopDto> Paradas { get; set; } = new List<RouteStopDto>();
    public string CriadoPor { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
}