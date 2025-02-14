using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Persistence.Repository.Read;

public class DepartementReadRepository(LocaliteContext dbContext) : LocaliteReadRepository<Departement>(dbContext), IDepartementReadRepository
{
    public async Task<IList<Departement>> GetAllByRegionId(Guid regionId)
    {
        return await DbContext.Departements
            .Include(d => d.Communes.Count)
            .Include(d => d.SousPrefectures.Count)
            .Where(x => x.RegionId == regionId)
            .ToListAsync();
    }

    public override async Task<Departement> GetById(Guid id)
    {
        return await DbContext.Departements
            .Include(d => d.Communes)
            .Include(d => d.SousPrefectures)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new DataException($"Aucun département avec id {id} n'a été trouvé.");
    }

    public override async Task<IList<Departement>> GetAll()
    {
        return await DbContext.Departements
            .Include(d => d.Communes.Count)
            .Include(d => d.SousPrefectures.Count)
            .ToListAsync();
    }
}
