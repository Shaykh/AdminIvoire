namespace AdminIvoire.Domain.Entite;

public class Region : Localite
{
    public Guid DistrictId { get; set; }
    public required District District { get; set; }
    public ICollection<Departement> Departements { get; set; } = new List<Departement>();
}
