namespace AdminIvoire.Application.ApiClient;

/// <summary>
/// Interface pour interagir avec l'API OpenStreetMap via Overpass
/// </summary>
public interface IOpenStreetMapApiClient
{
    /// <summary>
    /// Récupère les villages d'une sous-préfecture depuis OpenStreetMap
    /// </summary>
    /// <param name="sousPrefectureNom">Nom de la sous-préfecture</param>
    /// <param name="departementNom">Nom du département (optionnel)</param>
    /// <param name="regionNom">Nom de la région (optionnel)</param>
    /// <param name="cancellationToken">Token d'annulation</param>
    /// <returns>Liste des noms de villages</returns>
    Task<IList<string>> GetVillagesAsync(string sousPrefectureNom, string? departementNom = null, string? regionNom = null, CancellationToken cancellationToken = default);
}

