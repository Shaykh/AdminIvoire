using AdminIvoire.Application.ApiClient;
using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Services;

/// <summary>
/// Service pour récupérer les villages depuis OpenStreetMap via l'API client
/// </summary>
public sealed class VillageWebSourceService(
    IOpenStreetMapApiClient openStreetMapApiClient,
    ILogger<VillageWebSourceService> logger) : IVillageWebSourceService
{
    public async Task<IList<string>> GetVillagesAsync(string sousPrefectureNom, string? departementNom = null, string? regionNom = null, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Récupération des villages pour la sous-préfecture {SousPrefectureNom}", sousPrefectureNom);

        try
        {
            var villages = await openStreetMapApiClient.GetVillagesAsync(sousPrefectureNom, departementNom, regionNom, cancellationToken);
            logger.LogInformation("Récupération de {Count} villages pour {SousPrefectureNom}", villages.Count, sousPrefectureNom);
            return villages;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erreur lors de la récupération des villages pour {SousPrefectureNom}", sousPrefectureNom);
            return [];
        }
    }
}

