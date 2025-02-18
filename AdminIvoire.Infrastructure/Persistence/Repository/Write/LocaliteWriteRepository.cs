using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.ValueObject;
using Microsoft.EntityFrameworkCore;

namespace AdminIvoire.Infrastructure.Persistence.Repository.Write;

public abstract class LocaliteWriteRepository<T>(LocaliteContext dbContext) where T : Localite
{
    private readonly LocaliteContext _dbContext = dbContext;

    public virtual async Task<T> AddAsync(T localite, CancellationToken cancellationToken)
    {
        await _dbContext.Set<T>().AddAsync(localite, cancellationToken);
        return localite;
    }

    public virtual async Task UpdateAsync(T localite, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Set<T>().FindAsync([localite.Id], cancellationToken)
            ?? throw new DataException($"Aucune entité de type {typeof(T).Name} avec id {localite.Id} n'a été trouvée.");
        entity.Nom = localite.Nom;
        entity.Code = localite.Code;
        entity.Superficie = localite.Superficie;
        entity.Population = localite.Population;
        entity.CoordonneesGeographiques = localite.CoordonneesGeographiques;
    }

    public virtual async Task UpdateCoordonneesGeographiquesAsync(string nom, CoordonneesGeographiques coordonneesGeographiques, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Set<T>().FirstOrDefaultAsync(x => x.Nom == nom, cancellationToken)
            ?? throw new DataException($"Aucune entité de type {typeof(T).Name} avec nom {nom} n'a été trouvée.");
        entity.CoordonneesGeographiques = coordonneesGeographiques;
    }

    public virtual async Task UpdateSuperficieAsync(string nom, decimal superficie, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Set<T>().FirstOrDefaultAsync(x => x.Nom == nom, cancellationToken)
            ?? throw new DataException($"Aucune entité de type {typeof(T).Name} avec nom {nom} n'a été trouvée.");
        entity.Superficie = superficie;
    }

    public virtual async Task UpdatePopulationAsync(string nom, int population, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Set<T>().FirstOrDefaultAsync(x => x.Nom == nom, cancellationToken)
            ?? throw new DataException($"Aucune entité de type {typeof(T).Name} avec nom {nom} n'a été trouvée.");
        entity.Population = population;
    }

    public virtual async Task RemoveAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Set<T>().FindAsync([id], cancellationToken)
            ?? throw new DataException($"Aucune entité de type {typeof(T).Name} avec id {id} n'a été trouvée.");
        _dbContext.Set<T>().Remove(entity);
    }
}
