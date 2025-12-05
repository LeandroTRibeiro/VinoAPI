using BetterThanYou.Core.DTOs.Route;

namespace BetterThanYou.Web.Endpoints.Routes;

public class RouteListResponse
{
    public List<RouteDto> Routes { get; set; } = new List<RouteDto>();
}