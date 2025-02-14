using AdminIvoire.Domain.Entite;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Persistence.Repository.Read;

public abstract class LocaliteReadRepository<T>(LocaliteContext dbContext) where T : Localite
{
    protected readonly LocaliteContext DbContext = dbContext;

    public virtual async Task<IList<T>> GetAll()
    {
        return await DbContext.Set<T>()
            .ToListAsync();
    }

    public virtual async Task<T> GetById(Guid id)
    {
        return await DbContext.Set<T>()
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new DataException($"Aucune entité de type {typeof(T).Name} avec id {id} n'a été trouvée.");
    }
}
