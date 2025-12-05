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
    /// <summary>
    /// Récupère la liste des noms de villages pour une sous-préfecture donnée depuis OpenStreetMap
    /// </summary>
    /// <param name="sousPrefectureNom">Le nom de la sous-préfecture</param>
    /// <param name="departementNom">Le nom du département (optionnel, pour améliorer la précision)</param>
    /// <param name="regionNom">Le nom de la région (optionnel, pour améliorer la précision)</param>
    /// <param name="cancellationToken">Token d'annulation</param>
    /// <returns>Liste des noms de villages, ou une liste vide en cas d'erreur</returns>
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

