using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Write;

namespace AdminIvoire.Infrastructure.Persistence.Repository.Write;

public abstract class LocaliteWriteRepository<T>(LocaliteContext dbContext) : ILocaliteWriteRepository<T> where T : Localite
{
    private readonly LocaliteContext _dbContext = dbContext;
    public virtual async Task<T> Create(T localite)
    {
        await _dbContext.Set<T>().AddAsync(localite);
        await _dbContext.SaveChangesAsync();
        return localite;
    }

    public virtual async Task<T> Update(T localite)
    {
        _dbContext.Set<T>().Update(localite);
        await _dbContext.SaveChangesAsync();
        return localite;
    }

    public virtual async Task Delete(Guid id)
    {
        var entity = await _dbContext.Set<T>().FindAsync(id)
            ?? throw new DataException($"Aucune entité de type {typeof(T).Name} avec id {id} n'a été trouvée.");
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}
