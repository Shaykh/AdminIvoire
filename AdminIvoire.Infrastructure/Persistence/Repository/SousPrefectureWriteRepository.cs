using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository;

namespace AdminIvoire.Infrastructure.Persistence.Repository;

public class SousPrefectureWriteRepository(LocaliteContext dbContext) : LocaliteWriteRepository<SousPrefecture>(dbContext), ISousPrefectureWriteRepository
{
    public SousPrefectureWriteRepository(LocaliteContext dbContext) : base(dbContext) { }
}
