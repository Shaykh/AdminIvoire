using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;

namespace AdminIvoire.Domain.Factory;

public interface IRegionFactory
{
    Task<Region> GetOrCreateAsync(string nom, int population, District district, CancellationToken cancellationToken);
}

public class RegionFactory(IRegionReadRepository regionReadRepository, IRegionWriteRepository regionWriteRepository) : IRegionFactory
{
    public async Task<Region> GetOrCreateAsync(string nom, int population, District district, CancellationToken cancellationToken)
    {
        var region = await regionReadRepository.GetByNomAsync(nom, cancellationToken);
        if (region == null)
        {
            region = new Region { Nom = nom, Population = population, District = district };
            await regionWriteRepository.AddAsync(region, cancellationToken);
        }
        else
        {
            region.Population += population;
        }
        return region;
    }
}
