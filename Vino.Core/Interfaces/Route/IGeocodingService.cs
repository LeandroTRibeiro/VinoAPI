namespace BetterThanYou.Core.Interfaces.Route;

public interface IGeocodingService
{
    /// <summary>
    /// Obtém coordenadas (latitude, longitude) de um endereço
    /// </summary>
    Task<(double latitude, double longitude)?> GetCoordinatesAsync(string endereco, string? cidade, string? estado, string? cep);
    
    /// <summary>
    /// Calcula a distância entre dois pontos em quilômetros
    /// </summary>
    double CalculateDistance(double lat1, double lon1, double lat2, double lon2);
    
    /// <summary>
    /// Estima o tempo de viagem em minutos baseado na distância
    /// Velocidade média padrão: 40 km/h (considerando trânsito urbano)
    /// </summary>
    int EstimateTimeInMinutes(double distanceInKm, double averageSpeedKmh = 40);
}