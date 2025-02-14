using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Write;

namespace AdminIvoire.Infrastructure.Persistence.Repository.Write;

public class CommuneWriteRepository(LocaliteContext dbContext) : LocaliteWriteRepository<Commune>(dbContext), ICommuneWriteRepository
{
}
