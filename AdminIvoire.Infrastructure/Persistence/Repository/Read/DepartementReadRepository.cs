using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Persistence.Repository.Read;

public class DepartementReadRepository(LocaliteContext dbContext) : LocaliteReadRepository<Departement>(dbContext), IDepartementReadRepository
{
    public async Task<IList<Departement>> GetAllByRegionIdAsync(Guid regionId, CancellationToken cancellationToken)
    {
        return await DbContext.Departements
            .Include(d => d.Region)
                .ThenInclude(r => r.District)
            .Where(x => x.RegionId == regionId)
            .AsNoTracking()
            .AsSplitQuery()
            .OrderBy(x => x.Nom)
            .ToListAsync(cancellationToken);
    }

    public override async Task<Departement> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbContext.Departements
            .Include(d => d.SousPrefectures)
            .Include(d => d.Region)
                .ThenInclude(r => r.District)
            .AsNoTracking()
            .AsSplitQuery()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new DataAccessException($"Aucun département avec id {id} n'a été trouvé.");
    }

    public override async Task<IList<Departement>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await DbContext.Departements
            .AsNoTracking()
            .OrderBy(x => x.Nom)
            .ToListAsync(cancellationToken);
    }
}
