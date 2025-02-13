namespace AdminIvoire.Domain.Entite;

public class Departement : Localite
{
    public Guid RegionId { get; set; }
    public required Region Region { get; set; }
    public ICollection<Commune> Communes { get; set; } = [];
    public ICollection<SousPrefecture> SousPrefectures { get; set; } = [];
}
