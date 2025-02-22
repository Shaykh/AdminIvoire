using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;

namespace AdminIvoire.Domain.Factory;

public interface IVillageFactory
{
    Task<Village> GetOrCreateAsync(string nom, int population, SousPrefecture commune, CancellationToken cancellationToken);
}

public class VillageFactory(IVillageReadRepository villageReadRepository, IVillageWriteRepository villageWriteRepository) : IVillageFactory
{
    public async Task<Village> GetOrCreateAsync(string nom, int population, SousPrefecture commune, CancellationToken cancellationToken)
    {
        var village = await villageReadRepository.GetByNomAsync(nom, cancellationToken);
        if (village is null)
        {
            village = new Village { Nom = nom, Population = population, SousPrefecture = commune };
            await villageWriteRepository.AddAsync(village, cancellationToken);
        }
        else
        {
            village.Population += population;
        }
        return village;
    }
}
