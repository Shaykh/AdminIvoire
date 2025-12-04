using Microsoft.Extensions.Logging;

namespace AdminIvoire.Application.Services;

/// <summary>
/// Service pour récupérer les villages depuis une source web
/// Cette implémentation peut être étendue pour utiliser différentes sources (APIs, scraping, etc.)
/// </summary>
public class VillageWebSourceService(ILogger<VillageWebSourceService> logger) : IVillageWebSourceService
{
    public async Task<IList<string>> GetVillagesAsync(string sousPrefectureNom, string? departementNom = null, string? regionNom = null, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Récupération des villages pour la sous-préfecture {SousPrefectureNom}", sousPrefectureNom);

        // TODO: Implémenter la récupération depuis une source web valide
        // Exemples de sources possibles :
        // - API gouvernementale de la Côte d'Ivoire
        // - Base de données OpenStreetMap
        // - API Geonames
        // - Scraping de sites web officiels

        // Pour l'instant, retourner une liste vide
        // Cette méthode doit être implémentée avec une source web réelle
        logger.LogWarning("La récupération des villages depuis le web n'est pas encore implémentée pour {SousPrefectureNom}", sousPrefectureNom);

        await Task.CompletedTask;
        return [];
    }
}

