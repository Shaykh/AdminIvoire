namespace AdminIvoire.Domain.Entite;

/// <summary>
/// Entité représentant un district, niveau administratif le plus élevé en Côte d'Ivoire
/// </summary>
public class District : Localite
{
    /// <summary>
    /// La collection des régions appartenant à ce district
    /// </summary>
    public ICollection<Region> Regions { get; set; } = [];
}
