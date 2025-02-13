using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Persistence.Repository;

public class LocaliteReadRepository(LocaliteContext dbContext) : ILocaliteReadRepository
{
    private readonly LocaliteContext _dbContext = dbContext;

    public async Task<Commune> GetCommuneById(Guid id)
    {
        return await _dbContext.Communes
            .Include(c => c.Villages)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new DataException($"Aucune commune avec id {id} n'a été trouvée.");
    }

    public async Task<IList<Commune>> GetCommunes()
    {
        return await _dbContext.Communes.ToListAsync();
    }

    public async Task<IList<Commune>> GetCommunesByDepartementId(Guid departementId)
    {
        return await _dbContext.Communes
            .Include(c => c.Villages.Count)
            .Where(x => x.DepartementId == departementId)
            .ToListAsync();
    }

    public async Task<Departement> GetDepartementById(Guid id)
    {
        return await _dbContext.Departements
            .Include(d => d.Communes)
            .Include(d => d.SousPrefectures)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new DataException($"Aucun département avec id {id} n'a été trouvé.");
    }

    public async Task<IList<Departement>> GetDepartements()
    {
        return await _dbContext.Departements.ToListAsync();
    }

    public async Task<IList<Departement>> GetDepartementsByRegionId(Guid regionId)
    {
        return await _dbContext.Departements
            .Include(d => d.SousPrefectures.Count)
            .Include(d => d.Communes.Count)
            .Where(x => x.RegionId == regionId)
            .ToListAsync();
    }

    public async Task<District> GetDistrictById(Guid id)
    {
        return await _dbContext.Districts
            .Include(d => d.Regions)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new DataException($"Aucun district avec id {id} n'a été trouvé.");
    }

    public async Task<IList<District>> GetDistricts()
    {
        return await _dbContext.Districts
            .Include(d => d.Regions.Count)
            .ToListAsync();
    }

    public async Task<Region> GetRegionById(Guid id)
    {
        return await _dbContext.Regions
            .Include(r => r.Departements)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new DataException($"Aucune région avec id {id} n'a été trouvée.");
    }

    public async Task<IList<Region>> GetRegions()
    {
        return await _dbContext.Regions
            .Include(r => r.Departements.Count)
            .ToListAsync();
    }

    public async Task<IList<Region>> GetRegionsByDistrictId(Guid districtId)
    {
        return await _dbContext.Regions
            .Include(r => r.Departements.Count)
            .Where(x => x.DistrictId == districtId)
            .ToListAsync();
    }

    public async Task<SousPrefecture> GetSousPrefectureById(Guid id)
    {
        return await _dbContext.SousPrefectures
            .Include(s => s.Villages)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new DataException($"Aucune sous-préfecture avec id {id} n'a été trouvée.");
    }

    public async Task<IList<SousPrefecture>> GetSousPrefectures()
    {
        return await _dbContext.SousPrefectures
            .ToListAsync();
    }

    public async Task<IList<SousPrefecture>> GetSousPrefecturesByDepartementId(Guid departementId)
    {
        return await _dbContext.SousPrefectures
            .Include(s => s.Villages.Count)
            .Where(x => x.DepartementId == departementId)
            .ToListAsync();
    }

    public async Task<Village> GetVillageById(Guid id)
    {
        return await _dbContext.Villages
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new DataException($"Aucun village avec id {id} n'a été trouvé.");
    }

    public async Task<IList<Village>> GetVillages()
    {
        return await _dbContext.Villages
            .ToListAsync();
    }

    public async Task<IList<Village>> GetVillagesBySousPrefectureId(Guid sousPrefectureId)
    {
        return await _dbContext.Villages
            .Where(x => x.SousPrefectureId == sousPrefectureId)
            .ToListAsync();
    }
}
