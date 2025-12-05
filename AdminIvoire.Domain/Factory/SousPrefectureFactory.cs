using AdminIvoire.Domain.Entite;
using AdminIvoire.Domain.Repository.Read;
using AdminIvoire.Domain.Repository.Write;

namespace AdminIvoire.Domain.Factory;

/// <summary>
/// Interface pour créer ou récupérer des sous-préfectures
/// </summary>
public interface ISousPrefectureFactory
{
    /// <summary>
    /// Récupère une sous-préfecture existante par son nom ou en crée une nouvelle si elle n'existe pas
    /// </summary>
    /// <param name="nom">Le nom de la sous-préfecture</param>
    /// <param name="population">La population à ajouter à la sous-préfecture (si existante) ou la population initiale (si nouvelle)</param>
    /// <param name="departement">Le département auquel appartient la sous-préfecture</param>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    /// <returns>La sous-préfecture existante ou nouvellement créée</returns>
    Task<SousPrefecture> GetOrCreateAsync(string nom, int population, Departement departement, CancellationToken cancellationToken);
}

/// <summary>
/// Factory pour créer ou récupérer des sous-préfectures
/// </summary>
public class SousPrefectureFactory(ISousPrefectureWriteRepository sousPrefectureWriteRepository, ISousPrefectureReadRepository sousPrefectureReadRepository) : ISousPrefectureFactory
{
    /// <summary>
    /// Récupère une sous-préfecture existante par son nom ou en crée une nouvelle si elle n'existe pas
    /// </summary>
    /// <param name="nom">Le nom de la sous-préfecture</param>
    /// <param name="population">La population à ajouter à la sous-préfecture (si existante) ou la population initiale (si nouvelle)</param>
    /// <param name="departement">Le département auquel appartient la sous-préfecture</param>
    /// <param name="cancellationToken">Token d'annulation pour annuler l'opération asynchrone</param>
    /// <returns>La sous-préfecture existante ou nouvellement créée</returns>
    public async Task<SousPrefecture> GetOrCreateAsync(string nom, int population, Departement departement, CancellationToken cancellationToken)
    {
        var sousPrefecture = await sousPrefectureReadRepository.GetByNomAsync(nom, cancellationToken);
        if (sousPrefecture is null)
        {
            sousPrefecture = new SousPrefecture { Nom = nom, Departement = departement, Population = population };
            await sousPrefectureWriteRepository.AddAsync(sousPrefecture, cancellationToken);
        }
        else
        {
            sousPrefecture.Population += population;
        }
        return sousPrefecture;
    }
}