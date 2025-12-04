namespace AdminIvoire.Application.Services;

/// <summary>
/// Interface pour récupérer les villages d'une sous-préfecture depuis une source web
/// </summary>
public interface IVillageWebSourceService
{
    /// <summary>
    /// Récupère la liste des noms de villages pour une sous-préfecture donnée
    /// </summary>
    /// <param name="sousPrefectureNom">Le nom de la sous-préfecture</param>
    /// <param name="departementNom">Le nom du département (optionnel, pour améliorer la précision)</param>
    /// <param name="regionNom">Le nom de la région (optionnel, pour améliorer la précision)</param>
    /// <param name="cancellationToken">Token d'annulation</param>
    /// <returns>Liste des noms de villages</returns>
    Task<IList<string>> GetVillagesAsync(string sousPrefectureNom, string? departementNom = null, string? regionNom = null, CancellationToken cancellationToken = default);
}

