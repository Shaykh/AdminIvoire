using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository;

namespace AdminIvoire.Infrastructure.Persistence.Repository;

public class RegionWriteRepository(LocaliteContext dbContext) : LocaliteWriteRepository<Region>(dbContext), IRegionWriteRepository
{
    public RegionWriteRepository(LocaliteContext dbContext) : base(dbContext) { }
}
