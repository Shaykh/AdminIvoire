using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository;

namespace AdminIvoire.Infrastructure.Persistence.Repository;

public class DistrictWriteRepository(LocaliteContext dbContext) : LocaliteWriteRepository<District>(dbContext), IDistrictWriteRepository
{
    public DistrictWriteRepository(LocaliteContext dbContext) : base(dbContext) { }
}
