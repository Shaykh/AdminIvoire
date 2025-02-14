using AdminIvoire.Domain.Repository;

namespace AdminIvoire.Infrastructure.Persistence.Repository;

public class UnitOfWork(LocaliteContext localiteContext) : IUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await localiteContext.SaveChangesAsync(cancellationToken);
    }
}
