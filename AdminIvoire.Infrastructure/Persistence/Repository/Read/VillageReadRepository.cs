using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Persistence.Repository;

public class VillageReadRepository(LocaliteContext dbContext) : LocaliteReadRepository<Village>(dbContext), IVillageReadRepository
{
    public async Task<IList<Village>> GetAllBySousPrefectureId(Guid sousPrefectureId)
    {
        return await DbContext.Villages
            .Where(x => x.SousPrefectureId == sousPrefectureId)
            .ToListAsync();
    }

    public override async Task<Village> GetById(Guid id)
    {
        return await DbContext.Villages
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new DataException($"Aucun village avec id {id} n'a été trouvé.");
    }
}
