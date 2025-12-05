using BetterThanYou.Core.Entities;
using BetterThanYou.Core.Interfaces.Route;
using BetterThanYou.Infrastructure.Data;
using BetterThanYou.SharedKernel.Enums;
using Microsoft.EntityFrameworkCore;

namespace BetterThanYou.Infrastructure.Repositories;

public class RouteRepository : IRouteRepository
{
    private readonly AppDbContext _db;

    public RouteRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Route> CreateAsync(Route route)
    {
        await _db.Routes.AddAsync(route);
        await _db.SaveChangesAsync();
        return route;
    }

    public async Task<List<Route>> GetAllAsync()
    {
        return await _db.Routes
            .Include(r => r.Paradas)
                .ThenInclude(p => p.Cliente)
            .Where(r => r.Ativo)
            .OrderByDescending(r => r.DataRota)
            .ToListAsync();
    }

    public async Task<Route?> GetByIdAsync(Guid id)
    {
        return await _db.Routes
            .Include(r => r.Paradas)
                .ThenInclude(p => p.Cliente)
            .FirstOrDefaultAsync(r => r.Id == id && r.Ativo);
    }

public async Task<Route> UpdateAsync(Route route)
{
    
    try
    {
        // 1. LIMPA TODO O CHANGETRACKER
        _db.ChangeTracker.Clear();
        
        // 2. Remove paradas antigas diretamente do banco
        var deletedCount = await _db.RouteStops
            .Where(i => i.RouteId == route.Id)
            .ExecuteDeleteAsync();
        
        // 3. Busca a rota do banco (sem tracking)
        var routeFromDb = await _db.Routes
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == route.Id);
        
        if (routeFromDb == null)
        {
            throw new KeyNotFoundException("Rota não encontrada");
        }
        
        
        // 4. Copia todos os dados
        routeFromDb.Nome = route.Nome;
        routeFromDb.Descricao = route.Descricao;
        routeFromDb.DataRota = route.DataRota;
        routeFromDb.Status = route.Status;
        routeFromDb.EnderecoPartida = route.EnderecoPartida;
        routeFromDb.CidadePartida = route.CidadePartida;
        routeFromDb.EstadoPartida = route.EstadoPartida;
        routeFromDb.CepPartida = route.CepPartida;
        routeFromDb.LatitudePartida = route.LatitudePartida;
        routeFromDb.LongitudePartida = route.LongitudePartida;
        routeFromDb.DistanciaTotal = route.DistanciaTotal;
        routeFromDb.TempoEstimado = route.TempoEstimado;
        routeFromDb.Otimizada = route.Otimizada;
        routeFromDb.DataOtimizacao = route.DataOtimizacao;
        routeFromDb.ModificadoPor = route.ModificadoPor;
        routeFromDb.DataModificacao = route.DataModificacao;
        
        // 5. Marca a rota como modificada
        _db.Routes.Update(routeFromDb);
        
        // 6. Adiciona novas paradas (sem navigation property Route!)
        int count = 0;
        foreach (var parada in route.Paradas)
        {
            count++;
            // Cria uma nova RouteStop SEM a navigation property Route
            var novaParada = new RouteStop
            {
                Id = parada.Id,
                RouteId = routeFromDb.Id,
                ClienteId = parada.ClienteId,
                OrdemOriginal = parada.OrdemOriginal,
                OrdemOtimizada = parada.OrdemOtimizada,
                Latitude = parada.Latitude,
                Longitude = parada.Longitude,
                DistanciaParadaAnterior = parada.DistanciaParadaAnterior,
                TempoParadaAnterior = parada.TempoParadaAnterior,
                HorarioChegadaPrevisto = parada.HorarioChegadaPrevisto,
                HorarioChegadaReal = parada.HorarioChegadaReal,
                HorarioSaidaReal = parada.HorarioSaidaReal,
                Observacoes = parada.Observacoes,
                Visitado = parada.Visitado
            };
            
            await _db.RouteStops.AddAsync(novaParada);
        }
        
        // 7. Salva tudo
        var changes = await _db.SaveChangesAsync();
        
        // 8. Recarrega paradas
        await _db.Entry(routeFromDb)
            .Collection(r => r.Paradas)
            .Query()
            .Include(p => p.Cliente)
            .LoadAsync();
        
        return routeFromDb;
    }
    catch (Exception ex)
    {
        if (ex.InnerException != null)
        {
        }
        throw;
    }
}

    public async Task DeleteAsync(Route route)
    {
        route.Ativo = false;
        _db.Routes.Update(route);
        await _db.SaveChangesAsync();
    }

    public async Task<List<Route>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _db.Routes
            .Include(r => r.Paradas)
                .ThenInclude(p => p.Cliente)
            .Where(r => r.Ativo && r.DataRota >= startDate && r.DataRota <= endDate)
            .OrderByDescending(r => r.DataRota)
            .ToListAsync();
    }

    public async Task<List<Route>> GetByStatusAsync(RouteStatus status)
    {
        return await _db.Routes
            .Include(r => r.Paradas)
                .ThenInclude(p => p.Cliente)
            .Where(r => r.Ativo && r.Status == status)
            .OrderByDescending(r => r.DataRota)
            .ToListAsync();
    }
}