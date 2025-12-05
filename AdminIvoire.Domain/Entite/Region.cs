namespace AdminIvoire.Domain.Entite;

/// <summary>
/// Entité représentant une région, subdivision d'un district
/// </summary>
public class Region : Localite
{
    /// <summary>
    /// L'identifiant du district auquel appartient cette région
    /// </summary>
    public Guid DistrictId { get; set; }
    /// <summary>
    /// Le district auquel appartient cette région
    /// </summary>
    public required District District { get; set; }
    /// <summary>
    /// La collection des départements appartenant à cette région
    /// </summary>
    public ICollection<Departement> Departements { get; set; } = new List<Departement>();
}
