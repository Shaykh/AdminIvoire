using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;

namespace AdminIvoire.Domain.Factory;

/// <summary>
/// Interface pour créer ou récupérer des départements
/// </summary>
public interface IDepartementFactory
{
    /// <summary>
    /// Récupère un département existant par son nom ou en crée un nouveau s'il n'existe pas
    /// </summary>
    /// <param name="nom">Le nom du département</param>
    /// <param name="population">La population à ajouter au département (si existant) ou la population initiale (si nouveau)</param>
    /// <param name="region">La région à laquelle appartient le département</param>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    /// <returns>Le département existant ou nouvellement créé</returns>
    Task<Departement> GetOrCreateAsync(string nom, int population, Region region, CancellationToken cancellationToken);
}

/// <summary>
/// Factory pour créer ou récupérer des départements
/// </summary>
public class DepartementFactory(IDepartementWriteRepository departementWriteRepository, IDepartementReadRepository departementReadRepository) : IDepartementFactory
{
    /// <summary>
    /// Récupère un département existant par son nom ou en crée un nouveau s'il n'existe pas
    /// </summary>
    /// <param name="nom">Le nom du département</param>
    /// <param name="population">La population à ajouter au département (si existant) ou la population initiale (si nouveau)</param>
    /// <param name="region">La région à laquelle appartient le département</param>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    /// <returns>Le département existant ou nouvellement créé</returns>
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