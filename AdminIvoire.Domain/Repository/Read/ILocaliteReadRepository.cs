using AdminIvoire.Domain.Entite;

namespace AdminIvoire.Domain.Repository.Read;

/// <summary>
/// Interface générique pour les opérations de lecture sur les localités
/// </summary>
/// <typeparam name="T">Le type de localité (doit hériter de Localite)</typeparam>
public interface ILocaliteReadRepository<T> where T : Localite
{
    /// <summary>
    /// Récupère toutes les localités de type T
    /// </summary>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    /// <returns>La liste de toutes les localités</returns>
    Task<IList<T>> GetAllAsync(CancellationToken cancellationToken);
    /// <summary>
    /// Récupère une localité par son identifiant
    /// </summary>
    /// <param name="id">L'identifiant unique de la localité</param>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    /// <returns>La localité correspondante</returns>
    Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// Récupère une localité par son nom
    /// </summary>
    /// <param name="nom">Le nom de la localité</param>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    /// <returns>La localité correspondante, ou null si non trouvée</returns>
    Task<T?> GetByNomAsync(string nom, CancellationToken cancellationToken);
    /// <summary>
    /// Récupère la liste de tous les noms de localités de type T
    /// </summary>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    /// <returns>La liste des noms de localités</returns>
    Task<IList<string>> GetAllNomsAsync(CancellationToken cancellationToken);
}
