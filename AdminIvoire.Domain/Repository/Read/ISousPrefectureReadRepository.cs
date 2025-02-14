using AdminIvoire.Domain.Entite;

namespace AdminIvoire.Domain.Repository.Read;

public interface ISousPrefectureReadRepository : ILocaliteReadRepository<SousPrefecture>
{
    Task<IList<SousPrefecture>> GetAllByDepartementId(Guid departementId);
}
