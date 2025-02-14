using AdminIvoire.Domain.Entite;

namespace AdminIvoire.Domain.Repository.Read;

public interface IDepartementReadRepository : ILocaliteReadRepository<Departement>
{
    Task<IList<Departement>> GetAllByRegionId(Guid regionId);
}
