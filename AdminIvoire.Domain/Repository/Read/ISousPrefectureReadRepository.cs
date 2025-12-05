using AdminIvoire.Domain.Entite;

namespace AdminIvoire.Domain.Repository.Read;

/// <summary>
/// Interface pour les opérations de lecture sur les sous-préfectures
/// </summary>
public interface ISousPrefectureReadRepository : ILocaliteReadRepository<SousPrefecture>
{
    /// <summary>
    /// Récupère toutes les sous-préfectures appartenant à un département
    /// </summary>
    /// <param name="departementId">L'identifiant du département</param>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    /// <returns>La liste des sous-préfectures du département</returns>
    Task<IList<SousPrefecture>> GetAllByDepartementIdAsync(Guid departementId, CancellationToken cancellationToken);
}
