using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository;

namespace AdminIvoire.Infrastructure.Persistence.Repository;

public class DepartementWriteRepository(LocaliteContext dbContext) : LocaliteWriteRepository<Departement>(dbContext), IDepartementWriteRepository
{
    public DepartementWriteRepository(LocaliteContext dbContext) : base(dbContext) { }
}
