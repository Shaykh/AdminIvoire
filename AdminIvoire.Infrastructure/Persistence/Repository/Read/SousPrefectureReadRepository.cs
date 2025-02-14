using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Persistence.Repository.Read;

public class SousPrefectureReadRepository(LocaliteContext dbContext) : LocaliteReadRepository<SousPrefecture>(dbContext), ISousPrefectureReadRepository
{
    public async Task<IList<SousPrefecture>> GetAllByDepartementId(Guid departementId)
    {
        return await DbContext.SousPrefectures
            .Include(sp => sp.Villages.Count)
            .Where(x => x.DepartementId == departementId)
            .ToListAsync();
    }

    public override async Task<SousPrefecture> GetById(Guid id)
    {
        return await DbContext.SousPrefectures
            .Include(sp => sp.Villages)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new DataException($"Aucune sous-préfecture avec id {id} n'a été trouvée.");
    }
}
