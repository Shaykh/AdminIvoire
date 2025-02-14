using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Write;
using Mapster;

namespace AdminIvoire.Infrastructure.Persistence.Repository.Write;

public abstract class LocaliteWriteRepository<T>(LocaliteContext dbContext) : ILocaliteWriteRepository<T> where T : Localite
{
    private readonly LocaliteContext _dbContext = dbContext;

    public virtual async Task<T> AddAsync(T localite)
    {
        await _dbContext.Set<T>().AddAsync(localite);
        return localite;
    }

    public virtual async Task UpdateAsync(T localite)
    {
        var entity = await _dbContext.Set<T>().FindAsync(localite.Id)
            ?? throw new DataException($"Aucune entité de type {typeof(T).Name} avec id {localite.Id} n'a été trouvée.");
        entity.Adapt(localite);
    }

    public virtual async Task RemoveAsync(Guid id)
    {
        var entity = await _dbContext.Set<T>().FindAsync(id)
            ?? throw new DataException($"Aucune entité de type {typeof(T).Name} avec id {id} n'a été trouvée.");
        _dbContext.Set<T>().Remove(entity);
    }
}
