using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Persistence.Repository.Read;

public class CommuneReadRepository(LocaliteContext dbContext) : LocaliteReadRepository<Commune>(dbContext), ICommuneReadRepository
{
    public async Task<IList<Commune>> GetAllByDepartementIdAsync(Guid departementId, CancellationToken cancellationToken)
    {
        return await DbContext.Communes
            .AsNoTracking()
            .Where(x => x.DepartementId == departementId)
            .ToListAsync(cancellationToken);
    }

    public override async Task<Commune> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbContext.Communes
            .Include(c => c.Villages)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new DataException($"Aucune commune avec id {id} n'a été trouvée.");
    }
}
