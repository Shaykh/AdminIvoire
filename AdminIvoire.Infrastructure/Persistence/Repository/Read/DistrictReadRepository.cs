using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Persistence.Repository.Read;

public class DistrictReadRepository(LocaliteContext dbContext) : LocaliteReadRepository<District>(dbContext), IDistrictReadRepository
{
    public override async Task<District> GetById(Guid id)
    {
        return await DbContext.Districts
            .Include(d => d.Regions)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new DataException($"Aucun district avec id {id} n'a été trouvé.");
    }

    public override async Task<IList<District>> GetAll()
    {
        return await DbContext.Districts
            .Include(d => d.Regions)
            .ToListAsync();
    }
}
