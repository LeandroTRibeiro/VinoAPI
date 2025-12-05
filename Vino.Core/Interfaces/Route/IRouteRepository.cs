using BetterThanYou.SharedKernel.Enums;

namespace BetterThanYou.Core.Interfaces.Route;

public interface IRouteRepository
{
    Task<Entities.Route> CreateAsync(Entities.Route route);
    Task<List<Entities.Route>> GetAllAsync();
    Task<Entities.Route?> GetByIdAsync(Guid id);
    Task<Entities.Route> UpdateAsync(Entities.Route route);
    Task DeleteAsync(Entities.Route route);
    Task<List<Entities.Route>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<List<Entities.Route>> GetByStatusAsync(RouteStatus status);
}