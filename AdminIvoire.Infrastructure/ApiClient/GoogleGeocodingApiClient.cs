using AdminIvoire.Application.ApiClient;
using AdminIvoire.Domain.ValueObject;
using AdminIvoire.Infrastructure.ApiClient.Model;
using AdminIvoire.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace AdminIvoire.Infrastructure.ApiClient;

/// <summary>
/// Client pour interagir avec l'API Google Geocoding pour récupérer les coordonnées géographiques
/// </summary>
public class GoogleGeocodingApiClient(IConfiguration configuration, HttpClient httpClient, ILogger<GoogleGeocodingApiClient> logger) : IGeocodingApiClient
{
    const string BaseUrlKey = "GoogleMaps:BaseUrl";
    const string ApiKeyKey = "GoogleMaps:ApiKey";
    private readonly IConfiguration _configuration = configuration;
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<GoogleGeocodingApiClient> _logger = logger;

    /// <summary>
    /// Récupère les coordonnées géographiques (latitude, longitude) d'une localité via l'API Google Geocoding
    /// </summary>
    /// <param name="localite">Le nom de la localité pour laquelle récupérer les coordonnées</param>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    /// <returns>Les coordonnées géographiques de la localité</returns>
    /// <exception cref="ApiCallException">Lancée lorsqu'une erreur survient lors de l'appel à l'API</exception>
    public async Task<CoordonneesGeographiques> GetCoordonneesGeographiquesAsync(string localite, CancellationToken cancellationToken)
    {
        string url = FormatGeocodingRequestUrl(localite);
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
            _logger.LogInformation("Coordonnées géographiques de {Localite} : {Latitude}, {Longitude}", localite, localisation.Lat, localisation.Lng);
            return new CoordonneesGeographiques
            {
                Latitude = Convert.ToDecimal(localisation.Lat),
                Longitude = Convert.ToDecimal(localisation.Lng)
            };
        }
        catch (Exception ex)
        {
            throw new ApiCallException(ex);
        }
    }

    /// <summary>
    /// Formate l'URL de la requête de géocodage pour l'API Google Maps
    /// </summary>
    /// <param name="localite">Le nom de la localité</param>
    /// <returns>L'URL formatée avec les paramètres nécessaires</returns>
    /// <exception cref="ConfigurationException">Lancée si la clé API ou l'URL de base ne sont pas configurées</exception>
    private string FormatGeocodingRequestUrl(string localite)
    {
        var localitePrecise = $"{localite}, Côte d'Ivoire";
        var apiKey = _configuration[ApiKeyKey] ?? throw new ConfigurationException("Aucune valeur de configuration de clé n'a été définie pour l'api GoogleMaps");
        var baseUrl = _configuration[BaseUrlKey] ?? throw new ConfigurationException("Aucune valeur de configuration d'url n'a été définie pour l'api GoogleMaps");
        _logger.LogInformation("Récupération des coordonnées géographiques de {Localite}", localite);
        _logger.LogDebug("Url: {Url}", baseUrl);
        var url = $"{baseUrl}?address={Uri.EscapeDataString(localitePrecise)}&key={apiKey}";
        _logger.LogDebug("Url formatée: {Url}", url);
        return url;
    }
}
