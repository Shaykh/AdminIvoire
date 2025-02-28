using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Persistence.Repository.Read;

public class RegionReadRepository(LocaliteContext dbContext) : LocaliteReadRepository<Region>(dbContext), IRegionReadRepository
{
    public async Task<IList<Region>> GetAllByDistrictIdAsync(Guid districtId, CancellationToken cancellationToken)
    {
        return await DbContext.Regions
            .Include(r => r.District)
            .Include(r => r.Departements)
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.DistrictId == districtId)
            .OrderBy(x => x.Nom)
            .ToListAsync(cancellationToken);
    }

    public override async Task<Region> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbContext.Regions
            .Include(r => r.District)
            .Include(r => r.Departements)
            .AsNoTracking()
            .AsSplitQuery()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new DataAccessException($"Aucune région avec id {id} n'a été trouvée.");
    }

    public override async Task<IList<Region>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await DbContext.Regions
            .Include(r => r.District)
            .AsNoTracking()
            .AsSplitQuery()
            .OrderBy(x => x.Nom)
            .ToListAsync(cancellationToken);
    }
}
