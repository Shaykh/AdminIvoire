using AdminIvoire.Domain.Entite;

namespace AdminIvoire.Domain.Repository.Write;

public interface ILocaliteWriteRepository<T> where T : Localite
{
    Task<T> AddAsync(T localite, CancellationToken cancellationToken);
    Task UpdateAsync(T localite, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, CancellationToken cancellationToken);
}