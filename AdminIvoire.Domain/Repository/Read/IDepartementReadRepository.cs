using AdminIvoire.Domain.Entite;

namespace AdminIvoire.Domain.Repository.Read;

public interface IDepartementReadRepository : ILocaliteReadRepository<Departement>
{
    Task<IList<Departement>> GetAllByRegionIdAsync(Guid regionId, CancellationToken cancellationToken);
}
