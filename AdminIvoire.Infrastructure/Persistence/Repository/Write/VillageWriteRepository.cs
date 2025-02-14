using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Write;

namespace AdminIvoire.Infrastructure.Persistence.Repository.Write;

public class VillageWriteRepository(LocaliteContext dbContext) : LocaliteWriteRepository<Village>(dbContext), IVillageWriteRepository
{
}
