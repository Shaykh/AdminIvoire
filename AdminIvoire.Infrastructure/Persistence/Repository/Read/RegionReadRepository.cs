using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Persistence.Repository.Read;

public class RegionReadRepository(LocaliteContext dbContext) : LocaliteReadRepository<Region>(dbContext), IRegionReadRepository
{
    public async Task<IList<Region>> GetAllByDistrictIdAsync(Guid districtId, CancellationToken cancellationToken)
    {
        return await DbContext.Regions
            .AsNoTracking()
            .Where(x => x.DistrictId == districtId)
            .ToListAsync(cancellationToken);
    }

    public override async Task<Region> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbContext.Regions
            .Include(r => r.Departements)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new DataException($"Aucune r�gion avec id {id} n'a �t� trouv�e.");
    }

    public override async Task<IList<Region>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await DbContext.Regions
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
