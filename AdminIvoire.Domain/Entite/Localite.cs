using AdminIvoire.Domain.ValueObject;

namespace AdminIvoire.Domain.Entite;

public abstract class Localite
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Nom { get; set; }
    public string? Code { get; set; }
    public decimal Superficie { get; set; }
    public int Population { get; set; }
    public CoordonneesGeographiques? CoordonneesGeographiques { get; set; }
}
