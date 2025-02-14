using AdminIvoire.Domain.Entite;

namespace AdminIvoire.Domain.Repository.Read;

public interface IRegionReadRepository : ILocaliteReadRepository<Region>
{
    Task<IList<Region>> GetAllByDistrictIdAsync(Guid districtId, CancellationToken cancellationToken);
}
