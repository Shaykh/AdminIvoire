using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Write;

namespace AdminIvoire.Infrastructure.Persistence.Repository.Write;

public class DepartementWriteRepository(LocaliteContext dbContext) : LocaliteWriteRepository<Departement>(dbContext), IDepartementWriteRepository
{
    public DepartementWriteRepository(LocaliteContext dbContext) : base(dbContext) { }
}
