using AdminIvoire.Domain.Entite;

namespace AdminIvoire.Domain.Repository.Read;

public interface IVillageReadRepository : ILocaliteReadRepository<Village>
{
    Task<IList<Village>> GetAllBySousPrefectureId(Guid sousPrefectureId);
}
