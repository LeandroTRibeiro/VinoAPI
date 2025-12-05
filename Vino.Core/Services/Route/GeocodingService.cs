using BetterThanYou.Core.Interfaces.Route;

namespace BetterThanYou.Core.Services.Route;

/// <summary>
/// Serviço básico de geocodificação
/// NOTA: Esta implementação usa coordenadas simuladas para demonstração.
/// Em produção, você deve integrar com uma API real como Google Maps, OpenStreetMap, etc.
/// </summary>
public class GeocodingService : IGeocodingService
{
    public async Task<(double latitude, double longitude)?> GetCoordinatesAsync(
        string endereco, string? cidade, string? estado, string? cep)
    {
        // IMPORTANTE: Esta é uma implementação SIMULADA para demonstração
        // Em produção, você deve usar uma API real de geocodificação
        
        // Exemplo de como integrar com Google Maps Geocoding API:
        // var apiKey = _configuration["GoogleMaps:ApiKey"];
        // var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(fullAddress)}&key={apiKey}";
        // var response = await _httpClient.GetAsync(url);
        // ... processar resposta
        
        await Task.Delay(10); // Simula chamada assíncrona
        
        // Para demonstração, gera coordenadas baseadas no CEP ou endereço
        // Você pode substituir isso por coordenadas reais dos seus clientes
        var hash = GetSimpleHash(endereco, cidade, cep);
        
        // Coordenadas aproximadas do Brasil (centralizado)
        var baseLat = -15.0 + (hash % 20) - 10; // Variação de -10 a +10 graus
        var baseLon = -50.0 + (hash % 20) - 10; // Variação de -10 a +10 graus
        
        return (baseLat, baseLon);
    }
    
    public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        // Fórmula de Haversine para calcular distância entre dois pontos
        const double earthRadiusKm = 6371;
        
        var dLat = DegreesToRadians(lat2 - lat1);
        var dLon = DegreesToRadians(lon2 - lon1);
        
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        
        return earthRadiusKm * c;
    }
    
    public int EstimateTimeInMinutes(double distanceInKm, double averageSpeedKmh = 40)
    {
        // Tempo = Distância / Velocidade (em horas), depois converte para minutos
        var timeInHours = distanceInKm / averageSpeedKmh;
        return (int)Math.Ceiling(timeInHours * 60);
    }
    
    private double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }
    
    private int GetSimpleHash(string? endereco, string? cidade, string? cep)
    {
        var combined = $"{endereco}{cidade}{cep}".ToLower();
        return Math.Abs(combined.GetHashCode());
    }
}