using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository;

namespace AdminIvoire.Infrastructure.Persistence.Repository;

public class CommuneWriteRepository(LocaliteContext dbContext) : LocaliteWriteRepository<Commune>(dbContext), ICommuneWriteRepository
{
}
