using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;

namespace AdminIvoire.Domain.Factory;

/// <summary>
/// Interface pour créer ou récupérer des régions
/// </summary>
public interface IRegionFactory
{
    /// <summary>
    /// Récupère une région existante par son nom ou en crée une nouvelle si elle n'existe pas
    /// </summary>
    /// <param name="nom">Le nom de la région</param>
    /// <param name="population">La population à ajouter à la région (si existante) ou la population initiale (si nouvelle)</param>
    /// <param name="district">Le district auquel appartient la région</param>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    /// <returns>La région existante ou nouvellement créée</returns>
    Task<Region> GetOrCreateAsync(string nom, int population, District district, CancellationToken cancellationToken);
}

/// <summary>
/// Factory pour créer ou récupérer des régions
/// </summary>
public class RegionFactory(IRegionReadRepository regionReadRepository, IRegionWriteRepository regionWriteRepository) : IRegionFactory
{
    /// <summary>
    /// Récupère une région existante par son nom ou en crée une nouvelle si elle n'existe pas
    /// </summary>
    /// <param name="nom">Le nom de la région</param>
    /// <param name="population">La population à ajouter à la région (si existante) ou la population initiale (si nouvelle)</param>
    /// <param name="district">Le district auquel appartient la région</param>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    /// <returns>La région existante ou nouvellement créée</returns>
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
