namespace AdminIvoire.Domain.Entite;

public class SousPrefecture : Localite
{
    public Guid DepartementId { get; set; }
    public required Departement Departement { get; set; }
    public ICollection<Village> Villages { get; set; } = [];

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
