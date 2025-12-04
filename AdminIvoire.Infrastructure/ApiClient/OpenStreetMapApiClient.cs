using AdminIvoire.Application.ApiClient;
using AdminIvoire.Infrastructure.ApiClient.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text;

namespace AdminIvoire.Infrastructure.ApiClient;

/// <summary>
/// Client pour interagir avec l'API OpenStreetMap via Overpass
/// </summary>
public class OpenStreetMapApiClient(
    IConfiguration configuration,
    HttpClient httpClient,
    ILogger<OpenStreetMapApiClient> logger) : IOpenStreetMapApiClient
{
    private const string OverpassApiUrlKey = "OpenStreetMap:OverpassApiUrl";
    private const string DefaultOverpassApiUrl = "https://overpass-api.de/api/interpreter";
    private readonly IConfiguration _configuration = configuration;
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<OpenStreetMapApiClient> _logger = logger;

    public async Task<IList<string>> GetVillagesAsync(string sousPrefectureNom, string? departementNom = null, string? regionNom = null, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Récupération des villages depuis OpenStreetMap pour la sous-préfecture {SousPrefectureNom}, département {DepartementNom}, région {RegionNom}",
            sousPrefectureNom, departementNom ?? "N/A", regionNom ?? "N/A");

        try
        {
            var overpassQuery = BuildOverpassQuery(sousPrefectureNom, departementNom, regionNom);
            var overpassApiUrl = _configuration[OverpassApiUrlKey] ?? DefaultOverpassApiUrl;

            _logger.LogDebug("Requête Overpass: {Query}", overpassQuery);
            _logger.LogDebug("URL Overpass API: {Url}", overpassApiUrl);

            var requestContent = new StringContent(overpassQuery, Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await _httpClient.PostAsync(overpassApiUrl, requestContent, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Erreur lors de la récupération des villages depuis OpenStreetMap pour {SousPrefectureNom}: {StatusCode}",
                    sousPrefectureNom, response.StatusCode);
                return [];
            }

            var overpassResponse = await response.Content.ReadFromJsonAsync<OverpassResponse>(cancellationToken);

            if (overpassResponse == null)
            {
                _logger.LogWarning("Réponse vide depuis OpenStreetMap pour {SousPrefectureNom}", sousPrefectureNom);
                return [];
            }

            var villages = ExtractVillageNames(overpassResponse);
            _logger.LogInformation("Récupération de {Count} villages depuis OpenStreetMap pour {SousPrefectureNom}", villages.Count, sousPrefectureNom);

            return villages;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la récupération des villages depuis OpenStreetMap pour {SousPrefectureNom}", sousPrefectureNom);
            return [];
        }
    }

    internal static string BuildOverpassQuery(string sousPrefectureNom, string? departementNom, string? regionNom)
    {
        // Construction de la requête Overpass QL pour rechercher les villages
        // Bounding box approximative de la Côte d'Ivoire: (sud, ouest, nord, est)
        // Format Overpass: (minlat, minlon, maxlat, maxlon)
        const string coteIvoireBbox = "(4.3,-8.6,10.7,-2.5)";

        var queryBuilder = new StringBuilder();
        queryBuilder.AppendLine("[out:json][timeout:25];");
        queryBuilder.AppendLine("(");

        // Recherche des villages dans la Côte d'Ivoire avec filtres sur les tags administratifs
        var escapedSousPrefecture = EscapeOverpassString(sousPrefectureNom);

        // Recherche par tag addr:subdistrict (sous-préfecture)
        queryBuilder.AppendLine($"  node[\"place\"~\"^(village|hamlet|town)$\"][\"addr:subdistrict\"~\"^{escapedSousPrefecture}$\",i]{coteIvoireBbox};");
        queryBuilder.AppendLine($"  way[\"place\"~\"^(village|hamlet|town)$\"][\"addr:subdistrict\"~\"^{escapedSousPrefecture}$\",i]{coteIvoireBbox};");

        // Recherche alternative par tag is_in (qui peut contenir le nom de la sous-préfecture)
        queryBuilder.AppendLine($"  node[\"place\"~\"^(village|hamlet|town)$\"][\"is_in\"~\"{escapedSousPrefecture}\",i]{coteIvoireBbox};");
        queryBuilder.AppendLine($"  way[\"place\"~\"^(village|hamlet|town)$\"][\"is_in\"~\"{escapedSousPrefecture}\",i]{coteIvoireBbox};");

        // Si département est fourni, recherche supplémentaire
        if (!string.IsNullOrWhiteSpace(departementNom))
        {
            var escapedDepartement = EscapeOverpassString(departementNom);
            queryBuilder.AppendLine($"  node[\"place\"~\"^(village|hamlet|town)$\"][\"addr:district\"~\"^{escapedDepartement}$\",i][\"addr:subdistrict\"~\"{escapedSousPrefecture}\",i]{coteIvoireBbox};");
            queryBuilder.AppendLine($"  way[\"place\"~\"^(village|hamlet|town)$\"][\"addr:district\"~\"^{escapedDepartement}$\",i][\"addr:subdistrict\"~\"{escapedSousPrefecture}\",i]{coteIvoireBbox};");
        }

        queryBuilder.AppendLine(");");
        queryBuilder.AppendLine("out body;");
        queryBuilder.AppendLine(">;");
        queryBuilder.AppendLine("out skel qt;");

        return queryBuilder.ToString();
    }

    private static string EscapeOverpassString(string input)
    {
        // Échapper les caractères spéciaux pour Overpass QL
        return input.Replace("\\", "\\\\")
                    .Replace("\"", "\\\"")
                    .Replace("\n", "\\n")
                    .Replace("\r", "\\r")
                    .Replace("\t", "\\t");
    }

    private static List<string> ExtractVillageNames(OverpassResponse response)
    {
        var villageNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var element in response.Elements)
        {
            if (element.Tags == null || !element.Tags.Any())
                continue;

            // Extraire le nom du village depuis les tags
            if (element.Tags.TryGetValue("name", out var name) && !string.IsNullOrWhiteSpace(name))
            {
                // Vérifier que c'est bien un village (place=village ou place=hamlet)
                var place = element.Tags.GetValueOrDefault("place", "");
                if (place == "village" || place == "hamlet" || place == "town")
                {
                    villageNames.Add(name.Trim());
                }
            }
        }

        return [.. villageNames.OrderBy(v => v)];
    }
}

