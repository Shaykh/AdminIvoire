namespace AdminIvoire.Domain.Entite;

/// <summary>
/// Entité représentant un village, subdivision d'une sous-préfecture
/// </summary>
public class Village : Localite
{
    /// <summary>
    /// L'identifiant de la sous-préfecture à laquelle appartient ce village
    /// </summary>
    public Guid SousPrefectureId { get; set; }
    /// <summary>
    /// La sous-préfecture à laquelle appartient ce village
    /// </summary>
    public SousPrefecture? SousPrefecture { get; set; }
}