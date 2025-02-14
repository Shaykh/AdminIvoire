using AdminIvoire.Domain.Entite;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Persistence.Repository.Read;

public abstract class LocaliteReadRepository<T>(LocaliteContext dbContext) where T : Localite
{
    protected readonly LocaliteContext DbContext = dbContext;

    public virtual async Task<IList<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await DbContext.Set<T>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbContext.Set<T>()
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new DataException($"Aucune entité de type {typeof(T).Name} avec id {id} n'a été trouvée.");
    }
}
