using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Write;

namespace AdminIvoire.Infrastructure.Persistence.Repository.Write;

public class DistrictWriteRepository(LocaliteContext dbContext) : LocaliteWriteRepository<District>(dbContext), IDistrictWriteRepository
{
}
