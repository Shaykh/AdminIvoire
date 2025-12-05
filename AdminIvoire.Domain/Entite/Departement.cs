namespace AdminIvoire.Domain.Entite;

/// <summary>
/// Entité représentant un département, subdivision d'une région
/// </summary>
public class Departement : Localite
{
    /// <summary>
    /// L'identifiant de la région à laquelle appartient ce département
    /// </summary>
    public Guid RegionId { get; set; }
    /// <summary>
    /// La région à laquelle appartient ce département
    /// </summary>
    public required Region Region { get; set; }
    /// <summary>
    /// La collection des sous-préfectures appartenant à ce département
    /// </summary>
    public ICollection<SousPrefecture> SousPrefectures { get; set; } = [];
}
