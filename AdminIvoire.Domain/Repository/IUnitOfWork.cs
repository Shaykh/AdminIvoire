namespace AdminIvoire.Domain.Repository;

/// <summary>
/// Interface présentant une unité logique d'opération en base de données
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Persiste les changements en base de données
    /// </summary>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    /// <returns>Une tâche représentant l'opération asynchrone</returns>
    Task CommitAsync(CancellationToken cancellationToken);
}