using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Write;

namespace AdminIvoire.Infrastructure.Persistence.Repository.Write;

public class RegionWriteRepository(LocaliteContext dbContext) : LocaliteWriteRepository<Region>(dbContext), IRegionWriteRepository
{
    public RegionWriteRepository(LocaliteContext dbContext) : base(dbContext) { }
}
