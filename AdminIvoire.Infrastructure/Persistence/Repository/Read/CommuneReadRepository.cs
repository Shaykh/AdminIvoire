using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Persistence.Repository.Read;

public class CommuneReadRepository(LocaliteContext dbContext) : LocaliteReadRepository<Commune>(dbContext), ICommuneReadRepository
{
    public async Task<IList<Commune>> GetAllByDepartementId(Guid departementId)
    {
        return await DbContext.Communes
            .Include(c => c.Villages.Count)
            .Where(x => x.DepartementId == departementId)
            .ToListAsync();
    }

    public override async Task<Commune> GetById(Guid id)
    {
        return await DbContext.Communes
            .Include(c => c.Villages)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new DataException($"Aucune commune avec id {id} n'a été trouvée.");
    }
}
