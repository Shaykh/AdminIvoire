using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Write;

namespace AdminIvoire.Infrastructure.Persistence.Repository.Write;

public class SousPrefectureWriteRepository(LocaliteContext dbContext) : LocaliteWriteRepository<SousPrefecture>(dbContext), ISousPrefectureWriteRepository
{
    public SousPrefectureWriteRepository(LocaliteContext dbContext) : base(dbContext) { }
}
