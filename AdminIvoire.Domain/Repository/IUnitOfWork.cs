namespace AdminIvoire.Domain.Repository;

/// <summary>
/// Interface présentant une unité logique d'opération en base de données
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Persiste les changements en base de donnnées
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SaveAsync(CancellationToken cancellationToken);
}