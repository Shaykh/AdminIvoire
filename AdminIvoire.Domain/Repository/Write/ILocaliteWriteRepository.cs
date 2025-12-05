using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.ValueObject;

namespace AdminIvoire.Domain.Repository.Write;

/// <summary>
/// Interface générique pour les opérations d'écriture sur les localités
/// </summary>
/// <typeparam name="T">Le type de localité (doit hériter de Localite)</typeparam>
public interface ILocaliteWriteRepository<T> where T : Localite
{
    /// <summary>
    /// Ajoute une nouvelle localité en base de données
    /// </summary>
    /// <param name="localite">La localité à ajouter</param>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    /// <returns>La localité ajoutée</returns>
    Task<T> AddAsync(T localite, CancellationToken cancellationToken);
    /// <summary>
    /// Met à jour une localité existante en base de données
    /// </summary>
    /// <param name="localite">La localité à mettre à jour</param>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    Task UpdateAsync(T localite, CancellationToken cancellationToken);
    /// <summary>
    /// Met à jour les coordonnées géographiques d'une localité identifiée par son nom
    /// </summary>
    /// <param name="nom">Le nom de la localité à mettre à jour</param>
    /// <param name="coordonneesGeographiques">Les nouvelles coordonnées géographiques</param>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    Task UpdateCoordonneesGeographiquesAsync(string nom, CoordonneesGeographiques coordonneesGeographiques, CancellationToken cancellationToken);
    /// <summary>
    /// Met à jour la superficie d'une localité identifiée par son nom
    /// </summary>
    /// <param name="nom">Le nom de la localité à mettre à jour</param>
    /// <param name="superficie">La nouvelle superficie</param>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    Task UpdateSuperficieAsync(string nom, decimal superficie, CancellationToken cancellationToken);
    /// <summary>
    /// Met à jour la population d'une localité identifiée par son nom
    /// </summary>
    /// <param name="nom">Le nom de la localité à mettre à jour</param>
    /// <param name="population">La nouvelle population</param>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    Task UpdatePopulationAsync(string nom, int population, CancellationToken cancellationToken);
    /// <summary>
    /// Supprime une localité de la base de données
    /// </summary>
    /// <param name="id">L'identifiant de la localité à supprimer</param>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    Task RemoveAsync(Guid id, CancellationToken cancellationToken);
}