using AdminIvoire.Domain.Entite;

namespace AdminIvoire.Domain.Repository.Read;

public interface ILocaliteReadRepository<T> where T : Localite
{
    Task<IList<T>> GetAllAsync(CancellationToken cancellationToken);
    Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
