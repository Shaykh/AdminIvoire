namespace AdminIvoire.Domain.Entite;

/// <summary>
/// Entité représentant une sous-préfecture, subdivision d'un département
/// </summary>
public class SousPrefecture : Localite
{
    /// <summary>
    /// L'identifiant du département auquel appartient cette sous-préfecture
    /// </summary>
    public Guid DepartementId { get; set; }
    /// <summary>
    /// Le département auquel appartient cette sous-préfecture
    /// </summary>
    public required Departement Departement { get; set; }
    /// <summary>
    /// La collection des villages appartenant à cette sous-préfecture
    /// </summary>
    public ICollection<Village> Villages { get; set; } = [];

    /// <summary>
    /// Ajoute un village à cette sous-préfecture si celui-ci n'existe pas déjà (comparaison insensible à la casse)
    /// </summary>
    /// <param name="village">Le village à ajouter</param>
    /// <returns>True si le village a été ajouté, False s'il existe déjà</returns>
    public bool AddVillage(Village village)
    {
        if (Villages.Any(v => v.Nom.Equals(village.Nom, StringComparison.InvariantCultureIgnoreCase)))
        {
            return false;
        }
        Villages.Add(village);
        return true;
    }
}
