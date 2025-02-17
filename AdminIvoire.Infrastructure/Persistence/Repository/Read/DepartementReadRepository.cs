using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Persistence.Repository.Read;

public class DepartementReadRepository(LocaliteContext dbContext) : LocaliteReadRepository<Departement>(dbContext), IDepartementReadRepository
{
    public async Task<IList<Departement>> GetAllByRegionIdAsync(Guid regionId, CancellationToken cancellationToken)
    {
        return await DbContext.Departements
            .AsNoTracking()
            .Where(x => x.RegionId == regionId)
            .ToListAsync(cancellationToken);
    }

    public override async Task<Departement> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbContext.Departements
            .Include(d => d.Communes)
            .Include(d => d.SousPrefectures)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new DataException($"Aucun département avec id {id} n'a été trouvé.");
    }

    public override async Task<IList<Departement>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await DbContext.Departements
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
