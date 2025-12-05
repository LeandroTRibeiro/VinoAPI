using BetterThanYou.Core.DTOs.Route;
using BetterThanYou.Core.Entities;
using BetterThanYou.Core.Interfaces.Client;
using BetterThanYou.Core.Interfaces.Route;
using BetterThanYou.SharedKernel.Enums;

namespace BetterThanYou.Core.Services.Route;

public class RouteService : IRouteService
{
    private readonly IRouteRepository _routeRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IGeocodingService _geocodingService;

    public RouteService(
        IRouteRepository routeRepository,
        IClientRepository clientRepository,
        IGeocodingService geocodingService)
    {
        _routeRepository = routeRepository;
        _clientRepository = clientRepository;
        _geocodingService = geocodingService;
    }

    public async Task<RouteDto> CreateAsync(RouteCreateDto dto, string criadoPor)
    {
        if (string.IsNullOrWhiteSpace(dto.Nome))
            throw new ArgumentException("Nome da rota é obrigatório", nameof(dto.Nome));

        if (dto.ClientesIds == null || !dto.ClientesIds.Any())
            throw new ArgumentException("A rota deve ter pelo menos um cliente", nameof(dto.ClientesIds));

        var route = new Entities.Route
        {
            Id = Guid.NewGuid(),
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            DataRota = dto.DataRota,
            Status = RouteStatus.Planejada,
            EnderecoPartida = dto.EnderecoPartida,
            CidadePartida = dto.CidadePartida,
            EstadoPartida = dto.EstadoPartida,
            CepPartida = dto.CepPartida,
            CriadoPor = criadoPor,
            DataCriacao = DateTime.UtcNow,
            Ativo = true,
            Paradas = new List<RouteStop>()
        };

        // Geocodificar ponto de partida se fornecido
        if (!string.IsNullOrWhiteSpace(dto.EnderecoPartida))
        {
            var coordsPartida = await _geocodingService.GetCoordinatesAsync(
                dto.EnderecoPartida, dto.CidadePartida, dto.EstadoPartida, dto.CepPartida);
            
            if (coordsPartida.HasValue)
            {
                route.LatitudePartida = coordsPartida.Value.latitude;
                route.LongitudePartida = coordsPartida.Value.longitude;
            }
        }

        // Criar paradas para cada cliente
        int ordem = 1;
        foreach (var clienteId in dto.ClientesIds)
        {
            var cliente = await _clientRepository.GetByIdAsync(clienteId);
            if (cliente == null)
                throw new KeyNotFoundException($"Cliente {clienteId} não encontrado");

            // Geocodificar endereço do cliente
            (double latitude, double longitude)? coords = null;
            if (!string.IsNullOrWhiteSpace(cliente.Endereco))
            {
                coords = await _geocodingService.GetCoordinatesAsync(
                    cliente.Endereco, cliente.Cidade, cliente.Estado, cliente.Cep);
            }

            var parada = new RouteStop
            {
                Id = Guid.NewGuid(),
                RouteId = route.Id,
                ClienteId = clienteId,
                OrdemOriginal = ordem,
                OrdemOtimizada = ordem,
                Latitude = coords?.latitude,
                Longitude = coords?.longitude,
                Visitado = false
            };

            route.Paradas.Add(parada);
            ordem++;
        }

        var created = await _routeRepository.CreateAsync(route);
        return await MapToDto(created);
    }

    public async Task<List<RouteDto>> GetAllAsync()
    {
        var routes = await _routeRepository.GetAllAsync();
        var dtos = new List<RouteDto>();

        foreach (var route in routes)
        {
            dtos.Add(await MapToDto(route));
        }

        return dtos;
    }

    public async Task<RouteDto> GetByIdAsync(Guid id)
    {
        var route = await _routeRepository.GetByIdAsync(id);

        if (route == null)
            throw new KeyNotFoundException("Rota não encontrada");

        return await MapToDto(route);
    }

public async Task<RouteDto> UpdateAsync(Guid id, RouteUpdateDto dto, string modificadoPor)
{
    
    var route = await _routeRepository.GetByIdAsync(id);

    if (route == null)
    {
        throw new KeyNotFoundException("Rota não encontrada");
    }

    if (string.IsNullOrWhiteSpace(dto.Nome))
    {
        throw new ArgumentException("Nome da rota é obrigatório", nameof(dto.Nome));
    }

    if (dto.ClientesIds == null || !dto.ClientesIds.Any())
    {
        throw new ArgumentException("A rota deve ter pelo menos um cliente", nameof(dto.ClientesIds));
    }

    route.Nome = dto.Nome;
    route.Descricao = dto.Descricao;
    route.DataRota = dto.DataRota;
    route.Status = dto.Status;
    route.EnderecoPartida = dto.EnderecoPartida;
    route.CidadePartida = dto.CidadePartida;
    route.EstadoPartida = dto.EstadoPartida;
    route.CepPartida = dto.CepPartida;
    route.ModificadoPor = modificadoPor;
    route.DataModificacao = DateTime.UtcNow;

    // Geocodificar novo ponto de partida se alterado
    if (!string.IsNullOrWhiteSpace(dto.EnderecoPartida))
    {
        var coordsPartida = await _geocodingService.GetCoordinatesAsync(
            dto.EnderecoPartida, dto.CidadePartida, dto.EstadoPartida, dto.CepPartida);
        
        if (coordsPartida.HasValue)
        {
            route.LatitudePartida = coordsPartida.Value.latitude;
            route.LongitudePartida = coordsPartida.Value.longitude;
        }
    }

    // Atualizar paradas
    route.Paradas.Clear();
    route.Otimizada = false;
    route.DataOtimizacao = null;

    int ordem = 1;
    foreach (var clienteId in dto.ClientesIds)
    {
        
        var cliente = await _clientRepository.GetByIdAsync(clienteId);
        if (cliente == null)
        {
            throw new KeyNotFoundException($"Cliente {clienteId} não encontrado");
        }
        

        (double latitude, double longitude)? coords = null;
        if (!string.IsNullOrWhiteSpace(cliente.Endereco))
        {
            coords = await _geocodingService.GetCoordinatesAsync(
                cliente.Endereco, cliente.Cidade, cliente.Estado, cliente.Cep);
            
            if (coords.HasValue)
            {
            }
        }

        var parada = new RouteStop
        {
            Id = Guid.NewGuid(),
            RouteId = route.Id,
            ClienteId = clienteId,
            OrdemOriginal = ordem,
            OrdemOtimizada = ordem,
            Latitude = coords?.latitude,
            Longitude = coords?.longitude,
            Visitado = false
        };

        route.Paradas.Add(parada);
        ordem++;
    }

    
    var updated = await _routeRepository.UpdateAsync(route);
    
    
    var result = await MapToDto(updated);
    
    return result;
}

    public async Task DeleteAsync(Guid id)
    {
        var route = await _routeRepository.GetByIdAsync(id);

        if (route == null)
            throw new KeyNotFoundException("Rota não encontrada");

        await _routeRepository.DeleteAsync(route);
    }

    public async Task<RouteOptimizationResultDto> OptimizeRouteAsync(Guid id)
    {
        var route = await _routeRepository.GetByIdAsync(id);

        if (route == null)
            throw new KeyNotFoundException("Rota não encontrada");

        if (!route.Paradas.Any())
            throw new InvalidOperationException("Rota não possui paradas para otimizar");

        // Verificar se todas as paradas têm coordenadas
        var paradasSemCoordenadas = route.Paradas.Where(p => !p.Latitude.HasValue || !p.Longitude.HasValue).ToList();
        if (paradasSemCoordenadas.Any())
        {
            throw new InvalidOperationException(
                $"Algumas paradas não possuem coordenadas. Verifique os endereços dos clientes.");
        }

        // Algoritmo Nearest Neighbor (Vizinho Mais Próximo)
        var paradasNaoVisitadas = route.Paradas.ToList();
        var paradasOtimizadas = new List<RouteStop>();
        
        // Ponto inicial (partida ou primeira parada)
        double currentLat;
        double currentLon;
        
        if (route.LatitudePartida.HasValue && route.LongitudePartida.HasValue)
        {
            currentLat = route.LatitudePartida.Value;
            currentLon = route.LongitudePartida.Value;
        }
        else
        {
            // Se não há ponto de partida, começa pela primeira parada
            var primeira = paradasNaoVisitadas.First();
            currentLat = primeira.Latitude!.Value;
            currentLon = primeira.Longitude!.Value;
            paradasOtimizadas.Add(primeira);
            paradasNaoVisitadas.Remove(primeira);
        }

        double distanciaTotal = 0;
        int tempoTotal = 0;

        // Otimizar rota pelo vizinho mais próximo
        while (paradasNaoVisitadas.Any())
        {
            RouteStop? paradaMaisProxima = null;
            double menorDistancia = double.MaxValue;

            foreach (var parada in paradasNaoVisitadas)
            {
                var distancia = _geocodingService.CalculateDistance(
                    currentLat, currentLon,
                    parada.Latitude!.Value, parada.Longitude!.Value);

                if (distancia < menorDistancia)
                {
                    menorDistancia = distancia;
                    paradaMaisProxima = parada;
                }
            }

            if (paradaMaisProxima != null)
            {
                paradaMaisProxima.DistanciaParadaAnterior = menorDistancia;
                paradaMaisProxima.TempoParadaAnterior = _geocodingService.EstimateTimeInMinutes(menorDistancia);
                
                distanciaTotal += menorDistancia;
                tempoTotal += paradaMaisProxima.TempoParadaAnterior.Value;

                paradasOtimizadas.Add(paradaMaisProxima);
                paradasNaoVisitadas.Remove(paradaMaisProxima);

                currentLat = paradaMaisProxima.Latitude!.Value;
                currentLon = paradaMaisProxima.Longitude!.Value;
            }
        }

        // Atualizar ordem otimizada
        int novaOrdem = 1;
        foreach (var parada in paradasOtimizadas)
        {
            parada.OrdemOtimizada = novaOrdem++;
        }

        // Atualizar rota
        route.Paradas = paradasOtimizadas;
        route.DistanciaTotal = Math.Round(distanciaTotal, 2);
        route.TempoEstimado = tempoTotal;
        route.Otimizada = true;
        route.DataOtimizacao = DateTime.UtcNow;

        await _routeRepository.UpdateAsync(route);

        var paradasDto = new List<RouteStopDto>();
        foreach (var parada in paradasOtimizadas)
        {
            var cliente = await _clientRepository.GetByIdAsync(parada.ClienteId);
            paradasDto.Add(new RouteStopDto
            {
                Id = parada.Id,
                RouteId = parada.RouteId,
                ClienteId = parada.ClienteId,
                ClienteNome = cliente?.NomeFantasia ?? "",
                ClienteEndereco = cliente?.Endereco,
                ClienteCidade = cliente?.Cidade,
                ClienteEstado = cliente?.Estado,
                OrdemOriginal = parada.OrdemOriginal,
                OrdemOtimizada = parada.OrdemOtimizada,
                Latitude = parada.Latitude,
                Longitude = parada.Longitude,
                DistanciaParadaAnterior = parada.DistanciaParadaAnterior,
                TempoParadaAnterior = parada.TempoParadaAnterior,
                Visitado = parada.Visitado
            });
        }

        return new RouteOptimizationResultDto
        {
            RouteId = route.Id,
            DistanciaTotal = route.DistanciaTotal.Value,
            TempoEstimado = route.TempoEstimado.Value,
            ParadasOtimizadas = paradasDto,
            Mensagem = $"Rota otimizada com sucesso! Economia estimada de tempo e distância."
        };
    }

    public async Task<RouteDto> UpdateStopStatusAsync(Guid routeId, Guid stopId, 
        DateTime? horarioChegada, DateTime? horarioSaida, string? observacoes)
    {
        var route = await _routeRepository.GetByIdAsync(routeId);

        if (route == null)
            throw new KeyNotFoundException("Rota não encontrada");

        var parada = route.Paradas.FirstOrDefault(p => p.Id == stopId);
        
        if (parada == null)
            throw new KeyNotFoundException("Parada não encontrada");

        if (horarioChegada.HasValue)
        {
            parada.HorarioChegadaReal = horarioChegada.Value;
            parada.Visitado = true;
        }

        if (horarioSaida.HasValue)
            parada.HorarioSaidaReal = horarioSaida.Value;

        if (!string.IsNullOrWhiteSpace(observacoes))
            parada.Observacoes = observacoes;

        await _routeRepository.UpdateAsync(route);

        return await MapToDto(route);
    }

    public async Task<List<RouteDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var routes = await _routeRepository.GetByDateRangeAsync(startDate, endDate);
        var dtos = new List<RouteDto>();

        foreach (var route in routes)
        {
            dtos.Add(await MapToDto(route));
        }

        return dtos;
    }

    public async Task<List<RouteDto>> GetByStatusAsync(RouteStatus status)
    {
        var routes = await _routeRepository.GetByStatusAsync(status);
        var dtos = new List<RouteDto>();

        foreach (var route in routes)
        {
            dtos.Add(await MapToDto(route));
        }

        return dtos;
    }

    private async Task<RouteDto> MapToDto(Entities.Route route)
    {
        var paradasDto = new List<RouteStopDto>();

        foreach (var parada in route.Paradas.OrderBy(p => p.OrdemOtimizada))
        {
            var cliente = await _clientRepository.GetByIdAsync(parada.ClienteId);
            
            paradasDto.Add(new RouteStopDto
            {
                Id = parada.Id,
                RouteId = parada.RouteId,
                ClienteId = parada.ClienteId,
                ClienteNome = cliente?.NomeFantasia ?? "",
                ClienteEndereco = cliente?.Endereco,
                ClienteCidade = cliente?.Cidade,
                ClienteEstado = cliente?.Estado,
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
            });
        }

        return new RouteDto
        {
            Id = route.Id,
            Nome = route.Nome,
            Descricao = route.Descricao,
            DataRota = route.DataRota,
            Status = route.Status,
            EnderecoPartida = route.EnderecoPartida,
            CidadePartida = route.CidadePartida,
            EstadoPartida = route.EstadoPartida,
            CepPartida = route.CepPartida,
            LatitudePartida = route.LatitudePartida,
            LongitudePartida = route.LongitudePartida,
            DistanciaTotal = route.DistanciaTotal,
            TempoEstimado = route.TempoEstimado,
            Otimizada = route.Otimizada,
            DataOtimizacao = route.DataOtimizacao,
            CriadoPor = route.CriadoPor,
            DataCriacao = route.DataCriacao,
            ModificadoPor = route.ModificadoPor,
            DataModificacao = route.DataModificacao,
            Ativo = route.Ativo,
            Paradas = paradasDto
        };
    }
}