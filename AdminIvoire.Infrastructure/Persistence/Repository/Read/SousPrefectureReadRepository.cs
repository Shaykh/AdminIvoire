using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Persistence.Repository.Read;

public class SousPrefectureReadRepository(LocaliteContext dbContext) : LocaliteReadRepository<SousPrefecture>(dbContext), ISousPrefectureReadRepository
{
    public async Task<IList<SousPrefecture>> GetAllByDepartementIdAsync(Guid departementId, CancellationToken cancellationToken)
    {
        return await DbContext.SousPrefectures
            .AsNoTracking()
            .Where(x => x.DepartementId == departementId)
            .ToListAsync(cancellationToken);
    }

    public override async Task<SousPrefecture> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbContext.SousPrefectures
            .Include(sp => sp.Villages)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new DataException($"Aucune sous-préfecture avec id {id} n'a été trouvée.");
    }
}
