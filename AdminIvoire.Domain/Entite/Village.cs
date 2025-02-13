namespace AdminIvoire.Domain.Entite;

public class Village : Localite
{
    public Guid SousPrefectureId { get; set; }
    public required SousPrefecture SousPrefecture { get; set; }
}