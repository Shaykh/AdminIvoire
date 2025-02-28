using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Persistence.Repository.Read;

public class DistrictReadRepository(LocaliteContext dbContext) : LocaliteReadRepository<District>(dbContext), IDistrictReadRepository
{
    public override async Task<District> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbContext.Districts
            .Include(d => d.Regions)
            .AsNoTracking()
            .AsSplitQuery()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new DataAccessException($"Aucun district avec id {id} n'a été trouvé.");
    }

    public override async Task<IList<District>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await DbContext.Districts
            .Include(d => d.Regions)
            .AsNoTracking()
            .AsSplitQuery()
            .OrderBy(x => x.Nom)
            .ToListAsync(cancellationToken);
    }
}
