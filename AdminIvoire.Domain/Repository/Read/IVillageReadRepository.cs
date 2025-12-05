using AdminIvoire.Domain.Entite;

namespace AdminIvoire.Domain.Repository.Read;

/// <summary>
/// Interface pour les opérations de lecture sur les villages
/// </summary>
public interface IVillageReadRepository : ILocaliteReadRepository<Village>
{
    /// <summary>
    /// Récupère tous les villages appartenant à une sous-préfecture
    /// </summary>
    /// <param name="sousPrefectureId">L'identifiant de la sous-préfecture</param>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    /// <returns>La liste des villages de la sous-préfecture</returns>
    Task<IList<Village>> GetAllBySousPrefectureIdAsync(Guid sousPrefectureId, CancellationToken cancellationToken);
}
