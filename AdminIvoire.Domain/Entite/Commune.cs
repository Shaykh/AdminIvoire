namespace AdminIvoire.Domain.Entite;

public class Commune : Localite
{
    public Guid DepartementId { get; set; }
    public required Departement Departement { get; set; }
    public ICollection<Village> Villages { get; set; } = [];
}
