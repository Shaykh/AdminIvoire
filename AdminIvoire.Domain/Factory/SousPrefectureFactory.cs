using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;

namespace AdminIvoire.Domain.Factory;

public interface ISousPrefectureFactory
{
    Task<SousPrefecture> GetOrCreateAsync(string nom, int population, Departement departement, CancellationToken cancellationToken);
}

public class SousPrefectureFactory(ISousPrefectureWriteRepository sousPrefectureWriteRepository, ISousPrefectureReadRepository sousPrefectureReadRepository) : ISousPrefectureFactory
{
    public async Task<SousPrefecture> GetOrCreateAsync(string nom, int population, Departement departement, CancellationToken cancellationToken)
    {
        var sousPrefecture = await sousPrefectureReadRepository.GetByNomAsync(nom, cancellationToken);
        if (sousPrefecture is null)
        {
            sousPrefecture = new SousPrefecture { Nom = nom, Departement = departement, Population = population };
            await sousPrefectureWriteRepository.AddAsync(sousPrefecture, cancellationToken);
        }
        else
        {
            sousPrefecture.Population += population;
        }
        return sousPrefecture;
    }
}