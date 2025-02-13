using AdminIvoire.Domain.Entite;

namespace AdminIvoire.Domain.Repository;

public interface ILocaliteReadRepository
{
    Task<IList<District>> GetDistricts();
    Task<District> GetDistrictById(Guid id);
    Task<IList<Region>> GetRegions();
    Task<IList<Region>> GetRegionsByDistrictId(Guid districtId);
    Task<Region> GetRegionById(Guid id);
    Task<IList<Departement>> GetDepartements();
    Task<IList<Departement>> GetDepartementsByRegionId(Guid regionId);
    Task<Departement> GetDepartementById(Guid id);
    Task<IList<Commune>> GetCommunes();
    Task<IList<Commune>> GetCommunesByDepartementId(Guid departementId);
    Task<Commune> GetCommuneById(Guid id);
    Task<IList<SousPrefecture>> GetSousPrefectures();
    Task<IList<SousPrefecture>> GetSousPrefecturesByDepartementId(Guid departementId);
    Task<SousPrefecture> GetSousPrefectureById(Guid id);
    Task<IList<Village>> GetVillages();
    Task<IList<Village>> GetVillagesBySousPrefectureId(Guid sousPrefectureId);
    Task<Village> GetVillageById(Guid id);
}
