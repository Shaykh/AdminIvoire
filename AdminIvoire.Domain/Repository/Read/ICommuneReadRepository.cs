using AdminIvoire.Domain.Entite;

namespace AdminIvoire.Domain.Repository.Read;

public interface ICommuneReadRepository : ILocaliteReadRepository<Commune>
{
    Task<IList<Commune>> GetAllByDepartementIdAsync(Guid departementId, CancellationToken cancellationToken);
}
