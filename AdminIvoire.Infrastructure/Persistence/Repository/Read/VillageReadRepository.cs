using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Persistence.Repository.Read;

public class VillageReadRepository(LocaliteContext dbContext) : LocaliteReadRepository<Village>(dbContext), IVillageReadRepository
{
    public async Task<IList<Village>> GetAllBySousPrefectureIdAsync(Guid sousPrefectureId, CancellationToken cancellationToken)
    {
        return await DbContext.Villages
            .Where(x => x.SousPrefectureId == sousPrefectureId)
            .ToListAsync(cancellationToken);
    }

    public override async Task<Village> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbContext.Villages
            .FindAsync([id], cancellationToken)
            ?? throw new DataException($"Aucun village avec id {id} n'a été trouvé.");
    }
}
