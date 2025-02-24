namespace AdminIvoire.Domain.Entite;

public class Village : Localite
{
    public Guid SousPrefectureId { get; set; }
    public SousPrefecture? SousPrefecture { get; set; }
}