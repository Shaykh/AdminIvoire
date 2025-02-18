using AdminIvoire.Application.ApiClient;
using AdminIvoire.Domain.ValueObject;
using AdminIvoire.Infrastructure.ApiClient.Model;
using AdminIvoire.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace AdminIvoire.Infrastructure.ApiClient;

public class GoogleGeocodingApiClient(IConfiguration configuration, HttpClient httpClient, ILogger<GoogleGeocodingApiClient> logger) : IGeocodingApi
{
    const string BaseUrlKey = "GoogleMaps:BaseUrl";
    const string ApiKeyKey = "GoogleMaps:ApiKey";
    private readonly string _apiKey = configuration[ApiKeyKey] ?? throw new ConfigurationException("Aucune valeur de configuration de clé n'a été définie pour l'api GoogleMaps");
    private readonly string _baseUrl = configuration[BaseUrlKey] ?? throw new ConfigurationException("Aucune valeur de configuration d'url n'a été définie pour l'api GoogleMaps");
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<GoogleGeocodingApiClient> _logger = logger;

    public async Task<CoordonneesGeographiques> GetCoordonneesGeographiquesAsync(string localite, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Récupération des coordonnées géographiques de {Localite}", localite);
        _logger.LogDebug("Url: {Url}", _baseUrl);
        var url = $"{_baseUrl}?address={Uri.EscapeDataString(localite)}&key={_apiKey}";
        try
        {
            var response = await _httpClient.GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Erreur lors de la récupération des coordonnées géographiques de {localite} : L'api a repondu avec le code {response.StatusCode} et le contenu {response.Content}");
            }
            _logger.LogDebug("Réponse: {Response}", response);
            var content = await response.Content.ReadFromJsonAsync<GoogleCoordinateResponse>(cancellationToken) ?? 
                throw new HttpRequestException($"Erreur lors de la récupération des coordonnées géographiques de {localite} : Le contenu de la réponse est vide");
            if (content.Status != "OK")
            {
                throw new HttpRequestException($"Erreur lors de la récupération des coordonnées géographiques de {localite} : Le statut de la réponse est {content.Status}");
            }
            if (content.Results.Count == 0)
            {
                throw new HttpRequestException($"Erreur lors de la récupération des coordonnées géographiques de {localite} : Aucun résultat n'a été trouvé");
            }
            var localisation = (content.Results[0].Geometry?.Location) ?? 
                throw new HttpRequestException($"Erreur lors de la récupération des coordonnées géographiques de {localite} : Aucune localisation n'a été trouvée");
            _logger.LogInformation("Coordonnées géographiques de {Localite} : {Latitude}, {Longitude}", localite, localisation.Latitude, localisation.Longitude);
            return new CoordonneesGeographiques
            {
                Latitude = Convert.ToDecimal(localisation.Latitude),
                Longitude = Convert.ToDecimal(localisation.Longitude)
            };
        }
        catch (Exception ex)
        {
            throw new ApiCallException(ex);
        }
    }
}
