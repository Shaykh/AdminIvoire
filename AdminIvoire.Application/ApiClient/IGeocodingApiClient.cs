using AdminIvoire.Domain.ValueObject;

namespace AdminIvoire.Application.ApiClient;

/// <summary>
/// Interface pour récupérer les coordonnées géographiques d'une localité via un service de géocodage
/// </summary>
public interface IGeocodingApiClient
{
    /// <summary>
    /// Récupère les coordonnées géographiques (latitude, longitude) d'une localité
    /// </summary>
    /// <param name="localite">Le nom de la localité pour laquelle récupérer les coordonnées</param>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    /// <returns>Les coordonnées géographiques de la localité</returns>
    Task<CoordonneesGeographiques> GetCoordonneesGeographiquesAsync(string localite, CancellationToken cancellationToken);
}
