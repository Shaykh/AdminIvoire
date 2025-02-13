using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository;

namespace AdminIvoire.Infrastructure.Persistence.Repository;

public class VillageWriteRepository(LocaliteContext dbContext) : LocaliteWriteRepository<Village>(dbContext), IVillageWriteRepository
{
    public VillageWriteRepository(LocaliteContext dbContext) : base(dbContext) { }
}
