using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Persistence.Repository.Read;

public class RegionReadRepository(LocaliteContext dbContext) : LocaliteReadRepository<Region>(dbContext), IRegionReadRepository
{
    public async Task<IList<Region>> GetAllByDistrictId(Guid districtId)
    {
        return await DbContext.Regions
            .Include(r => r.Departements.Count)
            .Where(x => x.DistrictId == districtId)
            .ToListAsync();
    }

    public override async Task<Region> GetById(Guid id)
    {
        return await DbContext.Regions
            .Include(r => r.Departements)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new DataException($"Aucune région avec id {id} n'a été trouvée.");
    }

    public override async Task<IList<Region>> GetAll()
    {
        return await DbContext.Regions
            .Include(r => r.Departements.Count)
            .ToListAsync();
    }
}
