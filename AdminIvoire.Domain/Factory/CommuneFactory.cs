using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;

namespace AdminIvoire.Domain.Factory;

public interface ICommuneFactory
{
    Task<Commune> GetOrCreateAsync(string nom, int population, Departement departement, CancellationToken cancellationToken);
}

public class CommuneFactory(ICommuneWriteRepository communeWriteRepository, ICommuneReadRepository communeReadRepository) : ICommuneFactory
{
    public async Task<Commune> GetOrCreateAsync(string nom, int population, Departement departement, CancellationToken cancellationToken)
    {
        var commune = await communeReadRepository.GetByNomAsync(nom, cancellationToken);
        if (commune is null)
        {
            commune = new Commune { Nom = nom, Departement = departement, Population = population };
            await communeWriteRepository.AddAsync(commune, cancellationToken);
        }
        else
        {
            commune.Population += population;
        }
        return commune;
    }
}
