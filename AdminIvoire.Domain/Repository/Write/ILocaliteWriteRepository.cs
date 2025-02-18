using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.ValueObject;

namespace AdminIvoire.Domain.Repository.Write;

public interface ILocaliteWriteRepository<T> where T : Localite
{
    Task<T> AddAsync(T localite, CancellationToken cancellationToken);
    Task UpdateAsync(T localite, CancellationToken cancellationToken);
    Task UpdateCoordonneesGeographiquesAsync(string nom, CoordonneesGeographiques coordonneesGeographiques, CancellationToken cancellationToken);
    Task UpdateSuperficieAsync(string nom, decimal superficie, CancellationToken cancellationToken);
    Task UpdatePopulationAsync(string nom, int population, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, CancellationToken cancellationToken);
}