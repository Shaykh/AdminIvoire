using AdminIvoire.Domain.Entite;

namespace AdminIvoire.Domain.Repository.Read;

/// <summary>
/// Interface pour les opérations de lecture sur les départements
/// </summary>
public interface IDepartementReadRepository : ILocaliteReadRepository<Departement>
{
    /// <summary>
    /// Récupère tous les départements appartenant à une région
    /// </summary>
    /// <param name="regionId">L'identifiant de la région</param>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    /// <returns>La liste des départements de la région</returns>
    Task<IList<Departement>> GetAllByRegionIdAsync(Guid regionId, CancellationToken cancellationToken);
}
