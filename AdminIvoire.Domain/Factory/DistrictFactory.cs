using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;

namespace AdminIvoire.Domain.Factory;

public interface IDistrictFactory
{
    Task<District> GetOrCreateAsync(string nom, int population, CancellationToken cancellationToken);
}

public class DistrictFactory(IDistrictWriteRepository districtWriteRepository, IDistrictReadRepository districtReadRepository) : IDistrictFactory
{
    public async Task<District> GetOrCreateAsync(string nom, int population, CancellationToken cancellationToken)
    {
        var district = await districtReadRepository.GetByNomAsync(nom, cancellationToken);
        if (district == null)
        {
            district = new District { Nom = nom, Population = population };
            await districtWriteRepository.AddAsync(district, cancellationToken);
        }
        else
        {
            district.Population += population;
        }
        return district;
    }
}