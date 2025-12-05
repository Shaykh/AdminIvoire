using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;

namespace AdminIvoire.Domain.Factory;

/// <summary>
/// Interface pour créer ou récupérer des districts
/// </summary>
public interface IDistrictFactory
{
    /// <summary>
    /// Récupère un district existant par son nom ou en crée un nouveau s'il n'existe pas
    /// </summary>
    /// <param name="nom">Le nom du district</param>
    /// <param name="population">La population à ajouter au district (si existant) ou la population initiale (si nouveau)</param>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    /// <returns>Le district existant ou nouvellement créé</returns>
    Task<District> GetOrCreateAsync(string nom, int population, CancellationToken cancellationToken);
}

/// <summary>
/// Factory pour créer ou récupérer des districts
/// </summary>
public class DistrictFactory(IDistrictWriteRepository districtWriteRepository, IDistrictReadRepository districtReadRepository) : IDistrictFactory
{
    /// <summary>
    /// Récupère un district existant par son nom ou en crée un nouveau s'il n'existe pas
    /// </summary>
    /// <param name="nom">Le nom du district</param>
    /// <param name="population">La population à ajouter au district (si existant) ou la population initiale (si nouveau)</param>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    /// <returns>Le district existant ou nouvellement créé</returns>
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