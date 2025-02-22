using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;

namespace AdminIvoire.Domain.Factory;

public interface IDepartementFactory
{
    Task<Departement> GetOrCreateAsync(string nom, int population, Region region, CancellationToken cancellationToken);
}

public class DepartementFactory(IDepartementWriteRepository departementWriteRepository, IDepartementReadRepository departementReadRepository) : IDepartementFactory
{
    public async Task<Departement> GetOrCreateAsync(string nom, int population, Region region, CancellationToken cancellationToken)
    {
        var departement = await departementReadRepository.GetByNomAsync(nom, cancellationToken);
        if (departement is null)
        {
            departement = new Departement { Nom = nom, Region = region, Population = population };
            await departementWriteRepository.AddAsync(departement, cancellationToken);
        }
        else
        {
            departement.Population += population;
        }
        return departement;
    }
}