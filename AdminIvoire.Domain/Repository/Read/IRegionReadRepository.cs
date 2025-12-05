using AdminIvoire.Domain.Entite;

namespace AdminIvoire.Domain.Repository.Read;

/// <summary>
/// Interface pour les opérations de lecture sur les régions
/// </summary>
public interface IRegionReadRepository : ILocaliteReadRepository<Region>
{
    /// <summary>
    /// Récupère toutes les régions appartenant à un district
    /// </summary>
    /// <param name="districtId">L'identifiant du district</param>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    /// <returns>La liste des régions du district</returns>
    Task<IList<Region>> GetAllByDistrictIdAsync(Guid districtId, CancellationToken cancellationToken);
}
