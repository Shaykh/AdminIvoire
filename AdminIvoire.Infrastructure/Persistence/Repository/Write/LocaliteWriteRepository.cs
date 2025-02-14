using AdminIvoire.Domain.Entite;
using Mapster;

namespace AdminIvoire.Infrastructure.Persistence.Repository.Write;

public abstract class LocaliteWriteRepository<T>(LocaliteContext dbContext) where T : Localite
{
    private readonly LocaliteContext _dbContext = dbContext;

    public virtual async Task<T> AddAsync(T localite, CancellationToken cancellationToken)
    {
        await _dbContext.Set<T>().AddAsync(localite, cancellationToken);
        return localite;
    }

    public virtual async Task UpdateAsync(T localite, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Set<T>().FindAsync([localite.Id], cancellationToken)
            ?? throw new DataException($"Aucune entité de type {typeof(T).Name} avec id {localite.Id} n'a été trouvée.");
        entity.Adapt(localite);
    }

    public virtual async Task RemoveAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Set<T>().FindAsync([id], cancellationToken)
            ?? throw new DataException($"Aucune entité de type {typeof(T).Name} avec id {id} n'a été trouvée.");
        _dbContext.Set<T>().Remove(entity);
    }
}
