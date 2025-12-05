using BetterThanYou.Core.DTOs.Route;
using BetterThanYou.SharedKernel.Enums;

namespace BetterThanYou.Core.Interfaces.Route;

public interface IRouteService
{
    Task<RouteDto> CreateAsync(RouteCreateDto dto, string criadoPor);
    Task<List<RouteDto>> GetAllAsync();
    Task<RouteDto> GetByIdAsync(Guid id);
    Task<RouteDto> UpdateAsync(Guid id, RouteUpdateDto dto, string modificadoPor);
    Task DeleteAsync(Guid id);
    Task<RouteOptimizationResultDto> OptimizeRouteAsync(Guid id);
    Task<RouteDto> UpdateStopStatusAsync(Guid routeId, Guid stopId, DateTime? horarioChegada, DateTime? horarioSaida, string? observacoes);
    Task<List<RouteDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<List<RouteDto>> GetByStatusAsync(RouteStatus status);
}