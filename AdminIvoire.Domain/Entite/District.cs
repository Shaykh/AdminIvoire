namespace AdminIvoire.Domain.Entite;

public class District : Localite
{
    public ICollection<Region> Regions { get; set; } = new List<Region>();
}
